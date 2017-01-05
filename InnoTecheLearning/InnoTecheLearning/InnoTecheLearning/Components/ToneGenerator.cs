#if __ANDROID__
using Exception = System.Exception;
using System.Collections.Generic;
using Java.Lang;
using Android.Media;
using Logg = Android.Util.Log;

namespace InnoTecheLearning
{
    partial class Utils
    {
        class ToneGenerator
        {
            public static int sampleRate = 8000;
            double[] sample = null;
            byte[] generatedSnd = null;
            int m_ifreq = 400;
            Thread m_PlayThread = null;
            bool m_bStop = false;
            AudioTrack m_audioTrack = null;
            int m_play_length = 1000;//in seconds

            /// <summary>
            /// Plays a tone, provided frequency.
            /// </summary>
            /// <param name="freq">Frequency in Hertz.</param>
            /// <param name="play_length">Play length in seconds.</param>
            /// <returns></returns>
            static public ToneGenerator PlayTone(int freq = 400, int play_length = 1000)
            {
                ToneGenerator player = new ToneGenerator();
                player.m_ifreq = freq;
                player.m_play_length = play_length;
                player.play();
                return player;
            }

            void stop()
            {
                m_bStop = true;
                if (m_PlayThread != null)
                {
                    try
                    {
                        m_PlayThread.Interrupt();
                        m_PlayThread.Join();
                        m_PlayThread = null;
                    }
                    catch (Exception)
                    {

                    }
                }
                if (m_audioTrack != null)
                {
                    m_audioTrack.Stop();
                    m_audioTrack.Release();
                    m_audioTrack = null;
                }
            }

            void play()
            {
                m_bStop = false;
                m_PlayThread = new Thread(
            () =>
            {
                try
                {
                    int iToneStep = 0;

                    m_audioTrack = new AudioTrack(Stream.Music,
                            sampleRate, ChannelOut.Mono,
                            Encoding.Pcm16bit, 2 * sampleRate,
                            AudioTrackMode.Stream);

                    while (!m_bStop && m_play_length-- > 0)
                    {
                        genTone(iToneStep++);

                        m_audioTrack.Write(generatedSnd, 0, generatedSnd.Length);
                        if (iToneStep == 1)
                        {
                            m_audioTrack.Play();
                        }
                    }
                }
                catch (OutOfMemoryError e)
                {
                    Logg.Error("Tone", e.ToString());
                }
                catch (Exception e)
                {
                    Logg.Error("Tone", e.ToString());
                }

            }
        );
                m_PlayThread.Start();
            }

            //Generate tone data for 1 seconds
            void genTone(int iStep)
            {
                sample = new double[sampleRate];

                for (int i = 0; i < sampleRate; ++i)
                {
                    sample[i] = Math.Sin(2 * Math.Pi * (i + iStep * sampleRate) / (sampleRate / m_ifreq));
                }

                // convert to 16 bit pcm sound array
                // assumes the sample buffer is normalised.
                generatedSnd = new byte[2 * sampleRate];
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

        }
    }
}
#endif