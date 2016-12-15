using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Linq; 
using System.Runtime.InteropServices.WindowsRuntime; 
using Xamarin.Forms;
using InnoTecheLearning;
//using Xamarin.Media;
#if __IOS__
using AVFoundation;
using Foundation;
#elif __ANDROID__
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;
using Java.Lang;
#elif NETFX_CORE
using Windows.Foundation; 
using Windows.Foundation.Collections; 
using Windows.Media.Capture; 
using Windows.Media.MediaProperties; 
using Windows.Storage; 
using Windows.Storage.Streams; 
using Windows.UI.Core; 
using Windows.UI.Xaml; 
using Windows.UI.Xaml.Controls; 
using Windows.UI.Xaml.Controls.Primitives; 
using Windows.UI.Xaml.Data; 
using Windows.UI.Xaml.Input; 
using Windows.UI.Xaml.Media; 
using Windows.UI.Xaml.Navigation; 
#endif
[assembly: Dependency(typeof(SoundRecorder))]
/*      Windows.Storage.StorageFolder folder =
        await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(folderName);
    Windows.Storage.StorageFile file = await folder.GetFileAsync(fileName);
    Windows.Storage.Streams.IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

    MediaElement el = new MediaElement();
    el.SetSource(stream, file.ContentType);
    el.IsMuted = false;
    el.Volume = 1;
    await new Task(() =>
    {
        el.Position = new TimeSpan(0, 0, 0);
        el.Play();
    });*/
namespace InnoTecheLearning
{   /// <summary>
    /// Cross-platform access to <see cref="SoundRecorder"/>.
    /// </summary>
    public static class Recorder
    {/// <summary>
     /// Cross-platform access to <see cref="SoundRecorder"/>.
     /// </summary>
        public static ISoundRecorder GetImplementation
        { get { return DependencyService.Get<ISoundRecorder>(); } }
        /// <summary>
        /// Cross-platform access to <see cref="SoundRecorder.deleteRecord"/>.
        /// </summary>
        public static void deleteRecord()
        { GetImplementation.deleteRecord(); }
        /// <summary>
        /// Cross-platform access to <see cref="SoundRecorder.Record"/>.
        /// </summary>
        public static void Record()
        { GetImplementation.Record(); }
        /// <summary>
        /// Cross-platform access to <see cref="SoundRecorder.PlayRecord"/>.
        /// </summary>
        public static void PlayRecord()
        { GetImplementation.PlayRecord(); }
        /// <summary>
        /// Cross-platform access to <see cref="SoundRecorder.Stop"/>.
        /// </summary>
        public static void Stop()
        { GetImplementation.Stop(); }
        /// <summary>
        /// Cross-platform access to <see cref="SoundRecorder.Pause"/>.
        /// </summary>
        /// <returns>Whether the operation was successful.</returns>
        public static bool Pause()
        {
            try
            {
                GetImplementation.Pause();
                return true;
            }
            catch
            { return false; }
        }
    }
    /// <summary>
    /// Provides an interface for <see cref="SoundRecorder"/> and cross-platform sound recording.
    /// </summary>
    public interface ISoundRecorder
    {
        void Record();

        void PlayRecord();

        void Stop();

        void Pause();

        void deleteRecord();
    }
    /// <summary>
    /// The platform-specific implementation of <see cref="ISoundRecorder"/>.
    /// </summary>
#if __IOS__
    public class SoundRecorder : ISoundRecorder
    {

        AVAudioRecorder recorder;
        AVAudioPlayer player;
        NSUrl audioFilePath;
        byte[] audioDataBytes;

        public void PlayRecord()
        {
            try
            {
                NSError error = null;
                AVAudioSession.SharedInstance().SetCategory(AVAudioSession.CategoryPlayback, out error);
                if (error != null)
                    throw new Exception(error.DebugDescription);

                player = AVAudioPlayer.FromData(NSData.FromArray(audioDataBytes));
                player.FinishedPlaying += (sender, e) =>
                {
                    Console.WriteLine("send message to parent");
                    MessagingCenter.Send<ISoundRecorder, bool>(this, "finishReplaying", true);
                };
                player.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was a problem playing back audio: ");
                Console.WriteLine(ex.Message);
            }
        }

        public void Record()
        {
            Console.WriteLine("Begin Recording");

            var session = AVAudioSession.SharedInstance();

            NSError error = null;
            session.SetCategory(AVAudioSession.CategoryRecord, out error);
            if (error != null)
            {
                Console.WriteLine(error);
                return;
            }

            session.SetActive(true, out error);
            if (error != null)
            {
                Console.WriteLine(error);
                return;
            }

            if (!PrepareAudioRecording())
            {
                return;
            }

            if (!recorder.Record())
            {
                return;
            }
        }

        bool PrepareAudioRecording()
        {
            audioFilePath = CreateOutputUrl();

            var audioSettings = new AudioSettings
            {
                SampleRate = 44100,
                Format = AudioToolbox.AudioFormatType.MPEG4AAC,
                NumberChannels = 1,
                AudioQuality = AVAudioQuality.High
            };

            //Set recorder parameters
            NSError error;

            recorder = AVAudioRecorder.Create(audioFilePath, audioSettings, out error);
            if (error != null)
            {
                Console.WriteLine(error);
                return false;
            }

            //Set Recorder to Prepare To Record
            if (!recorder.PrepareToRecord())
            {
                recorder.Dispose();
                recorder = null;
                return false;
            }

            recorder.FinishedRecording += OnFinishedRecording;

            return true;
        }

        void OnFinishedRecording(object sender, AVStatusEventArgs e)
        {
            recorder.Dispose();
            recorder = null;
            Console.WriteLine("Done Recording (status: {0})", e.Status);
        }

        NSUrl CreateOutputUrl()
        {
            string fileName = string.Format("Myfile{0}.aac", DateTime.Now.ToString("yyyyMMddHHmmss"));
            string tempRecording = Path.Combine(Path.GetTempPath(), fileName);

            return NSUrl.FromFilename(tempRecording);
        }

        public void Stop()
        {
            if (recorder == null)
                return;

            recorder.Stop();

            NSError error = null;
            NSData audioData = NSData.FromFile(audioFilePath.Path, 0, out error);
            if (error != null)
            {
                Console.WriteLine(error);
                return;
            }

            audioDataBytes = new byte[audioData.Length];
            System.Runtime.InteropServices.Marshal.Copy(audioData.Bytes, audioDataBytes, 0, Convert.ToInt32(audioData.Length));
        }

        public void Pause()
        {
            if (player == null)
                return;

            if (recorder != null && recorder.Recording)
                return;

            player.Pause();
        }

        public void deleteRecord()
        {
            if (player != null)
            {
                player.Dispose();
                player = null;
            }

            if (recorder != null)
            {
                recorder.Dispose();
                recorder = null;
            }
        }
    }
#elif __ANDROID__
    public class SoundRecorder : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, 
        ISoundRecorder, AudioTrack.IOnPlaybackPositionUpdateListener
        {

            AudioRecord audRecorder;
            AudioTrack audioTrack;
            volatile bool _isRecording = false;
            private byte[] audioBuffer;
            private int audioData;
            public static string wavPath;
            byte[] audioDataBytes;

            public void PlayRecord()
            {
                new Thread(delegate ()
                {
                    PlayAudioTrack(audioDataBytes);
                }).Start();
            }

            public void Record()
            {
                _isRecording = true;
                new Thread(delegate ()
                {
                    RecordAudio();
                }).Start();
            }

            public void Stop()
            {
                if (_isRecording == true)
                {
                    _isRecording = false;
                    audRecorder.Stop();
                    audioDataBytes = File.ReadAllBytes(wavPath);
                    audRecorder.Release();
                }

            }

            private void PlayAudioTrack(byte[] audBuffer)
            {
                try
                {
                    audioTrack = new AudioTrack(
                    // Stream type
                    Android.Media.Stream.Music,
                    // Frequency
                    11025,
                    // Mono or stereo
                    ChannelOut.Mono,
                    // Audio encoding
                    Android.Media.Encoding.Pcm16bit,
                    // Length of the audio clip.
                    audBuffer.Length,
                    // Mode. Stream or static.
                    AudioTrackMode.Stream);

                    audioTrack.SetNotificationMarkerPosition(audBuffer.Length / 2);
                    audioTrack.SetPlaybackPositionUpdateListener(this);
                    audioTrack.Play();

                    audioTrack.Write(audBuffer, 0, audBuffer.Length);
                }
                catch 
                {
                    Console.WriteLine("Show something I'm giving up on you!!!");
                    MessagingCenter.Send<ISoundRecorder, bool>(this, "ErrorWhileReplaying", true);
                }

            }

            private void RecordAudio()
            {
                if (File.Exists(wavPath))
                    File.Delete(wavPath);

                System.IO.Stream outputStream = System.IO.File.Open(wavPath, FileMode.CreateNew);
                BinaryWriter bWriter = new BinaryWriter(outputStream);

                int bufferSize = AudioRecord.GetMinBufferSize(11025,
                    ChannelIn.Mono, Android.Media.Encoding.Pcm16bit);

                audioBuffer = new byte[bufferSize];

                audRecorder = new AudioRecord(
                    // Hardware source of recording.
                    AudioSource.Mic,
                    // Frequency
                    11025,
                    // Mono or stereo
                    ChannelIn.Mono,
                    // Audio encoding
                    Android.Media.Encoding.Pcm16bit,
                    // Length of the audio clip.
                    bufferSize
                );
                audRecorder.StartRecording();

                while (_isRecording == true)
                {
                    try
                    {
                        /// Keep reading the buffer while there is audio input.
                        audioData = audRecorder.Read(audioBuffer, 0, audioBuffer.Length);
                        bWriter.Write(audioBuffer);
                    }
                    catch (System.Exception ex)
                    {
                        System.Console.Out.WriteLine(ex.Message);
                        MessagingCenter.Send<ISoundRecorder, bool>(this, "finishReplaying", true);
                        break;
                    }
                }

                outputStream.Close();
                bWriter.Close();
            }

            protected override void OnCreate(Bundle bundle)
            {
                var cw = new ContextWrapper(this.ApplicationContext);
                var directory = cw.GetDir("testDirectory", FileCreationMode.Private);
                wavPath = string.Concat(directory.AbsolutePath, "/test.mp4");
            }

            public void Pause()
            {
                try
                {
                    if (audioTrack == null)
                        return;

                    if (audRecorder != null && audRecorder.RecordingState == RecordState.Recording)
                        return;

                    audioTrack.Pause();
                }
                catch 
                {
                    Console.WriteLine("Show something I'm giving up on you!!!");
                    MessagingCenter.Send<ISoundRecorder, bool>(this, "ErrorWhileReplaying", true);
                }

            }

            public void deleteRecord()
            {
                if (audioTrack != null)
                {
                    //audioTrack.Dispose();
                    //audioTrack = null;
                    audioTrack.Release();
                }

                if (audRecorder != null)
                {
                    //audRecorder.Dispose();
                    //audRecorder = null;
                    audRecorder.Release();
                }
            }

            public void OnMarkerReached(AudioTrack track)
            {
                Console.WriteLine("send message to parent");
                MessagingCenter.Send<ISoundRecorder, bool>(this, "finishReplaying", true);
            }

            public void OnPeriodicNotification(AudioTrack track) { }

        ~SoundRecorder()
        { deleteRecord(); }
        }
#elif NETFX_CORE
    public class SoundRecorder : ISoundRecorder
    {
        MediaCapture capture;
        InMemoryRandomAccessStream buffer;
        bool record;
        string filename;
        string audioFile = ".MP3";
        public async void Record()
        {
            if (record)
            {
                //already recored process  
            }
            else
            {
                await RecordProcess();
                await capture.StartRecordToStreamAsync(MediaEncodingProfile.CreateMp3(AudioEncodingQuality.Auto), buffer);
                if (record)
                    throw new InvalidOperationException();
                record = true;
            }
        }
        private async Task<bool> RecordProcess()
        {
            if (buffer != null)
            {
                buffer.Dispose();
            }
            buffer = new InMemoryRandomAccessStream();
            if (capture != null)
            {
                capture.Dispose();
            }
            try
            {
                MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings
                {
                    StreamingCaptureMode = StreamingCaptureMode.Audio
                };
                capture = new MediaCapture();
                await capture.InitializeAsync(settings);
                capture.RecordLimitationExceeded += async (MediaCapture sender) =>
                {
                    //Stop  
                    record = false;
                    await capture.StopRecordAsync();
                    throw new TimeoutException("Record Limitation Exceeded ");
                };
                capture.Failed += (MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs) =>
                {
                    record = false;
                    throw new InvalidOperationException(string.Format("Code: {0}.\n{1}", 
                        errorEventArgs.Code, errorEventArgs.Message));
                };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(UnauthorizedAccessException))
                {
                    throw ex.InnerException;
                }
                throw;
            }
            return true;
        }
        public async void PlayRecord()
        {
            await PlayRecordedAudio(global::Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher);
        }
        public async Task PlayRecordedAudio(CoreDispatcher UiDispatcher)
        {
            MediaElement playback = new MediaElement();
            IRandomAccessStream audio = buffer.CloneStream();

            if (audio == null)
                throw new ArgumentNullException("buffer");
            StorageFolder storageFolder = global::Windows.ApplicationModel.Package.Current.InstalledLocation;
            if (!string.IsNullOrEmpty(filename))
            {
                StorageFile original = await storageFolder.GetFileAsync(filename);
                await original.DeleteAsync();
            }
            await UiDispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                StorageFile storageFile = await storageFolder.CreateFileAsync(audioFile, CreationCollisionOption.GenerateUniqueName);
                filename = storageFile.Name;
                using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    await RandomAccessStream.CopyAndCloseAsync(audio.GetInputStreamAt(0), fileStream.GetOutputStreamAt(0));
                    await audio.FlushAsync();
                    audio.Dispose();
                }
                IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read);
                playback.SetSource(stream, storageFile.FileType);
                playback.Play();
            });
        }
#if WINDOWS_UWP
        public async void Pause()
        {
            await capture.PauseRecordAsync(Windows.Media.Devices.MediaCapturePauseBehavior.ReleaseHardwareResources);
        }
#else
        [DebuggerStepThrough, DebuggerHidden, DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Not supported in Windows 8.1 / Windows Phone 8.1", true)]
        public void Pause()
        {
            throw new PlatformNotSupportedException("Not supported in Windows 8.1 / Windows Phone 8.1");
        }
#endif
        public async void Stop()
        {
            await capture.StopRecordAsync();
            record = false;
        }
        public void deleteRecord()
        {   if (capture != null)
                capture.Dispose();
            if (buffer != null)
                buffer.Dispose();
        }

    }
#endif
}