namespace MathNet.Symbolics
{
    using <StartupCode$MathNet-Symbolics>;
    using FParsec;
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using Microsoft.FSharp.Core.CompilerServices;
    using Microsoft.FSharp.Reflection;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Numerics;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    [CompilationMapping(SourceConstructFlags.Module)]
    internal static class InfixParser
    {
        internal static FSharpFunc<CharStream<a>, Reply<Expression>> abs<a>(FSharpFunc<CharStream<a>, Reply<Expression>> p) => 
            Primitives.op_BarGreaterGreater<Expression, a, Expression>(Primitives.between<string, a, string, Expression>(Primitives.op_DotGreaterGreater<string, a, Unit>(CharParsers.pstring<a>("|"), CharParsers.spaces<a>()), Primitives.op_DotGreaterGreater<string, a, Unit>(CharParsers.pstring<a>("|"), CharParsers.spaces<a>()), p), new abs@30());

        internal static Expression applyFunction(MathNet.Symbolics.Function _arg1_0, FSharpList<Expression> _arg1_1)
        {
            MathNet.Symbolics.Function function;
            FSharpList<Expression> list2;
            if (_arg1_1.TailOrNull == null)
            {
                list2 = _arg1_1;
                function = _arg1_0;
            }
            else
            {
                FSharpList<Expression> list = _arg1_1;
                if (list.TailOrNull.TailOrNull == null)
                {
                    function = _arg1_0;
                    Expression x = list.HeadOrDefault;
                    return MathNet.Symbolics.Operators.apply(function, x);
                }
                function = _arg1_0;
                list2 = _arg1_1;
            }
            throw new Exception("not supported yet");
        }

        internal static Expression applyPseudo(Pseudo _arg1_0, FSharpList<Expression> _arg1_1)
        {
            FSharpList<Expression> list;
            if (_arg1_0.Tag != 1)
            {
                if (_arg1_1.TailOrNull != null)
                {
                    list = _arg1_1;
                    if (list.TailOrNull.TailOrNull == null)
                    {
                        return Expression.NewPower(list.HeadOrDefault, Expression.NewPower(MathNet.Symbolics.Operators.two, MathNet.Symbolics.Operators.minusOne));
                    }
                }
            }
            else if (_arg1_1.TailOrNull != null)
            {
                list = _arg1_1;
                if (list.TailOrNull.TailOrNull != null)
                {
                    FSharpList<Expression> list2 = list.TailOrNull;
                    if (list2.TailOrNull.TailOrNull == null)
                    {
                        Expression y = list2.HeadOrDefault;
                        Expression x = list.HeadOrDefault;
                        return MathNet.Symbolics.Operators.pow(x, y);
                    }
                }
            }
            throw new Exception("wrong matching");
        }

        [CompilerGenerated]
        internal static Tuple<string, MathNet.Symbolics.Function> f@1-2(UnionCaseInfo @case) => 
            new Tuple<string, MathNet.Symbolics.Function>(@case.Name.ToLower(), LanguagePrimitives.IntrinsicFunctions.UnboxGeneric<MathNet.Symbolics.Function>(FSharpValue.MakeUnion(@case, new object[0], null)));

        [CompilerGenerated]
        internal static Tuple<string, Pseudo> f@1-3(UnionCaseInfo @case) => 
            new Tuple<string, Pseudo>(@case.Name.ToLower(), LanguagePrimitives.IntrinsicFunctions.UnboxGeneric<Pseudo>(FSharpValue.MakeUnion(@case, new object[0], FSharpOption<BindingFlags>.Some(BindingFlags.NonPublic | BindingFlags.Public))));

        internal static bool isMathChar@46(char _arg1)
        {
            switch (_arg1)
            {
                case 'π':
                case '∞':
                case '⧝':
                    return true;
            }
            return false;
        }

        internal static FSharpFunc<CharStream<b>, Reply<a>> parens<a, b>(FSharpFunc<CharStream<b>, Reply<a>> p) => 
            Primitives.between<string, b, string, a>(Primitives.op_DotGreaterGreater<string, b, Unit>(CharParsers.pstring<b>("("), CharParsers.spaces<b>()), Primitives.op_DotGreaterGreater<string, b, Unit>(CharParsers.pstring<b>(")"), CharParsers.spaces<b>()), p);

        internal static ParseResult parse(string text)
        {
            CharParsers.ParserResult<Expression, Unit> result = CharParsers.run<Expression>(parser, text);
            if (result is CharParsers.ParserResult<Expression, Unit>.Failure)
            {
                CharParsers.ParserResult<Expression, Unit>.Failure failure = (CharParsers.ParserResult<Expression, Unit>.Failure) result;
                return ParseResult.NewParseFailure(failure.Item1);
            }
            CharParsers.ParserResult<Expression, Unit>.Success success = (CharParsers.ParserResult<Expression, Unit>.Success) result;
            return ParseResult.NewParsedExpression(success.Item1);
        }

        internal static FSharpFunc<CharStream<a>, Reply<string>> str_ws<a>(string s) => 
            Primitives.op_DotGreaterGreater<string, a, Unit>(CharParsers.pstring<a>(s), CharParsers.spaces<a>());

        internal static FSharpFunc<CharStream<a>, Reply<Unit>> ws<a>() => 
            CharParsers.spaces<a>();

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<Expression>> absTerm@95 =>
            $Infix.absTerm@95;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static Tuple<string, MathNet.Symbolics.Function>[] cases@61 =>
            $Infix.cases@61;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static Tuple<string, Pseudo>[] cases@75-2 =>
            $Infix.cases@75-1;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<MathNet.Symbolics.Function>> clo1@67 =>
            $Infix.clo1@67;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<Pseudo>> clo1@81-1 =>
            $Infix.clo1@81-1;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<Expression>> expr@92 =>
            $Infix.expr@92;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<Expression>> expression =>
            $Infix.expression@89;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<FSharpList<Expression>>> functionArgs@97 =>
            $Infix.functionArgs@97;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<MathNet.Symbolics.Function>> functionName =>
            $Infix.functionName@60;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<Expression>> functionTerm@98 =>
            $Infix.functionTerm@98;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<Expression>> identifier =>
            $Infix.identifier@45;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<char, bool> isIdentifierChar@48 =>
            $Infix.isIdentifierChar@48;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<char, bool> isIdentifierFirstChar@47 =>
            $Infix.isIdentifierFirstChar@47;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static int len@1 =>
            $Infix.len@1;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static int len@1-1 =>
            $Infix.len@1-1;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<Expression>> number =>
            $Infix.number@32-2;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static OperatorPrecedenceParser<Expression, Unit, Unit> opp@91 =>
            $Infix.opp@91;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<Expression>> parensTerm@94 =>
            $Infix.parensTerm@94;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<Expression>> parser =>
            $Infix.parser@114;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<Pseudo>> pseudoName =>
            $Infix.pseudoName@73;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<Expression>> pseudoTerm@100 =>
            $Infix.pseudoTerm@100;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static Tuple<string, MathNet.Symbolics.Function>[] res@1 =>
            $Infix.res@1;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static Tuple<string, Pseudo>[] res@1-1 =>
            $Infix.res@1-1;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<Expression>> term@102 =>
            $Infix.term@102;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static FSharpFunc<CharStream<Unit>, Reply<Expression>> value =>
            $Infix.value@58;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static UnionCaseInfo[] x@1 =>
            $Infix.x@1;

        [CompilationMapping(SourceConstructFlags.Value)]
        internal static UnionCaseInfo[] x@1-1 =>
            $Infix.x@1-1;

        [Serializable]
        internal class abs@30 : FSharpFunc<Expression, Expression>
        {
            internal abs@30()
            {
            }

            public override Expression Invoke(Expression arg00) => 
                MathNet.Symbolics.Operators.abs(arg00);
        }

        [Serializable]
        internal class cases@65-1 : FSharpFunc<Tuple<string, MathNet.Symbolics.Function>, int>
        {
            internal cases@65-1()
            {
            }

            public override int Invoke(Tuple<string, MathNet.Symbolics.Function> tupledArg)
            {
                string str = tupledArg.Item1;
                MathNet.Symbolics.Function function = tupledArg.Item2;
                return -str.Length;
            }
        }

        [Serializable]
        internal class cases@79-3 : FSharpFunc<Tuple<string, InfixParser.Pseudo>, int>
        {
            internal cases@79-3()
            {
            }

            public override int Invoke(Tuple<string, InfixParser.Pseudo> tupledArg)
            {
                string str = tupledArg.Item1;
                InfixParser.Pseudo pseudo = tupledArg.Item2;
                return -str.Length;
            }
        }

        [Serializable]
        internal class expression@105 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal expression@105()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                MathNet.Symbolics.Operators.add(x, y);
        }

        [Serializable]
        internal class expression@106-1 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal expression@106-1()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                MathNet.Symbolics.Operators.add(x, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, y));
        }

        [Serializable]
        internal class expression@107-2 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal expression@107-2()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                MathNet.Symbolics.Operators.multiply(x, y);
        }

        [Serializable]
        internal class expression@108-3 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal expression@108-3()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                MathNet.Symbolics.Operators.multiply(x, MathNet.Symbolics.Operators.invert(y));
        }

        [Serializable]
        internal class expression@109-4 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal expression@109-4()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                MathNet.Symbolics.Operators.pow(x, y);
        }

        [Serializable]
        internal class expression@110-5 : FSharpFunc<Expression, Expression>
        {
            internal expression@110-5()
            {
            }

            public override Expression Invoke(Expression x) => 
                x;
        }

        [Serializable]
        internal class expression@111-6 : FSharpFunc<Expression, Expression>
        {
            internal expression@111-6()
            {
            }

            public override Expression Invoke(Expression x) => 
                MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, x);
        }

        [Serializable, CompilationMapping(SourceConstructFlags.Closure)]
        internal sealed class functionName@67 : GeneratedSequenceBase<FSharpFunc<CharStream<Unit>, Reply<MathNet.Symbolics.Function>>>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public FSharpFunc<CharStream<Unit>, Reply<MathNet.Symbolics.Function>> current;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public IEnumerator<Tuple<string, MathNet.Symbolics.Function>> @enum;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public Tuple<string, MathNet.Symbolics.Function> matchValue;
            public string name;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public int pc;
            public MathNet.Symbolics.Function union;

            public functionName@67(Tuple<string, MathNet.Symbolics.Function> matchValue, MathNet.Symbolics.Function union, string name, IEnumerator<Tuple<string, MathNet.Symbolics.Function>> @enum, int pc, FSharpFunc<CharStream<Unit>, Reply<MathNet.Symbolics.Function>> current)
            {
                this.matchValue = matchValue;
                this.union = union;
                this.name = name;
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
                                LanguagePrimitives.IntrinsicFunctions.Dispose<IEnumerator<Tuple<string, MathNet.Symbolics.Function>>>(this.@enum);
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

            public override int GenerateNext(ref IEnumerable<FSharpFunc<CharStream<Unit>, Reply<MathNet.Symbolics.Function>>> next)
            {
                switch (this.pc)
                {
                    case 1:
                        goto Label_00DC;

                    case 2:
                        this.name = null;
                        this.union = null;
                        this.matchValue = null;
                        break;

                    case 3:
                        goto Label_00FD;

                    default:
                        this.@enum = ((IEnumerable<Tuple<string, MathNet.Symbolics.Function>>) InfixParser.cases@61).GetEnumerator();
                        this.pc = 1;
                        break;
                }
                if (this.@enum.MoveNext())
                {
                    this.matchValue = this.@enum.Current;
                    this.union = this.matchValue.Item2;
                    this.name = this.matchValue.Item1;
                    this.pc = 2;
                    this.current = Primitives.op_BarGreaterGreater<string, Unit, MathNet.Symbolics.Function>(Primitives.op_DotGreaterGreater<string, Unit, Unit>(CharParsers.pstring<Unit>(this.name), CharParsers.spaces<Unit>()), new InfixParser.functionName@67-1(this.union));
                    return 1;
                }
            Label_00DC:
                this.pc = 3;
                LanguagePrimitives.IntrinsicFunctions.Dispose<IEnumerator<Tuple<string, MathNet.Symbolics.Function>>>(this.@enum);
                this.@enum = null;
                this.pc = 3;
            Label_00FD:
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
            public override FSharpFunc<CharStream<Unit>, Reply<MathNet.Symbolics.Function>> LastGenerated => 
                this.current;

            [CompilerGenerated]
            public override IEnumerator<FSharpFunc<CharStream<Unit>, Reply<MathNet.Symbolics.Function>>> GetFreshEnumerator() => 
                new InfixParser.functionName@67(null, null, null, null, 0, null);
        }

        [Serializable]
        internal class functionName@67-1 : FSharpFunc<string, MathNet.Symbolics.Function>
        {
            public MathNet.Symbolics.Function union;

            internal functionName@67-1(MathNet.Symbolics.Function union)
            {
                this.union = union;
            }

            public override MathNet.Symbolics.Function Invoke(string _arg2) => 
                this.union;
        }

        [Serializable]
        internal class functionName@67-2 : FSharpFunc<CharStream<Unit>, Reply<MathNet.Symbolics.Function>>
        {
            internal functionName@67-2()
            {
            }

            public override Reply<MathNet.Symbolics.Function> Invoke(CharStream<Unit> arg10) => 
                InfixParser.clo1@67.Invoke(arg10);
        }

        [Serializable]
        internal class functionTerm@98-1 : FSharpFunc<Tuple<MathNet.Symbolics.Function, FSharpList<Expression>>, Expression>
        {
            internal functionTerm@98-1()
            {
            }

            public override Expression Invoke(Tuple<MathNet.Symbolics.Function, FSharpList<Expression>> tupledArg)
            {
                MathNet.Symbolics.Function function = tupledArg.Item1;
                FSharpList<Expression> list = tupledArg.Item2;
                return InfixParser.applyFunction(function, list);
            }
        }

        [Serializable]
        internal class identifier@50 : FSharpFunc<string, Expression>
        {
            internal identifier@50()
            {
            }

            public override Expression Invoke(string _arg2)
            {
                if (!string.Equals(_arg2, "pi") && !string.Equals(_arg2, "π"))
                {
                    if (string.Equals(_arg2, "e"))
                    {
                        return Expression.NewConstant(MathNet.Symbolics.Constant.E);
                    }
                    if (!string.Equals(_arg2, "oo") && (!string.Equals(_arg2, "inf") && !string.Equals(_arg2, "∞")))
                    {
                        if (string.Equals(_arg2, "⧝"))
                        {
                            return Expression.ComplexInfinity;
                        }
                        if (string.Equals(_arg2, "j"))
                        {
                            return Expression.NewConstant(MathNet.Symbolics.Constant.I);
                        }
                        return Expression.NewIdentifier(Symbol.NewSymbol(_arg2));
                    }
                    return Expression.PositiveInfinity;
                }
                return Expression.NewConstant(MathNet.Symbolics.Constant.Pi);
            }
        }

        [Serializable]
        internal class isIdentifierChar@48-1 : FSharpFunc<char, bool>
        {
            internal isIdentifierChar@48-1()
            {
            }

            public override bool Invoke(char c)
            {
                uint num = c | 0x20;
                return ((!(!(((num - 0x61) > (0x7a - 0x61)) ? ((c > '\x007f') && char.IsLetter(c)) : true) ? ((c - 0x30) <= (0x39 - 0x30)) : true) ? InfixParser.isMathChar@46(c) : true) || (c == '_'));
            }
        }

        [Serializable]
        internal class isIdentifierFirstChar@47-1 : FSharpFunc<char, bool>
        {
            internal isIdentifierFirstChar@47-1()
            {
            }

            public override bool Invoke(char c)
            {
                uint num = c | 0x20;
                return ((((num - 0x61) > (0x7a - 0x61)) ? ((c > '\x007f') && char.IsLetter(c)) : true) || InfixParser.isMathChar@46(c));
            }
        }

        [Serializable]
        internal class number@40-1 : FSharpFunc<CharParsers.NumberLiteral, Expression>
        {
            internal number@40-1()
            {
            }

            public override Expression Invoke(CharParsers.NumberLiteral num)
            {
                if (num.IsInfinity)
                {
                    return Expression.PositiveInfinity;
                }
                if (num.IsInteger)
                {
                    return Expression.NewNumber(BigRational.FromBigInt(BigInteger.Parse(num.String)));
                }
                return Values.unpack(ValueModule.real(double.Parse(num.String, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture)));
            }
        }

        [Serializable, DebuggerDisplay("{__DebugDisplay(),nq}"), CompilationMapping(SourceConstructFlags.NonPublicRepresentation | SourceConstructFlags.SumType)]
        internal sealed class Pseudo : IEquatable<InfixParser.Pseudo>, IStructuralEquatable, IComparable<InfixParser.Pseudo>, IComparable, IStructuralComparable
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly int _tag;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal static readonly InfixParser.Pseudo _unique_Pow = new InfixParser.Pseudo(1);
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal static readonly InfixParser.Pseudo _unique_Sqrt = new InfixParser.Pseudo(0);

            [CompilerGenerated]
            internal Pseudo(int _tag)
            {
                this._tag = _tag;
            }

            [CompilerGenerated]
            internal object __DebugDisplay() => 
                ExtraTopLevelOperators.PrintFormatToString<FSharpFunc<InfixParser.Pseudo, string>>(new PrintfFormat<FSharpFunc<InfixParser.Pseudo, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);

            [CompilerGenerated]
            public sealed override int CompareTo(InfixParser.Pseudo obj)
            {
                if (this > null)
                {
                    if (obj <= null)
                    {
                        return 1;
                    }
                    int num = this._tag;
                    int num2 = obj._tag;
                    if (num == num2)
                    {
                        return 0;
                    }
                    return (num - num2);
                }
                if (obj > null)
                {
                    return -1;
                }
                return 0;
            }

            [CompilerGenerated]
            public sealed override int CompareTo(object obj) => 
                this.CompareTo((InfixParser.Pseudo) obj);

            [CompilerGenerated]
            public sealed override int CompareTo(object obj, IComparer comp)
            {
                InfixParser.Pseudo pseudo = (InfixParser.Pseudo) obj;
                if (this > null)
                {
                    if (((InfixParser.Pseudo) obj) <= null)
                    {
                        return 1;
                    }
                    int num = this._tag;
                    int num2 = pseudo._tag;
                    if (num == num2)
                    {
                        return 0;
                    }
                    return (num - num2);
                }
                if (((InfixParser.Pseudo) obj) > null)
                {
                    return -1;
                }
                return 0;
            }

            [CompilerGenerated]
            public sealed override bool Equals(InfixParser.Pseudo obj)
            {
                if (this <= null)
                {
                    return (obj <= null);
                }
                if (obj > null)
                {
                    int num = this._tag;
                    int num2 = obj._tag;
                    return (num == num2);
                }
                return false;
            }

            [CompilerGenerated]
            public sealed override bool Equals(object obj)
            {
                InfixParser.Pseudo pseudo = obj as InfixParser.Pseudo;
                return ((pseudo != null) && this.Equals(pseudo));
            }

            [CompilerGenerated]
            public sealed override bool Equals(object obj, IEqualityComparer comp)
            {
                if (this <= null)
                {
                    return (obj <= null);
                }
                InfixParser.Pseudo pseudo = obj as InfixParser.Pseudo;
                if (pseudo != null)
                {
                    int num = this._tag;
                    int num2 = pseudo._tag;
                    return (num == num2);
                }
                return false;
            }

            [CompilerGenerated]
            public sealed override int GetHashCode() => 
                this.GetHashCode(LanguagePrimitives.GenericEqualityComparer);

            [CompilerGenerated]
            public sealed override int GetHashCode(IEqualityComparer comp)
            {
                if (this <= null)
                {
                    return 0;
                }
                if (this.Tag == 0)
                {
                    return 0;
                }
                return 1;
            }

            [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal bool IsPow =>
                (this.Tag == 1);

            [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal bool IsSqrt =>
                (this.Tag == 0);

            [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal static InfixParser.Pseudo Pow =>
                _unique_Pow;

            [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal static InfixParser.Pseudo Sqrt =>
                _unique_Sqrt;

            [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal int Tag =>
                this._tag;

            internal static class Tags
            {
                public const int Pow = 1;
                public const int Sqrt = 0;
            }
        }

        [Serializable, CompilationMapping(SourceConstructFlags.Closure)]
        internal sealed class pseudoName@81 : GeneratedSequenceBase<FSharpFunc<CharStream<Unit>, Reply<InfixParser.Pseudo>>>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public FSharpFunc<CharStream<Unit>, Reply<InfixParser.Pseudo>> current;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public IEnumerator<Tuple<string, InfixParser.Pseudo>> @enum;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public Tuple<string, InfixParser.Pseudo> matchValue;
            public string name;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public int pc;
            public InfixParser.Pseudo union;

            public pseudoName@81(Tuple<string, InfixParser.Pseudo> matchValue, InfixParser.Pseudo union, string name, IEnumerator<Tuple<string, InfixParser.Pseudo>> @enum, int pc, FSharpFunc<CharStream<Unit>, Reply<InfixParser.Pseudo>> current)
            {
                this.matchValue = matchValue;
                this.union = union;
                this.name = name;
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
                                LanguagePrimitives.IntrinsicFunctions.Dispose<IEnumerator<Tuple<string, InfixParser.Pseudo>>>(this.@enum);
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

            public override int GenerateNext(ref IEnumerable<FSharpFunc<CharStream<Unit>, Reply<InfixParser.Pseudo>>> next)
            {
                switch (this.pc)
                {
                    case 1:
                        goto Label_00DC;

                    case 2:
                        this.name = null;
                        this.union = null;
                        this.matchValue = null;
                        break;

                    case 3:
                        goto Label_00FD;

                    default:
                        this.@enum = ((IEnumerable<Tuple<string, InfixParser.Pseudo>>) InfixParser.cases@75-2).GetEnumerator();
                        this.pc = 1;
                        break;
                }
                if (this.@enum.MoveNext())
                {
                    this.matchValue = this.@enum.Current;
                    this.union = this.matchValue.Item2;
                    this.name = this.matchValue.Item1;
                    this.pc = 2;
                    this.current = Primitives.op_BarGreaterGreater<string, Unit, InfixParser.Pseudo>(Primitives.op_DotGreaterGreater<string, Unit, Unit>(CharParsers.pstring<Unit>(this.name), CharParsers.spaces<Unit>()), new InfixParser.pseudoName@81-1(this.union));
                    return 1;
                }
            Label_00DC:
                this.pc = 3;
                LanguagePrimitives.IntrinsicFunctions.Dispose<IEnumerator<Tuple<string, InfixParser.Pseudo>>>(this.@enum);
                this.@enum = null;
                this.pc = 3;
            Label_00FD:
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
            public override FSharpFunc<CharStream<Unit>, Reply<InfixParser.Pseudo>> LastGenerated => 
                this.current;

            [CompilerGenerated]
            public override IEnumerator<FSharpFunc<CharStream<Unit>, Reply<InfixParser.Pseudo>>> GetFreshEnumerator() => 
                new InfixParser.pseudoName@81(null, null, null, null, 0, null);
        }

        [Serializable]
        internal class pseudoName@81-1 : FSharpFunc<string, InfixParser.Pseudo>
        {
            public InfixParser.Pseudo union;

            internal pseudoName@81-1(InfixParser.Pseudo union)
            {
                this.union = union;
            }

            public override InfixParser.Pseudo Invoke(string _arg2) => 
                this.union;
        }

        [Serializable]
        internal class pseudoName@81-2 : FSharpFunc<CharStream<Unit>, Reply<InfixParser.Pseudo>>
        {
            internal pseudoName@81-2()
            {
            }

            public override Reply<InfixParser.Pseudo> Invoke(CharStream<Unit> arg10) => 
                InfixParser.clo1@81-1.Invoke(arg10);
        }

        [Serializable]
        internal class pseudoTerm@100-1 : FSharpFunc<Tuple<InfixParser.Pseudo, FSharpList<Expression>>, Expression>
        {
            internal pseudoTerm@100-1()
            {
            }

            public override Expression Invoke(Tuple<InfixParser.Pseudo, FSharpList<Expression>> tupledArg)
            {
                InfixParser.Pseudo pseudo = tupledArg.Item1;
                FSharpList<Expression> list = tupledArg.Item2;
                return InfixParser.applyPseudo(pseudo, list);
            }
        }
    }
}

