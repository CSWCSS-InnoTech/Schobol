using Exception = System.Exception;
using Math = System.Math;
using NotSupportedException = System.NotSupportedException;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System;
//using Logg = Android.Util.Log;

namespace InnoTecheLearning
{
    partial class Utils
    {

        
        public class ToneGenerator
        {
            public const int SampleRate = 8000;
            double[] sample = null;
            byte[] generatedSnd = null;
            int m_ifreq = 400;
            ValueTask<Unit> m_PlayThread = Unit.CompletedTask;
            bool m_bStop = false;
#if __IOS__
#elif __ANDROID__
#else
            #endif
            StreamPlayer m_audioTrack = null;

            int m_play_length = 1000;//in seconds
            CancellationTokenSource cancellation = new CancellationTokenSource();

            /// <summary>
            /// Plays a tone until stop, provided frequency.
            /// </summary>
            /// <param name="freq">Frequency in Hertz.</param>
            /// <returns></returns>
            public ToneGenerator(int freq = 400) => m_ifreq = freq;
            /// <summary>
            /// Plays a tone, provided frequency and length.
            /// </summary>
            /// <param name="freq">Frequency in Hertz.</param>
            /// <param name="play_length">Play length in seconds.</param>
            /// <returns></returns>
            public ToneGenerator(int freq = 400, int play_length = 1000) : this(freq) => m_play_length = play_length;
            /// <summary>
            /// Plays a tone, provided frequency and length.
            /// </summary>
            /// <param name="freq">Frequency in Hertz.</param>
            /// <param name="play_length">Play length.</param>
            /// <returns></returns>
            public ToneGenerator(int freq, System.TimeSpan play_length) : this(freq) =>
                m_play_length = (int)play_length.TotalSeconds;

            void Stop()
            {
                m_bStop = true;
                if (m_PlayThread != null)
                {
                    try
                    {
                        m_PlayThread.Interrupt();
                        m_PlayThread.Join();
                        m_PlayThread = Unit.CompletedTask;
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

            void Play()
            {
                m_bStop = false;
                m_PlayThread = Unit.InvokeAsync(() => {
                    try
                    {
                        int iToneStep = 0;

                        m_audioTrack = StreamPlayer.Create(new ProducerConsumerStream())

                        while (!m_bStop && m_play_length-- > 0)
                        {
                            GenTone(iToneStep++);

                            m_audioTrack.Write(generatedSnd, 0, generatedSnd.Length);
                            if (iToneStep == 1) m_audioTrack.Play();
                        }
                    }
                    catch (System.OutOfMemoryException)
                    {
                        //Logg.Error("Tone", e.ToString());
                    }
                    catch (Exception)
                    {
                        //Logg.Error("Tone", e.ToString());
                    }

                }, cancellation.Token);
                m_PlayThread.Start();
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

        }

        /*using System.Threading;
        struct Unit { public static Unit Default => new Unit(); }*/
        // This class is safe for 1 producer and 1 consumer.
        public class ProducerConsumerStream : Stream
        {
            private object SyncObject;
            private byte[] CircleBuff;
            private int Head;
            private int Tail;

            public bool IsReadingCompleted { get; private set; }
            public bool IsWritingCompleted { get; private set; }

            // For debugging
            private long TotalBytesRead = 0;
            private long TotalBytesWritten = 0;

            public ProducerConsumerStream(int size = byte.MaxValue)
            {
                CircleBuff = new byte[size + 1 < 1 ? byte.MaxValue : size];
                SyncObject = new object();
                Head = 1;
                Tail = 0;
            }

            /*
            [System.Diagnostics.Conditional("JIM_DEBUG")]
            private void DebugOut(string msg)
            {
                System.Console.WriteLine(msg);
            }

            [System.Diagnostics.Conditional("JIM_DEBUG")]
            private void DebugOut(string fmt, params object[] parms)
            {
                DebugOut(string.Format(fmt, parms));
            }
            */
            private int ReadBytesAvailable
            {
                get
                {
                    if (Head > Tail)
                        return Head - Tail - 1;
                    else
                        return CircleBuff.Length - Tail + Head - 1;
                }
            }

            private int WriteBytesAvailable { get { return CircleBuff.Length - ReadBytesAvailable - 1; } }

            private void IncrementTail()
            {
                Tail = (Tail + 1) % CircleBuff.Length;
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                CheckDisposed();
                if (IsReadingCompleted)
                {
                    throw new EndOfStreamException("The stream is empty and has been marked complete for adding.");
                }
                if (count == 0)
                {
                    return 0;
                }

                lock (SyncObject)
                {
                    //DebugOut("Read: requested {0:N0} bytes. Available = {1:N0}.", count, ReadBytesAvailable);
                    while (ReadBytesAvailable == 0)
                    {
                        if (IsWritingCompleted)
                        {
                            IsReadingCompleted = true;
                            return 0;
                        }
                        Monitor.Wait(SyncObject);
                    }

                    // If Head < Tail, then there are bytes available at the end of the buffer
                    // and also at the front of the buffer.
                    // If reading from Tail to the end doesn't fulfill the request,
                    // and there are still bytes available,
                    // then read from the start of the buffer.
                    //DebugOut("Read: Head={0}, Tail={1}, Avail={2}", Head, Tail, ReadBytesAvailable);

                    IncrementTail();
                    int bytesToRead;
                    if (Tail > Head)
                    {
                        // When Tail > Head, we know that there are at least
                        // (CircleBuff.Length - Tail) bytes available in the buffer.
                        bytesToRead = CircleBuff.Length - Tail;
                    }
                    else
                    {
                        bytesToRead = Head - Tail;
                    }

                    // Don't read more than count bytes!
                    bytesToRead = Math.Min(bytesToRead, count);

                    System.Buffer.BlockCopy(CircleBuff, Tail, buffer, offset, bytesToRead);
                    Tail += (bytesToRead - 1);
                    int bytesRead = bytesToRead;

                    // At this point, either we've exhausted the buffer,
                    // or Tail is at the end of the buffer and has to wrap around.
                    if (bytesRead < count && ReadBytesAvailable > 0)
                    {
                        // We haven't fulfilled the read.
                        IncrementTail();
                        // Tail is always equal to 0 here.
                        bytesToRead = Math.Min((count - bytesRead), (Head - Tail));
                        System.Buffer.BlockCopy(CircleBuff, Tail, buffer, offset + bytesRead, bytesToRead);
                        bytesRead += bytesToRead;
                        Tail += (bytesToRead - 1);
                    }

                    TotalBytesRead += bytesRead;
                    //DebugOut("Read: returning {0:N0} bytes. TotalRead={1:N0}", bytesRead, TotalBytesRead);
                    //DebugOut("Read: Head={0}, Tail={1}, Avail={2}", Head, Tail, ReadBytesAvailable);

                    Monitor.Pulse(SyncObject);
                    return bytesRead;
                }
            }
            
            public void WriteBytes(params byte[] values) => Write(values, 0, values.Length);
            public override void Write(byte[] buffer, int offset = 0, int count = 1)
            {
                CheckDisposed();
                if (IsWritingCompleted)
                {
                    throw new System.InvalidOperationException("The stream has been marked as complete for adding.");
                }
                try
                {
                    lock (SyncObject)
                    {
                        //DebugOut("Write: requested {0:N0} bytes. Available = {1:N0}", count, WriteBytesAvailable);
                        int bytesWritten = 0;
                        while (bytesWritten < count)
                        {
                            if (WriteBytesAvailable == 0) throw new System.OutOfMemoryException("Internal buffer is full.");
                            //while (WriteBytesAvailable == 0) Monitor.Wait(CircleBuff); 
                            //DebugOut("Write: Head={0}, Tail={1}, Avail={2}", Head, Tail, WriteBytesAvailable);
                            int bytesToCopy = Math.Min((count - bytesWritten), WriteBytesAvailable);
                            CopyBytes(buffer, offset + bytesWritten, bytesToCopy);
                            TotalBytesWritten += bytesToCopy;
                            //DebugOut("Write: {0} bytes written. TotalWritten={1:N0}", bytesToCopy, TotalBytesWritten);
                            //DebugOut("Write: Head={0}, Tail={1}, Avail={2}", Head, Tail, WriteBytesAvailable);
                            bytesWritten += bytesToCopy;
                        }
                    }
                }
                finally
                { Monitor.Pulse(SyncObject); }
            }


            private void CopyBytes(byte[] buffer, int srcOffset, int count)
            {
                // Insert at head
                // The copy might require two separate operations.

                // copy as much as can fit between Head and end of the circular buffer
                int offset = srcOffset;
                int bytesCopied = 0;
                int bytesToCopy = Math.Min(CircleBuff.Length - Head, count);
                if (bytesToCopy > 0)
                {
                    System.Buffer.BlockCopy(buffer, offset, CircleBuff, Head, bytesToCopy);
                    bytesCopied = bytesToCopy;
                    Head = (Head + bytesToCopy) % CircleBuff.Length;
                    offset += bytesCopied;
                }

                // Copy the remainder, which will go from the beginning of the buffer.
                if (bytesCopied < count)
                {
                    bytesToCopy = count - bytesCopied;
                    System.Buffer.BlockCopy(buffer, offset, CircleBuff, Head, bytesToCopy);
                    Head = (Head + bytesToCopy) % CircleBuff.Length;
                }
            }

            public void CompleteAdding()
            {
                CheckDisposed();
                lock (SyncObject)
                {
                    //DebugOut("CompleteAdding: {0:N0} bytes written.", TotalBytesWritten);
                    IsWritingCompleted = true;
                    Monitor.Pulse(SyncObject);
                }
            }

            public override bool CanRead => true;

            public override bool CanSeek => false;

            public override bool CanWrite => true;

            public override void Flush() { /* does nothing */ }

            public override long Length => throw new NotSupportedException("Getting length is not supported.");

            public override long Position
            {
                get => throw new NotSupportedException("Positioning is not supported.");
                set => throw new NotSupportedException("Positioning is not supported.");
            }

            public override long Seek(long offset, SeekOrigin origin) =>
                throw new NotSupportedException("Seeking is not supported.");

            public override void SetLength(long value) =>
                throw new NotSupportedException("Setting length is not supported.");

            private bool disposed = false;

            protected override void Dispose(bool disposing)
            {
                if (!disposed)
                {
                    base.Dispose(disposing);
                    disposed = true;
                }
            }

            private Unit CheckDisposed() =>
                disposed ? throw new System.ObjectDisposedException(GetType().Name, "The stream has been disposed.") : Unit.Default;

        }
        
    }
}