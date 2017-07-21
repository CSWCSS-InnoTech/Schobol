namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;
    using System.Runtime.CompilerServices;

    [RequireQualifiedAccess, CompilationMapping(SourceConstructFlags.Module)]
    public static class Numbers
    {
        public static FSharpOption<double> |RealConstant|_|(Expression _arg1)
        {
            switch (_arg1.Tag)
            {
                case 1:
                {
                    Expression.Approximation approximation = (Expression.Approximation) _arg1;
                    if (approximation.item is MathNet.Symbolics.Approximation.Real)
                    {
                        return FSharpOption<double>.Some(((MathNet.Symbolics.Approximation.Real) approximation.item).item);
                    }
                    break;
                }
                case 3:
                {
                    Expression.Constant constant = (Expression.Constant) _arg1;
                    switch (constant.item.Tag)
                    {
                        case 0:
                            return FSharpOption<double>.Some(2.7182818284590451);

                        case 1:
                            return FSharpOption<double>.Some(3.1415926535897931);
                    }
                    break;
                }
                case 9:
                    return FSharpOption<double>.Some(double.PositiveInfinity);

                case 10:
                    return FSharpOption<double>.Some(double.PositiveInfinity);

                case 11:
                    return FSharpOption<double>.Some(double.NegativeInfinity);
            }
            return null;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("compare")]
        public static int Compare(Expression x, Expression y)
        {
            if (x.Equals(y, LanguagePrimitives.GenericEqualityComparer))
            {
                return 0;
            }
            return compare$cont@21(x, y, null);
        }

        [CompilerGenerated]
        internal static int compare$cont@21(Expression x, Expression y, Unit unitVar)
        {
            FSharpOption<double> option;
            Expression.Number number;
            BigRational item;
            double num;
            double num2;
            switch (x.Tag)
            {
                case 0:
                    number = (Expression.Number) x;
                    switch (y.Tag)
                    {
                        case 0:
                        {
                            Expression.Number number2 = (Expression.Number) y;
                            item = number2.item;
                            return LanguagePrimitives.HashCompare.GenericComparisonIntrinsic<BigRational>(number.item, item);
                        }
                        case 9:
                            return -1;

                        case 10:
                            return -1;

                        case 11:
                            return 1;
                    }
                    option = |RealConstant|_|(y);
                    if (option == null)
                    {
                        break;
                    }
                    num = option.Value;
                    num2 = BigRational.ToDouble(number.item);
                    if (num2 < num)
                    {
                        return -1;
                    }
                    return (int) (num2 > num);

                case 9:
                    if (y.Tag != 0)
                    {
                        break;
                    }
                    return 1;

                case 10:
                    if (y.Tag != 0)
                    {
                        break;
                    }
                    return 1;

                case 11:
                    if (y.Tag != 0)
                    {
                        break;
                    }
                    return -1;
            }
            option = |RealConstant|_|(x);
            if ((option != null) && (y.Tag == 0))
            {
                number = (Expression.Number) y;
                item = number.item;
                num = option.Value;
                num2 = BigRational.ToDouble(item);
                if (num < num2)
                {
                    return -1;
                }
                return (int) (num > num2);
            }
            FSharpOption<double> option2 = |RealConstant|_|(x);
            if (option2 != null)
            {
                FSharpOption<double> option3 = |RealConstant|_|(y);
                if (option3 != null)
                {
                    num = option3.Value;
                    num2 = option2.Value;
                    if (num2 < num)
                    {
                        return -1;
                    }
                    return (int) (num2 > num);
                }
            }
            throw new Exception("only numbers and +/-infinity are supported");
        }

        [CompilationSourceName("gcd")]
        public static Expression GreatestCommonDivisor(FSharpList<Expression> ax) => 
            ListModule.Reduce<Expression>(new GreatestCommonDivisor@62(), ax);

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("gcd2")]
        public static Expression GreatestCommonDivisor2(Expression u, Expression v)
        {
            if (u.Tag == 0)
            {
                Expression.Number number = (Expression.Number) u;
                if (v.Tag == 0)
                {
                    Expression.Number number2 = (Expression.Number) v;
                    BigRational item = number2.item;
                    if (number.item.IsInteger && item.IsInteger)
                    {
                        item = number2.item;
                        BigRational rational2 = number.item;
                        return Expression.NewNumber(BigRational.FromBigInt(Euclid.GreatestCommonDivisor(rational2.Numerator, item.Numerator)));
                    }
                }
            }
            return Expression.Undefined;
        }

        [CompilationSourceName("lcm")]
        public static Expression LeastCommonMultiple(FSharpList<Expression> ax) => 
            ListModule.Reduce<Expression>(new LeastCommonMultiple@65(), ax);

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("lcm2")]
        public static Expression LeastCommonMultiple2(Expression u, Expression v)
        {
            if (u.Tag == 0)
            {
                Expression.Number number = (Expression.Number) u;
                if (v.Tag == 0)
                {
                    Expression.Number number2 = (Expression.Number) v;
                    BigRational item = number2.item;
                    if (number.item.IsInteger && item.IsInteger)
                    {
                        item = number2.item;
                        BigRational rational2 = number.item;
                        return Expression.NewNumber(BigRational.FromBigInt(Euclid.LeastCommonMultiple(rational2.Numerator, item.Numerator)));
                    }
                }
            }
            return Expression.Undefined;
        }

        [CompilationSourceName("max")]
        public static Expression Max(FSharpList<Expression> ax) => 
            ListModule.Reduce<Expression>(new Max@42(), ax);

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("max2")]
        public static Expression Max2(Expression u, Expression v)
        {
            if (Compare(u, v) >= 0)
            {
                return u;
            }
            return v;
        }

        [CompilationSourceName("min")]
        public static Expression Min(FSharpList<Expression> ax) => 
            ListModule.Reduce<Expression>(new Min@45(), ax);

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("min2")]
        public static Expression Min2(Expression u, Expression v)
        {
            if (Compare(u, v) <= 0)
            {
                return u;
            }
            return v;
        }

        [Serializable]
        internal class GreatestCommonDivisor@62 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal GreatestCommonDivisor@62()
            {
            }

            public override Expression Invoke(Expression u, Expression v) => 
                Numbers.GreatestCommonDivisor2(u, v);
        }

        [Serializable]
        internal class LeastCommonMultiple@65 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal LeastCommonMultiple@65()
            {
            }

            public override Expression Invoke(Expression u, Expression v) => 
                Numbers.LeastCommonMultiple2(u, v);
        }

        [Serializable]
        internal class Max@42 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal Max@42()
            {
            }

            public override Expression Invoke(Expression u, Expression v) => 
                Numbers.Max2(u, v);
        }

        [Serializable]
        internal class Min@45 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal Min@45()
            {
            }

            public override Expression Invoke(Expression u, Expression v) => 
                Numbers.Min2(u, v);
        }
    }
}

