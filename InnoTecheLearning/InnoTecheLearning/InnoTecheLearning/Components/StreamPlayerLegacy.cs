using System.Threading.Tasks;
using System.IO;
#if __IOS__
using AVFoundation;
using Foundation;
#elif __ANDROID__
using Android.Net;
using Android.Media;
using Java.IO;
using Xamarin.Forms;
using Stream = System.IO.Stream;
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
    /// Used to be the <see cref="StreamPlayer"/> between 0.10.0a65 to 0.10.0a104
    /// </summary>
    class StreamPlayerLegacy : ISoundPlayer
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
        public static StreamPlayerLegacy Create(Sounds Sound, double Volume = 1)
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
        public static StreamPlayerLegacy Play(Sounds Sound, double Volume = 1)
        {   var Return = Create(Sound, Volume);
            Return.Play();
            return Return;
        }
#if __IOS__
        AVAudioPlayer _player;
        public static StreamPlayerLegacy Create(Stream Content, bool Loop = false, double Volume = 1)
        {
            var Return = new StreamPlayerLegacy();
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
        ~StreamPlayerLegacy()
        { _player.Dispose(); }
#elif __ANDROID__
        public class StreamMediaDataSourceLegacy : MediaDataSource//sorry, but Android 6.0+ only!
        {
            void PlayAudioTrack(byte[] audioBuffer)
            {
                AudioTrack audioTrack = new AudioTrack(
                  // Stream type
                  Android.Media.Stream.Music,
                  // Frequency
                  11025,
                  // Mono or stereo
                  ChannelOut.Mono,
                  // Audio encoding
                  Android.Media.Encoding.Pcm16bit,
                  // Length of the audio clip.
                  audioBuffer.Length,
                  // Mode. Stream or static.
                  AudioTrackMode.Stream);

                audioTrack.Play();
                audioTrack.Write(audioBuffer, 0, audioBuffer.Length);
            }
            Stream data;

            public StreamMediaDataSourceLegacy(Stream Data)
            {
                data = Data;
            }

            public override long Size
            {
                get
                {
                    return data.Length;
                }
            }

            public override int ReadAt(long position, byte[] buffer, int offset, int size)
            {
                data.Seek(position, SeekOrigin.Begin);
                return data.Read(buffer, offset, size);
            }

            public override void Close()
            {
                if (data != null)
                {
                    data.Dispose();
                    data = null;
                }
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                if (data != null)
                {
                    data.Dispose();
                    data = null;
                }
            }
        }
        Android.Media.MediaPlayer _player = new Android.Media.MediaPlayer();
        bool _prepared;
        public static StreamPlayerLegacy Create(Stream Content, bool Loop = false, double Volume = 1)
        {
            var Return = new StreamPlayerLegacy();
            Return.Init(Content, Loop, Volume);
            return Return;
        }
        protected void Init(Stream Content, bool Loop, double Volume)
        {
            _player.Looping = Loop;
            _player.Reset();
            _player.SetDataSource(new StreamMediaDataSourceLegacy(Content));
            _player.Prepare();
            _player.Prepared += (sender, e) => {_prepared = true;};
        }
        public void Play()
        { if (_prepared) _player.Start(); }
        public void Pause()
        { if (_prepared) _player.Pause(); }
        public void Stop()
        {
            if (_player == null)
                return;

            _player.Stop();
            _player.Dispose();
            _player = null;
        }
        public event System.EventHandler Complete
        { add { _player.Completion += value; } remove { _player.Completion -= value; } }
        ~StreamPlayerLegacy()
        { Stop(); }
#elif NETFX_CORE
        MediaElement _player;
        public static StreamPlayerLegacy Create(Stream Content, bool Loop = false, double Volume = 1)
        {
            var Return = new StreamPlayerLegacy();
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
        private StreamPlayerLegacy() : base()
        { }
    }
}
