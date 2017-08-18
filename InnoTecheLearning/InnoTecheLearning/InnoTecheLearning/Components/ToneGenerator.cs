using System;
using System.Threading;
using System.Threading.Tasks;
    
namespace InnoTecheLearning
{
partial class Utils
    {
        public class ToneGenerator : IDisposable
        {
            public const int Infinite = -1;
            public const int SampleRate = 8000;
            double[] sample = null;
            byte[] generatedSnd = null;
            int m_ifreq = 400;
            ValueTask<Unit> m_PlayThread = Unit.CompletedTask;
            int m_play_length = 1000;//in seconds
            CancellationTokenSource cancellation;
            double m_volume = 1.0;
#if __IOS__
            Foundation.NSMutableData m_memory = new Foundation.NSMutableData(2 * SampleRate);
#elif WINDOWS_UWP
            System.IO.MemoryStream m_memory = null;
#endif
#if __ANDROID__
            Android.Media.AudioTrack
#elif __IOS__
            AVFoundation.AVAudioPlayer
#elif WINDOWS_UWP
            Windows.Media.Playback.MediaPlayer
#endif
            m_audioTrack = null;

            /// <summary>
            /// Creates a tone generator, provided frequency.
            /// </summary>
            /// <param name="Frequency">Frequency in Hertz.</param>
            /// <param name="PlayLength">Play length in seconds.</param>
            /// <returns></returns>
            public ToneGenerator(int Frequency = 400, int PlayLength = 1000, double Volume = 1.0)
            {
                m_ifreq = Frequency;
                m_play_length = PlayLength;
                this.Volume = Volume;
                if(PlayLength == Infinite) cancellation = new CancellationTokenSource();
                else cancellation = new CancellationTokenSource(PlayLength * 1000);
            }

            /// <summary>
            /// Plays a tone, provided frequency.
            /// </summary>
            /// <param name="Frequency">Frequency in Hertz.</param>
            /// <param name="PlayLength">Play length in seconds.</param>
            /// <returns></returns>
            static public ToneGenerator PlayTone(int Frequency = 400, int PlayLength = 1000, double Volume = 1.0)
            {
                ToneGenerator player = new ToneGenerator(Frequency, PlayLength, Volume);
                player.Play();
                return player;
            }

            public double Volume {
                get
                {
                    if (m_audioTrack == null) return m_volume;
#if __IOS__
                    return m_volume = (double)m_audioTrack?.Volume
#elif __ANDROID__
                    return m_volume
#elif WINDOWS_UWP
                    return m_volume = m_audioTrack.Volume
#endif
                        ;
                }
                set
                {
#if __IOS__
                    m_volume = value;
                    if (m_audioTrack != null) m_audioTrack.Volume = (float)m_volume;
#elif __ANDROID__
                    m_volume = value;
                    if (m_audioTrack != null) m_audioTrack.SetVolume((float)(m_volume = value));
#elif WINDOWS_UWP
                    m_volume = value;
                    if (m_audioTrack != null) m_audioTrack.Volume = m_volume;
#endif
                }
            }

            public async void Stop()
            {
                if (m_PlayThread != Unit.CompletedTask)
                {
                    try
                    {
                        cancellation.Cancel();
                        await m_PlayThread;
                    }
                    catch (Exception)
                    {

                    }
                }
                if (m_audioTrack != null)
                {
#if !WINDOWS_UWP
                    m_audioTrack.Stop();
                    m_audioTrack.Dispose();
#else
                    m_audioTrack.Pause();
#endif
                    m_audioTrack = null;
                }
            }

            public void Play()
            {
                m_PlayThread = Unit.InvokeAsync(
                    () =>
                    {
                        try
                        {
                            int iToneStep = 0;

                            m_audioTrack =
#if __ANDROID__
                                new Android.Media.AudioTrack
                                (
                                    Android.Media.Stream.Music,
                                    SampleRate,
                                    Android.Media.ChannelOut.Mono,
                                    Android.Media.Encoding.Pcm16bit,
                                    2 * SampleRate,
                                    Android.Media.AudioTrackMode.Stream
                                )
#elif __IOS__
                            AVFoundation.AVAudioPlayer.FromData(m_memory)
#elif WINDOWS_UWP
                            new Windows.Media.Playback.MediaPlayer
                            {
                                Source = Windows.Media.Core.MediaSource.CreateFromStream(
                                    System.IO.WindowsRuntimeStreamExtensions.AsRandomAccessStream(m_memory), 
                                    "audio/wav")
                            }
#endif
                            ;
#if __ANDROID__
                            m_audioTrack.SetVolume((float)m_volume);
#elif __IOS__
                            m_audioTrack.Volume = (float)m_volume;
#elif WINDOWS_UWP
                            m_audioTrack.Volume = m_volume;
#endif


                            while (!cancellation.IsCancellationRequested && m_play_length-- > 0)
                            {
                                GenTone(iToneStep++);

#if __ANDROID__
                                m_audioTrack.Write(generatedSnd, 0, generatedSnd.Length);
#elif __IOS__
                                m_memory.AppendBytes(generatedSnd);
#elif WINDOWS_UWP
                                m_memory.Write(generatedSnd, 0, generatedSnd.Length);
#endif

                                if (iToneStep == 1)
                                {
                                    m_audioTrack.Play();
                                }
                            }
                        }
                        catch (OutOfMemoryException e)
                        {
                            Log(e, LogImportance.E);
                        }
                        catch (Exception e)
                        {
                            Log(e, LogImportance.W);
                        }

                    }, cancellation.Token
                );
            }

            //Generate tone data for 1 seconds
            void GenTone(int iStep)
            {
                sample = new double[SampleRate];

                for (int i = 0; i < SampleRate; ++i)
                {
                    sample[i] = Math.Sin(2 * Math.PI * (i + iStep * SampleRate) / (SampleRate / m_ifreq));
                }

                // convert to 16 bit pcm sound array
                // assumes the sample buffer is normalised.
                generatedSnd = new byte[2 * SampleRate];
                int idx = 0;
                foreach (double dVal in sample)
                {
                    // scale to maximum amplitude
                    short val = (short)((dVal * 32767));
                    // in 16 bit wav PCM, first byte is the low order byte
                    generatedSnd[idx++] = (byte)(val & 0x00ff);
                    generatedSnd[idx++] = (byte)(unchecked((ushort)(val & 0xff00)) >> 8);
                }
            }

            #region IDisposable Support
            private bool disposedValue = false; // To detect redundant calls
            public bool Disposed => disposedValue;

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects).
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.
#if __IOS__
                    m_audioTrack?.Stop();
                    m_audioTrack?.Dispose();
                    m_memory?.Dispose();
                    m_memory = null;
#elif __ANDROID__
                    m_audioTrack?.Stop();
                    m_audioTrack?.Flush();
                    m_audioTrack?.Release();
                    m_audioTrack?.Dispose();
#elif NETFX_CORE
                    m_audioTrack?.Pause();
                    m_memory?.Flush();
                    m_memory?.Dispose();
                    m_memory = null;
#endif
                    m_audioTrack = null;
                    disposedValue = true;
                }
            }

            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            ~ToneGenerator() {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(false);
            }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                GC.SuppressFinalize(this);
            }
            #endregion

        }
    }
}