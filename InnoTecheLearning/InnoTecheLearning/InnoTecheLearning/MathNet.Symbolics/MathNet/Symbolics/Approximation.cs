namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    [Serializable, DebuggerDisplay("{__DebugDisplay(),nq}"), CompilationMapping(SourceConstructFlags.SumType)]
    public abstract class Approximation : IEquatable<MathNet.Symbolics.Approximation>, IStructuralEquatable
    {
        [CompilerGenerated]
        internal Approximation()
        {
        }

        [CompilerGenerated]
        internal object __DebugDisplay() => 
            ExtraTopLevelOperators.PrintFormatToString<FSharpFunc<MathNet.Symbolics.Approximation, string>>(new PrintfFormat<FSharpFunc<MathNet.Symbolics.Approximation, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);

        [CompilerGenerated]
        public sealed override bool Equals(MathNet.Symbolics.Approximation obj)
        {
            if (this <= null)
            {
                return (obj <= null);
            }
            if (obj <= null)
            {
                return false;
            }
            MathNet.Symbolics.Approximation approximation = this;
            int num = !(approximation is Complex) ? 0 : 1;
            MathNet.Symbolics.Approximation approximation2 = obj;
            int num2 = !(approximation2 is Complex) ? 0 : 1;
            if (num != num2)
            {
                return false;
            }
            if (this is Real)
            {
                Real real = (Real) this;
                Real real2 = (Real) obj;
                double item = real.item;
                double num4 = real2.item;
                return (((item != item) && !(num4 == num4)) || (item == num4));
            }
            Complex complex = (Complex) this;
            Complex complex2 = (Complex) obj;
            return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic<System.Numerics.Complex>(complex.item, complex2.item);
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj)
        {
            MathNet.Symbolics.Approximation approximation = obj as MathNet.Symbolics.Approximation;
            return ((approximation != null) && this.Equals(approximation));
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj, IEqualityComparer comp)
        {
            if (this <= null)
            {
                return (obj <= null);
            }
            MathNet.Symbolics.Approximation approximation = obj as MathNet.Symbolics.Approximation;
            if (approximation == null)
            {
                return false;
            }
            MathNet.Symbolics.Approximation approximation2 = this;
            int num = !(approximation2 is Complex) ? 0 : 1;
            MathNet.Symbolics.Approximation approximation3 = approximation;
            int num2 = !(approximation3 is Complex) ? 0 : 1;
            if (num != num2)
            {
                return false;
            }
            if (this is Real)
            {
                Real real = (Real) this;
                Real real2 = (Real) approximation;
                return (real.item == real2.item);
            }
            Complex complex = (Complex) this;
            Complex complex2 = (Complex) approximation;
            return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic<System.Numerics.Complex>(comp, complex.item, complex2.item);
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
            if (this is Real)
            {
                Real real = (Real) this;
                num = 0;
                return (-1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic<double>(comp, real.item) + ((num << 6) + (num >> 2))));
            }
            Complex complex = (Complex) this;
            num = 1;
            return (-1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic<System.Numerics.Complex>(comp, complex.item) + ((num << 6) + (num >> 2))));
        }

        [CompilationMapping(SourceConstructFlags.UnionCase, 1)]
        public static MathNet.Symbolics.Approximation NewComplex(System.Numerics.Complex item) => 
            new Complex(item);

        [CompilationMapping(SourceConstructFlags.UnionCase, 0)]
        public static MathNet.Symbolics.Approximation NewReal(double item) => 
            new Real(item);

        public static implicit operator MathNet.Symbolics.Approximation(double x) => 
            NewReal(x);

        public static implicit operator MathNet.Symbolics.Approximation(System.Numerics.Complex x) => 
            NewComplex(x);

        public System.Numerics.Complex ComplexValue
        {
            get
            {
                if (this is Complex)
                {
                    return ((Complex) this).item;
                }
                Real real = (Real) this;
                return new System.Numerics.Complex(real.item, 0.0);
            }
        }

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsComplex =>
            (this is Complex);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsReal =>
            (this is Real);

        public double RealValue
        {
            get
            {
                if (!(this is Complex))
                {
                    return ((Real) this).item;
                }
                Complex complex = (Complex) this;
                if (!complex.item.IsReal())
                {
                    throw new Exception("Value not convertible to a real number.");
                }
                System.Numerics.Complex item = complex.item;
                return item.Real;
            }
        }

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Tag
        {
            [CompilerGenerated]
            get
            {
                MathNet.Symbolics.Approximation approximation = this;
                return (!(approximation is Complex) ? 0 : 1);
            }
        }

        [Serializable, DebuggerTypeProxy(typeof(MathNet.Symbolics.Approximation.Complex@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Complex : MathNet.Symbolics.Approximation
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly System.Numerics.Complex item;

            [CompilerGenerated]
            internal Complex(System.Numerics.Complex item)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 1, 0), CompilerGenerated]
            public System.Numerics.Complex Item =>
                this.item;
        }

        internal class Complex@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal MathNet.Symbolics.Approximation.Complex _obj;

            [CompilerGenerated]
            public Complex@DebugTypeProxy(MathNet.Symbolics.Approximation.Complex obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 1, 0), CompilerGenerated]
            public System.Numerics.Complex Item =>
                this._obj.item;
        }

        [Serializable, DebuggerTypeProxy(typeof(MathNet.Symbolics.Approximation.Real@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Real : MathNet.Symbolics.Approximation
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly double item;

            [CompilerGenerated]
            internal Real(double item)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 0, 0), CompilerGenerated]
            public double Item =>
                this.item;
        }

        internal class Real@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal MathNet.Symbolics.Approximation.Real _obj;

            [CompilerGenerated]
            public Real@DebugTypeProxy(MathNet.Symbolics.Approximation.Real obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 0, 0), CompilerGenerated]
            public double Item =>
                this._obj.item;
        }

        public static class Tags
        {
            public const int Complex = 1;
            public const int Real = 0;
        }
    }
}

