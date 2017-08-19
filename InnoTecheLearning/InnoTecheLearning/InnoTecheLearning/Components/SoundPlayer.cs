using System;
using System.Threading.Tasks;
#if __IOS__
using AVFoundation;
using Foundation;
#elif __ANDROID__
using Android.Net;
using Android.Media;
using Java.IO;
using Xamarin.Forms;
using Uri = Android.Net.Uri;
#elif NETFX_CORE
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Streams;
//using static Windows.ApplicationModel.Package;
#endif

namespace InnoTecheLearning
{    /// <summary>
     /// Provides an interface for <see cref="SoundPlayer"/> and cross-platform sound playing.
     /// </summary>
    public interface ISoundPlayer
    {
        //static Task<ISoundPlayer> Create(string FilePath, bool Loop = false, double Volume = 1);
        //protected Task Init(string FilePath, bool Loop = false, double Volume = 1);
        //The task to wait because the method is asynchronous.
        /// <summary>
        /// Plays the sound.
        /// </summary>
        /*public */ void Play();
        /// <summary>
        /// Pauses the sound.
        /// </summary>
        /*public */ void Pause();
        /// <summary>
        /// Stops the sound.
        /// </summary>
        /*public */ void Stop();
        /// <summary>
        /// Occurs when the audio has completed playing.
        /// </summary>
        /// <returns></returns>
        event System.EventHandler Complete;
    }

    /// <summary>
    /// The platform-specific implementation of <see cref="ISoundPlayer"/>, but cross-platform.
    /// </summary>
    public class SoundPlayer : ISoundPlayer 
    {
        private SoundPlayer() : base()
        { }
#if __IOS__
        AVAudioPlayer _player;
        public static SoundPlayer Create(string FilePath, bool Loop = false, double Volume = 1)
        {
            var Return = new SoundPlayer();
            Return.Init(FilePath, Loop, Volume);
            return Return;
        }
        protected void Init(string FilePath, bool Loop, double Volume)
        {
            using (NSUrl url = NSUrl.FromString(FilePath))
                _player = AVAudioPlayer.FromUrl(url);
            _player.NumberOfLoops = Loop? 0: -1;
            _player.Volume = System.Convert.ToSingle(Volume);
            //_player.FinishedPlaying += (object sender, AVStatusEventArgs e) => { _player = null; };
        }
        public void Play()
        { _player.Play(); }
        public void Pause()
        { _player.Pause(); }
        public void Stop()
        { _player.Stop(); }
        public event System.EventHandler Complete
        { add { _player.FinishedPlaying += (System.EventHandler<AVFoundation.AVStatusEventArgs>)(System.MulticastDelegate)value; }
        remove { _player.FinishedPlaying -= (System.EventHandler<AVFoundation.AVStatusEventArgs>)(System.MulticastDelegate)value; } }
        ~SoundPlayer()
        { _player.Dispose(); }
#elif __ANDROID__
        MediaPlayer _player;
        public static SoundPlayer Create(string FilePath, bool Loop = false, double Volume = 1)
        {
            var Return = new SoundPlayer();
            Return.Init(FilePath, Loop, Volume);
            return Return;
        }
        protected void Init(string FilePath, bool Loop, double Volume)
        {
            _player = MediaPlayer.Create(Forms.Context, Uri.FromFile(new File(FilePath))); 
        }
        public void Play()
        { _player.Start(); }
        public void Pause()
        { _player.Pause(); }
        public void Stop()
        { _player.Stop(); }
        public event System.EventHandler Complete
        { add { _player.Completion += value; } remove { _player.Completion -= value; } }
        ~SoundPlayer()
        { _player.Dispose(); }
#elif NETFX_CORE
        MediaElement _player;
        public static SoundPlayer Create(string FilePath, bool Loop = false, double Volume = 1)
        { var Return = new SoundPlayer();
#pragma warning disable 4014
          /*await*/Return.Init(FilePath, Loop, Volume);
#pragma warning restore 4014
          return Return; }
        protected async Task Init(string FilePath, bool Loop, double Volume)
        {
           // await Current.InstalledLocation.GetFolderAsync(FolderName);
            StorageFile file = await StorageFile.GetFileFromPathAsync(FilePath);
            IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);

            _player = new MediaElement
            {
                IsMuted = false,
                Position = new TimeSpan(0, 0, 0),
                Volume = Volume,
                IsLooping = Loop
            };
            _player.SetSource(stream, file.ContentType);
            
        }
        /*public void Play()
        {
             _player.Play();
        }*/
        public void Play()
        { _player.Play(); }
        public void Pause()
        { _player.Pause(); }
        public void Stop()
        { _player.Stop(); }
        public event EventHandler Complete
        { add { _player.MediaEnded += (global::Windows.UI.Xaml.RoutedEventHandler)(System.MulticastDelegate)value; }
        remove { _player.MediaEnded -= (global::Windows.UI.Xaml.RoutedEventHandler)(System.MulticastDelegate)value; } }
#endif
    }
}
