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
using System.Runtime.InteropServices;
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
        public static StreamPlayer Create(Sounds Sound, float Volume = 1)
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
            return Create(Content: Utils.Resources.GetStream("Sounds." + Name), Loop: true, Volume: Volume);
        }
        public static StreamPlayer Play(Sounds Sound, float Volume = 1)
        {
            var Return = Create(Sound, Volume);
            Return.Play();
            return Return;
        }
#if __IOS__
        AVAudioPlayer _player;
        public static StreamPlayer Create(Stream Content, bool Loop = false, float Volume = 1)
        {
            var Return = new StreamPlayer();
            Return.Init(Content, Loop, Volume);
            return Return;
        }
        protected void Init(Stream Content, bool Loop, float Volume)
        {
            _player = AVAudioPlayer.FromData(NSData.FromStream(Content));
            _player.NumberOfLoops = Loop? 0: -1;
            _player.Volume = Volume;
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
        public float Volume { get { return _player.Volume; } set { _player.Volume = value; } }
        ~StreamPlayer()
        { _player.Dispose(); }
#elif __ANDROID__
        AudioTrack _player;
        Stream _content;
        bool _prepared;
        bool _loop;
        float _volume;
        public static StreamPlayer Create(Stream Content, bool Loop = false, float Volume = 1)
        {
            var Return = new StreamPlayer();
            Return.Init(Content, Loop, Volume);
            return Return;
        }
        protected void Init(Stream Content, bool Loop, float Volume)
        {
            _content = Content;
            _player = new AudioTrack(
            // Stream type
            Android.Media.Stream.Music,
            // Frequency
            11025,
            // Mono or stereo
            ChannelOut.Mono,
            // Audio encoding
            Android.Media.Encoding.Pcm16bit,
            // Length of the audio clip.
            (int)Content.Length,
            // Mode. Stream or static.
            AudioTrackMode.Stream);
            _loop = Loop;
            _volume = Volume;
            _player.SetVolume(_volume = Volume);
            _player.SetNotificationMarkerPosition((int)Content.Length / 2);
            _prepared = true;
        }
        public void Play()
        {
            if (!_prepared) return;
            _player.Flush();
            _player.Play();
            _player.Write(_content.ReadFully(), 0, (int)_content.Length);
        }
        public void Pause()
        { if (_prepared) _player.Pause(); }
        public void Stop()
        {
            if (_player == null)
                return;

            _player.Stop();
            _player.Dispose();
            _player = null;
            _prepared = false;
        }
        public event System.EventHandler Complete
        {
            add
            {
                _player.MarkerReached += MarkerReachedEventHandler(value);
            }
            remove
            {
                _player.MarkerReached -= MarkerReachedEventHandler(value);
            }
        }
        protected System.EventHandler<AudioTrack.MarkerReachedEventArgs>
            MarkerReachedEventHandler(System.EventHandler value)
        {
            return (object sender, AudioTrack.MarkerReachedEventArgs e) =>
               {
                   value(sender, e);
                   if (_loop) e.Track.SetPlaybackHeadPosition(0);
               };
        }
        public float Volume { get { return _volume; } set { _player.SetVolume(_volume = value); } }
        ~StreamPlayer()
        { Stop(); }
#elif NETFX_CORE
        MediaElement _player;
        public static StreamPlayer Create(Stream Content, bool Loop = false, float Volume = 1)
        {
            var Return = new StreamPlayer();
            Return.Init(Content, Loop, Volume);
            return Return;
        }
        protected void Init(Stream Content, bool Loop, float Volume)
        {
            _player = new MediaElement
            {
                IsMuted = false,
                Position = new TimeSpan(0, 0, 0),
                Volume = Volume,
                IsLooping = Loop
            };
            _player.SetSource(Content.AsRandomAccessStream(), GetMime(Content.ReadFully()));
        }
        public void Play()
        { _player.Play(); }
        public void Pause()
        { _player.Pause(); }
        public void Stop()
        { _player.Stop(); }
        public float Volume { get { return (float)_player.Volume; } set { _player.Volume = value; } }
        public event EventHandler Complete
        {
            add { _player.MediaEnded += (global::Windows.UI.Xaml.RoutedEventHandler)(MulticastDelegate)value; }
            remove { _player.MediaEnded -= (global::Windows.UI.Xaml.RoutedEventHandler)(MulticastDelegate)value; }
        }
        [DllImport(@"urlmon.dll")]
        private extern static uint FindMimeFromData(uint pBC, [MarshalAs(UnmanagedType.LPStr)] string pwzUrl,
                                                    [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer, uint cbSize,
                                                    [MarshalAs(UnmanagedType.LPStr)] string pwzMimeProposed,
                                                    uint dwMimeFlags, out uint ppwzMimeOut, uint dwReserverd);
        public static string GetMime(byte[] buffer)
        {
            try
            {
                uint mimetype;
                FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
                IntPtr mimeTypePtr = new IntPtr(mimetype);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch (Exception)
            {
                return "unknown/unknown";
            }
        }
#endif
        private StreamPlayer() : base()
        { }
    }
}
