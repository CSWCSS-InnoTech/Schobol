namespace System.Reflection.Emit
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    [ClassInterface(ClassInterfaceType.None), ComDefaultInterface(typeof(_PropertyBuilder)), ComVisible(true), HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort=true)]
    public sealed class PropertyBuilder : PropertyInfo, _PropertyBuilder
    {
        private PropertyAttributes m_attributes;
        private TypeBuilder m_containingType;
        private MethodInfo m_getMethod;
        private System.Reflection.Module m_module;
        private string m_name;
        private System.Reflection.Emit.PropertyToken m_prToken;
        private Type m_returnType;
        private MethodInfo m_setMethod;
        private SignatureHelper m_signature;
        private int m_tkProperty;

        private PropertyBuilder()
        {
        }

        internal PropertyBuilder(System.Reflection.Module mod, string name, SignatureHelper sig, PropertyAttributes attr, Type returnType, System.Reflection.Emit.PropertyToken prToken, TypeBuilder containingType)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
            }
            if (name[0] == '\0')
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_IllegalName"), "name");
            }
            this.m_name = name;
            this.m_module = mod;
            this.m_signature = sig;
            this.m_attributes = attr;
            this.m_returnType = returnType;
            this.m_prToken = prToken;
            this.m_tkProperty = prToken.Token;
            this.m_getMethod = null;
            this.m_setMethod = null;
            this.m_containingType = containingType;
        }

        public void AddOtherMethod(MethodBuilder mdBuilder)
        {
            if (mdBuilder == null)
            {
                throw new ArgumentNullException("mdBuilder");
            }
            this.m_containingType.ThrowIfCreated();
            TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_prToken.Token, MethodSemanticsAttributes.Other, mdBuilder.GetToken().Token);
        }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            if (nonPublic || (this.m_getMethod == null))
            {
                return this.m_getMethod;
            }
            if ((this.m_getMethod.Attributes & MethodAttributes.Public) == MethodAttributes.Public)
            {
                return this.m_getMethod;
            }
            return null;
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            if (nonPublic || (this.m_setMethod == null))
            {
                return this.m_setMethod;
            }
            if ((this.m_setMethod.Attributes & MethodAttributes.Public) == MethodAttributes.Public)
            {
                return this.m_setMethod;
            }
            return null;
        }

        public override object GetValue(object obj, object[] index)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        public void SetConstant(object defaultValue)
        {
            this.m_containingType.ThrowIfCreated();
            TypeBuilder.SetConstantValue(this.m_module, this.m_prToken.Token, this.m_returnType, defaultValue);
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            if (customBuilder == null)
            {
                throw new ArgumentNullException("customBuilder");
            }
            this.m_containingType.ThrowIfCreated();
            customBuilder.CreateCustomAttribute((ModuleBuilder) this.m_module, this.m_prToken.Token);
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
            this.m_containingType.ThrowIfCreated();
            TypeBuilder.InternalCreateCustomAttribute(this.m_prToken.Token, ((ModuleBuilder) this.m_module).GetConstructorToken(con).Token, binaryAttribute, this.m_module, false);
        }

        public void SetGetMethod(MethodBuilder mdBuilder)
        {
            if (mdBuilder == null)
            {
                throw new ArgumentNullException("mdBuilder");
            }
            this.m_containingType.ThrowIfCreated();
            TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_prToken.Token, MethodSemanticsAttributes.Getter, mdBuilder.GetToken().Token);
            this.m_getMethod = mdBuilder;
        }

        public void SetSetMethod(MethodBuilder mdBuilder)
        {
            if (mdBuilder == null)
            {
                throw new ArgumentNullException("mdBuilder");
            }
            this.m_containingType.ThrowIfCreated();
            TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_prToken.Token, MethodSemanticsAttributes.Setter, mdBuilder.GetToken().Token);
            this.m_setMethod = mdBuilder;
        }

        public override void SetValue(object obj, object value, object[] index)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        void _PropertyBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
        {
            throw new NotImplementedException();
        }

        void _PropertyBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
        {
            throw new NotImplementedException();
        }

        void _PropertyBuilder.GetTypeInfoCount(out uint pcTInfo)
        {
            throw new NotImplementedException();
        }

        void _PropertyBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
        {
            throw new NotImplementedException();
        }

        public override PropertyAttributes Attributes =>
            this.m_attributes;

        public override bool CanRead =>
            (this.m_getMethod != null);

        public override bool CanWrite =>
            (this.m_setMethod != null);

        public override Type DeclaringType =>
            this.m_containingType;

        internal override int MetadataTokenInternal =>
            this.m_tkProperty;

        public override System.Reflection.Module Module =>
            this.m_containingType.Module;

        public override string Name =>
            this.m_name;

        public System.Reflection.Emit.PropertyToken PropertyToken =>
            this.m_prToken;

        public override Type PropertyType =>
            this.m_returnType;

        public override Type ReflectedType =>
            this.m_containingType;
    }
}

