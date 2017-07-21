namespace MathNet.Symbolics
{
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [Serializable, DebuggerDisplay("{__DebugDisplay(),nq}"), CompilationMapping(SourceConstructFlags.SumType)]
    public sealed class Symbol : IEquatable<Symbol>, IStructuralEquatable, IComparable<Symbol>, IComparable, IStructuralComparable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal readonly string item;

        [CompilerGenerated]
        internal Symbol(string item)
        {
            this.item = item;
        }

        [CompilerGenerated]
        internal object __DebugDisplay() => 
            ExtraTopLevelOperators.PrintFormatToString<FSharpFunc<Symbol, string>>(new PrintfFormat<FSharpFunc<Symbol, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);

        [CompilerGenerated]
        public sealed override int CompareTo(Symbol obj)
        {
            if (this > null)
            {
                if (obj > null)
                {
                    Symbol symbol = this;
                    Symbol symbol2 = obj;
                    IComparer genericComparer = LanguagePrimitives.GenericComparer;
                    return string.CompareOrdinal(symbol.item, symbol2.item);
                }
                return 1;
            }
            if (obj > null)
            {
                return -1;
            }
            return 0;
        }

        [CompilerGenerated]
        public sealed override int CompareTo(object obj) => 
            this.CompareTo((Symbol) obj);

        [CompilerGenerated]
        public sealed override int CompareTo(object obj, IComparer comp)
        {
            Symbol symbol = (Symbol) obj;
            if (this > null)
            {
                if (((Symbol) obj) > null)
                {
                    Symbol symbol2 = this;
                    Symbol symbol3 = symbol;
                    return string.CompareOrdinal(symbol2.item, symbol3.item);
                }
                return 1;
            }
            if (((Symbol) obj) > null)
            {
                return -1;
            }
            return 0;
        }

        [CompilerGenerated]
        public sealed override bool Equals(Symbol obj)
        {
            if (this <= null)
            {
                return (obj <= null);
            }
            if (obj > null)
            {
                Symbol symbol = this;
                Symbol symbol2 = obj;
                return string.Equals(symbol.item, symbol2.item);
            }
            return false;
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj)
        {
            Symbol symbol = obj as Symbol;
            return ((symbol != null) && this.Equals(symbol));
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj, IEqualityComparer comp)
        {
            if (this <= null)
            {
                return (obj <= null);
            }
            Symbol symbol = obj as Symbol;
            if (symbol != null)
            {
                Symbol symbol2 = this;
                Symbol symbol3 = symbol;
                return string.Equals(symbol2.item, symbol3.item);
            }
            return false;
        }

        [CompilerGenerated]
        public sealed override int GetHashCode() => 
            this.GetHashCode(LanguagePrimitives.GenericEqualityComparer);

        [CompilerGenerated]
        public sealed override int GetHashCode(IEqualityComparer comp)
        {
            if (this > null)
            {
                int num = 0;
                Symbol symbol = this;
                num = 0;
                string item = symbol.item;
                return (-1640531527 + (((item == null) ? 0 : item.GetHashCode()) + ((num << 6) + (num >> 2))));
            }
            return 0;
        }

        [CompilationMapping(SourceConstructFlags.UnionCase, 0)]
        public static Symbol NewSymbol(string item) => 
            new Symbol(item);

        [CompilationMapping(SourceConstructFlags.Field, 0, 0), CompilerGenerated]
        public string Item =>
            this.item;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Tag =>
            0;
    }
}

