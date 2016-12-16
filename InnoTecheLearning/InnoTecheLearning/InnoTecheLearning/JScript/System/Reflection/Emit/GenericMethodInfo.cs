namespace System.Reflection.Emit
{
    using System;

    internal class GenericMethodInfo
    {
        internal RuntimeTypeHandle m_context;
        internal RuntimeMethodHandle m_method;

        internal GenericMethodInfo(RuntimeMethodHandle method, RuntimeTypeHandle context)
        {
            this.m_method = method;
            this.m_context = context;
        }
    }
}

