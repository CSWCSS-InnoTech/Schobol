namespace Microsoft.JScript
{
    using System;
    using System.Globalization;
    using System.Reflection;

    internal sealed class JSClosureMethod : JSMethod
    {
        internal MethodInfo method;

        internal JSClosureMethod(MethodInfo method) : base(null)
        {
            this.method = method;
        }

        internal override object Construct(object[] args)
        {
            throw new JScriptException(JSError.InternalError);
        }

        internal override MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals)
        {
            if (this.method is JSMethod)
            {
                return ((JSMethod) this.method).GetMethodInfo(compilerGlobals);
            }
            return this.method;
        }

        public override ParameterInfo[] GetParameters() => 
            this.method.GetParameters();

        internal override object Invoke(object obj, object thisob, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture)
        {
            if (!(obj is StackFrame))
            {
                throw new JScriptException(JSError.InternalError);
            }
            return this.method.Invoke(((StackFrame) ((StackFrame) obj).engine.ScriptObjectStackTop()).closureInstance, options, binder, parameters, culture);
        }

        public override MethodAttributes Attributes =>
            ((this.method.Attributes & ~MethodAttributes.Virtual) | MethodAttributes.Static);

        public override Type DeclaringType =>
            this.method.DeclaringType;

        public override string Name =>
            this.method.Name;

        public override Type ReturnType =>
            this.method.ReturnType;
    }
}

