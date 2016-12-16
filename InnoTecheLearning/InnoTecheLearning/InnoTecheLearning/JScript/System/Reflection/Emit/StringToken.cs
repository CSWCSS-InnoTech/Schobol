namespace System.Reflection.Emit
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential), ComVisible(true)]
    public struct StringToken
    {
        internal int m_string;
        internal StringToken(int str)
        {
            this.m_string = str;
        }

        public int Token =>
            this.m_string;
        public override int GetHashCode() => 
            this.m_string;

        public override bool Equals(object obj) => 
            ((obj is StringToken) && this.Equals((StringToken) obj));

        public bool Equals(StringToken obj) => 
            (obj.m_string == this.m_string);

        public static bool operator ==(StringToken a, StringToken b) => 
            a.Equals(b);

        public static bool operator !=(StringToken a, StringToken b) => 
            !(a == b);
    }
}

