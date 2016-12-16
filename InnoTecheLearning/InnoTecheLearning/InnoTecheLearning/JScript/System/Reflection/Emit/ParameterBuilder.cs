namespace System.Reflection.Emit
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [ComDefaultInterface(typeof(_ParameterBuilder)), ComVisible(true), ClassInterface(ClassInterfaceType.None)]
    public class ParameterBuilder : _ParameterBuilder
    {
        private ParameterAttributes m_attributes;
        private int m_iPosition;
        private MethodBuilder m_methodBuilder;
        private ParameterToken m_pdToken;
        private string m_strParamName;

        private ParameterBuilder()
        {
        }

        internal ParameterBuilder(MethodBuilder methodBuilder, int sequence, ParameterAttributes attributes, string strParamName)
        {
            this.m_iPosition = sequence;
            this.m_strParamName = strParamName;
            this.m_methodBuilder = methodBuilder;
            this.m_strParamName = strParamName;
            this.m_attributes = attributes;
            this.m_pdToken = new ParameterToken(TypeBuilder.InternalSetParamInfo(this.m_methodBuilder.GetModule(), this.m_methodBuilder.GetToken().Token, sequence, attributes, strParamName));
        }

        public virtual ParameterToken GetToken() => 
            this.m_pdToken;

        public virtual void SetConstant(object defaultValue)
        {
            TypeBuilder.SetConstantValue(this.m_methodBuilder.GetModule(), this.m_pdToken.Token, (this.m_iPosition == 0) ? this.m_methodBuilder.m_returnType : this.m_methodBuilder.m_parameterTypes[this.m_iPosition - 1], defaultValue);
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            if (customBuilder == null)
            {
                throw new ArgumentNullException("customBuilder");
            }
            customBuilder.CreateCustomAttribute((ModuleBuilder) this.m_methodBuilder.GetModule(), this.m_pdToken.Token);
        }

        [ComVisible(true)]
        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }
            if (binaryAttribute == null)
            {
                throw new ArgumentNullException("binaryAttribute");
            }
            TypeBuilder.InternalCreateCustomAttribute(this.m_pdToken.Token, ((ModuleBuilder) this.m_methodBuilder.GetModule()).GetConstructorToken(con).Token, binaryAttribute, this.m_methodBuilder.GetModule(), false);
        }

        [Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        public virtual void SetMarshal(UnmanagedMarshal unmanagedMarshal)
        {
            if (unmanagedMarshal == null)
            {
                throw new ArgumentNullException("unmanagedMarshal");
            }
            byte[] bytes = unmanagedMarshal.InternalGetBytes();
            TypeBuilder.InternalSetMarshalInfo(this.m_methodBuilder.GetModule(), this.m_pdToken.Token, bytes, bytes.Length);
        }

        void _ParameterBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
        {
            throw new NotImplementedException();
        }

        void _ParameterBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
        {
            throw new NotImplementedException();
        }

        void _ParameterBuilder.GetTypeInfoCount(out uint pcTInfo)
        {
            throw new NotImplementedException();
        }

        void _ParameterBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
        {
            throw new NotImplementedException();
        }

        public virtual int Attributes =>
            ((int) this.m_attributes);

        public bool IsIn =>
            ((this.m_attributes & ParameterAttributes.In) != ParameterAttributes.None);

        public bool IsOptional =>
            ((this.m_attributes & ParameterAttributes.Optional) != ParameterAttributes.None);

        public bool IsOut =>
            ((this.m_attributes & ParameterAttributes.Out) != ParameterAttributes.None);

        internal virtual int MetadataTokenInternal =>
            this.m_pdToken.Token;

        public virtual string Name =>
            this.m_strParamName;

        public virtual int Position =>
            this.m_iPosition;
    }
}

