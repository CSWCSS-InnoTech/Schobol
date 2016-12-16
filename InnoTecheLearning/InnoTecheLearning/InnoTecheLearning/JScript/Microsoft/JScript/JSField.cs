namespace Microsoft.JScript
{
    using System;
    using System.Reflection;

    public abstract class JSField : FieldInfo
    {
        protected JSField()
        {
        }

        internal virtual string GetClassFullName()
        {
            throw new JScriptException(JSError.InternalError);
        }

        public override object[] GetCustomAttributes(bool inherit) => 
            new FieldInfo[0];

        public override object[] GetCustomAttributes(Type t, bool inherit) => 
            new FieldInfo[0];

        internal virtual object GetMetaData()
        {
            throw new JScriptException(JSError.InternalError);
        }

        internal virtual PackageScope GetPackage()
        {
            throw new JScriptException(JSError.InternalError);
        }

        public override bool IsDefined(Type type, bool inherit) => 
            false;

        public override FieldAttributes Attributes =>
            FieldAttributes.PrivateScope;

        public override Type DeclaringType =>
            null;

        public override RuntimeFieldHandle FieldHandle =>
            ((FieldInfo) this.GetMetaData()).FieldHandle;

        public override Type FieldType =>
            Typeob.Object;

        public override MemberTypes MemberType =>
            MemberTypes.Field;

        public override string Name =>
            "";

        public override Type ReflectedType =>
            this.DeclaringType;
    }
}

