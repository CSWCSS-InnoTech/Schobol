namespace Microsoft.JScript
{
    using System;
    using System.Reflection;

    internal sealed class ParameterDeclaration : ParameterInfo
    {
        internal Context context;
        internal CustomAttributeList customAttributes;
        internal string identifier;
        internal TypeExpression type;

        internal ParameterDeclaration(Type type, string identifier)
        {
            this.identifier = identifier;
            this.type = new TypeExpression(new ConstantWrapper(type, null));
            this.customAttributes = null;
        }

        internal ParameterDeclaration(Context context, string identifier, TypeExpression type, CustomAttributeList customAttributes)
        {
            this.identifier = identifier;
            this.type = (type == null) ? new TypeExpression(new ConstantWrapper(Typeob.Object, context)) : type;
            this.context = context;
            ActivationObject obj2 = (ActivationObject) context.document.engine.Globals.ScopeStack.Peek();
            if (obj2.name_table[this.identifier] != null)
            {
                context.HandleError(JSError.DuplicateName, this.identifier, ((obj2 is ClassScope) || obj2.fast) || (type != null));
            }
            else
            {
                obj2.AddNewField(this.identifier, null, FieldAttributes.PrivateScope).originalContext = context;
            }
            this.customAttributes = customAttributes;
        }

        public override object[] GetCustomAttributes(bool inherit) => 
            new FieldInfo[0];

        public override object[] GetCustomAttributes(Type attributeType, bool inherit) => 
            new FieldInfo[0];

        public override bool IsDefined(Type attributeType, bool inherit) => 
            ((this.customAttributes != null) && (this.customAttributes.GetAttribute(attributeType) != null));

        internal void PartiallyEvaluate()
        {
            if (this.type != null)
            {
                this.type.PartiallyEvaluate();
            }
            if (this.customAttributes != null)
            {
                this.customAttributes.PartiallyEvaluate();
                if (Microsoft.JScript.CustomAttribute.IsDefined(this, typeof(ParamArrayAttribute), false))
                {
                    if (this.type != null)
                    {
                        IReflect reflect = this.type.ToIReflect();
                        if (((reflect is Type) && ((Type) reflect).IsArray) || (reflect is TypedArray))
                        {
                            return;
                        }
                    }
                    this.customAttributes.context.HandleError(JSError.IllegalParamArrayAttribute);
                }
            }
        }

        public override object DefaultValue =>
            System.Convert.DBNull;

        public override string Name =>
            this.identifier;

        internal IReflect ParameterIReflect =>
            this.type.ToIReflect();

        public override Type ParameterType
        {
            get
            {
                Type type = this.type.ToType();
                if (type == Typeob.Void)
                {
                    type = Typeob.Object;
                }
                return type;
            }
        }
    }
}

