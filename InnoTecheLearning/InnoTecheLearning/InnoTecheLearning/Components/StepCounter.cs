
using System;
#if __IOS__
using CoreMotion;
using Foundation;
using UIKit;
#elif __ANDROID__
using Android.Content.PM;
using Android.Hardware;
using Android.OS;
#endif

namespace InnoTecheLearning
{
    partial class Utils
    {
        public class StepCounter
        {

#if __IOS__
            public delegate void DailyStepCountChangedEventHandler(nint stepCount);

            private NSOperationQueue _queue;
            private DateTime _resetTime;
            private CMStepCounter _stepCounter;

            public StepCounter()
            {
                ForceUpdate();

                _stepCounter.StartStepCountingUpdates(_queue, 1, Updater);
            }

            public void ForceUpdate()
            {
                //If the last reset date wasn't today then we should update this.
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
                if (DailyStepCountChanged == null)
                    return;

#if DEBUG
                stepCount = 1245;

                //stepCount = 6481;

                //stepCount = 9328;
#endif


                DailyStepCountChanged(stepCount);
            }

            private void Updater(nint stepCount, NSDate date, NSError error)
            {
                NSDate sMidnight = DateHelpers.DateTimeToNSDate(_resetTime);
                _stepCounter.QueryStepCount(sMidnight, NSDate.Now, _queue, DailyStepQueryHandler);
            }

            public event DailyStepCountChangedEventHandler DailyStepCountChanged;

            public static class Conversion
            {
                public static double StepCountToPercentage(int stepCount)
                {
                    var per = (stepCount / (decimal)10000) * 100;
                    return ((double)per);
                }

                public static double StepCountToPercentage(Int64 stepCount)
                {
                    var per = (stepCount / (decimal)10000) * 100;
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
        public static bool IsKitKatWithStepCounter(PackageManager pm)
        {

            // Require at least Android KitKat
            int currentApiVersion = (int)Build.VERSION.SdkInt;
            // Check that the device supports the step counter and detector sensors
            return currentApiVersion >= 19
            && pm.HasSystemFeature(PackageManager.FeatureSensorStepCounter)
            && pm.HasSystemFeature(PackageManager.FeatureSensorStepDetector);

        }
        private int stepCounter = 0;
        private int counterSteps = 0;
        private int stepDetector = 0;

        public void OnSensorChanged(SensorEvent e)
        {
            switch (e.Sensor.Type)
            {
                case SensorType.StepDetector:
                    stepDetector++;
                    break;
                case SensorType.StepCounter:
                    //Since it will return the total number since we registered we need to subtract the initial amount
                    //for the current steps since we opened app
                    if (counterSteps < 1)
                    {
                        // initial value
                        counterSteps = (int)e.Values[0];
                    }

                    // Calculate steps taken based on first counter value received.
                    stepCounter = (int)e.Values[0] - counterSteps;
                    break;
            }
        }
            public override void OnStart(Intent intent, int startId)
            {
                //base.OnStart(intent, startId);
                if (isRunning || !Utils.IsKitKatWithStepCounter(PackageManager))
                    return;

                RegisterListeners(SensorType.StepCounter);
            }




            void RegisterListeners(SensorType sensorType)
            {


                var sensorManager = (SensorManager)GetSystemService(Context.SensorService);
                var sensor = sensorManager.GetDefaultSensor(sensorType);

                sensorManager.RegisterListener(this, sensor, SensorDelay.Normal);
                Console.WriteLine("Sensor listener registered of type: " + sensorType);
            }


            void UnregisterListeners()
            {

                var sensorManager = (SensorManager)GetSystemService(Context.SensorService);
                sensorManager.UnregisterListener(this);
                Console.WriteLine("Sensor listener unregistered.");

                isRunning = false;
            }

            public override void OnDestroy()
            {
                //base.OnDestroy();
                UnregisterListeners();
                isRunning = false;
            }
#endif
        }
}
}