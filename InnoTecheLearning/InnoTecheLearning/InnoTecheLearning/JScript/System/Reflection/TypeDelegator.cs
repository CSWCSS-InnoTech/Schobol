namespace System.Reflection
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;

    [Serializable, ComVisible(true)]
    public class TypeDelegator : Type
    {
        protected Type typeImpl;

        protected TypeDelegator()
        {
        }

        public TypeDelegator(Type delegatingType)
        {
            if (delegatingType == null)
            {
                throw new ArgumentNullException("delegatingType");
            }
            this.typeImpl = delegatingType;
        }

        protected override TypeAttributes GetAttributeFlagsImpl() => 
            this.typeImpl.Attributes;

        protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers) => 
            this.typeImpl.GetConstructor(bindingAttr, binder, callConvention, types, modifiers);

        [ComVisible(true)]
        public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) => 
            this.typeImpl.GetConstructors(bindingAttr);

        public override object[] GetCustomAttributes(bool inherit) => 
            this.typeImpl.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => 
            this.typeImpl.GetCustomAttributes(attributeType, inherit);

        public override Type GetElementType() => 
            this.typeImpl.GetElementType();

        public override EventInfo GetEvent(string name, BindingFlags bindingAttr) => 
            this.typeImpl.GetEvent(name, bindingAttr);

        public override EventInfo[] GetEvents() => 
            this.typeImpl.GetEvents();

        public override EventInfo[] GetEvents(BindingFlags bindingAttr) => 
            this.typeImpl.GetEvents(bindingAttr);

        public override FieldInfo GetField(string name, BindingFlags bindingAttr) => 
            this.typeImpl.GetField(name, bindingAttr);

        public override FieldInfo[] GetFields(BindingFlags bindingAttr) => 
            this.typeImpl.GetFields(bindingAttr);

        public override Type GetInterface(string name, bool ignoreCase) => 
            this.typeImpl.GetInterface(name, ignoreCase);

        [ComVisible(true)]
        public override InterfaceMapping GetInterfaceMap(Type interfaceType) => 
            this.typeImpl.GetInterfaceMap(interfaceType);

        public override Type[] GetInterfaces() => 
            this.typeImpl.GetInterfaces();

        public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr) => 
            this.typeImpl.GetMember(name, type, bindingAttr);

        public override MemberInfo[] GetMembers(BindingFlags bindingAttr) => 
            this.typeImpl.GetMembers(bindingAttr);

        protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
        {
            if (types == null)
            {
                return this.typeImpl.GetMethod(name, bindingAttr);
            }
            return this.typeImpl.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
        }

        public override MethodInfo[] GetMethods(BindingFlags bindingAttr) => 
            this.typeImpl.GetMethods(bindingAttr);

        public override Type GetNestedType(string name, BindingFlags bindingAttr) => 
            this.typeImpl.GetNestedType(name, bindingAttr);

        public override Type[] GetNestedTypes(BindingFlags bindingAttr) => 
            this.typeImpl.GetNestedTypes(bindingAttr);

        public override PropertyInfo[] GetProperties(BindingFlags bindingAttr) => 
            this.typeImpl.GetProperties(bindingAttr);

        protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            if ((returnType == null) && (types == null))
            {
                return this.typeImpl.GetProperty(name, bindingAttr);
            }
            return this.typeImpl.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
        }

        protected override bool HasElementTypeImpl() => 
            this.typeImpl.HasElementType;

        public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters) => 
            this.typeImpl.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);

        protected override bool IsArrayImpl() => 
            this.typeImpl.IsArray;

        protected override bool IsByRefImpl() => 
            this.typeImpl.IsByRef;

        protected override bool IsCOMObjectImpl() => 
            this.typeImpl.IsCOMObject;

        public override bool IsDefined(Type attributeType, bool inherit) => 
            this.typeImpl.IsDefined(attributeType, inherit);

        protected override bool IsPointerImpl() => 
            this.typeImpl.IsPointer;

        protected override bool IsPrimitiveImpl() => 
            this.typeImpl.IsPrimitive;

        protected override bool IsValueTypeImpl() => 
            this.typeImpl.IsValueType;

        public override System.Reflection.Assembly Assembly =>
            this.typeImpl.Assembly;

        public override string AssemblyQualifiedName =>
            this.typeImpl.AssemblyQualifiedName;

        public override Type BaseType =>
            this.typeImpl.BaseType;

        public override string FullName =>
            this.typeImpl.FullName;

        public override Guid GUID =>
            this.typeImpl.GUID;

        public override int MetadataToken =>
            this.typeImpl.MetadataToken;

        public override System.Reflection.Module Module =>
            this.typeImpl.Module;

        public override string Name =>
            this.typeImpl.Name;

        public override string Namespace =>
            this.typeImpl.Namespace;

        public override RuntimeTypeHandle TypeHandle =>
            this.typeImpl.TypeHandle;

        public override Type UnderlyingSystemType =>
            this.typeImpl.UnderlyingSystemType;
    }
}

