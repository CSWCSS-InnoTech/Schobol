namespace System.Reflection.Emit
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;

    [ComVisible(true), ComDefaultInterface(typeof(_TypeBuilder)), ClassInterface(ClassInterfaceType.None), HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort=true)]
    public sealed class TypeBuilder : Type, _TypeBuilder
    {
        private bool m_bIsGenParam;
        private bool m_bIsGenTypeDef;
        internal ArrayList m_ca;
        private int m_constructorCount;
        internal MethodBuilder m_currentMethod;
        private TypeBuilder m_DeclaringType;
        private MethodBuilder m_declMeth;
        internal System.Reflection.GenericParameterAttributes m_genParamAttributes;
        private int m_genParamPos;
        private TypeBuilder m_genTypeDef;
        internal bool m_hasBeenCreated;
        internal TypeAttributes m_iAttr;
        private GenericTypeParameterBuilder[] m_inst;
        private System.Reflection.Emit.PackingSize m_iPackingSize;
        internal bool m_isHiddenGlobalType;
        internal bool m_isHiddenType;
        private int m_iTypeSize;
        internal ArrayList m_listMethods;
        private ModuleBuilder m_module;
        internal RuntimeType m_runtimeType;
        private string m_strFullQualName;
        internal string m_strName;
        private string m_strNameSpace;
        private System.Reflection.Emit.TypeToken m_tdType;
        private Type[] m_typeInterfaces;
        private Type m_typeParent;
        private Type m_underlyingSystemType;
        public const int UnspecifiedTypeSize = 0;

        internal TypeBuilder(ModuleBuilder module)
        {
            this.m_tdType = new System.Reflection.Emit.TypeToken(0x2000000);
            this.m_isHiddenGlobalType = true;
            this.m_module = module;
            this.m_listMethods = new ArrayList();
        }

        private TypeBuilder(TypeBuilder genTypeDef, GenericTypeParameterBuilder[] inst)
        {
            this.m_genTypeDef = genTypeDef;
            this.m_DeclaringType = genTypeDef.m_DeclaringType;
            this.m_typeParent = genTypeDef.m_typeParent;
            this.m_runtimeType = genTypeDef.m_runtimeType;
            this.m_tdType = genTypeDef.m_tdType;
            this.m_strName = genTypeDef.m_strName;
            this.m_bIsGenParam = false;
            this.m_bIsGenTypeDef = false;
            this.m_module = genTypeDef.m_module;
            this.m_inst = inst;
            this.m_hasBeenCreated = true;
        }

        internal TypeBuilder(string szName, int genParamPos, MethodBuilder declMeth)
        {
            this.m_declMeth = declMeth;
            this.m_DeclaringType = (TypeBuilder) this.m_declMeth.DeclaringType;
            this.m_module = (ModuleBuilder) declMeth.Module;
            this.InitAsGenericParam(szName, genParamPos);
        }

        private TypeBuilder(string szName, int genParamPos, TypeBuilder declType)
        {
            this.m_DeclaringType = declType;
            this.m_module = (ModuleBuilder) declType.Module;
            this.InitAsGenericParam(szName, genParamPos);
        }

        internal TypeBuilder(string name, TypeAttributes attr, Type parent, System.Reflection.Module module, System.Reflection.Emit.PackingSize iPackingSize, int iTypeSize, TypeBuilder enclosingType)
        {
            this.Init(name, attr, parent, null, module, iPackingSize, iTypeSize, enclosingType);
        }

        internal TypeBuilder(string name, TypeAttributes attr, Type parent, Type[] interfaces, System.Reflection.Module module, System.Reflection.Emit.PackingSize iPackingSize, TypeBuilder enclosingType)
        {
            this.Init(name, attr, parent, interfaces, module, iPackingSize, 0, enclosingType);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _InternalAddDeclarativeSecurity(System.Reflection.Module module, int parent, SecurityAction action, byte[] blob);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _InternalAddInterfaceImpl(int tdTypeDef, int tkInterface, System.Reflection.Module module);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _InternalCreateCustomAttribute(int tkAssociate, int tkConstructor, byte[] attr, System.Reflection.Module module, bool toDisk, bool updateCompilerFlags);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int _InternalDefineClass(string fullname, int tkParent, int[] interfaceTokens, TypeAttributes attr, System.Reflection.Module module, Guid guid, int tkEnclosingType, int tkTypeDef);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern int _InternalDefineEvent(System.Reflection.Module module, int handle, string name, int attributes, int tkEventType);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern int _InternalDefineField(int handle, string name, byte[] signature, int sigLength, FieldAttributes attributes, System.Reflection.Module module);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int _InternalDefineGenParam(string name, int tkParent, int position, int attributes, int[] interfaceTokens, System.Reflection.Module module, int tkTypeDef);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern int _InternalDefineMethod(int handle, string name, byte[] signature, int sigLength, MethodAttributes attributes, System.Reflection.Module module);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _InternalDefineMethodImpl(System.Reflection.Module module, int tkType, int tkBody, int tkDecl);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _InternalDefineMethodSemantics(System.Reflection.Module module, int tkAssociation, MethodSemanticsAttributes semantics, int tkMethod);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern int _InternalDefineMethodSpec(int handle, byte[] signature, int sigLength, System.Reflection.Module module);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern int _InternalDefineProperty(System.Reflection.Module module, int handle, string name, int attributes, byte[] signature, int sigLength, int notifyChanging, int notifyChanged);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern int _InternalGetTokenFromSig(System.Reflection.Module module, byte[] signature, int sigLength);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _InternalSetClassLayout(System.Reflection.Module module, int tdToken, System.Reflection.Emit.PackingSize iPackingSize, int iTypeSize);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _InternalSetConstantValue(System.Reflection.Module module, int tk, ref Variant var);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _InternalSetFieldOffset(System.Reflection.Module module, int fdToken, int iOffset);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _InternalSetMarshalInfo(System.Reflection.Module module, int tk, byte[] ubMarshal, int ubSize);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _InternalSetMethodIL(int methodHandle, bool isInitLocals, byte[] body, byte[] LocalSig, int sigLength, int maxStackSize, int numExceptions, __ExceptionInstance[] exceptions, int[] tokenFixups, int[] rvaFixups, System.Reflection.Module module);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _InternalSetMethodImpl(System.Reflection.Module module, int tkMethod, MethodImplAttributes MethodImplAttributes);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern int _InternalSetParamInfo(System.Reflection.Module module, int tkMethod, int iSequence, ParameterAttributes iParamAttributes, string strParamName);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _InternalSetParentType(int tdTypeDef, int tkParent, System.Reflection.Module module);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _InternalSetPInvokeData(System.Reflection.Module module, string DllName, string name, int token, int linkType, int linkFlags);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern Type _TermCreateClass(int handle, System.Reflection.Module module);
        public void AddDeclarativeSecurity(SecurityAction action, PermissionSet pset)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    this.AddDeclarativeSecurityNoLock(action, pset);
                    return;
                }
            }
            this.AddDeclarativeSecurityNoLock(action, pset);
        }

        private void AddDeclarativeSecurityNoLock(SecurityAction action, PermissionSet pset)
        {
            this.ThrowIfGeneric();
            if (pset == null)
            {
                throw new ArgumentNullException("pset");
            }
            if ((!Enum.IsDefined(typeof(SecurityAction), action) || (action == SecurityAction.RequestMinimum)) || ((action == SecurityAction.RequestOptional) || (action == SecurityAction.RequestRefuse)))
            {
                throw new ArgumentOutOfRangeException("action");
            }
            this.ThrowIfCreated();
            byte[] blob = null;
            if (!pset.IsEmpty())
            {
                blob = pset.EncodeXml();
            }
            InternalAddDeclarativeSecurity(this.m_module, this.m_tdType.Token, action, blob);
        }

        [ComVisible(true)]
        public void AddInterfaceImplementation(Type interfaceType)
        {
            this.ThrowIfGeneric();
            this.CheckContext(new Type[] { interfaceType });
            if (interfaceType == null)
            {
                throw new ArgumentNullException("interfaceType");
            }
            this.ThrowIfCreated();
            System.Reflection.Emit.TypeToken typeTokenInternal = this.m_module.GetTypeTokenInternal(interfaceType);
            InternalAddInterfaceImpl(this.m_tdType.Token, typeTokenInternal.Token, this.m_module);
        }

        internal void CheckContext(params Type[][] typess)
        {
            ((AssemblyBuilder) this.Module.Assembly).CheckContext(typess);
        }

        internal void CheckContext(params Type[] types)
        {
            ((AssemblyBuilder) this.Module.Assembly).CheckContext(types);
        }

        public Type CreateType()
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.CreateTypeNoLock();
                }
            }
            return this.CreateTypeNoLock();
        }

        private Type CreateTypeNoLock()
        {
            if (this.IsCreated())
            {
                return this.m_runtimeType;
            }
            this.ThrowIfGeneric();
            this.ThrowIfCreated();
            if (this.m_typeInterfaces == null)
            {
                this.m_typeInterfaces = new Type[0];
            }
            int[] numArray = new int[this.m_typeInterfaces.Length];
            for (int i = 0; i < this.m_typeInterfaces.Length; i++)
            {
                numArray[i] = this.m_module.GetTypeTokenInternal(this.m_typeInterfaces[i]).Token;
            }
            int tkParent = 0;
            if (this.m_typeParent != null)
            {
                tkParent = this.m_module.GetTypeTokenInternal(this.m_typeParent).Token;
            }
            if (this.IsGenericParameter)
            {
                int[] interfaceTokens = new int[this.m_typeInterfaces.Length];
                if (this.m_typeParent != null)
                {
                    interfaceTokens = new int[this.m_typeInterfaces.Length + 1];
                    interfaceTokens[interfaceTokens.Length - 1] = tkParent;
                }
                for (int k = 0; k < this.m_typeInterfaces.Length; k++)
                {
                    interfaceTokens[k] = this.m_module.GetTypeTokenInternal(this.m_typeInterfaces[k]).Token;
                }
                int num4 = (this.m_declMeth == null) ? this.m_DeclaringType.m_tdType.Token : this.m_declMeth.GetToken().Token;
                this.m_tdType = new System.Reflection.Emit.TypeToken(this.InternalDefineGenParam(this.m_strName, num4, this.m_genParamPos, (int) this.m_genParamAttributes, interfaceTokens, this.m_module, 0));
                if (this.m_ca != null)
                {
                    foreach (CustAttr attr in this.m_ca)
                    {
                        attr.Bake(this.m_module, this.MetadataTokenInternal);
                    }
                }
                this.m_hasBeenCreated = true;
                return this;
            }
            if (((this.m_tdType.Token & 0xffffff) != 0) && ((tkParent & 0xffffff) != 0))
            {
                InternalSetParentType(this.m_tdType.Token, tkParent, this.m_module);
            }
            if (this.m_inst != null)
            {
                GenericTypeParameterBuilder[] inst = this.m_inst;
                for (int m = 0; m < inst.Length; m++)
                {
                    Type type = inst[m];
                    if (type is GenericTypeParameterBuilder)
                    {
                        ((GenericTypeParameterBuilder) type).m_type.CreateType();
                    }
                }
            }
            if (((!this.m_isHiddenGlobalType && (this.m_constructorCount == 0)) && (((this.m_iAttr & TypeAttributes.ClassSemanticsMask) == TypeAttributes.AnsiClass) && !base.IsValueType)) && ((this.m_iAttr & (TypeAttributes.Sealed | TypeAttributes.Abstract)) != (TypeAttributes.Sealed | TypeAttributes.Abstract)))
            {
                this.DefineDefaultConstructor(MethodAttributes.Public);
            }
            int count = this.m_listMethods.Count;
            for (int j = 0; j < count; j++)
            {
                MethodBuilder builder = (MethodBuilder) this.m_listMethods[j];
                if (builder.IsGenericMethodDefinition)
                {
                    builder.GetToken();
                }
                MethodAttributes attributes = builder.Attributes;
                if (((builder.GetMethodImplementationFlags() & (MethodImplAttributes.PreserveSig | MethodImplAttributes.ManagedMask | MethodImplAttributes.CodeTypeMask)) == MethodImplAttributes.IL) && ((attributes & MethodAttributes.PinvokeImpl) == MethodAttributes.PrivateScope))
                {
                    int maxStackSize;
                    int num8;
                    byte[] signature = builder.GetLocalsSignature().InternalGetSignature(out num8);
                    if (((attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope) && ((this.m_iAttr & TypeAttributes.Abstract) == TypeAttributes.AnsiClass))
                    {
                        throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadTypeAttributesNotAbstract"));
                    }
                    byte[] body = builder.GetBody();
                    if ((attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
                    {
                        if (body != null)
                        {
                            throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadMethodBody"));
                        }
                    }
                    else if ((body == null) || (body.Length == 0))
                    {
                        if (builder.m_ilGenerator != null)
                        {
                            builder.CreateMethodBodyHelper(builder.GetILGenerator());
                        }
                        body = builder.GetBody();
                        if (((body == null) || (body.Length == 0)) && !builder.m_canBeRuntimeImpl)
                        {
                            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("InvalidOperation_BadEmptyMethodBody"), new object[] { builder.Name }));
                        }
                    }
                    if (builder.m_ilGenerator != null)
                    {
                        maxStackSize = builder.m_ilGenerator.GetMaxStackSize();
                    }
                    else
                    {
                        maxStackSize = 0x10;
                    }
                    __ExceptionInstance[] exceptionInstances = builder.GetExceptionInstances();
                    int[] tokenFixups = builder.GetTokenFixups();
                    int[] rVAFixups = builder.GetRVAFixups();
                    __ExceptionInstance[] destinationArray = null;
                    int[] numArray5 = null;
                    int[] numArray6 = null;
                    if (exceptionInstances != null)
                    {
                        destinationArray = new __ExceptionInstance[exceptionInstances.Length];
                        Array.Copy(exceptionInstances, destinationArray, exceptionInstances.Length);
                    }
                    if (tokenFixups != null)
                    {
                        numArray5 = new int[tokenFixups.Length];
                        Array.Copy(tokenFixups, numArray5, tokenFixups.Length);
                    }
                    if (rVAFixups != null)
                    {
                        numArray6 = new int[rVAFixups.Length];
                        Array.Copy(rVAFixups, numArray6, rVAFixups.Length);
                    }
                    InternalSetMethodIL(builder.GetToken().Token, builder.InitLocals, body, signature, num8, maxStackSize, builder.GetNumberOfExceptions(), destinationArray, numArray5, numArray6, this.m_module);
                    if (this.Assembly.m_assemblyData.m_access == AssemblyBuilderAccess.Run)
                    {
                        builder.ReleaseBakedStructures();
                    }
                }
            }
            this.m_hasBeenCreated = true;
            Type type2 = this.TermCreateClass(this.m_tdType.Token, this.m_module);
            if (this.m_isHiddenGlobalType)
            {
                return null;
            }
            this.m_runtimeType = (RuntimeType) type2;
            if ((this.m_DeclaringType != null) && (this.m_DeclaringType.m_runtimeType != null))
            {
                this.m_DeclaringType.m_runtimeType.InvalidateCachedNestedType();
            }
            return type2;
        }

        [ComVisible(true)]
        public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes) => 
            this.DefineConstructor(attributes, callingConvention, parameterTypes, null, null);

        [ComVisible(true)]
        public ConstructorBuilder DefineConstructor(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefineConstructorNoLock(attributes, callingConvention, parameterTypes, requiredCustomModifiers, optionalCustomModifiers);
                }
            }
            return this.DefineConstructorNoLock(attributes, callingConvention, parameterTypes, requiredCustomModifiers, optionalCustomModifiers);
        }

        private ConstructorBuilder DefineConstructorNoLock(MethodAttributes attributes, CallingConventions callingConvention, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
        {
            string constructorName;
            this.CheckContext(parameterTypes);
            this.CheckContext(requiredCustomModifiers);
            this.CheckContext(optionalCustomModifiers);
            this.ThrowIfGeneric();
            this.ThrowIfCreated();
            if ((attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
            {
                constructorName = ConstructorInfo.ConstructorName;
            }
            else
            {
                constructorName = ConstructorInfo.TypeConstructorName;
            }
            attributes |= MethodAttributes.SpecialName;
            ConstructorBuilder builder = new ConstructorBuilder(constructorName, attributes, callingConvention, parameterTypes, requiredCustomModifiers, optionalCustomModifiers, this.m_module, this);
            this.m_constructorCount++;
            return builder;
        }

        private FieldBuilder DefineDataHelper(string name, byte[] data, int size, FieldAttributes attributes)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
            }
            if ((size <= 0) || (size >= 0x3f0000))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_BadSizeForData"));
            }
            this.ThrowIfCreated();
            string strTypeName = "$ArrayType$" + size;
            TypeBuilder type = this.m_module.FindTypeBuilderWithName(strTypeName, false) as TypeBuilder;
            if (type == null)
            {
                TypeAttributes attr = TypeAttributes.Sealed | TypeAttributes.ExplicitLayout | TypeAttributes.Public;
                type = this.m_module.DefineType(strTypeName, attr, typeof(ValueType), System.Reflection.Emit.PackingSize.Size1, size);
                type.m_isHiddenType = true;
                type.CreateType();
            }
            FieldBuilder builder2 = this.DefineField(name, type, attributes | FieldAttributes.Static);
            builder2.SetData(data, size);
            return builder2;
        }

        [ComVisible(true)]
        public ConstructorBuilder DefineDefaultConstructor(MethodAttributes attributes)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefineDefaultConstructorNoLock(attributes);
                }
            }
            return this.DefineDefaultConstructorNoLock(attributes);
        }

        private ConstructorBuilder DefineDefaultConstructorNoLock(MethodAttributes attributes)
        {
            this.ThrowIfGeneric();
            ConstructorInfo con = null;
            if (this.m_typeParent is TypeBuilderInstantiation)
            {
                Type genericTypeDefinition = this.m_typeParent.GetGenericTypeDefinition();
                if (genericTypeDefinition is TypeBuilder)
                {
                    genericTypeDefinition = ((TypeBuilder) genericTypeDefinition).m_runtimeType;
                }
                if (genericTypeDefinition == null)
                {
                    throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
                }
                Type type = genericTypeDefinition.MakeGenericType(this.m_typeParent.GetGenericArguments());
                if (type is TypeBuilderInstantiation)
                {
                    con = GetConstructor(type, genericTypeDefinition.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null));
                }
                else
                {
                    con = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                }
            }
            if (con == null)
            {
                con = this.m_typeParent.GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
            }
            if (con == null)
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_NoParentDefaultConstructor"));
            }
            ConstructorBuilder builder = this.DefineConstructor(attributes, CallingConventions.Standard, null);
            this.m_constructorCount++;
            ILGenerator iLGenerator = builder.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Call, con);
            iLGenerator.Emit(OpCodes.Ret);
            builder.m_ReturnILGen = false;
            return builder;
        }

        public EventBuilder DefineEvent(string name, EventAttributes attributes, Type eventtype)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefineEventNoLock(name, attributes, eventtype);
                }
            }
            return this.DefineEventNoLock(name, attributes, eventtype);
        }

        private EventBuilder DefineEventNoLock(string name, EventAttributes attributes, Type eventtype)
        {
            this.CheckContext(new Type[] { eventtype });
            this.ThrowIfGeneric();
            this.ThrowIfCreated();
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
            int token = this.m_module.GetTypeTokenInternal(eventtype).Token;
            return new EventBuilder(this.m_module, name, attributes, token, this, new EventToken(InternalDefineEvent(this.m_module, this.m_tdType.Token, name, (int) attributes, token)));
        }

        public FieldBuilder DefineField(string fieldName, Type type, FieldAttributes attributes) => 
            this.DefineField(fieldName, type, null, null, attributes);

        public FieldBuilder DefineField(string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefineFieldNoLock(fieldName, type, requiredCustomModifiers, optionalCustomModifiers, attributes);
                }
            }
            return this.DefineFieldNoLock(fieldName, type, requiredCustomModifiers, optionalCustomModifiers, attributes);
        }

        private FieldBuilder DefineFieldNoLock(string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
        {
            this.ThrowIfGeneric();
            this.ThrowIfCreated();
            this.CheckContext(new Type[] { type });
            this.CheckContext(requiredCustomModifiers);
            if (((this.m_underlyingSystemType == null) && base.IsEnum) && ((attributes & FieldAttributes.Static) == FieldAttributes.PrivateScope))
            {
                this.m_underlyingSystemType = type;
            }
            return new FieldBuilder(this, fieldName, type, requiredCustomModifiers, optionalCustomModifiers, attributes);
        }

        public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
        {
            if (this.m_inst != null)
            {
                throw new InvalidOperationException();
            }
            if (names == null)
            {
                throw new ArgumentNullException("names");
            }
            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] == null)
                {
                    throw new ArgumentNullException("names");
                }
            }
            if (names.Length == 0)
            {
                throw new ArgumentException();
            }
            this.m_bIsGenTypeDef = true;
            this.m_inst = new GenericTypeParameterBuilder[names.Length];
            for (int j = 0; j < names.Length; j++)
            {
                this.m_inst[j] = new GenericTypeParameterBuilder(new TypeBuilder(names[j], j, this));
            }
            return this.m_inst;
        }

        public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefineInitializedDataNoLock(name, data, attributes);
                }
            }
            return this.DefineInitializedDataNoLock(name, data, attributes);
        }

        private FieldBuilder DefineInitializedDataNoLock(string name, byte[] data, FieldAttributes attributes)
        {
            this.ThrowIfGeneric();
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            return this.DefineDataHelper(name, data, data.Length, attributes);
        }

        public MethodBuilder DefineMethod(string name, MethodAttributes attributes) => 
            this.DefineMethod(name, attributes, CallingConventions.Standard, null, null);

        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention) => 
            this.DefineMethod(name, attributes, callingConvention, null, null);

        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes) => 
            this.DefineMethod(name, attributes, CallingConventions.Standard, returnType, parameterTypes);

        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes) => 
            this.DefineMethod(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);

        public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefineMethodNoLock(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
                }
            }
            return this.DefineMethodNoLock(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
        }

        private MethodBuilder DefineMethodNoLock(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
        {
            this.CheckContext(new Type[] { returnType });
            this.CheckContext(new Type[][] { returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes });
            this.CheckContext(parameterTypeRequiredCustomModifiers);
            this.CheckContext(parameterTypeOptionalCustomModifiers);
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
            }
            if (parameterTypes != null)
            {
                if ((parameterTypeOptionalCustomModifiers != null) && (parameterTypeOptionalCustomModifiers.Length != parameterTypes.Length))
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_MismatchedArrays", new object[] { "parameterTypeOptionalCustomModifiers", "parameterTypes" }));
                }
                if ((parameterTypeRequiredCustomModifiers != null) && (parameterTypeRequiredCustomModifiers.Length != parameterTypes.Length))
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_MismatchedArrays", new object[] { "parameterTypeRequiredCustomModifiers", "parameterTypes" }));
                }
            }
            this.ThrowIfGeneric();
            this.ThrowIfCreated();
            if ((!this.m_isHiddenGlobalType && ((this.m_iAttr & TypeAttributes.ClassSemanticsMask) == TypeAttributes.ClassSemanticsMask)) && (((attributes & MethodAttributes.Abstract) == MethodAttributes.PrivateScope) && ((attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_BadAttributeOnInterfaceMethod"));
            }
            MethodBuilder builder = new MethodBuilder(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, this.m_module, this, false);
            if ((!this.m_isHiddenGlobalType && ((builder.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope)) && builder.Name.Equals(ConstructorInfo.ConstructorName))
            {
                this.m_constructorCount++;
            }
            this.m_listMethods.Add(builder);
            return builder;
        }

        public void DefineMethodOverride(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    this.DefineMethodOverrideNoLock(methodInfoBody, methodInfoDeclaration);
                    return;
                }
            }
            this.DefineMethodOverrideNoLock(methodInfoBody, methodInfoDeclaration);
        }

        private void DefineMethodOverrideNoLock(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration)
        {
            this.ThrowIfGeneric();
            this.ThrowIfCreated();
            if (methodInfoBody == null)
            {
                throw new ArgumentNullException("methodInfoBody");
            }
            if (methodInfoDeclaration == null)
            {
                throw new ArgumentNullException("methodInfoDeclaration");
            }
            if (methodInfoBody.DeclaringType != this)
            {
                throw new ArgumentException(Environment.GetResourceString("ArgumentException_BadMethodImplBody"));
            }
            MethodToken methodTokenInternal = this.m_module.GetMethodTokenInternal(methodInfoBody);
            MethodToken token2 = this.m_module.GetMethodTokenInternal(methodInfoDeclaration);
            InternalDefineMethodImpl(this.m_module, this.m_tdType.Token, methodTokenInternal.Token, token2.Token);
        }

        public TypeBuilder DefineNestedType(string name)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefineNestedTypeNoLock(name);
                }
            }
            return this.DefineNestedTypeNoLock(name);
        }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefineNestedTypeNoLock(name, attr);
                }
            }
            return this.DefineNestedTypeNoLock(name, attr);
        }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefineNestedTypeNoLock(name, attr, parent);
                }
            }
            return this.DefineNestedTypeNoLock(name, attr, parent);
        }

        [ComVisible(true)]
        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefineNestedTypeNoLock(name, attr, parent, interfaces);
                }
            }
            return this.DefineNestedTypeNoLock(name, attr, parent, interfaces);
        }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, int typeSize)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefineNestedTypeNoLock(name, attr, parent, typeSize);
                }
            }
            return this.DefineNestedTypeNoLock(name, attr, parent, typeSize);
        }

        public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, System.Reflection.Emit.PackingSize packSize)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefineNestedTypeNoLock(name, attr, parent, packSize);
                }
            }
            return this.DefineNestedTypeNoLock(name, attr, parent, packSize);
        }

        private TypeBuilder DefineNestedTypeNoLock(string name)
        {
            this.ThrowIfGeneric();
            TypeBuilder builder = new TypeBuilder(name, TypeAttributes.NestedPrivate, null, null, this.m_module, System.Reflection.Emit.PackingSize.Unspecified, this);
            this.m_module.m_TypeBuilderList.Add(builder);
            return builder;
        }

        private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr)
        {
            this.ThrowIfGeneric();
            TypeBuilder builder = new TypeBuilder(name, attr, null, null, this.m_module, System.Reflection.Emit.PackingSize.Unspecified, this);
            this.m_module.m_TypeBuilderList.Add(builder);
            return builder;
        }

        private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr, Type parent)
        {
            this.ThrowIfGeneric();
            TypeBuilder builder = new TypeBuilder(name, attr, parent, null, this.m_module, System.Reflection.Emit.PackingSize.Unspecified, this);
            this.m_module.m_TypeBuilderList.Add(builder);
            return builder;
        }

        private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr, Type parent, Type[] interfaces)
        {
            this.CheckContext(new Type[] { parent });
            this.CheckContext(interfaces);
            this.ThrowIfGeneric();
            TypeBuilder builder = new TypeBuilder(name, attr, parent, interfaces, this.m_module, System.Reflection.Emit.PackingSize.Unspecified, this);
            this.m_module.m_TypeBuilderList.Add(builder);
            return builder;
        }

        private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr, Type parent, int typeSize)
        {
            TypeBuilder builder = new TypeBuilder(name, attr, parent, this.m_module, System.Reflection.Emit.PackingSize.Unspecified, typeSize, this);
            this.m_module.m_TypeBuilderList.Add(builder);
            return builder;
        }

        private TypeBuilder DefineNestedTypeNoLock(string name, TypeAttributes attr, Type parent, System.Reflection.Emit.PackingSize packSize)
        {
            this.ThrowIfGeneric();
            TypeBuilder builder = new TypeBuilder(name, attr, parent, null, this.m_module, packSize, this);
            this.m_module.m_TypeBuilderList.Add(builder);
            return builder;
        }

        public MethodBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            this.ThrowIfGeneric();
            return this.DefinePInvokeMethodHelper(name, dllName, name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null, nativeCallConv, nativeCharSet);
        }

        public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet) => 
            this.DefinePInvokeMethodHelper(name, dllName, entryName, attributes, callingConvention, returnType, null, null, parameterTypes, null, null, nativeCallConv, nativeCharSet);

        public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            this.ThrowIfGeneric();
            return this.DefinePInvokeMethodHelper(name, dllName, entryName, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, nativeCallConv, nativeCharSet);
        }

        private MethodBuilder DefinePInvokeMethodHelper(string name, string dllName, string importName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            this.CheckContext(new Type[] { returnType });
            this.CheckContext(new Type[][] { returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes });
            this.CheckContext(parameterTypeRequiredCustomModifiers);
            this.CheckContext(parameterTypeOptionalCustomModifiers);
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefinePInvokeMethodHelperNoLock(name, dllName, importName, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, nativeCallConv, nativeCharSet);
                }
            }
            return this.DefinePInvokeMethodHelperNoLock(name, dllName, importName, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, nativeCallConv, nativeCharSet);
        }

        private MethodBuilder DefinePInvokeMethodHelperNoLock(string name, string dllName, string importName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            int num;
            this.ThrowIfCreated();
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
            }
            if (dllName == null)
            {
                throw new ArgumentNullException("dllName");
            }
            if (dllName.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "dllName");
            }
            if (importName == null)
            {
                throw new ArgumentNullException("importName");
            }
            if (importName.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "importName");
            }
            if ((this.m_iAttr & TypeAttributes.ClassSemanticsMask) == TypeAttributes.ClassSemanticsMask)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_BadPInvokeOnInterface"));
            }
            if ((attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_BadPInvokeMethod"));
            }
            attributes |= MethodAttributes.PinvokeImpl;
            MethodBuilder item = new MethodBuilder(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers, this.m_module, this, false);
            item.GetMethodSignature().InternalGetSignature(out num);
            if (this.m_listMethods.Contains(item))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_MethodRedefined"));
            }
            this.m_listMethods.Add(item);
            MethodToken token = item.GetToken();
            int linkFlags = 0;
            switch (nativeCallConv)
            {
                case CallingConvention.Winapi:
                    linkFlags = 0x100;
                    break;

                case CallingConvention.Cdecl:
                    linkFlags = 0x200;
                    break;

                case CallingConvention.StdCall:
                    linkFlags = 0x300;
                    break;

                case CallingConvention.ThisCall:
                    linkFlags = 0x400;
                    break;

                case CallingConvention.FastCall:
                    linkFlags = 0x500;
                    break;
            }
            switch (nativeCharSet)
            {
                case CharSet.None:
                    break;

                case CharSet.Ansi:
                    linkFlags |= 2;
                    break;

                case CharSet.Unicode:
                    linkFlags |= 4;
                    break;

                case CharSet.Auto:
                    linkFlags |= 6;
                    break;
            }
            InternalSetPInvokeData(this.m_module, dllName, importName, token.Token, 0, linkFlags);
            item.SetToken(token);
            return item;
        }

        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] parameterTypes) => 
            this.DefineProperty(name, attributes, returnType, null, null, parameterTypes, null, null);

        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers) => 
            this.DefineProperty(name, attributes, 0, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);

        public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefinePropertyNoLock(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
                }
            }
            return this.DefinePropertyNoLock(name, attributes, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
        }

        private PropertyBuilder DefinePropertyNoLock(string name, PropertyAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
        {
            int num;
            this.ThrowIfGeneric();
            this.CheckContext(new Type[] { returnType });
            this.CheckContext(new Type[][] { returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes });
            this.CheckContext(parameterTypeRequiredCustomModifiers);
            this.CheckContext(parameterTypeOptionalCustomModifiers);
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
            }
            this.ThrowIfCreated();
            SignatureHelper sig = SignatureHelper.GetPropertySigHelper(this.m_module, callingConvention, returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
            byte[] signature = sig.InternalGetSignature(out num);
            return new PropertyBuilder(this.m_module, name, sig, attributes, returnType, new PropertyToken(InternalDefineProperty(this.m_module, this.m_tdType.Token, name, (int) attributes, signature, num, 0, 0)), this);
        }

        [ComVisible(true)]
        public ConstructorBuilder DefineTypeInitializer()
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefineTypeInitializerNoLock();
                }
            }
            return this.DefineTypeInitializerNoLock();
        }

        private ConstructorBuilder DefineTypeInitializerNoLock()
        {
            this.ThrowIfGeneric();
            this.ThrowIfCreated();
            return new ConstructorBuilder(ConstructorInfo.TypeConstructorName, MethodAttributes.SpecialName | MethodAttributes.Static | MethodAttributes.Private, CallingConventions.Standard, null, this.m_module, this);
        }

        public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
        {
            if (this.Module.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (this.Module.Assembly.m_assemblyData)
                {
                    return this.DefineUninitializedDataNoLock(name, size, attributes);
                }
            }
            return this.DefineUninitializedDataNoLock(name, size, attributes);
        }

        private FieldBuilder DefineUninitializedDataNoLock(string name, int size, FieldAttributes attributes)
        {
            this.ThrowIfGeneric();
            return this.DefineDataHelper(name, null, size, attributes);
        }

        protected override TypeAttributes GetAttributeFlagsImpl() => 
            this.m_iAttr;

        public static ConstructorInfo GetConstructor(Type type, ConstructorInfo constructor)
        {
            if (!(type is TypeBuilder) && !(type is TypeBuilderInstantiation))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_MustBeTypeBuilder"));
            }
            if (!constructor.DeclaringType.IsGenericTypeDefinition)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_ConstructorNeedGenericDeclaringType"), "constructor");
            }
            if (!(type is TypeBuilderInstantiation))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NeedNonGenericType"), "type");
            }
            if ((type is TypeBuilder) && type.IsGenericTypeDefinition)
            {
                type = type.MakeGenericType(type.GetGenericArguments());
            }
            if (type.GetGenericTypeDefinition() != constructor.DeclaringType)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidConstructorDeclaringType"), "type");
            }
            return ConstructorOnTypeBuilderInstantiation.GetConstructor(constructor, type as TypeBuilderInstantiation);
        }

        protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers) => 
            this.m_runtimeType?.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);

        [ComVisible(true)]
        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) => 
            this.m_runtimeType?.GetConstructors(bindingAttr);

        public override object[] GetCustomAttributes(bool inherit)
        {
            if (this.m_runtimeType == null)
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
            }
            return CustomAttribute.GetCustomAttributes(this.m_runtimeType, typeof(object) as RuntimeType, inherit);
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            if (this.m_runtimeType == null)
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
            }
            if (attributeType == null)
            {
                throw new ArgumentNullException("attributeType");
            }
            RuntimeType underlyingSystemType = attributeType.UnderlyingSystemType as RuntimeType;
            if (underlyingSystemType == null)
            {
                throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "attributeType");
            }
            return CustomAttribute.GetCustomAttributes(this.m_runtimeType, underlyingSystemType, inherit);
        }

        public override Type GetElementType()
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        public override EventInfo GetEvent(string name, BindingFlags bindingAttr) => 
            this.m_runtimeType?.GetEvent(name, bindingAttr);

        public override EventInfo[] GetEvents() => 
            this.m_runtimeType?.GetEvents();

        public override EventInfo[] GetEvents(BindingFlags bindingAttr) => 
            this.m_runtimeType?.GetEvents(bindingAttr);

        public override FieldInfo GetField(string name, BindingFlags bindingAttr) => 
            this.m_runtimeType?.GetField(name, bindingAttr);

        public static FieldInfo GetField(Type type, FieldInfo field)
        {
            if (!(type is TypeBuilder) && !(type is TypeBuilderInstantiation))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_MustBeTypeBuilder"));
            }
            if (!field.DeclaringType.IsGenericTypeDefinition)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_FieldNeedGenericDeclaringType"), "field");
            }
            if (!(type is TypeBuilderInstantiation))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NeedNonGenericType"), "type");
            }
            if ((type is TypeBuilder) && type.IsGenericTypeDefinition)
            {
                type = type.MakeGenericType(type.GetGenericArguments());
            }
            if (type.GetGenericTypeDefinition() != field.DeclaringType)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidFieldDeclaringType"), "type");
            }
            return FieldOnTypeBuilderInstantiation.GetField(field, type as TypeBuilderInstantiation);
        }

        public override FieldInfo[] GetFields(BindingFlags bindingAttr) => 
            this.m_runtimeType?.GetFields(bindingAttr);

        public override Type[] GetGenericArguments() => 
            this.m_inst;

        public override Type GetGenericTypeDefinition()
        {
            if (this.IsGenericTypeDefinition)
            {
                return this;
            }
            if (this.m_genTypeDef == null)
            {
                throw new InvalidOperationException();
            }
            return this.m_genTypeDef;
        }

        public override Type GetInterface(string name, bool ignoreCase) => 
            this.m_runtimeType?.GetInterface(name, ignoreCase);

        [ComVisible(true)]
        public override InterfaceMapping GetInterfaceMap(Type interfaceType) => 
            this.m_runtimeType?.GetInterfaceMap(interfaceType);

        public override Type[] GetInterfaces()
        {
            if (this.m_runtimeType != null)
            {
                return this.m_runtimeType.GetInterfaces();
            }
            if (this.m_typeInterfaces == null)
            {
                return new Type[0];
            }
            Type[] destinationArray = new Type[this.m_typeInterfaces.Length];
            Array.Copy(this.m_typeInterfaces, destinationArray, this.m_typeInterfaces.Length);
            return destinationArray;
        }

        public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr) => 
            this.m_runtimeType?.GetMember(name, type, bindingAttr);

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr) => 
            this.m_runtimeType?.GetMembers(bindingAttr);

        public static MethodInfo GetMethod(Type type, MethodInfo method)
        {
            if (!(type is TypeBuilder) && !(type is TypeBuilderInstantiation))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_MustBeTypeBuilder"));
            }
            if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NeedGenericMethodDefinition"), "method");
            }
            if ((method.DeclaringType == null) || !method.DeclaringType.IsGenericTypeDefinition)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_MethodNeedGenericDeclaringType"), "method");
            }
            if (type.GetGenericTypeDefinition() != method.DeclaringType)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidMethodDeclaringType"), "type");
            }
            if (type.IsGenericTypeDefinition)
            {
                type = type.MakeGenericType(type.GetGenericArguments());
            }
            if (!(type is TypeBuilderInstantiation))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NeedNonGenericType"), "type");
            }
            return MethodOnTypeBuilderInstantiation.GetMethod(method, type as TypeBuilderInstantiation);
        }

        protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            if (this.m_runtimeType == null)
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
            }
            if (types == null)
            {
                return this.m_runtimeType.GetMethod(name, bindingAttr);
            }
            return this.m_runtimeType.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
        }

        public override MethodInfo[] GetMethods(BindingFlags bindingAttr) => 
            this.m_runtimeType?.GetMethods(bindingAttr);

        public override Type GetNestedType(string name, BindingFlags bindingAttr) => 
            this.m_runtimeType?.GetNestedType(name, bindingAttr);

        public override Type[] GetNestedTypes(BindingFlags bindingAttr) => 
            this.m_runtimeType?.GetNestedTypes(bindingAttr);

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr) => 
            this.m_runtimeType?.GetProperties(bindingAttr);

        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
        }

        protected override bool HasElementTypeImpl() => 
            false;

        private void Init(string fullname, TypeAttributes attr, Type parent, Type[] interfaces, System.Reflection.Module module, System.Reflection.Emit.PackingSize iPackingSize, int iTypeSize, TypeBuilder enclosingType)
        {
            this.m_bIsGenTypeDef = false;
            int[] interfaceTokens = null;
            this.m_bIsGenParam = false;
            this.m_hasBeenCreated = false;
            this.m_runtimeType = null;
            this.m_isHiddenGlobalType = false;
            this.m_isHiddenType = false;
            this.m_module = (ModuleBuilder) module;
            this.m_DeclaringType = enclosingType;
            System.Reflection.Assembly assembly = this.m_module.Assembly;
            this.m_underlyingSystemType = null;
            if (fullname == null)
            {
                throw new ArgumentNullException("fullname");
            }
            if (fullname.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "fullname");
            }
            if (fullname[0] == '\0')
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_IllegalName"), "fullname");
            }
            if (fullname.Length > 0x3ff)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_TypeNameTooLong"), "fullname");
            }
            assembly.m_assemblyData.CheckTypeNameConflict(fullname, enclosingType);
            if ((enclosingType != null) && (((attr & TypeAttributes.NestedFamORAssem) == TypeAttributes.Public) || ((attr & TypeAttributes.NestedFamORAssem) == TypeAttributes.AnsiClass)))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_BadNestedTypeFlags"), "attr");
            }
            if (interfaces != null)
            {
                int num;
                for (num = 0; num < interfaces.Length; num++)
                {
                    if (interfaces[num] == null)
                    {
                        throw new ArgumentNullException("interfaces");
                    }
                }
                interfaceTokens = new int[interfaces.Length];
                for (num = 0; num < interfaces.Length; num++)
                {
                    interfaceTokens[num] = this.m_module.GetTypeTokenInternal(interfaces[num]).Token;
                }
            }
            int length = fullname.LastIndexOf('.');
            switch (length)
            {
                case -1:
                case 0:
                    this.m_strNameSpace = string.Empty;
                    this.m_strName = fullname;
                    break;

                default:
                    this.m_strNameSpace = fullname.Substring(0, length);
                    this.m_strName = fullname.Substring(length + 1);
                    break;
            }
            this.VerifyTypeAttributes(attr);
            this.m_iAttr = attr;
            this.SetParent(parent);
            this.m_listMethods = new ArrayList();
            this.SetInterfaces(interfaces);
            this.m_constructorCount = 0;
            int tkParent = 0;
            if (this.m_typeParent != null)
            {
                tkParent = this.m_module.GetTypeTokenInternal(this.m_typeParent).Token;
            }
            int tkEnclosingType = 0;
            if (enclosingType != null)
            {
                tkEnclosingType = enclosingType.m_tdType.Token;
            }
            this.m_tdType = new System.Reflection.Emit.TypeToken(this.InternalDefineClass(fullname, tkParent, interfaceTokens, this.m_iAttr, this.m_module, Guid.Empty, tkEnclosingType, 0));
            this.m_iPackingSize = iPackingSize;
            this.m_iTypeSize = iTypeSize;
            if ((this.m_iPackingSize != System.Reflection.Emit.PackingSize.Unspecified) || (this.m_iTypeSize != 0))
            {
                InternalSetClassLayout(this.Module, this.m_tdType.Token, this.m_iPackingSize, this.m_iTypeSize);
            }
            if (IsPublicComType(this) && (assembly is AssemblyBuilder))
            {
                AssemblyBuilder builder = (AssemblyBuilder) assembly;
                if (builder.IsPersistable() && !this.m_module.IsTransient())
                {
                    builder.m_assemblyData.AddPublicComType(this);
                }
            }
        }

        private void InitAsGenericParam(string szName, int genParamPos)
        {
            this.m_strName = szName;
            this.m_genParamPos = genParamPos;
            this.m_bIsGenParam = true;
            this.m_bIsGenTypeDef = false;
            this.m_typeInterfaces = new Type[0];
        }

        internal static void InternalAddDeclarativeSecurity(System.Reflection.Module module, int parent, SecurityAction action, byte[] blob)
        {
            _InternalAddDeclarativeSecurity(module.InternalModule, parent, action, blob);
        }

        private static void InternalAddInterfaceImpl(int tdTypeDef, int tkInterface, System.Reflection.Module module)
        {
            _InternalAddInterfaceImpl(tdTypeDef, tkInterface, module.InternalModule);
        }

        internal static void InternalCreateCustomAttribute(int tkAssociate, int tkConstructor, byte[] attr, System.Reflection.Module module, bool toDisk)
        {
            byte[] destinationArray = null;
            if (attr != null)
            {
                destinationArray = new byte[attr.Length];
                Array.Copy(attr, destinationArray, attr.Length);
            }
            InternalCreateCustomAttribute(tkAssociate, tkConstructor, destinationArray, module.InternalModule, toDisk, false);
        }

        internal static void InternalCreateCustomAttribute(int tkAssociate, int tkConstructor, byte[] attr, System.Reflection.Module module, bool toDisk, bool updateCompilerFlags)
        {
            _InternalCreateCustomAttribute(tkAssociate, tkConstructor, attr, module.InternalModule, toDisk, updateCompilerFlags);
        }

        private int InternalDefineClass(string fullname, int tkParent, int[] interfaceTokens, TypeAttributes attr, System.Reflection.Module module, Guid guid, int tkEnclosingType, int tkTypeDef) => 
            this._InternalDefineClass(fullname, tkParent, interfaceTokens, attr, module.InternalModule, guid, tkEnclosingType, tkTypeDef);

        internal static int InternalDefineEvent(System.Reflection.Module module, int handle, string name, int attributes, int tkEventType) => 
            _InternalDefineEvent(module.InternalModule, handle, name, attributes, tkEventType);

        internal static int InternalDefineField(int handle, string name, byte[] signature, int sigLength, FieldAttributes attributes, System.Reflection.Module module) => 
            _InternalDefineField(handle, name, signature, sigLength, attributes, module.InternalModule);

        private int InternalDefineGenParam(string name, int tkParent, int position, int attributes, int[] interfaceTokens, System.Reflection.Module module, int tkTypeDef) => 
            this._InternalDefineGenParam(name, tkParent, position, attributes, interfaceTokens, module.InternalModule, tkTypeDef);

        internal static int InternalDefineMethod(int handle, string name, byte[] signature, int sigLength, MethodAttributes attributes, System.Reflection.Module module) => 
            _InternalDefineMethod(handle, name, signature, sigLength, attributes, module.InternalModule);

        internal static void InternalDefineMethodImpl(System.Reflection.Module module, int tkType, int tkBody, int tkDecl)
        {
            _InternalDefineMethodImpl(module.InternalModule, tkType, tkBody, tkDecl);
        }

        internal static void InternalDefineMethodSemantics(System.Reflection.Module module, int tkAssociation, MethodSemanticsAttributes semantics, int tkMethod)
        {
            _InternalDefineMethodSemantics(module.InternalModule, tkAssociation, semantics, tkMethod);
        }

        internal static int InternalDefineMethodSpec(int handle, byte[] signature, int sigLength, System.Reflection.Module module) => 
            _InternalDefineMethodSpec(handle, signature, sigLength, module.InternalModule);

        internal static int InternalDefineProperty(System.Reflection.Module module, int handle, string name, int attributes, byte[] signature, int sigLength, int notifyChanging, int notifyChanged) => 
            _InternalDefineProperty(module.InternalModule, handle, name, attributes, signature, sigLength, notifyChanging, notifyChanged);

        internal static int InternalGetTokenFromSig(System.Reflection.Module module, byte[] signature, int sigLength) => 
            _InternalGetTokenFromSig(module.InternalModule, signature, sigLength);

        internal static void InternalSetClassLayout(System.Reflection.Module module, int tdToken, System.Reflection.Emit.PackingSize iPackingSize, int iTypeSize)
        {
            _InternalSetClassLayout(module.InternalModule, tdToken, iPackingSize, iTypeSize);
        }

        private static void InternalSetConstantValue(System.Reflection.Module module, int tk, ref Variant var)
        {
            _InternalSetConstantValue(module.InternalModule, tk, ref var);
        }

        internal static void InternalSetFieldOffset(System.Reflection.Module module, int fdToken, int iOffset)
        {
            _InternalSetFieldOffset(module.InternalModule, fdToken, iOffset);
        }

        internal static void InternalSetMarshalInfo(System.Reflection.Module module, int tk, byte[] ubMarshal, int ubSize)
        {
            _InternalSetMarshalInfo(module.InternalModule, tk, ubMarshal, ubSize);
        }

        internal static void InternalSetMethodIL(int methodHandle, bool isInitLocals, byte[] body, byte[] LocalSig, int sigLength, int maxStackSize, int numExceptions, __ExceptionInstance[] exceptions, int[] tokenFixups, int[] rvaFixups, System.Reflection.Module module)
        {
            _InternalSetMethodIL(methodHandle, isInitLocals, body, LocalSig, sigLength, maxStackSize, numExceptions, exceptions, tokenFixups, rvaFixups, module.InternalModule);
        }

        internal static void InternalSetMethodImpl(System.Reflection.Module module, int tkMethod, MethodImplAttributes MethodImplAttributes)
        {
            _InternalSetMethodImpl(module.InternalModule, tkMethod, MethodImplAttributes);
        }

        internal static int InternalSetParamInfo(System.Reflection.Module module, int tkMethod, int iSequence, ParameterAttributes iParamAttributes, string strParamName) => 
            _InternalSetParamInfo(module.InternalModule, tkMethod, iSequence, iParamAttributes, strParamName);

        private static void InternalSetParentType(int tdTypeDef, int tkParent, System.Reflection.Module module)
        {
            _InternalSetParentType(tdTypeDef, tkParent, module.InternalModule);
        }

        internal static void InternalSetPInvokeData(System.Reflection.Module module, string DllName, string name, int token, int linkType, int linkFlags)
        {
            _InternalSetPInvokeData(module.InternalModule, DllName, name, token, linkType, linkFlags);
        }

        public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters) => 
            this.m_runtimeType?.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);

        protected override bool IsArrayImpl() => 
            false;

        public override bool IsAssignableFrom(Type c)
        {
            if (IsTypeEqual(c, this))
            {
                return true;
            }
            RuntimeType runtimeType = c as RuntimeType;
            TypeBuilder builder = c as TypeBuilder;
            if ((builder != null) && (builder.m_runtimeType != null))
            {
                runtimeType = builder.m_runtimeType;
            }
            if (runtimeType != null)
            {
                return this.m_runtimeType?.IsAssignableFrom(runtimeType);
            }
            if (builder != null)
            {
                if (builder.IsSubclassOf(this))
                {
                    return true;
                }
                if (!base.IsInterface)
                {
                    return false;
                }
                Type[] interfaces = builder.GetInterfaces();
                for (int i = 0; i < interfaces.Length; i++)
                {
                    if (IsTypeEqual(interfaces[i], this))
                    {
                        return true;
                    }
                    if (interfaces[i].IsSubclassOf(this))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected override bool IsByRefImpl() => 
            false;

        protected override bool IsCOMObjectImpl()
        {
            if ((this.GetAttributeFlagsImpl() & TypeAttributes.Import) == TypeAttributes.AnsiClass)
            {
                return false;
            }
            return true;
        }

        public bool IsCreated() => 
            this.m_hasBeenCreated;

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            if (this.m_runtimeType == null)
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
            }
            RuntimeType underlyingSystemType = attributeType.UnderlyingSystemType as RuntimeType;
            if (underlyingSystemType == null)
            {
                throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"), "caType");
            }
            return CustomAttribute.IsDefined(this.m_runtimeType, underlyingSystemType, inherit);
        }

        protected override bool IsPointerImpl() => 
            false;

        protected override bool IsPrimitiveImpl() => 
            false;

        private static bool IsPublicComType(Type type)
        {
            Type declaringType = type.DeclaringType;
            if (declaringType != null)
            {
                if (IsPublicComType(declaringType) && ((type.Attributes & TypeAttributes.NestedFamORAssem) == TypeAttributes.NestedPublic))
                {
                    return true;
                }
            }
            else if ((type.Attributes & TypeAttributes.NestedFamORAssem) == TypeAttributes.Public)
            {
                return true;
            }
            return false;
        }

        [ComVisible(true)]
        public override bool IsSubclassOf(Type c)
        {
            Type type = this;
            if (!IsTypeEqual(type, c))
            {
                for (type = type.BaseType; type != null; type = type.BaseType)
                {
                    if (IsTypeEqual(type, c))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal static bool IsTypeEqual(Type t1, Type t2)
        {
            if (t1 == t2)
            {
                return true;
            }
            TypeBuilder builder = null;
            TypeBuilder builder2 = null;
            Type runtimeType = null;
            Type type2 = null;
            if (t1 is TypeBuilder)
            {
                builder = (TypeBuilder) t1;
                runtimeType = builder.m_runtimeType;
            }
            else
            {
                runtimeType = t1;
            }
            if (t2 is TypeBuilder)
            {
                builder2 = (TypeBuilder) t2;
                type2 = builder2.m_runtimeType;
            }
            else
            {
                type2 = t2;
            }
            return ((((builder != null) && (builder2 != null)) && (builder == builder2)) || (((runtimeType != null) && (type2 != null)) && (runtimeType == type2)));
        }

        public override Type MakeArrayType() => 
            SymbolType.FormCompoundType("[]".ToCharArray(), this, 0);

        public override Type MakeArrayType(int rank)
        {
            if (rank <= 0)
            {
                throw new IndexOutOfRangeException();
            }
            string str = "";
            if (rank == 1)
            {
                str = "*";
            }
            else
            {
                for (int i = 1; i < rank; i++)
                {
                    str = str + ",";
                }
            }
            return SymbolType.FormCompoundType(string.Format(CultureInfo.InvariantCulture, "[{0}]", new object[] { str }).ToCharArray(), this, 0);
        }

        public override Type MakeByRefType() => 
            SymbolType.FormCompoundType("&".ToCharArray(), this, 0);

        public override Type MakeGenericType(params Type[] typeArguments)
        {
            this.CheckContext(typeArguments);
            if (!this.IsGenericTypeDefinition)
            {
                throw new InvalidOperationException();
            }
            return new TypeBuilderInstantiation(this, typeArguments);
        }

        public override Type MakePointerType() => 
            SymbolType.FormCompoundType("*".ToCharArray(), this, 0);

        internal static void SetConstantValue(System.Reflection.Module module, int tk, Type destType, object value)
        {
            Variant variant;
            if (value == null)
            {
                if (destType.IsValueType)
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_ConstantNull"));
                }
            }
            else
            {
                Type type = value.GetType();
                if (destType.IsEnum)
                {
                    if (destType.UnderlyingSystemType != type)
                    {
                        throw new ArgumentException(Environment.GetResourceString("Argument_ConstantDoesntMatch"));
                    }
                }
                else
                {
                    if (destType != type)
                    {
                        throw new ArgumentException(Environment.GetResourceString("Argument_ConstantDoesntMatch"));
                    }
                    switch (Type.GetTypeCode(type))
                    {
                        case TypeCode.Boolean:
                        case TypeCode.Char:
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                        case TypeCode.String:
                            goto Label_00C9;
                    }
                    if (type != typeof(DateTime))
                    {
                        throw new ArgumentException(Environment.GetResourceString("Argument_ConstantNotSupported"));
                    }
                }
            }
        Label_00C9:
            variant = new Variant(value);
            InternalSetConstantValue(module.InternalModule, tk, ref variant);
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            this.ThrowIfGeneric();
            if (customBuilder == null)
            {
                throw new ArgumentNullException("customBuilder");
            }
            customBuilder.CreateCustomAttribute(this.m_module, this.m_tdType.Token);
        }

        [ComVisible(true)]
        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
        {
            this.ThrowIfGeneric();
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }
            if (binaryAttribute == null)
            {
                throw new ArgumentNullException("binaryAttribute");
            }
            InternalCreateCustomAttribute(this.m_tdType.Token, this.m_module.GetConstructorToken(con).Token, binaryAttribute, this.m_module, false);
        }

        internal void SetInterfaces(params Type[] interfaces)
        {
            this.ThrowIfCreated();
            if (interfaces == null)
            {
                this.m_typeInterfaces = new Type[0];
            }
            else
            {
                this.m_typeInterfaces = new Type[interfaces.Length];
                Array.Copy(interfaces, this.m_typeInterfaces, interfaces.Length);
            }
        }

        public void SetParent(Type parent)
        {
            this.ThrowIfGeneric();
            this.ThrowIfCreated();
            this.CheckContext(new Type[] { parent });
            if (parent != null)
            {
                this.m_typeParent = parent;
            }
            else if ((this.m_iAttr & TypeAttributes.ClassSemanticsMask) != TypeAttributes.ClassSemanticsMask)
            {
                this.m_typeParent = typeof(object);
            }
            else
            {
                if ((this.m_iAttr & TypeAttributes.Abstract) == TypeAttributes.AnsiClass)
                {
                    throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadInterfaceNotAbstract"));
                }
                this.m_typeParent = null;
            }
        }

        void _TypeBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
        {
            throw new NotImplementedException();
        }

        void _TypeBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
        {
            throw new NotImplementedException();
        }

        void _TypeBuilder.GetTypeInfoCount(out uint pcTInfo)
        {
            throw new NotImplementedException();
        }

        void _TypeBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
        {
            throw new NotImplementedException();
        }

        private Type TermCreateClass(int handle, System.Reflection.Module module) => 
            this._TermCreateClass(handle, module.InternalModule);

        internal void ThrowIfCreated()
        {
            if (this.IsCreated())
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TypeHasBeenCreated"));
            }
        }

        internal void ThrowIfGeneric()
        {
            if (this.IsGenericType && !this.IsGenericTypeDefinition)
            {
                throw new InvalidOperationException();
            }
        }

        public override string ToString() => 
            TypeNameBuilder.ToString(this, TypeNameBuilder.Format.ToString);

        private void VerifyTypeAttributes(TypeAttributes attr)
        {
            if (this.DeclaringType == null)
            {
                if (((attr & TypeAttributes.NestedFamORAssem) != TypeAttributes.AnsiClass) && ((attr & TypeAttributes.NestedFamORAssem) != TypeAttributes.Public))
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeAttrNestedVisibilityOnNonNestedType"));
                }
            }
            else if (((attr & TypeAttributes.NestedFamORAssem) == TypeAttributes.AnsiClass) || ((attr & TypeAttributes.NestedFamORAssem) == TypeAttributes.Public))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeAttrNonNestedVisibilityNestedType"));
            }
            if ((((attr & TypeAttributes.LayoutMask) != TypeAttributes.AnsiClass) && ((attr & TypeAttributes.LayoutMask) != TypeAttributes.SequentialLayout)) && ((attr & TypeAttributes.LayoutMask) != TypeAttributes.ExplicitLayout))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeAttrInvalidLayout"));
            }
            if ((attr & TypeAttributes.ReservedMask) != TypeAttributes.AnsiClass)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_BadTypeAttrReservedBitsSet"));
            }
        }

        public override System.Reflection.Assembly Assembly =>
            this.m_module.Assembly;

        public override string AssemblyQualifiedName =>
            TypeNameBuilder.ToString(this, TypeNameBuilder.Format.AssemblyQualifiedName);

        public override Type BaseType =>
            this.m_typeParent;

        public override MethodBase DeclaringMethod =>
            this.m_declMeth;

        public override Type DeclaringType =>
            this.m_DeclaringType;

        public override string FullName
        {
            get
            {
                if (this.m_strFullQualName == null)
                {
                    this.m_strFullQualName = TypeNameBuilder.ToString(this, TypeNameBuilder.Format.FullName);
                }
                return this.m_strFullQualName;
            }
        }

        public override System.Reflection.GenericParameterAttributes GenericParameterAttributes =>
            this.m_genParamAttributes;

        public override int GenericParameterPosition =>
            this.m_genParamPos;

        public override Guid GUID =>
            this.m_runtimeType?.GUID;

        public override bool IsGenericParameter =>
            this.m_bIsGenParam;

        public override bool IsGenericType =>
            (this.m_inst != null);

        public override bool IsGenericTypeDefinition =>
            this.m_bIsGenTypeDef;

        internal override int MetadataTokenInternal =>
            this.m_tdType.Token;

        public override System.Reflection.Module Module =>
            this.m_module;

        public override string Name =>
            this.m_strName;

        public override string Namespace =>
            this.m_strNameSpace;

        public System.Reflection.Emit.PackingSize PackingSize =>
            this.m_iPackingSize;

        public override Type ReflectedType =>
            this.m_DeclaringType;

        public int Size =>
            this.m_iTypeSize;

        public override RuntimeTypeHandle TypeHandle
        {
            get
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
            }
        }

        public System.Reflection.Emit.TypeToken TypeToken
        {
            get
            {
                if (this.IsGenericParameter)
                {
                    this.ThrowIfCreated();
                }
                return this.m_tdType;
            }
        }

        public override Type UnderlyingSystemType
        {
            get
            {
                if (this.m_runtimeType != null)
                {
                    return this.m_runtimeType.UnderlyingSystemType;
                }
                if (!base.IsEnum)
                {
                    return this;
                }
                if (this.m_underlyingSystemType == null)
                {
                    throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NoUnderlyingTypeOnEnum"));
                }
                return this.m_underlyingSystemType;
            }
        }

        internal class CustAttr
        {
            private byte[] m_binaryAttribute;
            private ConstructorInfo m_con;
            private CustomAttributeBuilder m_customBuilder;

            public CustAttr(CustomAttributeBuilder customBuilder)
            {
                if (customBuilder == null)
                {
                    throw new ArgumentNullException("customBuilder");
                }
                this.m_customBuilder = customBuilder;
            }

            public CustAttr(ConstructorInfo con, byte[] binaryAttribute)
            {
                if (con == null)
                {
                    throw new ArgumentNullException("con");
                }
                if (binaryAttribute == null)
                {
                    throw new ArgumentNullException("binaryAttribute");
                }
                this.m_con = con;
                this.m_binaryAttribute = binaryAttribute;
            }

            public void Bake(ModuleBuilder module, int token)
            {
                if (this.m_customBuilder == null)
                {
                    TypeBuilder.InternalCreateCustomAttribute(token, module.GetConstructorToken(this.m_con).Token, this.m_binaryAttribute, module, false);
                }
                else
                {
                    this.m_customBuilder.CreateCustomAttribute(module, token);
                }
            }
        }
    }
}

