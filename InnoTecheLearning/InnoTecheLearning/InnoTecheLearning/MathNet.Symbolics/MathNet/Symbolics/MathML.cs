namespace MathNet.Symbolics
{
    using Microsoft.FSharp.Core;
    using System;
    using System.IO;
    using System.Xml.Linq;

    [RequireQualifiedAccess, CompilationMapping(SourceConstructFlags.Module)]
    public static class MathML
    {
        [CompilationSourceName("formatContentStrict")]
        public static string FormatContentStrict(Expression expression) => 
            MathMLFormatter.formatContentStrict(expression).ToString();

        [CompilationSourceName("formatContentStrictXml")]
        public static XElement FormatContentStrictXml(Expression expression) => 
            MathMLFormatter.formatContentStrict(expression);

        [CompilationSourceName("formatSemanticsAnnotated")]
        public static string FormatSemanticsAnnotated(Expression expression) => 
            MathMLFormatter.formatSemanticsAnnotated(expression).ToString();

        [CompilationSourceName("formatSemanticsAnnotatedXml")]
        public static XElement FormatSemanticsAnnotatedXml(Expression expression) => 
            MathMLFormatter.formatSemanticsAnnotated(expression);

        [CompilationSourceName("parse")]
        public static Expression Parse(string text) => 
            MathMLParser.parse(Xml.ofString(text));

        [CompilationSourceName("parseReader")]
        public static Expression ParseReader(TextReader reader) => 
            MathMLParser.parse(XDocument.Load(reader).Root);

        [CompilationSourceName("parseXml")]
        public static Expression ParseXml(XElement xml) => 
            MathMLParser.parse(xml);
    }
}

