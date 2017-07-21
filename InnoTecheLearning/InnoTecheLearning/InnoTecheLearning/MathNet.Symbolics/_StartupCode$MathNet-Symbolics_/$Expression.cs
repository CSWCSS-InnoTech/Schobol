namespace <StartupCode$MathNet-Symbolics>
{
    using MathNet.Symbolics;
    using Microsoft.FSharp.Core;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal static class $Expression
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated, DebuggerNonUserCode]
        internal static int init@;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly Expression minusOne@133 = Expression.NewNumber(BigRational.FromInt(-1));
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<int, Expression> number@151 = new MathNet.Symbolics.Operators.number@151();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly Expression one@131-2 = Expression.NewNumber(BigRational.One);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly Expression pi@134 = Expression.NewConstant(MathNet.Symbolics.Constant.get_Pi());
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly Expression two@132 = Expression.NewNumber(BigRational.FromInt(2));
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly Expression zero@130-2 = Expression.NewNumber(BigRational.Zero);
    }
}

