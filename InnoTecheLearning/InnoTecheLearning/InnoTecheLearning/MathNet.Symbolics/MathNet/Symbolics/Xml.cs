namespace MathNet.Symbolics
{
    using <StartupCode$MathNet-Symbolics>;
    using Microsoft.FSharp.Core;
    using System;
    using System.IO;
    using System.Xml.Linq;

    [RequireQualifiedAccess, CompilationMapping(SourceConstructFlags.Module)]
    public static class Xml
    {
        public static XElement ofReader(TextReader reader) => 
            XDocument.Load(reader).Root;

        public static XElement ofString(string text)
        {
            using (StringReader reader = new StringReader(text))
            {
                TextReader textReader = reader;
                return XDocument.Load(textReader).Root;
            }
        }

        public static string toString(XElement xml) => 
            xml.ToString();

        [CompilationMapping(SourceConstructFlags.Value)]
        public static FSharpFunc<string, string> normalizeString =>
            $MathML.normalizeString@152;

        [Serializable]
        internal class normalizeString@152 : FSharpFunc<string, string>
        {
            internal normalizeString@152()
            {
            }

            public override string Invoke(string x) => 
                Xml.ofString(x).ToString();
        }
    }
}

