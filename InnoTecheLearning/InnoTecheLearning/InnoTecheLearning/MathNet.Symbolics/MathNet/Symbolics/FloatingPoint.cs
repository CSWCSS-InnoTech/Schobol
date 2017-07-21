namespace MathNet.Symbolics
{
    using <StartupCode$MathNet-Symbolics>;
    using MathNet.Numerics;
    using MathNet.Numerics.LinearAlgebra;
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    [Serializable, NoComparison, DebuggerDisplay("{__DebugDisplay(),nq}"), CompilationMapping(SourceConstructFlags.SumType)]
    public class FloatingPoint : IEquatable<FloatingPoint>, IStructuralEquatable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal readonly int _tag;
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly FloatingPoint _unique_ComplexInf = new FloatingPoint(9);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly FloatingPoint _unique_NegInf = new FloatingPoint(8);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly FloatingPoint _unique_PosInf = new FloatingPoint(7);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly FloatingPoint _unique_Undef = new FloatingPoint(6);

        [CompilerGenerated]
        internal FloatingPoint(int _tag)
        {
            this._tag = _tag;
        }

        [CompilerGenerated]
        internal object __DebugDisplay() => 
            ExtraTopLevelOperators.PrintFormatToString<FSharpFunc<FloatingPoint, string>>(new PrintfFormat<FSharpFunc<FloatingPoint, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);

        [CompilerGenerated]
        public sealed override bool Equals(FloatingPoint obj)
        {
            if (this <= null)
            {
                return (obj <= null);
            }
            return ((obj > null) && $Evaluate.Equals$cont@11-1(this, obj, null));
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj)
        {
            FloatingPoint point = obj as FloatingPoint;
            return ((point != null) && this.Equals(point));
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj, IEqualityComparer comp)
        {
            if (this <= null)
            {
                return (obj <= null);
            }
            FloatingPoint that = obj as FloatingPoint;
            return ((that != null) && $Evaluate.Equals$cont@11(this, that, comp, null));
        }

        [CompilerGenerated]
        public sealed override int GetHashCode() => 
            this.GetHashCode(LanguagePrimitives.GenericEqualityComparer);

        [CompilerGenerated]
        public sealed override int GetHashCode(IEqualityComparer comp)
        {
            if (this > null)
            {
                return $Evaluate.GetHashCode$cont@11(comp, this, null);
            }
            return 0;
        }

        [CompilationMapping(SourceConstructFlags.UnionCase, 1)]
        public static FloatingPoint NewComplex(System.Numerics.Complex item) => 
            new Complex(item);

        [CompilationMapping(SourceConstructFlags.UnionCase, 5)]
        public static FloatingPoint NewComplexMatrix(Matrix<System.Numerics.Complex> item) => 
            new ComplexMatrix(item);

        [CompilationMapping(SourceConstructFlags.UnionCase, 3)]
        public static FloatingPoint NewComplexVector(Vector<System.Numerics.Complex> item) => 
            new ComplexVector(item);

        [CompilationMapping(SourceConstructFlags.UnionCase, 0)]
        public static FloatingPoint NewReal(double item) => 
            new Real(item);

        [CompilationMapping(SourceConstructFlags.UnionCase, 4)]
        public static FloatingPoint NewRealMatrix(Matrix<double> item) => 
            new RealMatrix(item);

        [CompilationMapping(SourceConstructFlags.UnionCase, 2)]
        public static FloatingPoint NewRealVector(Vector<double> item) => 
            new RealVector(item);

        public static implicit operator FloatingPoint(Matrix<double> x) => 
            NewRealMatrix(x);

        public static implicit operator FloatingPoint(Matrix<System.Numerics.Complex> x) => 
            NewComplexMatrix(x);

        public static implicit operator FloatingPoint(Vector<double> x) => 
            NewRealVector(x);

        public static implicit operator FloatingPoint(Vector<System.Numerics.Complex> x) => 
            NewComplexVector(x);

        public static implicit operator FloatingPoint(double x) => 
            NewReal(x);

        public static implicit operator FloatingPoint(System.Numerics.Complex x) => 
            NewComplex(x);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static FloatingPoint ComplexInf =>
            _unique_ComplexInf;

        public Matrix<System.Numerics.Complex> ComplexMatrixValue
        {
            get
            {
                if (this.Tag != 5)
                {
                    throw new Exception("Value not convertible to a complex matrix.");
                }
                return ((ComplexMatrix) this).item;
            }
        }

        public System.Numerics.Complex ComplexValue
        {
            get
            {
                switch (this.Tag)
                {
                    case 0:
                    {
                        Real real = (Real) this;
                        return new System.Numerics.Complex(real.item, 0.0);
                    }
                    case 1:
                        return ((Complex) this).item;
                }
                throw new Exception("Value not convertible to a complex number.");
            }
        }

        public Vector<System.Numerics.Complex> ComplexVectorValue
        {
            get
            {
                if (this.Tag != 3)
                {
                    throw new Exception("Value not convertible to a complex vector.");
                }
                return ((ComplexVector) this).item;
            }
        }

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsComplex =>
            (this.Tag == 1);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsComplexInf =>
            (this.Tag == 9);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsComplexMatrix =>
            (this.Tag == 5);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsComplexVector =>
            (this.Tag == 3);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsNegInf =>
            (this.Tag == 8);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsPosInf =>
            (this.Tag == 7);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsReal =>
            (this.Tag == 0);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsRealMatrix =>
            (this.Tag == 4);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsRealVector =>
            (this.Tag == 2);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsUndef =>
            (this.Tag == 6);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static FloatingPoint NegInf =>
            _unique_NegInf;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static FloatingPoint PosInf =>
            _unique_PosInf;

        public Matrix<double> RealMatrixValue
        {
            get
            {
                if (this.Tag != 4)
                {
                    throw new Exception("Value not convertible to a real matrix.");
                }
                return ((RealMatrix) this).item;
            }
        }

        public double RealValue
        {
            get
            {
                switch (this.Tag)
                {
                    case 0:
                        return ((Real) this).item;

                    case 1:
                    {
                        Complex complex = (Complex) this;
                        if (!complex.item.IsReal())
                        {
                            break;
                        }
                        return complex.item.Real;
                    }
                }
                throw new Exception("Value not convertible to a real number.");
            }
        }

        public Vector<double> RealVectorValue
        {
            get
            {
                if (this.Tag != 2)
                {
                    throw new Exception("Value not convertible to a real vector.");
                }
                return ((RealVector) this).item;
            }
        }

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Tag =>
            this._tag;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static FloatingPoint Undef =>
            _unique_Undef;

        [Serializable, DebuggerTypeProxy(typeof(FloatingPoint.Complex@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Complex : FloatingPoint
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly System.Numerics.Complex item;

            [CompilerGenerated]
            internal Complex(System.Numerics.Complex item) : base(1)
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
            internal FloatingPoint.Complex _obj;

            [CompilerGenerated]
            public Complex@DebugTypeProxy(FloatingPoint.Complex obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 1, 0), CompilerGenerated]
            public System.Numerics.Complex Item =>
                this._obj.item;
        }

        [Serializable, DebuggerTypeProxy(typeof(FloatingPoint.ComplexMatrix@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class ComplexMatrix : FloatingPoint
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly Matrix<System.Numerics.Complex> item;

            [CompilerGenerated]
            internal ComplexMatrix(Matrix<System.Numerics.Complex> item) : base(5)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 5, 0), CompilerGenerated]
            public Matrix<System.Numerics.Complex> Item =>
                this.item;
        }

        internal class ComplexMatrix@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal FloatingPoint.ComplexMatrix _obj;

            [CompilerGenerated]
            public ComplexMatrix@DebugTypeProxy(FloatingPoint.ComplexMatrix obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 5, 0), CompilerGenerated]
            public Matrix<System.Numerics.Complex> Item =>
                this._obj.item;
        }

        [Serializable, DebuggerTypeProxy(typeof(FloatingPoint.ComplexVector@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class ComplexVector : FloatingPoint
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly Vector<System.Numerics.Complex> item;

            [CompilerGenerated]
            internal ComplexVector(Vector<System.Numerics.Complex> item) : base(3)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 3, 0), CompilerGenerated]
            public Vector<System.Numerics.Complex> Item =>
                this.item;
        }

        internal class ComplexVector@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal FloatingPoint.ComplexVector _obj;

            [CompilerGenerated]
            public ComplexVector@DebugTypeProxy(FloatingPoint.ComplexVector obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 3, 0), CompilerGenerated]
            public Vector<System.Numerics.Complex> Item =>
                this._obj.item;
        }

        [Serializable, DebuggerTypeProxy(typeof(FloatingPoint.Real@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Real : FloatingPoint
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly double item;

            [CompilerGenerated]
            internal Real(double item) : base(0)
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
            internal FloatingPoint.Real _obj;

            [CompilerGenerated]
            public Real@DebugTypeProxy(FloatingPoint.Real obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 0, 0), CompilerGenerated]
            public double Item =>
                this._obj.item;
        }

        [Serializable, DebuggerTypeProxy(typeof(FloatingPoint.RealMatrix@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class RealMatrix : FloatingPoint
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly Matrix<double> item;

            [CompilerGenerated]
            internal RealMatrix(Matrix<double> item) : base(4)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 4, 0), CompilerGenerated]
            public Matrix<double> Item =>
                this.item;
        }

        internal class RealMatrix@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal FloatingPoint.RealMatrix _obj;

            [CompilerGenerated]
            public RealMatrix@DebugTypeProxy(FloatingPoint.RealMatrix obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 4, 0), CompilerGenerated]
            public Matrix<double> Item =>
                this._obj.item;
        }

        [Serializable, DebuggerTypeProxy(typeof(FloatingPoint.RealVector@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class RealVector : FloatingPoint
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly Vector<double> item;

            [CompilerGenerated]
            internal RealVector(Vector<double> item) : base(2)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 2, 0), CompilerGenerated]
            public Vector<double> Item =>
                this.item;
        }

        internal class RealVector@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal FloatingPoint.RealVector _obj;

            [CompilerGenerated]
            public RealVector@DebugTypeProxy(FloatingPoint.RealVector obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 2, 0), CompilerGenerated]
            public Vector<double> Item =>
                this._obj.item;
        }

        public static class Tags
        {
            public const int Complex = 1;
            public const int ComplexInf = 9;
            public const int ComplexMatrix = 5;
            public const int ComplexVector = 3;
            public const int NegInf = 8;
            public const int PosInf = 7;
            public const int Real = 0;
            public const int RealMatrix = 4;
            public const int RealVector = 2;
            public const int Undef = 6;
        }
    }
}

