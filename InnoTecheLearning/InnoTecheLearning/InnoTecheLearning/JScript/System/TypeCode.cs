using System;
using System.Runtime.InteropServices;

    /// <summary>Specifies the type of an object.</summary>
    /// <filterpriority>2</filterpriority>
    [__DynamicallyInvokable, ComVisible(true)]
    //[Serializable]
    public static class TypeCode
    {
        public const int Empty = 0;          // Null reference
        public const int Object = 1;         // Instance that isn't a value
        public const int DBNull = 2;         // Database null value
        public const int Boolean = 3;      // Boolean
        public const int Char = 4;         // Unicode character
        public const int SByte = 5;        // Signed 8-bit integer
        public const int Byte = 6;         // Unsigned 8-bit integer
        public const int Int16 = 7;        // Signed 16-bit integer
        public const int UInt16 = 8;       // Unsigned 16-bit integer
        public const int Int32 = 9;        // Signed 32-bit integer
        public const int UInt32 = 10;      // Unsigned 32-bit integer
        public const int Int64 = 11;       // Signed 64-bit integer
        public const int UInt64 = 12;      // Unsigned 64-bit integer
        public const int Single = 13;      // IEEE 32-bit float
        public const int Double = 14;      // IEEE 64-bit double
        public const int Decimal = 15;     // Decimal
        public const int DateTime = 16;    // DateTime
        public const int String = 18;      // Unicode character string
    }

[AttributeUsage(AttributeTargets.All, Inherited = false)]
internal sealed class __DynamicallyInvokableAttribute : Attribute
{
}
