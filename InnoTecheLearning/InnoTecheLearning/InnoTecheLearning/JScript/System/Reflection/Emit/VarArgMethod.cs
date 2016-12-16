namespace System.Reflection.Emit
{
    using System;
    using System.Reflection;

    internal class VarArgMethod
    {
        internal MethodInfo m_method;
        internal SignatureHelper m_signature;

        internal VarArgMethod(MethodInfo method, SignatureHelper signature)
        {
            this.m_method = method;
            this.m_signature = signature;
        }
    }
}

