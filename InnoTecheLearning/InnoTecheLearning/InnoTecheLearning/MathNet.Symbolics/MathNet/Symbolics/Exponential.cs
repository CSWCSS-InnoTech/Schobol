namespace MathNet.Symbolics
{
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;

    [RequireQualifiedAccess, CompilationMapping(SourceConstructFlags.Module)]
    public static class Exponential
    {
        [CompilationSourceName("contract")]
        public static Expression Contract(Expression x)
        {
            Expression expression2;
            Expression expression = Structure.Map(new Contract@43(), x);
            switch (expression.Tag)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    return expression;

                case 5:
                    expression2 = expression;
                    break;

                case 6:
                    expression2 = expression;
                    break;

                default:
                    return expression;
            }
            return rules@30(expression2);
        }

        [CompilationSourceName("expand")]
        public static Expression Expand(Expression x)
        {
            Expression expression = Structure.Map(new Expand@23-3(), x);
            if (expression.Tag == 7)
            {
                Expression.Function function = (Expression.Function) expression;
                switch (function.item1.Tag)
                {
                    case 0:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                        return expression;

                    case 1:
                        return lnRules@19(Algebraic.Expand(function.item2));

                    case 2:
                        return expRules@15(Algebraic.Expand(function.item2));
                }
            }
            return expression;
        }

        internal static Expression expRules@15(Expression _arg1)
        {
            FSharpList<Expression> item;
            FSharpFunc<Expression, Expression> mapping = new expRules@15-1();
            switch (_arg1.Tag)
            {
                case 4:
                    item = ((Expression.Sum) _arg1).item;
                    return MathNet.Symbolics.Operators.product(ListModule.Map<Expression, Expression>(mapping, item));

                case 5:
                {
                    Expression.Product product = (Expression.Product) _arg1;
                    if (product.item.TailOrNull == null)
                    {
                        break;
                    }
                    item = product.item;
                    if (ExpressionPatterns.|Integer|_|(item.HeadOrDefault) == null)
                    {
                        break;
                    }
                    Expression y = item.HeadOrDefault;
                    return MathNet.Symbolics.Operators.pow(expRules@15(MathNet.Symbolics.Operators.product(item.TailOrNull)), y);
                }
            }
            return MathNet.Symbolics.Operators.exp(_arg1);
        }

        internal static Expression lnRules@19(Expression _arg2)
        {
            FSharpFunc<Expression, Expression> mapping = new lnRules@19-1();
            switch (_arg2.Tag)
            {
                case 5:
                {
                    Expression.Product product = (Expression.Product) _arg2;
                    FSharpList<Expression> item = product.item;
                    return MathNet.Symbolics.Operators.sum(ListModule.Map<Expression, Expression>(mapping, item));
                }
                case 6:
                {
                    Expression.Power power = (Expression.Power) _arg2;
                    Expression expression = power.item1;
                    return Algebraic.Expand(MathNet.Symbolics.Operators.multiply(power.item2, lnRules@19(expression)));
                }
            }
            return MathNet.Symbolics.Operators.ln(_arg2);
        }

        internal static Expression rules@30(Expression x)
        {
            Expression expression2;
            Expression expression4;
            Expression expression5;
            FSharpList<Expression> item;
            Expression expression = Algebraic.ExpandMain(x);
            switch (expression.Tag)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    return expression;

                case 4:
                    item = ((Expression.Sum) expression).item;
                    return ListModule.Fold<Expression, Expression>(new rules@40-1(), MathNet.Symbolics.Operators.zero, item);

                case 5:
                {
                    item = ((Expression.Product) expression).item;
                    FSharpFunc<Tuple<Expression, Expression>, FSharpFunc<Expression, Tuple<Expression, Expression>>> folder = new f@37();
                    Tuple<Expression, Expression> tuple = ListModule.Fold<Expression, Tuple<Expression, Expression>>(folder, new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.one, MathNet.Symbolics.Operators.zero), item);
                    expression2 = tuple.Item2;
                    return MathNet.Symbolics.Operators.multiply(tuple.Item1, MathNet.Symbolics.Operators.exp(expression2));
                }
                case 6:
                {
                    Expression.Power power = (Expression.Power) expression;
                    if (power.item1.Tag != 7)
                    {
                        return expression;
                    }
                    Expression.Function function = (Expression.Function) power.item1;
                    if (function.item1.Tag != 2)
                    {
                        return expression;
                    }
                    expression2 = power.item2;
                    expression4 = MathNet.Symbolics.Operators.multiply(function.item2, expression2);
                    switch (expression4.Tag)
                    {
                        case 5:
                            expression5 = expression4;
                            goto Label_00E2;

                        case 6:
                            expression5 = expression4;
                            goto Label_00E2;
                    }
                    break;
                }
                default:
                    return expression;
            }
            return MathNet.Symbolics.Operators.exp(expression4);
        Label_00E2:
            return MathNet.Symbolics.Operators.exp(rules@30(expression5));
        }

        [CompilationSourceName("simplify")]
        public static Expression Simplify(Expression x)
        {
            Expression expression = Rational.Rationalize(x);
            Expression expression2 = Contract(Rational.Numerator(expression));
            Expression expression3 = Contract(Rational.Denominator(expression));
            return MathNet.Symbolics.Operators.multiply(expression2, MathNet.Symbolics.Operators.invert(expression3));
        }

        [Serializable]
        internal class Contract@43 : FSharpFunc<Expression, Expression>
        {
            internal Contract@43()
            {
            }

            public override Expression Invoke(Expression x) => 
                Exponential.Contract(x);
        }

        [Serializable]
        internal class Expand@23-3 : FSharpFunc<Expression, Expression>
        {
            internal Expand@23-3()
            {
            }

            public override Expression Invoke(Expression x) => 
                Exponential.Expand(x);
        }

        [Serializable]
        internal class expRules@15-1 : FSharpFunc<Expression, Expression>
        {
            internal expRules@15-1()
            {
            }

            public override Expression Invoke(Expression _arg1) => 
                Exponential.expRules@15(_arg1);
        }

        [Serializable]
        internal class f@37 : FSharpFunc<Tuple<Expression, Expression>, FSharpFunc<Expression, Tuple<Expression, Expression>>>
        {
            internal f@37()
            {
            }

            public override FSharpFunc<Expression, Tuple<Expression, Expression>> Invoke(Tuple<Expression, Expression> tupledArg)
            {
                Expression p = tupledArg.Item1;
                return new Exponential.f@37-1(p, tupledArg.Item2);
            }
        }

        [Serializable]
        internal class f@37-1 : FSharpFunc<Expression, Tuple<Expression, Expression>>
        {
            public Expression p;
            public Expression s;

            internal f@37-1(Expression p, Expression s)
            {
                this.p = p;
                this.s = s;
            }

            public override Tuple<Expression, Expression> Invoke(Expression _arg1)
            {
                Expression expression;
                if (_arg1.Tag != 7)
                {
                    expression = _arg1;
                }
                else
                {
                    Expression.Function function = (Expression.Function) _arg1;
                    if (function.item1.Tag == 2)
                    {
                        expression = function.item2;
                        return new Tuple<Expression, Expression>(this.p, MathNet.Symbolics.Operators.add(this.s, expression));
                    }
                    expression = _arg1;
                }
                return new Tuple<Expression, Expression>(MathNet.Symbolics.Operators.multiply(this.p, expression), this.s);
            }
        }

        [Serializable]
        internal class lnRules@19-1 : FSharpFunc<Expression, Expression>
        {
            internal lnRules@19-1()
            {
            }

            public override Expression Invoke(Expression _arg2) => 
                Exponential.lnRules@19(_arg2);
        }

        [Serializable]
        internal class rules@40-1 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal rules@40-1()
            {
            }

            public override Expression Invoke(Expression s, Expression _arg2)
            {
                Expression expression;
                switch (_arg2.Tag)
                {
                    case 5:
                        expression = _arg2;
                        break;

                    case 6:
                        expression = _arg2;
                        break;

                    default:
                        return MathNet.Symbolics.Operators.add(s, _arg2);
                }
                return MathNet.Symbolics.Operators.add(s, Exponential.rules@30(expression));
            }
        }
    }
}

