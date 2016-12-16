namespace System.Security.Policy
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable, ComVisible(true), Flags]
    public enum PolicyStatementAttribute
    {
        Nothing,
        Exclusive,
        LevelFinal,
        All
    }
}

