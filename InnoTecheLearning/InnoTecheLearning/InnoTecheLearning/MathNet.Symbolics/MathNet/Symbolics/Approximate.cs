namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections.Generic;
    using System.Numerics;

    [RequireQualifiedAccess, CompilationMapping(SourceConstructFlags.Module)]
    public static class Approximate
    {
        [CompilationSourceName("approximate")]
        public static Expression Approximate(Expression x) => 
            ApproximateSubstitute((IDictionary<string, MathNet.Symbolics.Approximation>) MapModule.Empty<string, MathNet.Symbolics.Approximation>(), x);

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("approximateSubstitute")]
        public static Expression ApproximateSubstitute(IDictionary<string, MathNet.Symbolics.Approximation> symbols, Expression x)
        {
            FSharpList<Expression> list;
            Expression expression;
            Expression expression2;
            switch (x.Tag)
            {
                case 1:
                    return x;

                case 2:
                {
                    Expression.Identifier identifier = (Expression.Identifier) x;
                    string key = identifier.item.item;
                    MathNet.Symbolics.Approximation approximation = null;
                    bool flag = symbols.TryGetValue(key, out approximation);
                    MathNet.Symbolics.Approximation approximation2 = approximation;
                    if (!flag)
                    {
                        return x;
                    }
                    return Values.unpack(ValueModule.approx(approximation2));
                }
                case 3:
                {
                    Expression.Constant constant = (Expression.Constant) x;
                    switch (constant.item.Tag)
                    {
                        case 1:
                            return Values.unpack(ValueModule.real(3.1415926535897931));

                        case 2:
                            return Values.unpack(ValueModule.complex(System.Numerics.Complex.ImaginaryOne));
                    }
                    return Values.unpack(ValueModule.real(2.7182818284590451));
                }
                case 4:
                {
                    Expression.Sum sum = (Expression.Sum) x;
                    list = sum.item;
                    return MathNet.Symbolics.Operators.sum(ListModule.Map<Expression, Expression>(new ApproximateSubstitute@27(symbols), list));
                }
                case 5:
                {
                    Expression.Product product = (Expression.Product) x;
                    list = product.item;
                    return MathNet.Symbolics.Operators.product(ListModule.Map<Expression, Expression>(new ApproximateSubstitute@28-1(symbols), list));
                }
                case 6:
                {
                    Expression.Power power = (Expression.Power) x;
                    expression = power.item2;
                    expression2 = power.item1;
                    return MathNet.Symbolics.Operators.pow(ApproximateSubstitute(symbols, expression2), ApproximateSubstitute(symbols, expression));
                }
                case 7:
                {
                    Expression.Function function = (Expression.Function) x;
                    MathNet.Symbolics.Function f = function.item1;
                    expression = function.item2;
                    expression2 = ApproximateSubstitute(symbols, expression);
                    FSharpOption<Value> option = Values.|Value|_|(expression2);
                    if (option == null)
                    {
                        return MathNet.Symbolics.Operators.apply(f, expression2);
                    }
                    Value value2 = option.Value;
                    return Values.unpack(ValueModule.apply(f, value2));
                }
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    return x;
            }
            Expression.Number number = (Expression.Number) x;
            BigRational item = number.item;
            return Values.unpack(ValueModule.real(BigRational.ToDouble(item)));
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("complex")]
        public static MathNet.Symbolics.Approximation Complex(double r, double i) => 
            MathNet.Symbolics.Approximation.NewComplex(new System.Numerics.Complex(r, i));

        [CompilationSourceName("real")]
        public static MathNet.Symbolics.Approximation Real(double x) => 
            MathNet.Symbolics.Approximation.NewReal(x);

        [Serializable]
        internal class ApproximateSubstitute@27 : FSharpFunc<Expression, Expression>
        {
            public IDictionary<string, MathNet.Symbolics.Approximation> symbols;

            internal ApproximateSubstitute@27(IDictionary<string, MathNet.Symbolics.Approximation> symbols)
            {
                this.symbols = symbols;
            }

            public override Expression Invoke(Expression x) => 
                Approximate.ApproximateSubstitute(this.symbols, x);
        }

        [Serializable]
        internal class ApproximateSubstitute@28-1 : FSharpFunc<Expression, Expression>
        {
            public IDictionary<string, MathNet.Symbolics.Approximation> symbols;

            internal ApproximateSubstitute@28-1(IDictionary<string, MathNet.Symbolics.Approximation> symbols)
            {
                this.symbols = symbols;
            }

            public override Expression Invoke(Expression x) => 
                Approximate.ApproximateSubstitute(this.symbols, x);
        }
    }
}

