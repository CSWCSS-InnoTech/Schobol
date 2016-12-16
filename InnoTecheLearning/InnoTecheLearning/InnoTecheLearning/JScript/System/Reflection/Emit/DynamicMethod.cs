namespace System.Reflection.Emit
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;
    using System.Threading;

    [ComVisible(true)]
    public sealed class DynamicMethod : MethodInfo
    {
        internal CompressedStack m_creationContext;
        private DynamicILInfo m_DynamicILInfo;
        private RTDynamicMethod m_dynMethod;
        private bool m_fInitLocals;
        private DynamicILGenerator m_ilGenerator;
        internal RuntimeMethodHandle m_method;
        internal ModuleHandle m_module;
        private RuntimeType[] m_parameterTypes;
        internal DynamicResolver m_resolver;
        internal bool m_restrictedSkipVisibility;
        private RuntimeType m_returnType;
        internal bool m_skipVisibility;
        internal RuntimeType m_typeOwner;
        private static System.Reflection.Module s_anonymouslyHostedDynamicMethodsModule;
        private static readonly object s_anonymouslyHostedDynamicMethodsModuleLock = new object();

        private DynamicMethod()
        {
        }

        public DynamicMethod(string name, Type returnType, Type[] parameterTypes)
        {
            this.Init(name, MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, returnType, parameterTypes, null, null, false, true);
        }

        public DynamicMethod(string name, Type returnType, Type[] parameterTypes, bool restrictedSkipVisibility)
        {
            this.Init(name, MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, returnType, parameterTypes, null, null, restrictedSkipVisibility, true);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public DynamicMethod(string name, Type returnType, Type[] parameterTypes, System.Reflection.Module m)
        {
            StackCrawlMark lookForMyCaller = StackCrawlMark.LookForMyCaller;
            PerformSecurityCheck(m, ref lookForMyCaller, false);
            this.Init(name, MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, returnType, parameterTypes, null, m, false, false);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner)
        {
            StackCrawlMark lookForMyCaller = StackCrawlMark.LookForMyCaller;
            PerformSecurityCheck(owner, ref lookForMyCaller, false);
            this.Init(name, MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, returnType, parameterTypes, owner, null, false, false);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public DynamicMethod(string name, Type returnType, Type[] parameterTypes, System.Reflection.Module m, bool skipVisibility)
        {
            StackCrawlMark lookForMyCaller = StackCrawlMark.LookForMyCaller;
            PerformSecurityCheck(m, ref lookForMyCaller, skipVisibility);
            this.Init(name, MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, returnType, parameterTypes, null, m, skipVisibility, false);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public DynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner, bool skipVisibility)
        {
            StackCrawlMark lookForMyCaller = StackCrawlMark.LookForMyCaller;
            PerformSecurityCheck(owner, ref lookForMyCaller, skipVisibility);
            this.Init(name, MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, returnType, parameterTypes, owner, null, skipVisibility, false);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, System.Reflection.Module m, bool skipVisibility)
        {
            StackCrawlMark lookForMyCaller = StackCrawlMark.LookForMyCaller;
            PerformSecurityCheck(m, ref lookForMyCaller, skipVisibility);
            this.Init(name, attributes, callingConvention, returnType, parameterTypes, null, m, skipVisibility, false);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public DynamicMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type owner, bool skipVisibility)
        {
            StackCrawlMark lookForMyCaller = StackCrawlMark.LookForMyCaller;
            PerformSecurityCheck(owner, ref lookForMyCaller, skipVisibility);
            this.Init(name, attributes, callingConvention, returnType, parameterTypes, owner, null, skipVisibility, false);
        }

        private static void CheckConsistency(MethodAttributes attributes, CallingConventions callingConvention)
        {
            if ((attributes & ~MethodAttributes.MemberAccessMask) != MethodAttributes.Static)
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicMethodFlags"));
            }
            if ((attributes & MethodAttributes.MemberAccessMask) != MethodAttributes.Public)
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicMethodFlags"));
            }
            if ((callingConvention != CallingConventions.Standard) && (callingConvention != CallingConventions.VarArgs))
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicMethodFlags"));
            }
            if (callingConvention == CallingConventions.VarArgs)
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicMethodFlags"));
            }
        }

        [ComVisible(true)]
        public Delegate CreateDelegate(Type delegateType)
        {
            if (this.m_restrictedSkipVisibility)
            {
                RuntimeHelpers._CompileMethod(this.GetMethodDescriptor().Value);
            }
            MulticastDelegate delegate2 = (MulticastDelegate) Delegate.CreateDelegate(delegateType, null, this.GetMethodDescriptor());
            delegate2.StoreDynamicMethod(this.GetMethodInfo());
            return delegate2;
        }

        [ComVisible(true)]
        public Delegate CreateDelegate(Type delegateType, object target)
        {
            if (this.m_restrictedSkipVisibility)
            {
                RuntimeHelpers._CompileMethod(this.GetMethodDescriptor().Value);
            }
            MulticastDelegate delegate2 = (MulticastDelegate) Delegate.CreateDelegate(delegateType, target, this.GetMethodDescriptor());
            delegate2.StoreDynamicMethod(this.GetMethodInfo());
            return delegate2;
        }

        public ParameterBuilder DefineParameter(int position, ParameterAttributes attributes, string parameterName)
        {
            if ((position < 0) || (position > this.m_parameterTypes.Length))
            {
                throw new ArgumentOutOfRangeException(Environment.GetResourceString("ArgumentOutOfRange_ParamSequence"));
            }
            position--;
            if (position >= 0)
            {
                ParameterInfo[] infoArray = this.m_dynMethod.LoadParameters();
                infoArray[position].SetName(parameterName);
                infoArray[position].SetAttributes(attributes);
            }
            return null;
        }

        public override MethodInfo GetBaseDefinition() => 
            this;

        public override object[] GetCustomAttributes(bool inherit) => 
            this.m_dynMethod.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => 
            this.m_dynMethod.GetCustomAttributes(attributeType, inherit);

        public DynamicILInfo GetDynamicILInfo()
        {
            new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
            if (this.m_DynamicILInfo != null)
            {
                return this.m_DynamicILInfo;
            }
            return this.GetDynamicILInfo(new DynamicScope());
        }

        internal DynamicILInfo GetDynamicILInfo(DynamicScope scope)
        {
            if (this.m_DynamicILInfo == null)
            {
                byte[] signature = SignatureHelper.GetMethodSigHelper(null, this.CallingConvention, this.ReturnType, null, null, this.m_parameterTypes, null, null).GetSignature(true);
                this.m_DynamicILInfo = new DynamicILInfo(scope, this, signature);
            }
            return this.m_DynamicILInfo;
        }

        private static System.Reflection.Module GetDynamicMethodsModule()
        {
            if (s_anonymouslyHostedDynamicMethodsModule == null)
            {
                lock (s_anonymouslyHostedDynamicMethodsModuleLock)
                {
                    if (s_anonymouslyHostedDynamicMethodsModule != null)
                    {
                        return s_anonymouslyHostedDynamicMethodsModule;
                    }
                    CustomAttributeBuilder builder = new CustomAttributeBuilder(typeof(SecurityTransparentAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
                    CustomAttributeBuilder[] assemblyAttributes = new CustomAttributeBuilder[] { builder };
                    AssemblyName name = new AssemblyName("Anonymously Hosted DynamicMethods Assembly");
                    AssemblyBuilder builder2 = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run, assemblyAttributes);
                    AppDomain.CurrentDomain.PublishAnonymouslyHostedDynamicMethodsAssembly(builder2.InternalAssembly);
                    s_anonymouslyHostedDynamicMethodsModule = builder2.ManifestModule;
                }
            }
            return s_anonymouslyHostedDynamicMethodsModule;
        }

        public ILGenerator GetILGenerator() => 
            this.GetILGenerator(0x40);

        public ILGenerator GetILGenerator(int streamSize)
        {
            if (this.m_ilGenerator == null)
            {
                byte[] signature = SignatureHelper.GetMethodSigHelper(null, this.CallingConvention, this.ReturnType, null, null, this.m_parameterTypes, null, null).GetSignature(true);
                this.m_ilGenerator = new DynamicILGenerator(this, signature, streamSize);
            }
            return this.m_ilGenerator;
        }

        internal unsafe RuntimeMethodHandle GetMethodDescriptor()
        {
            if (this.m_method.IsNullHandle())
            {
                lock (this)
                {
                    if (this.m_method.IsNullHandle())
                    {
                        if (this.m_DynamicILInfo != null)
                        {
                            this.m_method = this.m_DynamicILInfo.GetCallableMethod(this.m_module.Value);
                        }
                        else
                        {
                            if ((this.m_ilGenerator == null) || (this.m_ilGenerator.m_length == 0))
                            {
                                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_BadEmptyMethodBody"), new object[] { this.Name }));
                            }
                            this.m_method = this.m_ilGenerator.GetCallableMethod(this.m_module.Value);
                        }
                    }
                }
            }
            return this.m_method;
        }

        public override MethodImplAttributes GetMethodImplementationFlags() => 
            this.m_dynMethod.GetMethodImplementationFlags();

        internal MethodInfo GetMethodInfo() => 
            this.m_dynMethod;

        public override ParameterInfo[] GetParameters() => 
            this.m_dynMethod.GetParameters();

        private void Init(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] signature, Type owner, System.Reflection.Module m, bool skipVisibility, bool transparentMethod)
        {
            CheckConsistency(attributes, callingConvention);
            if (signature != null)
            {
                this.m_parameterTypes = new RuntimeType[signature.Length];
                for (int i = 0; i < signature.Length; i++)
                {
                    if (signature[i] == null)
                    {
                        throw new ArgumentException(Environment.GetResourceString("Arg_InvalidTypeInSignature"));
                    }
                    this.m_parameterTypes[i] = signature[i].UnderlyingSystemType as RuntimeType;
                    if ((this.m_parameterTypes[i] == null) || (this.m_parameterTypes[i] == typeof(void)))
                    {
                        throw new ArgumentException(Environment.GetResourceString("Arg_InvalidTypeInSignature"));
                    }
                }
            }
            else
            {
                this.m_parameterTypes = new RuntimeType[0];
            }
            this.m_returnType = (returnType == null) ? ((RuntimeType) typeof(void)) : (returnType.UnderlyingSystemType as RuntimeType);
            if ((this.m_returnType == null) || this.m_returnType.IsByRef)
            {
                throw new NotSupportedException(Environment.GetResourceString("Arg_InvalidTypeInRetType"));
            }
            if (transparentMethod)
            {
                this.m_module = GetDynamicMethodsModule().ModuleHandle;
                if (skipVisibility)
                {
                    this.m_restrictedSkipVisibility = true;
                }
                this.m_creationContext = CompressedStack.Capture();
            }
            else
            {
                this.m_typeOwner = (owner != null) ? (owner.UnderlyingSystemType as RuntimeType) : null;
                if ((this.m_typeOwner != null) && ((this.m_typeOwner.HasElementType || this.m_typeOwner.ContainsGenericParameters) || (this.m_typeOwner.IsGenericParameter || this.m_typeOwner.IsInterface)))
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_InvalidTypeForDynamicMethod"));
                }
                this.m_module = (m != null) ? m.ModuleHandle : this.m_typeOwner.Module.ModuleHandle;
                this.m_skipVisibility = skipVisibility;
            }
            this.m_ilGenerator = null;
            this.m_fInitLocals = true;
            this.m_method = new RuntimeMethodHandle(null);
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            this.m_dynMethod = new RTDynamicMethod(this, name, attributes, callingConvention);
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            RuntimeMethodHandle methodDescriptor = this.GetMethodDescriptor();
            if ((this.CallingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs)
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_CallToVarArg"));
            }
            RuntimeTypeHandle[] arguments = new RuntimeTypeHandle[this.m_parameterTypes.Length];
            for (int i = 0; i < arguments.Length; i++)
            {
                arguments[i] = this.m_parameterTypes[i].TypeHandle;
            }
            Signature sig = new Signature(methodDescriptor, arguments, this.m_returnType.TypeHandle, this.CallingConvention);
            int length = sig.Arguments.Length;
            int num3 = (parameters != null) ? parameters.Length : 0;
            if (length != num3)
            {
                throw new TargetParameterCountException(Environment.GetResourceString("Arg_ParmCnt"));
            }
            object obj2 = null;
            if (num3 > 0)
            {
                object[] objArray = base.CheckArguments(parameters, binder, invokeAttr, culture, sig);
                obj2 = methodDescriptor.InvokeMethodFast(null, objArray, sig, this.Attributes, RuntimeTypeHandle.EmptyHandle);
                for (int j = 0; j < num3; j++)
                {
                    parameters[j] = objArray[j];
                }
            }
            else
            {
                obj2 = methodDescriptor.InvokeMethodFast(null, null, sig, this.Attributes, RuntimeTypeHandle.EmptyHandle);
            }
            GC.KeepAlive(this);
            return obj2;
        }

        public override bool IsDefined(Type attributeType, bool inherit) => 
            this.m_dynMethod.IsDefined(attributeType, inherit);

        private static void PerformSecurityCheck(System.Reflection.Module m, ref StackCrawlMark stackMark, bool skipVisibility)
        {
            if (m == null)
            {
                throw new ArgumentNullException("m");
            }
            if (m.Equals(s_anonymouslyHostedDynamicMethodsModule))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidValue"), "m");
            }
            if (skipVisibility)
            {
                new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
            }
            RuntimeTypeHandle callerType = ModuleHandle.GetCallerType(ref stackMark);
            if (!m.Assembly.AssemblyHandle.Equals(callerType.GetAssemblyHandle()) || (m == typeof(object).Module))
            {
                PermissionSet set;
                PermissionSet set2;
                m.Assembly.nGetGrantSet(out set, out set2);
                if (set == null)
                {
                    set = new PermissionSet(PermissionState.Unrestricted);
                }
                CodeAccessSecurityEngine.ReflectionTargetDemandHelper(PermissionType.SecurityControlEvidence, set);
            }
        }

        private static void PerformSecurityCheck(Type owner, ref StackCrawlMark stackMark, bool skipVisibility)
        {
            if ((owner == null) || ((owner = owner.UnderlyingSystemType as RuntimeType) == null))
            {
                throw new ArgumentNullException("owner");
            }
            RuntimeTypeHandle callerType = ModuleHandle.GetCallerType(ref stackMark);
            if (skipVisibility)
            {
                new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
            }
            else if (!callerType.Equals(owner.TypeHandle))
            {
                new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
            }
            if (!owner.Assembly.AssemblyHandle.Equals(callerType.GetAssemblyHandle()) || (owner.Module == typeof(object).Module))
            {
                PermissionSet set;
                PermissionSet set2;
                owner.Assembly.nGetGrantSet(out set, out set2);
                if (set == null)
                {
                    set = new PermissionSet(PermissionState.Unrestricted);
                }
                CodeAccessSecurityEngine.ReflectionTargetDemandHelper(PermissionType.SecurityControlEvidence, set);
            }
        }

        public override string ToString() => 
            this.m_dynMethod.ToString();

        public override MethodAttributes Attributes =>
            this.m_dynMethod.Attributes;

        public override CallingConventions CallingConvention =>
            this.m_dynMethod.CallingConvention;

        public override Type DeclaringType =>
            this.m_dynMethod.DeclaringType;

        public bool InitLocals
        {
            get { return this.m_fInitLocals; }
            set
            {
                this.m_fInitLocals = value;
            }
        }

        internal override bool IsOverloaded =>
            this.m_dynMethod.IsOverloaded;

        internal override int MetadataTokenInternal =>
            this.m_dynMethod.MetadataTokenInternal;

        public override RuntimeMethodHandle MethodHandle
        {
            get
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
            }
        }

        public override System.Reflection.Module Module =>
            this.m_dynMethod.Module;

        public override string Name =>
            this.m_dynMethod.Name;

        public override Type ReflectedType =>
            this.m_dynMethod.ReflectedType;

        public override ParameterInfo ReturnParameter =>
            this.m_dynMethod.ReturnParameter;

        public override Type ReturnType =>
            this.m_dynMethod.ReturnType;

        public override ICustomAttributeProvider ReturnTypeCustomAttributes =>
            this.m_dynMethod.ReturnTypeCustomAttributes;

        internal class RTDynamicMethod : MethodInfo
        {
            private MethodAttributes m_attributes;
            private CallingConventions m_callingConvention;
            private string m_name;
            internal DynamicMethod m_owner;
            private ParameterInfo[] m_parameters;

            private RTDynamicMethod()
            {
            }

            internal RTDynamicMethod(DynamicMethod owner, string name, MethodAttributes attributes, CallingConventions callingConvention)
            {
                this.m_owner = owner;
                this.m_name = name;
                this.m_attributes = attributes;
                this.m_callingConvention = callingConvention;
            }

            public override MethodInfo GetBaseDefinition() => 
                this;

            public override object[] GetCustomAttributes(bool inherit) => 
                new object[] { new MethodImplAttribute(this.GetMethodImplementationFlags()) };

            public override object[] GetCustomAttributes(Type attributeType, bool inherit)
            {
                if (attributeType == null)
                {
                    throw new ArgumentNullException("attributeType");
                }
                if (attributeType.IsAssignableFrom(typeof(MethodImplAttribute)))
                {
                    return new object[] { new MethodImplAttribute(this.GetMethodImplementationFlags()) };
                }
                return new object[0];
            }

            private ICustomAttributeProvider GetEmptyCAHolder() => 
                new EmptyCAHolder();

            public override MethodImplAttributes GetMethodImplementationFlags() => 
                MethodImplAttributes.NoInlining;

            public override ParameterInfo[] GetParameters()
            {
                ParameterInfo[] sourceArray = this.LoadParameters();
                ParameterInfo[] destinationArray = new ParameterInfo[sourceArray.Length];
                Array.Copy(sourceArray, destinationArray, sourceArray.Length);
                return destinationArray;
            }

            internal override Type GetReturnType() => 
                this.m_owner.m_returnType;

            public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeMethodInfo"), "this");
            }

            public override bool IsDefined(Type attributeType, bool inherit) => 
                attributeType?.IsAssignableFrom(typeof(MethodImplAttribute));

            internal ParameterInfo[] LoadParameters()
            {
                if (this.m_parameters == null)
                {
                    RuntimeType[] parameterTypes = this.m_owner.m_parameterTypes;
                    ParameterInfo[] infoArray = new ParameterInfo[parameterTypes.Length];
                    for (int i = 0; i < parameterTypes.Length; i++)
                    {
                        infoArray[i] = new ParameterInfo(this, null, parameterTypes[i], i);
                    }
                    if (this.m_parameters == null)
                    {
                        this.m_parameters = infoArray;
                    }
                }
                return this.m_parameters;
            }

            public override string ToString() => 
                (this.ReturnType.SigToString() + " " + RuntimeMethodInfo.ConstructName(this));

            public override MethodAttributes Attributes =>
                this.m_attributes;

            public override CallingConventions CallingConvention =>
                this.m_callingConvention;

            public override Type DeclaringType =>
                null;

            internal override bool IsOverloaded =>
                false;

            internal override int MetadataTokenInternal =>
                0;

            public override RuntimeMethodHandle MethodHandle
            {
                get
                {
                    throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
                }
            }

            public override System.Reflection.Module Module =>
                this.m_owner.m_module.GetModule();

            public override string Name =>
                this.m_name;

            public override Type ReflectedType =>
                null;

            public override ParameterInfo ReturnParameter =>
                null;

            public override ICustomAttributeProvider ReturnTypeCustomAttributes =>
                this.GetEmptyCAHolder();

            private class EmptyCAHolder : ICustomAttributeProvider
            {
                internal EmptyCAHolder()
                {
                }

                object[] ICustomAttributeProvider.GetCustomAttributes(bool inherit) => 
                    new object[0];

                object[] ICustomAttributeProvider.GetCustomAttributes(Type attributeType, bool inherit) => 
                    new object[0];

                bool ICustomAttributeProvider.IsDefined(Type attributeType, bool inherit) => 
                    false;
            }
        }
    }
}

