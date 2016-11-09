using System.Threading.Tasks;
using System.IO;
#if __IOS__
using AVFoundation;
using Foundation;
#elif __ANDROID__
using Android.Net;
//using Android.Media;
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
{
    /// <summary>
    /// A <see cref="StreamPlayer"/> that plays streams.
    /// </summary>
    class StreamPlayer : ISoundPlayer
    {
        private StreamPlayer() : base()
        { }
#if __IOS__
        AVAudioPlayer _player;
        public static StreamPlayer Create(Stream Content, bool Loop = false, double Volume = 1)
        {
            var Return = new StreamPlayer();
            Return.Init(Content, Loop, Volume);
            return Return;
        }
        protected void Init(Stream Content, bool Loop, double Volume)
        {
            _player = AVAudioPlayer.FromData(NSData.FromStream(Content));
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
        { add { _player.FinishedPlaying += (System.EventHandler<AVStatusEventArgs>)(System.MulticastDelegate)value; }
        remove { _player.FinishedPlaying -= (System.EventHandler<AVStatusEventArgs>)(System.MulticastDelegate)value; } }
        ~StreamPlayer()
        { _player.Dispose(); }
#elif __ANDROID__
        Android.Media.MediaPlayer _player = new Android.Media.MediaPlayer();
        public static StreamPlayer Create(Stream Content, bool Loop = false, double Volume = 1)
        {
            var Return = new StreamPlayer();
            Return.Init(Content, Loop, Volume);
            return Return;
        }
        protected void Init(Stream Content, bool Loop, double Volume)
        {
            Utils.Temp.SaveStream("")

            // resetting _player instance to evade problems
            _player.Reset();

            // In case you run into issues with threading consider new instance like:
            // _player _player = new _player();                     

            // Tried passing path directly, but kept getting 
            // "Prepare failed.: status=0x1"
            // so using file descriptor instead
            FileInputStream fis = new FileInputStream(tempMp3);
            _player.SetDataSource(fis.FD);

            _player.Prepare();
            _player.Start();
        }
        public void Play()
        { _player.Start(); }
        public void Pause()
        { _player.Pause(); }
        public void Stop()
        { _player.Stop(); }
        public event System.EventHandler Complete
        { add { _player.Completion += value; } remove { _player.Completion -= value; } }
        ~StreamPlayer()
        { _player.Dispose(); }
        public class ByteArrayMediaDataSource : MediaDataSource
        {

    private readonly byte[] data;

        public ByteArrayMediaDataSource(byte[] data)
        {
            if( data != null)
            this.data = data;
        }
    public int readAt(long position, byte[] buffer, int offset, int size)
        {
            Copy(data, (int)position, buffer, offset, size);
        return size;
    }
    
    public long getSize()
    {
        return data.length;
    }
    
    public override void close()
    {
        // Nothing to do here
    }
}
#elif NETFX_CORE
        MediaElement _player;
        public static StreamPlayer Create(string FilePath, bool Loop = false, double Volume = 1)
        {
            var Return = new StreamPlayer();
#pragma warning disable 4014
            /*await*/
            Return.Init(FilePath, Loop, Volume);
#pragma warning restore 4014
            return Return;
        }
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
        {
            add { _player.MediaEnded += (global::Windows.UI.Xaml.RoutedEventHandler)(System.MulticastDelegate)value; }
            remove { _player.MediaEnded -= (global::Windows.UI.Xaml.RoutedEventHandler)(System.MulticastDelegate)value; }
        }
#endif
    }
}
