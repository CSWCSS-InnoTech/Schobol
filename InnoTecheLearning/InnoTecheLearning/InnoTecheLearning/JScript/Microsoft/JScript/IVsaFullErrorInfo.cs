namespace Microsoft.JScript
{
    using Microsoft.Vsa;
    using System;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("DC3691BC-F188-4b67-8338-326671E0F3F6")]
    public interface IVsaFullErrorInfo : IVsaError
    {
        int EndLine { get; }
    }
}

