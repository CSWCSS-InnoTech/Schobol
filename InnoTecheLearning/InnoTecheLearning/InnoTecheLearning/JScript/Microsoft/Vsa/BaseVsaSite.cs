namespace Microsoft.Vsa
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    [Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
    public class BaseVsaSite : IVsaSite
    {
        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual void GetCompiledState(out byte[] pe, out byte[] debugInfo)
        {
            pe = this.Assembly;
            debugInfo = this.DebugInfo;
        }

        public virtual object GetEventSourceInstance(string itemName, string eventSourceName)
        {
            throw new VsaException(VsaError.CallbackUnexpected);
        }

        public virtual object GetGlobalInstance(string name)
        {
            throw new VsaException(VsaError.CallbackUnexpected);
        }

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual void Notify(string notify, object optional)
        {
            throw new VsaException(VsaError.CallbackUnexpected);
        }

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual bool OnCompilerError(IVsaError error) => 
            false;

        public virtual byte[] Assembly
        {
            get
            {
                throw new VsaException(VsaError.GetCompiledStateFailed);
            }
        }

        public virtual byte[] DebugInfo =>
            null;
    }
}

