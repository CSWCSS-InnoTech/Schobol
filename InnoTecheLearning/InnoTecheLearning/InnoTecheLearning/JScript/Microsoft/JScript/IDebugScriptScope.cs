namespace Microsoft.JScript
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    [ComVisible(true), Guid("59447635-3E26-4873-BF26-05F173B80F5E")]
    public interface IDebugScriptScope
    {
        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        void SetThisValue([MarshalAs(UnmanagedType.Interface)] object thisValue);
    }
}

