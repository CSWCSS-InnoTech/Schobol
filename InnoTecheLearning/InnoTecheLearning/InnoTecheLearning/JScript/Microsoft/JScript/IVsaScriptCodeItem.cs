namespace Microsoft.JScript
{
    using Microsoft.Vsa;
    using System;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("E0C0FFE8-7eea-4ee5-b7e4-0080c7eb0b74"), ComVisible(true)]
    public interface IVsaScriptCodeItem : IVsaCodeItem, IVsaItem
    {
        int StartLine { get; set; }
        int StartColumn { get; set; }
        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        object Execute();
    }
}

