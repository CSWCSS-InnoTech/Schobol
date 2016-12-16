namespace System.Reflection.Emit
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal sealed class FieldOnTypeBuilderInstantiation : System.Reflection.FieldInfo
    {
        private System.Reflection.FieldInfo m_field;
        private static Hashtable m_hashtable = new Hashtable();
        private TypeBuilderInstantiation m_type;

        internal FieldOnTypeBuilderInstantiation(System.Reflection.FieldInfo field, TypeBuilderInstantiation type)
        {
            this.m_field = field;
            this.m_type = type;
        }

        public override object[] GetCustomAttributes(bool inherit) => 
            this.m_field.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => 
            this.m_field.GetCustomAttributes(attributeType, inherit);

        internal static System.Reflection.FieldInfo GetField(System.Reflection.FieldInfo Field, TypeBuilderInstantiation type)
        {
            Entry key = new Entry(Field, type);
            if (m_hashtable.Contains(key))
            {
                return (m_hashtable[key] as System.Reflection.FieldInfo);
            }
            System.Reflection.FieldInfo info = new FieldOnTypeBuilderInstantiation(Field, type);
            m_hashtable[key] = info;
            return info;
        }

        public override Type[] GetOptionalCustomModifiers() => 
            this.m_field.GetOptionalCustomModifiers();

        public override Type[] GetRequiredCustomModifiers() => 
            this.m_field.GetRequiredCustomModifiers();

        public Type GetType() => 
            base.GetType();

        public override object GetValue(object obj)
        {
            throw new InvalidOperationException();
        }

        public override object GetValueDirect(TypedReference obj)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit) => 
            this.m_field.IsDefined(attributeType, inherit);

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }

        public override void SetValueDirect(TypedReference obj, object value)
        {
            throw new NotImplementedException();
        }

        public override FieldAttributes Attributes =>
            this.m_field.Attributes;

        public override Type DeclaringType =>
            this.m_type;

        public override RuntimeFieldHandle FieldHandle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        internal System.Reflection.FieldInfo FieldInfo =>
            this.m_field;

        public override Type FieldType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MemberTypes MemberType =>
            MemberTypes.Field;

        internal override int MetadataTokenInternal
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override System.Reflection.Module Module =>
            this.m_field.Module;

        public override string Name =>
            this.m_field.Name;

        public override Type ReflectedType =>
            this.m_type;

        [StructLayout(LayoutKind.Sequential)]
        private struct Entry
        {
            public FieldInfo m_field;
            public TypeBuilderInstantiation m_type;
            public Entry(FieldInfo Field, TypeBuilderInstantiation type)
            {
                this.m_field = Field;
                this.m_type = type;
            }

            public override int GetHashCode() => 
                this.m_field.GetHashCode();

            public override bool Equals(object o) => 
                ((o is FieldOnTypeBuilderInstantiation.Entry) && this.Equals((FieldOnTypeBuilderInstantiation.Entry) o));

            public bool Equals(FieldOnTypeBuilderInstantiation.Entry obj) => 
                ((obj.m_field == this.m_field) && (obj.m_type == this.m_type));
        }
    }
}

