namespace <StartupCode$MathNet-Symbolics>
{
    using FParsec;
    using MathNet.Symbolics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using Microsoft.FSharp.Reflection;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    internal static class $Infix
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<Expression>> absTerm@95;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly Tuple<string, MathNet.Symbolics.Function>[] cases@61;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly Tuple<string, InfixParser.Pseudo>[] cases@75-1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<MathNet.Symbolics.Function>> clo1@67;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<InfixParser.Pseudo>> clo1@81-1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly CultureInfo culture@127;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<Expression>> expr@92;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<Expression>> expression@89;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<FSharpList<Expression>>> functionArgs@97;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<MathNet.Symbolics.Function>> functionName@60;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<Expression>> functionTerm@98;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<Expression>> identifier@45 = Primitives.op_BarGreaterGreater<string, Unit, Expression>(Primitives.op_DotGreaterGreater<string, Unit, Unit>(CharParsers.many1Satisfy2L<Unit>(InfixParser.isIdentifierFirstChar@47, InfixParser.isIdentifierChar@48, "identifier"), CharParsers.spaces<Unit>()), new InfixParser.identifier@50());
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated, DebuggerNonUserCode]
        internal static int init@;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<char, bool> isIdentifierChar@48 = new InfixParser.isIdentifierChar@48-1();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<char, bool> isIdentifierFirstChar@47 = new InfixParser.isIdentifierFirstChar@47-1();
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly int len@1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly int len@1-1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<Expression>> number@32-2 = Primitives.op_BarGreaterGreater<CharParsers.NumberLiteral, Unit, Expression>(Primitives.op_DotGreaterGreater<CharParsers.NumberLiteral, Unit, Unit>(CharParsers.numberLiteral<Unit>(CharParsers.NumberLiteralOptions.AllowInfinity | CharParsers.NumberLiteralOptions.AllowExponent | CharParsers.NumberLiteralOptions.AllowFractionWOIntegerPart | CharParsers.NumberLiteralOptions.AllowFraction, "number"), CharParsers.spaces<Unit>()), new InfixParser.number@40-1());
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly OperatorPrecedenceParser<Expression, Unit, Unit> opp@91;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<Expression>> parensTerm@94;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<Expression>> parser@114;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<InfixParser.Pseudo>> pseudoName@73;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<Expression>> pseudoTerm@100;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly Tuple<string, MathNet.Symbolics.Function>[] res@1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly Tuple<string, InfixParser.Pseudo>[] res@1-1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<Expression>> term@102;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly FSharpFunc<CharStream<Unit>, Reply<Expression>> value@58 = Primitives.op_LessBarGreater<Expression, Unit>(InfixParser.number, InfixParser.identifier);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly UnionCaseInfo[] x@1 = FSharpType.GetUnionCases(typeof(MathNet.Symbolics.Function), null);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal static readonly UnionCaseInfo[] x@1-1;

        static $Infix()
        {
            if (InfixParser.x@1 == null)
            {
                throw new ArgumentNullException("array");
            }
            len@1 = InfixParser.x@1.Length;
            res@1 = new Tuple<string, MathNet.Symbolics.Function>[InfixParser.len@1];
            FSharpFunc<Tuple<string, MathNet.Symbolics.Function>, int> projection = new InfixParser.cases@65-1();
            int index = 0;
            int num = InfixParser.len@1 - 1;
            if (num >= index)
            {
                do
                {
                    InfixParser.res@1[index] = InfixParser.f@1-2(InfixParser.x@1[index]);
                    index++;
                }
                while (index != (num + 1));
            }
            cases@61 = ArrayModule.SortBy<Tuple<string, MathNet.Symbolics.Function>, int>(projection, InfixParser.res@1);
            clo1@67 = Primitives.choice<MathNet.Symbolics.Function, Unit>((IEnumerable<FSharpFunc<CharStream<Unit>, Reply<MathNet.Symbolics.Function>>>) SeqModule.ToList<FSharpFunc<CharStream<Unit>, Reply<MathNet.Symbolics.Function>>>(new InfixParser.functionName@67(null, null, null, null, 0, null)));
            functionName@60 = Primitives.op_DotGreaterGreater<MathNet.Symbolics.Function, Unit, Unit>(new InfixParser.functionName@67-2(), CharParsers.spaces<Unit>());
            x@1-1 = FSharpType.GetUnionCases(typeof(InfixParser.Pseudo), FSharpOption<BindingFlags>.Some(BindingFlags.NonPublic | BindingFlags.Public));
            if (InfixParser.x@1-1 == null)
            {
                throw new ArgumentNullException("array");
            }
            len@1-1 = InfixParser.x@1-1.Length;
            res@1-1 = new Tuple<string, InfixParser.Pseudo>[InfixParser.len@1-1];
            FSharpFunc<Tuple<string, InfixParser.Pseudo>, int> func2 = new InfixParser.cases@79-3();
            index = 0;
            num = InfixParser.len@1-1 - 1;
            if (num >= index)
            {
                do
                {
                    InfixParser.res@1-1[index] = InfixParser.f@1-3(InfixParser.x@1-1[index]);
                    index++;
                }
                while (index != (num + 1));
            }
            cases@75-1 = ArrayModule.SortBy<Tuple<string, InfixParser.Pseudo>, int>(func2, InfixParser.res@1-1);
            clo1@81-1 = Primitives.choice<InfixParser.Pseudo, Unit>((IEnumerable<FSharpFunc<CharStream<Unit>, Reply<InfixParser.Pseudo>>>) SeqModule.ToList<FSharpFunc<CharStream<Unit>, Reply<InfixParser.Pseudo>>>(new InfixParser.pseudoName@81(null, null, null, null, 0, null)));
            pseudoName@73 = Primitives.op_DotGreaterGreater<InfixParser.Pseudo, Unit, Unit>(new InfixParser.pseudoName@81-2(), CharParsers.spaces<Unit>());
            opp@91 = new OperatorPrecedenceParser<Expression, Unit, Unit>();
            expr@92 = InfixParser.opp@91.ExpressionParser;
            parensTerm@94 = InfixParser.parens<Expression, Unit>(InfixParser.expr@92);
            absTerm@95 = InfixParser.abs<Unit>(InfixParser.expr@92);
            functionArgs@97 = InfixParser.parens<FSharpList<Expression>, Unit>(Primitives.sepBy<Expression, Unit, string>(InfixParser.expr@92, Primitives.op_DotGreaterGreater<string, Unit, Unit>(CharParsers.pstring<Unit>(","), CharParsers.spaces<Unit>())));
            functionTerm@98 = Primitives.op_BarGreaterGreater<Tuple<MathNet.Symbolics.Function, FSharpList<Expression>>, Unit, Expression>(Primitives.op_DotGreaterGreaterDot<MathNet.Symbolics.Function, Unit, FSharpList<Expression>>(InfixParser.functionName, InfixParser.functionArgs@97), new InfixParser.functionTerm@98-1());
            pseudoTerm@100 = Primitives.op_BarGreaterGreater<Tuple<InfixParser.Pseudo, FSharpList<Expression>>, Unit, Expression>(Primitives.op_DotGreaterGreaterDot<InfixParser.Pseudo, Unit, FSharpList<Expression>>(InfixParser.pseudoName, InfixParser.functionArgs@97), new InfixParser.pseudoTerm@100-1());
            term@102 = Primitives.op_LessBarGreater<Expression, Unit>(Primitives.op_LessBarGreater<Expression, Unit>(Primitives.op_LessBarGreater<Expression, Unit>(Primitives.op_LessBarGreater<Expression, Unit>(Primitives.op_LessBarGreater<Expression, Unit>(InfixParser.number, InfixParser.parensTerm@94), InfixParser.absTerm@95), Primitives.attempt<Expression, Unit>(InfixParser.functionTerm@98)), Primitives.attempt<Expression, Unit>(InfixParser.pseudoTerm@100)), InfixParser.identifier);
            InfixParser.opp@91.TermParser = InfixParser.term@102;
            InfixParser.opp@91.AddOperator(new InfixOperator<Expression, Unit, Unit>("+", CharParsers.spaces<Unit>(), 1, Associativity.Left, new InfixParser.expression@105()));
            InfixParser.opp@91.AddOperator(new InfixOperator<Expression, Unit, Unit>("-", CharParsers.spaces<Unit>(), 1, Associativity.Left, new InfixParser.expression@106-1()));
            InfixParser.opp@91.AddOperator(new InfixOperator<Expression, Unit, Unit>("*", CharParsers.spaces<Unit>(), 2, Associativity.Left, new InfixParser.expression@107-2()));
            InfixParser.opp@91.AddOperator(new InfixOperator<Expression, Unit, Unit>("/", CharParsers.spaces<Unit>(), 2, Associativity.Left, new InfixParser.expression@108-3()));
            InfixParser.opp@91.AddOperator(new InfixOperator<Expression, Unit, Unit>("^", CharParsers.spaces<Unit>(), 3, Associativity.Right, new InfixParser.expression@109-4()));
            InfixParser.opp@91.AddOperator(new PrefixOperator<Expression, Unit, Unit>("+", CharParsers.spaces<Unit>(), 4, true, new InfixParser.expression@110-5()));
            InfixParser.opp@91.AddOperator(new PrefixOperator<Expression, Unit, Unit>("-", CharParsers.spaces<Unit>(), 4, true, new InfixParser.expression@111-6()));
            expression@89 = InfixParser.expr@92;
            parser@114 = Primitives.op_DotGreaterGreater<Expression, Unit, Unit>(Primitives.op_GreaterGreaterDot<Unit, Unit, Expression>(CharParsers.spaces<Unit>(), InfixParser.expression), CharParsers.eof<Unit>());
            culture@127 = CultureInfo.InvariantCulture;
        }
    }
}

