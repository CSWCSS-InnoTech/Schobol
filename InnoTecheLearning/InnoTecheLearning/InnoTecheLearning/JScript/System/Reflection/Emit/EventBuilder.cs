namespace System.Reflection.Emit
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    [ComDefaultInterface(typeof(_EventBuilder)), ComVisible(true), ClassInterface(ClassInterfaceType.None), HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort=true)]
    public sealed class EventBuilder : _EventBuilder
    {
        private EventAttributes m_attributes;
        private EventToken m_evToken;
        private Module m_module;
        private string m_name;
        private TypeBuilder m_type;

        private EventBuilder()
        {
        }

        internal EventBuilder(Module mod, string name, EventAttributes attr, int eventType, TypeBuilder type, EventToken evToken)
        {
            this.m_name = name;
            this.m_module = mod;
            this.m_attributes = attr;
            this.m_evToken = evToken;
            this.m_type = type;
        }

        public void AddOtherMethod(MethodBuilder mdBuilder)
        {
            if (mdBuilder == null)
            {
                throw new ArgumentNullException("mdBuilder");
            }
            this.m_type.ThrowIfCreated();
            TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_evToken.Token, MethodSemanticsAttributes.Other, mdBuilder.GetToken().Token);
        }

        public EventToken GetEventToken() => 
            this.m_evToken;

        public void SetAddOnMethod(MethodBuilder mdBuilder)
        {
            if (mdBuilder == null)
            {
                throw new ArgumentNullException("mdBuilder");
            }
            this.m_type.ThrowIfCreated();
            TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_evToken.Token, MethodSemanticsAttributes.AddOn, mdBuilder.GetToken().Token);
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            if (customBuilder == null)
            {
                throw new ArgumentNullException("customBuilder");
            }
            this.m_type.ThrowIfCreated();
            customBuilder.CreateCustomAttribute((ModuleBuilder) this.m_module, this.m_evToken.Token);
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
            this.m_type.ThrowIfCreated();
            TypeBuilder.InternalCreateCustomAttribute(this.m_evToken.Token, ((ModuleBuilder) this.m_module).GetConstructorToken(con).Token, binaryAttribute, this.m_module, false);
        }

        public void SetRaiseMethod(MethodBuilder mdBuilder)
        {
            if (mdBuilder == null)
            {
                throw new ArgumentNullException("mdBuilder");
            }
            this.m_type.ThrowIfCreated();
            TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_evToken.Token, MethodSemanticsAttributes.Fire, mdBuilder.GetToken().Token);
        }

        public void SetRemoveOnMethod(MethodBuilder mdBuilder)
        {
            if (mdBuilder == null)
            {
                throw new ArgumentNullException("mdBuilder");
            }
            this.m_type.ThrowIfCreated();
            TypeBuilder.InternalDefineMethodSemantics(this.m_module, this.m_evToken.Token, MethodSemanticsAttributes.RemoveOn, mdBuilder.GetToken().Token);
        }

        void _EventBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
        {
            throw new NotImplementedException();
        }

        void _EventBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
        {
            throw new NotImplementedException();
        }

        void _EventBuilder.GetTypeInfoCount(out uint pcTInfo)
        {
            throw new NotImplementedException();
        }

        void _EventBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
        {
            throw new NotImplementedException();
        }
    }
}

