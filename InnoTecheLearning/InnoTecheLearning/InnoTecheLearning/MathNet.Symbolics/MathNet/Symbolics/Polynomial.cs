namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    [CompilationMapping(SourceConstructFlags.Module)]
    public static class Polynomial
    {
        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("coefficient")]
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
                return MathNet.Symbolics.Operators.sum(ListModule.Map<Tuple<Expression, Expression>, Expression>(new Coefficient@163(), ListModule.Filter<Tuple<Expression, Expression>>(new Coefficient@163-1(expression), ListModule.Map<Expression, Tuple<Expression, Expression>>(new Coefficient@163-2(symbol), item))));
            }
            return Expression.Undefined;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("coefficients")]
        public static Expression[] Coefficients(Expression symbol, Expression x)
        {
            int num2;
            FSharpList<Tuple<int, Expression>> list = collect@182(symbol, x);
            IEnumerable<int> enumerable = SeqModule.Map<Tuple<int, Expression>, int>(new degree@191-2(), (IEnumerable<Tuple<int, Expression>>) list);
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
            return ListModule.Fold<Tuple<int, Expression>, Expression[]>(new Coefficients@192(), ArrayModule.Create<Expression>(num + 1, MathNet.Symbolics.Operators.zero), list);
        }

        internal static FSharpList<Tuple<int, Expression>> collect@182(Expression symbol, Expression _arg1)
        {
            Expression expression;
            FSharpList<Expression> item;
            FSharpFunc<Expression, FSharpFunc<Expression, FSharpList<Tuple<int, Expression>>>> func = new collect@182-1();
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
                    return ListModule.Reduce<FSharpList<Tuple<int, Expression>>>(new collect@187-2(), ListModule.Map<Expression, FSharpList<Tuple<int, Expression>>>(func.Invoke(symbol), item));
                }
            }
            if (Structure.IsFreeOf(symbol, _arg1))
            {
                return FSharpList<Tuple<int, Expression>>.Cons(new Tuple<int, Expression>(0, _arg1), FSharpList<Tuple<int, Expression>>.Empty);
            }
            return FSharpList<Tuple<int, Expression>>.Empty;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("collectTermsMonomial")]
        public static Tuple<Expression, Expression> CollectMonomialTerms(Expression symbol, Expression _arg1)
        {
            Expression expression;
            if (_arg1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
            {
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.one, _arg1);
            }
            if (_arg1.Tag == 0)
            {
                expression = _arg1;
                return new Tuple<Expression, Expression>(expression, MathNet.Symbolics.Operators.one);
            }
            FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|PosIntPower|_|(_arg1);
            if ((option != null) && option.Value.Item1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
            {
                expression = _arg1;
                Expression expression2 = option.Value.Item1;
                Expression expression3 = option.Value.Item2;
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.one, expression);
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                FSharpList<Expression> item = product.item;
                return ListModule.Reduce<Tuple<Expression, Expression>>(new CollectMonomialTerms@199(), ListModule.Map<Expression, Tuple<Expression, Expression>>(new CollectMonomialTerms@199-2(symbol), item));
            }
            if (Structure.IsFreeOf(symbol, _arg1))
            {
                return new Tuple<Expression, Expression>(_arg1, MathNet.Symbolics.Operators.one);
            }
            return new Tuple<Expression, Expression>(Expression.Undefined, Expression.Undefined);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("collectTermsMonomialMV")]
        public static Tuple<Expression, Expression> CollectMultivariateMonomialTerms(HashSet<Expression> symbols, Expression _arg1)
        {
            Expression expression;
            if (symbols.Contains(_arg1))
            {
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.one, _arg1);
            }
            if (_arg1.Tag == 0)
            {
                expression = _arg1;
                return new Tuple<Expression, Expression>(expression, MathNet.Symbolics.Operators.one);
            }
            FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|PosIntPower|_|(_arg1);
            if ((option != null) && symbols.Contains(option.Value.Item1))
            {
                expression = _arg1;
                Expression expression2 = option.Value.Item1;
                Expression expression3 = option.Value.Item2;
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.one, expression);
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                FSharpList<Expression> item = product.item;
                return ListModule.Reduce<Tuple<Expression, Expression>>(new CollectMultivariateMonomialTerms@208(), ListModule.Map<Expression, Tuple<Expression, Expression>>(new CollectMultivariateMonomialTerms@208-2(symbols), item));
            }
            if (Structure.IsFreeOfSet(symbols, _arg1))
            {
                return new Tuple<Expression, Expression>(_arg1, MathNet.Symbolics.Operators.one);
            }
            return new Tuple<Expression, Expression>(Expression.Undefined, Expression.Undefined);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("collectTermsMV")]
        public static Expression CollectMultivariateTerms(HashSet<Expression> symbols, Expression _arg1)
        {
            if (_arg1.Tag == 4)
            {
                Expression.Sum sum = (Expression.Sum) _arg1;
                FSharpList<Expression> item = sum.item;
                FSharpList<Tuple<Expression, Expression>> list2 = ListModule.Map<Expression, Tuple<Expression, Expression>>(new CollectMultivariateTerms@219-4(symbols), item);
                return SeqModule.Fold<Expression, Expression>(new CollectMultivariateTerms@219(), MathNet.Symbolics.Operators.zero, SeqModule.Map<Tuple<Expression, IEnumerable<Tuple<Expression, Expression>>>, Expression>(new CollectMultivariateTerms@219-1(), SeqModule.GroupBy<Tuple<Expression, Expression>, Expression>(new CollectMultivariateTerms@219-5(), (IEnumerable<Tuple<Expression, Expression>>) list2)));
            }
            Tuple<Expression, Expression> tuple = CollectMultivariateMonomialTerms(symbols, _arg1);
            Expression y = tuple.Item2;
            Expression x = tuple.Item1;
            Expression expression3 = Expression.Undefined;
            if (!x.Equals(expression3, LanguagePrimitives.GenericEqualityComparer))
            {
                return MathNet.Symbolics.Operators.multiply(x, y);
            }
            return Expression.Undefined;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("collectTerms")]
        public static Expression CollectTerms(Expression symbol, Expression _arg1)
        {
            if (_arg1.Tag == 4)
            {
                Expression.Sum sum = (Expression.Sum) _arg1;
                FSharpList<Expression> item = sum.item;
                FSharpList<Tuple<Expression, Expression>> list2 = ListModule.Map<Expression, Tuple<Expression, Expression>>(new CollectTerms@214-4(symbol), item);
                return SeqModule.Fold<Expression, Expression>(new CollectTerms@214(), MathNet.Symbolics.Operators.zero, SeqModule.Map<Tuple<Expression, IEnumerable<Tuple<Expression, Expression>>>, Expression>(new CollectTerms@214-1(), SeqModule.GroupBy<Tuple<Expression, Expression>, Expression>(new CollectTerms@214-5(), (IEnumerable<Tuple<Expression, Expression>>) list2)));
            }
            Tuple<Expression, Expression> tuple = CollectMonomialTerms(symbol, _arg1);
            Expression y = tuple.Item2;
            Expression x = tuple.Item1;
            Expression expression3 = Expression.Undefined;
            if (!x.Equals(expression3, LanguagePrimitives.GenericEqualityComparer))
            {
                return MathNet.Symbolics.Operators.multiply(x, y);
            }
            return Expression.Undefined;
        }

        [CompilationSourceName("commonFactors")]
        public static Expression CommonFactors(Expression _arg1)
        {
            if (ExpressionPatterns.|Zero|_|(_arg1) != null)
            {
                return MathNet.Symbolics.Operators.zero;
            }
            return CommonMonomialFactors(Algebraic.Summands(_arg1));
        }

        [CompilationSourceName("commonMonomialFactors")]
        public static Expression CommonMonomialFactors(FSharpList<Expression> xs)
        {
            FSharpFunc<Tuple<Expression, BigInteger>, Expression> mapping = new normalizePowers@109();
            FSharpFunc<FSharpList<Tuple<Expression, BigInteger>>, FSharpList<Expression>> func = new normalizePowers@109-1(mapping);
            FSharpFunc<Expression, Tuple<Expression, BigInteger>> func4 = new denormalizePowers@110();
            FSharpFunc<FSharpList<Expression>, FSharpList<Tuple<Expression, BigInteger>>> denormalizePowers = new denormalizePowers@110-1(func4);
            FSharpFunc<Expression, Tuple<BigInteger, FSharpList<Tuple<Expression, BigInteger>>>> func5 = new monomialFactors@114(denormalizePowers);
            Tuple<BigInteger, FSharpList<Tuple<Expression, BigInteger>>> tuple = ListModule.Reduce<Tuple<BigInteger, FSharpList<Tuple<Expression, BigInteger>>>>(intersect@116<Expression, BigInteger>(), ListModule.Map<Expression, Tuple<BigInteger, FSharpList<Tuple<Expression, BigInteger>>>>(func5, xs));
            FSharpList<Tuple<Expression, BigInteger>> list = tuple.Item2;
            return MathNet.Symbolics.Operators.product(FSharpList<Expression>.Cons(Expression.NewNumber(BigRational.FromBigInt(tuple.Item1)), func.Invoke(list)));
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("degree")]
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
                return ListModule.Reduce<Expression>(new Degree@93(), ListModule.Map<Expression, Expression>(new Degree@93-1(symbol), item));
            }
            return Expression.Undefined;
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1, 1 }), CompilationSourceName("diophantineGcd")]
        public static Tuple<Expression, Expression> DiophantineGcd(Expression symbol, Expression u, Expression v, Expression w)
        {
            Expression x = halfDiophantineGcd(symbol, u, v, w);
            Expression y = MathNet.Symbolics.Operators.multiply(x, u);
            Expression expression2 = Algebraic.Expand(MathNet.Symbolics.Operators.add(w, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, y)));
            return new Tuple<Expression, Expression>(x, Divide(symbol, expression2, v).Item1);
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("divide")]
        public static Tuple<Expression, Expression> Divide(Expression symbol, Expression u, Expression v)
        {
            Expression x = Degree(symbol, v);
            if (Numbers.Compare(x, MathNet.Symbolics.Operators.one) < 0)
            {
                return new Tuple<Expression, Expression>(Algebraic.Expand(MathNet.Symbolics.Operators.multiply(u, MathNet.Symbolics.Operators.invert(v))), MathNet.Symbolics.Operators.zero);
            }
            Expression expression2 = LeadingCoefficientDegree(symbol, v).Item1;
            Expression y = MathNet.Symbolics.Operators.multiply(expression2, MathNet.Symbolics.Operators.pow(symbol, x));
            Expression w = MathNet.Symbolics.Operators.add(v, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, y));
            y = symbol;
            Expression expression5 = x;
            Expression expression6 = expression2;
            Expression expression7 = w;
            return pd@230(symbol, x, expression2, w, MathNet.Symbolics.Operators.zero, u);
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("extendedGcd")]
        public static Tuple<Expression, Expression, Expression> ExtendedGcd(Expression symbol, Expression u, Expression v)
        {
            Tuple<Expression, Expression> tuple = HalfExtendedGcd(symbol, u, v);
            Expression x = tuple.Item1;
            Expression expression2 = tuple.Item2;
            Expression y = MathNet.Symbolics.Operators.multiply(expression2, u);
            Expression expression3 = Algebraic.Expand(MathNet.Symbolics.Operators.add(x, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, y)));
            return new Tuple<Expression, Expression, Expression>(x, expression2, Divide(symbol, expression3, v).Item1);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("factorSquareFree")]
        public static Expression FactorSquareFree(Expression symbol, Expression x)
        {
            Expression expression = symbol;
            if (MathNet.Symbolics.Operators.isZero(x))
            {
                return x;
            }
            Expression expression2 = LeadingCoefficientDegree(symbol, x).Item1;
            Expression u = Algebraic.Expand(MathNet.Symbolics.Operators.multiply(x, MathNet.Symbolics.Operators.invert(expression2)));
            Expression v = Gcd(symbol, u, Calculus.Differentiate(symbol, u));
            Expression f = Divide(symbol, u, v).Item1;
            return MathNet.Symbolics.Operators.multiply(expression2, impl@338-13(symbol, 1, v, f, MathNet.Symbolics.Operators.one));
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("fromCoefficients")]
        public static Expression FromCoefficients(Expression symbol, FSharpList<Expression> coefficients)
        {
            FSharpFunc<int, FSharpFunc<Expression, Expression>> mapping = new mkMonomial@17(symbol);
            IEnumerable<Expression> enumerable = ListModule.MapIndexed<Expression, Expression>(mapping, coefficients);
            using (IEnumerator<Expression> enumerator = enumerable.GetEnumerator())
            {
                Expression zero = MathNet.Symbolics.Operators.zero;
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                    zero = MathNet.Symbolics.Operators.add(zero, enumerator.Current);
                }
                return zero;
            }
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("gcd")]
        public static Expression Gcd(Expression symbol, Expression u, Expression v)
        {
            if (MathNet.Symbolics.Operators.isZero(u) && MathNet.Symbolics.Operators.isZero(v))
            {
                return MathNet.Symbolics.Operators.zero;
            }
            Expression expression = symbol;
            Expression x = inner@276(symbol, u, v);
            Expression expression3 = LeadingCoefficientDegree(symbol, x).Item1;
            return Algebraic.Expand(MathNet.Symbolics.Operators.multiply(x, MathNet.Symbolics.Operators.invert(expression3)));
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1, 1 }), CompilationSourceName("halfDiophantineGcd")]
        public static Expression halfDiophantineGcd(Expression symbol, Expression u, Expression v, Expression w)
        {
            Tuple<Expression, Expression> tuple = HalfExtendedGcd(symbol, u, v);
            Expression y = tuple.Item2;
            Expression expression2 = tuple.Item1;
            Tuple<Expression, Expression> tuple2 = Divide(symbol, w, expression2);
            Expression expression3 = tuple2.Item2;
            Expression x = tuple2.Item1;
            if (!MathNet.Symbolics.Operators.isZero(expression3))
            {
                return Expression.Undefined;
            }
            Expression expression5 = Algebraic.Expand(MathNet.Symbolics.Operators.multiply(x, y));
            if (!MathNet.Symbolics.Operators.isZero(expression5) && (Numbers.Compare(Degree(symbol, expression5), Degree(symbol, v)) >= 0))
            {
                return Divide(symbol, expression5, v).Item2;
            }
            return expression5;
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("halfExtendedGcd")]
        public static Tuple<Expression, Expression> HalfExtendedGcd(Expression symbol, Expression u, Expression v)
        {
            if (MathNet.Symbolics.Operators.isZero(u) && MathNet.Symbolics.Operators.isZero(v))
            {
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.zero, MathNet.Symbolics.Operators.zero);
            }
            Expression expression = symbol;
            Tuple<Expression, Expression> tuple = inner@285-1(symbol, u, v, MathNet.Symbolics.Operators.zero, MathNet.Symbolics.Operators.one);
            Expression x = tuple.Item1;
            Expression expression3 = tuple.Item2;
            Expression expression4 = LeadingCoefficientDegree(symbol, x).Item1;
            return new Tuple<Expression, Expression>(Algebraic.Expand(MathNet.Symbolics.Operators.multiply(x, MathNet.Symbolics.Operators.invert(expression4))), Algebraic.Expand(MathNet.Symbolics.Operators.multiply(expression3, MathNet.Symbolics.Operators.invert(expression4))));
        }

        internal static void impl@29-9(FSharpFunc<Expression, Unit> keep, Expression _arg1)
        {
            FSharpFunc<FSharpFunc<Expression, Unit>, FSharpFunc<Expression, Unit>> func = new impl@29-10();
            if (_arg1.Tag != 0)
            {
                FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|PosIntPower|_|(_arg1);
                if (option != null)
                {
                    Expression expression = option.Value.Item1;
                    keep.Invoke(expression);
                }
                else
                {
                    switch (_arg1.Tag)
                    {
                        case 4:
                        {
                            Expression.Sum sum = (Expression.Sum) _arg1;
                            FSharpList<Expression> item = sum.item;
                            ListModule.Iterate<Expression>(func.Invoke(keep), item);
                            return;
                        }
                        case 5:
                        {
                            Expression.Product product = (Expression.Product) _arg1;
                            ListModule.Iterate<Expression>(new impl@34-11(keep), product.item);
                            return;
                        }
                        case 6:
                            keep.Invoke(_arg1);
                            return;
                    }
                    keep.Invoke(_arg1);
                }
            }
        }

        internal static Tuple<Expression, FSharpList<Expression>> impl@321-12(Expression symbol, Expression n, FSharpList<Expression> df)
        {
            Expression expression3;
            Tuple<Expression, Expression> tuple = Divide(symbol, n, Algebraic.Expand(Expression.NewProduct(df)));
            Expression head = tuple.Item2;
            Expression expression2 = tuple.Item1;
            if (df.TailOrNull == null)
            {
                return new Tuple<Expression, FSharpList<Expression>>(expression2, FSharpList<Expression>.Empty);
            }
            FSharpList<Expression> list = df;
            if (list.TailOrNull.TailOrNull == null)
            {
                expression3 = list.HeadOrDefault;
                return new Tuple<Expression, FSharpList<Expression>>(expression2, FSharpList<Expression>.Cons(head, FSharpList<Expression>.Empty));
            }
            FSharpList<Expression> item = list.TailOrNull;
            expression3 = list.HeadOrDefault;
            Tuple<Expression, Expression> tuple2 = DiophantineGcd(symbol, Algebraic.Expand(Expression.NewProduct(item)), expression3, head);
            Expression expression4 = tuple2.Item2;
            Expression expression5 = tuple2.Item1;
            Tuple<Expression, FSharpList<Expression>> tuple3 = impl@321-12(symbol, expression4, item);
            Expression y = tuple3.Item1;
            FSharpList<Expression> tail = tuple3.Item2;
            return new Tuple<Expression, FSharpList<Expression>>(MathNet.Symbolics.Operators.add(expression2, y), FSharpList<Expression>.Cons(expression5, tail));
        }

        internal static Expression impl@338-13(Expression symbol, int j, Expression r, Expression f, Expression p)
        {
            if (MathNet.Symbolics.Operators.isOne(r))
            {
                return MathNet.Symbolics.Operators.multiply(p, MathNet.Symbolics.Operators.pow(f, MathNet.Symbolics.Operators.number.Invoke(j)));
            }
            Expression v = Gcd(symbol, r, f);
            Expression x = Divide(symbol, f, v).Item1;
            return MathNet.Symbolics.Operators.multiply(impl@338-13(symbol, j + 1, Divide(symbol, r, v).Item1, v, p), MathNet.Symbolics.Operators.pow(x, MathNet.Symbolics.Operators.number.Invoke(j)));
        }

        internal static Expression inner@276(Expression symbol, Expression x, Expression y)
        {
            while (!MathNet.Symbolics.Operators.isZero(y))
            {
                y = Divide(symbol, x, y).Item2;
                x = y;
                symbol = symbol;
            }
            return x;
        }

        internal static Tuple<Expression, Expression> inner@285-1(Expression symbol, Expression x, Expression y, Expression a', Expression a'')
        {
            while (!MathNet.Symbolics.Operators.isZero(y))
            {
                Tuple<Expression, Expression> tuple = Divide(symbol, x, y);
                Expression expression = tuple.Item2;
                Expression expression2 = tuple.Item1;
                Expression expression3 = MathNet.Symbolics.Operators.multiply(expression2, a');
                a'' = a';
                a' = MathNet.Symbolics.Operators.add(a'', MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression3));
                y = expression;
                x = y;
                symbol = symbol;
            }
            return new Tuple<Expression, Expression>(x, a'');
        }

        internal static FSharpFunc<Tuple<BigInteger, FSharpList<Tuple<a, b>>>, FSharpFunc<Tuple<BigInteger, FSharpList<Tuple<a, b>>>, Tuple<BigInteger, FSharpList<Tuple<a, b>>>>> intersect@116<a, b>() => 
            new intersect@119-1<a, b>();

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("isMonomial")]
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
                return ListModule.ForAll<Expression>(new IsMonomial@45(symbol), product.item);
            }
            return Structure.IsFreeOf(symbol, _arg1);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("isMonomialMV")]
        public static bool IsMultivariateMonomial(HashSet<Expression> symbols, Expression _arg1)
        {
            if (symbols.Contains(_arg1))
            {
                return true;
            }
            if (_arg1.Tag == 0)
            {
                return true;
            }
            FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|PosIntPower|_|(_arg1);
            if ((option != null) && symbols.Contains(option.Value.Item1))
            {
                Expression expression = option.Value.Item1;
                return true;
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                return ListModule.ForAll<Expression>(new IsMultivariateMonomial@53(symbols), product.item);
            }
            return Structure.IsFreeOfSet(symbols, _arg1);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("isPolynomialMV")]
        public static bool IsMultivariatePolynomial(HashSet<Expression> symbols, Expression _arg1)
        {
            if (_arg1.Tag == 4)
            {
                Expression.Sum sum = (Expression.Sum) _arg1;
                return ListModule.ForAll<Expression>(new IsMultivariatePolynomial@64(symbols), sum.item);
            }
            return IsMultivariateMonomial(symbols, _arg1);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("isPolynomial")]
        public static bool IsPolynomial(Expression symbol, Expression _arg1)
        {
            if (_arg1.Tag == 4)
            {
                Expression.Sum sum = (Expression.Sum) _arg1;
                return ListModule.ForAll<Expression>(new IsPolynomial@58(symbol), sum.item);
            }
            return IsMonomial(symbol, _arg1);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("isSquareFree")]
        public static bool IsSquareFree(Expression symbol, Expression x)
        {
            Expression expression = Gcd(symbol, x, Calculus.Differentiate(symbol, x));
            return MathNet.Symbolics.Operators.one.Equals(expression, LanguagePrimitives.GenericEqualityComparer);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("leadingCoefficient")]
        public static Expression LeadingCoefficient(Expression symbol, Expression x) => 
            LeadingCoefficientDegree(symbol, x).Item1;

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("leadingCoefficientDegree")]
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
                FSharpList<Tuple<Expression, Expression>> list = ListModule.Map<Expression, Tuple<Expression, Expression>>(new cds@172-1(symbol), item);
                expression3 = ListModule.Reduce<Expression>(new degree@173(), ListModule.Map<Tuple<Expression, Expression>, Expression>(new degree@173-1(), list));
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.sum(ListModule.Map<Tuple<Expression, Expression>, Expression>(new LeadingCoefficientDegree@174(), ListModule.Filter<Tuple<Expression, Expression>>(new LeadingCoefficientDegree@174-1(expression3), list))), expression3);
            }
            return new Tuple<Expression, Expression>(Expression.Undefined, Expression.Undefined);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("coefficientMonomial")]
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
                Expression expression2 = option.Value.Item2;
                return MathNet.Symbolics.Operators.one;
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                FSharpList<Expression> item = product.item;
                return MathNet.Symbolics.Operators.product(ListModule.Map<Expression, Expression>(new MonomialCoefficient@133(symbol), item));
            }
            if (Structure.IsFreeOf(symbol, _arg1))
            {
                return _arg1;
            }
            return Expression.Undefined;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("coefficientDegreeMonomial")]
        public static Tuple<Expression, Expression> MonomialCoefficientDegree(Expression symbol, Expression _arg1)
        {
            Expression expression;
            if (_arg1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
            {
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.one, MathNet.Symbolics.Operators.one);
            }
            if (_arg1.Tag == 0)
            {
                expression = _arg1;
                return new Tuple<Expression, Expression>(expression, MathNet.Symbolics.Operators.zero);
            }
            FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|PosIntPower|_|(_arg1);
            if ((option != null) && option.Value.Item1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
            {
                expression = option.Value.Item1;
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.one, option.Value.Item2);
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                FSharpList<Expression> item = product.item;
                FSharpList<Tuple<Expression, Expression>> list = ListModule.Map<Expression, Tuple<Expression, Expression>>(new cds@152(symbol), item);
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.product(ListModule.Map<Tuple<Expression, Expression>, Expression>(new MonomialCoefficientDegree@153(), list)), MathNet.Symbolics.Operators.sum(ListModule.Map<Tuple<Expression, Expression>, Expression>(new MonomialCoefficientDegree@153-1(), list)));
            }
            if (Structure.IsFreeOf(symbol, _arg1))
            {
                return new Tuple<Expression, Expression>(_arg1, MathNet.Symbolics.Operators.zero);
            }
            return new Tuple<Expression, Expression>(Expression.Undefined, Expression.Undefined);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("degreeMonomial")]
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
                return MathNet.Symbolics.Operators.sum(ListModule.Map<Expression, Expression>(new MonomialDegree@74(symbol), item));
            }
            if (Structure.IsFreeOf(symbol, _arg1))
            {
                return MathNet.Symbolics.Operators.zero;
            }
            return Expression.Undefined;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("degreeMV")]
        public static Expression MultivariateDegree(HashSet<Expression> symbols, Expression x)
        {
            Expression expression = MultivariateMonomialDegree(symbols, x);
            Expression expression2 = Expression.Undefined;
            if (!expression.Equals(expression2, LanguagePrimitives.GenericEqualityComparer))
            {
                return expression;
            }
            if (x.Tag == 4)
            {
                Expression.Sum sum = (Expression.Sum) x;
                FSharpList<Expression> item = sum.item;
                return ListModule.Reduce<Expression>(new MultivariateDegree@101(), ListModule.Map<Expression, Expression>(new MultivariateDegree@101-1(symbols), item));
            }
            return Expression.Undefined;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("coefficientMonomialMV")]
        public static Expression MultivariateMonomialCoefficient(HashSet<Expression> symbols, Expression _arg1)
        {
            if (symbols.Contains(_arg1))
            {
                return MathNet.Symbolics.Operators.one;
            }
            if (_arg1.Tag == 0)
            {
                return _arg1;
            }
            FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|PosIntPower|_|(_arg1);
            if ((option != null) && symbols.Contains(option.Value.Item1))
            {
                Expression expression = option.Value.Item1;
                Expression expression2 = option.Value.Item2;
                return MathNet.Symbolics.Operators.one;
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                FSharpList<Expression> item = product.item;
                return MathNet.Symbolics.Operators.product(ListModule.Map<Expression, Expression>(new MultivariateMonomialCoefficient@142(symbols), item));
            }
            if (Structure.IsFreeOfSet(symbols, _arg1))
            {
                return _arg1;
            }
            return Expression.Undefined;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("degreeMonomialMV")]
        public static Expression MultivariateMonomialDegree(HashSet<Expression> symbols, Expression _arg1)
        {
            if (ExpressionPatterns.|Zero|_|(_arg1) != null)
            {
                return MathNet.Symbolics.Operators.negativeInfinity;
            }
            if (symbols.Contains(_arg1))
            {
                return MathNet.Symbolics.Operators.one;
            }
            if (_arg1.Tag == 0)
            {
                return MathNet.Symbolics.Operators.zero;
            }
            FSharpOption<Tuple<Expression, Expression>> option2 = ExpressionPatterns.|PosIntPower|_|(_arg1);
            if ((option2 != null) && symbols.Contains(option2.Value.Item1))
            {
                Expression expression = option2.Value.Item1;
                return option2.Value.Item2;
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                FSharpList<Expression> item = product.item;
                return MathNet.Symbolics.Operators.sum(ListModule.Map<Expression, Expression>(new MultivariateMonomialDegree@84(symbols), item));
            }
            if (Structure.IsFreeOfSet(symbols, _arg1))
            {
                return MathNet.Symbolics.Operators.zero;
            }
            return Expression.Undefined;
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("partialFraction")]
        public static Tuple<Expression, FSharpList<Expression>> PartialFraction(Expression symbol, Expression numerator, FSharpList<Expression> denominatorFactors)
        {
            Expression expression = symbol;
            return impl@321-12(symbol, numerator, denominatorFactors);
        }

        internal static Tuple<Expression, Expression> pd@230(Expression symbol, Expression dv, Expression lcv, Expression w, Expression q, Expression r)
        {
            while (true)
            {
                Expression x = Degree(symbol, r);
                if (Numbers.Compare(x, dv) < 0)
                {
                    return new Tuple<Expression, Expression>(q, r);
                }
                Expression expression2 = LeadingCoefficientDegree(symbol, r).Item1;
                Expression y = MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(expression2, MathNet.Symbolics.Operators.invert(lcv)), MathNet.Symbolics.Operators.pow(symbol, MathNet.Symbolics.Operators.add(x, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, dv))));
                Expression expression5 = MathNet.Symbolics.Operators.multiply(expression2, MathNet.Symbolics.Operators.pow(symbol, x));
                Expression expression4 = MathNet.Symbolics.Operators.add(r, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression5));
                expression5 = MathNet.Symbolics.Operators.multiply(w, y);
                r = Algebraic.Expand(MathNet.Symbolics.Operators.add(expression4, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression5)));
                q = MathNet.Symbolics.Operators.add(q, y);
                w = w;
                lcv = lcv;
                dv = dv;
                symbol = symbol;
            }
        }

        internal static Tuple<Expression, Expression, Expression> pd@249-1(Expression symbol, Expression u, Expression v, Expression dv, Expression lcv, Expression n, Expression q, Expression r)
        {
            while (true)
            {
                Expression x = Degree(symbol, r);
                if (Numbers.Compare(x, dv) < 0)
                {
                    Expression expression2 = MathNet.Symbolics.Operators.pow(lcv, n);
                    return new Tuple<Expression, Expression, Expression>(Algebraic.Expand(MathNet.Symbolics.Operators.multiply(expression2, q)), Algebraic.Expand(MathNet.Symbolics.Operators.multiply(expression2, r)), MathNet.Symbolics.Operators.pow(lcv, MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.add(Degree(symbol, u), MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, dv)), MathNet.Symbolics.Operators.number.Invoke(1))));
                }
                Expression expression3 = MathNet.Symbolics.Operators.multiply(LeadingCoefficientDegree(symbol, r).Item1, MathNet.Symbolics.Operators.pow(symbol, MathNet.Symbolics.Operators.add(x, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, dv))));
                Expression expression4 = MathNet.Symbolics.Operators.multiply(lcv, r);
                Expression y = MathNet.Symbolics.Operators.multiply(expression3, v);
                r = Algebraic.Expand(MathNet.Symbolics.Operators.add(expression4, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, y)));
                q = MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.multiply(lcv, q), expression3);
                n = MathNet.Symbolics.Operators.add(n, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, MathNet.Symbolics.Operators.one));
                lcv = lcv;
                dv = dv;
                v = v;
                u = u;
                symbol = symbol;
            }
        }

        internal static Expression pe@266(Expression symbol, Expression t, Expression v, Expression _arg1)
        {
            if (ExpressionPatterns.|Zero|_|(_arg1) != null)
            {
                return MathNet.Symbolics.Operators.zero;
            }
            Tuple<Expression, Expression> tuple = Divide(symbol, _arg1, v);
            Expression y = tuple.Item2;
            Expression expression2 = tuple.Item1;
            return Algebraic.Expand(MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.multiply(t, pe@266(symbol, t, v, expression2)), y));
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1, 1 }), CompilationSourceName("polynomialExpansion")]
        public static Expression PolynomialExpansion(Expression symbol, Expression t, Expression u, Expression v)
        {
            Expression expression = symbol;
            Expression expression2 = t;
            Expression expression3 = v;
            return CollectTerms(t, pe@266(symbol, t, v, u));
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("pseudoDivide")]
        public static Tuple<Expression, Expression, Expression> PseudoDivide(Expression symbol, Expression u, Expression v)
        {
            Expression x = Degree(symbol, v);
            if (Numbers.Compare(x, MathNet.Symbolics.Operators.one) < 0)
            {
                return new Tuple<Expression, Expression, Expression>(u, MathNet.Symbolics.Operators.zero, v);
            }
            Expression lcv = LeadingCoefficientDegree(symbol, v).Item1;
            Expression expression3 = symbol;
            Expression expression4 = u;
            Expression expression5 = v;
            Expression expression6 = x;
            Expression expression7 = lcv;
            return pd@249-1(symbol, u, v, x, lcv, MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.add(Degree(symbol, u), MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, x)), MathNet.Symbolics.Operators.one), MathNet.Symbolics.Operators.zero, u);
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("pseudoQuot")]
        public static Expression PseudoQuotient(Expression symbol, Expression u, Expression v) => 
            PseudoDivide(symbol, u, v).Item1;

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("pseudoRemainder")]
        public static Expression PseudoRemainder(Expression symbol, Expression u, Expression v) => 
            PseudoDivide(symbol, u, v).Item2;

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("quot")]
        public static Expression Quotient(Expression symbol, Expression u, Expression v) => 
            Divide(symbol, u, v).Item1;

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("remainder")]
        public static Expression Remainder(Expression symbol, Expression u, Expression v) => 
            Divide(symbol, u, v).Item2;

        [CompilationSourceName("symbols")]
        public static HashSet<Expression> Symbols(FSharpList<Expression> xs) => 
            new HashSet<Expression>((IEnumerable<Expression>) xs, new Symbols@25());

        [CompilationSourceName("totalDegree")]
        public static Expression TotalDegree(Expression x) => 
            MultivariateDegree(Variables(x), x);

        [CompilationSourceName("variables")]
        public static HashSet<Expression> Variables(Expression x)
        {
            HashSet<Expression> hs = Symbols(FSharpList<Expression>.Empty);
            impl@29-9(new Variables@37(hs), x);
            return hs;
        }

        [Serializable]
        internal class cds@152 : FSharpFunc<Expression, Tuple<Expression, Expression>>
        {
            public Expression symbol;

            internal cds@152(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Tuple<Expression, Expression> Invoke(Expression _arg1) => 
                Polynomial.MonomialCoefficientDegree(this.symbol, _arg1);
        }

        [Serializable]
        internal class cds@172-1 : FSharpFunc<Expression, Tuple<Expression, Expression>>
        {
            public Expression symbol;

            internal cds@172-1(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Tuple<Expression, Expression> Invoke(Expression _arg1) => 
                Polynomial.MonomialCoefficientDegree(this.symbol, _arg1);
        }

        [Serializable]
        internal class Coefficient@163 : FSharpFunc<Tuple<Expression, Expression>, Expression>
        {
            internal Coefficient@163()
            {
            }

            public override Expression Invoke(Tuple<Expression, Expression> tuple) => 
                tuple.Item1;
        }

        [Serializable]
        internal class Coefficient@163-1 : FSharpFunc<Tuple<Expression, Expression>, bool>
        {
            public Expression ke;

            internal Coefficient@163-1(Expression ke)
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
        internal class Coefficient@163-2 : FSharpFunc<Expression, Tuple<Expression, Expression>>
        {
            public Expression symbol;

            internal Coefficient@163-2(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Tuple<Expression, Expression> Invoke(Expression _arg1) => 
                Polynomial.MonomialCoefficientDegree(this.symbol, _arg1);
        }

        [Serializable]
        internal class Coefficients@192 : OptimizedClosures.FSharpFunc<Expression[], Tuple<int, Expression>, Expression[]>
        {
            internal Coefficients@192()
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
        internal class collect@182-1 : OptimizedClosures.FSharpFunc<Expression, Expression, FSharpList<Tuple<int, Expression>>>
        {
            internal collect@182-1()
            {
            }

            public override FSharpList<Tuple<int, Expression>> Invoke(Expression symbol, Expression _arg1) => 
                Polynomial.collect@182(symbol, _arg1);
        }

        [Serializable]
        internal class collect@187-2 : OptimizedClosures.FSharpFunc<FSharpList<Tuple<int, Expression>>, FSharpList<Tuple<int, Expression>>, FSharpList<Tuple<int, Expression>>>
        {
            internal collect@187-2()
            {
            }

            public override FSharpList<Tuple<int, Expression>> Invoke(FSharpList<Tuple<int, Expression>> a, FSharpList<Tuple<int, Expression>> b) => 
                ListModule.Fold<Tuple<int, Expression>, FSharpList<Tuple<int, Expression>>>(new Polynomial.collect@187-3(b), FSharpList<Tuple<int, Expression>>.Empty, a);
        }

        [Serializable]
        internal class collect@187-3 : OptimizedClosures.FSharpFunc<FSharpList<Tuple<int, Expression>>, Tuple<int, Expression>, FSharpList<Tuple<int, Expression>>>
        {
            public FSharpList<Tuple<int, Expression>> b;

            internal collect@187-3(FSharpList<Tuple<int, Expression>> b)
            {
                this.b = b;
            }

            public override FSharpList<Tuple<int, Expression>> Invoke(FSharpList<Tuple<int, Expression>> s, Tuple<int, Expression> tupledArg)
            {
                int num = tupledArg.Item1;
                Expression expression = tupledArg.Item2;
                return ListModule.Fold<Tuple<int, Expression>, FSharpList<Tuple<int, Expression>>>(new Polynomial.collect@187-4(num, expression), s, this.b);
            }
        }

        [Serializable]
        internal class collect@187-4 : OptimizedClosures.FSharpFunc<FSharpList<Tuple<int, Expression>>, Tuple<int, Expression>, FSharpList<Tuple<int, Expression>>>
        {
            public Expression e1;
            public int o1;

            internal collect@187-4(int o1, Expression e1)
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
        internal class CollectMonomialTerms@199 : FSharpFunc<Tuple<Expression, Expression>, FSharpFunc<Tuple<Expression, Expression>, Tuple<Expression, Expression>>>
        {
            internal CollectMonomialTerms@199()
            {
            }

            public override FSharpFunc<Tuple<Expression, Expression>, Tuple<Expression, Expression>> Invoke(Tuple<Expression, Expression> tupledArg)
            {
                Expression expression = tupledArg.Item1;
                return new Polynomial.CollectMonomialTerms@199-1(expression, tupledArg.Item2);
            }
        }

        [Serializable]
        internal class CollectMonomialTerms@199-1 : FSharpFunc<Tuple<Expression, Expression>, Tuple<Expression, Expression>>
        {
            public Expression c1;
            public Expression v1;

            internal CollectMonomialTerms@199-1(Expression c1, Expression v1)
            {
                this.c1 = c1;
                this.v1 = v1;
            }

            public override Tuple<Expression, Expression> Invoke(Tuple<Expression, Expression> tupledArg)
            {
                Expression y = tupledArg.Item1;
                Expression expression2 = tupledArg.Item2;
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.multiply(this.c1, y), MathNet.Symbolics.Operators.multiply(this.v1, expression2));
            }
        }

        [Serializable]
        internal class CollectMonomialTerms@199-2 : FSharpFunc<Expression, Tuple<Expression, Expression>>
        {
            public Expression symbol;

            internal CollectMonomialTerms@199-2(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Tuple<Expression, Expression> Invoke(Expression _arg1) => 
                Polynomial.CollectMonomialTerms(this.symbol, _arg1);
        }

        [Serializable]
        internal class CollectMultivariateMonomialTerms@208 : FSharpFunc<Tuple<Expression, Expression>, FSharpFunc<Tuple<Expression, Expression>, Tuple<Expression, Expression>>>
        {
            internal CollectMultivariateMonomialTerms@208()
            {
            }

            public override FSharpFunc<Tuple<Expression, Expression>, Tuple<Expression, Expression>> Invoke(Tuple<Expression, Expression> tupledArg)
            {
                Expression expression = tupledArg.Item1;
                return new Polynomial.CollectMultivariateMonomialTerms@208-1(expression, tupledArg.Item2);
            }
        }

        [Serializable]
        internal class CollectMultivariateMonomialTerms@208-1 : FSharpFunc<Tuple<Expression, Expression>, Tuple<Expression, Expression>>
        {
            public Expression c1;
            public Expression v1;

            internal CollectMultivariateMonomialTerms@208-1(Expression c1, Expression v1)
            {
                this.c1 = c1;
                this.v1 = v1;
            }

            public override Tuple<Expression, Expression> Invoke(Tuple<Expression, Expression> tupledArg)
            {
                Expression y = tupledArg.Item1;
                Expression expression2 = tupledArg.Item2;
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.multiply(this.c1, y), MathNet.Symbolics.Operators.multiply(this.v1, expression2));
            }
        }

        [Serializable]
        internal class CollectMultivariateMonomialTerms@208-2 : FSharpFunc<Expression, Tuple<Expression, Expression>>
        {
            public HashSet<Expression> symbols;

            internal CollectMultivariateMonomialTerms@208-2(HashSet<Expression> symbols)
            {
                this.symbols = symbols;
            }

            public override Tuple<Expression, Expression> Invoke(Expression _arg1) => 
                Polynomial.CollectMultivariateMonomialTerms(this.symbols, _arg1);
        }

        [Serializable]
        internal class CollectMultivariateTerms@219 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal CollectMultivariateTerms@219()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                MathNet.Symbolics.Operators.add(x, y);
        }

        [Serializable]
        internal class CollectMultivariateTerms@219-1 : FSharpFunc<Tuple<Expression, IEnumerable<Tuple<Expression, Expression>>>, Expression>
        {
            internal CollectMultivariateTerms@219-1()
            {
            }

            public override Expression Invoke(Tuple<Expression, IEnumerable<Tuple<Expression, Expression>>> tupledArg)
            {
                Expression y = tupledArg.Item1;
                IEnumerable<Tuple<Expression, Expression>> source = tupledArg.Item2;
                return MathNet.Symbolics.Operators.multiply(SeqModule.Fold<Expression, Expression>(new Polynomial.CollectMultivariateTerms@219-2(), MathNet.Symbolics.Operators.zero, SeqModule.Map<Tuple<Expression, Expression>, Expression>(new Polynomial.CollectMultivariateTerms@219-3(), source)), y);
            }
        }

        [Serializable]
        internal class CollectMultivariateTerms@219-2 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal CollectMultivariateTerms@219-2()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                MathNet.Symbolics.Operators.add(x, y);
        }

        [Serializable]
        internal class CollectMultivariateTerms@219-3 : FSharpFunc<Tuple<Expression, Expression>, Expression>
        {
            internal CollectMultivariateTerms@219-3()
            {
            }

            public override Expression Invoke(Tuple<Expression, Expression> tuple) => 
                tuple.Item1;
        }

        [Serializable]
        internal class CollectMultivariateTerms@219-4 : FSharpFunc<Expression, Tuple<Expression, Expression>>
        {
            public HashSet<Expression> symbols;

            internal CollectMultivariateTerms@219-4(HashSet<Expression> symbols)
            {
                this.symbols = symbols;
            }

            public override Tuple<Expression, Expression> Invoke(Expression _arg1) => 
                Polynomial.CollectMultivariateMonomialTerms(this.symbols, _arg1);
        }

        [Serializable]
        internal class CollectMultivariateTerms@219-5 : FSharpFunc<Tuple<Expression, Expression>, Expression>
        {
            internal CollectMultivariateTerms@219-5()
            {
            }

            public override Expression Invoke(Tuple<Expression, Expression> tuple) => 
                tuple.Item2;
        }

        [Serializable]
        internal class CollectTerms@214 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal CollectTerms@214()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                MathNet.Symbolics.Operators.add(x, y);
        }

        [Serializable]
        internal class CollectTerms@214-1 : FSharpFunc<Tuple<Expression, IEnumerable<Tuple<Expression, Expression>>>, Expression>
        {
            internal CollectTerms@214-1()
            {
            }

            public override Expression Invoke(Tuple<Expression, IEnumerable<Tuple<Expression, Expression>>> tupledArg)
            {
                Expression y = tupledArg.Item1;
                IEnumerable<Tuple<Expression, Expression>> source = tupledArg.Item2;
                return MathNet.Symbolics.Operators.multiply(SeqModule.Fold<Expression, Expression>(new Polynomial.CollectTerms@214-2(), MathNet.Symbolics.Operators.zero, SeqModule.Map<Tuple<Expression, Expression>, Expression>(new Polynomial.CollectTerms@214-3(), source)), y);
            }
        }

        [Serializable]
        internal class CollectTerms@214-2 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal CollectTerms@214-2()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                MathNet.Symbolics.Operators.add(x, y);
        }

        [Serializable]
        internal class CollectTerms@214-3 : FSharpFunc<Tuple<Expression, Expression>, Expression>
        {
            internal CollectTerms@214-3()
            {
            }

            public override Expression Invoke(Tuple<Expression, Expression> tuple) => 
                tuple.Item1;
        }

        [Serializable]
        internal class CollectTerms@214-4 : FSharpFunc<Expression, Tuple<Expression, Expression>>
        {
            public Expression symbol;

            internal CollectTerms@214-4(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Tuple<Expression, Expression> Invoke(Expression _arg1) => 
                Polynomial.CollectMonomialTerms(this.symbol, _arg1);
        }

        [Serializable]
        internal class CollectTerms@214-5 : FSharpFunc<Tuple<Expression, Expression>, Expression>
        {
            internal CollectTerms@214-5()
            {
            }

            public override Expression Invoke(Tuple<Expression, Expression> tuple) => 
                tuple.Item2;
        }

        [Serializable]
        internal class degree@173 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal degree@173()
            {
            }

            public override Expression Invoke(Expression u, Expression v) => 
                Numbers.Max2(u, v);
        }

        [Serializable]
        internal class degree@173-1 : FSharpFunc<Tuple<Expression, Expression>, Expression>
        {
            internal degree@173-1()
            {
            }

            public override Expression Invoke(Tuple<Expression, Expression> tuple) => 
                tuple.Item2;
        }

        [Serializable]
        internal class degree@191-2 : FSharpFunc<Tuple<int, Expression>, int>
        {
            internal degree@191-2()
            {
            }

            public override int Invoke(Tuple<int, Expression> tuple) => 
                tuple.Item1;
        }

        [Serializable]
        internal class Degree@93 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal Degree@93()
            {
            }

            public override Expression Invoke(Expression u, Expression v) => 
                Numbers.Max2(u, v);
        }

        [Serializable]
        internal class Degree@93-1 : FSharpFunc<Expression, Expression>
        {
            public Expression symbol;

            internal Degree@93-1(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Expression Invoke(Expression _arg1) => 
                Polynomial.MonomialDegree(this.symbol, _arg1);
        }

        [Serializable]
        internal class denormalizePowers@110 : FSharpFunc<Expression, Tuple<Expression, BigInteger>>
        {
            internal denormalizePowers@110()
            {
            }

            public override Tuple<Expression, BigInteger> Invoke(Expression _arg1)
            {
                FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|PosIntPower|_|(_arg1);
                if (option != null)
                {
                    FSharpOption<BigRational> option2 = ExpressionPatterns.|Integer|_|(option.Value.Item2);
                    if (option2 != null)
                    {
                        Expression expression = option.Value.Item1;
                        BigRational rational = option2.Value;
                        return new Tuple<Expression, BigInteger>(expression, rational.Numerator);
                    }
                }
                return new Tuple<Expression, BigInteger>(_arg1, BigInteger.One);
            }
        }

        [Serializable]
        internal class denormalizePowers@110-1 : FSharpFunc<FSharpList<Expression>, FSharpList<Tuple<Expression, BigInteger>>>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public FSharpFunc<Expression, Tuple<Expression, BigInteger>> mapping;

            internal denormalizePowers@110-1(FSharpFunc<Expression, Tuple<Expression, BigInteger>> mapping)
            {
                this.mapping = mapping;
            }

            public override FSharpList<Tuple<Expression, BigInteger>> Invoke(FSharpList<Expression> list) => 
                ListModule.Map<Expression, Tuple<Expression, BigInteger>>(this.mapping, list);
        }

        [Serializable]
        internal class impl@29-10 : OptimizedClosures.FSharpFunc<FSharpFunc<Expression, Unit>, Expression, Unit>
        {
            internal impl@29-10()
            {
            }

            public override Unit Invoke(FSharpFunc<Expression, Unit> keep, Expression _arg1)
            {
                Polynomial.impl@29-9(keep, _arg1);
                return null;
            }
        }

        [Serializable]
        internal class impl@34-11 : FSharpFunc<Expression, Unit>
        {
            public FSharpFunc<Expression, Unit> keep;

            internal impl@34-11(FSharpFunc<Expression, Unit> keep)
            {
                this.keep = keep;
            }

            public override Unit Invoke(Expression a)
            {
                if (a.Tag == 4)
                {
                    return this.keep.Invoke(a);
                }
                Polynomial.impl@29-9(this.keep, a);
                return null;
            }
        }

        [Serializable]
        internal class intersect@118-3<a, b> : FSharpFunc<Tuple<a, b>, FSharpOption<Tuple<a, b>>>
        {
            public FSharpList<Tuple<a, b>> x2;

            internal intersect@118-3(FSharpList<Tuple<a, b>> x2)
            {
                this.x2 = x2;
            }

            public override FSharpOption<Tuple<a, b>> Invoke(Tuple<a, b> tupledArg)
            {
                a local = tupledArg.Item1;
                b local2 = tupledArg.Item2;
                return ListModule.TryPick<Tuple<a, b>, Tuple<a, b>>(new Polynomial.intersect@118-4<a, b>(local, local2), this.x2);
            }
        }

        [Serializable]
        internal class intersect@118-4<a, b> : FSharpFunc<Tuple<a, b>, FSharpOption<Tuple<a, b>>>
        {
            public b p1;
            public a r1;

            internal intersect@118-4(a r1, b p1)
            {
                this.r1 = r1;
                this.p1 = p1;
            }

            public override FSharpOption<Tuple<a, b>> Invoke(Tuple<a, b> tupledArg)
            {
                a x = tupledArg.Item1;
                b y = tupledArg.Item2;
                if (LanguagePrimitives.HashCompare.GenericEqualityIntrinsic<a>(x, this.r1))
                {
                    return FSharpOption<Tuple<a, b>>.Some(new Tuple<a, b>(this.r1, !LanguagePrimitives.HashCompare.GenericLessThanIntrinsic<b>(this.p1, y) ? y : this.p1));
                }
                return null;
            }
        }

        [Serializable]
        internal class intersect@119-1<a, b> : FSharpFunc<Tuple<BigInteger, FSharpList<Tuple<a, b>>>, FSharpFunc<Tuple<BigInteger, FSharpList<Tuple<a, b>>>, Tuple<BigInteger, FSharpList<Tuple<a, b>>>>>
        {
            internal intersect@119-1()
            {
            }

            public override FSharpFunc<Tuple<BigInteger, FSharpList<Tuple<a, b>>>, Tuple<BigInteger, FSharpList<Tuple<a, b>>>> Invoke(Tuple<BigInteger, FSharpList<Tuple<a, b>>> tupledArg)
            {
                BigInteger integer = tupledArg.Item1;
                return new Polynomial.intersect@119-2<a, b>(integer, tupledArg.Item2);
            }
        }

        [Serializable]
        internal class intersect@119-2<a, b> : FSharpFunc<Tuple<BigInteger, FSharpList<Tuple<a, b>>>, Tuple<BigInteger, FSharpList<Tuple<a, b>>>>
        {
            public BigInteger n1;
            public FSharpList<Tuple<a, b>> x1;

            internal intersect@119-2(BigInteger n1, FSharpList<Tuple<a, b>> x1)
            {
                this.n1 = n1;
                this.x1 = x1;
            }

            public override Tuple<BigInteger, FSharpList<Tuple<a, b>>> Invoke(Tuple<BigInteger, FSharpList<Tuple<a, b>>> tupledArg)
            {
                BigInteger b = tupledArg.Item1;
                FSharpList<Tuple<a, b>> list = tupledArg.Item2;
                return new Tuple<BigInteger, FSharpList<Tuple<a, b>>>(Euclid.GreatestCommonDivisor(this.n1, b), ListModule.Choose<Tuple<a, b>, Tuple<a, b>>(new Polynomial.intersect@118-3<a, b>(list), this.x1));
            }
        }

        [Serializable]
        internal class IsMonomial@45 : FSharpFunc<Expression, bool>
        {
            public Expression symbol;

            internal IsMonomial@45(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override bool Invoke(Expression _arg1) => 
                Polynomial.IsMonomial(this.symbol, _arg1);
        }

        [Serializable]
        internal class IsMultivariateMonomial@53 : FSharpFunc<Expression, bool>
        {
            public HashSet<Expression> symbols;

            internal IsMultivariateMonomial@53(HashSet<Expression> symbols)
            {
                this.symbols = symbols;
            }

            public override bool Invoke(Expression _arg1) => 
                Polynomial.IsMultivariateMonomial(this.symbols, _arg1);
        }

        [Serializable]
        internal class IsMultivariatePolynomial@64 : FSharpFunc<Expression, bool>
        {
            public HashSet<Expression> symbols;

            internal IsMultivariatePolynomial@64(HashSet<Expression> symbols)
            {
                this.symbols = symbols;
            }

            public override bool Invoke(Expression _arg1) => 
                Polynomial.IsMultivariateMonomial(this.symbols, _arg1);
        }

        [Serializable]
        internal class IsPolynomial@58 : FSharpFunc<Expression, bool>
        {
            public Expression symbol;

            internal IsPolynomial@58(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override bool Invoke(Expression _arg1) => 
                Polynomial.IsMonomial(this.symbol, _arg1);
        }

        [Serializable]
        internal class LeadingCoefficientDegree@174 : FSharpFunc<Tuple<Expression, Expression>, Expression>
        {
            internal LeadingCoefficientDegree@174()
            {
            }

            public override Expression Invoke(Tuple<Expression, Expression> tuple) => 
                tuple.Item1;
        }

        [Serializable]
        internal class LeadingCoefficientDegree@174-1 : FSharpFunc<Tuple<Expression, Expression>, bool>
        {
            public Expression degree;

            internal LeadingCoefficientDegree@174-1(Expression degree)
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
        internal class mkMonomial@17 : OptimizedClosures.FSharpFunc<int, Expression, Expression>
        {
            public Expression symbol;

            internal mkMonomial@17(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Expression Invoke(int idx, Expression coeff)
            {
                if (idx == 0)
                {
                    return coeff;
                }
                return MathNet.Symbolics.Operators.multiply(coeff, MathNet.Symbolics.Operators.pow(this.symbol, Expression.NewNumber(BigRational.FromInt(idx))));
            }
        }

        [Serializable]
        internal class MonomialCoefficient@133 : FSharpFunc<Expression, Expression>
        {
            public Expression symbol;

            internal MonomialCoefficient@133(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Expression Invoke(Expression _arg1) => 
                Polynomial.MonomialCoefficient(this.symbol, _arg1);
        }

        [Serializable]
        internal class MonomialCoefficientDegree@153 : FSharpFunc<Tuple<Expression, Expression>, Expression>
        {
            internal MonomialCoefficientDegree@153()
            {
            }

            public override Expression Invoke(Tuple<Expression, Expression> tuple) => 
                tuple.Item1;
        }

        [Serializable]
        internal class MonomialCoefficientDegree@153-1 : FSharpFunc<Tuple<Expression, Expression>, Expression>
        {
            internal MonomialCoefficientDegree@153-1()
            {
            }

            public override Expression Invoke(Tuple<Expression, Expression> tuple) => 
                tuple.Item2;
        }

        [Serializable]
        internal class MonomialDegree@74 : FSharpFunc<Expression, Expression>
        {
            public Expression symbol;

            internal MonomialDegree@74(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Expression Invoke(Expression _arg1) => 
                Polynomial.MonomialDegree(this.symbol, _arg1);
        }

        [Serializable]
        internal class monomialFactors@114 : FSharpFunc<Expression, Tuple<BigInteger, FSharpList<Tuple<Expression, BigInteger>>>>
        {
            public FSharpFunc<FSharpList<Expression>, FSharpList<Tuple<Expression, BigInteger>>> denormalizePowers;

            internal monomialFactors@114(FSharpFunc<FSharpList<Expression>, FSharpList<Tuple<Expression, BigInteger>>> denormalizePowers)
            {
                this.denormalizePowers = denormalizePowers;
            }

            public override Tuple<BigInteger, FSharpList<Tuple<Expression, BigInteger>>> Invoke(Expression x)
            {
                Tuple<BigInteger, FSharpList<Expression>> tuple = Algebraic.FactorsInteger(x);
                FSharpList<Expression> func = tuple.Item2;
                return new Tuple<BigInteger, FSharpList<Tuple<Expression, BigInteger>>>(tuple.Item1, this.denormalizePowers.Invoke(func));
            }
        }

        [Serializable]
        internal class MultivariateDegree@101 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal MultivariateDegree@101()
            {
            }

            public override Expression Invoke(Expression u, Expression v) => 
                Numbers.Max2(u, v);
        }

        [Serializable]
        internal class MultivariateDegree@101-1 : FSharpFunc<Expression, Expression>
        {
            public HashSet<Expression> symbols;

            internal MultivariateDegree@101-1(HashSet<Expression> symbols)
            {
                this.symbols = symbols;
            }

            public override Expression Invoke(Expression _arg1) => 
                Polynomial.MultivariateMonomialDegree(this.symbols, _arg1);
        }

        [Serializable]
        internal class MultivariateMonomialCoefficient@142 : FSharpFunc<Expression, Expression>
        {
            public HashSet<Expression> symbols;

            internal MultivariateMonomialCoefficient@142(HashSet<Expression> symbols)
            {
                this.symbols = symbols;
            }

            public override Expression Invoke(Expression _arg1) => 
                Polynomial.MultivariateMonomialCoefficient(this.symbols, _arg1);
        }

        [Serializable]
        internal class MultivariateMonomialDegree@84 : FSharpFunc<Expression, Expression>
        {
            public HashSet<Expression> symbols;

            internal MultivariateMonomialDegree@84(HashSet<Expression> symbols)
            {
                this.symbols = symbols;
            }

            public override Expression Invoke(Expression _arg1) => 
                Polynomial.MultivariateMonomialDegree(this.symbols, _arg1);
        }

        [Serializable]
        internal class normalizePowers@109 : FSharpFunc<Tuple<Expression, BigInteger>, Expression>
        {
            internal normalizePowers@109()
            {
            }

            public override Expression Invoke(Tuple<Expression, BigInteger> tupledArg)
            {
                Expression x = tupledArg.Item1;
                BigInteger integer = tupledArg.Item2;
                return MathNet.Symbolics.Operators.pow(x, Expression.NewNumber(BigRational.FromBigInt(integer)));
            }
        }

        [Serializable]
        internal class normalizePowers@109-1 : FSharpFunc<FSharpList<Tuple<Expression, BigInteger>>, FSharpList<Expression>>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public FSharpFunc<Tuple<Expression, BigInteger>, Expression> mapping;

            internal normalizePowers@109-1(FSharpFunc<Tuple<Expression, BigInteger>, Expression> mapping)
            {
                this.mapping = mapping;
            }

            public override FSharpList<Expression> Invoke(FSharpList<Tuple<Expression, BigInteger>> list) => 
                ListModule.Map<Tuple<Expression, BigInteger>, Expression>(this.mapping, list);
        }

        [Serializable, CompilationMapping(SourceConstructFlags.Closure)]
        internal sealed class Symbols@25 : IEqualityComparer<Expression>
        {
            public Symbols@25() : this()
            {
            }

            private bool System-Collections-Generic-IEqualityComparer`1-Equals(Expression x, Expression y) => 
                x.Equals(y, LanguagePrimitives.GenericEqualityComparer);

            private int System-Collections-Generic-IEqualityComparer`1-GetHashCode(Expression obj) => 
                obj.GetHashCode(LanguagePrimitives.GenericEqualityERComparer);
        }

        [Serializable]
        internal class Variables@37 : FSharpFunc<Expression, Unit>
        {
            public HashSet<Expression> hs;

            internal Variables@37(HashSet<Expression> hs)
            {
                this.hs = hs;
            }

            public override Unit Invoke(Expression x)
            {
                bool flag = this.hs.Add(x);
                return null;
            }
        }
    }
}

