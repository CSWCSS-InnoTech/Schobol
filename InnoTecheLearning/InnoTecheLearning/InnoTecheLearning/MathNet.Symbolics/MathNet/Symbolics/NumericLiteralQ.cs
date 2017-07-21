namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Core;
    using System;
    using System.Numerics;

    [RequireQualifiedAccess, CompilationMapping(SourceConstructFlags.Module)]
    public static class NumericLiteralQ
    {
        public static Expression FromInt32(int x) => 
            Expression.NewNumber(BigRational.FromInt(x));

        public static Expression FromInt64(long x) => 
            Expression.NewNumber(BigRational.FromBigInt(new BigInteger(x)));

        public static Expression FromOne() => 
            MathNet.Symbolics.Operators.one;

        public static Expression FromString(string str) => 
            Expression.NewNumber(BigRational.Parse(str));

        public static Expression FromZero() => 
            MathNet.Symbolics.Operators.zero;
    }
}

