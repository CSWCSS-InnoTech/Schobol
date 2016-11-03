using System.Threading.Tasks;
#if __IOS__
using AVFoundation;
using Foundation;
#elif __ANDROID__
using Android.Net;
using Android.Media;
using Java.IO;
using Xamarin.Forms;
#elif NETFX_CORE
using System;
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
        /// <summary>
        /// Plays the sound.
        /// </summary>
        /// <returns>The task to wait because the method is asynchronous.</returns>
        /*public */ Task Play();
        /// <summary>
        /// Pauses the sound.
        /// </summary>
        /// <returns>The task to wait because the method is asynchronous.</returns>
        /*public */ Task Pause();
        /// <summary>
        /// Stops the sound.
        /// </summary>
        /// <returns>The task to wait because the method is asynchronous.</returns>
        /*public */ Task Stop();
    }

    /// <summary>
    /// The platform-specific implementation of <see cref="ISoundPlayer"/>, but cross-platform.
    /// </summary>
    class SoundPlayer : ISoundPlayer
    {
        private SoundPlayer() : base()
        { }
#if __IOS__
        AVAudioPlayer _player;
        public async static Task<SoundPlayer> Create(string FilePath, bool Loop = false, double Volume = 1)
        {
            var Return = new SoundPlayer();
            await Return.Init(FilePath, Loop, Volume);
            return Return;
        }
        protected async Task Init(string FilePath, bool Loop, double Volume)
        {
            await new Task(() => {
            var url = NSUrl.FromString(FilePath);
            _player = AVAudioPlayer.FromUrl(url);
            _player.NumberOfLoops = Loop? 0: -1;
            _player.Volume = System.Convert.ToSingle(Volume);
            //_player.FinishedPlaying += (object sender, AVStatusEventArgs e) => { _player = null; };
            });
        }
        public async Task Play()
        {
            await new Task(() => { _player.Play(); });
        }
        public async Task Pause()
        {
            await new Task(() => { _player.Pause(); });
        }
        public async Task Stop()
        {
            await new Task(() => { _player.Stop(); });
        }
#elif __ANDROID__
        MediaPlayer _player;
        public async static Task<SoundPlayer> Create(string FilePath, bool Loop = false, double Volume = 1)
        {
            var Return = new SoundPlayer();
            await Return.Init(FilePath, Loop, Volume);
            return Return;
        }
        protected async Task Init(string FilePath, bool Loop, double Volume)
        {
            await new Task(() => { _player = MediaPlayer.Create(Forms.Context,
                Uri.FromFile(new File(FilePath))); });
        }
        public async Task Play()
        {
            await new Task(() => { _player.Start(); });
        }
        public async Task Pause()
        {
            await new Task(() => { _player.Pause(); });
        }
        public async Task Stop()
        {
            await new Task(() => { _player.Stop(); });
        }
#elif NETFX_CORE
        MediaElement _mediaElement;
        public async static Task<SoundPlayer> Create(string FilePath, bool Loop = false, double Volume = 1)
        { var Return = new SoundPlayer();
          await Return.Init(FilePath, Loop, Volume);
          return Return; }
        protected async Task Init(string FilePath, bool Loop, double Volume)
        {
           // await Current.InstalledLocation.GetFolderAsync(FolderName);
            StorageFile file = await StorageFile.GetFileFromPathAsync(FilePath);
            IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);

            _mediaElement = new MediaElement
            {
                IsMuted = false,
                Position = new TimeSpan(0, 0, 0),
                Volume = Volume,
                IsLooping = Loop
            };
            _mediaElement.SetSource(stream, file.ContentType);
            
        }
        /*public void Play()
        {
             _mediaElement.Play();
        }*/
        public async Task Play()
        {
            await new Task( () => {_mediaElement.Play();} );
        }
        public async Task Pause()
        {
            await new Task(() => { _mediaElement.Pause(); });
        }
        public async Task Stop()
        {
            await new Task(() => { _mediaElement.Stop(); });
        }
#endif
    }
}
