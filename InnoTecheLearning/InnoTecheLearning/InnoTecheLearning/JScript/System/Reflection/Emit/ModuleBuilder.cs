namespace System.Reflection.Emit
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.SymbolStore;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;

    [ClassInterface(ClassInterfaceType.None), ComDefaultInterface(typeof(_ModuleBuilder)), ComVisible(true), HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort=true)]
    public class ModuleBuilder : Module, _ModuleBuilder
    {
        private AssemblyBuilder m_assemblyBuilder;
        internal ModuleBuilder m_internalModuleBuilder;
        private static readonly Dictionary<ModuleBuilder, ModuleBuilder> s_moduleBuilders = new Dictionary<ModuleBuilder, ModuleBuilder>();

        internal ModuleBuilder(AssemblyBuilder assemblyBuilder, ModuleBuilder internalModuleBuilder)
        {
            this.m_internalModuleBuilder = internalModuleBuilder;
            this.m_assemblyBuilder = assemblyBuilder;
            lock (s_moduleBuilders)
            {
                s_moduleBuilders[internalModuleBuilder] = this;
            }
        }

        internal void CheckContext(params Type[][] typess)
        {
            ((AssemblyBuilder) base.Assembly).CheckContext(typess);
        }

        internal void CheckContext(params Type[] types)
        {
            ((AssemblyBuilder) base.Assembly).CheckContext(types);
        }

        public void CreateGlobalFunctions()
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    this.CreateGlobalFunctionsNoLock();
                    return;
                }
            }
            this.CreateGlobalFunctionsNoLock();
        }

        private void CreateGlobalFunctionsNoLock()
        {
            if (base.m_moduleData.m_fGlobalBeenCreated)
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotADebugModule"));
            }
            base.m_moduleData.m_globalTypeBuilder.CreateType();
            base.m_moduleData.m_fGlobalBeenCreated = true;
        }

        public ISymbolDocumentWriter DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.DefineDocumentNoLock(url, language, languageVendor, documentType);
                }
            }
            return this.DefineDocumentNoLock(url, language, languageVendor, documentType);
        }

        private ISymbolDocumentWriter DefineDocumentNoLock(string url, Guid language, Guid languageVendor, Guid documentType) => 
            base.m_iSymWriter?.DefineDocument(url, language, languageVendor, documentType);

        public EnumBuilder DefineEnum(string name, TypeAttributes visibility, Type underlyingType)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            this.CheckContext(new Type[] { underlyingType });
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.DefineEnumNoLock(name, visibility, underlyingType);
                }
            }
            return this.DefineEnumNoLock(name, visibility, underlyingType);
        }

        private EnumBuilder DefineEnumNoLock(string name, TypeAttributes visibility, Type underlyingType)
        {
            EnumBuilder builder = new EnumBuilder(name, underlyingType, visibility, this);
            base.m_TypeBuilderList.Add(builder);
            return builder;
        }

        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, Type returnType, Type[] parameterTypes) => 
            this.DefineGlobalMethod(name, attributes, CallingConventions.Standard, returnType, parameterTypes);

        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes) => 
            this.DefineGlobalMethod(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);

        public MethodBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.DefineGlobalMethodNoLock(name, attributes, callingConvention, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
                }
            }
            return this.DefineGlobalMethodNoLock(name, attributes, callingConvention, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
        }

        private MethodBuilder DefineGlobalMethodNoLock(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
        {
            this.CheckContext(new Type[] { returnType });
            this.CheckContext(new Type[][] { requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes });
            this.CheckContext(requiredParameterTypeCustomModifiers);
            this.CheckContext(optionalParameterTypeCustomModifiers);
            if (base.m_moduleData.m_fGlobalBeenCreated)
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_GlobalsHaveBeenCreated"));
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
            }
            if ((attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_GlobalFunctionHasToBeStatic"));
            }
            base.m_moduleData.m_fHasGlobal = true;
            return base.m_moduleData.m_globalTypeBuilder.DefineMethod(name, attributes, callingConvention, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
        }

        public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.DefineInitializedDataNoLock(name, data, attributes);
                }
            }
            return this.DefineInitializedDataNoLock(name, data, attributes);
        }

        private FieldBuilder DefineInitializedDataNoLock(string name, byte[] data, FieldAttributes attributes)
        {
            if (base.m_moduleData.m_fGlobalBeenCreated)
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_GlobalsHaveBeenCreated"));
            }
            base.m_moduleData.m_fHasGlobal = true;
            return base.m_moduleData.m_globalTypeBuilder.DefineInitializedData(name, data, attributes);
        }

        public void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    this.DefineManifestResourceNoLock(name, stream, attribute);
                    return;
                }
            }
            this.DefineManifestResourceNoLock(name, stream, attribute);
        }

        private void DefineManifestResourceNoLock(string name, Stream stream, ResourceAttributes attribute)
        {
            if (this.IsTransient())
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
            }
            Assembly assembly = base.Assembly;
            if (!(assembly is AssemblyBuilder))
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
            }
            AssemblyBuilder builder = (AssemblyBuilder) assembly;
            if (!builder.IsPersistable())
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
            }
            builder.m_assemblyData.CheckResNameConflict(name);
            ResWriterData data = new ResWriterData(null, stream, name, string.Empty, string.Empty, attribute) {
                m_nextResWriter = base.m_moduleData.m_embeddedRes
            };
            base.m_moduleData.m_embeddedRes = data;
        }

        public MethodBuilder DefinePInvokeMethod(string name, string dllName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet) => 
            this.DefinePInvokeMethod(name, dllName, name, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);

        public MethodBuilder DefinePInvokeMethod(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.DefinePInvokeMethodNoLock(name, dllName, entryName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
                }
            }
            return this.DefinePInvokeMethodNoLock(name, dllName, entryName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
        }

        private MethodBuilder DefinePInvokeMethodNoLock(string name, string dllName, string entryName, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, CallingConvention nativeCallConv, CharSet nativeCharSet)
        {
            this.CheckContext(new Type[] { returnType });
            this.CheckContext(parameterTypes);
            if ((attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_GlobalFunctionHasToBeStatic"));
            }
            base.m_moduleData.m_fHasGlobal = true;
            return base.m_moduleData.m_globalTypeBuilder.DefinePInvokeMethod(name, dllName, entryName, attributes, callingConvention, returnType, parameterTypes, nativeCallConv, nativeCharSet);
        }

        public IResourceWriter DefineResource(string name, string description) => 
            this.DefineResource(name, description, ResourceAttributes.Public);

        public IResourceWriter DefineResource(string name, string description, ResourceAttributes attribute)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.DefineResourceNoLock(name, description, attribute);
                }
            }
            return this.DefineResourceNoLock(name, description, attribute);
        }

        private IResourceWriter DefineResourceNoLock(string name, string description, ResourceAttributes attribute)
        {
            if (this.IsTransient())
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
            }
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
            }
            Assembly assembly = base.Assembly;
            if (!(assembly is AssemblyBuilder))
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
            }
            AssemblyBuilder builder = (AssemblyBuilder) assembly;
            if (!builder.IsPersistable())
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadResourceContainer"));
            }
            builder.m_assemblyData.CheckResNameConflict(name);
            MemoryStream stream = new MemoryStream();
            ResourceWriter resWriter = new ResourceWriter(stream);
            ResWriterData data = new ResWriterData(resWriter, stream, name, string.Empty, string.Empty, attribute) {
                m_nextResWriter = base.m_moduleData.m_embeddedRes
            };
            base.m_moduleData.m_embeddedRes = data;
            return resWriter;
        }

        public TypeBuilder DefineType(string name)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.DefineTypeNoLock(name);
                }
            }
            return this.DefineTypeNoLock(name);
        }

        public TypeBuilder DefineType(string name, TypeAttributes attr)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.DefineTypeNoLock(name, attr);
                }
            }
            return this.DefineTypeNoLock(name, attr);
        }

        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.DefineTypeNoLock(name, attr, parent);
                }
            }
            return this.DefineTypeNoLock(name, attr, parent);
        }

        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, int typesize)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.DefineTypeNoLock(name, attr, parent, typesize);
                }
            }
            return this.DefineTypeNoLock(name, attr, parent, typesize);
        }

        [ComVisible(true)]
        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.DefineTypeNoLock(name, attr, parent, interfaces);
                }
            }
            return this.DefineTypeNoLock(name, attr, parent, interfaces);
        }

        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packsize)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.DefineTypeNoLock(name, attr, parent, packsize);
                }
            }
            return this.DefineTypeNoLock(name, attr, parent, packsize);
        }

        public TypeBuilder DefineType(string name, TypeAttributes attr, Type parent, PackingSize packingSize, int typesize)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.DefineTypeNoLock(name, attr, parent, packingSize, typesize);
                }
            }
            return this.DefineTypeNoLock(name, attr, parent, packingSize, typesize);
        }

        private TypeBuilder DefineTypeNoLock(string name)
        {
            TypeBuilder builder = new TypeBuilder(name, TypeAttributes.AnsiClass, null, null, this, PackingSize.Unspecified, null);
            base.m_TypeBuilderList.Add(builder);
            return builder;
        }

        private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr)
        {
            TypeBuilder builder = new TypeBuilder(name, attr, null, null, this, PackingSize.Unspecified, null);
            base.m_TypeBuilderList.Add(builder);
            return builder;
        }

        private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent)
        {
            this.CheckContext(new Type[] { parent });
            TypeBuilder builder = new TypeBuilder(name, attr, parent, null, this, PackingSize.Unspecified, null);
            base.m_TypeBuilderList.Add(builder);
            return builder;
        }

        private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent, int typesize)
        {
            TypeBuilder builder = new TypeBuilder(name, attr, parent, this, PackingSize.Unspecified, typesize, null);
            base.m_TypeBuilderList.Add(builder);
            return builder;
        }

        private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent, Type[] interfaces)
        {
            TypeBuilder builder = new TypeBuilder(name, attr, parent, interfaces, this, PackingSize.Unspecified, null);
            base.m_TypeBuilderList.Add(builder);
            return builder;
        }

        private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent, PackingSize packsize)
        {
            TypeBuilder builder = new TypeBuilder(name, attr, parent, null, this, packsize, null);
            base.m_TypeBuilderList.Add(builder);
            return builder;
        }

        private TypeBuilder DefineTypeNoLock(string name, TypeAttributes attr, Type parent, PackingSize packingSize, int typesize)
        {
            TypeBuilder builder = new TypeBuilder(name, attr, parent, this, packingSize, typesize, null);
            base.m_TypeBuilderList.Add(builder);
            return builder;
        }

        public FieldBuilder DefineUninitializedData(string name, int size, FieldAttributes attributes)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.DefineUninitializedDataNoLock(name, size, attributes);
                }
            }
            return this.DefineUninitializedDataNoLock(name, size, attributes);
        }

        private FieldBuilder DefineUninitializedDataNoLock(string name, int size, FieldAttributes attributes)
        {
            if (base.m_moduleData.m_fGlobalBeenCreated)
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_GlobalsHaveBeenCreated"));
            }
            base.m_moduleData.m_fHasGlobal = true;
            return base.m_moduleData.m_globalTypeBuilder.DefineUninitializedData(name, size, attributes);
        }

        public void DefineUnmanagedResource(byte[] resource)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    this.DefineUnmanagedResourceInternalNoLock(resource);
                    return;
                }
            }
            this.DefineUnmanagedResourceInternalNoLock(resource);
        }

        public void DefineUnmanagedResource(string resourceFileName)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    this.DefineUnmanagedResourceFileInternalNoLock(resourceFileName);
                    return;
                }
            }
            this.DefineUnmanagedResourceFileInternalNoLock(resourceFileName);
        }

        internal void DefineUnmanagedResourceFileInternalNoLock(string resourceFileName)
        {
            if ((base.m_moduleData.m_resourceBytes != null) || (base.m_moduleData.m_strResourceFileName != null))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
            }
            if (resourceFileName == null)
            {
                throw new ArgumentNullException("resourceFileName");
            }
            string fullPath = Path.GetFullPath(resourceFileName);
            new FileIOPermission(FileIOPermissionAccess.Read, fullPath).Demand();
            new EnvironmentPermission(PermissionState.Unrestricted).Assert();
            try
            {
                if (!File.Exists(resourceFileName))
                {
                    throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.FileNotFound_FileName"), new object[] { resourceFileName }), resourceFileName);
                }
            }
            finally
            {
                CodeAccessPermission.RevertAssert();
            }
            base.m_moduleData.m_strResourceFileName = fullPath;
        }

        internal void DefineUnmanagedResourceInternalNoLock(byte[] resource)
        {
            if ((base.m_moduleData.m_strResourceFileName != null) || (base.m_moduleData.m_resourceBytes != null))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
            }
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            base.m_moduleData.m_resourceBytes = new byte[resource.Length];
            Array.Copy(resource, base.m_moduleData.m_resourceBytes, resource.Length);
        }

        private void DemandGrantedAssemblyPermission()
        {
            ((AssemblyBuilder) base.Assembly).DemandGrantedPermission();
        }

        internal virtual Type FindTypeBuilderWithName(string strTypeName, bool ignoreCase)
        {
            int count = base.m_TypeBuilderList.Count;
            Type type = null;
            int num2 = 0;
            while (num2 < count)
            {
                type = (Type) base.m_TypeBuilderList[num2];
                if (ignoreCase)
                {
                    if (string.Compare(type.FullName, strTypeName, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) == 0)
                    {
                        break;
                    }
                }
                else if (type.FullName.Equals(strTypeName))
                {
                    break;
                }
                num2++;
            }
            if (num2 == count)
            {
                type = null;
            }
            return type;
        }

        public MethodInfo GetArrayMethod(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            this.CheckContext(new Type[] { returnType, arrayClass });
            this.CheckContext(parameterTypes);
            return new SymbolMethod(this, this.GetArrayMethodToken(arrayClass, methodName, callingConvention, returnType, parameterTypes), arrayClass, methodName, callingConvention, returnType, parameterTypes);
        }

        public MethodToken GetArrayMethodToken(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.GetArrayMethodTokenNoLock(arrayClass, methodName, callingConvention, returnType, parameterTypes);
                }
            }
            return this.GetArrayMethodTokenNoLock(arrayClass, methodName, callingConvention, returnType, parameterTypes);
        }

        private MethodToken GetArrayMethodTokenNoLock(Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
        {
            int num2;
            this.CheckContext(new Type[] { returnType, arrayClass });
            this.CheckContext(parameterTypes);
            if (arrayClass == null)
            {
                throw new ArgumentNullException("arrayClass");
            }
            if (methodName == null)
            {
                throw new ArgumentNullException("methodName");
            }
            if (methodName.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "methodName");
            }
            if (!arrayClass.IsArray)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_HasToBeArrayClass"));
            }
            byte[] signature = SignatureHelper.GetMethodSigHelper(this, callingConvention, returnType, null, null, parameterTypes, null, null).InternalGetSignature(out num2);
            Type elementType = arrayClass;
            while (elementType.IsArray)
            {
                elementType = elementType.GetElementType();
            }
            int baseToken = this.GetTypeTokenInternal(elementType).Token;
            TypeToken typeTokenInternal = this.GetTypeTokenInternal(arrayClass);
            return new MethodToken(base.nativeGetArrayMethodToken(typeTokenInternal.Token, methodName, signature, num2, baseToken));
        }

        internal override Assembly GetAssemblyInternal()
        {
            if (!this.IsInternal)
            {
                return this.m_assemblyBuilder;
            }
            return base._GetAssemblyInternal();
        }

        [ComVisible(true)]
        public MethodToken GetConstructorToken(ConstructorInfo con) => 
            this.InternalGetConstructorToken(con, false);

        public FieldToken GetFieldToken(FieldInfo field)
        {
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.GetFieldTokenNoLock(field);
                }
            }
            return this.GetFieldTokenNoLock(field);
        }

        private FieldToken GetFieldTokenNoLock(FieldInfo field)
        {
            int typeSpecTokenWithBytes;
            int num2 = 0;
            if (field == null)
            {
                throw new ArgumentNullException("con");
            }
            if (field is FieldBuilder)
            {
                FieldBuilder builder = (FieldBuilder) field;
                if ((field.DeclaringType != null) && field.DeclaringType.IsGenericType)
                {
                    int num3;
                    byte[] signature = SignatureHelper.GetTypeSigToken(this, field.DeclaringType).InternalGetSignature(out num3);
                    typeSpecTokenWithBytes = base.InternalGetTypeSpecTokenWithBytes(signature, num3);
                    num2 = base.InternalGetMemberRef(this, typeSpecTokenWithBytes, builder.GetToken().Token);
                }
                else
                {
                    if (builder.GetTypeBuilder().Module.InternalModule.Equals(this.InternalModule))
                    {
                        return builder.GetToken();
                    }
                    if (field.DeclaringType == null)
                    {
                        throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotImportGlobalFromDifferentModule"));
                    }
                    typeSpecTokenWithBytes = this.GetTypeTokenInternal(field.DeclaringType).Token;
                    num2 = base.InternalGetMemberRef(field.ReflectedType.Module, typeSpecTokenWithBytes, builder.GetToken().Token);
                }
            }
            else if (field is RuntimeFieldInfo)
            {
                if (field.DeclaringType == null)
                {
                    throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotImportGlobalFromDifferentModule"));
                }
                if ((field.DeclaringType != null) && field.DeclaringType.IsGenericType)
                {
                    int num4;
                    byte[] buffer2 = SignatureHelper.GetTypeSigToken(this, field.DeclaringType).InternalGetSignature(out num4);
                    typeSpecTokenWithBytes = base.InternalGetTypeSpecTokenWithBytes(buffer2, num4);
                    num2 = base.InternalGetMemberRefOfFieldInfo(typeSpecTokenWithBytes, field.DeclaringType.GetTypeHandleInternal(), field.MetadataTokenInternal);
                }
                else
                {
                    typeSpecTokenWithBytes = this.GetTypeTokenInternal(field.DeclaringType).Token;
                    num2 = base.InternalGetMemberRefOfFieldInfo(typeSpecTokenWithBytes, field.DeclaringType.GetTypeHandleInternal(), field.MetadataTokenInternal);
                }
            }
            else if (field is FieldOnTypeBuilderInstantiation)
            {
                int num5;
                FieldInfo fieldInfo = ((FieldOnTypeBuilderInstantiation) field).FieldInfo;
                byte[] buffer3 = SignatureHelper.GetTypeSigToken(this, field.DeclaringType).InternalGetSignature(out num5);
                typeSpecTokenWithBytes = base.InternalGetTypeSpecTokenWithBytes(buffer3, num5);
                num2 = base.InternalGetMemberRef(fieldInfo.ReflectedType.Module, typeSpecTokenWithBytes, fieldInfo.MetadataTokenInternal);
            }
            else
            {
                int num6;
                typeSpecTokenWithBytes = this.GetTypeTokenInternal(field.ReflectedType).Token;
                SignatureHelper fieldSigHelper = SignatureHelper.GetFieldSigHelper(this);
                fieldSigHelper.AddArgument(field.FieldType, field.GetRequiredCustomModifiers(), field.GetOptionalCustomModifiers());
                byte[] buffer4 = fieldSigHelper.InternalGetSignature(out num6);
                num2 = base.InternalGetMemberRefFromSignature(typeSpecTokenWithBytes, field.Name, buffer4, num6);
            }
            return new FieldToken(num2, field.GetType());
        }

        internal SignatureHelper GetMemberRefSignature(CallingConventions call, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes, int cGenericParameters)
        {
            int length;
            int num2;
            if (parameterTypes == null)
            {
                length = 0;
            }
            else
            {
                length = parameterTypes.Length;
            }
            SignatureHelper helper = SignatureHelper.GetMethodSigHelper(this, call, returnType, cGenericParameters);
            for (num2 = 0; num2 < length; num2++)
            {
                helper.AddArgument(parameterTypes[num2]);
            }
            if ((optionalParameterTypes != null) && (optionalParameterTypes.Length != 0))
            {
                helper.AddSentinel();
                for (num2 = 0; num2 < optionalParameterTypes.Length; num2++)
                {
                    helper.AddArgument(optionalParameterTypes[num2]);
                }
            }
            return helper;
        }

        internal int GetMemberRefToken(MethodBase method, Type[] optionalParameterTypes)
        {
            Type[] parameterTypes;
            Type returnType;
            int typeSpecTokenWithBytes;
            int num4;
            int cGenericParameters = 0;
            if (method.IsGenericMethod)
            {
                if (!method.IsGenericMethodDefinition)
                {
                    throw new InvalidOperationException();
                }
                cGenericParameters = method.GetGenericArguments().Length;
            }
            if ((optionalParameterTypes != null) && ((method.CallingConvention & CallingConventions.VarArgs) == 0))
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAVarArgCallingConvention"));
            }
            MethodInfo info = method as MethodInfo;
            if (method.DeclaringType.IsGenericType)
            {
                MethodBase ctor = null;
                MethodOnTypeBuilderInstantiation instantiation = method as MethodOnTypeBuilderInstantiation;
                if (instantiation != null)
                {
                    ctor = instantiation.m_method;
                }
                else
                {
                    ConstructorOnTypeBuilderInstantiation instantiation2 = method as ConstructorOnTypeBuilderInstantiation;
                    if (instantiation2 != null)
                    {
                        ctor = instantiation2.m_ctor;
                    }
                    else if ((method is MethodBuilder) || (method is ConstructorBuilder))
                    {
                        ctor = method;
                    }
                    else if (method.IsGenericMethod)
                    {
                        ctor = info.GetGenericMethodDefinition();
                        ctor = ctor.Module.ResolveMethod(ctor.MetadataTokenInternal, ctor.GetGenericArguments(), ctor.DeclaringType?.GetGenericArguments());
                    }
                    else
                    {
                        ctor = method;
                        ctor = method.Module.ResolveMethod(method.MetadataTokenInternal, null, ctor.DeclaringType?.GetGenericArguments());
                    }
                }
                parameterTypes = ctor.GetParameterTypes();
                returnType = ctor.GetReturnType();
            }
            else
            {
                parameterTypes = method.GetParameterTypes();
                returnType = method.GetReturnType();
            }
            if (method.DeclaringType.IsGenericType)
            {
                int num3;
                byte[] buffer = SignatureHelper.GetTypeSigToken(this, method.DeclaringType).InternalGetSignature(out num3);
                typeSpecTokenWithBytes = base.InternalGetTypeSpecTokenWithBytes(buffer, num3);
            }
            else if (method.Module.InternalModule != this.InternalModule)
            {
                typeSpecTokenWithBytes = this.GetTypeToken(method.DeclaringType).Token;
            }
            else if (info != null)
            {
                typeSpecTokenWithBytes = this.GetMethodToken(method as MethodInfo).Token;
            }
            else
            {
                typeSpecTokenWithBytes = this.GetConstructorToken(method as ConstructorInfo).Token;
            }
            byte[] signature = this.GetMemberRefSignature(method.CallingConvention, returnType, parameterTypes, optionalParameterTypes, cGenericParameters).InternalGetSignature(out num4);
            return base.InternalGetMemberRefFromSignature(typeSpecTokenWithBytes, method.Name, signature, num4);
        }

        public MethodToken GetMethodToken(MethodInfo method)
        {
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.GetMethodTokenNoLock(method, true);
                }
            }
            return this.GetMethodTokenNoLock(method, true);
        }

        internal MethodToken GetMethodTokenInternal(MethodInfo method)
        {
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.GetMethodTokenNoLock(method, false);
                }
            }
            return this.GetMethodTokenNoLock(method, false);
        }

        private MethodToken GetMethodTokenNoLock(MethodInfo method, bool getGenericTypeDefinition)
        {
            int num;
            int str = 0;
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }
            if (method is MethodBuilder)
            {
                if (method.Module.InternalModule == this.InternalModule)
                {
                    return new MethodToken(method.MetadataTokenInternal);
                }
                if (method.DeclaringType == null)
                {
                    throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotImportGlobalFromDifferentModule"));
                }
                num = getGenericTypeDefinition ? this.GetTypeToken(method.DeclaringType).Token : this.GetTypeTokenInternal(method.DeclaringType).Token;
                str = base.InternalGetMemberRef(method.DeclaringType.Module, num, method.MetadataTokenInternal);
            }
            else
            {
                if (method is MethodOnTypeBuilderInstantiation)
                {
                    return new MethodToken(this.GetMemberRefToken(method, null));
                }
                if (method is SymbolMethod)
                {
                    SymbolMethod method2 = method as SymbolMethod;
                    if (method2.GetModule() == this)
                    {
                        return method2.GetToken();
                    }
                    return method2.GetToken(this);
                }
                Type declaringType = method.DeclaringType;
                if (declaringType == null)
                {
                    throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotImportGlobalFromDifferentModule"));
                }
                if (declaringType.IsArray)
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    Type[] parameterTypes = new Type[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameterTypes[i] = parameters[i].ParameterType;
                    }
                    return this.GetArrayMethodToken(declaringType, method.Name, method.CallingConvention, method.ReturnType, parameterTypes);
                }
                if (method is RuntimeMethodInfo)
                {
                    num = getGenericTypeDefinition ? this.GetTypeToken(method.DeclaringType).Token : this.GetTypeTokenInternal(method.DeclaringType).Token;
                    str = base.InternalGetMemberRefOfMethodInfo(num, method.GetMethodHandle());
                }
                else
                {
                    SignatureHelper helper;
                    int num5;
                    ParameterInfo[] infoArray2 = method.GetParameters();
                    Type[] typeArray2 = new Type[infoArray2.Length];
                    Type[][] requiredParameterTypeCustomModifiers = new Type[typeArray2.Length][];
                    Type[][] optionalParameterTypeCustomModifiers = new Type[typeArray2.Length][];
                    for (int j = 0; j < infoArray2.Length; j++)
                    {
                        typeArray2[j] = infoArray2[j].ParameterType;
                        requiredParameterTypeCustomModifiers[j] = infoArray2[j].GetRequiredCustomModifiers();
                        optionalParameterTypeCustomModifiers[j] = infoArray2[j].GetOptionalCustomModifiers();
                    }
                    num = getGenericTypeDefinition ? this.GetTypeToken(method.DeclaringType).Token : this.GetTypeTokenInternal(method.DeclaringType).Token;
                    try
                    {
                        helper = SignatureHelper.GetMethodSigHelper(this, method.CallingConvention, method.ReturnType, method.ReturnParameter.GetRequiredCustomModifiers(), method.ReturnParameter.GetOptionalCustomModifiers(), typeArray2, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
                    }
                    catch (NotImplementedException)
                    {
                        helper = SignatureHelper.GetMethodSigHelper(this, method.ReturnType, typeArray2);
                    }
                    byte[] signature = helper.InternalGetSignature(out num5);
                    str = base.InternalGetMemberRefFromSignature(num, method.Name, signature, num5);
                }
            }
            return new MethodToken(str);
        }

        internal static Module GetModuleBuilder(Module module)
        {
            ModuleBuilder internalModule = module.InternalModule as ModuleBuilder;
            if (internalModule == null)
            {
                return module;
            }
            ModuleBuilder builder2 = null;
            lock (s_moduleBuilders)
            {
                if (s_moduleBuilders.TryGetValue(internalModule, out builder2))
                {
                    return builder2;
                }
                return internalModule;
            }
        }

        internal Type GetRootElementType(Type type)
        {
            if ((!type.IsByRef && !type.IsPointer) && !type.IsArray)
            {
                return type;
            }
            return this.GetRootElementType(type.GetElementType());
        }

        public SignatureToken GetSignatureToken(SignatureHelper sigHelper)
        {
            int num;
            if (sigHelper == null)
            {
                throw new ArgumentNullException("sigHelper");
            }
            byte[] signature = sigHelper.InternalGetSignature(out num);
            return new SignatureToken(TypeBuilder.InternalGetTokenFromSig(this, signature, num), this);
        }

        public SignatureToken GetSignatureToken(byte[] sigBytes, int sigLength)
        {
            byte[] destinationArray = null;
            if (sigBytes == null)
            {
                throw new ArgumentNullException("sigBytes");
            }
            destinationArray = new byte[sigBytes.Length];
            Array.Copy(sigBytes, destinationArray, sigBytes.Length);
            return new SignatureToken(TypeBuilder.InternalGetTokenFromSig(this, destinationArray, sigLength), this);
        }

        public StringToken GetStringConstant(string str) => 
            new StringToken(base.InternalGetStringConstant(str));

        public ISymbolWriter GetSymWriter()
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            return base.m_iSymWriter;
        }

        [ComVisible(true)]
        public override Type GetType(string className) => 
            this.GetType(className, false, false);

        [ComVisible(true)]
        public override Type GetType(string className, bool ignoreCase) => 
            this.GetType(className, false, ignoreCase);

        private Type GetType(string strFormat, Type baseType)
        {
            if ((strFormat != null) && !strFormat.Equals(string.Empty))
            {
                return SymbolType.FormCompoundType(strFormat.ToCharArray(), baseType, 0);
            }
            return baseType;
        }

        [ComVisible(true)]
        public override Type GetType(string className, bool throwOnError, bool ignoreCase)
        {
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.GetTypeNoLock(className, throwOnError, ignoreCase);
                }
            }
            return this.GetTypeNoLock(className, throwOnError, ignoreCase);
        }

        private Type GetTypeNoLock(string className, bool throwOnError, bool ignoreCase)
        {
            Type baseType = base.GetType(className, throwOnError, ignoreCase);
            if (baseType != null)
            {
                return baseType;
            }
            string str = null;
            string strFormat = null;
            int startIndex = 0;
            while (startIndex <= className.Length)
            {
                int length = className.IndexOfAny(new char[] { '[', '*', '&' }, startIndex);
                if (length == -1)
                {
                    str = className;
                    strFormat = null;
                    break;
                }
                int num3 = 0;
                for (int i = length - 1; (i >= 0) && (className[i] == '\\'); i--)
                {
                    num3++;
                }
                if ((num3 % 2) == 1)
                {
                    startIndex = length + 1;
                }
                else
                {
                    str = className.Substring(0, length);
                    strFormat = className.Substring(length);
                    break;
                }
            }
            if (str == null)
            {
                str = className;
                strFormat = null;
            }
            str = str.Replace(@"\\", @"\").Replace(@"\[", "[").Replace(@"\*", "*").Replace(@"\&", "&");
            if (strFormat != null)
            {
                baseType = base.GetType(str, false, ignoreCase);
            }
            bool flag = false;
            if (this.IsInternal)
            {
                try
                {
                    this.DemandGrantedAssemblyPermission();
                    flag = true;
                }
                catch (SecurityException)
                {
                    flag = false;
                }
            }
            else
            {
                flag = true;
            }
            if ((baseType == null) && flag)
            {
                baseType = this.FindTypeBuilderWithName(str, ignoreCase);
                if ((baseType == null) && (base.Assembly is AssemblyBuilder))
                {
                    ArrayList moduleBuilderList = base.Assembly.m_assemblyData.m_moduleBuilderList;
                    int count = moduleBuilderList.Count;
                    for (int j = 0; (j < count) && (baseType == null); j++)
                    {
                        baseType = ((ModuleBuilder) moduleBuilderList[j]).FindTypeBuilderWithName(str, ignoreCase);
                    }
                }
            }
            if (baseType == null)
            {
                return null;
            }
            if (strFormat == null)
            {
                return baseType;
            }
            return this.GetType(strFormat, baseType);
        }

        internal int GetTypeRefNested(Type type, Module refedModule, string strRefedModuleFileName)
        {
            Type declaringType = type.DeclaringType;
            int tkResolution = 0;
            string fullName = type.FullName;
            if (declaringType != null)
            {
                tkResolution = this.GetTypeRefNested(declaringType, refedModule, strRefedModuleFileName);
                fullName = UnmangleTypeName(fullName);
            }
            return base.InternalGetTypeToken(fullName, refedModule, strRefedModuleFileName, tkResolution);
        }

        public override Type[] GetTypes()
        {
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.GetTypesNoLock();
                }
            }
            return this.GetTypesNoLock();
        }

        internal Type[] GetTypesNoLock()
        {
            int count = base.m_TypeBuilderList.Count;
            List<Type> list = new List<Type>(count);
            bool flag = false;
            if (this.IsInternal)
            {
                try
                {
                    this.DemandGrantedAssemblyPermission();
                    flag = true;
                }
                catch (SecurityException)
                {
                    flag = false;
                }
            }
            else
            {
                flag = true;
            }
            for (int i = 0; i < count; i++)
            {
                TypeBuilder typeBuilder;
                EnumBuilder builder2 = base.m_TypeBuilderList[i] as EnumBuilder;
                if (builder2 != null)
                {
                    typeBuilder = builder2.m_typeBuilder;
                }
                else
                {
                    typeBuilder = base.m_TypeBuilderList[i] as TypeBuilder;
                }
                if (typeBuilder != null)
                {
                    if (typeBuilder.m_hasBeenCreated)
                    {
                        list.Add(typeBuilder.UnderlyingSystemType);
                    }
                    else if (flag)
                    {
                        list.Add(typeBuilder);
                    }
                }
                else
                {
                    list.Add((Type) base.m_TypeBuilderList[i]);
                }
            }
            return list.ToArray();
        }

        public TypeToken GetTypeToken(string name) => 
            this.GetTypeToken(base.GetType(name, false, true));

        public TypeToken GetTypeToken(Type type) => 
            this.GetTypeTokenInternal(type, true);

        internal TypeToken GetTypeTokenInternal(Type type) => 
            this.GetTypeTokenInternal(type, false);

        internal TypeToken GetTypeTokenInternal(Type type, bool getGenericDefinition)
        {
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    return this.GetTypeTokenWorkerNoLock(type, getGenericDefinition);
                }
            }
            return this.GetTypeTokenWorkerNoLock(type, getGenericDefinition);
        }

        private TypeToken GetTypeTokenWorkerNoLock(Type type, bool getGenericDefinition)
        {
            this.CheckContext(new Type[] { type });
            string strRefedModuleFileName = string.Empty;
            bool flag2 = false;
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            Module moduleBuilder = GetModuleBuilder(type.Module);
            bool flag = moduleBuilder.Equals(this);
            if (type.IsByRef)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_CannotGetTypeTokenForByRef"));
            }
            if ((type.IsGenericType && (!type.IsGenericTypeDefinition || !getGenericDefinition)) || ((type.IsGenericParameter || type.IsArray) || type.IsPointer))
            {
                int num;
                byte[] signature = SignatureHelper.GetTypeSigToken(this, type).InternalGetSignature(out num);
                return new TypeToken(base.InternalGetTypeSpecTokenWithBytes(signature, num));
            }
            if (flag)
            {
                TypeBuilder typeBuilder;
                EnumBuilder builder2 = type as EnumBuilder;
                if (builder2 != null)
                {
                    typeBuilder = builder2.m_typeBuilder;
                }
                else
                {
                    typeBuilder = type as TypeBuilder;
                }
                if (typeBuilder != null)
                {
                    return typeBuilder.TypeToken;
                }
                if (type is GenericTypeParameterBuilder)
                {
                    return new TypeToken(type.MetadataTokenInternal);
                }
                return new TypeToken(this.GetTypeRefNested(type, this, string.Empty));
            }
            ModuleBuilder builder3 = moduleBuilder as ModuleBuilder;
            if (builder3 != null)
            {
                if (builder3.IsTransient())
                {
                    flag2 = true;
                }
                strRefedModuleFileName = builder3.m_moduleData.m_strFileName;
            }
            else
            {
                strRefedModuleFileName = moduleBuilder.ScopeName;
            }
            if (!this.IsTransient() && flag2)
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadTransientModuleReference"));
            }
            return new TypeToken(this.GetTypeRefNested(type, moduleBuilder, strRefedModuleFileName));
        }

        internal void Init(string strModuleName, string strFileName, ISymbolWriter writer)
        {
            base.m_moduleData = new ModuleBuilderData(this, strModuleName, strFileName);
            base.m_TypeBuilderList = new ArrayList();
            base.m_iSymWriter = writer;
            if (writer != null)
            {
                new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
                writer.SetUnderlyingWriter(base.m_pInternalSymWriter);
            }
        }

        internal MethodToken InternalGetConstructorToken(ConstructorInfo con, bool usingRef)
        {
            int token;
            int str = 0;
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }
            if (con is ConstructorBuilder)
            {
                ConstructorBuilder builder = con as ConstructorBuilder;
                if (!usingRef && builder.ReflectedType.Module.InternalModule.Equals(this.InternalModule))
                {
                    return builder.GetToken();
                }
                token = this.GetTypeTokenInternal(con.ReflectedType).Token;
                str = base.InternalGetMemberRef(con.ReflectedType.Module, token, builder.GetToken().Token);
            }
            else if (con is ConstructorOnTypeBuilderInstantiation)
            {
                ConstructorOnTypeBuilderInstantiation instantiation = con as ConstructorOnTypeBuilderInstantiation;
                if (usingRef)
                {
                    throw new InvalidOperationException();
                }
                token = this.GetTypeTokenInternal(con.DeclaringType).Token;
                str = base.InternalGetMemberRef(con.DeclaringType.Module, token, instantiation.m_ctor.MetadataTokenInternal);
            }
            else if ((con is RuntimeConstructorInfo) && !con.ReflectedType.IsArray)
            {
                token = this.GetTypeTokenInternal(con.ReflectedType).Token;
                str = base.InternalGetMemberRefOfMethodInfo(token, con.GetMethodHandle());
            }
            else
            {
                int num4;
                ParameterInfo[] parameters = con.GetParameters();
                Type[] parameterTypes = new Type[parameters.Length];
                Type[][] requiredParameterTypeCustomModifiers = new Type[parameterTypes.Length][];
                Type[][] optionalParameterTypeCustomModifiers = new Type[parameterTypes.Length][];
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameterTypes[i] = parameters[i].ParameterType;
                    requiredParameterTypeCustomModifiers[i] = parameters[i].GetRequiredCustomModifiers();
                    optionalParameterTypeCustomModifiers[i] = parameters[i].GetOptionalCustomModifiers();
                }
                token = this.GetTypeTokenInternal(con.ReflectedType).Token;
                byte[] signature = SignatureHelper.GetMethodSigHelper(this, con.CallingConvention, null, null, null, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers).InternalGetSignature(out num4);
                str = base.InternalGetMemberRefFromSignature(token, con.Name, signature, num4);
            }
            return new MethodToken(str);
        }

        internal override bool IsDynamic() => 
            true;

        public bool IsTransient() => 
            base.m_moduleData.IsTransient();

        internal void PreSave(string fileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    this.PreSaveNoLock(fileName, portableExecutableKind, imageFileMachine);
                    return;
                }
            }
            this.PreSaveNoLock(fileName, portableExecutableKind, imageFileMachine);
        }

        private void PreSaveNoLock(string fileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            if (base.m_moduleData.m_isSaved)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("InvalidOperation_ModuleHasBeenSaved"), new object[] { base.m_moduleData.m_strModuleName }));
            }
            if (!base.m_moduleData.m_fGlobalBeenCreated && base.m_moduleData.m_fHasGlobal)
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_GlobalFunctionNotBaked"));
            }
            int count = base.m_TypeBuilderList.Count;
            for (int i = 0; i < count; i++)
            {
                TypeBuilder typeBuilder;
                object obj2 = base.m_TypeBuilderList[i];
                if (obj2 is TypeBuilder)
                {
                    typeBuilder = (TypeBuilder) obj2;
                }
                else
                {
                    EnumBuilder builder2 = (EnumBuilder) obj2;
                    typeBuilder = builder2.m_typeBuilder;
                }
                if (!typeBuilder.m_hasBeenCreated && !typeBuilder.m_isHiddenType)
                {
                    throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("NotSupported_NotAllTypesAreBaked"), new object[] { typeBuilder.FullName }));
                }
            }
            base.InternalPreSavePEFile((int) portableExecutableKind, (int) imageFileMachine);
        }

        internal void Save(string fileName, bool isAssemblyFile, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    this.SaveNoLock(fileName, isAssemblyFile, portableExecutableKind, imageFileMachine);
                    return;
                }
            }
            this.SaveNoLock(fileName, isAssemblyFile, portableExecutableKind, imageFileMachine);
        }

        private void SaveNoLock(string fileName, bool isAssemblyFile, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            if (base.m_moduleData.m_embeddedRes != null)
            {
                ResWriterData embeddedRes = base.m_moduleData.m_embeddedRes;
                int resCount = 0;
                while (embeddedRes != null)
                {
                    embeddedRes = embeddedRes.m_nextResWriter;
                    resCount++;
                }
                base.InternalSetResourceCounts(resCount);
                for (embeddedRes = base.m_moduleData.m_embeddedRes; embeddedRes != null; embeddedRes = embeddedRes.m_nextResWriter)
                {
                    if (embeddedRes.m_resWriter != null)
                    {
                        embeddedRes.m_resWriter.Generate();
                    }
                    byte[] buffer = new byte[embeddedRes.m_memoryStream.Length];
                    embeddedRes.m_memoryStream.Flush();
                    embeddedRes.m_memoryStream.Position = 0L;
                    embeddedRes.m_memoryStream.Read(buffer, 0, buffer.Length);
                    base.InternalAddResource(embeddedRes.m_strName, buffer, buffer.Length, base.m_moduleData.m_tkFile, (int) embeddedRes.m_attribute, (int) portableExecutableKind, (int) imageFileMachine);
                }
            }
            if (base.m_moduleData.m_strResourceFileName != null)
            {
                base.InternalDefineNativeResourceFile(base.m_moduleData.m_strResourceFileName, (int) portableExecutableKind, (int) imageFileMachine);
            }
            else if (base.m_moduleData.m_resourceBytes != null)
            {
                base.InternalDefineNativeResourceBytes(base.m_moduleData.m_resourceBytes, (int) portableExecutableKind, (int) imageFileMachine);
            }
            if (isAssemblyFile)
            {
                base.InternalSavePEFile(fileName, base.m_EntryPoint, (int) base.Assembly.m_assemblyData.m_peFileKind, true);
            }
            else
            {
                base.InternalSavePEFile(fileName, base.m_EntryPoint, 1, false);
            }
            base.m_moduleData.m_isSaved = true;
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (customBuilder == null)
            {
                throw new ArgumentNullException("customBuilder");
            }
            customBuilder.CreateCustomAttribute(this, 1);
        }

        [ComVisible(true)]
        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }
            if (binaryAttribute == null)
            {
                throw new ArgumentNullException("binaryAttribute");
            }
            TypeBuilder.InternalCreateCustomAttribute(1, this.GetConstructorToken(con).Token, binaryAttribute, this, false);
        }

        internal void SetEntryPoint(MethodInfo entryPoint)
        {
            base.m_EntryPoint = this.GetMethodTokenInternal(entryPoint);
        }

        public void SetSymCustomAttribute(string name, byte[] data)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    this.SetSymCustomAttributeNoLock(name, data);
                    return;
                }
            }
            this.SetSymCustomAttributeNoLock(name, data);
        }

        private void SetSymCustomAttributeNoLock(string name, byte[] data)
        {
            if (base.m_iSymWriter == null)
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotADebugModule"));
            }
        }

        public void SetUserEntryPoint(MethodInfo entryPoint)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedAssemblyPermission();
            }
            if (base.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (base.Assembly.m_assemblyData)
                {
                    this.SetUserEntryPointNoLock(entryPoint);
                    return;
                }
            }
            this.SetUserEntryPointNoLock(entryPoint);
        }

        private void SetUserEntryPointNoLock(MethodInfo entryPoint)
        {
            if (entryPoint == null)
            {
                throw new ArgumentNullException("entryPoint");
            }
            if (base.m_iSymWriter == null)
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotADebugModule"));
            }
            if (entryPoint.DeclaringType != null)
            {
                if (entryPoint.Module.InternalModule != this.InternalModule)
                {
                    throw new InvalidOperationException(Environment.GetResourceString("Argument_NotInTheSameModuleBuilder"));
                }
            }
            else
            {
                MethodBuilder builder = entryPoint as MethodBuilder;
                if ((builder != null) && (builder.GetModule().InternalModule != this.InternalModule))
                {
                    throw new InvalidOperationException(Environment.GetResourceString("Argument_NotInTheSameModuleBuilder"));
                }
            }
            SymbolToken entryMethod = new SymbolToken(this.GetMethodTokenInternal(entryPoint).Token);
            base.m_iSymWriter.SetUserEntryPoint(entryMethod);
        }

        void _ModuleBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
        {
            throw new NotImplementedException();
        }

        void _ModuleBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
        {
            throw new NotImplementedException();
        }

        void _ModuleBuilder.GetTypeInfoCount(out uint pcTInfo)
        {
            throw new NotImplementedException();
        }

        void _ModuleBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
        {
            throw new NotImplementedException();
        }

        internal static string UnmangleTypeName(string typeName)
        {
            int startIndex = typeName.Length - 1;
        Label_0009:
            startIndex = typeName.LastIndexOf('+', startIndex);
            if (startIndex != -1)
            {
                bool flag = true;
                int num2 = startIndex;
                while (typeName[--num2] == '\\')
                {
                    flag = !flag;
                }
                if (!flag)
                {
                    startIndex = num2;
                    goto Label_0009;
                }
            }
            return typeName.Substring(startIndex + 1);
        }

        public override string FullyQualifiedName
        {
            get
            {
                string strFileName = base.m_moduleData.m_strFileName;
                if (strFileName == null)
                {
                    return null;
                }
                if (base.Assembly.m_assemblyData.m_strDir != null)
                {
                    strFileName = Path.GetFullPath(Path.Combine(base.Assembly.m_assemblyData.m_strDir, strFileName));
                }
                if ((base.Assembly.m_assemblyData.m_strDir != null) && (strFileName != null))
                {
                    new FileIOPermission(FileIOPermissionAccess.PathDiscovery, strFileName).Demand();
                }
                return strFileName;
            }
        }

        internal override Module InternalModule
        {
            get
            {
                if (this.IsInternal)
                {
                    return this;
                }
                return this.m_internalModuleBuilder;
            }
        }

        private bool IsInternal =>
            (this.m_internalModuleBuilder == null);
    }
}

