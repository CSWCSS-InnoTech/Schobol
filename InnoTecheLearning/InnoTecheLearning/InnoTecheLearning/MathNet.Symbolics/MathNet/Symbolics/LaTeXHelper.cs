namespace MathNet.Symbolics
{
    using Microsoft.FSharp.Core;
    using System;
    using System.Text;

    [CompilationMapping(SourceConstructFlags.Module)]
    internal static class LaTeXHelper
    {
        internal static string addBracets(string str)
        {
            StringBuilder builder2;
            StringBuilder builder = new StringBuilder();
            int count = 0;
            int num3 = 0;
            int num2 = str.Length - 1;
            if (num2 >= num3)
            {
                do
                {
                    char ch = str[num3];
                    builder2 = builder.Append(ch);
                    if (ch == '_')
                    {
                        builder2 = builder.Append('{');
                        count++;
                    }
                    num3++;
                }
                while (num3 != (num2 + 1));
            }
            builder2 = builder.Append(new string('}', count));
            return builder.ToString();
        }
    }
}

