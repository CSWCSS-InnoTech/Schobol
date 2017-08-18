using System;
using System.Threading.Tasks;
#if __IOS__
using CoreMotion;
using Foundation;
using UIKit;
#elif __ANDROID__
using Android.Hardware;
using static Java.Lang.JavaSystem;
using JavaObject = Java.Lang.Object;
using Android.Content;
using static Android.Util.Log;
#elif NETFX_CORE
using Windows.Devices.Sensors;
#endif

namespace InnoTecheLearning
{
    public static partial class Utils
    {
        public delegate void StepCountChangedEventHandler(uint Steps, TimeSpan TimePassed, float Distance);
        /// <summary>
        /// A Component that acts like a Pedometer. It senses motion via the 
        /// Accerleromter and attempts to determine if a step has been 
        /// taken. Using a configurable stride length, it can estimate the 
        /// distance traveled as well.
        /// </summary>
        public interface IStepCounter
        {
            event StepCountChangedEventHandler StepsChanged;
            ValueTask<uint> Steps { get; }
            ValueTask<Unit> Start();
            DateTime StartTime { get; }
            TimeSpan TimePassed { get; }
            void Stop();
            void Reset();
            //  Properties
            /// <summary>
            /// Set the nonnegative average stride length in meters. Default: 0.73
            /// </summary>
            float StrideLength
            { get; set; }


            /// <summary>
            /// The approximate distance traveled in meters.
            /// </summary>
            float Distance
            { get; }
        }
        /// <summary>
        /// A Component that acts like a Pedometer. It senses motion via the 
        /// Accerleromter and attempts to determine if a step has been 
        /// taken. Using a configurable stride length, it can estimate the 
        /// distance traveled as well.
        /// </summary>
        public class StepCounter :
#if __IOS__
        IStepCounter {
            public ValueTask<uint> Steps
            {
                get
                {
                    //If the last reset date wasn't today then we should update 
                    if (_resetTime.Date.Day != DateTime.Now.Date.Day)
                    {
                        _resetTime = DateTime.Today; //Forces update as the day may have changed.
                    }

                    NSDate sMidnight = DateHelpers.DateTimeToNSDate(_resetTime);

                    _queue = _queue ?? NSOperationQueue.CurrentQueue;
                    if (_stepCounter == null)
                        _stepCounter = new CMStepCounter();

                    return new ValueTask<uint>(async () => 
                        Steps_ = (uint)await _stepCounter.QueryStepCountAsync(sMidnight, NSDate.Now, _queue));
                }
            }
            public uint Steps_ { get; private set; }
            public DateTime StartTime { get; private set; }
           // public delegate void DailyStepCountChangedEventHandler(nint stepCount);

            private NSOperationQueue _queue;
            private DateTime _resetTime;
            private CMStepCounter _stepCounter;

            public StepCounter() { }
            public ValueTask<Unit> Start()
            {
                return Unit.InvokeAsync(() =>
                {
                    StartTime = DateTime.Now;
                    ForceUpdate();

                    _stepCounter.StartStepCountingUpdates(_queue, 1, Updater);
                });
            }

            public void Stop()
            { _stepCounter.StopStepCountingUpdates(); }

            public void Reset()
            { Steps_ = 0; StartTime = DateTime.MinValue; }

            public void ForceUpdate()
            {
                //If the last reset date wasn't today then we should update 
                if (_resetTime.Date.Day != DateTime.Now.Date.Day)
                {
                    _resetTime = DateTime.Today; //Forces update as the day may have changed.
                }

                NSDate sMidnight = DateHelpers.DateTimeToNSDate(_resetTime);

                _queue = _queue ?? NSOperationQueue.CurrentQueue;
                if (_stepCounter == null)
                    _stepCounter = new CMStepCounter();

                _stepCounter.QueryStepCount(sMidnight, NSDate.Now, _queue, DailyStepQueryHandler);
            }

            public void StartCountingFrom(DateTime date)
            {
                _resetTime = date;
                ForceUpdate();
            }

            private void DailyStepQueryHandler(nint stepCount, NSError error)
            {

#if DEBUG
                //stepCount = 1245;

                //stepCount = 6481;

                //stepCount = 9328;
#endif


                StepsChanged?.Invoke((uint)stepCount, TimePassed, Distance);
            }

            private void Updater(nint stepCount, NSDate date, NSError error)
            {
                NSDate sMidnight = DateHelpers.DateTimeToNSDate(_resetTime);
                _stepCounter.QueryStepCount(sMidnight, NSDate.Now, _queue, DailyStepQueryHandler);
            }

            //public event DailyStepCountChangedEventHandler DailyStepCountChanged;

            public static class DateHelpers
            {
                public static DateTime NSDateToDateTime(NSDate date)
                {
                    return (new DateTime(2001, 1, 1, 0, 0, 0)).AddSeconds(date.SecondsSinceReferenceDate);
                }

                public static NSDate DateTimeToNSDate(DateTime date)
                {
                    return NSDate.FromTimeIntervalSinceReferenceDate((date - (new DateTime(2001, 1, 1, 0, 0, 0))).TotalSeconds);
                }
            }
            public static class Fonts
            {
                public static UIFont SemiBold
                {
                    get { return UIFont.FromName("Raleway-SemiBold", 75); }
                }

                public static UIFont Light
                {
                    get { return UIFont.FromName("Raleway-Light", 18); }
                }

            }
            public class Keys
            {
                public static string InsightsKey = "";
            }
            public static class Settings
            {
                public static bool DistanceIsMetric
                {
                    get
                    {
                        return NSUserDefaults.StandardUserDefaults.BoolForKey("DistanceInKM");
                    }
                    set
                    {
                        NSUserDefaults.StandardUserDefaults.SetBool(value, "DistanceInKM");
                        NSUserDefaults.StandardUserDefaults.Synchronize();
                    }
                }

                public static bool OverideDistance
                {
                    get
                    {
                        return NSUserDefaults.StandardUserDefaults.BoolForKey("OverideDistance");
                    }
                    set
                    {
                        NSUserDefaults.StandardUserDefaults.SetBool(value, "OverideDistance");
                        NSUserDefaults.StandardUserDefaults.Synchronize();
                    }
                }

                public static bool HealthKitHeightEnabled
                {
                    get
                    {
                        return NSUserDefaults.StandardUserDefaults.BoolForKey("HealthKitHeightEnabled");
                    }
                    set
                    {
                        NSUserDefaults.StandardUserDefaults.SetBool(value, "HealthKitHeightEnabled");
                        NSUserDefaults.StandardUserDefaults.Synchronize();
                    }
                }
            }
#elif __ANDROID__
        JavaObject, IStepCounter, ISensorEventListener, IDisposable
        {
            public ValueTask<uint> Steps { get => new ValueTask<uint>(Steps_); }
            public uint Steps_ { get => SmallSteps; }
            public uint SmallSteps { get => numStepsRaw; }
            public uint BigSteps { get => numStepsWithFilter; }
            public DateTime StartTime { get; private set; }
            public TimeSpan TimeOffset { get; private set; }

            private static string TAG = "InnoTecheLearning.Sports";

            private static string PREFS_NAME = "PedometerPrefs";

            private static int INTERVAL_VARIATION = 250;

            private static uint NUM_INTERVALS = 2;

            private static int WIN_SIZE = 100;

            private static float STRIDE_LENGTH = 0.73f;

            private static float PEAK_VALLEY_RANGE = 40.0f;

            private Context context;

            private SensorManager sensorManager;

            private uint stopDetectionTimeout = 2000;

            private int winPos = 0;

            private uint intervalPos = 0;

            private uint numStepsWithFilter = 0;

            private uint numStepsRaw = 0;

            private float lastValley = 0;

            private float[] lastValues = new float[WIN_SIZE];

            private float strideLength = STRIDE_LENGTH;

            //private float totalDistance = 0;

            private long[] stepInterval = new long[NUM_INTERVALS];

            private long stepTimestamp = 0;

            private long startTime = 0;

            private long prevStopClockTime = 0;

            private bool foundValley = false;

            private bool startPeaking = false;

            private bool foundNonStep = true;

            private bool pedometerPaused = true;

            private float[] avgWindow = new float[10];

            private int avgPos = 0;

            public StepCounter() : base()
            {
                context = Xamarin.Forms.Forms.Context;
                //  some initialization
                winPos = 0;
                startPeaking = false;
                numStepsWithFilter = 0;
                numStepsRaw = 0;
                foundValley = true;
                lastValley = 0;
                sensorManager = ((SensorManager)(context.GetSystemService(Context.SensorService)));
                //  Restore preferences
                ISharedPreferences settings = context.GetSharedPreferences(PREFS_NAME, FileCreationMode.Private);
                strideLength = settings.GetFloat("Pedometer.stridelength", STRIDE_LENGTH);
                Distance = settings.GetFloat("Pedometer.distance", 0);
                numStepsRaw = (uint)settings.GetInt("Pedometer.prevStepCount", 0);
                prevStopClockTime = settings.GetLong("Pedometer.clockTime", 0);
                numStepsWithFilter = numStepsRaw;
                startTime = CurrentTimeMillis();
                Debug(TAG, "Pedometer Created");
            }

            //  Simple functions
            ///<summary>Start counting steps</summary>
            public ValueTask<Unit> Start()
            {
                return Unit.InvokeAsync(() =>
                {
                    if (pedometerPaused)
                    {
                        pedometerPaused = false;
                        StartTime = DateTime.Now - TimeOffset;
                        sensorManager.RegisterListener(this,
                            sensorManager.GetSensorList(SensorType.Accelerometer)[0], SensorDelay.Fastest);
                        startTime = CurrentTimeMillis();
                        TimeOffset = TimeSpan.Zero;
                    }
                });
            }

            ///<summary>Stop counting steps</summary>
            public void Stop()
            {
                Pause();
            }

            ///<summary>Resets the step counter, distance measure and time running.</summary>
            public void Reset()
            {
                numStepsWithFilter = 0;
                numStepsRaw = 0;
                Distance = 0;
                prevStopClockTime = 0;
                startTime = CurrentTimeMillis();
                StartTime = default(DateTime);
                TimeOffset = TimeSpan.Zero;
            }

            ///<summary>Resumes counting, synonym of Start.</summary>
            public void Resume()
            {
                Start();
            }

            ///<summary>Pause counting of steps and distance.</summary>
            public void Pause()
            {
                if (!pedometerPaused)
                {
                    TimeOffset = DateTime.Now - StartTime;
                    pedometerPaused = true;
                    //StartTime = DateTime.MinValue;
                    sensorManager.UnregisterListener(this);
                    Debug(TAG, "Unregistered listener on pause");
                    prevStopClockTime = (prevStopClockTime + (CurrentTimeMillis() - startTime));
                }

            }

            ///<summary>Saves the pedometer state to the phone. Permits
            ///accumulation of steps and distance between invocations of an App that uses 
            ///the pedometer. Different Apps will have their own saved state.</summary>
            public void Save()
            {
                //  Store preferences
                ISharedPreferences settings = context.GetSharedPreferences(PREFS_NAME, FileCreationMode.Private);
                ISharedPreferencesEditor editor = settings.Edit();
                editor.PutFloat("Pedometer.stridelength", strideLength);
                editor.PutFloat("Pedometer.distance", Distance);
                editor.PutInt("Pedometer.prevStepCount", (int)numStepsRaw);
                if (pedometerPaused)
                {
                    editor.PutLong("Pedometer.clockTime", prevStopClockTime);
                }
                else
                {
                    editor.PutLong("Pedometer.clockTime", (prevStopClockTime
                                    + (CurrentTimeMillis() - startTime)));
                }

                editor.PutLong("Pedometer.closeTime", CurrentTimeMillis());
                editor.Commit();
                Debug(TAG, "Pedometer state saved.");
            }

            //  Events
            public void RaiseSimpleStep(uint simpleSteps, float distance)
            {
                SimpleStep?.Invoke(simpleSteps, distance);
                StepsChanged?.Invoke(Steps_, TimePassed, distance);
            }
            ///<summary>This event is run when a raw step is detected</summary>
            public event StepEventHandler SimpleStep;
            /// <summary>
            /// This event is run when a walking step is detected. 
            /// A walking step is a step that appears to be involved in forward motion.
            /// </summary>
            public event StepEventHandler WalkStep;
            public delegate void StepEventHandler(uint Steps, float Distance);
            public void RaiseWalkStep(uint walkSteps, float distance)
            {
                WalkStep?.Invoke(walkSteps, distance);
            }

            /// <summary>
            /// The duration in milliseconds of idleness (no steps detected) 
            /// after which to go into a "stopped" state. Default: 2000
            /// </summary>
            public uint StopDetectionTimeout
            {
                get
                {
                    return stopDetectionTimeout;
                }
                set { stopDetectionTimeout = value; }
            }

            /// <summary>
            /// Time elapsed in milliseconds since the pedometer was started.
            /// </summary>
            public long ElapsedTime
            {
                get
                {
                    if (pedometerPaused)
                    {
                        return prevStopClockTime;
                    }
                    else
                    {
                        return (prevStopClockTime + (CurrentTimeMillis() - startTime));
                    }
                }
            }

            /// <summary>
            /// The number of simple steps taken since the pedometer has started.
            /// </summary>
            public uint SimpleSteps
            {
                get
                {
                    return numStepsRaw;
                }
            }

            /// <summary>
            /// The number of walk steps taken since the pedometer has started.
            /// </summary>
            public uint WalkSteps
            {
                get
                {
                    return numStepsWithFilter;
                }
            }

            private bool StepsEquallySpaced
            {
                get
                {
                    float avg = 0;
                    int num = 0;
                    foreach (long interval in stepInterval)
                    {
                        if ((interval > 0))
                        {
                            num++;
                            avg = (avg + interval);
                        }

                    }

                    avg = (avg / num);
                    foreach (long interval in stepInterval)
                    {
                        if (Java.Lang.Math.Abs(interval - avg) > INTERVAL_VARIATION)
                        {
                            return false;
                        }

                    }

                    return true;
                }
            }

            private bool Peak
            {
                get
                {
                    int mid = ((winPos
                                + (WIN_SIZE / 2))
                                % WIN_SIZE);
                    for (int i = 0; (i < WIN_SIZE); i++)
                    {
                        if (((i != mid)
                                    && (lastValues[i] > lastValues[mid])))
                        {
                            return false;
                        }

                    }

                    return true;
                }
            }

            private bool Valley
            {
                get
                {
                    int mid = ((winPos
                                + (WIN_SIZE / 2))
                                % WIN_SIZE);
                    for (int i = 0; (i < WIN_SIZE); i++)
                    {
                        if (((i != mid)
                                    && (lastValues[i] < lastValues[mid])))
                        {
                            return false;
                        }

                    }

                    return true;
                }
            }

            //  SensorEventListener implementation
            public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
            {
                Debug(TAG, "Accelerometer accuracy changed.");
            }
            /* How to debug a method
            public void OnSensorChanged(SensorEvent @event)
            {
                int Line = 534;
                try
                {
       Line = 537;  if (@event.Sensor.Type != SensorType.Accelerometer) return;
       Line = 538;  System.Collections.Generic.IList<float> values = @event.Values;
       Line = 539;  float magnitude = 0;
       Line = 540;  foreach (float v in values) magnitude += v * v;
                    // Check if the middle reading within the current window represents
                    // a peak/valley.
       Line = 543;  int mid = (winPos + WIN_SIZE / 2) % WIN_SIZE;

                    // Peak is detected
       Line = 546;  if (startPeaking && isPeak)
                    {
       Line = 548;      if (foundValley && lastValues[mid] - lastValley > PEAK_VALLEY_RANGE)
                        {
                            // Step detected on axis k with maximum peak-valley range.
               Line = 551;  long timestamp = CurrentTimeMillis();
               Line = 552;  stepInterval[intervalPos] = timestamp - stepTimestamp;
               Line = 553;  intervalPos = (intervalPos + 1) % NUM_INTERVALS;
               Line = 554;  stepTimestamp = timestamp;
               Line = 555;  if (areStepsEquallySpaced)
                            {
               Line = 557;      if (foundNonStep)
                                {
               Line = 559;          numStepsWithFilter += NUM_INTERVALS;
               Line = 560;          Distance += strideLength * NUM_INTERVALS;
               Line = 561;          foundNonStep = false;
                                }
               Line = 563;        numStepsWithFilter++;
               Line = 564;        RaiseWalkStep(numStepsWithFilter, Distance);
               Line = 565;        Distance += strideLength;
                            }
                            else
                            {
               Line = 569;        foundNonStep = true;
                            }
               Line = 571;   numStepsRaw++;
               Line = 572;   RaiseSimpleStep(numStepsRaw, Distance);
               Line = 573;   foundValley = false;
                        }
                    }
        Line = 576; if (startPeaking && isValley)
                    {
        Line = 578;     foundValley = true;
        Line = 579;     lastValley = lastValues[mid];
                    }
                    // Store latest accelerometer reading in the window.
        Line = 582; avgWindow[avgPos] = magnitude;
        Line = 583; avgPos = (avgPos + 1) % avgWindow.Length;
        Line = 584; lastValues[winPos] = 0;
        Line = 585; foreach (float m in avgWindow) lastValues[winPos] += m;
        Line = 586; lastValues[winPos] /= avgWindow.Length;
        Line = 587; if (startPeaking || winPos > 1)
                    {
        Line = 589;     int i = winPos;
        Line = 590;     if (--i < 0) i += WIN_SIZE;
        Line = 591;     lastValues[winPos] += 2 * lastValues[i];
        Line = 592;     if (--i < 0) i += WIN_SIZE;
        Line = 593;     lastValues[winPos] += lastValues[i];
        Line = 594;     lastValues[winPos] /= 4;
        Line = 595; }
                    else if (!startPeaking && winPos == 1)
                    {
        Line = 598;     lastValues[1] = (lastValues[1] + lastValues[0]) / 2f;
                    }

        Line = 601; long elapsedTimestamp = CurrentTimeMillis();
        Line = 602; if (elapsedTimestamp - stepTimestamp > stopDetectionTimeout)
                    {
        Line = 604;     stepTimestamp = elapsedTimestamp;
                    }
                    // Once the buffer is full, start peak/valley detection.
        Line = 607; if (winPos == WIN_SIZE - 1 && !startPeaking)
                    {
        Line = 609;     startPeaking = true;
                    }
                    // Increment position within the window.
        Line = 612; winPos = (winPos + 1) % WIN_SIZE;
                }
                catch (Exception ex)
                {
                    throw new Exception(Line.ToString(), ex);
                }
            }
            */
            public void OnSensorChanged(SensorEvent @event)
            {
                if (@event.Sensor.Type != SensorType.Accelerometer) return;
                System.Collections.Generic.IList<float> values = @event.Values;
                float magnitude = 0;
                foreach (float v in values) magnitude += v * v;
                // Check if the middle reading within the current window represents
                // a peak/valley.
                int mid = (winPos + WIN_SIZE / 2) % WIN_SIZE;

                // Peak is detected
                if (startPeaking && Peak)
                {
                    if (foundValley && lastValues[mid] - lastValley > PEAK_VALLEY_RANGE)
                    {
                        // Step detected on axis k with maximum peak-valley range.
                        long timestamp = CurrentTimeMillis();
                        stepInterval[intervalPos] = timestamp - stepTimestamp;
                        intervalPos = (intervalPos + 1) % NUM_INTERVALS;
                        stepTimestamp = timestamp;
                        if (StepsEquallySpaced)
                        {
                            if (foundNonStep)
                            {
                                numStepsWithFilter += NUM_INTERVALS;
                                Distance += strideLength * NUM_INTERVALS;
                                foundNonStep = false;
                            }
                            numStepsWithFilter++;
                            RaiseWalkStep(numStepsWithFilter, Distance);
                            Distance += strideLength;
                        }
                        else
                        {
                            foundNonStep = true;
                        }
                        numStepsRaw++;
                        RaiseSimpleStep(numStepsRaw, Distance);
                        foundValley = false;
                    }
                }
                if (startPeaking && Valley)
                {
                    foundValley = true;
                    lastValley = lastValues[mid];
                }
                // Store latest accelerometer reading in the window.
                avgWindow[avgPos] = magnitude;
                avgPos = (avgPos + 1) % avgWindow.Length;
                lastValues[winPos] = 0;
                foreach (float m in avgWindow) lastValues[winPos] += m;
                lastValues[winPos] /= avgWindow.Length;
                if (startPeaking || winPos > 1)
                {
                    int i = winPos;
                    if (--i < 0) i += WIN_SIZE;
                    lastValues[winPos] += 2 * lastValues[i];
                    if (--i < 0) i += WIN_SIZE;
                    lastValues[winPos] += lastValues[i];
                    lastValues[winPos] /= 4;
                }
                else if (!startPeaking && winPos == 1)
                {
                    lastValues[1] = (lastValues[1] + lastValues[0]) / 2f;
                }

                long elapsedTimestamp = CurrentTimeMillis();
                if (elapsedTimestamp - stepTimestamp > stopDetectionTimeout)
                {
                    stepTimestamp = elapsedTimestamp;
                }
                // Once the buffer is full, start peak/valley detection.
                if (winPos == WIN_SIZE - 1 && !startPeaking)
                {
                    startPeaking = true;
                }
                // Increment position within the window.
                winPos = (winPos + 1) % WIN_SIZE;
            }
            //  Deleteable implementation
            public new void Dispose()
            {
                base.Dispose();
                sensorManager.UnregisterListener(this);
            }

            //  DEPRECATED:
            //  Everything below here is deprecated. We cannot completely remove them
            //  because older projects loaded into the system that use them would break.
            //  Instead we leave stub routines which are annotated as @Obsolete. This
            //  will result in blocks for these routines to be marked bad when a project
            //  that has them is loaded.
            [Obsolete("App Inventor has already deprecated these.", true)]
            //[SimpleEvent()]
            public void StartedMoving()
            {

            }

            [Obsolete("App Inventor has already deprecated these.", true)]
            //[SimpleEvent()]
            public void StoppedMoving()
            {

            }

            [Obsolete("App Inventor has already deprecated these.", true)]
            //[SimpleProperty(category = PropertyCategory.BEHAVIOR)]
            public void UseGPS(bool gps)
            {

            }

            [Obsolete("App Inventor has already deprecated these.", true)]
            //[SimpleEvent()]
            public void CalibrationFailed()
            {

            }

            [Obsolete("App Inventor has already deprecated these.", true)]
            //[SimpleEvent()]
            public void GPSAvailable()
            {

            }

            [Obsolete("App Inventor has already deprecated these.", true)]
            //[SimpleEvent()]
            public void GPSLost()
            {

            }

            //  Properties
            [Obsolete("App Inventor has already deprecated these.", true)]
            public bool CalibrateStrideLength
            {
                get
                {
                    return false;
                }
                set { }
            }

            [Obsolete("App Inventor has already deprecated these.", true)]
            public bool Moving
            {
                get
                {
                    return false;
                }
            }
#elif WINDOWS_UWP
        IStepCounter { 
            public async ValueTask<Unit> Start()
            {
                readings = await Pedometer.GetDefaultAsync();
                StartTime = DateTime.Now;
                readings.ReportInterval = 100;
                readings.ReadingChanged += Readings_ReadingChanged;
                return Unit.Default;
            }
            public void Stop()
            { readings.ReadingChanged -= Readings_ReadingChanged; }
            public void Reset()
            { SmallSteps = BigSteps = 0; StartTime = DateTime.MinValue; }

            public uint SmallSteps { get; private set; }
            public uint BigSteps { get; private set; }
            public ValueTask<uint> Steps { get => new ValueTask<uint>(Steps_); }
            public uint Steps_ { get => SmallSteps + BigSteps; }
            public DateTime StartTime { get; private set; }
            Pedometer readings;
            
            private void Readings_ReadingChanged(Pedometer sender, PedometerReadingChangedEventArgs args)
            {
                PedometerReading readvalues = args.Reading;
                StepsChanged?.Invoke(Steps_, TimePassed, Distance);
                if (readvalues.StepKind == PedometerStepKind.Walking)
                {
                    SmallSteps = (uint)readvalues.CumulativeSteps;
                }
                else if (readvalues.StepKind == PedometerStepKind.Running)
                {
                    BigSteps = (uint)readvalues.CumulativeSteps;
                }
            }
            /*public async void gettingHistory()
            {
                var history = await Pedometer.GetSystemHistoryAsync(DateTime.Now.AddDays(-30));
            }*/
#elif NETFX_CORE
        IStepCounter {
            private bool hasChanged;
            public uint Steps { get; private set; } = 0;
            public DateTime StartTime { get; private set; }
            private double x_old;
            private double y_old;
            private double z_old;
            Accelerometer Sensor;

            public void Start()
            {
                Sensor = Accelerometer.GetDefault();
                StartTime = DateTime.Now;
                Sensor.ReportInterval = 100;
                Sensor.ReadingChanged += checkIsMovement;
            }
            public void Stop()
            { Sensor.ReadingChanged -= checkIsMovement; }

            public void Reset()
            { Steps = 0; StartTime = DateTime.MinValue; }

            private void checkIsMovement(Accelerometer sender, AccelerometerReadingChangedEventArgs e)
            {
                double x = e.Reading.AccelerationX;
                double y = e.Reading.AccelerationY;
                double z = e.Reading.AccelerationZ;
                double oldValue = ((x_old * x) + (y_old * y)) + (z_old * z);
                double oldValueSqrt = Math.Abs(Math.Sqrt((double)(((x_old * x_old) + (y_old * y_old)) + (z_old * z_old))));
                double newValue = Math.Abs(Math.Sqrt((double)(((x * x) + (y * y)) + (z * z))));
                oldValue /= oldValueSqrt * newValue;
                if ((oldValue <= 0.994) && (oldValue > 0.9))
                {
                    if (!hasChanged)
                    {
                        hasChanged = true;
                        StepsChanged?.Invoke(Steps, TimePassed, Distance);
                        Steps++; //here the counter
                    }
                    else
                    {
                        hasChanged = false;
                    }
                }
                x_old = x;
                y_old = y;
                z_old = z;
            }
#else
        {
#endif
            public TimeSpan TimePassed
            {
                get
                {
#if __ANDROID__
                    if (TimeOffset != TimeSpan.Zero) return TimeOffset;
#endif
                    return StartTime == default(DateTime) ? TimeSpan.Zero : DateTime.Now - StartTime;
                }
            }
            public event StepCountChangedEventHandler StepsChanged;

            //  Properties
            /// <summary>
            /// Set the nonnegative average stride length in meters. Default: 0.73
            /// </summary>
            public float StrideLength
            { get; set; }
            
            /// <summary>
            /// The approximate distance traveled in meters.
            /// </summary>
            public float Distance
            {
#if __ANDROID__
                get; private set;
#else
                get { return StrideLength * Steps_; }
#endif

            }

            public StepCounter(StepCountChangedEventHandler Handler = null)
#if __ANDROID__
                :this()
#endif
            { if(Handler != null) StepsChanged += Handler; }
            
            public static class Conversion
            {
                public static double StepCountToPercentage(int stepCount)
                {
                    var per = (stepCount / (decimal)10000) * 100;
                    return ((double)per);
                }

                public static double StepCountToPercentage(Int64 stepCount)
                {
                    var per = (stepCount / 10000m) * 100;
                    return ((double)per);
                }

                public static int PercentageToStepCount(double percent)
                {
                    if (!(percent > 0)) return 0;
                    var steps = (10000 / (decimal)percent) * 100;
                    return ((int)steps);
                }

                public static float StepsToMiles(int stepCount)
                {
                    if (stepCount <= 0) return 0.00f;
                    //Average steps in a mile
                    const float stepsPerMile = 2000;
                    return stepCount / stepsPerMile;
                }

                public static float StepsToMiles(Int64 stepCount)
                {
                    if (stepCount <= 0) return 0.00f;
                    //Average steps in a mile
                    const float stepsPerMile = 2000;
                    return stepCount / stepsPerMile;
                }

                public static float StepsToKilometers(int stepCount)
                {
                    var miles = StepsToMiles(stepCount);
                    return miles * 1.609344f;
                }

                public static float StepsToKilometers(Int64 stepCount)
                {
                    var miles = StepsToMiles(stepCount);
                    return miles * 1.609344f;
                }


                public static string CaloriesBurnt(float miles)
                {
                    const int caloriesBurntPerMile = 100;
                    var val = miles * caloriesBurntPerMile;
                    return val <= 0 ? "0" : val.ToString("N0");
                }

                /// <summary>
                /// Calorieses the burnt with weight entered
                /// view-source:http://walking.about.com/library/cal/uccalc1.htm
                /// </summary>
                /// <returns>The burnt calories.</returns>
                /// <param name="miles">Miles.</param>
                /// <param name="lbs">Lbs.</param>
                public static string CaloriesBurnt(float miles, float lbs, string cadence)
                {
                    if (lbs <= 0)
                        return CaloriesBurnt(miles);

                    var met = 3.5;
                    var pace = 3.0;
                    switch (cadence)
                    {
                        case "0":
                            met = 2.0;
                            pace = 2.0;
                            break;
                        case "1":
                            met = 2.5;
                            pace = 2.0;
                            break;
                        case "2":
                            met = 3.0;
                            pace = 2.5;
                            break;
                        case "3":
                            met = 3.5;
                            pace = 3.0;
                            break;
                        case "4":
                            met = 5.0;
                            pace = 4.0;
                            break;
                        case "5":
                            met = 6.3;
                            pace = 4.5;
                            break;
                        case "6":
                            met = 8.0;
                            pace = 5.0;
                            break;
                    }

                    var adjusted_weight = lbs / 2.2;
                    var val = Math.Round(((adjusted_weight * met) / pace) * miles);
                    return val <= 0 ? "0" : val.ToString("N0");
                }
            }

        }
    }
}
