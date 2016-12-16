namespace System.Reflection.Emit
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential), ComVisible(true)]
    public struct TypeToken
    {
        public static readonly TypeToken Empty;
        internal int m_class;
        internal TypeToken(int str)
        {
            this.m_class = str;
        }

        public int Token =>
            this.m_class;
        public override int GetHashCode() => 
            this.m_class;

        public override bool Equals(object obj) => 
            ((obj is TypeToken) && this.Equals((TypeToken) obj));

        public bool Equals(TypeToken obj) => 
            (obj.m_class == this.m_class);

        public static bool operator ==(TypeToken a, TypeToken b) => 
            a.Equals(b);

        public static bool operator !=(TypeToken a, TypeToken b) => 
            !(a == b);

        static TypeToken()
        {
            Empty = new TypeToken();
        }
    }
}

