namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Core;
    using System;
    using System.Numerics;

    [CompilationMapping(SourceConstructFlags.Module)]
    public static class ExpressionPatterns
    {
        public static FSharpOption<BigRational> Integer(Expression _arg1)
        {
            if (_arg1.Tag == 0)
            {
                Expression.Number number = (Expression.Number) _arg1;
                if (number.item.IsInteger)
                {
                    BigRational item = number.item;
                    return FSharpOption<BigRational>.Some(item);
                }
            }
            return null;
        }

        public static FSharpOption<Unit> MinusOne(Expression _arg1)
        {
            if (_arg1.Tag == 0)
            {
                Expression.Number number = (Expression.Number) _arg1;
                BigRational item = number.item;
                if (item.IsInteger && LanguagePrimitives.HashCompare.GenericEqualityIntrinsic<BigInteger>(item.Numerator, BigInteger.MinusOne))
                {
                    item = number.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            if (_arg1.Tag == 1)
            {
                Expression.Approximation approximation = (Expression.Approximation) _arg1;
                if (ApproximationModule.isMinusOne(approximation.item))
                {
                    MathNet.Symbolics.Approximation approximation2 = approximation.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            return null;
        }

        public static FSharpOption<Unit> Negative(Expression _arg1)
        {
            if (_arg1.Tag == 0)
            {
                Expression.Number number = (Expression.Number) _arg1;
                if (number.item.IsNegative)
                {
                    BigRational item = number.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            if (_arg1.Tag == 1)
            {
                Expression.Approximation approximation = (Expression.Approximation) _arg1;
                if (ApproximationModule.isNegative(approximation.item))
                {
                    MathNet.Symbolics.Approximation approximation2 = approximation.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            if (_arg1.Tag == 11)
            {
                return FSharpOption<Unit>.Some(null);
            }
            return null;
        }

        public static FSharpOption<Tuple<Expression, Expression>> NegIntPower(Expression _arg1)
        {
            if (_arg1.Tag == 6)
            {
                Expression.Power power = (Expression.Power) _arg1;
                if (power.item2.Tag == 0)
                {
                    Expression.Number number = (Expression.Number) power.item2;
                    BigRational item = number.item;
                    if (item.IsInteger && item.IsNegative)
                    {
                        Expression expression = power.item1;
                        Expression expression2 = power.item2;
                        item = number.item;
                        return FSharpOption<Tuple<Expression, Expression>>.Some(new Tuple<Expression, Expression>(expression, expression2));
                    }
                }
            }
            return null;
        }

        public static FSharpOption<Tuple<Expression, Expression>> NegPower(Expression _arg1)
        {
            if (_arg1.Tag == 6)
            {
                Expression.Power power = (Expression.Power) _arg1;
                if (|Negative|_|(power.item2) != null)
                {
                    Expression expression = power.item1;
                    Expression expression2 = power.item2;
                    return FSharpOption<Tuple<Expression, Expression>>.Some(new Tuple<Expression, Expression>(expression, expression2));
                }
            }
            return null;
        }

        public static FSharpOption<Tuple<Expression, Expression>> NegRationalPower(Expression _arg1)
        {
            if (_arg1.Tag == 6)
            {
                Expression.Power power = (Expression.Power) _arg1;
                if (power.item2.Tag == 0)
                {
                    Expression.Number number = (Expression.Number) power.item2;
                    if (number.item.IsNegative)
                    {
                        Expression expression = power.item1;
                        Expression expression2 = power.item2;
                        BigRational item = number.item;
                        return FSharpOption<Tuple<Expression, Expression>>.Some(new Tuple<Expression, Expression>(expression, expression2));
                    }
                }
            }
            return null;
        }

        public static FSharpOption<Unit> One(Expression _arg1)
        {
            if (_arg1.Tag == 0)
            {
                Expression.Number number = (Expression.Number) _arg1;
                if (number.item.IsOne)
                {
                    BigRational item = number.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            if (_arg1.Tag == 1)
            {
                Expression.Approximation approximation = (Expression.Approximation) _arg1;
                if (ApproximationModule.isOne(approximation.item))
                {
                    MathNet.Symbolics.Approximation approximation2 = approximation.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            return null;
        }

        public static FSharpOption<Tuple<Expression, Expression>> PosIntPower(Expression _arg1)
        {
            if (_arg1.Tag == 6)
            {
                Expression.Power power = (Expression.Power) _arg1;
                if (power.item2.Tag == 0)
                {
                    Expression.Number number = (Expression.Number) power.item2;
                    BigRational item = number.item;
                    if (item.IsInteger && item.IsPositive)
                    {
                        Expression expression = power.item1;
                        Expression expression2 = power.item2;
                        item = number.item;
                        return FSharpOption<Tuple<Expression, Expression>>.Some(new Tuple<Expression, Expression>(expression, expression2));
                    }
                }
            }
            return null;
        }

        public static FSharpOption<Unit> Positive(Expression _arg1)
        {
            if (_arg1.Tag == 0)
            {
                Expression.Number number = (Expression.Number) _arg1;
                if (number.item.IsPositive)
                {
                    BigRational item = number.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            switch (_arg1.Tag)
            {
                case 1:
                {
                    Expression.Approximation approximation = (Expression.Approximation) _arg1;
                    if (!ApproximationModule.isPositive(approximation.item))
                    {
                        break;
                    }
                    MathNet.Symbolics.Approximation approximation2 = approximation.item;
                    return FSharpOption<Unit>.Some(null);
                }
                case 3:
                {
                    Expression.Constant constant = (Expression.Constant) _arg1;
                    switch (constant.item.Tag)
                    {
                        case 0:
                        case 1:
                            return FSharpOption<Unit>.Some(null);
                    }
                    break;
                }
                case 10:
                    return FSharpOption<Unit>.Some(null);
            }
            return null;
        }

        public static FSharpOption<Expression> SinCos(Expression _arg1)
        {
            Expression expression;
            if (_arg1.Tag == 7)
            {
                Expression.Function function = (Expression.Function) _arg1;
                switch (function.item1.Tag)
                {
                    case 3:
                        expression = _arg1;
                        goto Label_0061;

                    case 4:
                        expression = _arg1;
                        goto Label_0061;
                }
            }
            return null;
        Label_0061:
            return FSharpOption<Expression>.Some(expression);
        }

        public static FSharpOption<Tuple<Expression, Expression>> SinCosPosIntPower(Expression _arg1)
        {
            Expression.Power power;
            Expression.Function function;
            Expression.Number number;
            BigRational item;
            Expression expression;
            Expression expression2;
            switch (_arg1.Tag)
            {
                case 6:
                    power = (Expression.Power) _arg1;
                    if (power.item1.Tag != 7)
                    {
                        break;
                    }
                    function = (Expression.Function) power.item1;
                    if ((function.item1.Tag != 3) || (power.item2.Tag != 0))
                    {
                        break;
                    }
                    number = (Expression.Number) power.item2;
                    item = number.item;
                    if (!(item.IsInteger && item.IsPositive))
                    {
                        break;
                    }
                    expression = power.item1;
                    expression2 = power.item2;
                    item = number.item;
                    return FSharpOption<Tuple<Expression, Expression>>.Some(new Tuple<Expression, Expression>(expression, expression2));

                case 7:
                    function = (Expression.Function) _arg1;
                    switch (function.item1.Tag)
                    {
                        case 3:
                            expression = _arg1;
                            goto Label_0137;

                        case 4:
                            expression = _arg1;
                            goto Label_0137;
                    }
                    break;
            }
            if (_arg1.Tag == 6)
            {
                power = (Expression.Power) _arg1;
                if (power.item1.Tag == 7)
                {
                    function = (Expression.Function) power.item1;
                    if ((function.item1.Tag == 4) && (power.item2.Tag == 0))
                    {
                        number = (Expression.Number) power.item2;
                        item = number.item;
                        if (item.IsInteger && item.IsPositive)
                        {
                            expression = power.item1;
                            expression2 = power.item2;
                            item = number.item;
                            return FSharpOption<Tuple<Expression, Expression>>.Some(new Tuple<Expression, Expression>(expression, expression2));
                        }
                    }
                }
            }
            return null;
        Label_0137:
            return FSharpOption<Tuple<Expression, Expression>>.Some(new Tuple<Expression, Expression>(expression, Expression.NewNumber(BigRational.One)));
        }

        public static FSharpOption<Expression> Terminal(Expression _arg1)
        {
            Expression expression;
            switch (_arg1.Tag)
            {
                case 0:
                    expression = _arg1;
                    break;

                case 2:
                    expression = _arg1;
                    break;

                case 3:
                    expression = _arg1;
                    break;

                default:
                    return null;
            }
            return FSharpOption<Expression>.Some(expression);
        }

        public static FSharpOption<Unit> Zero(Expression _arg1)
        {
            if (_arg1.Tag == 0)
            {
                Expression.Number number = (Expression.Number) _arg1;
                if (number.item.IsZero)
                {
                    BigRational item = number.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            if (_arg1.Tag == 1)
            {
                Expression.Approximation approximation = (Expression.Approximation) _arg1;
                if (ApproximationModule.isZero(approximation.item))
                {
                    MathNet.Symbolics.Approximation approximation2 = approximation.item;
                    return FSharpOption<Unit>.Some(null);
                }
            }
            return null;
        }
    }
}

