namespace Microsoft.JScript
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [Guid("561AC104-8869-4368-902F-4E0D7DDEDDDD"), ComVisible(true)]
    public abstract class JSMethod : MethodInfo
    {
        internal object obj;

        internal JSMethod(object obj)
        {
            this.obj = obj;
        }

        internal abstract object Construct(object[] args);
        public override MethodInfo GetBaseDefinition() => 
            this;

        internal virtual string GetClassFullName()
        {
            if (!(this.obj is ClassScope))
            {
                throw new JScriptException(JSError.InternalError);
            }
            return ((ClassScope) this.obj).GetFullName();
        }

        public override object[] GetCustomAttributes(bool inherit) => 
            new object[0];

        public override object[] GetCustomAttributes(Type t, bool inherit) => 
            new object[0];

        public override MethodImplAttributes GetMethodImplementationFlags() => 
            MethodImplAttributes.IL;

        internal abstract MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals);
        internal virtual PackageScope GetPackage()
        {
            if (!(this.obj is ClassScope))
            {
                throw new JScriptException(JSError.InternalError);
            }
            return ((ClassScope) this.obj).GetPackage();
        }

        [DebuggerHidden, DebuggerStepThrough]
        public override object Invoke(object obj, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture) => 
            this.Invoke(obj, obj, options, binder, parameters, culture);

        internal abstract object Invoke(object obj, object thisob, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture);
        public sealed override bool IsDefined(Type type, bool inherit) => 
            false;

        public override MemberTypes MemberType =>
            MemberTypes.Method;

        public override RuntimeMethodHandle MethodHandle =>
            this.GetMethodInfo(null).MethodHandle;

        public override Type ReflectedType =>
            this.DeclaringType;

        public override ICustomAttributeProvider ReturnTypeCustomAttributes =>
            null;
    }
}

