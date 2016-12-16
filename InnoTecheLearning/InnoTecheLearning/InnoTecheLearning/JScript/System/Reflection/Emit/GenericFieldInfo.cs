namespace System.Reflection.Emit
{
    using System;

    internal class GenericFieldInfo
    {
        internal RuntimeTypeHandle m_context;
        internal RuntimeFieldHandle m_field;

        internal GenericFieldInfo(RuntimeFieldHandle field, RuntimeTypeHandle context)
        {
            this.m_field = field;
            this.m_context = context;
        }
    }
}

