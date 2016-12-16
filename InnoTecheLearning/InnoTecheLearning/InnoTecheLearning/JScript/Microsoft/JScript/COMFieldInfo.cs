namespace Microsoft.JScript
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [ComVisible(true), Guid("CA0F511A-FAF2-4942-B9A8-17D5E46514E8")]
    public class COMFieldInfo : FieldInfo, MemberInfoInitializer
    {
        private COMMemberInfo _comObject = null;
        private string _name = null;

        public COMMemberInfo GetCOMMemberInfo() => 
            this._comObject;

        public override object[] GetCustomAttributes(bool inherit) => 
            new FieldInfo[0];

        public override object[] GetCustomAttributes(Type t, bool inherit) => 
            new FieldInfo[0];

        public override object GetValue(object obj) => 
            this._comObject.GetValue(BindingFlags.Default, null, new object[0], null);

        public virtual void Initialize(string name, COMMemberInfo dispatch)
        {
            this._name = name;
            this._comObject = dispatch;
        }

        public override bool IsDefined(Type t, bool inherit) => 
            false;

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
        {
            this._comObject.SetValue(value, invokeAttr, binder, new object[0], culture);
        }

        public override FieldAttributes Attributes =>
            FieldAttributes.Public;

        public override Type DeclaringType =>
            null;

        public override RuntimeFieldHandle FieldHandle
        {
            get
            {
                throw new JScriptException(JSError.InternalError);
            }
        }

        public override Type FieldType =>
            typeof(object);

        public override MemberTypes MemberType =>
            MemberTypes.Field;

        public override string Name =>
            this._name;

        public override Type ReflectedType =>
            null;
    }
}

