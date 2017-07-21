namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Core;
    using System;
    using System.Linq.Expressions;
    using System.Numerics;

    [RequireQualifiedAccess, CompilationMapping(SourceConstructFlags.Module)]
    public static class Linq
    {
        [CompilationSourceName("parse")]
        public static MathNet.Symbolics.Expression Parse(System.Linq.Expressions.Expression q)
        {
            BinaryExpression expression3;
            BinaryExpression expression4;
            MathNet.Symbolics.Expression expression5;
            MathNet.Symbolics.Expression expression6;
            UnaryExpression expression9;
            UnaryExpression expression10;
        Label_0000:
            switch (q.NodeType)
            {
                case ExpressionType.Add:
                    expression3 = q as BinaryExpression;
                    if (expression3 == null)
                    {
                        break;
                    }
                    expression4 = expression3;
                    return MathNet.Symbolics.Operators.add(Parse(expression4.Left), Parse(expression4.Right));

                case ExpressionType.Constant:
                {
                    ConstantExpression expression15 = q as ConstantExpression;
                    if (expression15 == null)
                    {
                        break;
                    }
                    ConstantExpression expression16 = expression15;
                    return MathNet.Symbolics.Expression.NewNumber(BigRational.FromBigInt(new BigInteger(Convert.ToInt64(expression16.Value))));
                }
                case ExpressionType.Convert:
                    expression9 = q as UnaryExpression;
                    if (expression9 == null)
                    {
                        break;
                    }
                    expression10 = expression9;
                    q = expression10.Operand;
                    goto Label_0000;

                case ExpressionType.Divide:
                    expression3 = q as BinaryExpression;
                    if (expression3 == null)
                    {
                        break;
                    }
                    expression4 = expression3;
                    expression5 = Parse(expression4.Left);
                    expression6 = Parse(expression4.Right);
                    return MathNet.Symbolics.Operators.multiply(expression5, MathNet.Symbolics.Operators.invert(expression6));

                case ExpressionType.Lambda:
                {
                    LambdaExpression expression13 = q as LambdaExpression;
                    if (expression13 == null)
                    {
                        break;
                    }
                    LambdaExpression expression14 = expression13;
                    q = expression14.Body;
                    goto Label_0000;
                }
                case ExpressionType.MemberAccess:
                {
                    MemberExpression expression11 = q as MemberExpression;
                    if (expression11 == null)
                    {
                        break;
                    }
                    MemberExpression expression12 = expression11;
                    return MathNet.Symbolics.Expression.NewIdentifier(Symbol.NewSymbol(expression12.Member.Name));
                }
                case ExpressionType.Multiply:
                    expression3 = q as BinaryExpression;
                    if (expression3 == null)
                    {
                        break;
                    }
                    expression4 = expression3;
                    return MathNet.Symbolics.Operators.multiply(Parse(expression4.Left), Parse(expression4.Right));

                case ExpressionType.Negate:
                    expression9 = q as UnaryExpression;
                    if (expression9 == null)
                    {
                        break;
                    }
                    expression10 = expression9;
                    return MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, Parse(expression10.Operand));

                case ExpressionType.UnaryPlus:
                    expression9 = q as UnaryExpression;
                    if (expression9 == null)
                    {
                        break;
                    }
                    expression10 = expression9;
                    q = expression10.Operand;
                    goto Label_0000;

                case ExpressionType.Parameter:
                {
                    ParameterExpression expression7 = q as ParameterExpression;
                    if (expression7 == null)
                    {
                        break;
                    }
                    ParameterExpression expression8 = expression7;
                    return MathNet.Symbolics.Expression.NewIdentifier(Symbol.NewSymbol(expression8.Name));
                }
                case ExpressionType.Subtract:
                    expression3 = q as BinaryExpression;
                    if (expression3 == null)
                    {
                        break;
                    }
                    expression4 = expression3;
                    expression5 = Parse(expression4.Left);
                    expression6 = Parse(expression4.Right);
                    return MathNet.Symbolics.Operators.add(expression5, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression6));

                case ExpressionType.Try:
                {
                    TryExpression expression = q as TryExpression;
                    if (expression == null)
                    {
                        break;
                    }
                    TryExpression expression2 = expression;
                    q = expression2.Body;
                    goto Label_0000;
                }
            }
            throw new Exception("not supported");
        }
    }
}

