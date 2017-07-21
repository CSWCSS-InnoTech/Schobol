namespace MathNet.Symbolics
{
    using <StartupCode$MathNet-Symbolics>;
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;
    using System.Globalization;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    [CompilationMapping(SourceConstructFlags.Module)]
    internal static class InfixFormatter
    {
        internal static Expression denominator(Expression _arg1)
        {
            FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|NegPower|_|(_arg1);
            if (option != null)
            {
                Expression x = option.Value.Item1;
                Expression y = option.Value.Item2;
                return MathNet.Symbolics.Operators.pow(x, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, y));
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                FSharpList<Expression> item = product.item;
                return MathNet.Symbolics.Operators.product(ListModule.Map<Expression, Expression>(new denominator@210(), item));
            }
            return MathNet.Symbolics.Operators.one;
        }

        internal static string functionName(MathNet.Symbolics.Function _arg1)
        {
            switch (_arg1.Tag)
            {
                case 1:
                    return "ln";

                case 2:
                    return "exp";

                case 3:
                    return "sin";

                case 4:
                    return "cos";

                case 5:
                    return "tan";

                case 6:
                    return "cot";

                case 7:
                    return "sec";

                case 8:
                    return "csc";

                case 9:
                    return "cosh";

                case 10:
                    return "sinh";

                case 11:
                    return "tanh";

                case 12:
                    return "asin";

                case 13:
                    return "acos";

                case 14:
                    return "atan";
            }
            return "abs";
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 })]
        internal static void nice(FSharpFunc<string, Unit> write, int priority, Expression _arg3)
        {
            Expression expression;
            Expression expression2;
            MathNet.Symbolics.Function function2;
            FSharpList<Expression> list;
            FSharpList<Expression> list2;
            Expression.Sum sum;
            Expression.Product product;
            BigRational item;
        Label_0000:
            switch (_arg3.Tag)
            {
                case 0:
                    item = ((Expression.Number) _arg3).item;
                    if ((item.IsInteger || (priority <= 1)) ? ((item.IsInteger && (priority > 0)) && (item.Sign < 0)) : true)
                    {
                        write.Invoke("(");
                    }
                    write.Invoke(item.ToString());
                    if ((item.IsInteger || (priority <= 1)) ? ((item.IsInteger && (priority > 0)) && (item.Sign < 0)) : true)
                    {
                        write.Invoke(")");
                        return;
                    }
                    return;

                case 1:
                {
                    Expression.Approximation approximation = (Expression.Approximation) _arg3;
                    if (!(approximation.item is MathNet.Symbolics.Approximation.Complex))
                    {
                        double num = ((MathNet.Symbolics.Approximation.Real) approximation.item).item;
                        if (num >= 0.0)
                        {
                            write.Invoke(num.ToString((IFormatProvider) culture));
                            return;
                        }
                        if (priority > 0)
                        {
                            write.Invoke("(");
                        }
                        write.Invoke(num.ToString((IFormatProvider) culture));
                        if (priority > 0)
                        {
                            write.Invoke(")");
                            return;
                        }
                        return;
                    }
                    System.Numerics.Complex complex = ((MathNet.Symbolics.Approximation.Complex) approximation.item).item;
                    write.Invoke("(");
                    write.Invoke(complex.ToString((IFormatProvider) culture));
                    write.Invoke(")");
                    return;
                }
                case 2:
                {
                    Expression.Identifier identifier = (Expression.Identifier) _arg3;
                    string func = identifier.item.item;
                    write.Invoke(func);
                    return;
                }
                case 3:
                {
                    Expression.Constant constant = (Expression.Constant) _arg3;
                    switch (constant.item.Tag)
                    {
                        case 1:
                            write.Invoke("π");
                            return;

                        case 2:
                            write.Invoke("j");
                            return;
                    }
                    write.Invoke("e");
                    return;
                }
                case 4:
                    sum = (Expression.Sum) _arg3;
                    if (sum.item.TailOrNull == null)
                    {
                        break;
                    }
                    list = sum.item;
                    list2 = list.TailOrNull;
                    expression = list.HeadOrDefault;
                    if (priority > 1)
                    {
                        write.Invoke("(");
                    }
                    niceSummand(write, true, expression);
                    ListModule.Iterate<Expression>(new nice@262-1(write), list2);
                    if (priority > 1)
                    {
                        write.Invoke(")");
                    }
                    return;

                case 5:
                {
                    product = (Expression.Product) _arg3;
                    if (product.item.TailOrNull == null)
                    {
                        break;
                    }
                    list = product.item;
                    if (list.HeadOrDefault.Tag != 0)
                    {
                        break;
                    }
                    Expression.Number number = (Expression.Number) list.HeadOrDefault;
                    if (!number.item.IsNegative)
                    {
                        break;
                    }
                    list2 = list.TailOrNull;
                    item = number.item;
                    write.Invoke("-");
                    _arg3 = MathNet.Symbolics.Operators.product(FSharpList<Expression>.Cons(Expression.NewNumber(-item), list2));
                    priority = 2;
                    write = write;
                    goto Label_0000;
                }
                case 9:
                    write.Invoke("⧝");
                    return;

                case 10:
                    write.Invoke("∞");
                    return;

                case 11:
                    if (priority > 0)
                    {
                        write.Invoke("(");
                    }
                    write.Invoke("-∞");
                    if (priority > 0)
                    {
                        write.Invoke(")");
                        return;
                    }
                    return;

                case 12:
                    write.Invoke("Undefined");
                    return;
            }
            if (_arg3.Tag == 5)
            {
                expression = _arg3;
                expression2 = numerator(expression);
                Expression expression3 = denominator(expression);
                if (MathNet.Symbolics.Operators.isOne(expression3))
                {
                    if (priority > 2)
                    {
                        write.Invoke("(");
                    }
                    niceFractionPart(write, 2, expression2);
                    if (priority > 2)
                    {
                        write.Invoke(")");
                        return;
                    }
                    return;
                }
                if (priority > 2)
                {
                    write.Invoke("(");
                }
                niceFractionPart(write, 3, expression2);
                write.Invoke("/");
                niceFractionPart(write, 3, expression3);
                if (priority > 2)
                {
                    write.Invoke(")");
                    return;
                }
                return;
            }
            FSharpOption<Tuple<Expression, Expression>> option = ExpressionPatterns.|NegIntPower|_|(_arg3);
            if (option != null)
            {
                expression = option.Value.Item1;
                expression2 = option.Value.Item2;
                if (priority > 2)
                {
                    write.Invoke("(");
                }
                write.Invoke("1/");
                nice(write, 3, expression);
                if (!expression2.Equals(MathNet.Symbolics.Operators.minusOne, LanguagePrimitives.GenericEqualityComparer))
                {
                    write.Invoke("^");
                    nice(write, 3, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression2));
                }
                if (priority > 2)
                {
                    write.Invoke(")");
                    return;
                }
                return;
            }
            switch (_arg3.Tag)
            {
                case 4:
                    sum = (Expression.Sum) _arg3;
                    if (sum.item.TailOrNull != null)
                    {
                        break;
                    }
                    goto Label_02B1;

                case 5:
                    product = (Expression.Product) _arg3;
                    if (product.item.TailOrNull != null)
                    {
                        break;
                    }
                    goto Label_02B1;

                case 6:
                {
                    Expression.Power power = (Expression.Power) _arg3;
                    expression = power.item1;
                    expression2 = power.item2;
                    if (priority > 3)
                    {
                        write.Invoke("(");
                    }
                    nice(write, 4, expression);
                    write.Invoke("^");
                    nice(write, 4, expression2);
                    if (priority > 3)
                    {
                        write.Invoke(")");
                        return;
                    }
                    return;
                }
                case 7:
                {
                    Expression.Function function = (Expression.Function) _arg3;
                    if (function.item1.Tag != 0)
                    {
                        expression = function.item2;
                        function2 = function.item1;
                        write.Invoke(functionName(function2));
                        write.Invoke("(");
                        nice(write, 0, expression);
                        write.Invoke(")");
                        return;
                    }
                    expression = function.item2;
                    write.Invoke("|");
                    nice(write, 0, expression);
                    write.Invoke("|");
                    return;
                }
                case 8:
                {
                    Expression.FunctionN nn = (Expression.FunctionN) _arg3;
                    if (nn.item2.TailOrNull != null)
                    {
                        list = nn.item2;
                        list2 = list.TailOrNull;
                        expression = list.HeadOrDefault;
                        function2 = nn.item1;
                        write.Invoke(functionName(function2));
                        write.Invoke("(");
                        nice(write, 0, expression);
                        ListModule.Iterate<Expression>(new nice@307(write), list2);
                        write.Invoke(")");
                        return;
                    }
                    goto Label_02B1;
                }
            }
            throw new MatchFailureException(@"D:\Dev\Math.NET\mathnet-symbolics\src\Symbolics\Convert\Infix.fs", 0xe9, 30);
        Label_02B1:
            throw new Exception("invalid expression");
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 })]
        internal static void niceFractionPart(FSharpFunc<string, Unit> write, int priority, Expression _arg1)
        {
            Expression expression;
            if (_arg1.Tag != 5)
            {
                expression = _arg1;
            }
            else
            {
                Expression.Product product = (Expression.Product) _arg1;
                if (product.item.TailOrNull != null)
                {
                    FSharpList<Expression> item = product.item;
                    FSharpList<Expression> list = item.TailOrNull;
                    expression = item.HeadOrDefault;
                    if (priority > 2)
                    {
                        write.Invoke("(");
                    }
                    nice(write, 2, expression);
                    ListModule.Iterate<Expression>(new niceFractionPart@217(write), list);
                    if (priority > 2)
                    {
                        write.Invoke(")");
                        return;
                    }
                    return;
                }
                expression = _arg1;
            }
            nice(write, priority, expression);
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 })]
        internal static void niceSummand(FSharpFunc<string, Unit> write, bool first, Expression _arg2)
        {
            if (_arg2.Tag == 0)
            {
                Expression.Number number = (Expression.Number) _arg2;
                if (number.item.IsNegative)
                {
                    Expression y = _arg2;
                    BigRational item = number.item;
                    write.Invoke("-");
                    nice(write, 1, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, y));
                    return;
                }
            }
            niceSummand$cont@220(write, first, _arg2, null);
        }

        [CompilerGenerated]
        internal static void niceSummand$cont@220(FSharpFunc<string, Unit> write, bool first, Expression _arg2, Unit unitVar)
        {
            if (_arg2.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg2;
                if (product.item.TailOrNull != null)
                {
                    FSharpList<Expression> item = product.item;
                    if (item.HeadOrDefault.Tag == 0)
                    {
                        Expression.Number number = (Expression.Number) item.HeadOrDefault;
                        if (number.item.IsNegative)
                        {
                            FSharpList<Expression> tail = item.TailOrNull;
                            BigRational rational = number.item;
                            if (first)
                            {
                                write.Invoke("-");
                                nice(write, 2, MathNet.Symbolics.Operators.product(FSharpList<Expression>.Cons(Expression.NewNumber(-rational), tail)));
                                return;
                            }
                            write.Invoke(" - ");
                            nice(write, 2, MathNet.Symbolics.Operators.product(FSharpList<Expression>.Cons(Expression.NewNumber(-rational), tail)));
                            return;
                        }
                    }
                }
            }
            if (_arg2.Tag == 5)
            {
                if (first)
                {
                    nice(write, 1, _arg2);
                }
                else
                {
                    write.Invoke(" + ");
                    nice(write, 1, _arg2);
                }
            }
            else if (first)
            {
                nice(write, 1, _arg2);
            }
            else
            {
                write.Invoke(" + ");
                nice(write, 1, _arg2);
            }
        }

        internal static Expression numerator(Expression _arg1)
        {
            if (ExpressionPatterns.|NegPower|_|(_arg1) != null)
            {
                return MathNet.Symbolics.Operators.one;
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                FSharpList<Expression> item = product.item;
                return MathNet.Symbolics.Operators.product(ListModule.Map<Expression, Expression>(new numerator@206(), item));
            }
            return _arg1;
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 })]
        internal static void strict(FSharpFunc<string, Unit> write, int priority, Expression _arg1)
        {
            FSharpList<Expression> list;
            FSharpList<Expression> list2;
            Expression expression;
            MathNet.Symbolics.Function function2;
            switch (_arg1.Tag)
            {
                case 1:
                {
                    Expression.Approximation approximation = (Expression.Approximation) _arg1;
                    if (!(approximation.item is MathNet.Symbolics.Approximation.Complex))
                    {
                        double num = ((MathNet.Symbolics.Approximation.Real) approximation.item).item;
                        if (num >= 0.0)
                        {
                            write.Invoke(num.ToString((IFormatProvider) culture));
                            return;
                        }
                        if (priority > 0)
                        {
                            write.Invoke("(");
                        }
                        write.Invoke(num.ToString((IFormatProvider) culture));
                        if (priority > 0)
                        {
                            write.Invoke(")");
                            return;
                        }
                        return;
                    }
                    System.Numerics.Complex item = ((MathNet.Symbolics.Approximation.Complex) approximation.item).item;
                    write.Invoke("(");
                    write.Invoke(item.ToString((IFormatProvider) culture));
                    write.Invoke(")");
                    return;
                }
                case 2:
                {
                    Expression.Identifier identifier = (Expression.Identifier) _arg1;
                    string func = identifier.item.item;
                    write.Invoke(func);
                    return;
                }
                case 3:
                {
                    Expression.Constant constant = (Expression.Constant) _arg1;
                    switch (constant.item.Tag)
                    {
                        case 1:
                            write.Invoke("pi");
                            return;

                        case 2:
                            write.Invoke("j");
                            return;
                    }
                    write.Invoke("e");
                    return;
                }
                case 4:
                {
                    Expression.Sum sum = (Expression.Sum) _arg1;
                    if (sum.item.TailOrNull != null)
                    {
                        list = sum.item;
                        list2 = list.TailOrNull;
                        expression = list.HeadOrDefault;
                        if (priority > 1)
                        {
                            write.Invoke("(");
                        }
                        strict(write, 1, expression);
                        ListModule.Iterate<Expression>(new strict@171(write), list2);
                        if (priority > 1)
                        {
                            write.Invoke(")");
                            return;
                        }
                        return;
                    }
                    break;
                }
                case 5:
                {
                    Expression.Product product = (Expression.Product) _arg1;
                    if (product.item.TailOrNull != null)
                    {
                        list = product.item;
                        list2 = list.TailOrNull;
                        expression = list.HeadOrDefault;
                        if (priority > 2)
                        {
                            write.Invoke("(");
                        }
                        strict(write, 2, expression);
                        ListModule.Iterate<Expression>(new strict@176-1(write), list2);
                        if (priority > 2)
                        {
                            write.Invoke(")");
                            return;
                        }
                        return;
                    }
                    break;
                }
                case 6:
                {
                    Expression.Power power = (Expression.Power) _arg1;
                    expression = power.item1;
                    Expression expression2 = power.item2;
                    if (priority > 2)
                    {
                        write.Invoke("(");
                    }
                    strict(write, 3, expression);
                    write.Invoke("^");
                    strict(write, 3, expression2);
                    if (priority > 2)
                    {
                        write.Invoke(")");
                        return;
                    }
                    return;
                }
                case 7:
                {
                    Expression.Function function = (Expression.Function) _arg1;
                    if (function.item1.Tag != 0)
                    {
                        expression = function.item2;
                        function2 = function.item1;
                        write.Invoke(functionName(function2));
                        write.Invoke("(");
                        strict(write, 0, expression);
                        write.Invoke(")");
                        return;
                    }
                    expression = function.item2;
                    write.Invoke("|");
                    strict(write, 0, expression);
                    write.Invoke("|");
                    return;
                }
                case 8:
                {
                    Expression.FunctionN nn = (Expression.FunctionN) _arg1;
                    if (nn.item2.TailOrNull != null)
                    {
                        list = nn.item2;
                        list2 = list.TailOrNull;
                        expression = list.HeadOrDefault;
                        function2 = nn.item1;
                        write.Invoke(functionName(function2));
                        write.Invoke("(");
                        strict(write, 0, expression);
                        ListModule.Iterate<Expression>(new strict@197-2(write), list2);
                        write.Invoke(")");
                        return;
                    }
                    break;
                }
                case 9:
                    write.Invoke("ComplexInfinity");
                    return;

                case 10:
                    write.Invoke("Infinity");
                    return;

                case 11:
                    if (priority > 0)
                    {
                        write.Invoke("(");
                    }
                    write.Invoke("-Infinity");
                    if (priority > 0)
                    {
                        write.Invoke(")");
                        return;
                    }
                    return;

                case 12:
                    write.Invoke("Undefined");
                    return;

                default:
                {
                    BigRational rational = ((Expression.Number) _arg1).item;
                    if ((rational.IsInteger || (priority <= 1)) ? ((rational.IsInteger && (priority > 0)) && (rational.Sign < 0)) : true)
                    {
                        write.Invoke("(");
                    }
                    write.Invoke(rational.ToString());
                    if ((rational.IsInteger || (priority <= 1)) ? ((rational.IsInteger && (priority > 0)) && (rational.Sign < 0)) : true)
                    {
                        write.Invoke(")");
                        return;
                    }
                    return;
                }
            }
            throw new Exception("invalid expression");
        }

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static CultureInfo culture =>
            $Infix.culture@127;

        [Serializable]
        internal class denominator@210 : FSharpFunc<Expression, Expression>
        {
            internal denominator@210()
            {
            }

            public override Expression Invoke(Expression _arg1) => 
                InfixFormatter.denominator(_arg1);
        }

        [Serializable]
        internal class nice@262-1 : FSharpFunc<Expression, Unit>
        {
            public FSharpFunc<string, Unit> write;

            internal nice@262-1(FSharpFunc<string, Unit> write)
            {
                this.write = write;
            }

            public override Unit Invoke(Expression _arg2)
            {
                InfixFormatter.niceSummand(this.write, false, _arg2);
                return null;
            }
        }

        [Serializable]
        internal class nice@307 : FSharpFunc<Expression, Unit>
        {
            public FSharpFunc<string, Unit> write;

            internal nice@307(FSharpFunc<string, Unit> write)
            {
                this.write = write;
            }

            public override Unit Invoke(Expression x)
            {
                this.write.Invoke(",");
                InfixFormatter.nice(this.write, 0, x);
                return null;
            }
        }

        [Serializable]
        internal class niceFractionPart@217 : FSharpFunc<Expression, Unit>
        {
            public FSharpFunc<string, Unit> write;

            internal niceFractionPart@217(FSharpFunc<string, Unit> write)
            {
                this.write = write;
            }

            public override Unit Invoke(Expression x)
            {
                this.write.Invoke("*");
                InfixFormatter.nice(this.write, 2, x);
                return null;
            }
        }

        [Serializable]
        internal class numerator@206 : FSharpFunc<Expression, Expression>
        {
            internal numerator@206()
            {
            }

            public override Expression Invoke(Expression _arg1) => 
                InfixFormatter.numerator(_arg1);
        }

        [Serializable]
        internal class strict@171 : FSharpFunc<Expression, Unit>
        {
            public FSharpFunc<string, Unit> write;

            internal strict@171(FSharpFunc<string, Unit> write)
            {
                this.write = write;
            }

            public override Unit Invoke(Expression x)
            {
                this.write.Invoke(" + ");
                InfixFormatter.strict(this.write, 1, x);
                return null;
            }
        }

        [Serializable]
        internal class strict@176-1 : FSharpFunc<Expression, Unit>
        {
            public FSharpFunc<string, Unit> write;

            internal strict@176-1(FSharpFunc<string, Unit> write)
            {
                this.write = write;
            }

            public override Unit Invoke(Expression x)
            {
                this.write.Invoke("*");
                InfixFormatter.strict(this.write, 2, x);
                return null;
            }
        }

        [Serializable]
        internal class strict@197-2 : FSharpFunc<Expression, Unit>
        {
            public FSharpFunc<string, Unit> write;

            internal strict@197-2(FSharpFunc<string, Unit> write)
            {
                this.write = write;
            }

            public override Unit Invoke(Expression x)
            {
                this.write.Invoke(",");
                InfixFormatter.strict(this.write, 0, x);
                return null;
            }
        }
    }
}

