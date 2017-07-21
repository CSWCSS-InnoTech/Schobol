namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections.Generic;

    [CompilationMapping(SourceConstructFlags.Module)]
    public static class SingleVariablePolynomial
    {
        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("coefficientSV")]
        public static Expression Coefficient(Expression symbol, int k, Expression x)
        {
            Expression expression = MathNet.Symbolics.Operators.number.Invoke(k);
            Tuple<Expression, Expression> tuple = MonomialCoefficientDegree(symbol, x);
            Expression expression2 = tuple.Item2;
            Expression expression3 = tuple.Item1;
            if (expression2.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
            {
                return expression3;
            }
            if (x.Tag == 4)
            {
                Expression.Sum sum = (Expression.Sum) x;
                FSharpList<Expression> item = sum.item;
                return MathNet.Symbolics.Operators.sum(ListModule.Map<Tuple<Expression, Expression>, Expression>(new Coefficient@410-3(), ListModule.Filter<Tuple<Expression, Expression>>(new Coefficient@410-4(expression), ListModule.Map<Expression, Tuple<Expression, Expression>>(new Coefficient@410-5(symbol), item))));
            }
            return Expression.Undefined;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("coefficientsSV")]
        public static Expression[] Coefficients(Expression symbol, Expression x)
        {
            int num2;
            FSharpList<Tuple<int, Expression>> list = collect@429-5(symbol, x);
            IEnumerable<int> enumerable = SeqModule.Map<Tuple<int, Expression>, int>(new degree@437-5(), (IEnumerable<Tuple<int, Expression>>) list);
            if (enumerable == null)
            {
                throw new ArgumentNullException("source");
            }
            using (IEnumerator<int> enumerator = enumerable.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new ArgumentException(LanguagePrimitives.ErrorStrings.InputSequenceEmptyString, "source");
                }
                int current = enumerator.Current;
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    int num4 = enumerator.Current;
                    if (num4 > current)
                    {
                        current = num4;
                    }
                }
                num2 = current;
            }
            int num = num2;
            return ListModule.Fold<Tuple<int, Expression>, Expression[]>(new Coefficients@438-1(), ArrayModule.Create<Expression>(num + 1, MathNet.Symbolics.Operators.zero), list);
        }

        internal static FSharpList<Tuple<int, Expression>> collect@429-5(Expression symbol, Expression _arg1)
        {
            Expression expression;
            FSharpList<Expression> item;
            FSharpFunc<Expression, FSharpFunc<Expression, FSharpList<Tuple<int, Expression>>>> func = new collect@429-6();
            if (_arg1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
            {
                return FSharpList<Tuple<int, Expression>>.Cons(new Tuple<int, Expression>(1, MathNet.Symbolics.Operators.one), FSharpList<Tuple<int, Expression>>.Empty);
            }
            if (_arg1.Tag == 0)
            {
                expression = _arg1;
                return FSharpList<Tuple<int, Expression>>.Cons(new Tuple<int, Expression>(0, expression), FSharpList<Tuple<int, Expression>>.Empty);
            }
            FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|PosIntPower|_|(_arg1);
            if ((option != null) && (option.Value.Item2.Tag == 0))
            {
                Expression.Number number = (Expression.Number) option.Value.Item2;
                if (option.Value.Item1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
                {
                    expression = option.Value.Item1;
                    return FSharpList<Tuple<int, Expression>>.Cons(new Tuple<int, Expression>(BigRational.ToInt32(number.item), MathNet.Symbolics.Operators.one), FSharpList<Tuple<int, Expression>>.Empty);
                }
            }
            switch (_arg1.Tag)
            {
                case 4:
                {
                    Expression.Sum sum = (Expression.Sum) _arg1;
                    item = sum.item;
                    return ListModule.Collect<Expression, Tuple<int, Expression>>(func.Invoke(symbol), item);
                }
                case 5:
                {
                    Expression.Product product = (Expression.Product) _arg1;
                    item = product.item;
                    return ListModule.Reduce<FSharpList<Tuple<int, Expression>>>(new collect@434-7(), ListModule.Map<Expression, FSharpList<Tuple<int, Expression>>>(func.Invoke(symbol), item));
                }
            }
            return FSharpList<Tuple<int, Expression>>.Empty;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("degreeSV")]
        public static Expression Degree(Expression symbol, Expression x)
        {
            Expression expression = MonomialDegree(symbol, x);
            Expression expression2 = Expression.Undefined;
            if (!expression.Equals(expression2, LanguagePrimitives.GenericEqualityComparer))
            {
                return expression;
            }
            if (x.Tag == 4)
            {
                Expression.Sum sum = (Expression.Sum) x;
                FSharpList<Expression> item = sum.item;
                return ListModule.Reduce<Expression>(new Degree@382-2(), ListModule.Map<Expression, Expression>(new Degree@382-3(symbol), item));
            }
            return Expression.Undefined;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("isMonomialSV")]
        public static bool IsMonomial(Expression symbol, Expression _arg1)
        {
            if (_arg1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
            {
                return true;
            }
            if (_arg1.Tag == 0)
            {
                return true;
            }
            FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|PosIntPower|_|(_arg1);
            if ((option != null) && option.Value.Item1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
            {
                Expression expression = option.Value.Item1;
                return true;
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                return ListModule.ForAll<Expression>(new IsMonomial@359-1(symbol), product.item);
            }
            return false;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("isPolynomialSV")]
        public static bool IsPolynomial(Expression symbol, Expression _arg1)
        {
            if (_arg1.Tag == 4)
            {
                Expression.Sum sum = (Expression.Sum) _arg1;
                return ListModule.ForAll<Expression>(new IsPolynomial@364-1(symbol), sum.item);
            }
            return IsMonomial(symbol, _arg1);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("leadingCoefficientSV")]
        public static Expression LeadingCoefficient(Expression symbol, Expression x) => 
            LeadingCoefficientDegree(symbol, x).Item1;

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("leadingCoefficientDegreeSV")]
        public static Tuple<Expression, Expression> LeadingCoefficientDegree(Expression symbol, Expression x)
        {
            Tuple<Expression, Expression> tuple = MonomialCoefficientDegree(symbol, x);
            Expression expression = tuple.Item2;
            Expression expression2 = tuple.Item1;
            Expression expression3 = Expression.Undefined;
            if (!expression.Equals(expression3, LanguagePrimitives.GenericEqualityComparer))
            {
                return new Tuple<Expression, Expression>(expression2, expression);
            }
            if (x.Tag == 4)
            {
                Expression.Sum sum = (Expression.Sum) x;
                FSharpList<Expression> item = sum.item;
                FSharpList<Tuple<Expression, Expression>> list = ListModule.Map<Expression, Tuple<Expression, Expression>>(new cds@419-3(symbol), item);
                expression3 = ListModule.Reduce<Expression>(new degree@420-3(), ListModule.Map<Tuple<Expression, Expression>, Expression>(new degree@420-4(), list));
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.sum(ListModule.Map<Tuple<Expression, Expression>, Expression>(new LeadingCoefficientDegree@421-2(), ListModule.Filter<Tuple<Expression, Expression>>(new LeadingCoefficientDegree@421-3(expression3), list))), expression3);
            }
            return new Tuple<Expression, Expression>(Expression.Undefined, Expression.Undefined);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("coefficientMonomialSV")]
        public static Expression MonomialCoefficient(Expression symbol, Expression _arg1)
        {
            if (_arg1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
            {
                return MathNet.Symbolics.Operators.one;
            }
            if (_arg1.Tag == 0)
            {
                return _arg1;
            }
            FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|PosIntPower|_|(_arg1);
            if ((option != null) && option.Value.Item1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
            {
                Expression expression = option.Value.Item1;
                return MathNet.Symbolics.Operators.one;
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                FSharpList<Expression> item = product.item;
                return MathNet.Symbolics.Operators.product(ListModule.Map<Expression, Expression>(new MonomialCoefficient@390-1(symbol), item));
            }
            return Expression.Undefined;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("coefficientDegreeMonomialSV")]
        public static Tuple<Expression, Expression> MonomialCoefficientDegree(Expression symbol, Expression _arg1)
        {
            Expression expression;
            if (ExpressionPatterns.|Zero|_|(_arg1) != null)
            {
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.zero, MathNet.Symbolics.Operators.negativeInfinity);
            }
            if (_arg1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
            {
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.one, MathNet.Symbolics.Operators.one);
            }
            if (_arg1.Tag == 0)
            {
                expression = _arg1;
                return new Tuple<Expression, Expression>(expression, MathNet.Symbolics.Operators.zero);
            }
            FSharpOption<Tuple<Expression, Expression>> option2 = ExpressionPatterns.|PosIntPower|_|(_arg1);
            if ((option2 != null) && option2.Value.Item1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
            {
                expression = option2.Value.Item1;
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.one, option2.Value.Item2);
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                FSharpList<Expression> item = product.item;
                FSharpList<Tuple<Expression, Expression>> list = ListModule.Map<Expression, Tuple<Expression, Expression>>(new cds@400-2(symbol), item);
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.product(ListModule.Map<Tuple<Expression, Expression>, Expression>(new MonomialCoefficientDegree@401-2(), list)), MathNet.Symbolics.Operators.sum(ListModule.Map<Tuple<Expression, Expression>, Expression>(new MonomialCoefficientDegree@401-3(), list)));
            }
            return new Tuple<Expression, Expression>(Expression.Undefined, Expression.Undefined);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("degreeMonomialSV")]
        public static Expression MonomialDegree(Expression symbol, Expression _arg1)
        {
            if (ExpressionPatterns.|Zero|_|(_arg1) != null)
            {
                return MathNet.Symbolics.Operators.negativeInfinity;
            }
            if (_arg1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
            {
                return MathNet.Symbolics.Operators.one;
            }
            if (_arg1.Tag == 0)
            {
                return MathNet.Symbolics.Operators.zero;
            }
            FSharpOption<Tuple<Expression, Expression>> option2 = ExpressionPatterns.|PosIntPower|_|(_arg1);
            if ((option2 != null) && option2.Value.Item1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
            {
                Expression expression = option2.Value.Item1;
                return option2.Value.Item2;
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                FSharpList<Expression> item = product.item;
                return MathNet.Symbolics.Operators.sum(ListModule.Map<Expression, Expression>(new MonomialDegree@374-1(symbol), item));
            }
            return Expression.Undefined;
        }

        [Serializable]
        internal class cds@400-2 : FSharpFunc<Expression, Tuple<Expression, Expression>>
        {
            public Expression symbol;

            internal cds@400-2(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Tuple<Expression, Expression> Invoke(Expression _arg1) => 
                SingleVariablePolynomial.MonomialCoefficientDegree(this.symbol, _arg1);
        }

        [Serializable]
        internal class cds@419-3 : FSharpFunc<Expression, Tuple<Expression, Expression>>
        {
            public Expression symbol;

            internal cds@419-3(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Tuple<Expression, Expression> Invoke(Expression _arg1) => 
                SingleVariablePolynomial.MonomialCoefficientDegree(this.symbol, _arg1);
        }

        [Serializable]
        internal class Coefficient@410-3 : FSharpFunc<Tuple<Expression, Expression>, Expression>
        {
            internal Coefficient@410-3()
            {
            }

            public override Expression Invoke(Tuple<Expression, Expression> tuple) => 
                tuple.Item1;
        }

        [Serializable]
        internal class Coefficient@410-4 : FSharpFunc<Tuple<Expression, Expression>, bool>
        {
            public Expression ke;

            internal Coefficient@410-4(Expression ke)
            {
                this.ke = ke;
            }

            public override bool Invoke(Tuple<Expression, Expression> tupledArg)
            {
                Expression expression = tupledArg.Item1;
                Expression expression2 = tupledArg.Item2;
                return expression2.Equals(this.ke, LanguagePrimitives.GenericEqualityComparer);
            }
        }

        [Serializable]
        internal class Coefficient@410-5 : FSharpFunc<Expression, Tuple<Expression, Expression>>
        {
            public Expression symbol;

            internal Coefficient@410-5(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Tuple<Expression, Expression> Invoke(Expression _arg1) => 
                SingleVariablePolynomial.MonomialCoefficientDegree(this.symbol, _arg1);
        }

        [Serializable]
        internal class Coefficients@438-1 : OptimizedClosures.FSharpFunc<Expression[], Tuple<int, Expression>, Expression[]>
        {
            internal Coefficients@438-1()
            {
            }

            public override Expression[] Invoke(Expression[] s, Tuple<int, Expression> tupledArg)
            {
                int index = tupledArg.Item1;
                Expression y = tupledArg.Item2;
                s[index] = MathNet.Symbolics.Operators.add(s[index], y);
                return s;
            }
        }

        [Serializable]
        internal class collect@429-6 : OptimizedClosures.FSharpFunc<Expression, Expression, FSharpList<Tuple<int, Expression>>>
        {
            internal collect@429-6()
            {
            }

            public override FSharpList<Tuple<int, Expression>> Invoke(Expression symbol, Expression _arg1) => 
                SingleVariablePolynomial.collect@429-5(symbol, _arg1);
        }

        [Serializable]
        internal class collect@434-7 : OptimizedClosures.FSharpFunc<FSharpList<Tuple<int, Expression>>, FSharpList<Tuple<int, Expression>>, FSharpList<Tuple<int, Expression>>>
        {
            internal collect@434-7()
            {
            }

            public override FSharpList<Tuple<int, Expression>> Invoke(FSharpList<Tuple<int, Expression>> a, FSharpList<Tuple<int, Expression>> b) => 
                ListModule.Fold<Tuple<int, Expression>, FSharpList<Tuple<int, Expression>>>(new SingleVariablePolynomial.collect@434-8(b), FSharpList<Tuple<int, Expression>>.Empty, a);
        }

        [Serializable]
        internal class collect@434-8 : OptimizedClosures.FSharpFunc<FSharpList<Tuple<int, Expression>>, Tuple<int, Expression>, FSharpList<Tuple<int, Expression>>>
        {
            public FSharpList<Tuple<int, Expression>> b;

            internal collect@434-8(FSharpList<Tuple<int, Expression>> b)
            {
                this.b = b;
            }

            public override FSharpList<Tuple<int, Expression>> Invoke(FSharpList<Tuple<int, Expression>> s, Tuple<int, Expression> tupledArg)
            {
                int num = tupledArg.Item1;
                Expression expression = tupledArg.Item2;
                return ListModule.Fold<Tuple<int, Expression>, FSharpList<Tuple<int, Expression>>>(new SingleVariablePolynomial.collect@434-9(num, expression), s, this.b);
            }
        }

        [Serializable]
        internal class collect@434-9 : OptimizedClosures.FSharpFunc<FSharpList<Tuple<int, Expression>>, Tuple<int, Expression>, FSharpList<Tuple<int, Expression>>>
        {
            public Expression e1;
            public int o1;

            internal collect@434-9(int o1, Expression e1)
            {
                this.o1 = o1;
                this.e1 = e1;
            }

            public override FSharpList<Tuple<int, Expression>> Invoke(FSharpList<Tuple<int, Expression>> s, Tuple<int, Expression> tupledArg)
            {
                int num = tupledArg.Item1;
                Expression y = tupledArg.Item2;
                return FSharpList<Tuple<int, Expression>>.Cons(new Tuple<int, Expression>(this.o1 + num, MathNet.Symbolics.Operators.multiply(this.e1, y)), s);
            }
        }

        [Serializable]
        internal class Degree@382-2 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal Degree@382-2()
            {
            }

            public override Expression Invoke(Expression u, Expression v) => 
                Numbers.Max2(u, v);
        }

        [Serializable]
        internal class Degree@382-3 : FSharpFunc<Expression, Expression>
        {
            public Expression symbol;

            internal Degree@382-3(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Expression Invoke(Expression _arg1) => 
                SingleVariablePolynomial.MonomialDegree(this.symbol, _arg1);
        }

        [Serializable]
        internal class degree@420-3 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal degree@420-3()
            {
            }

            public override Expression Invoke(Expression u, Expression v) => 
                Numbers.Max2(u, v);
        }

        [Serializable]
        internal class degree@420-4 : FSharpFunc<Tuple<Expression, Expression>, Expression>
        {
            internal degree@420-4()
            {
            }

            public override Expression Invoke(Tuple<Expression, Expression> tuple) => 
                tuple.Item2;
        }

        [Serializable]
        internal class degree@437-5 : FSharpFunc<Tuple<int, Expression>, int>
        {
            internal degree@437-5()
            {
            }

            public override int Invoke(Tuple<int, Expression> tuple) => 
                tuple.Item1;
        }

        [Serializable]
        internal class IsMonomial@359-1 : FSharpFunc<Expression, bool>
        {
            public Expression symbol;

            internal IsMonomial@359-1(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override bool Invoke(Expression _arg1) => 
                SingleVariablePolynomial.IsMonomial(this.symbol, _arg1);
        }

        [Serializable]
        internal class IsPolynomial@364-1 : FSharpFunc<Expression, bool>
        {
            public Expression symbol;

            internal IsPolynomial@364-1(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override bool Invoke(Expression _arg1) => 
                SingleVariablePolynomial.IsMonomial(this.symbol, _arg1);
        }

        [Serializable]
        internal class LeadingCoefficientDegree@421-2 : FSharpFunc<Tuple<Expression, Expression>, Expression>
        {
            internal LeadingCoefficientDegree@421-2()
            {
            }

            public override Expression Invoke(Tuple<Expression, Expression> tuple) => 
                tuple.Item1;
        }

        [Serializable]
        internal class LeadingCoefficientDegree@421-3 : FSharpFunc<Tuple<Expression, Expression>, bool>
        {
            public Expression degree;

            internal LeadingCoefficientDegree@421-3(Expression degree)
            {
                this.degree = degree;
            }

            public override bool Invoke(Tuple<Expression, Expression> tupledArg)
            {
                Expression expression = tupledArg.Item1;
                Expression expression2 = tupledArg.Item2;
                return expression2.Equals(this.degree, LanguagePrimitives.GenericEqualityComparer);
            }
        }

        [Serializable]
        internal class MonomialCoefficient@390-1 : FSharpFunc<Expression, Expression>
        {
            public Expression symbol;

            internal MonomialCoefficient@390-1(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Expression Invoke(Expression _arg1) => 
                SingleVariablePolynomial.MonomialCoefficient(this.symbol, _arg1);
        }

        [Serializable]
        internal class MonomialCoefficientDegree@401-2 : FSharpFunc<Tuple<Expression, Expression>, Expression>
        {
            internal MonomialCoefficientDegree@401-2()
            {
            }

            public override Expression Invoke(Tuple<Expression, Expression> tuple) => 
                tuple.Item1;
        }

        [Serializable]
        internal class MonomialCoefficientDegree@401-3 : FSharpFunc<Tuple<Expression, Expression>, Expression>
        {
            internal MonomialCoefficientDegree@401-3()
            {
            }

            public override Expression Invoke(Tuple<Expression, Expression> tuple) => 
                tuple.Item2;
        }

        [Serializable]
        internal class MonomialDegree@374-1 : FSharpFunc<Expression, Expression>
        {
            public Expression symbol;

            internal MonomialDegree@374-1(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Expression Invoke(Expression _arg1) => 
                SingleVariablePolynomial.MonomialDegree(this.symbol, _arg1);
        }
    }
}

