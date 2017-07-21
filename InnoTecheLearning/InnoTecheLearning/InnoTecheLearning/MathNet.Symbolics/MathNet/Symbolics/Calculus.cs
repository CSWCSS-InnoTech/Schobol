namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;

    [RequireQualifiedAccess, CompilationMapping(SourceConstructFlags.Module)]
    public static class Calculus
    {
        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("differentiate")]
        public static Expression Differentiate(Expression symbol, Expression _arg1)
        {
            Expression expression;
            FSharpList<Expression> item;
            Expression expression2;
            Expression expression3;
            Expression.Function function;
        Label_0000:
            if (_arg1.Equals(symbol, LanguagePrimitives.GenericEqualityComparer))
            {
                return MathNet.Symbolics.Operators.one;
            }
            switch (_arg1.Tag)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 9:
                case 10:
                case 11:
                    return MathNet.Symbolics.Operators.zero;

                case 4:
                    item = ((Expression.Sum) _arg1).item;
                    return MathNet.Symbolics.Operators.sum(ListModule.Map<Expression, Expression>(new Differentiate@14(symbol), item));

                case 5:
                {
                    Expression.Product product = (Expression.Product) _arg1;
                    if (product.item.TailOrNull == null)
                    {
                        throw new Exception("invalid expression");
                    }
                    item = product.item;
                    if (item.TailOrNull.TailOrNull == null)
                    {
                        expression = item.HeadOrDefault;
                        _arg1 = expression;
                        symbol = symbol;
                        goto Label_0000;
                    }
                    FSharpList<Expression> list2 = item.TailOrNull;
                    expression = item.HeadOrDefault;
                    expression2 = Differentiate(symbol, expression);
                    expression3 = Differentiate(symbol, Expression.NewProduct(list2));
                    return MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.multiply(expression, expression3), MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.product(list2), expression2));
                }
                case 6:
                {
                    Expression.Power power = (Expression.Power) _arg1;
                    expression = power.item1;
                    expression2 = _arg1;
                    expression3 = power.item2;
                    Expression y = Differentiate(symbol, expression);
                    return MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(Differentiate(symbol, expression3), MathNet.Symbolics.Operators.ln(expression)), expression2), MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(expression3, y), MathNet.Symbolics.Operators.pow(expression, expression3 - 1)));
                }
                case 7:
                    function = (Expression.Function) _arg1;
                    switch (function.item1.Tag)
                    {
                        case 0:
                            throw new Exception("not supported");

                        case 1:
                            expression = function.item2;
                            return MathNet.Symbolics.Operators.multiply(Differentiate(symbol, expression), MathNet.Symbolics.Operators.invert(expression));

                        case 3:
                            expression = function.item2;
                            return MathNet.Symbolics.Operators.multiply(Differentiate(symbol, expression), MathNet.Symbolics.Operators.cos(expression));

                        case 4:
                            expression = function.item2;
                            return MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, Differentiate(symbol, expression)), MathNet.Symbolics.Operators.sin(expression));

                        case 5:
                            expression = function.item2;
                            expression3 = Differentiate(symbol, expression);
                            expression2 = MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(2), expression3);
                            expression3 = MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.cos(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(2), expression)), MathNet.Symbolics.Operators.number.Invoke(1));
                            return MathNet.Symbolics.Operators.multiply(expression2, MathNet.Symbolics.Operators.invert(expression3));

                        case 6:
                            expression = function.item2;
                            expression2 = Expression.NewNumber(BigRational.FromInt(-1));
                            expression3 = MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.sin(expression), MathNet.Symbolics.Operators.sin(expression));
                            return MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(expression2, MathNet.Symbolics.Operators.invert(expression3)), Differentiate(symbol, expression));

                        case 7:
                            expression = function.item2;
                            return MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.tan(expression), Expression.NewFunction(MathNet.Symbolics.Function.Sec, expression)), Differentiate(symbol, expression));

                        case 8:
                            expression = function.item2;
                            return MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, Expression.NewFunction(MathNet.Symbolics.Function.Cot, expression)), Expression.NewFunction(MathNet.Symbolics.Function.Csc, expression)), Differentiate(symbol, expression));

                        case 9:
                            expression = function.item2;
                            return MathNet.Symbolics.Operators.multiply(Differentiate(symbol, expression), Expression.NewFunction(MathNet.Symbolics.Function.Sinh, expression));

                        case 10:
                            expression = function.item2;
                            return MathNet.Symbolics.Operators.multiply(Differentiate(symbol, expression), Expression.NewFunction(MathNet.Symbolics.Function.Cosh, expression));

                        case 11:
                            expression = function.item2;
                            expression3 = Differentiate(symbol, expression);
                            expression2 = MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(2), expression3);
                            expression3 = MathNet.Symbolics.Operators.add(Expression.NewFunction(MathNet.Symbolics.Function.Cosh, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(2), expression)), MathNet.Symbolics.Operators.number.Invoke(1));
                            return MathNet.Symbolics.Operators.multiply(expression2, MathNet.Symbolics.Operators.invert(expression3));

                        case 12:
                            expression = function.item2;
                            expression2 = Expression.NewPower(MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.one, MathNet.Symbolics.Operators.pow(expression, Expression.NewNumber(BigRational.FromInt(2)))), Expression.NewPower(MathNet.Symbolics.Operators.two, MathNet.Symbolics.Operators.minusOne));
                            return MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.one, MathNet.Symbolics.Operators.invert(expression2)), Differentiate(symbol, expression));

                        case 13:
                            expression = function.item2;
                            expression2 = Expression.NewNumber(BigRational.FromInt(-1));
                            expression3 = Expression.NewPower(MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.one, MathNet.Symbolics.Operators.pow(expression, Expression.NewNumber(BigRational.FromInt(2)))), Expression.NewPower(MathNet.Symbolics.Operators.two, MathNet.Symbolics.Operators.minusOne));
                            return MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(expression2, MathNet.Symbolics.Operators.invert(expression3)), Differentiate(symbol, expression));

                        case 14:
                            expression = function.item2;
                            return MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.one, MathNet.Symbolics.Operators.invert(MathNet.Symbolics.Operators.one)), MathNet.Symbolics.Operators.pow(expression, Expression.NewNumber(BigRational.FromInt(2)))), Differentiate(symbol, expression));
                    }
                    break;

                case 8:
                {
                    Expression.FunctionN nn = (Expression.FunctionN) _arg1;
                    item = nn.item2;
                    MathNet.Symbolics.Function function2 = nn.item1;
                    throw new Exception("not supported");
                }
                case 12:
                    return _arg1;

                default:
                    return _arg1;
            }
            expression = function.item2;
            expression2 = _arg1;
            return MathNet.Symbolics.Operators.multiply(Differentiate(symbol, expression), expression2);
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("differentiateAt")]
        public static Expression DifferentiateAt(Expression symbol, Expression value, Expression expression) => 
            Structure.Substitute(symbol, value, Differentiate(symbol, expression));

        internal static Expression impl@50-8(int k, Expression symbol, Expression value, int n, int nf, Expression acc, Expression dxn)
        {
            while (n != k)
            {
                Expression x = Structure.Substitute(symbol, value, dxn);
                Expression expression2 = MathNet.Symbolics.Operators.number.Invoke(nf);
                dxn = Differentiate(symbol, dxn);
                acc = MathNet.Symbolics.Operators.add(acc, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(x, MathNet.Symbolics.Operators.invert(expression2)), MathNet.Symbolics.Operators.pow(MathNet.Symbolics.Operators.add(symbol, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, value)), MathNet.Symbolics.Operators.number.Invoke(n))));
                nf *= n + 1;
                n++;
                value = value;
                symbol = symbol;
                k = k;
            }
            return acc;
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("normalLine")]
        public static Expression NormalLine(Expression symbol, Expression value, Expression expression)
        {
            Expression expression2 = Structure.Substitute(symbol, value, Differentiate(symbol, expression));
            Expression y = Structure.Substitute(symbol, value, expression);
            return Algebraic.Expand(MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(1), MathNet.Symbolics.Operators.invert(expression2))), MathNet.Symbolics.Operators.add(symbol, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, value))), y));
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("tangentLine")]
        public static Expression TangentLine(Expression symbol, Expression value, Expression expression)
        {
            Expression x = Structure.Substitute(symbol, value, Differentiate(symbol, expression));
            Expression y = Structure.Substitute(symbol, value, expression);
            return Algebraic.Expand(MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.multiply(x, MathNet.Symbolics.Operators.add(symbol, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, value))), y));
        }

        [CompilationArgumentCounts(new int[] { 1, 1, 1, 1 }), CompilationSourceName("taylor")]
        public static Expression Taylor(int k, Expression symbol, Expression value, Expression expression)
        {
            int num = k;
            Expression expression2 = symbol;
            Expression expression3 = value;
            return Algebraic.Expand(impl@50-8(k, symbol, value, 0, 1, MathNet.Symbolics.Operators.zero, expression));
        }

        [Serializable]
        internal class Differentiate@14 : FSharpFunc<Expression, Expression>
        {
            public Expression symbol;

            internal Differentiate@14(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override Expression Invoke(Expression _arg1) => 
                Calculus.Differentiate(this.symbol, _arg1);
        }
    }
}

