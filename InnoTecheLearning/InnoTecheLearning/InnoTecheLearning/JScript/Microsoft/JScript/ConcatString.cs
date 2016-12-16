namespace Microsoft.JScript
{
    using System;
    using System.Text;

    internal sealed class ConcatString : IConvertible
    {
        private StringBuilder buf;
        private bool isOwner;
        private int length;

        internal ConcatString(ConcatString str1, string str2)
        {
            this.length = str1.length + str2.Length;
            if (str1.isOwner)
            {
                this.buf = str1.buf;
                str1.isOwner = false;
            }
            else
            {
                int capacity = this.length * 2;
                if (capacity < 0x100)
                {
                    capacity = 0x100;
                }
                this.buf = new StringBuilder(str1.ToString(), capacity);
            }
            this.buf.Append(str2);
            this.isOwner = true;
        }

        internal ConcatString(string str1, string str2)
        {
            this.length = str1.Length + str2.Length;
            int capacity = this.length * 2;
            if (capacity < 0x100)
            {
                capacity = 0x100;
            }
            this.buf = new StringBuilder(str1, capacity);
            this.buf.Append(str2);
            this.isOwner = true;
        }

        TypeCode IConvertible.GetTypeCode() => 
            TypeCode.String;

        bool IConvertible.ToBoolean(IFormatProvider provider) => 
            this.ToIConvertible().ToBoolean(provider);

        byte IConvertible.ToByte(IFormatProvider provider) => 
            this.ToIConvertible().ToByte(provider);

        char IConvertible.ToChar(IFormatProvider provider) => 
            this.ToIConvertible().ToChar(provider);

        DateTime IConvertible.ToDateTime(IFormatProvider provider) => 
            this.ToIConvertible().ToDateTime(provider);

        decimal IConvertible.ToDecimal(IFormatProvider provider) => 
            this.ToIConvertible().ToDecimal(provider);

        double IConvertible.ToDouble(IFormatProvider provider) => 
            this.ToIConvertible().ToDouble(provider);

        short IConvertible.ToInt16(IFormatProvider provider) => 
            this.ToIConvertible().ToInt16(provider);

        int IConvertible.ToInt32(IFormatProvider provider) => 
            this.ToIConvertible().ToInt32(provider);

        long IConvertible.ToInt64(IFormatProvider provider) => 
            this.ToIConvertible().ToInt64(provider);

        sbyte IConvertible.ToSByte(IFormatProvider provider) => 
            this.ToIConvertible().ToSByte(provider);

        float IConvertible.ToSingle(IFormatProvider provider) => 
            this.ToIConvertible().ToSingle(provider);

        string IConvertible.ToString(IFormatProvider provider) => 
            this.ToIConvertible().ToString(provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => 
            this.ToIConvertible().ToType(conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => 
            this.ToIConvertible().ToUInt16(provider);

        uint IConvertible.ToUInt32(IFormatProvider provider) => 
            this.ToIConvertible().ToUInt32(provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider) => 
            this.ToIConvertible().ToUInt64(provider);

        private IConvertible ToIConvertible() => 
            this.ToString();

        public override string ToString() => 
            this.buf.ToString(0, this.length);
    }
}

