namespace MathNet.Symbolics
{
    using Microsoft.FSharp.Core;
    using System;
    using System.IO;
    using System.Text;

    [RequireQualifiedAccess, CompilationMapping(SourceConstructFlags.Module)]
    public static class LaTeX
    {
        [CompilationSourceName("format")]
        public static string Format(Expression expression)
        {
            StringBuilder sb = new StringBuilder();
            LaTeXFormatter.tex(new Format@199-1(sb), 0, expression);
            return sb.ToString();
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("formatWriter")]
        public static void FormatWriter(TextWriter writer, Expression expression)
        {
            LaTeXFormatter.tex(new FormatWriter@208-1(writer), 0, expression);
        }

        [Obsolete("Use Format instead"), CompilationSourceName("print")]
        public static string Print(Expression q) => 
            Format(q);

        [Obsolete("Use FormatWriter instead"), CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("printTextWriter")]
        public static void PrintToTextWriter(TextWriter writer, Expression q)
        {
            FormatWriter(writer, q);
        }

        [Serializable]
        internal class Format@199-1 : FSharpFunc<string, Unit>
        {
            public StringBuilder sb;

            internal Format@199-1(StringBuilder sb)
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
        internal class FormatWriter@208-1 : FSharpFunc<string, Unit>
        {
            public TextWriter writer;

            internal FormatWriter@208-1(TextWriter writer)
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

