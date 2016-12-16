namespace System.IO
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, ComVisible(true)]
    public class MemoryStream : Stream
    {
        private byte[] _buffer;
        private int _capacity;
        private bool _expandable;
        private bool _exposable;
        private bool _isOpen;
        private int _length;
        private int _origin;
        private int _position;
        private bool _writable;
        private const int MemStreamMaxLength = 0x7fffffff;

        public MemoryStream() : this(0)
        {
        }

        public MemoryStream(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException("capacity", Environment.GetResourceString("ArgumentOutOfRange_NegativeCapacity"));
            }
            this._buffer = new byte[capacity];
            this._capacity = capacity;
            this._expandable = true;
            this._writable = true;
            this._exposable = true;
            this._origin = 0;
            this._isOpen = true;
        }

        public MemoryStream(byte[] buffer) : this(buffer, true)
        {
        }

        public MemoryStream(byte[] buffer, bool writable)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
            }
            this._buffer = buffer;
            this._length = this._capacity = buffer.Length;
            this._writable = writable;
            this._exposable = false;
            this._origin = 0;
            this._isOpen = true;
        }

        public MemoryStream(byte[] buffer, int index, int count) : this(buffer, index, count, true, false)
        {
        }

        public MemoryStream(byte[] buffer, int index, int count, bool writable) : this(buffer, index, count, writable, false)
        {
        }

        public MemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if ((buffer.Length - index) < count)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
            }
            this._buffer = buffer;
            this._origin = this._position = index;
            this._length = this._capacity = index + count;
            this._writable = writable;
            this._exposable = publiclyVisible;
            this._expandable = false;
            this._isOpen = true;
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    this._isOpen = false;
                    this._writable = false;
                    this._expandable = false;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private bool EnsureCapacity(int value)
        {
            if (value < 0)
            {
                throw new IOException(Environment.GetResourceString("IO.IO_StreamTooLong"));
            }
            if (value <= this._capacity)
            {
                return false;
            }
            int num = value;
            if (num < 0x100)
            {
                num = 0x100;
            }
            if (num < (this._capacity * 2))
            {
                num = this._capacity * 2;
            }
            this.Capacity = num;
            return true;
        }

        public override void Flush()
        {
        }

        public virtual byte[] GetBuffer()
        {
            if (!this._exposable)
            {
                throw new UnauthorizedAccessException(Environment.GetResourceString("UnauthorizedAccess_MemStreamBuffer"));
            }
            return this._buffer;
        }

        internal int InternalEmulateRead(int count)
        {
            if (!this._isOpen)
            {
                __Error.StreamIsClosed();
            }
            int num = this._length - this._position;
            if (num > count)
            {
                num = count;
            }
            if (num < 0)
            {
                num = 0;
            }
            this._position += num;
            return num;
        }

        internal byte[] InternalGetBuffer() => 
            this._buffer;

        internal void InternalGetOriginAndLength(out int origin, out int length)
        {
            if (!this._isOpen)
            {
                __Error.StreamIsClosed();
            }
            origin = this._origin;
            length = this._length;
        }

        internal int InternalGetPosition()
        {
            if (!this._isOpen)
            {
                __Error.StreamIsClosed();
            }
            return this._position;
        }

        internal int InternalReadInt32()
        {
            if (!this._isOpen)
            {
                __Error.StreamIsClosed();
            }
            int num = this._position += 4;
            if (num > this._length)
            {
                this._position = this._length;
                __Error.EndOfFile();
            }
            return (((this._buffer[num - 4] | (this._buffer[num - 3] << 8)) | (this._buffer[num - 2] << 0x10)) | (this._buffer[num - 1] << 0x18));
        }

        public override int Read([In, Out] byte[] buffer, int offset, int count)
        {
            if (!this._isOpen)
            {
                __Error.StreamIsClosed();
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if ((buffer.Length - offset) < count)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
            }
            int num = this._length - this._position;
            if (num > count)
            {
                num = count;
            }
            if (num <= 0)
            {
                return 0;
            }
            if (num <= 8)
            {
                int num2 = num;
                while (--num2 >= 0)
                {
                    buffer[offset + num2] = this._buffer[this._position + num2];
                }
            }
            else
            {
                Buffer.InternalBlockCopy(this._buffer, this._position, buffer, offset, num);
            }
            this._position += num;
            return num;
        }

        public override int ReadByte()
        {
            if (!this._isOpen)
            {
                __Error.StreamIsClosed();
            }
            if (this._position >= this._length)
            {
                return -1;
            }
            return this._buffer[this._position++];
        }

        public override long Seek(long offset, SeekOrigin loc)
        {
            if (!this._isOpen)
            {
                __Error.StreamIsClosed();
            }
            if (offset > 0x7fffffffL)
            {
                throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_MemStreamLength"));
            }
            switch (loc)
            {
                case SeekOrigin.Begin:
                    if (offset < 0L)
                    {
                        throw new IOException(Environment.GetResourceString("IO.IO_SeekBeforeBegin"));
                    }
                    this._position = this._origin + ((int) offset);
                    break;

                case SeekOrigin.Current:
                    if ((offset + this._position) < this._origin)
                    {
                        throw new IOException(Environment.GetResourceString("IO.IO_SeekBeforeBegin"));
                    }
                    this._position += (int) offset;
                    break;

                case SeekOrigin.End:
                    if ((this._length + offset) < this._origin)
                    {
                        throw new IOException(Environment.GetResourceString("IO.IO_SeekBeforeBegin"));
                    }
                    this._position = this._length + ((int) offset);
                    break;

                default:
                    throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSeekOrigin"));
            }
            return (long) this._position;
        }

        public override void SetLength(long value)
        {
            if (!this._writable)
            {
                __Error.WriteNotSupported();
            }
            if (value > 0x7fffffffL)
            {
                throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_MemStreamLength"));
            }
            if ((value < 0L) || (value > (0x7fffffff - this._origin)))
            {
                throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_MemStreamLength"));
            }
            int num = this._origin + ((int) value);
            if (!this.EnsureCapacity(num) && (num > this._length))
            {
                Array.Clear(this._buffer, this._length, num - this._length);
            }
            this._length = num;
            if (this._position > num)
            {
                this._position = num;
            }
        }

        public virtual byte[] ToArray()
        {
            byte[] dst = new byte[this._length - this._origin];
            Buffer.InternalBlockCopy(this._buffer, this._origin, dst, 0, this._length - this._origin);
            return dst;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!this._isOpen)
            {
                __Error.StreamIsClosed();
            }
            if (!this._writable)
            {
                __Error.WriteNotSupported();
            }
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer", Environment.GetResourceString("ArgumentNull_Buffer"));
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
            }
            if ((buffer.Length - offset) < count)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
            }
            int num = this._position + count;
            if (num < 0)
            {
                throw new IOException(Environment.GetResourceString("IO.IO_StreamTooLong"));
            }
            if (num > this._length)
            {
                bool flag = this._position > this._length;
                if ((num > this._capacity) && this.EnsureCapacity(num))
                {
                    flag = false;
                }
                if (flag)
                {
                    Array.Clear(this._buffer, this._length, num - this._length);
                }
                this._length = num;
            }
            if (count <= 8)
            {
                int num2 = count;
                while (--num2 >= 0)
                {
                    this._buffer[this._position + num2] = buffer[offset + num2];
                }
            }
            else
            {
                Buffer.InternalBlockCopy(buffer, offset, this._buffer, this._position, count);
            }
            this._position = num;
        }

        public override void WriteByte(byte value)
        {
            if (!this._isOpen)
            {
                __Error.StreamIsClosed();
            }
            if (!this._writable)
            {
                __Error.WriteNotSupported();
            }
            if (this._position >= this._length)
            {
                int num = this._position + 1;
                bool flag = this._position > this._length;
                if ((num >= this._capacity) && this.EnsureCapacity(num))
                {
                    flag = false;
                }
                if (flag)
                {
                    Array.Clear(this._buffer, this._length, this._position - this._length);
                }
                this._length = num;
            }
            this._buffer[this._position++] = value;
        }

        public virtual void WriteTo(Stream stream)
        {
            if (!this._isOpen)
            {
                __Error.StreamIsClosed();
            }
            if (stream == null)
            {
                throw new ArgumentNullException("stream", Environment.GetResourceString("ArgumentNull_Stream"));
            }
            stream.Write(this._buffer, this._origin, this._length - this._origin);
        }

        public override bool CanRead =>
            this._isOpen;

        public override bool CanSeek =>
            this._isOpen;

        public override bool CanWrite =>
            this._writable;

        public virtual int Capacity
        {
            get
            {
                if (!this._isOpen)
                {
                    __Error.StreamIsClosed();
                }
                return (this._capacity - this._origin);
            }
            set
            {
                if (!this._isOpen)
                {
                    __Error.StreamIsClosed();
                }
                if (value != this._capacity)
                {
                    if (!this._expandable)
                    {
                        __Error.MemoryStreamNotExpandable();
                    }
                    if (value < this._length)
                    {
                        throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_SmallCapacity"));
                    }
                    if (value > 0)
                    {
                        byte[] dst = new byte[value];
                        if (this._length > 0)
                        {
                            Buffer.InternalBlockCopy(this._buffer, 0, dst, 0, this._length);
                        }
                        this._buffer = dst;
                    }
                    else
                    {
                        this._buffer = null;
                    }
                    this._capacity = value;
                }
            }
        }

        public override long Length
        {
            get
            {
                if (!this._isOpen)
                {
                    __Error.StreamIsClosed();
                }
                return (long) (this._length - this._origin);
            }
        }

        public override long Position
        {
            get
            {
                if (!this._isOpen)
                {
                    __Error.StreamIsClosed();
                }
                return (long) (this._position - this._origin);
            }
            set
            {
                if (!this._isOpen)
                {
                    __Error.StreamIsClosed();
                }
                if (value < 0L)
                {
                    throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
                }
                if (value > 0x7fffffffL)
                {
                    throw new ArgumentOutOfRangeException("value", Environment.GetResourceString("ArgumentOutOfRange_MemStreamLength"));
                }
                this._position = this._origin + ((int) value);
            }
        }
    }
}

