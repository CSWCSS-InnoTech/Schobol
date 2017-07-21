namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [Serializable, RequireQualifiedAccess, DebuggerDisplay("{__DebugDisplay(),nq}"), CompilationMapping(SourceConstructFlags.SumType)]
    public class Value : IEquatable<Value>, IStructuralEquatable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal readonly int _tag;
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly Value _unique_ComplexInfinity = new Value(2);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly Value _unique_NegativeInfinity = new Value(4);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly Value _unique_PositiveInfinity = new Value(3);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly Value _unique_Undefined = new Value(5);

        [CompilerGenerated]
        internal Value(int _tag)
        {
            this._tag = _tag;
        }

        [CompilerGenerated]
        internal object __DebugDisplay() => 
            ExtraTopLevelOperators.PrintFormatToString<FSharpFunc<Value, string>>(new PrintfFormat<FSharpFunc<Value, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);

        [CompilerGenerated]
        public sealed override bool Equals(Value obj)
        {
            if (this <= null)
            {
                return (obj <= null);
            }
            if (obj <= null)
            {
                return false;
            }
            int num = this._tag;
            int num2 = obj._tag;
            if (num != num2)
            {
                return false;
            }
            switch (this.Tag)
            {
                case 0:
                {
                    Number number = (Number) this;
                    Number number2 = (Number) obj;
                    return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic<BigRational>(number.item, number2.item);
                }
                case 1:
                {
                    Approximation approximation = (Approximation) this;
                    Approximation approximation2 = (Approximation) obj;
                    return approximation.item.Equals(approximation2.item);
                }
            }
            return true;
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj)
        {
            Value value2 = obj as Value;
            return ((value2 != null) && this.Equals(value2));
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj, IEqualityComparer comp)
        {
            if (this <= null)
            {
                return (obj <= null);
            }
            Value value2 = obj as Value;
            if (value2 == null)
            {
                return false;
            }
            int num = this._tag;
            int num2 = value2._tag;
            if (num != num2)
            {
                return false;
            }
            switch (this.Tag)
            {
                case 0:
                {
                    Number number = (Number) this;
                    Number number2 = (Number) value2;
                    return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic<BigRational>(comp, number.item, number2.item);
                }
                case 1:
                {
                    Approximation approximation = (Approximation) this;
                    Approximation approximation2 = (Approximation) value2;
                    MathNet.Symbolics.Approximation item = approximation.item;
                    MathNet.Symbolics.Approximation approximation4 = approximation2.item;
                    return item.Equals(approximation4, comp);
                }
            }
            return true;
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
            switch (this.Tag)
            {
                case 1:
                {
                    Approximation approximation = (Approximation) this;
                    num = 1;
                    return (-1640531527 + (approximation.item.GetHashCode(comp) + ((num << 6) + (num >> 2))));
                }
                case 2:
                    return 2;

                case 3:
                    return 3;

                case 4:
                    return 4;

                case 5:
                    return 5;
            }
            Number number = (Number) this;
            num = 0;
            return (-1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic<BigRational>(comp, number.item) + ((num << 6) + (num >> 2))));
        }

        [CompilationMapping(SourceConstructFlags.UnionCase, 1)]
        public static Value NewApproximation(MathNet.Symbolics.Approximation item) => 
            new Approximation(item);

        [CompilationMapping(SourceConstructFlags.UnionCase, 0)]
        public static Value NewNumber(BigRational item) => 
            new Number(item);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Value ComplexInfinity =>
            _unique_ComplexInfinity;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsApproximation =>
            (this.Tag == 1);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsComplexInfinity =>
            (this.Tag == 2);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsNegativeInfinity =>
            (this.Tag == 4);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsNumber =>
            (this.Tag == 0);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsPositiveInfinity =>
            (this.Tag == 3);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsUndefined =>
            (this.Tag == 5);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Value NegativeInfinity =>
            _unique_NegativeInfinity;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Value PositiveInfinity =>
            _unique_PositiveInfinity;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Tag =>
            this._tag;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Value Undefined =>
            _unique_Undefined;

        [Serializable, DebuggerTypeProxy(typeof(Value.Approximation@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Approximation : Value
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly MathNet.Symbolics.Approximation item;

            [CompilerGenerated]
            internal Approximation(MathNet.Symbolics.Approximation item) : base(1)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 1, 0), CompilerGenerated]
            public MathNet.Symbolics.Approximation Item =>
                this.item;
        }

        internal class Approximation@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal Value.Approximation _obj;

            [CompilerGenerated]
            public Approximation@DebugTypeProxy(Value.Approximation obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 1, 0), CompilerGenerated]
            public MathNet.Symbolics.Approximation Item =>
                this._obj.item;
        }

        [Serializable, DebuggerTypeProxy(typeof(Value.Number@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Number : Value
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly BigRational item;

            [CompilerGenerated]
            internal Number(BigRational item) : base(0)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 0, 0), CompilerGenerated]
            public BigRational Item =>
                this.item;
        }

        internal class Number@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal Value.Number _obj;

            [CompilerGenerated]
            public Number@DebugTypeProxy(Value.Number obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 0, 0), CompilerGenerated]
            public BigRational Item =>
                this._obj.item;
        }

        public static class Tags
        {
            public const int Approximation = 1;
            public const int ComplexInfinity = 2;
            public const int NegativeInfinity = 4;
            public const int Number = 0;
            public const int PositiveInfinity = 3;
            public const int Undefined = 5;
        }
    }
}

