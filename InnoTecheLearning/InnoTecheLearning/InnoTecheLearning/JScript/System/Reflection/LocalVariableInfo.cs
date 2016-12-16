namespace System.Reflection
{
    using System;
    using System.Runtime.InteropServices;

    [ComVisible(true)]
    public class LocalVariableInfo
    {
        private int m_isPinned;
        private int m_localIndex;
        private RuntimeTypeHandle m_typeHandle;

        internal LocalVariableInfo()
        {
        }

        public override string ToString()
        {
            string str = string.Concat(new object[] { this.LocalType.ToString(), " (", this.LocalIndex, ")" });
            if (this.IsPinned)
            {
                str = str + " (pinned)";
            }
            return str;
        }

        public virtual bool IsPinned =>
            (this.m_isPinned != 0);

        public virtual int LocalIndex =>
            this.m_localIndex;

        public virtual Type LocalType =>
            this.m_typeHandle.GetRuntimeType();
    }
}

