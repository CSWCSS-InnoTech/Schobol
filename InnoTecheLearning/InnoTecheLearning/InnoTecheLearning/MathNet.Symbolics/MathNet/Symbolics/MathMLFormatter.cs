namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;
    using System.Numerics;
    using System.Xml.Linq;

    [CompilationMapping(SourceConstructFlags.Module)]
    internal static class MathMLFormatter
    {
        [CompilationArgumentCounts(new int[] { 1, 1, 1 })]
        internal static XElement apply(string dict, string @operator, FSharpList<XElement> args) => 
            node("apply", FSharpList<XElement>.Cons(csymbol(dict, @operator), args));

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        internal static XAttribute attribute<a>(string name, a value) => 
            new XAttribute(XName.Get(name), value);

        internal static XElement ci(Symbol _arg1)
        {
            string item = _arg1.item;
            object[] content = new object[] { item };
            return new XElement(XName.Get("ci"), content);
        }

        internal static XElement cn<a>(a value)
        {
            string str = PrintfModule.PrintFormatToStringThen<string, FSharpFunc<a, string>>(new cn@93(), new PrintfFormat<FSharpFunc<a, string>, Unit, string, string, a>("%A")).Invoke(value);
            object[] content = new object[] { str };
            return new XElement(XName.Get("cn"), content);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        internal static XElement csymbol(string dict, string name)
        {
            object[] content = new object[] { new XAttribute(XName.Get("cd"), dict), name };
            return new XElement(XName.Get("csymbol"), content);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        internal static XElement element(string name, object[] values) => 
            new XElement(XName.Get(name), values);

        internal static XElement empty(string name) => 
            new XElement(XName.Get(name), new object[0]);

        internal static XElement formatContentStrict(Expression _arg1)
        {
            BigRational item;
            Expression.Product product;
            FSharpList<Expression> list;
            Expression expression;
            Expression expression2;
            Expression expression3;
            FSharpList<XElement> list2;
            FSharpOption<BigRational> option = ExpressionPatterns.|Integer|_|(_arg1);
            if (option != null)
            {
                item = option.Value;
                return cn<BigInteger>(item.Numerator);
            }
            switch (_arg1.Tag)
            {
                case 0:
                    item = ((Expression.Number) _arg1).item;
                    list2 = FSharpList<XElement>.Cons(cn<BigInteger>(item.Numerator), FSharpList<XElement>.Cons(cn<BigInteger>(item.Denominator), FSharpList<XElement>.Empty));
                    return node("apply", FSharpList<XElement>.Cons(csymbol("nums1", "rational"), list2));

                case 1:
                {
                    Expression.Approximation approximation = (Expression.Approximation) _arg1;
                    if (approximation.item is MathNet.Symbolics.Approximation.Complex)
                    {
                        return cn<System.Numerics.Complex>(((MathNet.Symbolics.Approximation.Complex) approximation.item).item);
                    }
                    return cn<double>(((MathNet.Symbolics.Approximation.Real) approximation.item).item);
                }
                case 2:
                {
                    string str = ((Expression.Identifier) _arg1).item.item;
                    object[] content = new object[] { str };
                    return new XElement(XName.Get("ci"), content);
                }
                case 3:
                {
                    Expression.Constant constant = (Expression.Constant) _arg1;
                    switch (constant.item.Tag)
                    {
                        case 1:
                            return csymbol("nums1", "pi");

                        case 2:
                            return csymbol("nums1", "i");
                    }
                    return csymbol("nums1", "e");
                }
                case 4:
                    list = ((Expression.Sum) _arg1).item;
                    list2 = ListModule.Map<Expression, XElement>(new formatContentStrict@108-1(), list);
                    return node("apply", FSharpList<XElement>.Cons(csymbol("arith1", "plus"), list2));

                case 5:
                {
                    product = (Expression.Product) _arg1;
                    if (product.item.TailOrNull == null)
                    {
                        break;
                    }
                    list = product.item;
                    if (!list.HeadOrDefault.Equals(MathNet.Symbolics.Operators.minusOne, LanguagePrimitives.GenericEqualityComparer))
                    {
                        break;
                    }
                    FSharpList<Expression> xs = list.TailOrNull;
                    expression = list.HeadOrDefault;
                    list2 = FSharpList<XElement>.Cons(formatContentStrict(MathNet.Symbolics.Operators.product(xs)), FSharpList<XElement>.Empty);
                    return node("apply", FSharpList<XElement>.Cons(csymbol("arith1", "unary_minus"), list2));
                }
            }
            if (_arg1.Tag == 5)
            {
                product = (Expression.Product) _arg1;
                list = product.item;
                expression = _arg1;
                expression2 = InfixFormatter.numerator(expression);
                expression3 = InfixFormatter.denominator(expression);
                if (MathNet.Symbolics.Operators.isOne(expression3))
                {
                    list2 = ListModule.Map<Expression, XElement>(new formatContentStrict@114(), list);
                    return node("apply", FSharpList<XElement>.Cons(csymbol("arith1", "times"), list2));
                }
                list2 = FSharpList<XElement>.Cons(formatContentStrict(expression2), FSharpList<XElement>.Cons(formatContentStrict(expression3), FSharpList<XElement>.Empty));
                return node("apply", FSharpList<XElement>.Cons(csymbol("arith1", "divide"), list2));
            }
            FSharpOption<Tuple<Expression, Expression>> option2 = ExpressionPatterns.|NegIntPower|_|(_arg1);
            if ((option2 != null) && option2.Value.Item2.Equals(MathNet.Symbolics.Operators.minusOne, LanguagePrimitives.GenericEqualityComparer))
            {
                expression = option2.Value.Item1;
                expression2 = option2.Value.Item2;
                list2 = FSharpList<XElement>.Cons(cn<int>(1), FSharpList<XElement>.Cons(formatContentStrict(expression), FSharpList<XElement>.Empty));
                return node("apply", FSharpList<XElement>.Cons(csymbol("arith1", "divide"), list2));
            }
            FSharpOption<Tuple<Expression, Expression>> option3 = ExpressionPatterns.|NegIntPower|_|(_arg1);
            if (option3 != null)
            {
                expression = option3.Value.Item1;
                expression2 = option3.Value.Item2;
                list2 = FSharpList<XElement>.Cons(formatContentStrict(expression), FSharpList<XElement>.Cons(formatContentStrict(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression2)), FSharpList<XElement>.Empty));
                XElement head = node("apply", FSharpList<XElement>.Cons(csymbol("arith1", "power"), list2));
                list2 = FSharpList<XElement>.Cons(cn<int>(1), FSharpList<XElement>.Cons(head, FSharpList<XElement>.Empty));
                return node("apply", FSharpList<XElement>.Cons(csymbol("arith1", "divide"), list2));
            }
            if (_arg1.Tag == 6)
            {
                Expression.Power power = (Expression.Power) _arg1;
                if (power.item2.Tag == 0)
                {
                    Expression.Number number = (Expression.Number) power.item2;
                    item = number.item;
                    if (item.IsPositive && LanguagePrimitives.HashCompare.GenericEqualityIntrinsic<BigInteger>(item.Numerator, BigInteger.One))
                    {
                        expression = power.item1;
                        item = number.item;
                        list2 = FSharpList<XElement>.Cons(formatContentStrict(expression), FSharpList<XElement>.Cons(formatContentStrict(Expression.NewNumber(BigRational.Reciprocal(item))), FSharpList<XElement>.Empty));
                        return node("apply", FSharpList<XElement>.Cons(csymbol("arith1", "root"), list2));
                    }
                }
            }
            if (_arg1.Tag == 6)
            {
                Expression.Power power2 = (Expression.Power) _arg1;
                if (power2.item2.Tag == 6)
                {
                    Expression.Power power3 = (Expression.Power) power2.item2;
                    if (power3.item2.Equals(MathNet.Symbolics.Operators.minusOne, LanguagePrimitives.GenericEqualityComparer))
                    {
                        expression = power2.item1;
                        expression2 = power3.item1;
                        expression3 = power3.item2;
                        list2 = FSharpList<XElement>.Cons(formatContentStrict(expression), FSharpList<XElement>.Cons(formatContentStrict(expression2), FSharpList<XElement>.Empty));
                        return node("apply", FSharpList<XElement>.Cons(csymbol("arith1", "root"), list2));
                    }
                }
            }
            switch (_arg1.Tag)
            {
                case 6:
                {
                    Expression.Power power4 = (Expression.Power) _arg1;
                    expression = power4.item1;
                    expression2 = power4.item2;
                    list2 = FSharpList<XElement>.Cons(formatContentStrict(expression), FSharpList<XElement>.Cons(formatContentStrict(expression2), FSharpList<XElement>.Empty));
                    return node("apply", FSharpList<XElement>.Cons(csymbol("arith1", "power"), list2));
                }
                case 7:
                    throw new Exception("not implemented");

                case 8:
                    throw new Exception("not implemented");

                case 9:
                    return csymbol("nums1", "infinity");

                case 10:
                    return csymbol("nums1", "infinity");

                case 11:
                    list2 = FSharpList<XElement>.Cons(csymbol("nums1", "infinity"), FSharpList<XElement>.Empty);
                    return node("apply", FSharpList<XElement>.Cons(csymbol("arith1", "unary_minus"), list2));

                case 12:
                    return csymbol("nums1", "NaN");
            }
            throw new MatchFailureException(@"D:\Dev\Math.NET\mathnet-symbolics\src\Symbolics\Convert\MathML.fs", 0x63, 0x22);
        }

        internal static XElement formatSemanticsAnnotated(Expression x)
        {
            XElement head = formatContentStrict(x);
            string str = LaTeX.Format(x);
            string str2 = Infix.Format(x);
            object[] content = new object[] { new XAttribute(XName.Get("encoding"), "application/x-tex"), str };
            content = new object[] { new XAttribute(XName.Get("encoding"), "application/x-mathnet-infix"), str2 };
            return node("semantics", FSharpList<XElement>.Cons(head, FSharpList<XElement>.Cons(new XElement(XName.Get("annotation"), content), FSharpList<XElement>.Cons(new XElement(XName.Get("annotation"), content), FSharpList<XElement>.Empty))));
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        internal static XElement leaf(string name, string body)
        {
            object[] content = new object[] { body };
            return new XElement(XName.Get(name), content);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        internal static XElement node(string name, FSharpList<XElement> children)
        {
            XElement[] elementArray = ListModule.ToArray<XElement>(children);
            if (elementArray == null)
            {
                throw new ArgumentNullException("array");
            }
            int length = elementArray.Length;
            object[] content = new object[length];
            int index = 0;
            int num2 = length - 1;
            if (num2 >= index)
            {
                do
                {
                    content[index] = elementArray[index];
                    index++;
                }
                while (index != (num2 + 1));
            }
            return new XElement(XName.Get(name), content);
        }

        [Serializable]
        internal class cn@93 : FSharpFunc<string, string>
        {
            internal cn@93()
            {
            }

            public override string Invoke(string x) => 
                x;
        }

        [Serializable]
        internal class formatContentStrict@108-1 : FSharpFunc<Expression, XElement>
        {
            internal formatContentStrict@108-1()
            {
            }

            public override XElement Invoke(Expression _arg1) => 
                MathMLFormatter.formatContentStrict(_arg1);
        }

        [Serializable]
        internal class formatContentStrict@114 : FSharpFunc<Expression, XElement>
        {
            internal formatContentStrict@114()
            {
            }

            public override XElement Invoke(Expression _arg1) => 
                MathMLFormatter.formatContentStrict(_arg1);
        }
    }
}

