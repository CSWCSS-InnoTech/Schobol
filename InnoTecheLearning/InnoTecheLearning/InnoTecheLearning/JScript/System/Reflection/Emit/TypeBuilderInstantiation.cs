namespace System.Reflection.Emit
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal sealed class TypeBuilderInstantiation : Type
    {
        private Type[] m_inst;
        private string m_strFullQualName;
        private Type m_type;

        internal TypeBuilderInstantiation(Type type, Type[] inst)
        {
            this.m_type = type;
            this.m_inst = inst;
        }

        protected override TypeAttributes GetAttributeFlagsImpl() => 
            this.m_type.Attributes;

        protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            throw new NotSupportedException();
        }

        [ComVisible(true)]
        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr)
        {
            throw new NotSupportedException();
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotSupportedException();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotSupportedException();
        }

        public override Type GetElementType()
        {
            throw new NotSupportedException();
        }

        public override EventInfo GetEvent(string name, BindingFlags bindingAttr)
        {
            throw new NotSupportedException();
        }

        public override EventInfo[] GetEvents()
        {
            throw new NotSupportedException();
        }

        public override EventInfo[] GetEvents(BindingFlags bindingAttr)
        {
            throw new NotSupportedException();
        }

        public override FieldInfo GetField(string name, BindingFlags bindingAttr)
        {
            throw new NotSupportedException();
        }

        public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            throw new NotSupportedException();
        }

        public override Type[] GetGenericArguments() => 
            this.m_inst;

        public override Type GetGenericTypeDefinition() => 
            this.m_type;

        public override Type GetInterface(string name, bool ignoreCase)
        {
            throw new NotSupportedException();
        }

        [ComVisible(true)]
        public override InterfaceMapping GetInterfaceMap(Type interfaceType)
        {
            throw new NotSupportedException();
        }

        public override Type[] GetInterfaces()
        {
            throw new NotSupportedException();
        }

        public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
        {
            throw new NotSupportedException();
        }

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            throw new NotSupportedException();
        }

        protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            throw new NotSupportedException();
        }

        public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            throw new NotSupportedException();
        }

        public override Type GetNestedType(string name, BindingFlags bindingAttr)
        {
            throw new NotSupportedException();
        }

        public override Type[] GetNestedTypes(BindingFlags bindingAttr)
        {
            throw new NotSupportedException();
        }

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        {
            throw new NotSupportedException();
        }

        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            throw new NotSupportedException();
        }

        protected override bool HasElementTypeImpl() => 
            false;

        public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
        {
            throw new NotSupportedException();
        }

        protected override bool IsArrayImpl() => 
            false;

        public override bool IsAssignableFrom(Type c)
        {
            throw new NotSupportedException();
        }

        protected override bool IsByRefImpl() => 
            false;

        protected override bool IsCOMObjectImpl() => 
            false;

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotSupportedException();
        }

        protected override bool IsPointerImpl() => 
            false;

        protected override bool IsPrimitiveImpl() => 
            false;

        [ComVisible(true)]
        public override bool IsSubclassOf(Type c)
        {
            throw new NotSupportedException();
        }

        protected override bool IsValueTypeImpl() => 
            this.m_type.IsValueType;

        public override Type MakeArrayType() => 
            SymbolType.FormCompoundType("[]".ToCharArray(), this, 0);

        public override Type MakeArrayType(int rank)
        {
            if (rank <= 0)
            {
                throw new IndexOutOfRangeException();
            }
            string str = "";
            for (int i = 1; i < rank; i++)
            {
                str = str + ",";
            }
            return SymbolType.FormCompoundType(string.Format(CultureInfo.InvariantCulture, "[{0}]", new object[] { str }).ToCharArray(), this, 0);
        }

        public override Type MakeByRefType() => 
            SymbolType.FormCompoundType("&".ToCharArray(), this, 0);

        public override Type MakeGenericType(params Type[] inst)
        {
            throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericTypeDefinition"));
        }

        public override Type MakePointerType() => 
            SymbolType.FormCompoundType("*".ToCharArray(), this, 0);

        internal Type Substitute(Type[] substitutes)
        {
            Type[] genericArguments = this.GetGenericArguments();
            Type[] typeArguments = new Type[genericArguments.Length];
            for (int i = 0; i < typeArguments.Length; i++)
            {
                Type type = genericArguments[i];
                if (type is TypeBuilderInstantiation)
                {
                    typeArguments[i] = (type as TypeBuilderInstantiation).Substitute(substitutes);
                }
                else if (type is GenericTypeParameterBuilder)
                {
                    typeArguments[i] = substitutes[type.GenericParameterPosition];
                }
                else
                {
                    typeArguments[i] = type;
                }
            }
            return this.GetGenericTypeDefinition().MakeGenericType(typeArguments);
        }

        public override string ToString() => 
            TypeNameBuilder.ToString(this, TypeNameBuilder.Format.ToString);

        public override System.Reflection.Assembly Assembly =>
            this.m_type.Assembly;

        public override string AssemblyQualifiedName =>
            TypeNameBuilder.ToString(this, TypeNameBuilder.Format.AssemblyQualifiedName);

        public override Type BaseType
        {
            get
            {
                Type baseType = this.m_type.BaseType;
                if (baseType == null)
                {
                    return null;
                }
                TypeBuilderInstantiation instantiation = baseType as TypeBuilderInstantiation;
                return instantiation?.Substitute(this.GetGenericArguments());
            }
        }

        public override bool ContainsGenericParameters
        {
            get
            {
                for (int i = 0; i < this.m_inst.Length; i++)
                {
                    if (this.m_inst[i].ContainsGenericParameters)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public override MethodBase DeclaringMethod =>
            null;

        public override Type DeclaringType =>
            this.m_type.DeclaringType;

        public override string FullName
        {
            get
            {
                if (this.m_strFullQualName == null)
                {
                    this.m_strFullQualName = TypeNameBuilder.ToString(this, TypeNameBuilder.Format.FullName);
                }
                return this.m_strFullQualName;
            }
        }

        public override int GenericParameterPosition
        {
            get
            {
                throw new InvalidOperationException();
            }
        }

        public override Guid GUID
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override bool IsGenericParameter =>
            false;

        public override bool IsGenericType =>
            true;

        public override bool IsGenericTypeDefinition =>
            false;

        internal override int MetadataTokenInternal
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override System.Reflection.Module Module =>
            this.m_type.Module;

        public override string Name =>
            this.m_type.Name;

        public override string Namespace =>
            this.m_type.Namespace;

        public override Type ReflectedType =>
            this.m_type.ReflectedType;

        public override RuntimeTypeHandle TypeHandle
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        public override Type UnderlyingSystemType =>
            this;
    }
}

