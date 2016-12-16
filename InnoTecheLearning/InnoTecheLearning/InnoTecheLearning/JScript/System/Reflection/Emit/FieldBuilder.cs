namespace System.Reflection.Emit
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    [ComVisible(true), ClassInterface(ClassInterfaceType.None), ComDefaultInterface(typeof(_FieldBuilder)), HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort=true)]
    public sealed class FieldBuilder : FieldInfo, _FieldBuilder
    {
        private FieldAttributes m_Attributes;
        internal byte[] m_data;
        private string m_fieldName;
        private int m_fieldTok;
        private Type m_fieldType;
        private FieldToken m_tkField;
        private TypeBuilder m_typeBuilder;

        internal FieldBuilder(TypeBuilder typeBuilder, string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
        {
            int num;
            if (fieldName == null)
            {
                throw new ArgumentNullException("fieldName");
            }
            if (fieldName.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "fieldName");
            }
            if (fieldName[0] == '\0')
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_IllegalName"), "fieldName");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            if (type == typeof(void))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_BadFieldType"));
            }
            this.m_fieldName = fieldName;
            this.m_typeBuilder = typeBuilder;
            this.m_fieldType = type;
            this.m_Attributes = attributes & ~FieldAttributes.ReservedMask;
            SignatureHelper fieldSigHelper = SignatureHelper.GetFieldSigHelper(this.m_typeBuilder.Module);
            fieldSigHelper.AddArgument(type, requiredCustomModifiers, optionalCustomModifiers);
            byte[] signature = fieldSigHelper.InternalGetSignature(out num);
            this.m_fieldTok = TypeBuilder.InternalDefineField(typeBuilder.TypeToken.Token, fieldName, signature, num, this.m_Attributes, this.m_typeBuilder.Module);
            this.m_tkField = new FieldToken(this.m_fieldTok, type);
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        public FieldToken GetToken() => 
            this.m_tkField;

        internal TypeBuilder GetTypeBuilder() => 
            this.m_typeBuilder;

        public override object GetValue(object obj)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        public void SetConstant(object defaultValue)
        {
            this.m_typeBuilder.ThrowIfCreated();
            TypeBuilder.SetConstantValue(this.m_typeBuilder.Module, this.GetToken().Token, this.m_fieldType, defaultValue);
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            this.m_typeBuilder.ThrowIfCreated();
            if (customBuilder == null)
            {
                throw new ArgumentNullException("customBuilder");
            }
            ModuleBuilder module = this.m_typeBuilder.Module as ModuleBuilder;
            customBuilder.CreateCustomAttribute(module, this.m_tkField.Token);
        }

        [ComVisible(true)]
        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
        {
            ModuleBuilder module = this.m_typeBuilder.Module as ModuleBuilder;
            this.m_typeBuilder.ThrowIfCreated();
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }
            if (binaryAttribute == null)
            {
                throw new ArgumentNullException("binaryAttribute");
            }
            TypeBuilder.InternalCreateCustomAttribute(this.m_tkField.Token, module.GetConstructorToken(con).Token, binaryAttribute, module, false);
        }

        internal void SetData(byte[] data, int size)
        {
            if (data != null)
            {
                this.m_data = new byte[data.Length];
                Array.Copy(data, this.m_data, data.Length);
            }
            this.m_typeBuilder.Module.InternalSetFieldRVAContent(this.m_tkField.Token, data, size);
        }

        [Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        public void SetMarshal(UnmanagedMarshal unmanagedMarshal)
        {
            this.m_typeBuilder.ThrowIfCreated();
            if (unmanagedMarshal == null)
            {
                throw new ArgumentNullException("unmanagedMarshal");
            }
            byte[] bytes = unmanagedMarshal.InternalGetBytes();
            TypeBuilder.InternalSetMarshalInfo(this.m_typeBuilder.Module, this.GetToken().Token, bytes, bytes.Length);
        }

        public void SetOffset(int iOffset)
        {
            this.m_typeBuilder.ThrowIfCreated();
            TypeBuilder.InternalSetFieldOffset(this.m_typeBuilder.Module, this.GetToken().Token, iOffset);
        }

        public override void SetValue(object obj, object val, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        void _FieldBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
        {
            throw new NotImplementedException();
        }

        void _FieldBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
        {
            throw new NotImplementedException();
        }

        void _FieldBuilder.GetTypeInfoCount(out uint pcTInfo)
        {
            throw new NotImplementedException();
        }

        void _FieldBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
        {
            throw new NotImplementedException();
        }

        public override FieldAttributes Attributes =>
            this.m_Attributes;

        public override Type DeclaringType
        {
            get
            {
                if (this.m_typeBuilder.m_isHiddenGlobalType)
                {
                    return null;
                }
                return this.m_typeBuilder;
            }
        }

        public override RuntimeFieldHandle FieldHandle
        {
            get
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
            }
        }

        public override Type FieldType =>
            this.m_fieldType;

        internal override int MetadataTokenInternal =>
            this.m_fieldTok;

        public override System.Reflection.Module Module =>
            this.m_typeBuilder.Module;

        public override string Name =>
            this.m_fieldName;

        public override Type ReflectedType
        {
            get
            {
                if (this.m_typeBuilder.m_isHiddenGlobalType)
                {
                    return null;
                }
                return this.m_typeBuilder;
            }
        }
    }
}

