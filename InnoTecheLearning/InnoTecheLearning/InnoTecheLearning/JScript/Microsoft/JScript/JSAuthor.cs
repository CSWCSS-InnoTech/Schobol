namespace Microsoft.JScript
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;

    [ComVisible(true), Guid("0E4EFFC0-2387-11d3-B372-00105A98B7CE")]
    public class JSAuthor : IAuthorServices
    {
        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual IParseText GetCodeSense() => 
            new JSCodeSense();

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual IColorizeText GetColorizer() => 
            new JSColorizer();
    }
}

