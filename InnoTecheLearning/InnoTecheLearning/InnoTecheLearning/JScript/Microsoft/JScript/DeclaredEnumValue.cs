namespace Microsoft.JScript
{
    using System;
    using System.Reflection;

    internal sealed class DeclaredEnumValue : EnumWrapper
    {
        internal ClassScope _classScope;
        private string _name;
        internal object _value;

        internal DeclaredEnumValue(object value, string name, ClassScope classScope)
        {
            this._name = name;
            this._classScope = classScope;
            this._value = value;
        }

        internal void CoerceToBaseType(Type bt, Context errCtx)
        {
            object obj2 = 0;
            AST ast = ((AST) this.value).PartiallyEvaluate();
            if (ast is ConstantWrapper)
            {
                obj2 = ((ConstantWrapper) ast).Evaluate();
            }
            else
            {
                ast.context.HandleError(JSError.NotConst);
            }
            try
            {
                this._value = Microsoft.JScript.Convert.CoerceT(obj2, bt);
            }
            catch
            {
                errCtx.HandleError(JSError.TypeMismatch);
                this._value = Microsoft.JScript.Convert.CoerceT(0, bt);
            }
        }

        internal override IReflect classScopeOrType =>
            this._classScope;

        internal override string name =>
            this._name;

        internal override Type type =>
            this._classScope.GetTypeBuilderOrEnumBuilder();

        internal override object value =>
            this._value;
    }
}

