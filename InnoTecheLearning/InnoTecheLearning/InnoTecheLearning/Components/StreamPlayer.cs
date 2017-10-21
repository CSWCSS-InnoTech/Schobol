using System;
using System.Threading.Tasks;
#if __IOS__
using AVFoundation;
using Foundation;
#elif __ANDROID__
using Android.Media;
using Xamarin.Forms;
#elif NETFX_CORE
using static System.IO.WindowsRuntimeStreamExtensions;
using Windows.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Streams;
#endif

namespace InnoTecheLearnUtilities
{
    public partial class Utils
    {
        /// <summary>
        /// A <see cref="StreamPlayer"/> that plays streams.
        /// </summary>
        public class StreamPlayer : ISoundPlayer, IDisposable
        {
            private StreamPlayer() : base() { }

            #region IDisposable Support
            public bool Disposed { get; private set; } = false; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (!Disposed)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects).
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.
#if __IOS__
                    _player?.Stop();
                    _player?.Dispose();
                    _player = null;
#elif __ANDROID__
                    _prepared = false;
                    _player.Flush();
                    _player.Release();
                    _player.Dispose();
                    _content = null;
#elif __ANDROID__
                    for (int i = 0; i < mp.Length; i++)
                    {
                        mp[i]?.Stop();
                        mp[i]?.Release();
                        mp[i]?.Dispose();
                    }
                    mp = null;
#elif NETFX_CORE
                    _player?.Stop();
                    _player?.ClearValue(MediaElement.SourceProperty);
                    _player = null;
#endif
                    Disposed = true;
                }
            }

            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            ~StreamPlayer()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(false);
            }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                System.GC.SuppressFinalize(this);
            }
            #endregion
            public static StreamPlayer Create(Sounds Sound, bool Loop = false, float Volume = 1) =>
                Create(new PcmWavStream(Sound, Loop, Volume));
            public static StreamPlayer Play(Sounds Sound, bool Loop = false, float Volume = 1) =>
                Play(new PcmWavStream(Sound, Loop, Volume));
            public static StreamPlayer Play(MusicStream Stream)
            {
                var Return = Create(Stream);
                Return.Play();
                return Return;
            }
#if __IOS__
            public static StreamPlayer Create(MusicStream Wave)
            {
                var Return = new StreamPlayer();
                Return.Init(Wave);
                return Return;
            }
            protected void Init(MusicStream Wave)
            {
                _player = AVAudioPlayer.FromData(NSData.FromStream(Wave.Content));
                _player.NumberOfLoops = Wave.Loop ? 0 : -1;
                _player.Volume = Wave.Volume;
            }
            AVAudioPlayer _player;
            public void Play()
            { _player.Play(); }
            public void Pause()
            { _player.Pause(); }
            public void Stop()
            { _player.Stop(); }
            public event System.EventHandler Complete
            {
                add { _player.FinishedPlaying += (System.EventHandler<AVStatusEventArgs>)(System.MulticastDelegate)value; }
                remove { _player.FinishedPlaying -= (System.EventHandler<AVStatusEventArgs>)(System.MulticastDelegate)value; }
            }
            public float Volume { get { return _player.Volume; } set { _player.Volume = value; } }
            public bool Loop { get { return _player.NumberOfLoops == -1; } set { _player.NumberOfLoops = value ? -1 : 0; } }
#elif __ANDROID__
            AudioTrack _player;
            public bool _prepared { get; private set; }
            bool _loop;
            float _volume;
            int _frames, _buffersize;
            AudioTrackMode _mode;
            TimeSpan _duration;
            byte[] _content;
            protected MusicStream _Wave { get; private set; }
            public static StreamPlayer Create(MusicStream Wave)
            {
                return new StreamPlayer { _Wave = Wave };
            }
            protected void Init(MusicStream Wave)
            {
                _mode = (AudioTrackMode)Wave.Mode;
                if (_mode == AudioTrackMode.Static)
                    _player = new AudioTrack(
                    // Stream type
                    (Android.Media.Stream)Wave.Type,
                    // Frequency
                    Wave.SampleRate,
                    // Mono or stereo
                    (ChannelOut)Wave.Config,
                    // Audio encoding
                    (Encoding)Wave.Format,
                    // Length of the audio clip.
                    (int)Wave.SizeInBytes,
                    // Mode. Stream or static.
                    AudioTrackMode.Static);
                else _player = new AudioTrack(
                // Stream type
                (Android.Media.Stream)Wave.Type,
                // Frequency
                Wave.SampleRate,
                // Mono or stereo
                (ChannelOut)Wave.Config,
                // Audio encoding
                (Encoding)Wave.Format,
                // Length of the audio clip.
                _buffersize = AudioTrack.GetMinBufferSize(Wave.SampleRate,
                    (ChannelOut)Wave.Config, (Encoding)Wave.Format),
                // Mode. Stream or static.
                AudioTrackMode.Stream);
                _duration = Wave.Duration;
                _loop = Wave.Loop;
                _frames = Wave.Samples;
                _player.SetVolume(_volume = Wave.Volume);
                _player.SetNotificationMarkerPosition(_frames * 31 / 32);
                if (_mode == AudioTrackMode.Static)
                    _player.Write(Wave.Content.ReadFully(true), 0, (int)Wave.Content.Length);
                else
                {
                    Set((sender, e) => { if (_loop) { _player.Release(); Init(_Wave); Write(); }; });
                    _content = Wave.Content.ReadFully(true);
                }
                _prepared = true;
            }
            Task _play;
            protected virtual void Write()
            {
                _play = Task.Run(() => { 
                for (int i = 0; i <= _content.Length; i += _buffersize)
                {
                    _player.Write(_content, i, _buffersize);
                } });
            }
            public void Play()
            {
                Stop();
                Init(_Wave);
                if (_mode == AudioTrackMode.Static)
                {
                    if (_loop) _player.SetLoopPoints(0, _frames, -1);
                }
                else
                {
                    Write();
                    Device.StartTimer(_duration,
                        () => { Complete?.Invoke(this, EventArgs.Empty); return !Disposed && _loop; });
                }
                _player.Play();
            } 
            public void Pause()
            { if (_prepared) _player.Pause(); }
            public void Stop()
            {
                if (_player == null)
                    return;
                if (_loop) _player.SetLoopPoints(0, 0, 0);
                _player.Stop();
                _player.Release();
                _player = null;
            }
            bool _Set;
            void Set(EventHandler Handler)
            { if (_Set) return; Complete += Handler; _Set = true; }
            public event EventHandler Complete;
            public float Volume { get { return _volume; } set { _player?.SetVolume(_volume = value); } }
            public bool Loop { get { return _loop; } set { _loop = value;
                    if (_mode == AudioTrackMode.Static && _prepared && !value) _player.SetLoopPoints(0, 0, 0); } }
#elif NETFX_CORE
            MediaElement _player;
            public static StreamPlayer Create(MusicStream Wave)
            {
                var Return = new StreamPlayer();
                Return.Init(Wave);
                return Return;
            }
            protected void Init(MusicStream Wave)
            {
                _player = new MediaElement
                {
                    IsMuted = false,
                    Position = new TimeSpan(0, 0, 0),
                    Volume = Wave.Volume,
                    IsLooping = Wave.Loop,
                };
                _player.SetSource(Wave.Content.AsRandomAccessStream(), Wave.MimeType);
            }
            public void Play()
            { _player?.Play(); }
            public void Pause()
            { _player?.Pause(); }
            public void Stop()
            { _player?.Stop(); }
            public float Volume { get { return (float)_player.Volume; } set { _player.Volume = value; } }
            public event EventHandler Complete
            {
                add { _player.MediaEnded += (global::Windows.UI.Xaml.RoutedEventHandler)(MulticastDelegate)value; }
                remove { _player.MediaEnded -= (global::Windows.UI.Xaml.RoutedEventHandler)(MulticastDelegate)value; }
            }
            public bool Loop { get { return _player.IsLooping; } set { _player.IsLooping = value; } }
#endif
    }
}
}