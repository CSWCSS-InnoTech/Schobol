namespace Microsoft.JScript
{
    using System;
    using System.Reflection;

    internal abstract class EnumWrapper : IConvertible
    {
        protected EnumWrapper()
        {
        }

        TypeCode IConvertible.GetTypeCode() => 
            Microsoft.JScript.Convert.GetTypeCode(this.value);

        bool IConvertible.ToBoolean(IFormatProvider provider) => 
            ((IConvertible) this.value).ToBoolean(provider);

        byte IConvertible.ToByte(IFormatProvider provider) => 
            ((IConvertible) this.value).ToByte(provider);

        char IConvertible.ToChar(IFormatProvider provider) => 
            ((IConvertible) this.value).ToChar(provider);

        DateTime IConvertible.ToDateTime(IFormatProvider provider) => 
            ((IConvertible) this.value).ToDateTime(provider);

        decimal IConvertible.ToDecimal(IFormatProvider provider) => 
            ((IConvertible) this.value).ToDecimal(provider);

        double IConvertible.ToDouble(IFormatProvider provider) => 
            ((IConvertible) this.value).ToDouble(provider);

        short IConvertible.ToInt16(IFormatProvider provider) => 
            ((IConvertible) this.value).ToInt16(provider);

        int IConvertible.ToInt32(IFormatProvider provider) => 
            ((IConvertible) this.value).ToInt32(provider);

        long IConvertible.ToInt64(IFormatProvider provider) => 
            ((IConvertible) this.value).ToInt64(provider);

        sbyte IConvertible.ToSByte(IFormatProvider provider) => 
            ((IConvertible) this.value).ToSByte(provider);

        float IConvertible.ToSingle(IFormatProvider provider) => 
            ((IConvertible) this.value).ToSingle(provider);

        string IConvertible.ToString(IFormatProvider provider) => 
            ((IConvertible) this.value).ToString(provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) => 
            ((IConvertible) this.value).ToType(conversionType, provider);

        ushort IConvertible.ToUInt16(IFormatProvider provider) => 
            ((IConvertible) this.value).ToUInt16(provider);

        uint IConvertible.ToUInt32(IFormatProvider provider) => 
            ((IConvertible) this.value).ToUInt32(provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider) => 
            ((IConvertible) this.value).ToUInt64(provider);

        internal object ToNumericValue() => 
            this.value;

        public override string ToString()
        {
            if (this.name != null)
            {
                return this.name;
            }
            foreach (FieldInfo info in this.type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (StrictEquality.JScriptStrictEquals(this.value, info.GetValue(null)))
                {
                    return info.Name;
                }
            }
            return Microsoft.JScript.Convert.ToString(this.value);
        }

        internal virtual IReflect classScopeOrType =>
            this.type;

        internal abstract string name { get; }

        internal abstract Type type { get; }

        internal abstract object value { get; }
    }
}

