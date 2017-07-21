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
    internal static class LaTeXFormatter
    {
        internal static string functionName(MathNet.Symbolics.Function _arg1)
        {
            switch (_arg1.Tag)
            {
                case 1:
                    return @"\ln";

                case 2:
                    return @"\exp";

                case 3:
                    return @"\sin";

                case 4:
                    return @"\cos";

                case 5:
                    return @"\tan";

                case 6:
                    return @"\cot";

                case 7:
                    return @"\sec";

                case 8:
                    return @"\csc";

                case 9:
                    return @"\cosh";

                case 10:
                    return @"\sinh";

                case 11:
                    return @"\tanh";

                case 12:
                    return @"\arcsin";

                case 13:
                    return @"\arccos";

                case 14:
                    return @"\arctan";
            }
            return @"\mathrm{abs}";
        }

        internal static bool nextNumber(Expression _arg1)
        {
            if (_arg1.Tag == 6)
            {
                Expression.Power power = (Expression.Power) _arg1;
                return (power.item1.Tag == 0);
            }
            return false;
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 })]
        internal static void tex(FSharpFunc<string, Unit> write, int priority, Expression _arg3)
        {
            Expression expression;
            Expression expression2;
            Expression expression3;
            MathNet.Symbolics.Function function2;
            FSharpList<Expression> list;
            FSharpList<Expression> list2;
            Expression.Sum sum;
            Expression.Product product;
            BigRational item;
            switch (_arg3.Tag)
            {
                case 0:
                    item = ((Expression.Number) _arg3).item;
                    if (!item.IsInteger)
                    {
                        if (priority > 2)
                        {
                            write.Invoke(@"\left(");
                        }
                        write.Invoke(@"\frac{");
                        write.Invoke(item.Numerator.ToString());
                        write.Invoke("}{");
                        write.Invoke(item.Denominator.ToString());
                        write.Invoke("}");
                        if (priority > 2)
                        {
                            write.Invoke(@"\right)");
                            return;
                        }
                        return;
                    }
                    if (item.Sign < 0)
                    {
                        if (priority > 0)
                        {
                            write.Invoke(@"\left({");
                        }
                        write.Invoke(item.ToString());
                        if (priority > 0)
                        {
                            write.Invoke(@"}\right)");
                            return;
                        }
                        return;
                    }
                    write.Invoke(item.ToString());
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
                            write.Invoke(@"\left({");
                        }
                        write.Invoke(num.ToString((IFormatProvider) culture));
                        if (priority > 0)
                        {
                            write.Invoke(@"}\right)");
                            return;
                        }
                        return;
                    }
                    System.Numerics.Complex complex = ((MathNet.Symbolics.Approximation.Complex) approximation.item).item;
                    write.Invoke(@"\left({");
                    write.Invoke(complex.ToString((IFormatProvider) culture));
                    write.Invoke(@"}\right)");
                    return;
                }
                case 2:
                {
                    Expression.Identifier identifier = (Expression.Identifier) _arg3;
                    string str = identifier.item.item;
                    if (str.Length > 1)
                    {
                        write.Invoke("{");
                    }
                    write.Invoke(LaTeXHelper.addBracets(str));
                    if (str.Length > 1)
                    {
                        write.Invoke("}");
                        return;
                    }
                    return;
                }
                case 3:
                {
                    Expression.Constant constant = (Expression.Constant) _arg3;
                    switch (constant.item.Tag)
                    {
                        case 0:
                            write.Invoke("e");
                            return;

                        case 2:
                            write.Invoke(@"\jmath");
                            return;
                    }
                    write.Invoke(@"\pi");
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
                        write.Invoke(@"\left(");
                    }
                    texSummand(write, true, expression);
                    ListModule.Iterate<Expression>(new tex@115-1(write), list2);
                    if (priority > 1)
                    {
                        write.Invoke(@"\right)");
                        return;
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
                    if (priority > 1)
                    {
                        write.Invoke(@"\left(");
                    }
                    write.Invoke("-");
                    tex(write, 2, MathNet.Symbolics.Operators.product(FSharpList<Expression>.Cons(Expression.NewNumber(-item), list2)));
                    if (priority > 1)
                    {
                        write.Invoke(@"\right)");
                    }
                    return;
                }
                case 9:
                    write.Invoke(@"\infty");
                    return;

                case 10:
                    write.Invoke(@"\infty");
                    return;

                case 11:
                    if (priority > 0)
                    {
                        write.Invoke(@"\left({");
                    }
                    write.Invoke(@"-\infty");
                    if (priority > 0)
                    {
                        write.Invoke(@"}\right)");
                        return;
                    }
                    return;

                case 12:
                    write.Invoke(@"\mathrm{undefined}");
                    return;
            }
            if (_arg3.Tag == 5)
            {
                expression = _arg3;
                expression2 = InfixFormatter.numerator(expression);
                expression3 = InfixFormatter.denominator(expression);
                if (MathNet.Symbolics.Operators.isOne(expression3))
                {
                    if (priority > 2)
                    {
                        write.Invoke(@"\left(");
                    }
                    texFractionPart(write, 2, expression2);
                    if (priority > 2)
                    {
                        write.Invoke(@"\right)");
                        return;
                    }
                    return;
                }
                if (priority > 2)
                {
                    write.Invoke(@"\left(");
                }
                write.Invoke(@"\frac{");
                texFractionPart(write, 0, expression2);
                write.Invoke("}{");
                texFractionPart(write, 0, expression3);
                write.Invoke("}");
                if (priority > 2)
                {
                    write.Invoke(@"\right)");
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
                    write.Invoke(@"\left(");
                }
                write.Invoke(@"\frac{1}{");
                tex(write, 3, expression);
                if (!expression2.Equals(MathNet.Symbolics.Operators.minusOne, LanguagePrimitives.GenericEqualityComparer))
                {
                    write.Invoke("^");
                    tex(write, 3, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression2));
                }
                write.Invoke("}");
                if (priority > 2)
                {
                    write.Invoke(@"\right)");
                    return;
                }
                return;
            }
            if (_arg3.Tag == 6)
            {
                Expression.Power power = (Expression.Power) _arg3;
                if (power.item2.Tag == 6)
                {
                    Expression.Power power2 = (Expression.Power) power.item2;
                    if (power2.item2.Equals(MathNet.Symbolics.Operators.minusOne, LanguagePrimitives.GenericEqualityComparer))
                    {
                        expression = power.item1;
                        expression2 = power2.item1;
                        expression3 = power2.item2;
                        if (priority > 3)
                        {
                            write.Invoke(@"\left(");
                        }
                        write.Invoke(@"\sqrt[");
                        tex(write, 4, expression2);
                        write.Invoke("]{");
                        tex(write, 4, expression);
                        write.Invoke("}");
                        if (priority > 3)
                        {
                            write.Invoke(@"\right)");
                            return;
                        }
                        return;
                    }
                }
            }
            switch (_arg3.Tag)
            {
                case 4:
                    sum = (Expression.Sum) _arg3;
                    if (sum.item.TailOrNull != null)
                    {
                        break;
                    }
                    goto Label_0468;

                case 5:
                    product = (Expression.Product) _arg3;
                    if (product.item.TailOrNull != null)
                    {
                        break;
                    }
                    goto Label_0468;

                case 6:
                {
                    Expression.Power power3 = (Expression.Power) _arg3;
                    expression = power3.item1;
                    expression2 = power3.item2;
                    if (priority > 3)
                    {
                        write.Invoke(@"\left(");
                    }
                    write.Invoke("{");
                    tex(write, 4, expression);
                    write.Invoke("}");
                    write.Invoke("^");
                    write.Invoke("{");
                    tex(write, 4, expression2);
                    write.Invoke("}");
                    if (priority > 3)
                    {
                        write.Invoke(@"\right)");
                        return;
                    }
                    return;
                }
                case 7:
                {
                    Expression.Function function = (Expression.Function) _arg3;
                    switch (function.item1.Tag)
                    {
                        case 0:
                            expression = function.item2;
                            write.Invoke(@"\left|");
                            tex(write, 0, expression);
                            write.Invoke(@"\right|");
                            return;

                        case 2:
                            expression = function.item2;
                            if (priority > 3)
                            {
                                write.Invoke(@"\left(");
                            }
                            write.Invoke(@"\mathrm{e}^");
                            tex(write, 4, expression);
                            if (priority > 3)
                            {
                                write.Invoke(@"\right)");
                                return;
                            }
                            return;
                    }
                    expression = function.item2;
                    function2 = function.item1;
                    if (priority > 3)
                    {
                        write.Invoke(@"\left(");
                    }
                    write.Invoke(functionName(function2));
                    write.Invoke("{");
                    tex(write, 3, expression);
                    write.Invoke("}");
                    if (priority > 3)
                    {
                        write.Invoke(@"\right)");
                        return;
                    }
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
                        if (priority > 3)
                        {
                            write.Invoke(@"\left(");
                        }
                        write.Invoke(functionName(function2));
                        write.Invoke(@"{\left(");
                        tex(write, 0, expression);
                        ListModule.Iterate<Expression>(new tex@185(write), list2);
                        write.Invoke(@"\right)}");
                        if (priority > 3)
                        {
                            write.Invoke(@"\right)");
                            return;
                        }
                        return;
                    }
                    goto Label_0468;
                }
            }
            throw new MatchFailureException(@"D:\Dev\Math.NET\mathnet-symbolics\src\Symbolics\Convert\LaTeX.fs", 0x48, 0x1d);
        Label_0468:
            throw new Exception("invalid expression");
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 })]
        internal static void texFractionPart(FSharpFunc<string, Unit> write, int priority, Expression _arg1)
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
                        write.Invoke(@"\left(");
                    }
                    tex(write, 2, expression);
                    if ((expression.Tag == 0) && nextNumber(list.Head))
                    {
                        write.Invoke("*");
                    }
                    ListModule.Iterate<Expression>(new texFractionPart@56(write), list);
                    if (priority > 2)
                    {
                        write.Invoke(@"\right)");
                        return;
                    }
                    return;
                }
                expression = _arg1;
            }
            tex(write, priority, expression);
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 })]
        internal static void texSummand(FSharpFunc<string, Unit> write, bool first, Expression _arg2)
        {
            if (_arg2.Tag == 0)
            {
                Expression.Number number = (Expression.Number) _arg2;
                if (number.item.IsNegative)
                {
                    Expression y = _arg2;
                    BigRational item = number.item;
                    write.Invoke("-");
                    tex(write, 1, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, y));
                    return;
                }
            }
            texSummand$cont@59(write, first, _arg2, null);
        }

        [CompilerGenerated]
        internal static void texSummand$cont@59(FSharpFunc<string, Unit> write, bool first, Expression _arg2, Unit unitVar)
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
                                tex(write, 2, MathNet.Symbolics.Operators.product(FSharpList<Expression>.Cons(Expression.NewNumber(-rational), tail)));
                                return;
                            }
                            write.Invoke(" - ");
                            tex(write, 2, MathNet.Symbolics.Operators.product(FSharpList<Expression>.Cons(Expression.NewNumber(-rational), tail)));
                            return;
                        }
                    }
                }
            }
            if (_arg2.Tag == 5)
            {
                if (first)
                {
                    tex(write, 1, _arg2);
                }
                else
                {
                    write.Invoke(" + ");
                    tex(write, 1, _arg2);
                }
            }
            else if (first)
            {
                tex(write, 1, _arg2);
            }
            else
            {
                write.Invoke(" + ");
                tex(write, 1, _arg2);
            }
        }

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static CultureInfo culture =>
            $LaTeX.culture@30-2;

        [Serializable]
        internal class tex@115-1 : FSharpFunc<Expression, Unit>
        {
            public FSharpFunc<string, Unit> write;

            internal tex@115-1(FSharpFunc<string, Unit> write)
            {
                this.write = write;
            }

            public override Unit Invoke(Expression _arg2)
            {
                LaTeXFormatter.texSummand(this.write, false, _arg2);
                return null;
            }
        }

        [Serializable]
        internal class tex@185 : FSharpFunc<Expression, Unit>
        {
            public FSharpFunc<string, Unit> write;

            internal tex@185(FSharpFunc<string, Unit> write)
            {
                this.write = write;
            }

            public override Unit Invoke(Expression x)
            {
                this.write.Invoke(",");
                LaTeXFormatter.tex(this.write, 0, x);
                return null;
            }
        }

        [Serializable]
        internal class texFractionPart@56 : FSharpFunc<Expression, Unit>
        {
            public FSharpFunc<string, Unit> write;

            internal texFractionPart@56(FSharpFunc<string, Unit> write)
            {
                this.write = write;
            }

            public override Unit Invoke(Expression x)
            {
                LaTeXFormatter.tex(this.write, 2, x);
                return null;
            }
        }
    }
}

