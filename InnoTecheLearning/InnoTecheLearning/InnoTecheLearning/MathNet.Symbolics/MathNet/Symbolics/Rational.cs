namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections.Generic;

    [CompilationMapping(SourceConstructFlags.Module)]
    public static class Rational
    {
        [CompilationSourceName("denominator")]
        public static Expression Denominator(Expression _arg1)
        {
            if (_arg1.Tag == 0)
            {
                Expression.Number number = (Expression.Number) _arg1;
                if (!number.item.IsInteger)
                {
                    BigRational item = number.item;
                    return Expression.NewNumber(BigRational.FromBigInt(item.Denominator));
                }
            }
            FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|NegRationalPower|_|(_arg1);
            if (option != null)
            {
                Expression x = option.Value.Item1;
                Expression y = option.Value.Item2;
                return MathNet.Symbolics.Operators.pow(x, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, y));
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                FSharpList<Expression> list = product.item;
                return MathNet.Symbolics.Operators.product(ListModule.Map<Expression, Expression>(new Denominator@22(), list));
            }
            return MathNet.Symbolics.Operators.one;
        }

        [CompilationSourceName("expand")]
        public static Expression Expand(Expression x)
        {
            Expression expression = Reduce(expandRationalize@63(x));
            if (Denominator(expression).Tag == 0)
            {
                return Algebraic.Expand(expression);
            }
            return expression;
        }

        internal static Expression expandRationalize@63(Expression x)
        {
            while (true)
            {
                Expression expression = Algebraic.Expand(Numerator(x));
                Expression expression2 = Algebraic.Expand(Denominator(x));
                Expression expression3 = Rationalize(MathNet.Symbolics.Operators.multiply(expression, MathNet.Symbolics.Operators.invert(expression2)));
                if (x.Equals(expression3, LanguagePrimitives.GenericEqualityComparer))
                {
                    return expression3;
                }
                x = expression3;
            }
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("isRationalMV")]
        public static bool IsMultivariateRational(HashSet<Expression> symbols, Expression x) => 
            (Polynomial.IsMultivariatePolynomial(symbols, Numerator(x)) && Polynomial.IsMultivariatePolynomial(symbols, Denominator(x)));

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("isRational")]
        public static bool IsRational(Expression symbol, Expression x) => 
            (Polynomial.IsPolynomial(symbol, Numerator(x)) && Polynomial.IsPolynomial(symbol, Denominator(x)));

        [CompilationSourceName("numerator")]
        public static Expression Numerator(Expression _arg1)
        {
            if (_arg1.Tag == 0)
            {
                Expression.Number number = (Expression.Number) _arg1;
                if (!number.item.IsInteger)
                {
                    BigRational item = number.item;
                    return Expression.NewNumber(BigRational.FromBigInt(item.Numerator));
                }
            }
            if (ExpressionPatterns.|NegRationalPower|_|(_arg1) != null)
            {
                return MathNet.Symbolics.Operators.one;
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                FSharpList<Expression> list = product.item;
                return MathNet.Symbolics.Operators.product(ListModule.Map<Expression, Expression>(new Numerator@15(), list));
            }
            return _arg1;
        }

        [CompilationSourceName("rationalize")]
        public static Expression Rationalize(Expression _arg1)
        {
            FSharpList<Expression> item;
            switch (_arg1.Tag)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    return _arg1;

                case 4:
                {
                    Expression.Sum sum = (Expression.Sum) _arg1;
                    item = sum.item;
                    return ListModule.Reduce<Expression>(new Rationalize@58-1(), ListModule.Map<Expression, Expression>(new Rationalize@58-2(), item));
                }
                case 5:
                {
                    Expression.Product product = (Expression.Product) _arg1;
                    item = product.item;
                    return MathNet.Symbolics.Operators.product(ListModule.Map<Expression, Expression>(new Rationalize@57(), item));
                }
                case 6:
                {
                    Expression.Power power = (Expression.Power) _arg1;
                    Expression expression = power.item1;
                    Expression y = power.item2;
                    return MathNet.Symbolics.Operators.pow(Rationalize(expression), y);
                }
            }
            return _arg1;
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 })]
        internal static Expression rationalizeSum(Expression d, Expression x, Expression y)
        {
            while (true)
            {
                Expression expression = Denominator(x);
                Expression expression2 = Denominator(y);
                if (MathNet.Symbolics.Operators.isOne(expression) && MathNet.Symbolics.Operators.isOne(expression2))
                {
                    return MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.add(x, y), MathNet.Symbolics.Operators.invert(d));
                }
                y = MathNet.Symbolics.Operators.multiply(Numerator(y), expression);
                x = MathNet.Symbolics.Operators.multiply(Numerator(x), expression2);
                d = MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(expression, expression2), d);
            }
        }

        [CompilationSourceName("reduce")]
        public static Expression Reduce(Expression x)
        {
            Expression expression = Numerator(x);
            Expression expression2 = Denominator(x);
            Expression head = Polynomial.CommonFactors(expression);
            Expression expression4 = Polynomial.CommonFactors(expression2);
            Expression expression5 = Polynomial.CommonMonomialFactors(FSharpList<Expression>.Cons(head, FSharpList<Expression>.Cons(expression4, FSharpList<Expression>.Empty)));
            if (MathNet.Symbolics.Operators.isOne(expression5))
            {
                return x;
            }
            Expression expression6 = Algebraic.Expand(MathNet.Symbolics.Operators.multiply(expression, MathNet.Symbolics.Operators.invert(expression5)));
            Expression expression7 = Algebraic.Expand(MathNet.Symbolics.Operators.multiply(expression2, MathNet.Symbolics.Operators.invert(expression5)));
            return MathNet.Symbolics.Operators.multiply(expression6, MathNet.Symbolics.Operators.invert(expression7));
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("simplify")]
        public static Expression Simplify(Expression symbol, Expression x)
        {
            Expression expression = Expand(x);
            Expression u = Numerator(expression);
            Expression v = Denominator(expression);
            Expression expression4 = Polynomial.Gcd(symbol, u, v);
            Expression expression5 = Polynomial.Divide(symbol, u, expression4).Item1;
            Expression expression6 = Polynomial.Divide(symbol, v, expression4).Item1;
            return MathNet.Symbolics.Operators.multiply(expression5, MathNet.Symbolics.Operators.invert(expression6));
        }

        [CompilationSourceName("variables")]
        public static HashSet<Expression> Variables(Expression x)
        {
            HashSet<Expression> set = Polynomial.Variables(Numerator(x));
            set.UnionWith((IEnumerable<Expression>) Polynomial.Variables(Denominator(x)));
            return set;
        }

        [Serializable]
        internal class Denominator@22 : FSharpFunc<Expression, Expression>
        {
            internal Denominator@22()
            {
            }

            public override Expression Invoke(Expression _arg1) => 
                Rational.Denominator(_arg1);
        }

        [Serializable]
        internal class Numerator@15 : FSharpFunc<Expression, Expression>
        {
            internal Numerator@15()
            {
            }

            public override Expression Invoke(Expression _arg1) => 
                Rational.Numerator(_arg1);
        }

        [Serializable]
        internal class Rationalize@57 : FSharpFunc<Expression, Expression>
        {
            internal Rationalize@57()
            {
            }

            public override Expression Invoke(Expression _arg1) => 
                Rational.Rationalize(_arg1);
        }

        [Serializable]
        internal class Rationalize@58-1 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal Rationalize@58-1()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                Rational.rationalizeSum(MathNet.Symbolics.Operators.one, x, y);
        }

        [Serializable]
        internal class Rationalize@58-2 : FSharpFunc<Expression, Expression>
        {
            internal Rationalize@58-2()
            {
            }

            public override Expression Invoke(Expression _arg1) => 
                Rational.Rationalize(_arg1);
        }
    }
}

