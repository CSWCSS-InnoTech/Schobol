namespace <StartupCode$MathNet-Symbolics>
{
    using MathNet.Symbolics;
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections;
    using System.Runtime.CompilerServices;

    internal static class $Evaluate
    {
        [CompilerGenerated]
        internal static bool Equals$cont@11(FloatingPoint @this, FloatingPoint that, IEqualityComparer comp, Unit unitVar)
        {
            int num = @this._tag;
            int num2 = that._tag;
            if (num != num2)
            {
                return false;
            }
            switch (@this.get_Tag())
            {
                case 0:
                {
                    FloatingPoint.Real real = (FloatingPoint.Real) @this;
                    FloatingPoint.Real real2 = (FloatingPoint.Real) that;
                    return (real.item == real2.item);
                }
                case 1:
                {
                    FloatingPoint.Complex complex = (FloatingPoint.Complex) @this;
                    FloatingPoint.Complex complex2 = (FloatingPoint.Complex) that;
                    return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic<System.Numerics.Complex>(comp, complex.item, complex2.item);
                }
                case 2:
                {
                    FloatingPoint.RealVector vector = (FloatingPoint.RealVector) @this;
                    FloatingPoint.RealVector vector2 = (FloatingPoint.RealVector) that;
                    return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic<Vector<double>>(comp, vector.item, vector2.item);
                }
                case 3:
                {
                    FloatingPoint.ComplexVector vector3 = (FloatingPoint.ComplexVector) @this;
                    FloatingPoint.ComplexVector vector4 = (FloatingPoint.ComplexVector) that;
                    return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic<Vector<System.Numerics.Complex>>(comp, vector3.item, vector4.item);
                }
                case 4:
                {
                    FloatingPoint.RealMatrix matrix = (FloatingPoint.RealMatrix) @this;
                    FloatingPoint.RealMatrix matrix2 = (FloatingPoint.RealMatrix) that;
                    return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic<Matrix<double>>(comp, matrix.item, matrix2.item);
                }
                case 5:
                {
                    FloatingPoint.ComplexMatrix matrix3 = (FloatingPoint.ComplexMatrix) @this;
                    FloatingPoint.ComplexMatrix matrix4 = (FloatingPoint.ComplexMatrix) that;
                    return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic<Matrix<System.Numerics.Complex>>(comp, matrix3.item, matrix4.item);
                }
            }
            return true;
        }

        [CompilerGenerated]
        internal static bool Equals$cont@11-1(FloatingPoint @this, FloatingPoint obj, Unit unitVar)
        {
            int num = @this._tag;
            int num2 = obj._tag;
            if (num != num2)
            {
                return false;
            }
            switch (@this.get_Tag())
            {
                case 0:
                {
                    FloatingPoint.Real real = (FloatingPoint.Real) @this;
                    FloatingPoint.Real real2 = (FloatingPoint.Real) obj;
                    double item = real.item;
                    double num4 = real2.item;
                    if ((item == item) || (num4 == num4))
                    {
                        return (item == num4);
                    }
                    return true;
                }
                case 1:
                {
                    FloatingPoint.Complex complex = (FloatingPoint.Complex) @this;
                    FloatingPoint.Complex complex2 = (FloatingPoint.Complex) obj;
                    return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic<System.Numerics.Complex>(complex.item, complex2.item);
                }
                case 2:
                {
                    FloatingPoint.RealVector vector = (FloatingPoint.RealVector) @this;
                    FloatingPoint.RealVector vector2 = (FloatingPoint.RealVector) obj;
                    return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic<Vector<double>>(vector.item, vector2.item);
                }
                case 3:
                {
                    FloatingPoint.ComplexVector vector3 = (FloatingPoint.ComplexVector) @this;
                    FloatingPoint.ComplexVector vector4 = (FloatingPoint.ComplexVector) obj;
                    return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic<Vector<System.Numerics.Complex>>(vector3.item, vector4.item);
                }
                case 4:
                {
                    FloatingPoint.RealMatrix matrix = (FloatingPoint.RealMatrix) @this;
                    FloatingPoint.RealMatrix matrix2 = (FloatingPoint.RealMatrix) obj;
                    return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic<Matrix<double>>(matrix.item, matrix2.item);
                }
                case 5:
                {
                    FloatingPoint.ComplexMatrix matrix3 = (FloatingPoint.ComplexMatrix) @this;
                    FloatingPoint.ComplexMatrix matrix4 = (FloatingPoint.ComplexMatrix) obj;
                    return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic<Matrix<System.Numerics.Complex>>(matrix3.item, matrix4.item);
                }
            }
            return true;
        }

        [CompilerGenerated]
        internal static int GetHashCode$cont@11(IEqualityComparer comp, FloatingPoint @this, Unit unitVar)
        {
            int num = 0;
            switch (@this.get_Tag())
            {
                case 1:
                {
                    FloatingPoint.Complex complex = (FloatingPoint.Complex) @this;
                    num = 1;
                    return (-1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic<System.Numerics.Complex>(comp, complex.item) + ((num << 6) + (num >> 2))));
                }
                case 2:
                {
                    FloatingPoint.RealVector vector = (FloatingPoint.RealVector) @this;
                    num = 2;
                    return (-1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic<Vector<double>>(comp, vector.item) + ((num << 6) + (num >> 2))));
                }
                case 3:
                {
                    FloatingPoint.ComplexVector vector2 = (FloatingPoint.ComplexVector) @this;
                    num = 3;
                    return (-1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic<Vector<System.Numerics.Complex>>(comp, vector2.item) + ((num << 6) + (num >> 2))));
                }
                case 4:
                {
                    FloatingPoint.RealMatrix matrix = (FloatingPoint.RealMatrix) @this;
                    num = 4;
                    return (-1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic<Matrix<double>>(comp, matrix.item) + ((num << 6) + (num >> 2))));
                }
                case 5:
                {
                    FloatingPoint.ComplexMatrix matrix2 = (FloatingPoint.ComplexMatrix) @this;
                    num = 5;
                    return (-1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic<Matrix<System.Numerics.Complex>>(comp, matrix2.item) + ((num << 6) + (num >> 2))));
                }
                case 6:
                    return 6;

                case 7:
                    return 7;

                case 8:
                    return 8;

                case 9:
                    return 9;
            }
            FloatingPoint.Real real = (FloatingPoint.Real) @this;
            num = 0;
            return (-1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic<double>(comp, real.item) + ((num << 6) + (num >> 2))));
        }
    }
}

