namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Core;
    using System;
    using System.Numerics;

    [RequireQualifiedAccess, CompilationMapping(SourceConstructFlags.Module)]
    public static class Values
    {
        public static FSharpOption<Value> |Value|_|(Expression _arg1)
        {
            switch (_arg1.Tag)
            {
                case 0:
                {
                    Expression.Number number = (Expression.Number) _arg1;
                    return FSharpOption<Value>.Some(Value.NewNumber(number.item));
                }
                case 1:
                {
                    Expression.Approximation approximation = (Expression.Approximation) _arg1;
                    return FSharpOption<Value>.Some(Value.NewApproximation(approximation.item));
                }
                case 9:
                    return FSharpOption<Value>.Some(Value.ComplexInfinity);

                case 10:
                    return FSharpOption<Value>.Some(Value.PositiveInfinity);

                case 11:
                    return FSharpOption<Value>.Some(Value.NegativeInfinity);
            }
            return null;
        }

        public static Expression abs(Value a) => 
            unpack(ValueModule.abs(a));

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static Expression apply(MathNet.Symbolics.Function f, Value x) => 
            unpack(ValueModule.apply(f, x));

        public static Expression complex(System.Numerics.Complex x) => 
            unpack(ValueModule.complex(x));

        public static Expression invert(Value a) => 
            unpack(ValueModule.invert(a));

        public static Expression negate(Value a) => 
            unpack(ValueModule.negate(a));

        public static Expression power(Value a, Value b) => 
            unpack(ValueModule.power(a, b));

        public static Expression product(Value a, Value b) => 
            unpack(ValueModule.product(a, b));

        public static Expression rational(BigRational x) => 
            Expression.NewNumber(x);

        public static Expression real(double x) => 
            unpack(ValueModule.real(x));

        public static Expression sum(Value a, Value b) => 
            unpack(ValueModule.sum(a, b));

        public static Expression unpack(Value _arg1)
        {
            switch (_arg1.Tag)
            {
                case 1:
                {
                    Value.Approximation approximation = (Value.Approximation) _arg1;
                    return Expression.NewApproximation(approximation.item);
                }
                case 2:
                    return Expression.ComplexInfinity;

                case 3:
                    return Expression.PositiveInfinity;

                case 4:
                    return Expression.NegativeInfinity;

                case 5:
                    return Expression.Undefined;
            }
            Value.Number number = (Value.Number) _arg1;
            return Expression.NewNumber(number.item);
        }
    }
}

