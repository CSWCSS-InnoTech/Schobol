namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using Microsoft.FSharp.Core.CompilerServices;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [RequireQualifiedAccess, CompilationMapping(SourceConstructFlags.Module)]
    public static class Trigonometric
    {
        [CompilationArgumentCounts(new int[] { 1, 1 })]
        internal static Expression binomial(int n, int k) => 
            MathNet.Symbolics.Operators.number.Invoke((int) SpecialFunctions.Binomial(n, k));

        [CompilationSourceName("contract")]
        public static Expression Contract(Expression x)
        {
            Expression expression2;
            Expression expression = Structure.Map(new Contract@100-1(), x);
            switch (expression.Tag)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    return expression;

                case 5:
                    expression2 = expression;
                    break;

                case 6:
                    expression2 = expression;
                    break;

                default:
                    return expression;
            }
            return rules@85-10(expression2);
        }

        [CompilationSourceName("expand")]
        public static Expression Expand(Expression x)
        {
            Expression expression = Structure.Map(new Expand@34-4(), x);
            if (expression.Tag == 7)
            {
                Expression.Function function = (Expression.Function) expression;
                switch (function.item1.Tag)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                        return expression;

                    case 3:
                        return rules@17-2(Algebraic.Expand(function.item2)).Item1;

                    case 4:
                        return rules@17-2(Algebraic.Expand(function.item2)).Item2;
                }
            }
            return expression;
        }

        internal static bool isSinCosPart@42(Expression _arg1)
        {
            while (true)
            {
                FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|PosIntPower|_|(_arg1);
                if (option == null)
                {
                    break;
                }
                Expression expression = option.Value.Item1;
                _arg1 = expression;
            }
            return (ExpressionPatterns.|SinCos|_|(_arg1) > null);
        }

        internal static int oneIfEven(int k)
        {
            if (k.IsEven())
            {
                return 1;
            }
            return -1;
        }

        [CompilerGenerated]
        internal static Expression powerRules$cont@54-1(Expression r, Expression p, Unit unitVar)
        {
            if (r.Tag == 7)
            {
                Expression.Function function = (Expression.Function) r;
                if ((function.item1.Tag == 4) && (p.Tag == 0))
                {
                    Expression.Number number = (Expression.Number) p;
                    BigRational item = number.item;
                    if (item.IsInteger && item.IsPositive)
                    {
                        Expression expression3;
                        Expression expression4;
                        int num2;
                        Expression x = function.item2;
                        Expression expression2 = p;
                        item = number.item;
                        int num = BigRational.ToInt32(item);
                        if (num.IsEven())
                        {
                            expression4 = Expression.NewNumber(BigRational.FromInt(2));
                            num2 = 1 - num;
                            expression3 = MathNet.Symbolics.Operators.pow(expression4, MathNet.Symbolics.Operators.number.Invoke(num2));
                            expression4 = MathNet.Symbolics.Operators.sum(SeqModule.ToList<Expression>(new z@68-1(x, num, expression3, 0, null, 0, null)));
                            num2 = num / 2;
                            Expression expression5 = MathNet.Symbolics.Operators.number.Invoke((int) SpecialFunctions.Binomial(num, num2));
                            Expression expression6 = MathNet.Symbolics.Operators.pow(Expression.NewNumber(BigRational.FromInt(2)), MathNet.Symbolics.Operators.number.Invoke(num));
                            return MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.multiply(expression5, MathNet.Symbolics.Operators.invert(expression6)), expression4);
                        }
                        expression4 = Expression.NewNumber(BigRational.FromInt(2));
                        num2 = 1 - num;
                        expression3 = MathNet.Symbolics.Operators.pow(expression4, MathNet.Symbolics.Operators.number.Invoke(num2));
                        return MathNet.Symbolics.Operators.sum(SeqModule.ToList<Expression>(new powerRules@72-2(x, num, expression3, 0, null, 0, null)));
                    }
                }
            }
            return MathNet.Symbolics.Operators.pow(r, p);
        }

        [CompilerGenerated]
        internal static Expression powerRules$cont@56(Expression x, BigRational n, Unit unitVar)
        {
            Expression expression;
            int num2;
            int num3;
            Expression expression2;
            Expression expression3;
            int number = BigRational.ToInt32(n);
            if (number.IsEven())
            {
                num3 = number / 2;
                num2 = !num3.IsEven() ? -1 : 1;
                expression3 = Expression.NewNumber(BigRational.FromInt(2));
                num3 = 1 - number;
                expression2 = MathNet.Symbolics.Operators.pow(expression3, MathNet.Symbolics.Operators.number.Invoke(num3));
                expression = MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(num2), expression2);
                expression2 = MathNet.Symbolics.Operators.sum(SeqModule.ToList<Expression>(new z@59(x, number, expression, 0, null, 0, null)));
                num2 = !number.IsEven() ? -1 : 1;
                num3 = number / 2;
                Expression y = MathNet.Symbolics.Operators.number.Invoke((int) SpecialFunctions.Binomial(number, num3));
                expression3 = MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(num2), y);
                y = MathNet.Symbolics.Operators.pow(Expression.NewNumber(BigRational.FromInt(2)), MathNet.Symbolics.Operators.number.Invoke(number));
                return MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.multiply(expression3, MathNet.Symbolics.Operators.invert(y)), expression2);
            }
            num3 = (number - 1) / 2;
            num2 = !num3.IsEven() ? -1 : 1;
            expression3 = Expression.NewNumber(BigRational.FromInt(2));
            num3 = 1 - number;
            expression2 = MathNet.Symbolics.Operators.pow(expression3, MathNet.Symbolics.Operators.number.Invoke(num3));
            expression = MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(num2), expression2);
            return MathNet.Symbolics.Operators.sum(SeqModule.ToList<Expression>(new powerRules@63-1(x, number, expression, 0, null, 0, null)));
        }

        internal static Expression powerRules@53(Expression r, Expression p)
        {
            if (r.Tag == 7)
            {
                Expression.Function function = (Expression.Function) r;
                if ((function.item1.Tag == 3) && (p.Tag == 0))
                {
                    Expression.Number number = (Expression.Number) p;
                    BigRational item = number.item;
                    if (item.IsInteger && item.IsPositive)
                    {
                        Expression x = function.item2;
                        Expression expression2 = p;
                        item = number.item;
                        return powerRules$cont@56(x, item, null);
                    }
                }
            }
            return powerRules$cont@54-1(r, p, null);
        }

        [CompilerGenerated]
        internal static Expression productRules$cont@76(Expression v, Expression u, Unit unitVar)
        {
            Expression.Power power;
            Expression expression;
            Expression expression2;
            Expression expression3;
            switch (u.Tag)
            {
                case 6:
                    power = (Expression.Power) u;
                    expression3 = v;
                    expression2 = power.item2;
                    expression = power.item1;
                    goto Label_005F;

                case 7:
                {
                    Expression.Function function = (Expression.Function) u;
                    switch (v.Tag)
                    {
                        case 6:
                            power = (Expression.Power) v;
                            expression3 = u;
                            expression2 = power.item2;
                            expression = power.item1;
                            goto Label_005F;

                        case 7:
                        {
                            Expression expression4;
                            Expression.Function function2 = (Expression.Function) v;
                            switch (function2.item1.Tag)
                            {
                                case 3:
                                    Expression expression5;
                                    Expression expression6;
                                    switch (function.item1.Tag)
                                    {
                                        case 3:
                                            expression = function2.item2;
                                            expression2 = function.item2;
                                            expression4 = MathNet.Symbolics.Operators.cos(MathNet.Symbolics.Operators.add(expression2, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression)));
                                            expression5 = MathNet.Symbolics.Operators.number.Invoke(2);
                                            expression3 = MathNet.Symbolics.Operators.multiply(expression4, MathNet.Symbolics.Operators.invert(expression5));
                                            expression5 = MathNet.Symbolics.Operators.cos(MathNet.Symbolics.Operators.add(expression2, expression));
                                            expression6 = MathNet.Symbolics.Operators.number.Invoke(2);
                                            expression4 = MathNet.Symbolics.Operators.multiply(expression5, MathNet.Symbolics.Operators.invert(expression6));
                                            return MathNet.Symbolics.Operators.add(expression3, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression4));

                                        case 4:
                                            expression = function2.item2;
                                            expression2 = function.item2;
                                            expression4 = MathNet.Symbolics.Operators.sin(MathNet.Symbolics.Operators.add(expression2, expression));
                                            expression5 = MathNet.Symbolics.Operators.number.Invoke(2);
                                            expression3 = MathNet.Symbolics.Operators.multiply(expression4, MathNet.Symbolics.Operators.invert(expression5));
                                            expression5 = MathNet.Symbolics.Operators.sin(MathNet.Symbolics.Operators.add(expression, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression2)));
                                            expression6 = MathNet.Symbolics.Operators.number.Invoke(2);
                                            expression4 = MathNet.Symbolics.Operators.multiply(expression5, MathNet.Symbolics.Operators.invert(expression6));
                                            return MathNet.Symbolics.Operators.add(expression3, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression4));
                                    }
                                    break;

                                case 4:
                                    switch (function.item1.Tag)
                                    {
                                        case 3:
                                            expression = function2.item2;
                                            expression2 = function.item2;
                                            expression3 = MathNet.Symbolics.Operators.sin(MathNet.Symbolics.Operators.add(expression2, expression));
                                            expression4 = MathNet.Symbolics.Operators.number.Invoke(2);
                                            expression3 = MathNet.Symbolics.Operators.sin(MathNet.Symbolics.Operators.add(expression2, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression)));
                                            expression4 = MathNet.Symbolics.Operators.number.Invoke(2);
                                            return MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.multiply(expression3, MathNet.Symbolics.Operators.invert(expression4)), MathNet.Symbolics.Operators.multiply(expression3, MathNet.Symbolics.Operators.invert(expression4)));

                                        case 4:
                                            expression = function2.item2;
                                            expression2 = function.item2;
                                            expression3 = MathNet.Symbolics.Operators.cos(MathNet.Symbolics.Operators.add(expression2, expression));
                                            expression4 = MathNet.Symbolics.Operators.number.Invoke(2);
                                            expression3 = MathNet.Symbolics.Operators.cos(MathNet.Symbolics.Operators.add(expression2, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression)));
                                            expression4 = MathNet.Symbolics.Operators.number.Invoke(2);
                                            return MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.multiply(expression3, MathNet.Symbolics.Operators.invert(expression4)), MathNet.Symbolics.Operators.multiply(expression3, MathNet.Symbolics.Operators.invert(expression4)));
                                    }
                                    break;
                            }
                            break;
                        }
                    }
                    goto Label_0073;
                }
            }
            if (v.Tag != 6)
            {
                goto Label_0073;
            }
            power = (Expression.Power) v;
            expression = power.item1;
            expression2 = power.item2;
            expression3 = u;
        Label_005F:
            return rules@85-10(MathNet.Symbolics.Operators.multiply(expression3, powerRules@53(expression, expression2)));
        Label_0073:
            throw new Exception("unexpected expression");
        }

        internal static Expression productRules@74(FSharpList<Expression> _arg1)
        {
            Expression expression;
            FSharpList<Expression> list3;
            if (_arg1.TailOrNull == null)
            {
                throw new Exception("algorithm error 2");
            }
            FSharpList<Expression> list = _arg1;
            if (list.TailOrNull.TailOrNull == null)
            {
                expression = list.HeadOrDefault;
                list3 = list.TailOrNull;
            }
            else
            {
                FSharpList<Expression> list2 = list.TailOrNull;
                if (list2.TailOrNull.TailOrNull == null)
                {
                    expression = list2.HeadOrDefault;
                    Expression u = list.HeadOrDefault;
                    return productRules$cont@76(expression, u, null);
                }
                list3 = list.TailOrNull;
                expression = list.HeadOrDefault;
            }
            return rules@85-10(MathNet.Symbolics.Operators.multiply(expression, productRules@74(list3)));
        }

        internal static Tuple<Expression, Expression> rules@17-2(Expression _arg1)
        {
            FSharpList<Expression> item;
            FSharpFunc<Expression, Tuple<Expression, Expression>> mapping = new rules@17-3();
            switch (_arg1.Tag)
            {
                case 4:
                    item = ((Expression.Sum) _arg1).item;
                    return ListModule.Reduce<Tuple<Expression, Expression>>(new rules@20-4(), ListModule.Map<Expression, Tuple<Expression, Expression>>(mapping, item));

                case 5:
                {
                    Expression.Product product = (Expression.Product) _arg1;
                    if (product.item.TailOrNull == null)
                    {
                        break;
                    }
                    item = product.item;
                    FSharpOption<BigRational> option = ExpressionPatterns.|Integer|_|(item.HeadOrDefault);
                    if ((option == null) || !option.Value.IsPositive)
                    {
                        break;
                    }
                    BigRational n = option.Value;
                    FSharpList<Expression> xs = item.TailOrNull;
                    int e = BigRational.ToInt32(n);
                    Expression expression = MathNet.Symbolics.Operators.product(xs);
                    Expression sint = MathNet.Symbolics.Operators.sin(expression);
                    Expression cost = MathNet.Symbolics.Operators.cos(expression);
                    return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.sum(ListModule.Map<Tuple<int, Expression>, Expression>(new rules@28-6(e, sint, cost), SeqModule.ToList<Tuple<int, Expression>>(new rules@27-7(e, 0, null, 0, null)))), MathNet.Symbolics.Operators.sum(ListModule.Map<Tuple<int, Expression>, Expression>(new rules@31-8(e, sint, cost), SeqModule.ToList<Tuple<int, Expression>>(new rules@30-9(e, 0, null, 0, null)))));
                }
            }
            return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.sin(_arg1), MathNet.Symbolics.Operators.cos(_arg1));
        }

        internal static Expression rules@85-10(Expression x)
        {
            Expression expression2;
            Expression expression3;
            Expression expression = Algebraic.ExpandMain(x);
            FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|SinCosPosIntPower|_|(expression);
            if (option != null)
            {
                expression2 = option.Value.Item1;
                expression3 = option.Value.Item2;
                return powerRules@53(expression2, expression3);
            }
            switch (expression.Tag)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    return expression;

                case 4:
                {
                    Expression.Sum sum = (Expression.Sum) expression;
                    return ListModule.Fold<Expression, Expression>(new rules@97-11(), MathNet.Symbolics.Operators.zero, sum.item);
                }
                case 5:
                {
                    Tuple<Expression, Expression> tuple = SeparateFactors(expression);
                    expression2 = tuple.Item2;
                    expression3 = tuple.Item1;
                    if (ExpressionPatterns.|One|_|(expression2) == null)
                    {
                        if (ExpressionPatterns.|SinCos|_|(expression2) != null)
                        {
                            return expression;
                        }
                        switch (expression2.Tag)
                        {
                            case 5:
                            {
                                Expression.Product product = (Expression.Product) expression2;
                                FSharpList<Expression> item = product.item;
                                return Algebraic.ExpandMain(MathNet.Symbolics.Operators.multiply(expression3, productRules@74(item)));
                            }
                            case 6:
                            {
                                Expression.Power power = (Expression.Power) expression2;
                                Expression r = power.item1;
                                Expression p = power.item2;
                                return Algebraic.ExpandMain(MathNet.Symbolics.Operators.multiply(expression3, powerRules@53(r, p)));
                            }
                        }
                        break;
                    }
                    return expression;
                }
                default:
                    return expression;
            }
            return MathNet.Symbolics.Operators.multiply(expression3, expression2);
        }

        [CompilationSourceName("separateFactors")]
        public static Tuple<Expression, Expression> SeparateFactors(Expression x)
        {
            FSharpFunc<Expression, bool> predicate = new isSinCosPart@42-1();
            if (x.Tag == 5)
            {
                Expression.Product product = (Expression.Product) x;
                FSharpList<Expression> item = product.item;
                Tuple<FSharpList<Expression>, FSharpList<Expression>> tuple = ListModule.Partition<Expression>(predicate, item);
                FSharpList<Expression> xs = tuple.Item1;
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.product(tuple.Item2), MathNet.Symbolics.Operators.product(xs));
            }
            if (isSinCosPart@42(x))
            {
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.one, x);
            }
            return new Tuple<Expression, Expression>(x, MathNet.Symbolics.Operators.one);
        }

        [CompilationSourceName("simplify")]
        public static Expression Simplify(Expression x)
        {
            Expression expression2 = Rational.Rationalize(Substitute(x));
            Expression expression3 = Contract(Expand(Rational.Numerator(expression2)));
            Expression expression4 = Contract(Expand(Rational.Denominator(expression2)));
            return MathNet.Symbolics.Operators.multiply(expression3, MathNet.Symbolics.Operators.invert(expression4));
        }

        [CompilationSourceName("substitute")]
        public static Expression Substitute(Expression x)
        {
            Expression expression;
            Expression expression2;
            FSharpList<Expression> item;
            switch (x.Tag)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 9:
                case 10:
                case 11:
                case 12:
                    return x;

                case 4:
                    item = ((Expression.Sum) x).item;
                    return MathNet.Symbolics.Operators.sum(ListModule.Map<Expression, Expression>(new Substitute@109-3(), item));

                case 5:
                    item = ((Expression.Product) x).item;
                    return MathNet.Symbolics.Operators.product(ListModule.Map<Expression, Expression>(new Substitute@110-4(), item));

                case 6:
                {
                    Expression.Power power = (Expression.Power) x;
                    expression = power.item1;
                    expression2 = power.item2;
                    return MathNet.Symbolics.Operators.pow(Substitute(expression), Substitute(expression2));
                }
                case 7:
                {
                    Expression.Function function = (Expression.Function) x;
                    if (function.item1.Tag != 5)
                    {
                        expression = function.item2;
                        return MathNet.Symbolics.Operators.apply(function.item1, Substitute(expression));
                    }
                    expression2 = Substitute(function.item2);
                    Expression expression3 = MathNet.Symbolics.Operators.sin(expression2);
                    Expression expression4 = MathNet.Symbolics.Operators.cos(expression2);
                    return MathNet.Symbolics.Operators.multiply(expression3, MathNet.Symbolics.Operators.invert(expression4));
                }
                case 8:
                {
                    Expression.FunctionN nn = (Expression.FunctionN) x;
                    item = nn.item2;
                    MathNet.Symbolics.Function function2 = nn.item1;
                    FSharpList<Expression> list2 = ListModule.Map<Expression, Expression>(new Substitute@113-5(), item);
                    throw new Exception("not supported yet");
                }
            }
            return x;
        }

        [Serializable]
        internal class Contract@100-1 : FSharpFunc<Expression, Expression>
        {
            internal Contract@100-1()
            {
            }

            public override Expression Invoke(Expression x) => 
                Trigonometric.Contract(x);
        }

        [Serializable]
        internal class Expand@34-4 : FSharpFunc<Expression, Expression>
        {
            internal Expand@34-4()
            {
            }

            public override Expression Invoke(Expression x) => 
                Trigonometric.Expand(x);
        }

        [Serializable]
        internal class isSinCosPart@42-1 : FSharpFunc<Expression, bool>
        {
            internal isSinCosPart@42-1()
            {
            }

            public override bool Invoke(Expression _arg1) => 
                Trigonometric.isSinCosPart@42(_arg1);
        }

        [Serializable, CompilationMapping(SourceConstructFlags.Closure)]
        internal sealed class powerRules@63-1 : GeneratedSequenceBase<Expression>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public Expression current;
            public int e;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public IEnumerator<int> @enum;
            public int j;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public int pc;
            public Expression w;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public Expression x;

            public powerRules@63-1(Expression x, int e, Expression w, int j, IEnumerator<int> @enum, int pc, Expression current)
            {
                this.x = x;
                this.e = e;
                this.w = w;
                this.j = j;
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

            public override int GenerateNext(ref IEnumerable<Expression> next)
            {
                switch (this.pc)
                {
                    case 1:
                        goto Label_0107;

                    case 2:
                        this.j = 0;
                        break;

                    case 3:
                        goto Label_0128;

                    default:
                        this.@enum = Microsoft.FSharp.Core.Operators.OperatorIntrinsics.RangeInt32(0, 1, this.e / 2).GetEnumerator();
                        this.pc = 1;
                        break;
                }
                if (this.@enum.MoveNext())
                {
                    this.j = this.@enum.Current;
                    this.pc = 2;
                    int func = !this.j.IsEven() ? -1 : 1;
                    func = this.e - (2 * this.j);
                    this.current = MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(func), this.w), MathNet.Symbolics.Operators.number.Invoke((int) SpecialFunctions.Binomial(this.e, this.j))), MathNet.Symbolics.Operators.sin(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(func), this.x)));
                    return 1;
                }
            Label_0107:
                this.pc = 3;
                LanguagePrimitives.IntrinsicFunctions.Dispose<IEnumerator<int>>(this.@enum);
                this.@enum = null;
                this.pc = 3;
            Label_0128:
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
            public override Expression LastGenerated => 
                this.current;

            [CompilerGenerated]
            public override IEnumerator<Expression> GetFreshEnumerator() => 
                new Trigonometric.powerRules@63-1(this.x, this.e, this.w, 0, null, 0, null);
        }

        [Serializable, CompilationMapping(SourceConstructFlags.Closure)]
        internal sealed class powerRules@72-2 : GeneratedSequenceBase<Expression>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public Expression current;
            public int e;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public IEnumerator<int> @enum;
            public int j;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public int pc;
            public Expression w;
            public Expression x;

            public powerRules@72-2(Expression x, int e, Expression w, int j, IEnumerator<int> @enum, int pc, Expression current)
            {
                this.x = x;
                this.e = e;
                this.w = w;
                this.j = j;
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

            public override int GenerateNext(ref IEnumerable<Expression> next)
            {
                switch (this.pc)
                {
                    case 1:
                        goto Label_00E3;

                    case 2:
                        this.j = 0;
                        break;

                    case 3:
                        goto Label_0104;

                    default:
                        this.@enum = Microsoft.FSharp.Core.Operators.OperatorIntrinsics.RangeInt32(0, 1, this.e / 2).GetEnumerator();
                        this.pc = 1;
                        break;
                }
                if (this.@enum.MoveNext())
                {
                    this.j = this.@enum.Current;
                    this.pc = 2;
                    int func = this.e - (2 * this.j);
                    this.current = MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(this.w, MathNet.Symbolics.Operators.number.Invoke((int) SpecialFunctions.Binomial(this.e, this.j))), MathNet.Symbolics.Operators.cos(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(func), this.x)));
                    return 1;
                }
            Label_00E3:
                this.pc = 3;
                LanguagePrimitives.IntrinsicFunctions.Dispose<IEnumerator<int>>(this.@enum);
                this.@enum = null;
                this.pc = 3;
            Label_0104:
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
            public override Expression LastGenerated => 
                this.current;

            [CompilerGenerated]
            public override IEnumerator<Expression> GetFreshEnumerator() => 
                new Trigonometric.powerRules@72-2(this.x, this.e, this.w, 0, null, 0, null);
        }

        [Serializable]
        internal class rules@17-3 : FSharpFunc<Expression, Tuple<Expression, Expression>>
        {
            internal rules@17-3()
            {
            }

            public override Tuple<Expression, Expression> Invoke(Expression _arg1) => 
                Trigonometric.rules@17-2(_arg1);
        }

        [Serializable]
        internal class rules@20-4 : FSharpFunc<Tuple<Expression, Expression>, FSharpFunc<Tuple<Expression, Expression>, Tuple<Expression, Expression>>>
        {
            internal rules@20-4()
            {
            }

            public override FSharpFunc<Tuple<Expression, Expression>, Tuple<Expression, Expression>> Invoke(Tuple<Expression, Expression> tupledArg)
            {
                Expression expression = tupledArg.Item1;
                return new Trigonometric.rules@20-5(expression, tupledArg.Item2);
            }
        }

        [Serializable]
        internal class rules@20-5 : FSharpFunc<Tuple<Expression, Expression>, Tuple<Expression, Expression>>
        {
            public Expression c1;
            public Expression s1;

            internal rules@20-5(Expression s1, Expression c1)
            {
                this.s1 = s1;
                this.c1 = c1;
            }

            public override Tuple<Expression, Expression> Invoke(Tuple<Expression, Expression> tupledArg)
            {
                Expression y = tupledArg.Item1;
                Expression expression2 = tupledArg.Item2;
                Expression x = MathNet.Symbolics.Operators.multiply(this.c1, expression2);
                Expression expression4 = MathNet.Symbolics.Operators.multiply(this.s1, y);
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.multiply(this.s1, expression2), MathNet.Symbolics.Operators.multiply(this.c1, y)), MathNet.Symbolics.Operators.add(x, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression4)));
            }
        }

        [Serializable, CompilationMapping(SourceConstructFlags.Closure)]
        internal sealed class rules@27-7 : GeneratedSequenceBase<Tuple<int, Expression>>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public Tuple<int, Expression> current;
            public int e;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public IEnumerator<int> @enum;
            public int k;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public int pc;

            public rules@27-7(int e, int k, IEnumerator<int> @enum, int pc, Tuple<int, Expression> current)
            {
                this.e = e;
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

            public override int GenerateNext(ref IEnumerable<Tuple<int, Expression>> next)
            {
                switch (this.pc)
                {
                    case 1:
                        goto Label_00DD;

                    case 2:
                        this.k = 0;
                        break;

                    case 3:
                        goto Label_00FE;

                    default:
                        this.@enum = Microsoft.FSharp.Core.Operators.OperatorIntrinsics.RangeInt32(1, 2, this.e).GetEnumerator();
                        this.pc = 1;
                        break;
                }
                if (this.@enum.MoveNext())
                {
                    this.k = this.@enum.Current;
                    this.pc = 2;
                    int number = (this.k - 1) / 2;
                    int func = !number.IsEven() ? -1 : 1;
                    Expression y = MathNet.Symbolics.Operators.number.Invoke((int) SpecialFunctions.Binomial(this.e, this.k));
                    this.current = new Tuple<int, Expression>(this.k, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(func), y));
                    return 1;
                }
            Label_00DD:
                this.pc = 3;
                LanguagePrimitives.IntrinsicFunctions.Dispose<IEnumerator<int>>(this.@enum);
                this.@enum = null;
                this.pc = 3;
            Label_00FE:
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
            public override Tuple<int, Expression> LastGenerated => 
                this.current;

            [CompilerGenerated]
            public override IEnumerator<Tuple<int, Expression>> GetFreshEnumerator() => 
                new Trigonometric.rules@27-7(this.e, 0, null, 0, null);
        }

        [Serializable]
        internal class rules@28-6 : FSharpFunc<Tuple<int, Expression>, Expression>
        {
            public Expression cost;
            public int e;
            public Expression sint;

            internal rules@28-6(int e, Expression sint, Expression cost)
            {
                this.e = e;
                this.sint = sint;
                this.cost = cost;
            }

            public override Expression Invoke(Tuple<int, Expression> tupledArg)
            {
                int func = tupledArg.Item1;
                Expression x = tupledArg.Item2;
                return MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(x, MathNet.Symbolics.Operators.pow(this.cost, MathNet.Symbolics.Operators.number.Invoke(this.e - func))), MathNet.Symbolics.Operators.pow(this.sint, MathNet.Symbolics.Operators.number.Invoke(func)));
            }
        }

        [Serializable, CompilationMapping(SourceConstructFlags.Closure)]
        internal sealed class rules@30-9 : GeneratedSequenceBase<Tuple<int, Expression>>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public Tuple<int, Expression> current;
            public int e;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public IEnumerator<int> @enum;
            public int k;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public int pc;

            public rules@30-9(int e, int k, IEnumerator<int> @enum, int pc, Tuple<int, Expression> current)
            {
                this.e = e;
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

            public override int GenerateNext(ref IEnumerable<Tuple<int, Expression>> next)
            {
                switch (this.pc)
                {
                    case 1:
                        goto Label_00DB;

                    case 2:
                        this.k = 0;
                        break;

                    case 3:
                        goto Label_00FC;

                    default:
                        this.@enum = Microsoft.FSharp.Core.Operators.OperatorIntrinsics.RangeInt32(0, 2, this.e).GetEnumerator();
                        this.pc = 1;
                        break;
                }
                if (this.@enum.MoveNext())
                {
                    this.k = this.@enum.Current;
                    this.pc = 2;
                    int number = this.k / 2;
                    int func = !number.IsEven() ? -1 : 1;
                    Expression y = MathNet.Symbolics.Operators.number.Invoke((int) SpecialFunctions.Binomial(this.e, this.k));
                    this.current = new Tuple<int, Expression>(this.k, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(func), y));
                    return 1;
                }
            Label_00DB:
                this.pc = 3;
                LanguagePrimitives.IntrinsicFunctions.Dispose<IEnumerator<int>>(this.@enum);
                this.@enum = null;
                this.pc = 3;
            Label_00FC:
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
            public override Tuple<int, Expression> LastGenerated => 
                this.current;

            [CompilerGenerated]
            public override IEnumerator<Tuple<int, Expression>> GetFreshEnumerator() => 
                new Trigonometric.rules@30-9(this.e, 0, null, 0, null);
        }

        [Serializable]
        internal class rules@31-8 : FSharpFunc<Tuple<int, Expression>, Expression>
        {
            public Expression cost;
            public int e;
            public Expression sint;

            internal rules@31-8(int e, Expression sint, Expression cost)
            {
                this.e = e;
                this.sint = sint;
                this.cost = cost;
            }

            public override Expression Invoke(Tuple<int, Expression> tupledArg)
            {
                int func = tupledArg.Item1;
                Expression x = tupledArg.Item2;
                return MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(x, MathNet.Symbolics.Operators.pow(this.cost, MathNet.Symbolics.Operators.number.Invoke(this.e - func))), MathNet.Symbolics.Operators.pow(this.sint, MathNet.Symbolics.Operators.number.Invoke(func)));
            }
        }

        [Serializable]
        internal class rules@97-11 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal rules@97-11()
            {
            }

            public override Expression Invoke(Expression s, Expression _arg2)
            {
                Expression expression;
                switch (_arg2.Tag)
                {
                    case 5:
                        expression = _arg2;
                        break;

                    case 6:
                        expression = _arg2;
                        break;

                    default:
                        return MathNet.Symbolics.Operators.add(s, _arg2);
                }
                return MathNet.Symbolics.Operators.add(s, Trigonometric.rules@85-10(expression));
            }
        }

        [Serializable]
        internal class Substitute@109-3 : FSharpFunc<Expression, Expression>
        {
            internal Substitute@109-3()
            {
            }

            public override Expression Invoke(Expression x) => 
                Trigonometric.Substitute(x);
        }

        [Serializable]
        internal class Substitute@110-4 : FSharpFunc<Expression, Expression>
        {
            internal Substitute@110-4()
            {
            }

            public override Expression Invoke(Expression x) => 
                Trigonometric.Substitute(x);
        }

        [Serializable]
        internal class Substitute@113-5 : FSharpFunc<Expression, Expression>
        {
            internal Substitute@113-5()
            {
            }

            public override Expression Invoke(Expression x) => 
                Trigonometric.Substitute(x);
        }

        [Serializable, CompilationMapping(SourceConstructFlags.Closure)]
        internal sealed class z@59 : GeneratedSequenceBase<Expression>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public Expression current;
            public int e;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public IEnumerator<int> @enum;
            public int j;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public int pc;
            public Expression w;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public Expression x;

            public z@59(Expression x, int e, Expression w, int j, IEnumerator<int> @enum, int pc, Expression current)
            {
                this.x = x;
                this.e = e;
                this.w = w;
                this.j = j;
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

            public override int GenerateNext(ref IEnumerable<Expression> next)
            {
                switch (this.pc)
                {
                    case 1:
                        goto Label_0109;

                    case 2:
                        this.j = 0;
                        break;

                    case 3:
                        goto Label_012A;

                    default:
                        this.@enum = Microsoft.FSharp.Core.Operators.OperatorIntrinsics.RangeInt32(0, 1, (this.e / 2) - 1).GetEnumerator();
                        this.pc = 1;
                        break;
                }
                if (this.@enum.MoveNext())
                {
                    this.j = this.@enum.Current;
                    this.pc = 2;
                    int func = !this.j.IsEven() ? -1 : 1;
                    func = this.e - (2 * this.j);
                    this.current = MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(func), this.w), MathNet.Symbolics.Operators.number.Invoke((int) SpecialFunctions.Binomial(this.e, this.j))), MathNet.Symbolics.Operators.cos(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(func), this.x)));
                    return 1;
                }
            Label_0109:
                this.pc = 3;
                LanguagePrimitives.IntrinsicFunctions.Dispose<IEnumerator<int>>(this.@enum);
                this.@enum = null;
                this.pc = 3;
            Label_012A:
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
            public override Expression LastGenerated => 
                this.current;

            [CompilerGenerated]
            public override IEnumerator<Expression> GetFreshEnumerator() => 
                new Trigonometric.z@59(this.x, this.e, this.w, 0, null, 0, null);
        }

        [Serializable, CompilationMapping(SourceConstructFlags.Closure)]
        internal sealed class z@68-1 : GeneratedSequenceBase<Expression>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public Expression current;
            public int e;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public IEnumerator<int> @enum;
            public int j;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public int pc;
            public Expression w;
            public Expression x;

            public z@68-1(Expression x, int e, Expression w, int j, IEnumerator<int> @enum, int pc, Expression current)
            {
                this.x = x;
                this.e = e;
                this.w = w;
                this.j = j;
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

            public override int GenerateNext(ref IEnumerable<Expression> next)
            {
                switch (this.pc)
                {
                    case 1:
                        goto Label_00E5;

                    case 2:
                        this.j = 0;
                        break;

                    case 3:
                        goto Label_0106;

                    default:
                        this.@enum = Microsoft.FSharp.Core.Operators.OperatorIntrinsics.RangeInt32(0, 1, (this.e / 2) - 1).GetEnumerator();
                        this.pc = 1;
                        break;
                }
                if (this.@enum.MoveNext())
                {
                    this.j = this.@enum.Current;
                    this.pc = 2;
                    int func = this.e - (2 * this.j);
                    this.current = MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(this.w, MathNet.Symbolics.Operators.number.Invoke((int) SpecialFunctions.Binomial(this.e, this.j))), MathNet.Symbolics.Operators.cos(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(func), this.x)));
                    return 1;
                }
            Label_00E5:
                this.pc = 3;
                LanguagePrimitives.IntrinsicFunctions.Dispose<IEnumerator<int>>(this.@enum);
                this.@enum = null;
                this.pc = 3;
            Label_0106:
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
            public override Expression LastGenerated => 
                this.current;

            [CompilerGenerated]
            public override IEnumerator<Expression> GetFreshEnumerator() => 
                new Trigonometric.z@68-1(this.x, this.e, this.w, 0, null, 0, null);
        }
    }
}

