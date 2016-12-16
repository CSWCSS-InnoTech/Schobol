namespace System.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [ComVisible(true)]
    public sealed class MethodBody
    {
        private ExceptionHandlingClause[] m_exceptionHandlingClauses;
        private byte[] m_IL;
        private bool m_initLocals;
        private int m_localSignatureMetadataToken;
        private LocalVariableInfo[] m_localVariables;
        private int m_maxStackSize;
        internal MethodBase m_methodBase;

        private MethodBody()
        {
        }

        public byte[] GetILAsByteArray() => 
            this.m_IL;

        public IList<ExceptionHandlingClause> ExceptionHandlingClauses =>
            Array.AsReadOnly<ExceptionHandlingClause>(this.m_exceptionHandlingClauses);

        public bool InitLocals =>
            this.m_initLocals;

        public int LocalSignatureMetadataToken =>
            this.m_localSignatureMetadataToken;

        public IList<LocalVariableInfo> LocalVariables =>
            Array.AsReadOnly<LocalVariableInfo>(this.m_localVariables);

        public int MaxStackSize =>
            this.m_maxStackSize;
    }
}

