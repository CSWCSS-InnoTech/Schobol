namespace Microsoft.Vsa
{
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Security.Policy;
    using System.Threading;

    [Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help."), ComVisible(true), Guid("F8932A50-9127-48B6-B115-2BFDC627CEE3")]
    public abstract class BaseVsaEngine : IVsaEngine
    {
        protected string applicationPath = "";
        protected string assemblyVersion;
        protected string compiledRootNamespace = null;
        protected string engineMoniker = "";
        protected string engineName = "";
        protected IVsaSite engineSite = null;
        protected int errorLocale = CultureInfo.CurrentUICulture.LCID;
        protected System.Security.Policy.Evidence executionEvidence;
        protected bool failedCompilation = false;
        protected bool genDebugInfo = false;
        protected bool haveCompiledState = false;
        protected bool isClosed = false;
        protected bool isDebugInfoSupported;
        protected bool isEngineCompiled = false;
        protected bool isEngineDirty = false;
        protected bool isEngineInitialized = false;
        protected bool isEngineRunning = false;
        protected System.Reflection.Assembly loadedAssembly;
        protected static Hashtable nameTable = new Hashtable(10);
        protected string rootNamespace = "";
        protected string scriptLanguage;
        protected Type startupClass;
        protected BaseVsaStartup startupInstance;
        protected IVsaItems vsaItems = null;

        internal BaseVsaEngine(string language, string version, bool supportDebug)
        {
            this.scriptLanguage = language;
            this.assemblyVersion = version;
            this.isDebugInfoSupported = supportDebug;
            this.executionEvidence = null;
        }

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual void Close()
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(Pre.EngineNotClosed);
                if (this.isEngineRunning)
                {
                    this.Reset();
                }
                lock (nameTable)
                {
                    if ((this.engineName != null) && (this.engineName.Length > 0))
                    {
                        nameTable[this.engineName] = null;
                    }
                }
                this.DoClose();
                this.isClosed = true;
            }
            finally
            {
                this.ReleaseLock();
            }
        }

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust"), PermissionSet(SecurityAction.Demand, Name="FullTrust")]
        public virtual bool Compile()
        {
            bool isEngineCompiled;
            this.TryObtainLock();
            try
            {
                this.Preconditions(Pre.EngineInitialised | Pre.RootNamespaceSet | Pre.EngineNotRunning | Pre.EngineNotClosed);
                bool flag = false;
                int num = 0;
                int count = this.vsaItems.Count;
                while (!flag && (num < count))
                {
                    IVsaItem item1 = this.vsaItems[num];
                    flag = this.vsaItems[num].ItemType == VsaItemType.Code;
                    num++;
                }
                if (!flag)
                {
                    throw this.Error(VsaError.EngineClosed);
                }
                try
                {
                    this.ResetCompiledState();
                    this.isEngineCompiled = this.DoCompile();
                }
                catch (VsaException)
                {
                    throw;
                }
                catch (Exception exception)
                {
                    throw new VsaException(VsaError.InternalCompilerError, exception.ToString(), exception);
                }
                catch
                {
                    throw new VsaException(VsaError.InternalCompilerError);
                }
                if (this.isEngineCompiled)
                {
                    this.haveCompiledState = true;
                    this.failedCompilation = false;
                    this.compiledRootNamespace = this.rootNamespace;
                }
                isEngineCompiled = this.isEngineCompiled;
            }
            finally
            {
                this.ReleaseLock();
            }
            return isEngineCompiled;
        }

        protected abstract void DoClose();
        protected abstract bool DoCompile();
        protected abstract void DoLoadSourceState(IVsaPersistSite site);
        protected abstract void DoSaveCompiledState(out byte[] pe, out byte[] debugInfo);
        protected abstract void DoSaveSourceState(IVsaPersistSite site);
        protected VsaException Error(VsaError vsaErrorNumber) => 
            new VsaException(vsaErrorNumber);

        protected abstract object GetCustomOption(string name);
        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual object GetOption(string name)
        {
            object customOption;
            this.TryObtainLock();
            try
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotClosed);
                customOption = this.GetCustomOption(name);
            }
            finally
            {
                this.ReleaseLock();
            }
            return customOption;
        }

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual void InitNew()
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(Pre.EngineNotInitialised | Pre.SiteSet | Pre.RootMonikerSet | Pre.EngineNotClosed);
                this.isEngineInitialized = true;
            }
            finally
            {
                this.ReleaseLock();
            }
        }

        private bool IsCondition(Pre flag, Pre test) => 
            ((flag & test) != Pre.None);

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public abstract bool IsValidIdentifier(string ident);
        protected abstract bool IsValidNamespaceName(string name);
        protected virtual System.Reflection.Assembly LoadCompiledState()
        {
            byte[] buffer;
            byte[] buffer2;
            this.DoSaveCompiledState(out buffer, out buffer2);
            return System.Reflection.Assembly.Load(buffer, buffer2, this.executionEvidence);
        }

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual void LoadSourceState(IVsaPersistSite site)
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(Pre.EngineNotInitialised | Pre.SiteSet | Pre.RootMonikerSet | Pre.EngineNotClosed);
                this.isEngineInitialized = true;
                try
                {
                    this.DoLoadSourceState(site);
                }
                catch
                {
                    this.isEngineInitialized = false;
                    throw;
                }
                this.isEngineDirty = false;
            }
            finally
            {
                this.ReleaseLock();
            }
        }

        protected void Preconditions(Pre flags)
        {
            if (this.isClosed)
            {
                throw this.Error(VsaError.EngineClosed);
            }
            if (flags != Pre.EngineNotClosed)
            {
                if (this.IsCondition(flags, Pre.SupportForDebug) && !this.isDebugInfoSupported)
                {
                    throw this.Error(VsaError.DebugInfoNotSupported);
                }
                if (this.IsCondition(flags, Pre.EngineCompiled) && !this.haveCompiledState)
                {
                    throw this.Error(VsaError.EngineNotCompiled);
                }
                if (this.IsCondition(flags, Pre.EngineRunning) && !this.isEngineRunning)
                {
                    throw this.Error(VsaError.EngineNotRunning);
                }
                if (this.IsCondition(flags, Pre.EngineNotRunning) && this.isEngineRunning)
                {
                    throw this.Error(VsaError.EngineRunning);
                }
                if (this.IsCondition(flags, Pre.RootMonikerSet) && (this.engineMoniker == ""))
                {
                    throw this.Error(VsaError.RootMonikerNotSet);
                }
                if (this.IsCondition(flags, Pre.RootMonikerNotSet) && (this.engineMoniker != ""))
                {
                    throw this.Error(VsaError.RootMonikerAlreadySet);
                }
                if (this.IsCondition(flags, Pre.RootNamespaceSet) && (this.rootNamespace == ""))
                {
                    throw this.Error(VsaError.RootNamespaceNotSet);
                }
                if (this.IsCondition(flags, Pre.SiteSet) && (this.engineSite == null))
                {
                    throw this.Error(VsaError.SiteNotSet);
                }
                if (this.IsCondition(flags, Pre.SiteNotSet) && (this.engineSite != null))
                {
                    throw this.Error(VsaError.SiteAlreadySet);
                }
                if (this.IsCondition(flags, Pre.EngineInitialised) && !this.isEngineInitialized)
                {
                    throw this.Error(VsaError.EngineNotInitialized);
                }
                if (this.IsCondition(flags, Pre.EngineNotInitialised) && this.isEngineInitialized)
                {
                    throw this.Error(VsaError.EngineInitialized);
                }
            }
        }

        internal void ReleaseLock()
        {
            Monitor.Exit(this);
        }

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual void Reset()
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(Pre.EngineRunning | Pre.EngineNotClosed);
                try
                {
                    this.startupInstance.Shutdown();
                }
                catch (Exception exception)
                {
                    throw new VsaException(VsaError.EngineCannotReset, exception.ToString(), exception);
                }
                catch
                {
                    throw new VsaException(VsaError.EngineCannotReset);
                }
                this.isEngineRunning = false;
                this.loadedAssembly = null;
            }
            finally
            {
                this.ReleaseLock();
            }
        }

        protected abstract void ResetCompiledState();
        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual void RevokeCache()
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(Pre.RootMonikerSet | Pre.EngineNotRunning | Pre.EngineNotClosed);
                try
                {
                    System.AppDomain.CurrentDomain.SetData(this.engineMoniker, null);
                }
                catch (Exception exception)
                {
                    throw new VsaException(VsaError.RevokeFailed, exception.ToString(), exception);
                }
                catch
                {
                    throw new VsaException(VsaError.RevokeFailed);
                }
            }
            finally
            {
                this.ReleaseLock();
            }
        }

        [PermissionSet(SecurityAction.Demand, Name="FullTrust"), PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual void Run()
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(Pre.SiteSet | Pre.RootNamespaceSet | Pre.RootMonikerSet | Pre.EngineNotRunning | Pre.EngineNotClosed);
                System.AppDomain currentDomain = System.AppDomain.CurrentDomain;
                if (this.haveCompiledState)
                {
                    if (this.rootNamespace != this.compiledRootNamespace)
                    {
                        throw new VsaException(VsaError.RootNamespaceInvalid);
                    }
                    this.loadedAssembly = this.LoadCompiledState();
                    currentDomain.SetData(this.engineMoniker, this.loadedAssembly);
                }
                else
                {
                    if (this.failedCompilation)
                    {
                        throw new VsaException(VsaError.EngineNotCompiled);
                    }
                    this.startupClass = null;
                    this.loadedAssembly = currentDomain.GetData(this.engineMoniker) as System.Reflection.Assembly;
                    if (this.loadedAssembly == null)
                    {
                        string name = this.engineMoniker + "/" + currentDomain.GetHashCode().ToString(CultureInfo.InvariantCulture);
                        Mutex mutex = new Mutex(false, name);
                        if (mutex.WaitOne())
                        {
                            try
                            {
                                this.loadedAssembly = currentDomain.GetData(this.engineMoniker) as System.Reflection.Assembly;
                                if (this.loadedAssembly == null)
                                {
                                    byte[] buffer;
                                    byte[] buffer2;
                                    this.engineSite.GetCompiledState(out buffer, out buffer2);
                                    if (buffer == null)
                                    {
                                        throw new VsaException(VsaError.GetCompiledStateFailed);
                                    }
                                    this.loadedAssembly = System.Reflection.Assembly.Load(buffer, buffer2, this.executionEvidence);
                                    currentDomain.SetData(this.engineMoniker, this.loadedAssembly);
                                }
                            }
                            finally
                            {
                                mutex.ReleaseMutex();
                                mutex.Close();
                            }
                        }
                    }
                }
                try
                {
                    if (this.startupClass == null)
                    {
                        this.startupClass = this.loadedAssembly.GetType(this.rootNamespace + "._Startup", true);
                    }
                }
                catch (Exception exception)
                {
                    throw new VsaException(VsaError.BadAssembly, exception.ToString(), exception);
                }
                catch
                {
                    throw new VsaException(VsaError.BadAssembly);
                }
                try
                {
                    this.startupInstance = (BaseVsaStartup) Activator.CreateInstance(this.startupClass);
                    this.isEngineRunning = true;
                    this.startupInstance.SetSite(this.engineSite);
                    this.startupInstance.Startup();
                }
                catch (Exception exception2)
                {
                    throw new VsaException(VsaError.UnknownError, exception2.ToString(), exception2);
                }
                catch
                {
                    throw new VsaException(VsaError.UnknownError);
                }
            }
            finally
            {
                this.ReleaseLock();
            }
        }

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual void SaveCompiledState(out byte[] pe, out byte[] debugInfo)
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotRunning | Pre.EngineCompiled | Pre.EngineNotClosed);
                this.DoSaveCompiledState(out pe, out debugInfo);
            }
            finally
            {
                this.ReleaseLock();
            }
        }

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual void SaveSourceState(IVsaPersistSite site)
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotRunning | Pre.EngineNotClosed);
                if (site == null)
                {
                    throw this.Error(VsaError.SiteInvalid);
                }
                try
                {
                    this.DoSaveSourceState(site);
                }
                catch (Exception exception)
                {
                    throw new VsaException(VsaError.SaveElementFailed, exception.ToString(), exception);
                }
                catch
                {
                    throw new VsaException(VsaError.SaveElementFailed);
                }
            }
            finally
            {
                this.ReleaseLock();
            }
        }

        protected abstract void SetCustomOption(string name, object value);
        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual void SetOption(string name, object value)
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotRunning | Pre.EngineNotClosed);
                this.SetCustomOption(name, value);
            }
            finally
            {
                this.ReleaseLock();
            }
        }

        internal void TryObtainLock()
        {
            if (!Monitor.TryEnter(this))
            {
                throw new VsaException(VsaError.EngineBusy);
            }
        }

        protected virtual void ValidateRootMoniker(string rootMoniker)
        {
            if (rootMoniker == null)
            {
                throw new VsaException(VsaError.RootMonikerInvalid);
            }
            Uri uri = null;
            try
            {
                uri = new Uri(rootMoniker);
            }
            catch (UriFormatException)
            {
                throw new VsaException(VsaError.RootMonikerInvalid);
            }
            string scheme = uri.Scheme;
            if (scheme.Length == 0)
            {
                throw new VsaException(VsaError.RootMonikerProtocolInvalid);
            }
            string[] subKeyNames = new string[] { 
                "file", "ftp", "gopher", "http", "https", "javascript", "mailto", "microsoft", "news", "res", "smtp", "socks", "vbscript", "xlang", "xml", "xpath",
                "xsd", "xsl"
            };
            try
            {
                new RegistryPermission(RegistryPermissionAccess.Read, @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\PROTOCOLS\Handler").Assert();
                subKeyNames = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes\PROTOCOLS\Handler").GetSubKeyNames();
            }
            catch
            {
            }
            foreach (string str2 in subKeyNames)
            {
                if (string.Compare(str2, scheme, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    throw new VsaException(VsaError.RootMonikerProtocolInvalid);
                }
            }
        }

        public _AppDomain AppDomain
        {
            get
            {
                this.Preconditions(Pre.EngineNotClosed);
                throw new NotSupportedException();
            }
            set
            {
                this.Preconditions(Pre.EngineNotClosed);
                throw new VsaException(VsaError.AppDomainCannotBeSet);
            }
        }

        public string ApplicationBase
        {
            get
            {
                this.Preconditions(Pre.EngineNotClosed);
                throw new NotSupportedException();
            }
            set
            {
                this.Preconditions(Pre.EngineNotClosed);
                throw new VsaException(VsaError.ApplicationBaseCannotBeSet);
            }
        }

        public System.Reflection.Assembly Assembly
        {
            get
            {
                this.Preconditions(Pre.EngineRunning | Pre.EngineNotClosed);
                return this.loadedAssembly;
            }
        }

        public System.Security.Policy.Evidence Evidence
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust"), SecurityPermission(SecurityAction.Demand, ControlEvidence=true)]
            get
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotClosed);
                return this.executionEvidence;
            }
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust"), SecurityPermission(SecurityAction.Demand, ControlEvidence=true)]
            set
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotRunning | Pre.EngineNotClosed);
                this.executionEvidence = value;
            }
        }

        public bool GenerateDebugInfo
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotClosed);
                return this.genDebugInfo;
            }
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            set
            {
                this.TryObtainLock();
                try
                {
                    this.Preconditions(Pre.EngineInitialised | Pre.EngineNotRunning | Pre.SupportForDebug | Pre.EngineNotClosed);
                    if (this.genDebugInfo != value)
                    {
                        this.genDebugInfo = value;
                        this.isEngineDirty = true;
                        this.isEngineCompiled = false;
                    }
                }
                finally
                {
                    this.ReleaseLock();
                }
            }
        }

        public bool IsCompiled
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotClosed);
                return this.isEngineCompiled;
            }
        }

        public bool IsDirty
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotClosed);
                return this.isEngineDirty;
            }
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            set
            {
                this.TryObtainLock();
                try
                {
                    this.Preconditions(Pre.EngineNotClosed);
                    this.isEngineDirty = value;
                    if (this.isEngineDirty)
                    {
                        this.isEngineCompiled = false;
                    }
                }
                finally
                {
                    this.ReleaseLock();
                }
            }
        }

        public bool IsRunning
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotClosed);
                return this.isEngineRunning;
            }
        }

        public IVsaItems Items
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotClosed);
                return this.vsaItems;
            }
        }

        public string Language
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotClosed);
                return this.scriptLanguage;
            }
        }

        public int LCID
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotClosed);
                return this.errorLocale;
            }
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            set
            {
                this.TryObtainLock();
                try
                {
                    this.Preconditions(Pre.EngineInitialised | Pre.EngineNotRunning | Pre.EngineNotClosed);
                    try
                    {
                        new CultureInfo(value);
                    }
                    catch (ArgumentException)
                    {
                        throw this.Error(VsaError.LCIDNotSupported);
                    }
                    this.errorLocale = value;
                    this.isEngineDirty = true;
                }
                finally
                {
                    this.ReleaseLock();
                }
            }
        }

        public string Name
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotClosed);
                return this.engineName;
            }
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            set
            {
                this.TryObtainLock();
                try
                {
                    this.Preconditions(Pre.EngineInitialised | Pre.EngineNotRunning | Pre.EngineNotClosed);
                    if (this.engineName != value)
                    {
                        lock (nameTable)
                        {
                            if (nameTable[value] != null)
                            {
                                throw this.Error(VsaError.EngineNameInUse);
                            }
                            nameTable[value] = new object();
                            if ((this.engineName != null) && (this.engineName.Length > 0))
                            {
                                nameTable[this.engineName] = null;
                            }
                        }
                        this.engineName = value;
                        this.isEngineDirty = true;
                        this.isEngineCompiled = false;
                    }
                }
                finally
                {
                    this.ReleaseLock();
                }
            }
        }

        public string RootMoniker
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                this.Preconditions(Pre.EngineNotClosed);
                return this.engineMoniker;
            }
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            set
            {
                this.TryObtainLock();
                try
                {
                    this.Preconditions(Pre.RootMonikerNotSet | Pre.EngineNotClosed);
                    this.ValidateRootMoniker(value);
                    this.engineMoniker = value;
                }
                finally
                {
                    this.ReleaseLock();
                }
            }
        }

        public string RootNamespace
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotClosed);
                return this.rootNamespace;
            }
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            set
            {
                this.TryObtainLock();
                try
                {
                    this.Preconditions(Pre.EngineInitialised | Pre.EngineNotRunning | Pre.EngineNotClosed);
                    if (!this.IsValidNamespaceName(value))
                    {
                        throw this.Error(VsaError.RootNamespaceInvalid);
                    }
                    this.rootNamespace = value;
                    this.isEngineDirty = true;
                    this.isEngineCompiled = false;
                }
                finally
                {
                    this.ReleaseLock();
                }
            }
        }

        public IVsaSite Site
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                this.Preconditions(Pre.RootMonikerSet | Pre.EngineNotClosed);
                return this.engineSite;
            }
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            set
            {
                this.TryObtainLock();
                try
                {
                    this.Preconditions(Pre.SiteNotSet | Pre.RootMonikerSet | Pre.EngineNotClosed);
                    if (value == null)
                    {
                        throw this.Error(VsaError.SiteInvalid);
                    }
                    this.engineSite = value;
                }
                finally
                {
                    this.ReleaseLock();
                }
            }
        }

        public string Version
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                this.Preconditions(Pre.EngineInitialised | Pre.EngineNotClosed);
                return this.assemblyVersion;
            }
        }

        [Flags]
        protected enum Pre
        {
            EngineCompiled = 4,
            EngineInitialised = 0x400,
            EngineNotClosed = 1,
            EngineNotInitialised = 0x800,
            EngineNotRunning = 0x10,
            EngineRunning = 8,
            None = 0,
            RootMonikerNotSet = 0x40,
            RootMonikerSet = 0x20,
            RootNamespaceSet = 0x80,
            SiteNotSet = 0x200,
            SiteSet = 0x100,
            SupportForDebug = 2
        }
    }
}

