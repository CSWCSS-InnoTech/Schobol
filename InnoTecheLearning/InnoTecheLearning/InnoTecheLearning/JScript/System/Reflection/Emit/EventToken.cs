namespace System.Reflection.Emit
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential), ComVisible(true)]
    public struct EventToken
    {
        public static readonly EventToken Empty;
        internal int m_event;
        internal EventToken(int str)
        {
            this.m_event = str;
        }

        public int Token =>
            this.m_event;
        public override int GetHashCode() => 
            this.m_event;

        public override bool Equals(object obj) => 
            ((obj is EventToken) && this.Equals((EventToken) obj));

        public bool Equals(EventToken obj) => 
            (obj.m_event == this.m_event);

        public static bool operator ==(EventToken a, EventToken b) => 
            a.Equals(b);

        public static bool operator !=(EventToken a, EventToken b) => 
            !(a == b);

        static EventToken()
        {
            Empty = new EventToken();
        }
    }
}

