namespace Microsoft.JScript
{
    using System;
    using System.IO;
    using System.Text;

    public class COMCharStream : Stream
    {
        private StringBuilder buffer;
        private IMessageReceiver messageReceiver;

        public COMCharStream(IMessageReceiver messageReceiver)
        {
            this.messageReceiver = messageReceiver;
            this.buffer = new StringBuilder(0x80);
        }

        public override void Close()
        {
            this.Flush();
        }

        public override void Flush()
        {
            this.messageReceiver.Message(this.buffer.ToString());
            this.buffer = new StringBuilder(0x80);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override long Seek(long offset, SeekOrigin origin) => 
            0L;

        public override void SetLength(long value)
        {
            this.buffer.Length = (int) value;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            for (int i = count; i > 0; i--)
            {
                this.buffer.Append((char) buffer[offset++]);
            }
        }

        public override bool CanRead =>
            false;

        public override bool CanSeek =>
            false;

        public override bool CanWrite =>
            true;

        public override long Length =>
            ((long) this.buffer.Length);

        public override long Position
        {
            get => 
                ((long) this.buffer.Length);
            set
            {
            }
        }
    }
}

