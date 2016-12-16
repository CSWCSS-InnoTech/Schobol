namespace Microsoft.JScript
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;

    internal class JSWrappedProperty : PropertyInfo, IWrappedMember
    {
        internal object obj;
        internal PropertyInfo property;

        internal JSWrappedProperty(PropertyInfo property, object obj)
        {
            this.obj = obj;
            this.property = property;
            if (obj is JSObject)
            {
                Type declaringType = property.DeclaringType;
                if (((declaringType == Typeob.Object) || (declaringType == Typeob.String)) || (declaringType.IsPrimitive || (declaringType == Typeob.Array)))
                {
                    if (obj is BooleanObject)
                    {
                        this.obj = ((BooleanObject) obj).value;
                    }
                    else if (obj is NumberObject)
                    {
                        this.obj = ((NumberObject) obj).value;
                    }
                    else if (obj is StringObject)
                    {
                        this.obj = ((StringObject) obj).value;
                    }
                    else if (obj is ArrayWrapper)
                    {
                        this.obj = ((ArrayWrapper) obj).value;
                    }
                }
            }
        }

        public override MethodInfo[] GetAccessors(bool nonPublic) => 
            this.property.GetAccessors(nonPublic);

        internal virtual string GetClassFullName()
        {
            if (this.property is JSProperty)
            {
                return ((JSProperty) this.property).GetClassFullName();
            }
            return this.property.DeclaringType.FullName;
        }

        public override object[] GetCustomAttributes(bool inherit) => 
            this.property.GetCustomAttributes(inherit);

        public override object[] GetCustomAttributes(Type t, bool inherit) => 
            Microsoft.JScript.CustomAttribute.GetCustomAttributes(this.property, t, inherit);

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            MethodInfo getMethod = JSProperty.GetGetMethod(this.property, nonPublic);
            if (getMethod == null)
            {
                return null;
            }
            return new JSWrappedMethod(getMethod, this.obj);
        }

        public override ParameterInfo[] GetIndexParameters() => 
            this.property.GetIndexParameters();

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            MethodInfo setMethod = JSProperty.GetSetMethod(this.property, nonPublic);
            if (setMethod == null)
            {
                return null;
            }
            return new JSWrappedMethod(setMethod, this.obj);
        }

        [DebuggerStepThrough, DebuggerHidden]
        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) => 
            this.property.GetValue(this.obj, invokeAttr, binder, index, culture);

        public object GetWrappedObject() => 
            this.obj;

        public override bool IsDefined(Type type, bool inherit) => 
            Microsoft.JScript.CustomAttribute.IsDefined(this.property, type, inherit);

        [DebuggerStepThrough, DebuggerHidden]
        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            this.property.SetValue(this.obj, value, invokeAttr, binder, index, culture);
        }

        public override PropertyAttributes Attributes =>
            this.property.Attributes;

        public override bool CanRead =>
            this.property.CanRead;

        public override bool CanWrite =>
            this.property.CanWrite;

        public override Type DeclaringType =>
            this.property.DeclaringType;

        public override MemberTypes MemberType =>
            MemberTypes.Property;

        public override string Name
        {
            get
            {
                if ((this.obj is LenientGlobalObject) && this.property.Name.StartsWith("Slow", StringComparison.Ordinal))
                {
                    return this.property.Name.Substring(4);
                }
                return this.property.Name;
            }
        }

        public override Type PropertyType =>
            this.property.PropertyType;

        public override Type ReflectedType =>
            this.property.ReflectedType;
    }
}

