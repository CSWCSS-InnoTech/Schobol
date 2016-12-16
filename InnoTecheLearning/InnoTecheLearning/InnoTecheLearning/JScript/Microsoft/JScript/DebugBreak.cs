namespace Microsoft.JScript
{
    using System;
    using System.Diagnostics;
    using System.Reflection.Emit;

    public class DebugBreak : AST
    {
        internal DebugBreak(Context context) : base(context)
        {
        }

        internal override object Evaluate()
        {
            Debugger.Break();
            return new Completion();
        }

        internal override AST PartiallyEvaluate() => 
            this;

        internal override void TranslateToIL(ILGenerator il, Type rtype)
        {
            base.context.EmitLineInfo(il);
            il.Emit(OpCodes.Call, CompilerGlobals.debugBreak);
        }

        internal override void TranslateToILInitializer(ILGenerator il)
        {
        }
    }
}

