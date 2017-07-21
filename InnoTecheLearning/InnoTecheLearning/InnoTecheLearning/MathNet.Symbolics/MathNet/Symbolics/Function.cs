namespace MathNet.Symbolics
{
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [Serializable, DebuggerDisplay("{__DebugDisplay(),nq}"), CompilationMapping(SourceConstructFlags.SumType)]
    public sealed class Function : IEquatable<MathNet.Symbolics.Function>, IStructuralEquatable, IComparable<MathNet.Symbolics.Function>, IComparable, IStructuralComparable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal readonly int _tag;
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Abs = new MathNet.Symbolics.Function(0);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Acos = new MathNet.Symbolics.Function(13);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Asin = new MathNet.Symbolics.Function(12);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Atan = new MathNet.Symbolics.Function(14);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Cos = new MathNet.Symbolics.Function(4);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Cosh = new MathNet.Symbolics.Function(9);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Cot = new MathNet.Symbolics.Function(6);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Csc = new MathNet.Symbolics.Function(8);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Exp = new MathNet.Symbolics.Function(2);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Ln = new MathNet.Symbolics.Function(1);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Sec = new MathNet.Symbolics.Function(7);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Sin = new MathNet.Symbolics.Function(3);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Sinh = new MathNet.Symbolics.Function(10);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Tan = new MathNet.Symbolics.Function(5);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly MathNet.Symbolics.Function _unique_Tanh = new MathNet.Symbolics.Function(11);

        [CompilerGenerated]
        internal Function(int _tag)
        {
            this._tag = _tag;
        }

        [CompilerGenerated]
        internal object __DebugDisplay() => 
            ExtraTopLevelOperators.PrintFormatToString<FSharpFunc<MathNet.Symbolics.Function, string>>(new PrintfFormat<FSharpFunc<MathNet.Symbolics.Function, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);

        [CompilerGenerated]
        public sealed override int CompareTo(MathNet.Symbolics.Function obj)
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
            this.CompareTo((MathNet.Symbolics.Function) obj);

        [CompilerGenerated]
        public sealed override int CompareTo(object obj, IComparer comp)
        {
            MathNet.Symbolics.Function function = (MathNet.Symbolics.Function) obj;
            if (this > null)
            {
                if (((MathNet.Symbolics.Function) obj) <= null)
                {
                    return 1;
                }
                int num = this._tag;
                int num2 = function._tag;
                if (num == num2)
                {
                    return 0;
                }
                return (num - num2);
            }
            if (((MathNet.Symbolics.Function) obj) > null)
            {
                return -1;
            }
            return 0;
        }

        [CompilerGenerated]
        public sealed override bool Equals(MathNet.Symbolics.Function obj)
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
            MathNet.Symbolics.Function function = obj as MathNet.Symbolics.Function;
            return ((function != null) && this.Equals(function));
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj, IEqualityComparer comp)
        {
            if (this <= null)
            {
                return (obj <= null);
            }
            MathNet.Symbolics.Function function = obj as MathNet.Symbolics.Function;
            if (function != null)
            {
                int num = this._tag;
                int num2 = function._tag;
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

                    case 3:
                        return 3;

                    case 4:
                        return 4;

                    case 5:
                        return 5;

                    case 6:
                        return 6;

                    case 7:
                        return 7;

                    case 8:
                        return 8;

                    case 9:
                        return 9;

                    case 10:
                        return 10;

                    case 11:
                        return 11;

                    case 12:
                        return 12;

                    case 13:
                        return 13;

                    case 14:
                        return 14;
                }
            }
            return 0;
        }

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Abs =>
            _unique_Abs;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Acos =>
            _unique_Acos;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Asin =>
            _unique_Asin;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Atan =>
            _unique_Atan;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Cos =>
            _unique_Cos;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Cosh =>
            _unique_Cosh;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Cot =>
            _unique_Cot;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Csc =>
            _unique_Csc;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Exp =>
            _unique_Exp;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsAbs =>
            (this.Tag == 0);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsAcos =>
            (this.Tag == 13);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsAsin =>
            (this.Tag == 12);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsAtan =>
            (this.Tag == 14);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsCos =>
            (this.Tag == 4);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsCosh =>
            (this.Tag == 9);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsCot =>
            (this.Tag == 6);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsCsc =>
            (this.Tag == 8);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsExp =>
            (this.Tag == 2);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsLn =>
            (this.Tag == 1);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsSec =>
            (this.Tag == 7);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsSin =>
            (this.Tag == 3);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsSinh =>
            (this.Tag == 10);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsTan =>
            (this.Tag == 5);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsTanh =>
            (this.Tag == 11);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Ln =>
            _unique_Ln;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Sec =>
            _unique_Sec;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Sin =>
            _unique_Sin;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Sinh =>
            _unique_Sinh;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Tag =>
            this._tag;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Tan =>
            _unique_Tan;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static MathNet.Symbolics.Function Tanh =>
            _unique_Tanh;

        public static class Tags
        {
            public const int Abs = 0;
            public const int Acos = 13;
            public const int Asin = 12;
            public const int Atan = 14;
            public const int Cos = 4;
            public const int Cosh = 9;
            public const int Cot = 6;
            public const int Csc = 8;
            public const int Exp = 2;
            public const int Ln = 1;
            public const int Sec = 7;
            public const int Sin = 3;
            public const int Sinh = 10;
            public const int Tan = 5;
            public const int Tanh = 11;
        }
    }
}

