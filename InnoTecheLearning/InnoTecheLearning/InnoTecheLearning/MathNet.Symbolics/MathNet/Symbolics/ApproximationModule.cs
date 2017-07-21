namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;
    using System.Numerics;

    [RequireQualifiedAccess, CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix), CompilationMapping(SourceConstructFlags.Module)]
    public static class ApproximationModule
    {
        public static MathNet.Symbolics.Approximation abs(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewReal(System.Numerics.Complex.Abs(item));
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Math.Abs(real.item));
        }

        public static MathNet.Symbolics.Approximation acos(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Acos(item));
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Math.Acos(real.item));
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static MathNet.Symbolics.Approximation apply(MathNet.Symbolics.Function f, MathNet.Symbolics.Approximation a)
        {
            switch (f.Tag)
            {
                case 1:
                    return ln(a);

                case 2:
                    return exp(a);

                case 3:
                    return sin(a);

                case 4:
                    return cos(a);

                case 5:
                    return tan(a);

                case 6:
                    return cot(a);

                case 7:
                    return sec(a);

                case 8:
                    return csc(a);

                case 9:
                    return cosh(a);

                case 10:
                    return sinh(a);

                case 11:
                    return tanh(a);

                case 12:
                    return asin(a);

                case 13:
                    return acos(a);

                case 14:
                    return atan(a);
            }
            return abs(a);
        }

        public static MathNet.Symbolics.Approximation asin(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Asin(item));
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Math.Asin(real.item));
        }

        public static MathNet.Symbolics.Approximation atan(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Atan(item));
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Math.Atan(real.item));
        }

        public static MathNet.Symbolics.Approximation cos(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Cos(item));
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Math.Cos(real.item));
        }

        public static MathNet.Symbolics.Approximation cosh(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Cosh(item));
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Math.Cosh(real.item));
        }

        public static MathNet.Symbolics.Approximation cot(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(item.Cot());
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Trig.Cot(real.item));
        }

        public static MathNet.Symbolics.Approximation csc(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(item.Csc());
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Trig.Csc(real.item));
        }

        public static MathNet.Symbolics.Approximation exp(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Exp(item));
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Math.Exp(real.item));
        }

        public static MathNet.Symbolics.Approximation fromRational(BigRational x) => 
            MathNet.Symbolics.Approximation.NewReal(BigRational.ToDouble(x));

        public static MathNet.Symbolics.Approximation invert(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex complex2 = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Reciprocal(complex2));
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            double item = real.item;
            return MathNet.Symbolics.Approximation.NewReal(1.0 / item);
        }

        public static bool isMinusOne(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Real)
            {
                MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
                if (real.item == -1.0)
                {
                    double item = real.item;
                    return true;
                }
            }
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex complex2 = complex.item;
                if (complex2.IsReal() && (complex2.Real == -1.0))
                {
                    complex2 = complex.item;
                    return true;
                }
            }
            return false;
        }

        public static bool isNegative(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Real)
            {
                MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
                if (real.item < 0.0)
                {
                    double item = real.item;
                    return true;
                }
            }
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex complex2 = complex.item;
                if (complex2.IsReal() && (complex2.Real < 0.0))
                {
                    complex2 = complex.item;
                    return true;
                }
            }
            return false;
        }

        public static bool isOne(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Real)
            {
                MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
                if (real.item == 1.0)
                {
                    double item = real.item;
                    return true;
                }
            }
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                if (LanguagePrimitives.HashCompare.GenericEqualityIntrinsic<System.Numerics.Complex>(complex.item, System.Numerics.Complex.One))
                {
                    System.Numerics.Complex complex2 = complex.item;
                    return true;
                }
            }
            return false;
        }

        public static bool isPositive(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Real)
            {
                MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
                if (real.item > 0.0)
                {
                    double item = real.item;
                    return true;
                }
            }
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex complex2 = complex.item;
                if (complex2.IsReal() && (complex2.Real > 0.0))
                {
                    complex2 = complex.item;
                    return true;
                }
            }
            return false;
        }

        public static bool isZero(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Real)
            {
                MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
                if (real.item == 0.0)
                {
                    double item = real.item;
                    return true;
                }
            }
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                if (complex.item.IsZero())
                {
                    System.Numerics.Complex complex2 = complex.item;
                    return true;
                }
            }
            return false;
        }

        public static MathNet.Symbolics.Approximation ln(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Log(item));
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Math.Log(real.item));
        }

        public static MathNet.Symbolics.Approximation negate(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(-item);
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(-real.item);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        internal static bool orderRelation(MathNet.Symbolics.Approximation x, MathNet.Symbolics.Approximation y)
        {
            MathNet.Symbolics.Approximation.Complex complex;
            MathNet.Symbolics.Approximation.Real real;
            double item;
            System.Numerics.Complex complex2;
            if (x is MathNet.Symbolics.Approximation.Complex)
            {
                complex = (MathNet.Symbolics.Approximation.Complex) x;
                if (y is MathNet.Symbolics.Approximation.Real)
                {
                    real = (MathNet.Symbolics.Approximation.Real) y;
                    item = real.item;
                    complex2 = complex.item;
                    return (complex2.IsReal() && (complex2.Real < item));
                }
                MathNet.Symbolics.Approximation.Complex complex3 = (MathNet.Symbolics.Approximation.Complex) y;
                complex2 = complex3.item;
                System.Numerics.Complex complex4 = complex.item;
                return ((complex4.Real < complex2.Real) || ((complex4.Real == complex2.Real) && (complex4.Imaginary < complex2.Imaginary)));
            }
            real = (MathNet.Symbolics.Approximation.Real) x;
            if (y is MathNet.Symbolics.Approximation.Complex)
            {
                complex = (MathNet.Symbolics.Approximation.Complex) y;
                complex2 = complex.item;
                item = real.item;
                return (!complex2.IsReal() || (item < complex2.Real));
            }
            MathNet.Symbolics.Approximation.Real real2 = (MathNet.Symbolics.Approximation.Real) y;
            item = real2.item;
            double num2 = real.item;
            return (num2 < item);
        }

        public static MathNet.Symbolics.Approximation pow(MathNet.Symbolics.Approximation _arg1_0, MathNet.Symbolics.Approximation _arg1_1)
        {
            MathNet.Symbolics.Approximation.Complex complex;
            MathNet.Symbolics.Approximation.Real real;
            double item;
            System.Numerics.Complex complex2;
            if (_arg1_0 is MathNet.Symbolics.Approximation.Complex)
            {
                complex = (MathNet.Symbolics.Approximation.Complex) _arg1_0;
                if (_arg1_1 is MathNet.Symbolics.Approximation.Real)
                {
                    real = (MathNet.Symbolics.Approximation.Real) _arg1_1;
                    item = real.item;
                    complex2 = complex.item;
                    return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Pow(complex2, new System.Numerics.Complex(item, 0.0)));
                }
                MathNet.Symbolics.Approximation.Complex complex3 = (MathNet.Symbolics.Approximation.Complex) _arg1_1;
                complex2 = complex3.item;
                return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Pow(complex.item, complex2));
            }
            real = (MathNet.Symbolics.Approximation.Real) _arg1_0;
            if (_arg1_1 is MathNet.Symbolics.Approximation.Complex)
            {
                complex = (MathNet.Symbolics.Approximation.Complex) _arg1_1;
                complex2 = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Pow(new System.Numerics.Complex(real.item, 0.0), complex2));
            }
            MathNet.Symbolics.Approximation.Real real2 = (MathNet.Symbolics.Approximation.Real) _arg1_1;
            item = real2.item;
            return MathNet.Symbolics.Approximation.NewReal(Math.Pow(real.item, item));
        }

        public static MathNet.Symbolics.Approximation product(MathNet.Symbolics.Approximation _arg1_0, MathNet.Symbolics.Approximation _arg1_1)
        {
            MathNet.Symbolics.Approximation.Complex complex;
            MathNet.Symbolics.Approximation.Real real;
            double item;
            System.Numerics.Complex complex2;
            if (_arg1_0 is MathNet.Symbolics.Approximation.Complex)
            {
                complex = (MathNet.Symbolics.Approximation.Complex) _arg1_0;
                if (!(_arg1_1 is MathNet.Symbolics.Approximation.Real))
                {
                    MathNet.Symbolics.Approximation.Complex complex3 = (MathNet.Symbolics.Approximation.Complex) _arg1_1;
                    complex2 = complex3.item;
                    return MathNet.Symbolics.Approximation.NewComplex(complex.item * complex2);
                }
                real = (MathNet.Symbolics.Approximation.Real) _arg1_1;
                item = real.item;
                complex2 = complex.item;
            }
            else
            {
                real = (MathNet.Symbolics.Approximation.Real) _arg1_0;
                if (!(_arg1_1 is MathNet.Symbolics.Approximation.Complex))
                {
                    MathNet.Symbolics.Approximation.Real real2 = (MathNet.Symbolics.Approximation.Real) _arg1_1;
                    item = real2.item;
                    return MathNet.Symbolics.Approximation.NewReal(real.item * item);
                }
                complex = (MathNet.Symbolics.Approximation.Complex) _arg1_1;
                complex2 = complex.item;
                item = real.item;
            }
            return MathNet.Symbolics.Approximation.NewComplex(complex2 * new System.Numerics.Complex(item, 0.0));
        }

        public static MathNet.Symbolics.Approximation sec(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(item.Sec());
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Trig.Sec(real.item));
        }

        public static MathNet.Symbolics.Approximation sin(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Sin(item));
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Math.Sin(real.item));
        }

        public static MathNet.Symbolics.Approximation sinh(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Sinh(item));
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Math.Sinh(real.item));
        }

        [CompilationSourceName("sortList")]
        public static FSharpList<MathNet.Symbolics.Approximation> SortList(FSharpList<MathNet.Symbolics.Approximation> list) => 
            ListModule.SortWith<MathNet.Symbolics.Approximation>(new SortList@150(), list);

        public static MathNet.Symbolics.Approximation sum(MathNet.Symbolics.Approximation _arg1_0, MathNet.Symbolics.Approximation _arg1_1)
        {
            MathNet.Symbolics.Approximation.Complex complex;
            MathNet.Symbolics.Approximation.Real real;
            double item;
            System.Numerics.Complex complex2;
            if (_arg1_0 is MathNet.Symbolics.Approximation.Complex)
            {
                complex = (MathNet.Symbolics.Approximation.Complex) _arg1_0;
                if (!(_arg1_1 is MathNet.Symbolics.Approximation.Real))
                {
                    MathNet.Symbolics.Approximation.Complex complex3 = (MathNet.Symbolics.Approximation.Complex) _arg1_1;
                    complex2 = complex3.item;
                    return MathNet.Symbolics.Approximation.NewComplex(complex.item + complex2);
                }
                real = (MathNet.Symbolics.Approximation.Real) _arg1_1;
                item = real.item;
                complex2 = complex.item;
            }
            else
            {
                real = (MathNet.Symbolics.Approximation.Real) _arg1_0;
                if (!(_arg1_1 is MathNet.Symbolics.Approximation.Complex))
                {
                    MathNet.Symbolics.Approximation.Real real2 = (MathNet.Symbolics.Approximation.Real) _arg1_1;
                    item = real2.item;
                    return MathNet.Symbolics.Approximation.NewReal(real.item + item);
                }
                complex = (MathNet.Symbolics.Approximation.Complex) _arg1_1;
                complex2 = complex.item;
                item = real.item;
            }
            return MathNet.Symbolics.Approximation.NewComplex(complex2 + new System.Numerics.Complex(item, 0.0));
        }

        public static MathNet.Symbolics.Approximation tan(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Tan(item));
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Math.Tan(real.item));
        }

        public static MathNet.Symbolics.Approximation tanh(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                System.Numerics.Complex item = complex.item;
                return MathNet.Symbolics.Approximation.NewComplex(System.Numerics.Complex.Tanh(item));
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return MathNet.Symbolics.Approximation.NewReal(Math.Tanh(real.item));
        }

        [Serializable]
        internal class SortList@150 : OptimizedClosures.FSharpFunc<MathNet.Symbolics.Approximation, MathNet.Symbolics.Approximation, int>
        {
            internal SortList@150()
            {
            }

            public override int Invoke(MathNet.Symbolics.Approximation a, MathNet.Symbolics.Approximation b)
            {
                if (a.Equals(b, LanguagePrimitives.GenericEqualityComparer))
                {
                    return 0;
                }
                if (ApproximationModule.orderRelation(a, b))
                {
                    return -1;
                }
                return 1;
            }
        }
    }
}

