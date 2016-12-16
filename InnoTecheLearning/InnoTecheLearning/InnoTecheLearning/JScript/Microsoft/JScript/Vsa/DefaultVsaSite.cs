namespace Microsoft.JScript.Vsa
{
    using Microsoft.JScript;
    using Microsoft.Vsa;
    using System;

    internal class DefaultVsaSite : BaseVsaSite
    {
        public override bool OnCompilerError(IVsaError error)
        {
            throw ((JScriptException) error);
        }
    }
}

