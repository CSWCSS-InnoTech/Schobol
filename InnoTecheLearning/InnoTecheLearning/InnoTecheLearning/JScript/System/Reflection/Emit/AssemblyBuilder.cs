namespace System.Reflection.Emit
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.SymbolStore;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Security.Permissions;
    using System.Threading;

    [ClassInterface(ClassInterfaceType.None), ComDefaultInterface(typeof(_AssemblyBuilder)), ComVisible(true), HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort=true)]
    public sealed class AssemblyBuilder : Assembly, _AssemblyBuilder
    {
        private PermissionSet m_grantedPermissionSet;
        private AssemblyBuilder m_internalAssemblyBuilder;

        private AssemblyBuilder()
        {
        }

        internal AssemblyBuilder(AssemblyBuilder internalAssemblyBuilder)
        {
            this.m_internalAssemblyBuilder = internalAssemblyBuilder;
        }

        public void AddResourceFile(string name, string fileName)
        {
            this.AddResourceFile(name, fileName, ResourceAttributes.Public);
        }

        public void AddResourceFile(string name, string fileName, ResourceAttributes attribute)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            if (base.m_assemblyData.m_isSynchronized)
            {
                lock (base.m_assemblyData)
                {
                    this.AddResourceFileNoLock(name, fileName, attribute);
                    return;
                }
            }
            this.AddResourceFileNoLock(name, fileName, attribute);
        }

        private void AddResourceFileNoLock(string name, string fileName, ResourceAttributes attribute)
        {
            string fullPath;
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), name);
            }
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (fileName.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), fileName);
            }
            if (!string.Equals(fileName, Path.GetFileName(fileName)))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NotSimpleFileName"), "fileName");
            }
            base.m_assemblyData.CheckResNameConflict(name);
            base.m_assemblyData.CheckFileNameConflict(fileName);
            if (base.m_assemblyData.m_strDir == null)
            {
                fullPath = Path.Combine(Environment.CurrentDirectory, fileName);
            }
            else
            {
                fullPath = Path.Combine(base.m_assemblyData.m_strDir, fileName);
            }
            fullPath = Path.GetFullPath(fullPath);
            fileName = Path.GetFileName(fullPath);
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.FileNotFound_FileName"), new object[] { fileName }), fileName);
            }
            base.m_assemblyData.AddResWriter(new ResWriterData(null, null, name, fileName, fullPath, attribute));
        }

        internal void CheckContext(params Type[][] typess)
        {
            if (typess != null)
            {
                foreach (Type[] typeArray in typess)
                {
                    if (typeArray != null)
                    {
                        this.CheckContext(typeArray);
                    }
                }
            }
        }

        internal void CheckContext(params Type[] types)
        {
            if (types != null)
            {
                foreach (Type type in types)
                {
                    if ((type == null) || (type.Module.Assembly == typeof(object).Module.Assembly))
                    {
                        break;
                    }
                    if (type.Module.Assembly.ReflectionOnly && !this.ReflectionOnly)
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arugment_EmitMixedContext1"), new object[] { type.AssemblyQualifiedName }));
                    }
                    if (!type.Module.Assembly.ReflectionOnly && this.ReflectionOnly)
                    {
                        throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arugment_EmitMixedContext2"), new object[] { type.AssemblyQualifiedName }));
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public ModuleBuilder DefineDynamicModule(string name)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            StackCrawlMark lookForMyCaller = StackCrawlMark.LookForMyCaller;
            return this.DefineDynamicModuleInternal(name, false, ref lookForMyCaller);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public ModuleBuilder DefineDynamicModule(string name, bool emitSymbolInfo)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            StackCrawlMark lookForMyCaller = StackCrawlMark.LookForMyCaller;
            return this.DefineDynamicModuleInternal(name, emitSymbolInfo, ref lookForMyCaller);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public ModuleBuilder DefineDynamicModule(string name, string fileName)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            StackCrawlMark lookForMyCaller = StackCrawlMark.LookForMyCaller;
            return this.DefineDynamicModuleInternal(name, fileName, false, ref lookForMyCaller);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public ModuleBuilder DefineDynamicModule(string name, string fileName, bool emitSymbolInfo)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            StackCrawlMark lookForMyCaller = StackCrawlMark.LookForMyCaller;
            return this.DefineDynamicModuleInternal(name, fileName, emitSymbolInfo, ref lookForMyCaller);
        }

        private ModuleBuilder DefineDynamicModuleInternal(string name, bool emitSymbolInfo, ref StackCrawlMark stackMark)
        {
            if (base.m_assemblyData.m_isSynchronized)
            {
                lock (base.m_assemblyData)
                {
                    return this.DefineDynamicModuleInternalNoLock(name, emitSymbolInfo, ref stackMark);
                }
            }
            return this.DefineDynamicModuleInternalNoLock(name, emitSymbolInfo, ref stackMark);
        }

        private ModuleBuilder DefineDynamicModuleInternal(string name, string fileName, bool emitSymbolInfo, ref StackCrawlMark stackMark)
        {
            if (base.m_assemblyData.m_isSynchronized)
            {
                lock (base.m_assemblyData)
                {
                    return this.DefineDynamicModuleInternalNoLock(name, fileName, emitSymbolInfo, ref stackMark);
                }
            }
            return this.DefineDynamicModuleInternalNoLock(name, fileName, emitSymbolInfo, ref stackMark);
        }

        private ModuleBuilder DefineDynamicModuleInternalNoLock(string name, bool emitSymbolInfo, ref StackCrawlMark stackMark)
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
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidName"), "name");
            }
            base.m_assemblyData.CheckNameConflict(name);
            ModuleBuilder internalModuleBuilder = (ModuleBuilder) Assembly.nDefineDynamicModule(this, emitSymbolInfo, name, ref stackMark);
            internalModuleBuilder = new ModuleBuilder(this, internalModuleBuilder);
            ISymbolWriter writer = null;
            if (emitSymbolInfo)
            {
                Type type = this.LoadISymWrapper().GetType("System.Diagnostics.SymbolStore.SymWriter", true, false);
                if ((type != null) && !type.IsVisible)
                {
                    type = null;
                }
                if (type == null)
                {
                    throw new ExecutionEngineException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingType"), new object[] { "SymWriter" }));
                }
                new ReflectionPermission(ReflectionPermissionFlag.ReflectionEmit).Demand();
                try
                {
                    new PermissionSet(PermissionState.Unrestricted).Assert();
                    writer = (ISymbolWriter) Activator.CreateInstance(type);
                }
                finally
                {
                    CodeAccessPermission.RevertAssert();
                }
            }
            internalModuleBuilder.Init(name, null, writer);
            base.m_assemblyData.AddModule(internalModuleBuilder);
            return internalModuleBuilder;
        }

        private ModuleBuilder DefineDynamicModuleInternalNoLock(string name, string fileName, bool emitSymbolInfo, ref StackCrawlMark stackMark)
        {
            if (base.m_assemblyData.m_access == AssemblyBuilderAccess.Run)
            {
                throw new NotSupportedException(Environment.GetResourceString("Argument_BadPersistableModuleInTransientAssembly"));
            }
            if (base.m_assemblyData.m_isSaved)
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CannotAlterAssembly"));
            }
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
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidName"), "name");
            }
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (fileName.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "fileName");
            }
            if (!string.Equals(fileName, Path.GetFileName(fileName)))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NotSimpleFileName"), "fileName");
            }
            base.m_assemblyData.CheckNameConflict(name);
            base.m_assemblyData.CheckFileNameConflict(fileName);
            ModuleBuilder internalModuleBuilder = (ModuleBuilder) Assembly.nDefineDynamicModule(this, emitSymbolInfo, fileName, ref stackMark);
            internalModuleBuilder = new ModuleBuilder(this, internalModuleBuilder);
            ISymbolWriter writer = null;
            if (emitSymbolInfo)
            {
                Type type = this.LoadISymWrapper().GetType("System.Diagnostics.SymbolStore.SymWriter", true, false);
                if ((type != null) && !type.IsVisible)
                {
                    type = null;
                }
                if (type == null)
                {
                    throw new ExecutionEngineException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("MissingType"), new object[] { "SymWriter" }));
                }
                new ReflectionPermission(ReflectionPermissionFlag.ReflectionEmit).Demand();
                try
                {
                    new PermissionSet(PermissionState.Unrestricted).Assert();
                    writer = (ISymbolWriter) Activator.CreateInstance(type);
                }
                finally
                {
                    CodeAccessPermission.RevertAssert();
                }
            }
            internalModuleBuilder.Init(name, fileName, writer);
            base.m_assemblyData.AddModule(internalModuleBuilder);
            return internalModuleBuilder;
        }

        private int DefineNestedComType(Type type, int tkResolutionScope, int tkTypeDef)
        {
            Type declaringType = type.DeclaringType;
            if (declaringType != null)
            {
                tkResolutionScope = this.DefineNestedComType(declaringType, tkResolutionScope, tkTypeDef);
            }
            return base.nSaveExportedType(type.FullName, tkResolutionScope, tkTypeDef, type.Attributes);
        }

        public IResourceWriter DefineResource(string name, string description, string fileName) => 
            this.DefineResource(name, description, fileName, ResourceAttributes.Public);

        public IResourceWriter DefineResource(string name, string description, string fileName, ResourceAttributes attribute)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            if (base.m_assemblyData.m_isSynchronized)
            {
                lock (base.m_assemblyData)
                {
                    return this.DefineResourceNoLock(name, description, fileName, attribute);
                }
            }
            return this.DefineResourceNoLock(name, description, fileName, attribute);
        }

        private IResourceWriter DefineResourceNoLock(string name, string description, string fileName, ResourceAttributes attribute)
        {
            ResourceWriter writer;
            string fullPath;
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), name);
            }
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }
            if (fileName.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "fileName");
            }
            if (!string.Equals(fileName, Path.GetFileName(fileName)))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NotSimpleFileName"), "fileName");
            }
            base.m_assemblyData.CheckResNameConflict(name);
            base.m_assemblyData.CheckFileNameConflict(fileName);
            if (base.m_assemblyData.m_strDir == null)
            {
                fullPath = Path.Combine(Environment.CurrentDirectory, fileName);
                writer = new ResourceWriter(fullPath);
            }
            else
            {
                fullPath = Path.Combine(base.m_assemblyData.m_strDir, fileName);
                writer = new ResourceWriter(fullPath);
            }
            fullPath = Path.GetFullPath(fullPath);
            fileName = Path.GetFileName(fullPath);
            base.m_assemblyData.AddResWriter(new ResWriterData(writer, null, name, fileName, fullPath, attribute));
            return writer;
        }

        public void DefineUnmanagedResource(byte[] resource)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            if (base.m_assemblyData.m_isSynchronized)
            {
                lock (base.m_assemblyData)
                {
                    this.DefineUnmanagedResourceNoLock(resource);
                    return;
                }
            }
            this.DefineUnmanagedResourceNoLock(resource);
        }

        public void DefineUnmanagedResource(string resourceFileName)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            if (resourceFileName == null)
            {
                throw new ArgumentNullException("resourceFileName");
            }
            if (base.m_assemblyData.m_isSynchronized)
            {
                lock (base.m_assemblyData)
                {
                    this.DefineUnmanagedResourceNoLock(resourceFileName);
                    return;
                }
            }
            this.DefineUnmanagedResourceNoLock(resourceFileName);
        }

        private void DefineUnmanagedResourceNoLock(byte[] resource)
        {
            if (((base.m_assemblyData.m_strResourceFileName != null) || (base.m_assemblyData.m_resourceBytes != null)) || (base.m_assemblyData.m_nativeVersion != null))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
            }
            base.m_assemblyData.m_resourceBytes = new byte[resource.Length];
            Array.Copy(resource, base.m_assemblyData.m_resourceBytes, resource.Length);
        }

        private void DefineUnmanagedResourceNoLock(string resourceFileName)
        {
            string fullPath;
            if (((base.m_assemblyData.m_strResourceFileName != null) || (base.m_assemblyData.m_resourceBytes != null)) || (base.m_assemblyData.m_nativeVersion != null))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
            }
            if (base.m_assemblyData.m_strDir == null)
            {
                fullPath = Path.Combine(Environment.CurrentDirectory, resourceFileName);
            }
            else
            {
                fullPath = Path.Combine(base.m_assemblyData.m_strDir, resourceFileName);
            }
            fullPath = Path.GetFullPath(resourceFileName);
            new FileIOPermission(FileIOPermissionAccess.Read, fullPath).Demand();
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("IO.FileNotFound_FileName"), new object[] { resourceFileName }), resourceFileName);
            }
            base.m_assemblyData.m_strResourceFileName = fullPath;
        }

        public void DefineVersionInfoResource()
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            if (base.m_assemblyData.m_isSynchronized)
            {
                lock (base.m_assemblyData)
                {
                    this.DefineVersionInfoResourceNoLock();
                    return;
                }
            }
            this.DefineVersionInfoResourceNoLock();
        }

        public void DefineVersionInfoResource(string product, string productVersion, string company, string copyright, string trademark)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            if (base.m_assemblyData.m_isSynchronized)
            {
                lock (base.m_assemblyData)
                {
                    this.DefineVersionInfoResourceNoLock(product, productVersion, company, copyright, trademark);
                    return;
                }
            }
            this.DefineVersionInfoResourceNoLock(product, productVersion, company, copyright, trademark);
        }

        private void DefineVersionInfoResourceNoLock()
        {
            if (((base.m_assemblyData.m_strResourceFileName != null) || (base.m_assemblyData.m_resourceBytes != null)) || (base.m_assemblyData.m_nativeVersion != null))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
            }
            base.m_assemblyData.m_hasUnmanagedVersionInfo = true;
            base.m_assemblyData.m_nativeVersion = new NativeVersionInfo();
        }

        private void DefineVersionInfoResourceNoLock(string product, string productVersion, string company, string copyright, string trademark)
        {
            if (((base.m_assemblyData.m_strResourceFileName != null) || (base.m_assemblyData.m_resourceBytes != null)) || (base.m_assemblyData.m_nativeVersion != null))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NativeResourceAlreadyDefined"));
            }
            base.m_assemblyData.m_nativeVersion = new NativeVersionInfo();
            base.m_assemblyData.m_nativeVersion.m_strCopyright = copyright;
            base.m_assemblyData.m_nativeVersion.m_strTrademark = trademark;
            base.m_assemblyData.m_nativeVersion.m_strCompany = company;
            base.m_assemblyData.m_nativeVersion.m_strProduct = product;
            base.m_assemblyData.m_nativeVersion.m_strProductVersion = productVersion;
            base.m_assemblyData.m_hasUnmanagedVersionInfo = true;
            base.m_assemblyData.m_OverrideUnmanagedVersionInfo = true;
        }

        internal void DemandGrantedPermission()
        {
            this.GrantedPermissionSet.Demand();
        }

        public ModuleBuilder GetDynamicModule(string name)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            if (base.m_assemblyData.m_isSynchronized)
            {
                lock (base.m_assemblyData)
                {
                    return this.GetDynamicModuleNoLock(name);
                }
            }
            return this.GetDynamicModuleNoLock(name);
        }

        private ModuleBuilder GetDynamicModuleNoLock(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "name");
            }
            int count = base.m_assemblyData.m_moduleBuilderList.Count;
            for (int i = 0; i < count; i++)
            {
                ModuleBuilder builder = (ModuleBuilder) base.m_assemblyData.m_moduleBuilderList[i];
                if (builder.m_moduleData.m_strModuleName.Equals(name))
                {
                    return builder;
                }
            }
            return null;
        }

        public override Type[] GetExportedTypes()
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
        }

        public override FileStream GetFile(string name)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
        }

        public override FileStream[] GetFiles(bool getResourceModules)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
        }

        public override ManifestResourceInfo GetManifestResourceInfo(string resourceName)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
        }

        public override string[] GetManifestResourceNames()
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
        }

        public override Stream GetManifestResourceStream(string name)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
        }

        public override Stream GetManifestResourceStream(Type type, string name)
        {
            throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
        }

        internal override Module GetModuleInternal(string name)
        {
            Module module = this.InternalAssembly._GetModule(name);
            if (module == null)
            {
                return null;
            }
            if (!this.IsInternal)
            {
                return ModuleBuilder.GetModuleBuilder(module);
            }
            return module;
        }

        internal bool IsPersistable() => 
            ((base.m_assemblyData.m_access & AssemblyBuilderAccess.Save) == AssemblyBuilderAccess.Save);

        private Assembly LoadISymWrapper()
        {
            if (base.m_assemblyData.m_ISymWrapperAssembly != null)
            {
                return base.m_assemblyData.m_ISymWrapperAssembly;
            }
            Assembly assembly = Assembly.Load("ISymWrapper, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");
            base.m_assemblyData.m_ISymWrapperAssembly = assembly;
            return assembly;
        }

        internal override Module[] nGetModules(bool loadIfNotFound, bool getResourceModules)
        {
            Module[] moduleArray = this.InternalAssembly._nGetModules(loadIfNotFound, getResourceModules);
            if (!this.IsInternal)
            {
                for (int i = 0; i < moduleArray.Length; i++)
                {
                    moduleArray[i] = ModuleBuilder.GetModuleBuilder(moduleArray[i]);
                }
            }
            return moduleArray;
        }

        public void Save(string assemblyFileName)
        {
            this.Save(assemblyFileName, PortableExecutableKinds.ILOnly, ImageFileMachine.I386);
        }

        public void Save(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            if (base.m_assemblyData.m_isSynchronized)
            {
                lock (base.m_assemblyData)
                {
                    this.SaveNoLock(assemblyFileName, portableExecutableKind, imageFileMachine);
                    return;
                }
            }
            this.SaveNoLock(assemblyFileName, portableExecutableKind, imageFileMachine);
        }

        private void SaveNoLock(string assemblyFileName, PortableExecutableKinds portableExecutableKind, ImageFileMachine imageFileMachine)
        {
            int[] numArray = null;
            int[] numArray2 = null;
            string resourceFileName = null;
            try
            {
                int num;
                if (base.m_assemblyData.m_iCABuilder != 0)
                {
                    numArray = new int[base.m_assemblyData.m_iCABuilder];
                }
                if (base.m_assemblyData.m_iCAs != 0)
                {
                    numArray2 = new int[base.m_assemblyData.m_iCAs];
                }
                if (base.m_assemblyData.m_isSaved)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("InvalidOperation_AssemblyHasBeenSaved"), new object[] { base.nGetSimpleName() }));
                }
                if ((base.m_assemblyData.m_access & AssemblyBuilderAccess.Save) != AssemblyBuilderAccess.Save)
                {
                    throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_CantSaveTransientAssembly"));
                }
                if (assemblyFileName == null)
                {
                    throw new ArgumentNullException("assemblyFileName");
                }
                if (assemblyFileName.Length == 0)
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_EmptyFileName"), "assemblyFileName");
                }
                if (!string.Equals(assemblyFileName, Path.GetFileName(assemblyFileName)))
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_NotSimpleFileName"), "assemblyFileName");
                }
                ModuleBuilder modBuilder = base.m_assemblyData.FindModuleWithFileName(assemblyFileName);
                if (modBuilder != null)
                {
                    base.m_assemblyData.SetOnDiskAssemblyModule(modBuilder);
                }
                if (modBuilder == null)
                {
                    base.m_assemblyData.CheckFileNameConflict(assemblyFileName);
                }
                if (base.m_assemblyData.m_strDir == null)
                {
                    base.m_assemblyData.m_strDir = Environment.CurrentDirectory;
                }
                else if (!Directory.Exists(base.m_assemblyData.m_strDir))
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, Environment.GetResourceString("Argument_InvalidDirectory"), new object[] { base.m_assemblyData.m_strDir }));
                }
                assemblyFileName = Path.Combine(base.m_assemblyData.m_strDir, assemblyFileName);
                assemblyFileName = Path.GetFullPath(assemblyFileName);
                new FileIOPermission(FileIOPermissionAccess.Append | FileIOPermissionAccess.Write, assemblyFileName).Demand();
                if (modBuilder != null)
                {
                    for (num = 0; num < base.m_assemblyData.m_iCABuilder; num++)
                    {
                        numArray[num] = base.m_assemblyData.m_CABuilders[num].PrepareCreateCustomAttributeToDisk(modBuilder);
                    }
                    for (num = 0; num < base.m_assemblyData.m_iCAs; num++)
                    {
                        numArray2[num] = modBuilder.InternalGetConstructorToken(base.m_assemblyData.m_CACons[num], true).Token;
                    }
                    modBuilder.PreSave(assemblyFileName, portableExecutableKind, imageFileMachine);
                }
                base.nPrepareForSavingManifestToDisk(modBuilder);
                ModuleBuilder onDiskAssemblyModule = base.m_assemblyData.GetOnDiskAssemblyModule();
                if (base.m_assemblyData.m_strResourceFileName != null)
                {
                    onDiskAssemblyModule.DefineUnmanagedResourceFileInternalNoLock(base.m_assemblyData.m_strResourceFileName);
                }
                else if (base.m_assemblyData.m_resourceBytes != null)
                {
                    onDiskAssemblyModule.DefineUnmanagedResourceInternalNoLock(base.m_assemblyData.m_resourceBytes);
                }
                else if (base.m_assemblyData.m_hasUnmanagedVersionInfo)
                {
                    base.m_assemblyData.FillUnmanagedVersionInfo();
                    string strFileVersion = base.m_assemblyData.m_nativeVersion.m_strFileVersion;
                    if (strFileVersion == null)
                    {
                        strFileVersion = base.GetVersion().ToString();
                    }
                    resourceFileName = Assembly.nDefineVersionInfoResource(assemblyFileName, base.m_assemblyData.m_nativeVersion.m_strTitle, null, base.m_assemblyData.m_nativeVersion.m_strDescription, base.m_assemblyData.m_nativeVersion.m_strCopyright, base.m_assemblyData.m_nativeVersion.m_strTrademark, base.m_assemblyData.m_nativeVersion.m_strCompany, base.m_assemblyData.m_nativeVersion.m_strProduct, base.m_assemblyData.m_nativeVersion.m_strProductVersion, strFileVersion, base.m_assemblyData.m_nativeVersion.m_lcid, base.m_assemblyData.m_peFileKind == PEFileKinds.Dll);
                    onDiskAssemblyModule.DefineUnmanagedResourceFileInternalNoLock(resourceFileName);
                }
                if (modBuilder == null)
                {
                    for (num = 0; num < base.m_assemblyData.m_iCABuilder; num++)
                    {
                        numArray[num] = base.m_assemblyData.m_CABuilders[num].PrepareCreateCustomAttributeToDisk(onDiskAssemblyModule);
                    }
                    for (num = 0; num < base.m_assemblyData.m_iCAs; num++)
                    {
                        numArray2[num] = onDiskAssemblyModule.InternalGetConstructorToken(base.m_assemblyData.m_CACons[num], true).Token;
                    }
                }
                int count = base.m_assemblyData.m_moduleBuilderList.Count;
                for (num = 0; num < count; num++)
                {
                    ModuleBuilder builder5 = (ModuleBuilder) base.m_assemblyData.m_moduleBuilderList[num];
                    if (!builder5.IsTransient() && (builder5 != modBuilder))
                    {
                        string strFileName = builder5.m_moduleData.m_strFileName;
                        if (base.m_assemblyData.m_strDir != null)
                        {
                            strFileName = Path.GetFullPath(Path.Combine(base.m_assemblyData.m_strDir, strFileName));
                        }
                        new FileIOPermission(FileIOPermissionAccess.Append | FileIOPermissionAccess.Write, strFileName).Demand();
                        builder5.m_moduleData.m_tkFile = base.nSaveToFileList(builder5.m_moduleData.m_strFileName);
                        builder5.PreSave(strFileName, portableExecutableKind, imageFileMachine);
                        builder5.Save(strFileName, false, portableExecutableKind, imageFileMachine);
                        base.nSetHashValue(builder5.m_moduleData.m_tkFile, strFileName);
                    }
                }
                for (num = 0; num < base.m_assemblyData.m_iPublicComTypeCount; num++)
                {
                    ModuleBuilder module;
                    Type type = base.m_assemblyData.m_publicComTypeList[num];
                    if (type is RuntimeType)
                    {
                        module = base.m_assemblyData.FindModuleWithName(type.Module.m_moduleData.m_strModuleName);
                        if (module != modBuilder)
                        {
                            this.DefineNestedComType(type, module.m_moduleData.m_tkFile, type.MetadataTokenInternal);
                        }
                    }
                    else
                    {
                        TypeBuilder builder = (TypeBuilder) type;
                        module = (ModuleBuilder) type.Module;
                        if (module != modBuilder)
                        {
                            this.DefineNestedComType(type, module.m_moduleData.m_tkFile, builder.MetadataTokenInternal);
                        }
                    }
                }
                for (num = 0; num < base.m_assemblyData.m_iCABuilder; num++)
                {
                    base.m_assemblyData.m_CABuilders[num].CreateCustomAttribute(onDiskAssemblyModule, 0x20000001, numArray[num], true);
                }
                for (num = 0; num < base.m_assemblyData.m_iCAs; num++)
                {
                    TypeBuilder.InternalCreateCustomAttribute(0x20000001, numArray2[num], base.m_assemblyData.m_CABytes[num], onDiskAssemblyModule, true);
                }
                if (((base.m_assemblyData.m_RequiredPset != null) || (base.m_assemblyData.m_OptionalPset != null)) || (base.m_assemblyData.m_RefusedPset != null))
                {
                    byte[] required = null;
                    byte[] optional = null;
                    byte[] refused = null;
                    if (base.m_assemblyData.m_RequiredPset != null)
                    {
                        required = base.m_assemblyData.m_RequiredPset.EncodeXml();
                    }
                    if (base.m_assemblyData.m_OptionalPset != null)
                    {
                        optional = base.m_assemblyData.m_OptionalPset.EncodeXml();
                    }
                    if (base.m_assemblyData.m_RefusedPset != null)
                    {
                        refused = base.m_assemblyData.m_RefusedPset.EncodeXml();
                    }
                    base.nSavePermissionRequests(required, optional, refused);
                }
                count = base.m_assemblyData.m_resWriterList.Count;
                for (num = 0; num < count; num++)
                {
                    ResWriterData data = null;
                    try
                    {
                        data = (ResWriterData) base.m_assemblyData.m_resWriterList[num];
                        if (data.m_resWriter != null)
                        {
                            new FileIOPermission(FileIOPermissionAccess.Append | FileIOPermissionAccess.Write, data.m_strFullFileName).Demand();
                        }
                    }
                    finally
                    {
                        if ((data != null) && (data.m_resWriter != null))
                        {
                            data.m_resWriter.Close();
                        }
                    }
                    base.nAddStandAloneResource(data.m_strName, data.m_strFileName, data.m_strFullFileName, (int) data.m_attribute);
                }
                if (modBuilder == null)
                {
                    if (onDiskAssemblyModule.m_moduleData.m_strResourceFileName != null)
                    {
                        onDiskAssemblyModule.InternalDefineNativeResourceFile(onDiskAssemblyModule.m_moduleData.m_strResourceFileName, (int) portableExecutableKind, (int) imageFileMachine);
                    }
                    else if (onDiskAssemblyModule.m_moduleData.m_resourceBytes != null)
                    {
                        onDiskAssemblyModule.InternalDefineNativeResourceBytes(onDiskAssemblyModule.m_moduleData.m_resourceBytes, (int) portableExecutableKind, (int) imageFileMachine);
                    }
                    if (base.m_assemblyData.m_entryPointModule != null)
                    {
                        base.nSaveManifestToDisk(assemblyFileName, base.m_assemblyData.m_entryPointModule.m_moduleData.m_tkFile, (int) base.m_assemblyData.m_peFileKind, (int) portableExecutableKind, (int) imageFileMachine);
                    }
                    else
                    {
                        base.nSaveManifestToDisk(assemblyFileName, 0, (int) base.m_assemblyData.m_peFileKind, (int) portableExecutableKind, (int) imageFileMachine);
                    }
                }
                else
                {
                    if ((base.m_assemblyData.m_entryPointModule != null) && (base.m_assemblyData.m_entryPointModule != modBuilder))
                    {
                        modBuilder.m_EntryPoint = new MethodToken(base.m_assemblyData.m_entryPointModule.m_moduleData.m_tkFile);
                    }
                    modBuilder.Save(assemblyFileName, true, portableExecutableKind, imageFileMachine);
                }
                base.m_assemblyData.m_isSaved = true;
            }
            finally
            {
                if (resourceFileName != null)
                {
                    File.Delete(resourceFileName);
                }
            }
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            if (customBuilder == null)
            {
                throw new ArgumentNullException("customBuilder");
            }
            if (base.m_assemblyData.m_isSynchronized)
            {
                lock (base.m_assemblyData)
                {
                    this.SetCustomAttributeNoLock(customBuilder);
                    return;
                }
            }
            this.SetCustomAttributeNoLock(customBuilder);
        }

        [ComVisible(true)]
        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }
            if (binaryAttribute == null)
            {
                throw new ArgumentNullException("binaryAttribute");
            }
            if (base.m_assemblyData.m_isSynchronized)
            {
                lock (base.m_assemblyData)
                {
                    this.SetCustomAttributeNoLock(con, binaryAttribute);
                    return;
                }
            }
            this.SetCustomAttributeNoLock(con, binaryAttribute);
        }

        private void SetCustomAttributeNoLock(CustomAttributeBuilder customBuilder)
        {
            ModuleBuilder inMemoryAssemblyModule = base.m_assemblyData.GetInMemoryAssemblyModule();
            customBuilder.CreateCustomAttribute(inMemoryAssemblyModule, 0x20000001);
            if (base.m_assemblyData.m_access != AssemblyBuilderAccess.Run)
            {
                base.m_assemblyData.AddCustomAttribute(customBuilder);
            }
        }

        private void SetCustomAttributeNoLock(ConstructorInfo con, byte[] binaryAttribute)
        {
            ModuleBuilder inMemoryAssemblyModule = base.m_assemblyData.GetInMemoryAssemblyModule();
            TypeBuilder.InternalCreateCustomAttribute(0x20000001, inMemoryAssemblyModule.GetConstructorToken(con).Token, binaryAttribute, inMemoryAssemblyModule, false, typeof(DebuggableAttribute) == con.DeclaringType);
            if (base.m_assemblyData.m_access != AssemblyBuilderAccess.Run)
            {
                base.m_assemblyData.AddCustomAttribute(con, binaryAttribute);
            }
        }

        public void SetEntryPoint(MethodInfo entryMethod)
        {
            this.SetEntryPoint(entryMethod, PEFileKinds.ConsoleApplication);
        }

        public void SetEntryPoint(MethodInfo entryMethod, PEFileKinds fileKind)
        {
            if (this.IsInternal)
            {
                this.DemandGrantedPermission();
            }
            if (base.m_assemblyData.m_isSynchronized)
            {
                lock (base.m_assemblyData)
                {
                    this.SetEntryPointNoLock(entryMethod, fileKind);
                    return;
                }
            }
            this.SetEntryPointNoLock(entryMethod, fileKind);
        }

        private void SetEntryPointNoLock(MethodInfo entryMethod, PEFileKinds fileKind)
        {
            if (entryMethod == null)
            {
                throw new ArgumentNullException("entryMethod");
            }
            Module internalModule = entryMethod.Module.InternalModule;
            if (!(internalModule is ModuleBuilder) || !this.InternalAssembly.Equals(internalModule.Assembly.InternalAssembly))
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EntryMethodNotDefinedInAssembly"));
            }
            base.m_assemblyData.m_entryPointModule = (ModuleBuilder) ModuleBuilder.GetModuleBuilder(internalModule);
            base.m_assemblyData.m_entryPointMethod = entryMethod;
            base.m_assemblyData.m_peFileKind = fileKind;
            base.m_assemblyData.m_entryPointModule.SetEntryPoint(entryMethod);
        }

        void _AssemblyBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
        {
            throw new NotImplementedException();
        }

        void _AssemblyBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
        {
            throw new NotImplementedException();
        }

        void _AssemblyBuilder.GetTypeInfoCount(out uint pcTInfo)
        {
            throw new NotImplementedException();
        }

        void _AssemblyBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
        {
            throw new NotImplementedException();
        }

        public override string CodeBase
        {
            get
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
            }
        }

        public override MethodInfo EntryPoint
        {
            get
            {
                if (this.IsInternal)
                {
                    this.DemandGrantedPermission();
                }
                return base.m_assemblyData.m_entryPointMethod;
            }
        }

        private PermissionSet GrantedPermissionSet
        {
            get
            {
                AssemblyBuilder internalAssembly = (AssemblyBuilder) this.InternalAssembly;
                if (internalAssembly.m_grantedPermissionSet == null)
                {
                    PermissionSet newDenied = null;
                    this.InternalAssembly.nGetGrantSet(out internalAssembly.m_grantedPermissionSet, out newDenied);
                    if (internalAssembly.m_grantedPermissionSet == null)
                    {
                        internalAssembly.m_grantedPermissionSet = new PermissionSet(PermissionState.Unrestricted);
                    }
                }
                return internalAssembly.m_grantedPermissionSet;
            }
        }

        public override string ImageRuntimeVersion =>
            RuntimeEnvironment.GetSystemVersion();

        internal override Assembly InternalAssembly
        {
            get
            {
                if (this.IsInternal)
                {
                    return this;
                }
                return this.m_internalAssemblyBuilder;
            }
        }

        private bool IsInternal =>
            (this.m_internalAssemblyBuilder == null);

        public override string Location
        {
            get
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicAssembly"));
            }
        }
    }
}

