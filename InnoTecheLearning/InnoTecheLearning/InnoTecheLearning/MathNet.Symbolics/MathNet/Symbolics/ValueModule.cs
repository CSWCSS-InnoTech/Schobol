namespace MathNet.Symbolics
{
    using <StartupCode$MathNet-Symbolics>;
    using MathNet.Numerics;
    using Microsoft.FSharp.Core;
    using System;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    [RequireQualifiedAccess, CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix), CompilationMapping(SourceConstructFlags.Module)]
    public static class ValueModule
    {
        public static FSharpOption<Unit> |MinusOne|_|(Value _arg1)
        {
            if (_arg1.Tag == 0)
            {
                Value.Number number = (Value.Number) _arg1;
                BigRational item = number.item;
                if (item.IsInteger && LanguagePrimitives.HashCompare.GenericEqualityIntrinsic<BigInteger>(item.Numerator, BigInteger.MinusOne))
                {
                    item = number.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            if (_arg1.Tag == 1)
            {
                Value.Approximation approximation = (Value.Approximation) _arg1;
                if (ApproximationModule.isMinusOne(approximation.item))
                {
                    MathNet.Symbolics.Approximation approximation2 = approximation.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            return null;
        }

        public static FSharpOption<Unit> |Negative|_|(Value _arg1)
        {
            if (_arg1.Tag == 0)
            {
                Value.Number number = (Value.Number) _arg1;
                if (number.item.IsNegative)
                {
                    BigRational item = number.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            if (_arg1.Tag == 1)
            {
                Value.Approximation approximation = (Value.Approximation) _arg1;
                if (ApproximationModule.isNegative(approximation.item))
                {
                    MathNet.Symbolics.Approximation approximation2 = approximation.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            if (_arg1.Tag == 4)
            {
                return FSharpOption<Unit>.Some(null);
            }
            return null;
        }

        public static FSharpOption<Unit> |One|_|(Value _arg1)
        {
            if (_arg1.Tag == 0)
            {
                Value.Number number = (Value.Number) _arg1;
                if (number.item.IsOne)
                {
                    BigRational item = number.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            if (_arg1.Tag == 1)
            {
                Value.Approximation approximation = (Value.Approximation) _arg1;
                if (ApproximationModule.isOne(approximation.item))
                {
                    MathNet.Symbolics.Approximation approximation2 = approximation.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            return null;
        }

        public static FSharpOption<Unit> |Positive|_|(Value _arg1)
        {
            if (_arg1.Tag == 0)
            {
                Value.Number number = (Value.Number) _arg1;
                if (number.item.IsPositive)
                {
                    BigRational item = number.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            if (_arg1.Tag == 1)
            {
                Value.Approximation approximation = (Value.Approximation) _arg1;
                if (ApproximationModule.isPositive(approximation.item))
                {
                    MathNet.Symbolics.Approximation approximation2 = approximation.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            if (_arg1.Tag == 3)
            {
                return FSharpOption<Unit>.Some(null);
            }
            return null;
        }

        public static FSharpOption<Unit> |Zero|_|(Value _arg1)
        {
            if (_arg1.Tag == 0)
            {
                Value.Number number = (Value.Number) _arg1;
                if (number.item.IsZero)
                {
                    BigRational item = number.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            if (_arg1.Tag == 1)
            {
                Value.Approximation approximation = (Value.Approximation) _arg1;
                if (ApproximationModule.isZero(approximation.item))
                {
                    MathNet.Symbolics.Approximation approximation2 = approximation.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            return null;
        }

        public static Value abs(Value _arg1)
        {
            if (_arg1.Tag == 0)
            {
                Value.Number number = (Value.Number) _arg1;
                if (number.item.IsNegative)
                {
                    BigRational item = number.item;
                    return Value.NewNumber(-item);
                }
            }
            switch (_arg1.Tag)
            {
                case 0:
                    return _arg1;

                case 1:
                {
                    Value.Approximation approximation = (Value.Approximation) _arg1;
                    return approx(ApproximationModule.abs(approximation.item));
                }
                case 2:
                case 3:
                case 4:
                    return Value.PositiveInfinity;

                case 5:
                    return Value.Undefined;
            }
            return _arg1;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static Value apply(MathNet.Symbolics.Function f, Value _arg1)
        {
            switch (_arg1.Tag)
            {
                case 0:
                {
                    Value.Number number = (Value.Number) _arg1;
                    BigRational n = number.item;
                    return approx(ApproximationModule.apply(f, MathNet.Symbolics.Approximation.NewReal(BigRational.ToDouble(n))));
                }
                case 2:
                    return Value.Undefined;

                case 3:
                    return Value.Undefined;

                case 4:
                    return Value.Undefined;

                case 5:
                    return Value.Undefined;
            }
            Value.Approximation approximation = (Value.Approximation) _arg1;
            MathNet.Symbolics.Approximation item = approximation.item;
            return approx(ApproximationModule.apply(f, item));
        }

        public static Value approx(MathNet.Symbolics.Approximation _arg1)
        {
            if (_arg1 is MathNet.Symbolics.Approximation.Complex)
            {
                MathNet.Symbolics.Approximation.Complex complex = (MathNet.Symbolics.Approximation.Complex) _arg1;
                return ValueModule.complex(complex.item);
            }
            MathNet.Symbolics.Approximation.Real real = (MathNet.Symbolics.Approximation.Real) _arg1;
            return ValueModule.real(real.item);
        }

        public static Value complex(System.Numerics.Complex x)
        {
            if (x.IsReal())
            {
                return real(x.Real);
            }
            if (x.IsInfinity())
            {
                return Value.ComplexInfinity;
            }
            if (x.IsNaN())
            {
                return Value.Undefined;
            }
            return Value.NewApproximation(MathNet.Symbolics.Approximation.NewComplex(x));
        }

        public static Value invert(Value _arg1)
        {
            if (|Zero|_|(_arg1) != null)
            {
                return Value.ComplexInfinity;
            }
            switch (_arg1.Tag)
            {
                case 1:
                {
                    Value.Approximation approximation = (Value.Approximation) _arg1;
                    return approx(ApproximationModule.invert(approximation.item));
                }
                case 2:
                case 3:
                case 4:
                    return zero;

                case 5:
                    return Value.Undefined;
            }
            Value.Number number = (Value.Number) _arg1;
            return Value.NewNumber(BigRational.Reciprocal(number.item));
        }

        public static bool isMinusOne(Value _arg1) => 
            (|MinusOne|_|(_arg1) > null);

        public static bool isNegative(Value _arg1) => 
            (|Negative|_|(_arg1) > null);

        public static bool isOne(Value _arg1) => 
            (|One|_|(_arg1) > null);

        public static bool isPositive(Value _arg1) => 
            (|Positive|_|(_arg1) > null);

        public static bool isZero(Value _arg1) => 
            (|Zero|_|(_arg1) > null);

        public static Value negate(Value _arg1)
        {
            switch (_arg1.Tag)
            {
                case 1:
                {
                    Value.Approximation approximation = (Value.Approximation) _arg1;
                    return approx(ApproximationModule.negate(approximation.item));
                }
                case 2:
                    return Value.ComplexInfinity;

                case 3:
                    return Value.NegativeInfinity;

                case 4:
                    return Value.PositiveInfinity;

                case 5:
                    return Value.Undefined;
            }
            Value.Number number = (Value.Number) _arg1;
            BigRational item = number.item;
            return Value.NewNumber(-item);
        }

        public static Value power(Value _arg1_0, Value _arg1_1)
        {
            if ((_arg1_0.Tag != 5) && (_arg1_1.Tag != 5))
            {
                if ((|Zero|_|(_arg1_0) != null) && (|Zero|_|(_arg1_1) != null))
                {
                    return Value.Undefined;
                }
                if (|Zero|_|(_arg1_1) != null)
                {
                    return one;
                }
                if (|One|_|(_arg1_1) != null)
                {
                    return _arg1_0;
                }
                if (|One|_|(_arg1_0) != null)
                {
                    return one;
                }
                return power$cont@122(_arg1_0, _arg1_1, null);
            }
            return Value.Undefined;
        }

        [CompilerGenerated]
        internal static Value power$cont@122(Value _arg1_0, Value _arg1_1, Unit unitVar)
        {
            BigRational item;
            Value.Approximation approximation;
            MathNet.Symbolics.Approximation approximation3;
            Value.Number number3;
            if (_arg1_0.Tag == 0)
            {
                Value.Number number = (Value.Number) _arg1_0;
                if (_arg1_1.Tag == 0)
                {
                    Value.Number number2 = (Value.Number) _arg1_1;
                    if (number2.item.IsInteger)
                    {
                        item = number2.item;
                        BigRational n = number.item;
                        if (!item.IsNegative)
                        {
                            return Value.NewNumber(BigRational.Pow(n, (int) item.Numerator));
                        }
                        if (n.IsZero)
                        {
                            return Value.ComplexInfinity;
                        }
                        return Value.NewNumber(BigRational.Pow(BigRational.Reciprocal(n), -((int) item.Numerator)));
                    }
                }
            }
            switch (_arg1_0.Tag)
            {
                case 0:
                    number3 = (Value.Number) _arg1_0;
                    switch (_arg1_1.Tag)
                    {
                        case 0:
                        {
                            Value.Number number4 = (Value.Number) _arg1_1;
                            item = number4.item;
                            return approx(ApproximationModule.pow(MathNet.Symbolics.Approximation.NewReal(BigRational.ToDouble(number3.item)), MathNet.Symbolics.Approximation.NewReal(BigRational.ToDouble(item))));
                        }
                        case 1:
                            approximation = (Value.Approximation) _arg1_1;
                            approximation3 = approximation.item;
                            return approx(ApproximationModule.pow(MathNet.Symbolics.Approximation.NewReal(BigRational.ToDouble(number3.item)), approximation3));
                    }
                    break;

                case 1:
                    approximation = (Value.Approximation) _arg1_0;
                    switch (_arg1_1.Tag)
                    {
                        case 0:
                            number3 = (Value.Number) _arg1_1;
                            item = number3.item;
                            return approx(ApproximationModule.pow(approximation.item, MathNet.Symbolics.Approximation.NewReal(BigRational.ToDouble(item))));

                        case 1:
                        {
                            Value.Approximation approximation2 = (Value.Approximation) _arg1_1;
                            approximation3 = approximation2.item;
                            return approx(ApproximationModule.pow(approximation.item, approximation3));
                        }
                    }
                    break;
            }
            return Value.Undefined;
        }

        public static Value product(Value _arg1_0, Value _arg1_1)
        {
            if ((_arg1_0.Tag != 5) && (_arg1_1.Tag != 5))
            {
                if (|One|_|(_arg1_0) != null)
                {
                    return _arg1_1;
                }
                if (|One|_|(_arg1_1) != null)
                {
                    return _arg1_0;
                }
                if ((|Zero|_|(_arg1_0) == null) && (|Zero|_|(_arg1_1) == null))
                {
                    return product$cont@100(_arg1_0, _arg1_1, null);
                }
                return zero;
            }
            return Value.Undefined;
        }

        [CompilerGenerated]
        internal static Value product$cont@100(Value _arg1_0, Value _arg1_1, Unit unitVar)
        {
            Value.Number number;
            BigRational item;
            Value.Approximation approximation;
            MathNet.Symbolics.Approximation approximation2;
            switch (_arg1_0.Tag)
            {
                case 0:
                    number = (Value.Number) _arg1_0;
                    switch (_arg1_1.Tag)
                    {
                        case 0:
                        {
                            Value.Number number2 = (Value.Number) _arg1_1;
                            item = number2.item;
                            return Value.NewNumber(number.item * item);
                        }
                        case 1:
                            approximation = (Value.Approximation) _arg1_1;
                            approximation2 = approximation.item;
                            item = number.item;
                            goto Label_0117;

                        case 2:
                            goto Label_0093;

                        case 3:
                            if (|Positive|_|(_arg1_0) == null)
                            {
                                break;
                            }
                            goto Label_00A3;
                    }
                    goto Label_0046;

                case 1:
                    approximation = (Value.Approximation) _arg1_0;
                    switch (_arg1_1.Tag)
                    {
                        case 0:
                            number = (Value.Number) _arg1_1;
                            item = number.item;
                            approximation2 = approximation.item;
                            goto Label_0117;

                        case 1:
                        {
                            Value.Approximation approximation3 = (Value.Approximation) _arg1_1;
                            approximation2 = approximation3.item;
                            return approx(ApproximationModule.product(approximation.item, approximation2));
                        }
                        case 2:
                            goto Label_0093;

                        case 3:
                            if (|Positive|_|(_arg1_0) == null)
                            {
                                break;
                            }
                            goto Label_00A3;
                    }
                    goto Label_0046;

                case 2:
                    goto Label_0093;

                case 3:
                    switch (_arg1_1.Tag)
                    {
                        case 2:
                            goto Label_0093;

                        case 3:
                            if ((|Positive|_|(_arg1_1) == null) && (|Positive|_|(_arg1_0) == null))
                            {
                                goto Label_0046;
                            }
                            goto Label_00A3;
                    }
                    if (|Positive|_|(_arg1_1) == null)
                    {
                        goto Label_0046;
                    }
                    goto Label_00A3;
            }
            switch (_arg1_1.Tag)
            {
                case 2:
                    goto Label_0093;

                case 3:
                    if (|Positive|_|(_arg1_0) == null)
                    {
                        break;
                    }
                    goto Label_00A3;
            }
        Label_0046:
            if (_arg1_0.Tag != 3)
            {
                if ((|Negative|_|(_arg1_0) == null) || (_arg1_1.Tag != 3))
                {
                    goto Label_0075;
                }
            }
            else if ((|Negative|_|(_arg1_1) == null) && ((|Negative|_|(_arg1_0) == null) || (_arg1_1.Tag != 3)))
            {
                goto Label_0075;
            }
            return Value.NegativeInfinity;
        Label_0075:
            return product$cont@100-1(_arg1_0, _arg1_1, null);
        Label_0093:
            return Value.ComplexInfinity;
        Label_00A3:
            return Value.PositiveInfinity;
        Label_0117:
            return approx(ApproximationModule.product(MathNet.Symbolics.Approximation.NewReal(BigRational.ToDouble(item)), approximation2));
        }

        [CompilerGenerated]
        internal static Value product$cont@100-1(Value _arg1_0, Value _arg1_1, Unit unitVar)
        {
            if (_arg1_0.Tag != 4)
            {
                if ((|Positive|_|(_arg1_0) == null) || (_arg1_1.Tag != 4))
                {
                    goto Label_0032;
                }
            }
            else if ((|Positive|_|(_arg1_1) == null) && ((|Positive|_|(_arg1_0) == null) || (_arg1_1.Tag != 4)))
            {
                goto Label_0032;
            }
            return Value.NegativeInfinity;
        Label_0032:
            if (_arg1_0.Tag != 4)
            {
                if ((|Negative|_|(_arg1_0) == null) || (_arg1_1.Tag != 4))
                {
                    goto Label_0063;
                }
            }
            else if ((|Negative|_|(_arg1_1) == null) && ((|Negative|_|(_arg1_0) == null) || (_arg1_1.Tag != 4)))
            {
                goto Label_0063;
            }
            return Value.PositiveInfinity;
        Label_0063:
            switch (_arg1_0.Tag)
            {
                case 3:
                    switch (_arg1_1.Tag)
                    {
                    }
                    goto Label_00BE;

                case 4:
                    break;

                default:
                    switch (_arg1_1.Tag)
                    {
                        case 3:
                            goto Label_00BE;

                        case 4:
                            break;

                        default:
                            throw new MatchFailureException(@"D:\Dev\Math.NET\mathnet-symbolics\src\Symbolics\Value.fs", 100, 0x12);
                    }
                    break;
            }
            return Value.NegativeInfinity;
        Label_00BE:
            return Value.PositiveInfinity;
        }

        public static Value real(double x)
        {
            if (double.IsPositiveInfinity(x))
            {
                return Value.PositiveInfinity;
            }
            if (double.IsNegativeInfinity(x))
            {
                return Value.NegativeInfinity;
            }
            if (double.IsNaN(x))
            {
                return Value.Undefined;
            }
            return Value.NewApproximation(MathNet.Symbolics.Approximation.NewReal(x));
        }

        public static Value sum(Value _arg1_0, Value _arg1_1)
        {
            if ((_arg1_0.Tag != 5) && (_arg1_1.Tag != 5))
            {
                if (|Zero|_|(_arg1_0) != null)
                {
                    return _arg1_1;
                }
                if (|Zero|_|(_arg1_1) != null)
                {
                    return _arg1_0;
                }
                return sum$cont@87(_arg1_0, _arg1_1, null);
            }
            return Value.Undefined;
        }

        [CompilerGenerated]
        internal static Value sum$cont@87(Value _arg1_0, Value _arg1_1, Unit unitVar)
        {
            Value.Number number;
            BigRational item;
            Value.Approximation approximation;
            MathNet.Symbolics.Approximation approximation2;
            switch (_arg1_0.Tag)
            {
                case 0:
                    number = (Value.Number) _arg1_0;
                    switch (_arg1_1.Tag)
                    {
                        case 0:
                        {
                            Value.Number number2 = (Value.Number) _arg1_1;
                            item = number2.item;
                            return Value.NewNumber(number.item + item);
                        }
                        case 1:
                            approximation = (Value.Approximation) _arg1_1;
                            approximation2 = approximation.item;
                            item = number.item;
                            goto Label_00CE;

                        case 2:
                            goto Label_0055;

                        case 3:
                            goto Label_005C;

                        case 4:
                            goto Label_0062;
                    }
                    break;

                case 1:
                    approximation = (Value.Approximation) _arg1_0;
                    switch (_arg1_1.Tag)
                    {
                        case 0:
                            number = (Value.Number) _arg1_1;
                            item = number.item;
                            approximation2 = approximation.item;
                            goto Label_00CE;

                        case 1:
                        {
                            Value.Approximation approximation3 = (Value.Approximation) _arg1_1;
                            approximation2 = approximation3.item;
                            return approx(ApproximationModule.sum(approximation.item, approximation2));
                        }
                        case 2:
                            goto Label_0055;

                        case 3:
                            goto Label_005C;

                        case 4:
                            goto Label_0062;
                    }
                    break;

                case 2:
                    switch (_arg1_1.Tag)
                    {
                        case 2:
                        case 3:
                        case 4:
                            return Value.Undefined;
                    }
                    goto Label_0055;

                case 3:
                    switch (_arg1_1.Tag)
                    {
                        case 2:
                            goto Label_01B1;

                        case 4:
                            goto Label_01B7;
                    }
                    goto Label_005C;

                case 4:
                    switch (_arg1_1.Tag)
                    {
                        case 2:
                            goto Label_01B1;

                        case 3:
                            goto Label_01B7;
                    }
                    goto Label_0062;

                default:
                    switch (_arg1_1.Tag)
                    {
                        case 2:
                            goto Label_0055;

                        case 3:
                            goto Label_005C;

                        case 4:
                            goto Label_0062;
                    }
                    break;
            }
            throw new MatchFailureException(@"D:\Dev\Math.NET\mathnet-symbolics\src\Symbolics\Value.fs", 0x57, 14);
        Label_0055:
            return Value.ComplexInfinity;
        Label_005C:
            return Value.PositiveInfinity;
        Label_0062:
            return Value.NegativeInfinity;
        Label_00CE:
            return approx(ApproximationModule.sum(MathNet.Symbolics.Approximation.NewReal(BigRational.ToDouble(item)), approximation2));
        Label_01B1:
            return Value.Undefined;
        Label_01B7:
            return Value.Undefined;
        }

        [CompilationMapping(SourceConstructFlags.Value)]
        public static Value one =>
            $Value.one@37;

        [CompilationMapping(SourceConstructFlags.Value)]
        public static Value zero =>
            $Value.zero@36;
    }
}

