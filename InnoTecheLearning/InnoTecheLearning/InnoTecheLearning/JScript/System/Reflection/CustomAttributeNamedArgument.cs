namespace System.Reflection
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential), ComVisible(true)]
    public struct CustomAttributeNamedArgument
    {
        private System.Reflection.MemberInfo m_memberInfo;
        private CustomAttributeTypedArgument m_value;
        public static bool operator ==(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right) => 
            left.Equals(right);

        public static bool operator !=(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right) => 
            !left.Equals(right);

        internal CustomAttributeNamedArgument(System.Reflection.MemberInfo memberInfo, object value)
        {
            this.m_memberInfo = memberInfo;
            this.m_value = new CustomAttributeTypedArgument(value);
        }

        internal CustomAttributeNamedArgument(System.Reflection.MemberInfo memberInfo, CustomAttributeTypedArgument value)
        {
            this.m_memberInfo = memberInfo;
            this.m_value = value;
        }

        public override string ToString() => 
            string.Format(CultureInfo.CurrentCulture, "{0} = {1}", new object[] { this.MemberInfo.Name, this.TypedValue.ToString(this.ArgumentType != typeof(object)) });

        public override int GetHashCode() => 
            base.GetHashCode();

        public override bool Equals(object obj) => 
            (obj == this);

        internal Type ArgumentType
        {
            get
            {
                if (this.m_memberInfo is FieldInfo)
                {
                    return ((FieldInfo) this.m_memberInfo).FieldType;
                }
                return ((PropertyInfo) this.m_memberInfo).PropertyType;
            }
        }
        public System.Reflection.MemberInfo MemberInfo =>
            this.m_memberInfo;
        public CustomAttributeTypedArgument TypedValue =>
            this.m_value;
    }
}

