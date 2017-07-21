namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;
    using System.Xml.Linq;

    [CompilationMapping(SourceConstructFlags.Module)]
    internal static class MathMLParser
    {
        internal static FSharpOption<Tuple<XElement, FSharpList<XElement>>> |Apply|_|(XElement _arg1)
        {
            FSharpOption<Tuple<FSharpList<XAttribute>, FSharpList<XElement>, string>> option = |ElementNamed|_|("apply", _arg1);
            if (((option != null) && (option.Value.Item1.TailOrNull == null)) && (option.Value.Item2.TailOrNull != null))
            {
                FSharpList<XElement> list = option.Value.Item2;
                XElement element = list.HeadOrDefault;
                FSharpList<XElement> list2 = list.TailOrNull;
                return FSharpOption<Tuple<XElement, FSharpList<XElement>>>.Some(new Tuple<XElement, FSharpList<XElement>>(element, list2));
            }
            return null;
        }

        internal static Tuple<string, string> |Attribute|(XAttribute xml) => 
            new Tuple<string, string>(xml.Name.LocalName, xml.Value);

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        internal static FSharpOption<string> |AttributeNamed|_|(string name, XAttribute _arg1)
        {
            Tuple<string, string> tuple = new Tuple<string, string>(_arg1.Name.LocalName, _arg1.Value);
            if (string.Equals(tuple.Item1, name))
            {
                string str = tuple.Item2;
                string str2 = tuple.Item1;
                return FSharpOption<string>.Some(str);
            }
            return null;
        }

        internal static FSharpOption<Tuple<string, string>> |CSymbol|_|(XElement _arg1)
        {
            FSharpOption<Tuple<FSharpList<XAttribute>, FSharpList<XElement>, string>> option = |ElementNamed|_|("csymbol", _arg1);
            if ((option != null) && (option.Value.Item1.TailOrNull != null))
            {
                FSharpList<XAttribute> list = option.Value.Item1;
                FSharpOption<string> option2 = |AttributeNamed|_|("cd", list.HeadOrDefault);
                if (((option2 != null) && (list.TailOrNull.TailOrNull == null)) && (option.Value.Item2.TailOrNull == null))
                {
                    string str = option.Value.Item3;
                    string str2 = option2.Value;
                    return FSharpOption<Tuple<string, string>>.Some(new Tuple<string, string>(str2, str));
                }
            }
            return null;
        }

        internal static Tuple<string, FSharpList<XAttribute>, FSharpList<XElement>, string> |Element|(XElement xml) => 
            new Tuple<string, FSharpList<XAttribute>, FSharpList<XElement>, string>(xml.Name.LocalName, SeqModule.ToList<XAttribute>(xml.Attributes()), SeqModule.ToList<XElement>(xml.Elements()), xml.Value);

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        internal static FSharpOption<Tuple<FSharpList<XAttribute>, FSharpList<XElement>, string>> |ElementNamed|_|(string name, XElement _arg1)
        {
            Tuple<string, FSharpList<XAttribute>, FSharpList<XElement>, string> tuple = |Element|(_arg1);
            if (string.Equals(tuple.Item1, name))
            {
                string str = tuple.Item4;
                string str2 = tuple.Item1;
                FSharpList<XElement> list = tuple.Item3;
                FSharpList<XAttribute> list2 = tuple.Item2;
                return FSharpOption<Tuple<FSharpList<XAttribute>, FSharpList<XElement>, string>>.Some(new Tuple<FSharpList<XAttribute>, FSharpList<XElement>, string>(list2, list, str));
            }
            return null;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        internal static FSharpOption<string> |LeafNamed|_|(string name, XElement _arg1)
        {
            FSharpOption<Tuple<FSharpList<XAttribute>, FSharpList<XElement>, string>> option = |ElementNamed|_|(name, _arg1);
            if (((option != null) && (option.Value.Item1.TailOrNull == null)) && (option.Value.Item2.TailOrNull == null))
            {
                string str = option.Value.Item3;
                return FSharpOption<string>.Some(str);
            }
            return null;
        }

        internal static Expression parse(XElement _arg1)
        {
            string str;
            FSharpList<XElement> list2;
            FSharpList<XElement> list3;
            XElement element;
            XElement element2;
            Expression expression;
            FSharpOption<Tuple<XElement, FSharpList<XElement>>> option6;
            FSharpOption<string> option = |LeafNamed|_|("ci", _arg1);
            if (option != null)
            {
                str = option.Value;
                return Expression.NewIdentifier(Symbol.NewSymbol(str));
            }
            FSharpOption<string> option2 = |LeafNamed|_|("cn", _arg1);
            if (option2 != null)
            {
                return Expression.NewNumber(BigRational.Parse(option2.Value));
            }
            FSharpOption<Tuple<string, string>> option3 = |CSymbol|_|(_arg1);
            if (option3 != null)
            {
                str = option3.Value.Item2;
                if (string.Equals(option3.Value.Item1, "nums1"))
                {
                    if (string.Equals(str, "pi"))
                    {
                        return MathNet.Symbolics.Operators.pi;
                    }
                    if (string.Equals(str, "e"))
                    {
                        return Expression.NewConstant(MathNet.Symbolics.Constant.E);
                    }
                    if (string.Equals(str, "i"))
                    {
                        return Expression.NewConstant(MathNet.Symbolics.Constant.I);
                    }
                    if (string.Equals(str, "infinity"))
                    {
                        return Expression.PositiveInfinity;
                    }
                    if (string.Equals(str, "NaN"))
                    {
                        return Expression.Undefined;
                    }
                }
                return Expression.Undefined;
            }
            FSharpOption<Tuple<XElement, FSharpList<XElement>>> option4 = |Apply|_|(_arg1);
            if (option4 == null)
            {
                goto Label_039A;
            }
            FSharpOption<Tuple<string, string>> option5 = |CSymbol|_|(option4.Value.Item1);
            if (option5 == null)
            {
                goto Label_039A;
            }
            str = option5.Value.Item2;
            FSharpList<XElement> list = option4.Value.Item2;
            string a = option5.Value.Item1;
            if (!string.Equals(a, "nums1"))
            {
                if (string.Equals(a, "arith1"))
                {
                    if (!string.Equals(str, "divide"))
                    {
                        if (!string.Equals(str, "unary_minus"))
                        {
                            if (string.Equals(str, "plus"))
                            {
                                list2 = list;
                                return MathNet.Symbolics.Operators.sum(ListModule.Map<XElement, Expression>(new parse@61(), list2));
                            }
                            if (string.Equals(str, "times"))
                            {
                                list2 = list;
                                return MathNet.Symbolics.Operators.product(ListModule.Map<XElement, Expression>(new parse@62-1(), list2));
                            }
                            if (!string.Equals(str, "power"))
                            {
                                if (string.Equals(str, "root") && (list.TailOrNull != null))
                                {
                                    list2 = list;
                                    if (list2.TailOrNull.TailOrNull != null)
                                    {
                                        list3 = list2.TailOrNull;
                                        if (list3.TailOrNull.TailOrNull == null)
                                        {
                                            element = list2.HeadOrDefault;
                                            element2 = list3.HeadOrDefault;
                                            return MathNet.Symbolics.Operators.pow(parse(element), MathNet.Symbolics.Operators.invert(parse(element2)));
                                        }
                                    }
                                }
                            }
                            else if (list.TailOrNull != null)
                            {
                                list2 = list;
                                if (list2.TailOrNull.TailOrNull != null)
                                {
                                    list3 = list2.TailOrNull;
                                    if (list3.TailOrNull.TailOrNull == null)
                                    {
                                        element = list2.HeadOrDefault;
                                        element2 = list3.HeadOrDefault;
                                        return MathNet.Symbolics.Operators.pow(parse(element), parse(element2));
                                    }
                                }
                            }
                        }
                        else if (list.TailOrNull != null)
                        {
                            list2 = list;
                            if (list2.TailOrNull.TailOrNull == null)
                            {
                                element = list2.HeadOrDefault;
                                return MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, parse(element));
                            }
                        }
                    }
                    else if (list.TailOrNull != null)
                    {
                        list2 = list;
                        if (list2.TailOrNull.TailOrNull != null)
                        {
                            list3 = list2.TailOrNull;
                            if (list3.TailOrNull.TailOrNull == null)
                            {
                                element2 = list3.HeadOrDefault;
                                element = list2.HeadOrDefault;
                                goto Label_019C;
                            }
                        }
                    }
                }
                goto Label_01BD;
            }
            if (!string.Equals(str, "rational") || (list.TailOrNull == null))
            {
                goto Label_01BD;
            }
            list2 = list;
            if (list2.TailOrNull.TailOrNull == null)
            {
                goto Label_01BD;
            }
            list3 = list2.TailOrNull;
            if (list3.TailOrNull.TailOrNull != null)
            {
                goto Label_01BD;
            }
            element = list2.HeadOrDefault;
            element2 = list3.HeadOrDefault;
        Label_019C:
            expression = parse(element);
            Expression expression2 = parse(element2);
            return MathNet.Symbolics.Operators.multiply(expression, MathNet.Symbolics.Operators.invert(expression2));
        Label_01BD:
            return Expression.Undefined;
        Label_039A:
            option6 = |Apply|_|(_arg1);
            if (option6 != null)
            {
                Tuple<string, FSharpList<XAttribute>, FSharpList<XElement>, string> tuple = |Element|(option6.Value.Item1);
                if ((tuple.Item2.TailOrNull == null) && (tuple.Item3.TailOrNull == null))
                {
                    FSharpList<XElement> list4;
                    str = tuple.Item1;
                    list = option6.Value.Item2;
                    if (!string.Equals(str, "minus"))
                    {
                        if (string.Equals(str, "plus"))
                        {
                            list2 = list;
                            return MathNet.Symbolics.Operators.sum(ListModule.Map<XElement, Expression>(new parse@71-2(), list2));
                        }
                        if (string.Equals(str, "times"))
                        {
                            list2 = list;
                            return MathNet.Symbolics.Operators.product(ListModule.Map<XElement, Expression>(new parse@72-3(), list2));
                        }
                        if (!string.Equals(str, "divide"))
                        {
                            if (!string.Equals(str, "power"))
                            {
                                if (string.Equals(str, "root") && (list.TailOrNull != null))
                                {
                                    list2 = list;
                                    FSharpOption<Tuple<FSharpList<XAttribute>, FSharpList<XElement>, string>> option7 = |ElementNamed|_|("degree", list2.HeadOrDefault);
                                    if (((option7 != null) && (option7.Value.Item1.TailOrNull == null)) && (option7.Value.Item2.TailOrNull != null))
                                    {
                                        list3 = option7.Value.Item2;
                                        if ((list3.TailOrNull.TailOrNull == null) && (list2.TailOrNull.TailOrNull != null))
                                        {
                                            list4 = list2.TailOrNull;
                                            if (list4.TailOrNull.TailOrNull == null)
                                            {
                                                element = list4.HeadOrDefault;
                                                element2 = list3.HeadOrDefault;
                                                return MathNet.Symbolics.Operators.pow(parse(element), MathNet.Symbolics.Operators.invert(parse(element2)));
                                            }
                                        }
                                    }
                                }
                            }
                            else if (list.TailOrNull != null)
                            {
                                list2 = list;
                                if (list2.TailOrNull.TailOrNull != null)
                                {
                                    list3 = list2.TailOrNull;
                                    if (list3.TailOrNull.TailOrNull == null)
                                    {
                                        element = list2.HeadOrDefault;
                                        element2 = list3.HeadOrDefault;
                                        return MathNet.Symbolics.Operators.pow(parse(element), parse(element2));
                                    }
                                }
                            }
                        }
                        else if (list.TailOrNull != null)
                        {
                            list2 = list;
                            if (list2.TailOrNull.TailOrNull != null)
                            {
                                list3 = list2.TailOrNull;
                                if (list3.TailOrNull.TailOrNull == null)
                                {
                                    element = list2.HeadOrDefault;
                                    element2 = list3.HeadOrDefault;
                                    expression = parse(element);
                                    expression2 = parse(element2);
                                    return MathNet.Symbolics.Operators.multiply(expression, MathNet.Symbolics.Operators.invert(expression2));
                                }
                            }
                        }
                    }
                    else if (list.TailOrNull != null)
                    {
                        list2 = list;
                        if (list2.TailOrNull.TailOrNull == null)
                        {
                            element = list2.HeadOrDefault;
                            return MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, parse(element));
                        }
                        list3 = list2.TailOrNull;
                        if (list3.TailOrNull.TailOrNull == null)
                        {
                            element = list3.HeadOrDefault;
                            element2 = list2.HeadOrDefault;
                            expression = parse(element2);
                            expression2 = parse(element);
                            return MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, MathNet.Symbolics.Operators.add(expression, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression2)));
                        }
                    }
                    if (string.Equals(str, "root") && (list.TailOrNull != null))
                    {
                        list4 = list;
                        if (list4.TailOrNull.TailOrNull == null)
                        {
                            return Expression.NewPower(parse(list4.HeadOrDefault), Expression.NewPower(MathNet.Symbolics.Operators.two, MathNet.Symbolics.Operators.minusOne));
                        }
                    }
                    return Expression.Undefined;
                }
            }
            return Expression.Undefined;
        }

        [Serializable]
        internal class parse@61 : FSharpFunc<XElement, Expression>
        {
            internal parse@61()
            {
            }

            public override Expression Invoke(XElement _arg1) => 
                MathMLParser.parse(_arg1);
        }

        [Serializable]
        internal class parse@62-1 : FSharpFunc<XElement, Expression>
        {
            internal parse@62-1()
            {
            }

            public override Expression Invoke(XElement _arg1) => 
                MathMLParser.parse(_arg1);
        }

        [Serializable]
        internal class parse@71-2 : FSharpFunc<XElement, Expression>
        {
            internal parse@71-2()
            {
            }

            public override Expression Invoke(XElement _arg1) => 
                MathMLParser.parse(_arg1);
        }

        [Serializable]
        internal class parse@72-3 : FSharpFunc<XElement, Expression>
        {
            internal parse@72-3()
            {
            }

            public override Expression Invoke(XElement _arg1) => 
                MathMLParser.parse(_arg1);
        }
    }
}

