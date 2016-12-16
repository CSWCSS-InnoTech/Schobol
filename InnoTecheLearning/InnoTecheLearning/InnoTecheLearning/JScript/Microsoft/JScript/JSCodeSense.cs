namespace Microsoft.JScript
{
    using Microsoft.JScript.Vsa;
    using Microsoft.Vsa;
    using System;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
    internal class JSCodeSense : IVsaSite, IParseText
    {
        private IVsaCodeItem _codeBlock;
        private VsaEngine _engine = new VsaEngine(true);
        private IErrorHandler _errorHandler;

        internal JSCodeSense()
        {
            this._engine.InitVsaEngine("JSC://Microsoft.JScript.Vsa.VsaEngine", this);
            this._codeBlock = (IVsaCodeItem) this._engine.Items.CreateItem("Code", VsaItemType.Code, VsaItemFlag.None);
            this._errorHandler = null;
        }

        public virtual void GetCompiledState(out byte[] pe, out byte[] debugInfo)
        {
            pe = null;
            debugInfo = null;
        }

        public virtual object GetEventSourceInstance(string ItemName, string EventSourceName) => 
            null;

        public virtual object GetGlobalInstance(string Name) => 
            null;

        public virtual void Notify(string notification, object value)
        {
        }

        public virtual bool OnCompilerError(IVsaError error)
        {
            if (error is IVsaFullErrorInfo)
            {
                return this._errorHandler.OnCompilerError((IVsaFullErrorInfo) error);
            }
            return true;
        }

        public virtual void Parse(string code, IErrorHandler errorHandler)
        {
            this._engine.Reset();
            this._errorHandler = errorHandler;
            this._codeBlock.SourceText = code;
            this._engine.CheckForErrors();
        }
    }
}

