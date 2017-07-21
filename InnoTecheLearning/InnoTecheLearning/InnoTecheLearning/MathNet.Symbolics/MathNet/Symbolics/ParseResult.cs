namespace MathNet.Symbolics
{
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [Serializable, DebuggerDisplay("{__DebugDisplay(),nq}"), CompilationMapping(SourceConstructFlags.SumType)]
    public abstract class ParseResult : IEquatable<ParseResult>, IStructuralEquatable
    {
        [CompilerGenerated]
        internal ParseResult()
        {
        }

        [CompilerGenerated]
        internal object __DebugDisplay() => 
            ExtraTopLevelOperators.PrintFormatToString<FSharpFunc<ParseResult, string>>(new PrintfFormat<FSharpFunc<ParseResult, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);

        [CompilerGenerated]
        public sealed override bool Equals(ParseResult obj)
        {
            if (this <= null)
            {
                return (obj <= null);
            }
            if (obj <= null)
            {
                return false;
            }
            ParseResult result = this;
            int num = !(result is ParseFailure) ? 0 : 1;
            ParseResult result2 = obj;
            int num2 = !(result2 is ParseFailure) ? 0 : 1;
            if (num != num2)
            {
                return false;
            }
            if (this is ParsedExpression)
            {
                ParsedExpression expression = (ParsedExpression) this;
                ParsedExpression expression2 = (ParsedExpression) obj;
                return expression.item.Equals(expression2.item);
            }
            ParseFailure failure = (ParseFailure) this;
            ParseFailure failure2 = (ParseFailure) obj;
            return string.Equals(failure.item, failure2.item);
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj)
        {
            ParseResult result = obj as ParseResult;
            return ((result != null) && this.Equals(result));
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj, IEqualityComparer comp)
        {
            if (this <= null)
            {
                return (obj <= null);
            }
            ParseResult result = obj as ParseResult;
            if (result == null)
            {
                return false;
            }
            ParseResult result2 = this;
            int num = !(result2 is ParseFailure) ? 0 : 1;
            ParseResult result3 = result;
            int num2 = !(result3 is ParseFailure) ? 0 : 1;
            if (num != num2)
            {
                return false;
            }
            if (this is ParsedExpression)
            {
                ParsedExpression expression = (ParsedExpression) this;
                ParsedExpression expression2 = (ParsedExpression) result;
                Expression item = expression.item;
                Expression expression4 = expression2.item;
                return item.Equals(expression4, comp);
            }
            ParseFailure failure = (ParseFailure) this;
            ParseFailure failure2 = (ParseFailure) result;
            return string.Equals(failure.item, failure2.item);
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
            int num = 0;
            if (this is ParsedExpression)
            {
                ParsedExpression expression = (ParsedExpression) this;
                num = 0;
                return (-1640531527 + (expression.item.GetHashCode(comp) + ((num << 6) + (num >> 2))));
            }
            ParseFailure failure = (ParseFailure) this;
            num = 1;
            string item = failure.item;
            return (-1640531527 + (((item == null) ? 0 : item.GetHashCode()) + ((num << 6) + (num >> 2))));
        }

        [CompilationMapping(SourceConstructFlags.UnionCase, 0)]
        public static ParseResult NewParsedExpression(Expression item) => 
            new ParsedExpression(item);

        [CompilationMapping(SourceConstructFlags.UnionCase, 1)]
        public static ParseResult NewParseFailure(string item) => 
            new ParseFailure(item);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsParsedExpression =>
            (this is ParsedExpression);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsParseFailure =>
            (this is ParseFailure);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Tag
        {
            [CompilerGenerated]
            get
            {
                ParseResult result = this;
                return (!(result is ParseFailure) ? 0 : 1);
            }
        }

        [Serializable, DebuggerTypeProxy(typeof(ParseResult.ParsedExpression@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class ParsedExpression : ParseResult
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly Expression item;

            [CompilerGenerated]
            internal ParsedExpression(Expression item)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 0, 0), CompilerGenerated]
            public Expression Item =>
                this.item;
        }

        internal class ParsedExpression@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal ParseResult.ParsedExpression _obj;

            [CompilerGenerated]
            public ParsedExpression@DebugTypeProxy(ParseResult.ParsedExpression obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 0, 0), CompilerGenerated]
            public Expression Item =>
                this._obj.item;
        }

        [Serializable, DebuggerTypeProxy(typeof(ParseResult.ParseFailure@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class ParseFailure : ParseResult
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly string item;

            [CompilerGenerated]
            internal ParseFailure(string item)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 1, 0), CompilerGenerated]
            public string Item =>
                this.item;
        }

        internal class ParseFailure@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal ParseResult.ParseFailure _obj;

            [CompilerGenerated]
            public ParseFailure@DebugTypeProxy(ParseResult.ParseFailure obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 1, 0), CompilerGenerated]
            public string Item =>
                this._obj.item;
        }

        public static class Tags
        {
            public const int ParsedExpression = 0;
            public const int ParseFailure = 1;
        }
    }
}

