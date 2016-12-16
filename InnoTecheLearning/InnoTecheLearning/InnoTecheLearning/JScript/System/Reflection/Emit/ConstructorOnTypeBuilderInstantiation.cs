namespace System.Reflection.Emit
{
    using System;
    using System.Globalization;
    using System.Reflection;

    internal sealed class ConstructorOnTypeBuilderInstantiation : ConstructorInfo
    {
        internal ConstructorInfo m_ctor;
        private TypeBuilderInstantiation m_type;

        internal ConstructorOnTypeBuilderInstantiation(ConstructorInfo constructor, TypeBuilderInstantiation type)
        {
            this.m_ctor = constructor;
            this.m_type = type;
        }

        internal static ConstructorInfo GetConstructor(ConstructorInfo Constructor, TypeBuilderInstantiation type) => 
            new ConstructorOnTypeBuilderInstantiation(Constructor, type);

        public override object[] GetCustomAttributes(bool inherit) => 
            this.m_ctor.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => 
            this.m_ctor.GetCustomAttributes(attributeType, inherit);

        public override Type[] GetGenericArguments() => 
            this.m_ctor.GetGenericArguments();

        public override MethodImplAttributes GetMethodImplementationFlags() => 
            this.m_ctor.GetMethodImplementationFlags();

        public override ParameterInfo[] GetParameters() => 
            this.m_ctor.GetParameters();

        internal override Type[] GetParameterTypes() => 
            this.m_ctor.GetParameterTypes();

        public Type GetType() => 
            base.GetType();

        public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit) => 
            this.m_ctor.IsDefined(attributeType, inherit);

        public override MethodAttributes Attributes =>
            this.m_ctor.Attributes;

        public override CallingConventions CallingConvention =>
            this.m_ctor.CallingConvention;

        public override bool ContainsGenericParameters =>
            false;

        public override Type DeclaringType =>
            this.m_type;

        public override bool IsGenericMethod =>
            false;

        public override bool IsGenericMethodDefinition =>
            false;

        public override MemberTypes MemberType =>
            this.m_ctor.MemberType;

        internal override int MetadataTokenInternal
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override RuntimeMethodHandle MethodHandle =>
            this.m_ctor.MethodHandle;

        public override System.Reflection.Module Module =>
            this.m_ctor.Module;

        public override string Name =>
            this.m_ctor.Name;

        public override Type ReflectedType =>
            this.m_type;
    }
}

