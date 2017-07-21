namespace MathNet.Symbolics
{
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [Serializable, DebuggerDisplay("{__DebugDisplay(),nq}"), CompilationMapping(SourceConstructFlags.SumType)]
    public sealed class Constant : IEquatable<MathNet.Symbolics.Constant>, IStructuralEquatable, IComparable<MathNet.Symbolics.Constant>, IComparable, IStructuralComparable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal readonly int _tag;
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Constant _unique_E = new MathNet.Symbolics.Constant(0);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Constant _unique_I = new MathNet.Symbolics.Constant(2);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Constant _unique_Pi = new MathNet.Symbolics.Constant(1);

        [CompilerGenerated]
        internal Constant(int _tag)
        {
            this._tag = _tag;
        }

        [CompilerGenerated]
        internal object __DebugDisplay() => 
            ExtraTopLevelOperators.PrintFormatToString<FSharpFunc<MathNet.Symbolics.Constant, string>>(new PrintfFormat<FSharpFunc<MathNet.Symbolics.Constant, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);

        [CompilerGenerated]
        public sealed override int CompareTo(MathNet.Symbolics.Constant obj)
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
            this.CompareTo((MathNet.Symbolics.Constant) obj);

        [CompilerGenerated]
        public sealed override int CompareTo(object obj, IComparer comp)
        {
            MathNet.Symbolics.Constant constant = (MathNet.Symbolics.Constant) obj;
            if (this > null)
            {
                if (((MathNet.Symbolics.Constant) obj) <= null)
                {
                    return 1;
                }
                int num = this._tag;
                int num2 = constant._tag;
                if (num == num2)
                {
                    return 0;
                }
                return (num - num2);
            }
            if (((MathNet.Symbolics.Constant) obj) > null)
            {
                return -1;
            }
            return 0;
        }

        [CompilerGenerated]
        public sealed override bool Equals(MathNet.Symbolics.Constant obj)
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
            MathNet.Symbolics.Constant constant = obj as MathNet.Symbolics.Constant;
            return ((constant != null) && this.Equals(constant));
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj, IEqualityComparer comp)
        {
            if (this <= null)
            {
                return (obj <= null);
            }
            MathNet.Symbolics.Constant constant = obj as MathNet.Symbolics.Constant;
            if (constant != null)
            {
                int num = this._tag;
                int num2 = constant._tag;
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
            if (this > null)
            {
                switch (this.Tag)
                {
                    case 1:
                        return 1;

                    case 2:
                        return 2;
                }
            }
            return 0;
        }

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Constant E =>
            _unique_E;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Constant I =>
            _unique_I;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsE =>
            (this.Tag == 0);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsI =>
            (this.Tag == 2);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsPi =>
            (this.Tag == 1);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Constant Pi =>
            _unique_Pi;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Tag =>
            this._tag;

        public static class Tags
        {
            public const int E = 0;
            public const int I = 2;
            public const int Pi = 1;
        }
    }
}

