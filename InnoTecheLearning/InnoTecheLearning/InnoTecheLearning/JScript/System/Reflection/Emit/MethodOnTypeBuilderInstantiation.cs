namespace System.Reflection.Emit
{
    using System;
    using System.Globalization;
    using System.Reflection;

    internal sealed class MethodOnTypeBuilderInstantiation : MethodInfo
    {
        internal MethodInfo m_method;
        private TypeBuilderInstantiation m_type;

        internal MethodOnTypeBuilderInstantiation(MethodInfo method, TypeBuilderInstantiation type)
        {
            this.m_method = method;
            this.m_type = type;
        }

        public override MethodInfo GetBaseDefinition()
        {
            throw new NotSupportedException();
        }

        public override object[] GetCustomAttributes(bool inherit) => 
            this.m_method.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => 
            this.m_method.GetCustomAttributes(attributeType, inherit);

        public override Type[] GetGenericArguments() => 
            this.m_method.GetGenericArguments();

        public override MethodInfo GetGenericMethodDefinition() => 
            this.m_method;

        internal static MethodInfo GetMethod(MethodInfo method, TypeBuilderInstantiation type) => 
            new MethodOnTypeBuilderInstantiation(method, type);

        public override MethodImplAttributes GetMethodImplementationFlags() => 
            this.m_method.GetMethodImplementationFlags();

        public override ParameterInfo[] GetParameters() => 
            this.m_method.GetParameters();

        internal override Type[] GetParameterTypes() => 
            this.m_method.GetParameterTypes();

        internal override Type GetReturnType() => 
            this.m_method.GetReturnType();

        public Type GetType() => 
            base.GetType();

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit) => 
            this.m_method.IsDefined(attributeType, inherit);

        public override MethodInfo MakeGenericMethod(params Type[] typeArgs)
        {
            if (!this.IsGenericMethodDefinition)
            {
                throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericMethodDefinition"));
            }
            return MethodBuilderInstantiation.MakeGenericMethod(this, typeArgs);
        }

        public override MethodAttributes Attributes =>
            this.m_method.Attributes;

        public override CallingConventions CallingConvention =>
            this.m_method.CallingConvention;

        public override bool ContainsGenericParameters =>
            this.m_method.ContainsGenericParameters;

        public override Type DeclaringType =>
            this.m_type;

        public override bool IsGenericMethod =>
            this.m_method.IsGenericMethod;

        public override bool IsGenericMethodDefinition =>
            this.m_method.IsGenericMethodDefinition;

        public override MemberTypes MemberType =>
            this.m_method.MemberType;

        internal override int MetadataTokenInternal =>
            this.m_method.MetadataTokenInternal;

        public override RuntimeMethodHandle MethodHandle =>
            this.m_method.MethodHandle;

        public override System.Reflection.Module Module =>
            this.m_method.Module;

        public override string Name =>
            this.m_method.Name;

        public override Type ReflectedType =>
            this.m_type;

        public override ParameterInfo ReturnParameter
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override ICustomAttributeProvider ReturnTypeCustomAttributes
        {
            get
            {
                throw new NotSupportedException();
            }
        }
    }
}

