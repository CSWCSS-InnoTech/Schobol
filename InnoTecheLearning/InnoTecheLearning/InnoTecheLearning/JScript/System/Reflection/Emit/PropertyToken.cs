namespace System.Reflection.Emit
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential), ComVisible(true)]
    public struct PropertyToken
    {
        public static readonly PropertyToken Empty;
        internal int m_property;
        internal PropertyToken(int str)
        {
            this.m_property = str;
        }

        public int Token =>
            this.m_property;
        public override int GetHashCode() => 
            this.m_property;

        public override bool Equals(object obj) => 
            ((obj is PropertyToken) && this.Equals((PropertyToken) obj));

        public bool Equals(PropertyToken obj) => 
            (obj.m_property == this.m_property);

        public static bool operator ==(PropertyToken a, PropertyToken b) => 
            a.Equals(b);

        public static bool operator !=(PropertyToken a, PropertyToken b) => 
            !(a == b);

        static PropertyToken()
        {
            Empty = new PropertyToken();
        }
    }
}

