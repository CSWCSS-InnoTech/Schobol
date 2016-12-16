namespace Microsoft.JScript
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [Guid("6A02951C-B129-4d26-AB92-B9CA19BDCA26"), ComVisible(true)]
    public sealed class COMPropertyInfo : PropertyInfo, MemberInfoInitializer
    {
        private COMMemberInfo _comObject = null;
        private string _name = null;

        public override MethodInfo[] GetAccessors(bool nonPublic) => 
            new MethodInfo[] { this.GetGetMethod(nonPublic), this.GetSetMethod(nonPublic) };

        public COMMemberInfo GetCOMMemberInfo() => 
            this._comObject;

        public override object[] GetCustomAttributes(bool inherit) => 
            new FieldInfo[0];

        public override object[] GetCustomAttributes(Type t, bool inherit) => 
            new FieldInfo[0];

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            COMGetterMethod method = new COMGetterMethod();
            method.Initialize(this._name, this._comObject);
            return method;
        }

        public override ParameterInfo[] GetIndexParameters() => 
            new ParameterInfo[0];

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            COMSetterMethod method = new COMSetterMethod();
            method.Initialize(this._name, this._comObject);
            return method;
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) => 
            this._comObject.GetValue(invokeAttr, binder, (index != null) ? index : new object[0], culture);

        public void Initialize(string name, COMMemberInfo dispatch)
        {
            this._name = name;
            this._comObject = dispatch;
        }

        public override bool IsDefined(Type t, bool inherit) => 
            false;

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            this._comObject.SetValue(value, invokeAttr, binder, (index != null) ? index : new object[0], culture);
        }

        public override PropertyAttributes Attributes =>
            PropertyAttributes.None;

        public override bool CanRead =>
            true;

        public override bool CanWrite =>
            true;

        public override Type DeclaringType =>
            null;

        public override MemberTypes MemberType =>
            MemberTypes.Property;

        public override string Name =>
            this._name;

        public override Type PropertyType =>
            typeof(object);

        public override Type ReflectedType =>
            null;
    }
}

