namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using Microsoft.FSharp.Core.CompilerServices;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    [RequireQualifiedAccess, CompilationMapping(SourceConstructFlags.Module)]
    public static class Algebraic
    {
        [CompilationSourceName("expand")]
        public static Expression Expand(Expression _arg1)
        {
            FSharpList<Expression> item;
            switch (_arg1.Tag)
            {
                case 4:
                    item = ((Expression.Sum) _arg1).item;
                    return MathNet.Symbolics.Operators.sum(ListModule.Map<Expression, Expression>(new Expand@57(), item));

                case 5:
                    item = ((Expression.Product) _arg1).item;
                    return ListModule.Reduce<Expression>(new Expand@58-1(), ListModule.Map<Expression, Expression>(new Expand@58-2(), item));
            }
            FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|PosIntPower|_|(_arg1);
            if ((option != null) && (option.Value.Item2.Tag == 0))
            {
                Expression.Number number = (Expression.Number) option.Value.Item2;
                Expression expression = option.Value.Item1;
                BigRational n = number.item;
                return expandPower(Expand(expression), BigRational.ToInt32(n));
            }
            return _arg1;
        }

        [CompilationSourceName("expandMain")]
        public static Expression ExpandMain(Expression _arg1)
        {
            if (_arg1.Tag == 5)
            {
                FSharpList<Expression> item = ((Expression.Product) _arg1).item;
                return ListModule.Reduce<Expression>(new ExpandMain@65(), item);
            }
            FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|PosIntPower|_|(_arg1);
            if ((option != null) && (option.Value.Item2.Tag == 0))
            {
                Expression.Number number = (Expression.Number) option.Value.Item2;
                Expression x = option.Value.Item1;
                BigRational n = number.item;
                return expandPower(x, BigRational.ToInt32(n));
            }
            return _arg1;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        internal static Expression expandPower(Expression x, int y)
        {
            if (x.Tag == 4)
            {
                Expression.Sum sum = (Expression.Sum) x;
                if (sum.item.TailOrNull != null)
                {
                    int num;
                    Expression expression;
                    FSharpList<Expression> item = sum.item;
                    if (item.TailOrNull.TailOrNull == null)
                    {
                        num = y;
                        expression = item.HeadOrDefault;
                        return MathNet.Symbolics.Operators.pow(expression, MathNet.Symbolics.Operators.number.Invoke(num));
                    }
                    if (y > 1)
                    {
                        num = y;
                        FSharpList<Expression> ax = item.TailOrNull;
                        expression = item.HeadOrDefault;
                        return MathNet.Symbolics.Operators.sum(ListModule.Map<Tuple<int, int>, Expression>(new expandPower@50(num, ax, expression), SeqModule.ToList<Tuple<int, int>>(new expandPower@49-1(num, 0, null, 0, null))));
                    }
                }
            }
            return MathNet.Symbolics.Operators.pow(x, MathNet.Symbolics.Operators.number.Invoke(y));
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        internal static Expression expandProduct(Expression x, Expression y)
        {
            Expression.Sum sum;
            Expression expression;
            FSharpList<Expression> item;
            if (x.Tag != 4)
            {
                if (y.Tag != 4)
                {
                    return MathNet.Symbolics.Operators.multiply(x, y);
                }
                sum = (Expression.Sum) y;
                item = sum.item;
                expression = x;
            }
            else
            {
                sum = (Expression.Sum) x;
                expression = y;
                item = sum.item;
            }
            return MathNet.Symbolics.Operators.sum(ListModule.Map<Expression, Expression>(new expandProduct@41(expression), item));
        }

        [CompilationSourceName("factors")]
        public static FSharpList<Expression> Factors(Expression _arg1)
        {
            if (_arg1.Tag == 5)
            {
                return ((Expression.Product) _arg1).item;
            }
            return FSharpList<Expression>.Cons(_arg1, FSharpList<Expression>.Empty);
        }

        [CompilationSourceName("factorsInteger")]
        public static Tuple<BigInteger, FSharpList<Expression>> FactorsInteger(Expression x)
        {
            FSharpOption<BigRational> option = ExpressionPatterns.|Integer|_|(x);
            if (option != null)
            {
                BigRational rational = option.Value;
                return new Tuple<BigInteger, FSharpList<Expression>>(rational.Numerator, FSharpList<Expression>.Empty);
            }
            return factorsInteger$cont@24(x, null);
        }

        [CompilerGenerated]
        internal static Tuple<BigInteger, FSharpList<Expression>> factorsInteger$cont@24(Expression x, Unit unitVar)
        {
            Expression.Product product;
            FSharpList<Expression> list;
            BigRational item;
            FSharpList<Expression> list2;
            BigInteger one;
            BigInteger denominator;
            Expression expression;
            switch (x.Tag)
            {
                case 0:
                    item = ((Expression.Number) x).item;
                    one = BigInteger.One;
                    denominator = item.Denominator;
                    return new Tuple<BigInteger, FSharpList<Expression>>(item.Numerator, FSharpList<Expression>.Cons(Expression.NewNumber(BigRational.FromBigIntFraction(one, denominator)), FSharpList<Expression>.Empty));

                case 5:
                {
                    product = (Expression.Product) x;
                    if (product.item.TailOrNull == null)
                    {
                        break;
                    }
                    list = product.item;
                    FSharpOption<BigRational> option = ExpressionPatterns.|Integer|_|(list.HeadOrDefault);
                    if (option == null)
                    {
                        break;
                    }
                    item = option.Value;
                    list2 = list.TailOrNull;
                    return new Tuple<BigInteger, FSharpList<Expression>>(item.Numerator, list2);
                }
            }
            if (x.Tag != 5)
            {
                expression = x;
            }
            else
            {
                product = (Expression.Product) x;
                if (product.item.TailOrNull == null)
                {
                    expression = x;
                }
                else
                {
                    list = product.item;
                    if (list.HeadOrDefault.Tag == 0)
                    {
                        Expression.Number number = (Expression.Number) list.HeadOrDefault;
                        item = number.item;
                        list2 = list.TailOrNull;
                        one = BigInteger.One;
                        denominator = item.Denominator;
                        return new Tuple<BigInteger, FSharpList<Expression>>(item.Numerator, FSharpList<Expression>.Cons(Expression.NewNumber(BigRational.FromBigIntFraction(one, denominator)), list2));
                    }
                    expression = x;
                }
            }
            return new Tuple<BigInteger, FSharpList<Expression>>(BigInteger.One, FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty));
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("separateFactors")]
        public static Tuple<Expression, Expression> SeparateFactors(Expression symbol, Expression x)
        {
            if (x.Tag == 5)
            {
                Expression.Product product = (Expression.Product) x;
                FSharpList<Expression> item = product.item;
                Tuple<FSharpList<Expression>, FSharpList<Expression>> tuple = ListModule.Partition<Expression>(new SeparateFactors@35(symbol), item);
                FSharpList<Expression> xs = tuple.Item1;
                FSharpList<Expression> list3 = tuple.Item2;
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.product(xs), MathNet.Symbolics.Operators.product(list3));
            }
            if (Structure.IsFreeOf(symbol, x))
            {
                return new Tuple<Expression, Expression>(x, MathNet.Symbolics.Operators.one);
            }
            return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.one, x);
        }

        [CompilationSourceName("summands")]
        public static FSharpList<Expression> Summands(Expression _arg1)
        {
            if (_arg1.Tag == 4)
            {
                return ((Expression.Sum) _arg1).item;
            }
            return FSharpList<Expression>.Cons(_arg1, FSharpList<Expression>.Empty);
        }

        [Serializable]
        internal class Expand@57 : FSharpFunc<Expression, Expression>
        {
            internal Expand@57()
            {
            }

            public override Expression Invoke(Expression _arg1) => 
                Algebraic.Expand(_arg1);
        }

        [Serializable]
        internal class Expand@58-1 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal Expand@58-1()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                Algebraic.expandProduct(x, y);
        }

        [Serializable]
        internal class Expand@58-2 : FSharpFunc<Expression, Expression>
        {
            internal Expand@58-2()
            {
            }

            public override Expression Invoke(Expression _arg1) => 
                Algebraic.Expand(_arg1);
        }

        [Serializable]
        internal class ExpandMain@65 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal ExpandMain@65()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                Algebraic.expandProduct(x, y);
        }

        [Serializable, CompilationMapping(SourceConstructFlags.Closure)]
        internal sealed class expandPower@49-1 : GeneratedSequenceBase<Tuple<int, int>>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public Tuple<int, int> current;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public IEnumerator<int> @enum;
            public int k;
            public int n;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public int pc;

            public expandPower@49-1(int n, int k, IEnumerator<int> @enum, int pc, Tuple<int, int> current)
            {
                this.n = n;
                this.k = k;
                this.@enum = @enum;
                this.pc = pc;
                this.current = current;
            }

            public override void Close()
            {
                Exception exception = null;
                while (true)
                {
                    Unit unit;
                    switch (this.pc)
                    {
                        case 3:
                            goto Label_007E;
                    }
                    try
                    {
                        switch (this.pc)
                        {
                            case 0:
                            case 3:
                                break;

                            default:
                                this.pc = 3;
                                LanguagePrimitives.IntrinsicFunctions.Dispose<IEnumerator<int>>(this.@enum);
                                break;
                        }
                        this.pc = 3;
                        this.current = null;
                        unit = null;
                    }
                    catch (object obj1)
                    {
                        exception = (Exception) obj1;
                        unit = null;
                    }
                }
            Label_007E:
                if (exception > null)
                {
                    throw exception;
                }
            }

            public override int GenerateNext(ref IEnumerable<Tuple<int, int>> next)
            {
                switch (this.pc)
                {
                    case 1:
                        goto Label_009E;

                    case 2:
                        this.k = 0;
                        break;

                    case 3:
                        goto Label_00BF;

                    default:
                        this.@enum = Microsoft.FSharp.Core.Operators.OperatorIntrinsics.RangeInt32(0, 1, this.n).GetEnumerator();
                        this.pc = 1;
                        break;
                }
                if (this.@enum.MoveNext())
                {
                    this.k = this.@enum.Current;
                    this.pc = 2;
                    this.current = new Tuple<int, int>(this.k, (int) SpecialFunctions.Binomial(this.n, this.k));
                    return 1;
                }
            Label_009E:
                this.pc = 3;
                LanguagePrimitives.IntrinsicFunctions.Dispose<IEnumerator<int>>(this.@enum);
                this.@enum = null;
                this.pc = 3;
            Label_00BF:
                this.current = null;
                return 0;
            }

            public override bool CheckClose
            {
                switch (this.pc)
                {
                    case 0:
                    case 3:
                        return false;

                    case 1:
                        return true;
                }
                return true;
            }

            [CompilerGenerated]
            public override Tuple<int, int> LastGenerated => 
                this.current;

            [CompilerGenerated]
            public override IEnumerator<Tuple<int, int>> GetFreshEnumerator() => 
                new Algebraic.expandPower@49-1(this.n, 0, null, 0, null);
        }

        [Serializable]
        internal class expandPower@50 : FSharpFunc<Tuple<int, int>, Expression>
        {
            public Expression a;
            public FSharpList<Expression> ax;
            public int n;

            internal expandPower@50(int n, FSharpList<Expression> ax, Expression a)
            {
                this.n = n;
                this.ax = ax;
                this.a = a;
            }

            public override Expression Invoke(Tuple<int, int> tupledArg)
            {
                int y = tupledArg.Item1;
                int func = tupledArg.Item2;
                Expression expression = MathNet.Symbolics.Operators.pow(this.a, MathNet.Symbolics.Operators.number.Invoke(this.n - y));
                return Algebraic.expandProduct(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(func), expression), Algebraic.expandPower(Expression.NewSum(this.ax), y));
            }
        }

        [Serializable]
        internal class expandProduct@41 : FSharpFunc<Expression, Expression>
        {
            public Expression b;

            internal expandProduct@41(Expression b)
            {
                this.b = b;
            }

            public override Expression Invoke(Expression y) => 
                Algebraic.expandProduct(this.b, y);
        }

        [Serializable]
        internal class SeparateFactors@35 : FSharpFunc<Expression, bool>
        {
            public Expression symbol;

            internal SeparateFactors@35(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override bool Invoke(Expression x) => 
                Structure.IsFreeOf(this.symbol, x);
        }
    }
}

