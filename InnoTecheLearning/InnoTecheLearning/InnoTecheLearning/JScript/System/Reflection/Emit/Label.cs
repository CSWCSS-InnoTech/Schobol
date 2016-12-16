namespace System.Reflection.Emit
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential), ComVisible(true)]
    public struct Label
    {
        internal int m_label;
        internal Label(int label)
        {
            this.m_label = label;
        }

        internal int GetLabelValue() => 
            this.m_label;

        public override int GetHashCode() => 
            this.m_label;

        public override bool Equals(object obj) => 
            ((obj is Label) && this.Equals((Label) obj));

        public bool Equals(Label obj) => 
            (obj.m_label == this.m_label);

        public static bool operator ==(Label a, Label b) => 
            a.Equals(b);

        public static bool operator !=(Label a, Label b) => 
            !(a == b);
    }
}

