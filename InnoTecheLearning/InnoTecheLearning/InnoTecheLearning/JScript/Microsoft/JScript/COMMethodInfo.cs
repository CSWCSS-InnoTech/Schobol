namespace Microsoft.JScript
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("C7B9C313-2FD4-4384-8571-7ABC08BD17E5")]
    public class COMMethodInfo : JSMethod, MemberInfoInitializer
    {
        protected COMMemberInfo _comObject;
        protected string _name;
        protected static readonly ParameterInfo[] EmptyParams = new ParameterInfo[0];

        public COMMethodInfo() : base(null)
        {
            this._comObject = null;
            this._name = null;
        }

        internal override object Construct(object[] args) => 
            this._comObject.Call(BindingFlags.CreateInstance, null, (args != null) ? args : new object[0], null);

        public override MethodInfo GetBaseDefinition() => 
            this;

        public COMMemberInfo GetCOMMemberInfo() => 
            this._comObject;

        public override MethodImplAttributes GetMethodImplementationFlags() => 
            MethodImplAttributes.IL;

        internal override MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals) => 
            null;

        public override ParameterInfo[] GetParameters() => 
            EmptyParams;

        public virtual void Initialize(string name, COMMemberInfo dispatch)
        {
            this._name = name;
            this._comObject = dispatch;
        }

        public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => 
            this._comObject.Call(invokeAttr, binder, (parameters != null) ? parameters : new object[0], culture);

        internal override object Invoke(object obj, object thisob, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture) => 
            this.Invoke(thisob, options, binder, parameters, culture);

        public override string ToString() => 
            "";

        public override MethodAttributes Attributes =>
            MethodAttributes.Public;

        public override Type DeclaringType =>
            null;

        public override MemberTypes MemberType =>
            MemberTypes.Method;

        public override RuntimeMethodHandle MethodHandle
        {
            get
            {
                throw new JScriptException(JSError.InternalError);
            }
        }

        public override string Name =>
            this._name;

        public override Type ReflectedType =>
            null;

        public override Type ReturnType =>
            null;

        public override ICustomAttributeProvider ReturnTypeCustomAttributes =>
            null;
    }
}

