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
        public enum Sounds : byte
        {
            Violin_G,
            Violin_D,
            Violin_A,
            Violin_E,
            Cello_C,
            Cello_G,
            Cello_D,
            Cello_A
        }
        public static StreamPlayer Create(Sounds Sound, double Volume = 1)
        {
            string Name = "";
            switch (Sound)
            {
                case Sounds.Violin_G:
                    Name = "ViolinG.wav";
                    break;
                case Sounds.Violin_D:
                    Name = "ViolinD.wav";
                    break;
                case Sounds.Violin_A:
                    Name = "ViolinA.wav";
                    break;
                case Sounds.Violin_E:
                    Name = "ViolinE.wav";
                    break;
                case Sounds.Cello_C:
                    Name = "CelloCC.wav";
                    break;
                case Sounds.Cello_G:
                    Name = "CelloGG.wav";
                    break;
                case Sounds.Cello_D:
                    Name = "CelloD.wav";
                    break;
                case Sounds.Cello_A:
                    Name = "CelloA.wav";
                    break;
                default:
                    break;
            }
            return Create(Content: Utils.Resources.GetStream("Sounds." + Name), Loop: true,Volume: Volume);
        }
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
#if true
            _player.Reset();
            _player.SetDataSource(new ByteArrayMediaDataSource(Utils.Temp.ToBytes(Content)));
            _player.Prepare();
#else
            const string FileName = "playertemp";
            Utils.Temp.SaveStream(FileName, Content);

            // resetting _player instance to evade problems
            _player.Reset();

            // In case you run into issues with threading consider new instance like:
            // _player _player = new _player();                     

            // Tried passing path directly, but kept getting 
            // "Prepare failed.: status=0x1"
            // so using file descriptor instead
            FileInputStream fis = new FileInputStream(Utils.Temp.GetFile(FileName));
            _player.SetDataSource(fis.FD);

            _player.Prepare();
            _player.SetOnPreparedListener(new ByteArrayMediaDataSource(new byte[]{ }));
            //_player.Start();
#endif
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
        public class ByteArrayMediaDataSource : Android.Media.MediaDataSource,
            Android.Media.MediaPlayer.IOnPreparedListener
        {

            private readonly byte[] data;

            public ByteArrayMediaDataSource(byte[] data)
            {
                if (data != null)
                    this.data = data;
            }
            public override int ReadAt(long position, byte[] buffer, int offset = 0, int size = 1)
            {
                //data.CopyTo(buffer, offset);
                for (int i = 0; i < size; i++)
                    buffer[offset+i] = data[position+i];
                return size;
            }

            public override long Size
            {
                get { return data.Length; }
            }

            public override void Close()
            {
                // Nothing to do here
            }

            //Implement OnPreparedListener 
            public void OnPrepared(Android.Media.MediaPlayer _player)
            {
                _player.Start();
            }
        }
#elif NETFX_CORE
        MediaElement _player;
        public static StreamPlayer Create(Stream Content, bool Loop = false, double Volume = 1)
        {
            var Return = new StreamPlayer();
//#pragma warning disable 4014
            /*await*/
            Utils.Do(Return.Init(Content, Loop, Volume));
//#pragma warning restore 4014
            return Return;
        }
        protected async Task Init(Stream Content, bool Loop, double Volume)
        {
            const string FileName = "TempContent";
            Utils.Temp.SaveStream(FileName, Content);
            // await Current.InstalledLocation.GetFolderAsync(FolderName);
            StorageFile file = await StorageFile.GetFileFromPathAsync(Utils.Temp.GetFile(FileName));
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
            add { _player.MediaEnded += (global::Windows.UI.Xaml.RoutedEventHandler)(MulticastDelegate)value; }
            remove { _player.MediaEnded -= (global::Windows.UI.Xaml.RoutedEventHandler)(MulticastDelegate)value; }
        }
#endif
        private StreamPlayer() : base()
        { }
    }
}
