namespace Microsoft.JScript
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;

    public sealed class JSConstructor : ConstructorInfo
    {
        internal FunctionObject cons;

        internal JSConstructor(FunctionObject cons)
        {
            this.cons = cons;
        }

        internal object Construct(object thisob, object[] args) => 
            LateBinding.CallValue(this.cons, args, true, false, this.cons.engine, thisob, JSBinder.ob, null, null);

        internal string GetClassFullName() => 
            ((ClassScope) this.cons.enclosing_scope).GetFullName();

        internal ClassScope GetClassScope() => 
            ((ClassScope) this.cons.enclosing_scope);

        internal ConstructorInfo GetConstructorInfo(CompilerGlobals compilerGlobals) => 
            this.cons.GetConstructorInfo(compilerGlobals);

        public override object[] GetCustomAttributes(bool inherit)
        {
            if (this.cons != null)
            {
                CustomAttributeList customAttributes = this.cons.customAttributes;
                if (customAttributes != null)
                {
                    return (object[]) customAttributes.Evaluate(false);
                }
            }
            return new object[0];
        }

        public override object[] GetCustomAttributes(Type t, bool inherit) => 
            new object[0];

        public override MethodImplAttributes GetMethodImplementationFlags() => 
            MethodImplAttributes.IL;

        internal PackageScope GetPackage() => 
            ((ClassScope) this.cons.enclosing_scope).GetPackage();

        public override ParameterInfo[] GetParameters() => 
            this.cons.parameter_declarations;

        [DebuggerStepThrough, DebuggerHidden]
        public override object Invoke(BindingFlags options, Binder binder, object[] parameters, CultureInfo culture) => 
            LateBinding.CallValue(this.cons, parameters, true, false, this.cons.engine, null, binder, culture, null);

        [DebuggerStepThrough, DebuggerHidden]
        public override object Invoke(object obj, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture) => 
            this.cons.Call(parameters, obj, binder, culture);

        internal bool IsAccessibleFrom(ScriptObject scope)
        {
            while ((scope != null) && !(scope is ClassScope))
            {
                scope = scope.GetParent();
            }
            ClassScope other = (ClassScope) this.cons.enclosing_scope;
            if (base.IsPrivate)
            {
                if (scope == null)
                {
                    return false;
                }
                if (scope != other)
                {
                    return ((ClassScope) scope).IsNestedIn(other, false);
                }
                return true;
            }
            if (base.IsFamily)
            {
                if (scope == null)
                {
                    return false;
                }
                if (!((ClassScope) scope).IsSameOrDerivedFrom(other))
                {
                    return ((ClassScope) scope).IsNestedIn(other, false);
                }
                return true;
            }
            if ((base.IsFamilyOrAssembly && (scope != null)) && (((ClassScope) scope).IsSameOrDerivedFrom(other) || ((ClassScope) scope).IsNestedIn(other, false)))
            {
                return true;
            }
            if (scope == null)
            {
                return (other.GetPackage() == null);
            }
            return (other.GetPackage() == ((ClassScope) scope).GetPackage());
        }

        public override bool IsDefined(Type type, bool inherit) => 
            false;

        internal Type OuterClassType()
        {
            FieldInfo outerClassField = ((ClassScope) this.cons.enclosing_scope).outerClassField;
            if (outerClassField != null)
            {
                return outerClassField.FieldType;
            }
            return null;
        }

        public override MethodAttributes Attributes =>
            this.cons.attributes;

        public override Type DeclaringType =>
            Microsoft.JScript.Convert.ToType(this.cons.enclosing_scope);

        public override MemberTypes MemberType =>
            MemberTypes.Constructor;

        public override RuntimeMethodHandle MethodHandle =>
            this.GetConstructorInfo(null).MethodHandle;

        public override string Name =>
            this.cons.name;

        public override Type ReflectedType =>
            this.DeclaringType;
    }
}

