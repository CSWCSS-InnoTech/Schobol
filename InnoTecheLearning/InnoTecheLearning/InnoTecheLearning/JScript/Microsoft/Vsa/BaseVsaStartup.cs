namespace Microsoft.Vsa
{
    using System;

    [Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
    public abstract class BaseVsaStartup
    {
        protected IVsaSite site;

        protected BaseVsaStartup()
        {
        }

        public void SetSite(IVsaSite site)
        {
            this.site = site;
        }

        public abstract void Shutdown();
        public abstract void Startup();
    }
}

