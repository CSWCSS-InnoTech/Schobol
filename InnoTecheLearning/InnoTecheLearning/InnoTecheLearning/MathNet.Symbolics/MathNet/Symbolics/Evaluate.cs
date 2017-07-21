namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using MathNet.Numerics.LinearAlgebra;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    [CompilationMapping(SourceConstructFlags.Module)]
    public static class Evaluate
    {
        public static FSharpOption<Unit> |Infinity|_|(FloatingPoint _arg1)
        {
            switch (_arg1.Tag)
            {
                case 7:
                case 8:
                case 9:
                    return FSharpOption<Unit>.Some(null);
            }
            return null;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("fcomplex")]
        public static FloatingPoint Complex(double r, double i) => 
            FloatingPoint.NewComplex(new System.Numerics.Complex(r, i));

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("evaluate")]
        public static FloatingPoint Evaluate(IDictionary<string, FloatingPoint> symbols, Expression _arg1)
        {
            FSharpList<Expression> list;
            Expression expression;
            switch (_arg1.Tag)
            {
                case 1:
                {
                    Expression.Approximation approximation = (Expression.Approximation) _arg1;
                    if (approximation.item is MathNet.Symbolics.Approximation.Complex)
                    {
                        return FloatingPoint.NewComplex(((MathNet.Symbolics.Approximation.Complex) approximation.item).item);
                    }
                    return FloatingPoint.NewReal(((MathNet.Symbolics.Approximation.Real) approximation.item).item);
                }
                case 2:
                {
                    Expression.Identifier identifier = (Expression.Identifier) _arg1;
                    string key = identifier.item.item;
                    FloatingPoint point = null;
                    bool flag = symbols.TryGetValue(key, out point);
                    FloatingPoint point2 = point;
                    if (!flag)
                    {
                        return PrintfModule.PrintFormatToStringThen<FloatingPoint, FSharpFunc<string, FloatingPoint>>(new Evaluate@184(), new PrintfFormat<FSharpFunc<string, FloatingPoint>, Unit, string, FloatingPoint, string>("Failed to find symbol: %s")).Invoke(key);
                    }
                    return fnormalize(point2);
                }
                case 3:
                {
                    Expression.Constant constant = (Expression.Constant) _arg1;
                    switch (constant.item.Tag)
                    {
                        case 1:
                            return FloatingPoint.NewReal(3.1415926535897931);

                        case 2:
                            return FloatingPoint.NewComplex(System.Numerics.Complex.ImaginaryOne);
                    }
                    return FloatingPoint.NewReal(2.7182818284590451);
                }
                case 4:
                    list = ((Expression.Sum) _arg1).item;
                    return fnormalize(ListModule.Reduce<FloatingPoint>(new Evaluate@185-1(), ListModule.Map<Expression, FloatingPoint>(new Evaluate@185-2(symbols), list)));

                case 5:
                    list = ((Expression.Product) _arg1).item;
                    return fnormalize(ListModule.Reduce<FloatingPoint>(new Evaluate@186-3(), ListModule.Map<Expression, FloatingPoint>(new Evaluate@186-4(symbols), list)));

                case 6:
                {
                    Expression.Power power = (Expression.Power) _arg1;
                    expression = power.item1;
                    Expression expression2 = power.item2;
                    return fnormalize(fpower(Evaluate(symbols, expression), Evaluate(symbols, expression2)));
                }
                case 7:
                {
                    Expression.Function function = (Expression.Function) _arg1;
                    expression = function.item2;
                    return fnormalize(fapply(function.item1, Evaluate(symbols, expression)));
                }
                case 8:
                {
                    Expression.FunctionN nn = (Expression.FunctionN) _arg1;
                    list = nn.item2;
                    MathNet.Symbolics.Function function2 = nn.item1;
                    FSharpList<FloatingPoint> list2 = ListModule.Map<Expression, FloatingPoint>(new Evaluate@189-5(symbols), list);
                    if (0 == 0)
                    {
                        throw new Exception("not supported yet");
                    }
                    return fnormalize(null);
                }
                case 9:
                    return FloatingPoint.ComplexInf;

                case 10:
                    return FloatingPoint.PosInf;

                case 11:
                    return FloatingPoint.NegInf;

                case 12:
                    return FloatingPoint.Undef;
            }
            BigRational item = ((Expression.Number) _arg1).item;
            return fnormalize(FloatingPoint.NewReal(BigRational.ToDouble(item)));
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static FloatingPoint fadd(FloatingPoint u, FloatingPoint v)
        {
            MathNet.Symbolics.FloatingPoint.Real real;
            double item;
            MathNet.Symbolics.FloatingPoint.Complex complex;
            System.Numerics.Complex complex2;
            switch (u.Tag)
            {
                case 0:
                    real = (MathNet.Symbolics.FloatingPoint.Real) u;
                    switch (v.Tag)
                    {
                        case 0:
                        {
                            MathNet.Symbolics.FloatingPoint.Real real2 = (MathNet.Symbolics.FloatingPoint.Real) v;
                            item = real2.item;
                            return FloatingPoint.NewReal(real.item + item);
                        }
                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) v;
                            complex2 = complex.item;
                            item = real.item;
                            goto Label_01E4;

                        case 6:
                            goto Label_0154;

                        case 9:
                            if (|Infinity|_|(u) == null)
                            {
                                break;
                            }
                            goto Label_0167;
                    }
                    goto Label_0066;

                case 1:
                    complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                    switch (v.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) v;
                            item = real.item;
                            complex2 = complex.item;
                            goto Label_01E4;

                        case 1:
                        {
                            MathNet.Symbolics.FloatingPoint.Complex complex3 = (MathNet.Symbolics.FloatingPoint.Complex) v;
                            complex2 = complex3.item;
                            return FloatingPoint.NewComplex(complex.item + complex2);
                        }
                        case 6:
                            goto Label_0154;

                        case 9:
                            if (|Infinity|_|(u) == null)
                            {
                                break;
                            }
                            goto Label_0167;
                    }
                    goto Label_0066;

                case 2:
                {
                    FloatingPoint.RealVector vector = (FloatingPoint.RealVector) u;
                    switch (v.Tag)
                    {
                        case 2:
                        {
                            FloatingPoint.RealVector vector2 = (FloatingPoint.RealVector) v;
                            Vector<double> vector3 = vector2.item;
                            return FloatingPoint.NewRealVector(vector.item + vector3);
                        }
                        case 6:
                            goto Label_0154;

                        case 9:
                            if (|Infinity|_|(u) == null)
                            {
                                break;
                            }
                            goto Label_0167;
                    }
                    goto Label_0066;
                }
                case 3:
                {
                    FloatingPoint.ComplexVector vector5 = (FloatingPoint.ComplexVector) u;
                    switch (v.Tag)
                    {
                        case 3:
                        {
                            FloatingPoint.ComplexVector vector6 = (FloatingPoint.ComplexVector) v;
                            Vector<System.Numerics.Complex> vector7 = vector6.item;
                            return FloatingPoint.NewComplexVector(vector5.item + vector7);
                        }
                        case 6:
                            goto Label_0154;

                        case 9:
                            if (|Infinity|_|(u) == null)
                            {
                                break;
                            }
                            goto Label_0167;
                    }
                    goto Label_0066;
                }
                case 4:
                {
                    FloatingPoint.RealMatrix matrix = (FloatingPoint.RealMatrix) u;
                    switch (v.Tag)
                    {
                        case 4:
                        {
                            FloatingPoint.RealMatrix matrix2 = (FloatingPoint.RealMatrix) v;
                            Matrix<double> matrix3 = matrix2.item;
                            return FloatingPoint.NewRealMatrix(matrix.item + matrix3);
                        }
                        case 6:
                            goto Label_0154;

                        case 9:
                            if (|Infinity|_|(u) == null)
                            {
                                break;
                            }
                            goto Label_0167;
                    }
                    goto Label_0066;
                }
                case 5:
                {
                    FloatingPoint.ComplexMatrix matrix5 = (FloatingPoint.ComplexMatrix) u;
                    switch (v.Tag)
                    {
                        case 5:
                        {
                            FloatingPoint.ComplexMatrix matrix6 = (FloatingPoint.ComplexMatrix) v;
                            Matrix<System.Numerics.Complex> matrix7 = matrix6.item;
                            return FloatingPoint.NewComplexMatrix(matrix5.item + matrix7);
                        }
                        case 6:
                            goto Label_0154;

                        case 9:
                            if (|Infinity|_|(u) == null)
                            {
                                break;
                            }
                            goto Label_0167;
                    }
                    goto Label_0066;
                }
                case 6:
                    goto Label_0154;

                case 9:
                    switch (v.Tag)
                    {
                        case 6:
                            goto Label_0154;

                        case 9:
                            if ((|Infinity|_|(v) == null) && (|Infinity|_|(u) == null))
                            {
                                goto Label_0066;
                            }
                            goto Label_0167;
                    }
                    if (|Infinity|_|(v) == null)
                    {
                        goto Label_0066;
                    }
                    goto Label_0167;
            }
            switch (v.Tag)
            {
                case 6:
                    goto Label_0154;

                case 9:
                    if (|Infinity|_|(u) == null)
                    {
                        break;
                    }
                    goto Label_0167;
            }
        Label_0066:
            switch (u.Tag)
            {
                case 7:
                    switch (v.Tag)
                    {
                        case 8:
                            return FloatingPoint.Undef;
                    }
                    break;

                case 8:
                    switch (v.Tag)
                    {
                    }
                    goto Label_00DE;

                default:
                    switch (v.Tag)
                    {
                        case 7:
                            break;

                        case 8:
                            goto Label_00DE;

                        default:
                            throw new Exception("not supported");
                    }
                    break;
            }
            return FloatingPoint.PosInf;
        Label_00DE:
            return FloatingPoint.NegInf;
        Label_0154:
            return FloatingPoint.Undef;
        Label_0167:
            return FloatingPoint.ComplexInf;
        Label_01E4:
            return FloatingPoint.NewComplex(new System.Numerics.Complex(item, 0.0) + complex2);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static FloatingPoint fapply(MathNet.Symbolics.Function f, FloatingPoint u)
        {
            MathNet.Symbolics.FloatingPoint.Real real;
            MathNet.Symbolics.FloatingPoint.Complex complex;
            switch (f.Tag)
            {
                case 1:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Math.Log(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewComplex(System.Numerics.Complex.Log(complex.item));
                    }
                    break;

                case 2:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Math.Exp(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewComplex(System.Numerics.Complex.Exp(complex.item));
                    }
                    break;

                case 3:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Math.Sin(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewComplex(System.Numerics.Complex.Sin(complex.item));
                    }
                    break;

                case 4:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Math.Cos(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewComplex(System.Numerics.Complex.Cos(complex.item));
                    }
                    break;

                case 5:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Math.Tan(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewComplex(System.Numerics.Complex.Tan(complex.item));
                    }
                    break;

                case 6:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Trig.Cot(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewComplex(complex.item.Cot());
                    }
                    break;

                case 7:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Trig.Sec(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewComplex(complex.item.Sec());
                    }
                    break;

                case 8:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Trig.Sec(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewComplex(complex.item.Sec());
                    }
                    break;

                case 9:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Math.Cosh(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewComplex(System.Numerics.Complex.Cosh(complex.item));
                    }
                    break;

                case 10:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Math.Sinh(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewComplex(System.Numerics.Complex.Sinh(complex.item));
                    }
                    break;

                case 11:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Math.Tanh(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewComplex(System.Numerics.Complex.Tanh(complex.item));
                    }
                    break;

                case 12:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Math.Asin(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewComplex(System.Numerics.Complex.Asin(complex.item));
                    }
                    break;

                case 13:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Math.Acos(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewComplex(System.Numerics.Complex.Acos(complex.item));
                    }
                    break;

                case 14:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Math.Atan(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewComplex(System.Numerics.Complex.Atan(complex.item));
                    }
                    break;

                default:
                    switch (u.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) u;
                            return FloatingPoint.NewReal(Math.Abs(real.item));

                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                            return FloatingPoint.NewReal(System.Numerics.Complex.Abs(complex.item));

                        case 2:
                        {
                            FloatingPoint.RealVector vector = (FloatingPoint.RealVector) u;
                            return FloatingPoint.NewReal(vector.item.L2Norm());
                        }
                        case 3:
                        {
                            FloatingPoint.ComplexVector vector3 = (FloatingPoint.ComplexVector) u;
                            return FloatingPoint.NewReal(vector3.item.L2Norm());
                        }
                        case 4:
                        {
                            FloatingPoint.RealMatrix matrix = (FloatingPoint.RealMatrix) u;
                            return FloatingPoint.NewReal(matrix.item.L2Norm());
                        }
                        case 5:
                        {
                            FloatingPoint.ComplexMatrix matrix3 = (FloatingPoint.ComplexMatrix) u;
                            return FloatingPoint.NewReal(matrix3.item.L2Norm());
                        }
                    }
                    break;
            }
            throw new Exception("not supported");
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static c fapplyN<a, b, c>(a f, b xs)
        {
            throw new Exception("not supported yet");
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static FloatingPoint fmultiply(FloatingPoint u, FloatingPoint v)
        {
            MathNet.Symbolics.FloatingPoint.Real real;
            double item;
            MathNet.Symbolics.FloatingPoint.Complex complex;
            System.Numerics.Complex complex2;
            switch (u.Tag)
            {
                case 0:
                    real = (MathNet.Symbolics.FloatingPoint.Real) u;
                    switch (v.Tag)
                    {
                        case 0:
                        {
                            MathNet.Symbolics.FloatingPoint.Real real2 = (MathNet.Symbolics.FloatingPoint.Real) v;
                            item = real2.item;
                            return FloatingPoint.NewReal(real.item * item);
                        }
                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) v;
                            complex2 = complex.item;
                            item = real.item;
                            goto Label_00FA;

                        case 6:
                            goto Label_006F;

                        case 9:
                            if (|Infinity|_|(u) == null)
                            {
                                break;
                            }
                            goto Label_0080;
                    }
                    goto Label_0066;

                case 1:
                    complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                    switch (v.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) v;
                            item = real.item;
                            complex2 = complex.item;
                            goto Label_00FA;

                        case 1:
                        {
                            MathNet.Symbolics.FloatingPoint.Complex complex3 = (MathNet.Symbolics.FloatingPoint.Complex) v;
                            complex2 = complex3.item;
                            return FloatingPoint.NewComplex(complex.item * complex2);
                        }
                        case 6:
                            goto Label_006F;

                        case 9:
                            if (|Infinity|_|(u) == null)
                            {
                                break;
                            }
                            goto Label_0080;
                    }
                    goto Label_0066;

                case 2:
                {
                    FloatingPoint.RealVector vector = (FloatingPoint.RealVector) u;
                    switch (v.Tag)
                    {
                        case 2:
                        {
                            FloatingPoint.RealVector vector2 = (FloatingPoint.RealVector) v;
                            Vector<double> vector3 = vector2.item;
                            return FloatingPoint.NewReal((double) (vector.item * vector3));
                        }
                        case 6:
                            goto Label_006F;

                        case 9:
                            if (|Infinity|_|(u) == null)
                            {
                                break;
                            }
                            goto Label_0080;
                    }
                    goto Label_0066;
                }
                case 3:
                {
                    FloatingPoint.ComplexVector vector5 = (FloatingPoint.ComplexVector) u;
                    switch (v.Tag)
                    {
                        case 3:
                        {
                            FloatingPoint.ComplexVector vector6 = (FloatingPoint.ComplexVector) v;
                            Vector<System.Numerics.Complex> vector7 = vector6.item;
                            return FloatingPoint.NewComplex((System.Numerics.Complex) (vector5.item * vector7));
                        }
                        case 6:
                            goto Label_006F;

                        case 9:
                            if (|Infinity|_|(u) == null)
                            {
                                break;
                            }
                            goto Label_0080;
                    }
                    goto Label_0066;
                }
                case 4:
                {
                    FloatingPoint.RealMatrix matrix = (FloatingPoint.RealMatrix) u;
                    switch (v.Tag)
                    {
                        case 4:
                        {
                            FloatingPoint.RealMatrix matrix2 = (FloatingPoint.RealMatrix) v;
                            Matrix<double> matrix3 = matrix2.item;
                            return FloatingPoint.NewRealMatrix(matrix.item * matrix3);
                        }
                        case 6:
                            goto Label_006F;

                        case 9:
                            if (|Infinity|_|(u) == null)
                            {
                                break;
                            }
                            goto Label_0080;
                    }
                    goto Label_0066;
                }
                case 5:
                {
                    FloatingPoint.ComplexMatrix matrix5 = (FloatingPoint.ComplexMatrix) u;
                    switch (v.Tag)
                    {
                        case 5:
                        {
                            FloatingPoint.ComplexMatrix matrix6 = (FloatingPoint.ComplexMatrix) v;
                            Matrix<System.Numerics.Complex> matrix7 = matrix6.item;
                            return FloatingPoint.NewComplexMatrix(matrix5.item * matrix7);
                        }
                        case 6:
                            goto Label_006F;

                        case 9:
                            if (|Infinity|_|(u) == null)
                            {
                                break;
                            }
                            goto Label_0080;
                    }
                    goto Label_0066;
                }
                case 6:
                    goto Label_006F;

                case 9:
                    switch (v.Tag)
                    {
                        case 6:
                            goto Label_006F;

                        case 9:
                            if ((|Infinity|_|(v) == null) && (|Infinity|_|(u) == null))
                            {
                                goto Label_0066;
                            }
                            goto Label_0080;
                    }
                    if (|Infinity|_|(v) == null)
                    {
                        goto Label_0066;
                    }
                    goto Label_0080;
            }
            switch (v.Tag)
            {
                case 6:
                    goto Label_006F;

                case 9:
                    if (|Infinity|_|(u) == null)
                    {
                        break;
                    }
                    goto Label_0080;
            }
        Label_0066:
            return fmultiply$cont@98(u, v, null);
        Label_006F:
            return FloatingPoint.Undef;
        Label_0080:
            return FloatingPoint.ComplexInf;
        Label_00FA:
            return FloatingPoint.NewComplex(new System.Numerics.Complex(item, 0.0) * complex2);
        }

        [CompilerGenerated]
        internal static FloatingPoint fmultiply$cont@98(FloatingPoint u, FloatingPoint v, Unit unitVar)
        {
            double item;
            switch (u.Tag)
            {
                case 0:
                {
                    MathNet.Symbolics.FloatingPoint.Real real = (MathNet.Symbolics.FloatingPoint.Real) u;
                    switch (v.Tag)
                    {
                        case 7:
                            item = real.item;
                            goto Label_00C5;

                        case 8:
                            item = real.item;
                            goto Label_0142;
                    }
                    break;
                }
                case 7:
                    switch (v.Tag)
                    {
                        case 0:
                            item = ((MathNet.Symbolics.FloatingPoint.Real) v).item;
                            goto Label_00C5;

                        case 8:
                            return FloatingPoint.NegInf;
                    }
                    goto Label_0072;

                case 8:
                    switch (v.Tag)
                    {
                        case 0:
                            item = ((MathNet.Symbolics.FloatingPoint.Real) v).item;
                            goto Label_0142;

                        case 7:
                            goto Label_0072;
                    }
                    goto Label_0078;

                default:
                    switch (v.Tag)
                    {
                        case 7:
                            goto Label_0072;

                        case 8:
                            goto Label_0078;
                    }
                    break;
            }
            throw new Exception("not supported");
        Label_0072:
            return FloatingPoint.PosInf;
        Label_0078:
            return FloatingPoint.NegInf;
        Label_00C5:
            if (item < 0.0)
            {
                return FloatingPoint.NegInf;
            }
            if (item > 0.0)
            {
                return FloatingPoint.PosInf;
            }
            return FloatingPoint.Undef;
        Label_0142:
            if (item < 0.0)
            {
                return FloatingPoint.PosInf;
            }
            if (item > 0.0)
            {
                return FloatingPoint.NegInf;
            }
            return FloatingPoint.Undef;
        }

        public static FloatingPoint fnormalize(FloatingPoint _arg1)
        {
            double item;
            System.Numerics.Complex complex2;
        Label_0000:
            if (_arg1.Tag == 0)
            {
                MathNet.Symbolics.FloatingPoint.Real real = (MathNet.Symbolics.FloatingPoint.Real) _arg1;
                if (double.IsPositiveInfinity(real.item))
                {
                    item = real.item;
                    return FloatingPoint.PosInf;
                }
            }
            if (_arg1.Tag == 0)
            {
                MathNet.Symbolics.FloatingPoint.Real real2 = (MathNet.Symbolics.FloatingPoint.Real) _arg1;
                if (double.IsNegativeInfinity(real2.item))
                {
                    item = real2.item;
                    return FloatingPoint.NegInf;
                }
            }
            if (_arg1.Tag == 0)
            {
                MathNet.Symbolics.FloatingPoint.Real real3 = (MathNet.Symbolics.FloatingPoint.Real) _arg1;
                if (double.IsInfinity(real3.item))
                {
                    item = real3.item;
                    return FloatingPoint.ComplexInf;
                }
            }
            if (_arg1.Tag == 0)
            {
                MathNet.Symbolics.FloatingPoint.Real real4 = (MathNet.Symbolics.FloatingPoint.Real) _arg1;
                if (double.IsNaN(real4.item))
                {
                    item = real4.item;
                    return FloatingPoint.Undef;
                }
            }
            if (_arg1.Tag == 1)
            {
                MathNet.Symbolics.FloatingPoint.Complex complex = (MathNet.Symbolics.FloatingPoint.Complex) _arg1;
                complex2 = complex.item;
                if (complex2.IsInfinity() && complex2.IsReal())
                {
                    if (complex.item.Real > 0.0)
                    {
                        return FloatingPoint.PosInf;
                    }
                    return FloatingPoint.NegInf;
                }
            }
            if (_arg1.Tag == 1)
            {
                MathNet.Symbolics.FloatingPoint.Complex complex3 = (MathNet.Symbolics.FloatingPoint.Complex) _arg1;
                if (complex3.item.IsInfinity())
                {
                    complex2 = complex3.item;
                    return FloatingPoint.ComplexInf;
                }
            }
            if (_arg1.Tag == 1)
            {
                MathNet.Symbolics.FloatingPoint.Complex complex4 = (MathNet.Symbolics.FloatingPoint.Complex) _arg1;
                if (complex4.item.IsReal())
                {
                    _arg1 = FloatingPoint.NewReal(complex4.item.Real);
                    goto Label_0000;
                }
            }
            if (_arg1.Tag == 1)
            {
                MathNet.Symbolics.FloatingPoint.Complex complex5 = (MathNet.Symbolics.FloatingPoint.Complex) _arg1;
                if (complex5.item.IsNaN())
                {
                    complex2 = complex5.item;
                    return FloatingPoint.Undef;
                }
            }
            return _arg1;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static FloatingPoint fpower(FloatingPoint u, FloatingPoint v)
        {
            MathNet.Symbolics.FloatingPoint.Real real;
            double item;
            MathNet.Symbolics.FloatingPoint.Complex complex;
            System.Numerics.Complex complex2;
            switch (u.Tag)
            {
                case 0:
                    real = (MathNet.Symbolics.FloatingPoint.Real) u;
                    switch (v.Tag)
                    {
                        case 0:
                        {
                            MathNet.Symbolics.FloatingPoint.Real real2 = (MathNet.Symbolics.FloatingPoint.Real) v;
                            item = real2.item;
                            return FloatingPoint.NewReal(Math.Pow(real.item, item));
                        }
                        case 1:
                            complex = (MathNet.Symbolics.FloatingPoint.Complex) v;
                            complex2 = complex.item;
                            return FloatingPoint.NewComplex(System.Numerics.Complex.Pow(new System.Numerics.Complex(real.item, 0.0), complex2));

                        case 6:
                            goto Label_00AD;

                        case 9:
                            if (|Infinity|_|(u) == null)
                            {
                                break;
                            }
                            goto Label_00BD;
                    }
                    goto Label_0066;

                case 1:
                    complex = (MathNet.Symbolics.FloatingPoint.Complex) u;
                    switch (v.Tag)
                    {
                        case 0:
                            real = (MathNet.Symbolics.FloatingPoint.Real) v;
                            item = real.item;
                            return FloatingPoint.NewComplex(System.Numerics.Complex.Pow(complex.item, item));

                        case 1:
                        {
                            MathNet.Symbolics.FloatingPoint.Complex complex3 = (MathNet.Symbolics.FloatingPoint.Complex) v;
                            complex2 = complex3.item;
                            return FloatingPoint.NewComplex(System.Numerics.Complex.Pow(complex.item, complex2));
                        }
                        case 6:
                            goto Label_00AD;

                        case 9:
                            if (|Infinity|_|(u) == null)
                            {
                                break;
                            }
                            goto Label_00BD;
                    }
                    goto Label_0066;

                case 6:
                    goto Label_00AD;

                case 9:
                    switch (v.Tag)
                    {
                        case 6:
                            goto Label_00AD;

                        case 9:
                            if ((|Infinity|_|(v) == null) && (|Infinity|_|(u) == null))
                            {
                                goto Label_0066;
                            }
                            goto Label_00BD;
                    }
                    if (|Infinity|_|(v) == null)
                    {
                        goto Label_0066;
                    }
                    goto Label_00BD;
            }
            switch (v.Tag)
            {
                case 6:
                    goto Label_00AD;

                case 9:
                    if (|Infinity|_|(u) == null)
                    {
                        break;
                    }
                    goto Label_00BD;
            }
        Label_0066:
            if ((|Infinity|_|(u) != null) && (v.Tag == 7))
            {
                return FloatingPoint.ComplexInf;
            }
            if ((|Infinity|_|(u) != null) && (v.Tag == 8))
            {
                return FloatingPoint.NewReal(0.0);
            }
            throw new Exception("not supported");
        Label_00AD:
            return FloatingPoint.Undef;
        Label_00BD:
            return FloatingPoint.ComplexInf;
        }

        [CompilationSourceName("freal")]
        public static FloatingPoint Real(double x) => 
            FloatingPoint.NewReal(x);

        [Serializable]
        internal class Evaluate@184 : FSharpFunc<string, FloatingPoint>
        {
            internal Evaluate@184()
            {
            }

            public override FloatingPoint Invoke(string message)
            {
                throw new Exception(message);
            }
        }

        [Serializable]
        internal class Evaluate@185-1 : OptimizedClosures.FSharpFunc<FloatingPoint, FloatingPoint, FloatingPoint>
        {
            internal Evaluate@185-1()
            {
            }

            public override FloatingPoint Invoke(FloatingPoint u, FloatingPoint v) => 
                MathNet.Symbolics.Evaluate.fadd(u, v);
        }

        [Serializable]
        internal class Evaluate@185-2 : FSharpFunc<Expression, FloatingPoint>
        {
            public IDictionary<string, FloatingPoint> symbols;

            internal Evaluate@185-2(IDictionary<string, FloatingPoint> symbols)
            {
                this.symbols = symbols;
            }

            public override FloatingPoint Invoke(Expression _arg1) => 
                MathNet.Symbolics.Evaluate.Evaluate(this.symbols, _arg1);
        }

        [Serializable]
        internal class Evaluate@186-3 : OptimizedClosures.FSharpFunc<FloatingPoint, FloatingPoint, FloatingPoint>
        {
            internal Evaluate@186-3()
            {
            }

            public override FloatingPoint Invoke(FloatingPoint u, FloatingPoint v) => 
                MathNet.Symbolics.Evaluate.fmultiply(u, v);
        }

        [Serializable]
        internal class Evaluate@186-4 : FSharpFunc<Expression, FloatingPoint>
        {
            public IDictionary<string, FloatingPoint> symbols;

            internal Evaluate@186-4(IDictionary<string, FloatingPoint> symbols)
            {
                this.symbols = symbols;
            }

            public override FloatingPoint Invoke(Expression _arg1) => 
                MathNet.Symbolics.Evaluate.Evaluate(this.symbols, _arg1);
        }

        [Serializable]
        internal class Evaluate@189-5 : FSharpFunc<Expression, FloatingPoint>
        {
            public IDictionary<string, FloatingPoint> symbols;

            internal Evaluate@189-5(IDictionary<string, FloatingPoint> symbols)
            {
                this.symbols = symbols;
            }

            public override FloatingPoint Invoke(Expression _arg1) => 
                MathNet.Symbolics.Evaluate.Evaluate(this.symbols, _arg1);
        }
    }
}

