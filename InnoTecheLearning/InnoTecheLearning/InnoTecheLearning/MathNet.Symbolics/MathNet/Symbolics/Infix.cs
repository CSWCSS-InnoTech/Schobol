namespace MathNet.Symbolics
{
    using Microsoft.FSharp.Core;
    using System;
    using System.IO;
    using System.Text;

    [RequireQualifiedAccess, CompilationMapping(SourceConstructFlags.Module)]
    public static class Infix
    {
        [CompilationSourceName("format")]
        public static string Format(Expression expression)
        {
            StringBuilder sb = new StringBuilder();
            InfixFormatter.nice(new Format@339(sb), 0, expression);
            return sb.ToString();
        }

        [CompilationSourceName("formatStrict")]
        public static string FormatStrict(Expression expression)
        {
            StringBuilder sb = new StringBuilder();
            InfixFormatter.strict(new FormatStrict@320(sb), 0, expression);
            return sb.ToString();
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("formatStrictWriter")]
        public static void FormatStrictWriter(TextWriter writer, Expression expression)
        {
            InfixFormatter.strict(new FormatStrictWriter@329(writer), 0, expression);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("formatWriter")]
        public static void FormatWriter(TextWriter writer, Expression expression)
        {
            InfixFormatter.nice(new FormatWriter@348(writer), 0, expression);
        }

        [CompilationSourceName("parse")]
        public static ParseResult Parse(string infix) => 
            InfixParser.parse(infix);

        [CompilationSourceName("parseOrThrow")]
        public static Expression ParseOrThrow(string infix)
        {
            ParseResult result = InfixParser.parse(infix);
            if (result is ParseResult.ParseFailure)
            {
                ParseResult.ParseFailure failure = (ParseResult.ParseFailure) result;
                throw new Exception(failure.item);
            }
            return ((ParseResult.ParsedExpression) result).item;
        }

        [CompilationSourceName("parseOrUndefined")]
        public static Expression ParseOrUndefined(string infix)
        {
            ParseResult result = InfixParser.parse(infix);
            if (result is ParseResult.ParsedExpression)
            {
                return ((ParseResult.ParsedExpression) result).item;
            }
            return Expression.Undefined;
        }

        [Obsolete("Use Format instead"), CompilationSourceName("print")]
        public static string Print(Expression q) => 
            Format(q);

        [Obsolete("Use FormatStrict instead"), CompilationSourceName("printStrict")]
        public static string PrintStrict(Expression q) => 
            FormatStrict(q);

        [Obsolete("Use FormatStrictWriter instead"), CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("printStrictTextWriter")]
        public static void PrintStrictToTextWriter(TextWriter writer, Expression q)
        {
            FormatStrictWriter(writer, q);
        }

        [Obsolete("Use FormatWriter instead"), CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("printTextWriter")]
        public static void PrintToTextWriter(TextWriter writer, Expression q)
        {
            FormatWriter(writer, q);
        }

        [CompilationSourceName("tryParse")]
        public static FSharpOption<Expression> TryParse(string infix)
        {
            ParseResult result = InfixParser.parse(infix);
            if (result is ParseResult.ParsedExpression)
            {
                ParseResult.ParsedExpression expression = (ParseResult.ParsedExpression) result;
                return FSharpOption<Expression>.Some(expression.item);
            }
            return null;
        }

        [Serializable]
        internal class Format@339 : FSharpFunc<string, Unit>
        {
            public StringBuilder sb;

            internal Format@339(StringBuilder sb)
            {
                this.sb = sb;
            }

            public override Unit Invoke(string x)
            {
                StringBuilder builder = this.sb.Append(x);
                return null;
            }
        }

        [Serializable]
        internal class FormatStrict@320 : FSharpFunc<string, Unit>
        {
            public StringBuilder sb;

            internal FormatStrict@320(StringBuilder sb)
            {
                this.sb = sb;
            }

            public override Unit Invoke(string x)
            {
                StringBuilder builder = this.sb.Append(x);
                return null;
            }
        }

        [Serializable]
        internal class FormatStrictWriter@329 : FSharpFunc<string, Unit>
        {
            public TextWriter writer;

            internal FormatStrictWriter@329(TextWriter writer)
            {
                this.writer = writer;
            }

            public override Unit Invoke(string arg00)
            {
                this.writer.Write(arg00);
                return null;
            }
        }

        [Serializable]
        internal class FormatWriter@348 : FSharpFunc<string, Unit>
        {
            public TextWriter writer;

            internal FormatWriter@348(TextWriter writer)
            {
                this.writer = writer;
            }

            public override Unit Invoke(string arg00)
            {
                this.writer.Write(arg00);
                return null;
            }
        }
    }
}

