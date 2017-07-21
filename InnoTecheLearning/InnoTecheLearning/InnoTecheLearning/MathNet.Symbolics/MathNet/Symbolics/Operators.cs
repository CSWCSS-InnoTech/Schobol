namespace MathNet.Symbolics
{
    using <StartupCode$MathNet-Symbolics>;
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    [CompilationMapping(SourceConstructFlags.Module)]
    public static class Operators
    {
        internal static FSharpOption<Tuple<Value, Expression>> |Term|_|@211(Expression _arg1)
        {
            Expression.Product product;
            FSharpList<Expression> item;
            FSharpOption<Value> option;
            FSharpList<Expression> list2;
            Value value2;
            switch (_arg1.Tag)
            {
                case 0:
                    return null;

                case 1:
                    return null;

                case 5:
                {
                    product = (Expression.Product) _arg1;
                    if (product.item.TailOrNull == null)
                    {
                        break;
                    }
                    item = product.item;
                    option = Values.|Value|_|(item.HeadOrDefault);
                    if ((option == null) || (item.TailOrNull.TailOrNull == null))
                    {
                        break;
                    }
                    list2 = item.TailOrNull;
                    if (list2.TailOrNull.TailOrNull != null)
                    {
                        break;
                    }
                    value2 = option.Value;
                    Expression expression = list2.HeadOrDefault;
                    return FSharpOption<Tuple<Value, Expression>>.Some(new Tuple<Value, Expression>(value2, expression));
                }
            }
            if (_arg1.Tag == 5)
            {
                product = (Expression.Product) _arg1;
                if (product.item.TailOrNull != null)
                {
                    item = product.item;
                    option = Values.|Value|_|(item.HeadOrDefault);
                    if (option != null)
                    {
                        list2 = item.TailOrNull;
                        value2 = option.Value;
                        return FSharpOption<Tuple<Value, Expression>>.Some(new Tuple<Value, Expression>(value2, Expression.NewProduct(list2)));
                    }
                }
            }
            return FSharpOption<Tuple<Value, Expression>>.Some(new Tuple<Value, Expression>(ValueModule.one, _arg1));
        }

        internal static FSharpOption<Tuple<Expression, Expression>> |Term|_|@266-1(Expression _arg2)
        {
            switch (_arg2.Tag)
            {
                case 0:
                    return null;

                case 1:
                    return null;

                case 6:
                {
                    Expression.Power power = (Expression.Power) _arg2;
                    Expression expression = power.item1;
                    Expression expression2 = power.item2;
                    return FSharpOption<Tuple<Expression, Expression>>.Some(new Tuple<Expression, Expression>(expression, expression2));
                }
            }
            return FSharpOption<Tuple<Expression, Expression>>.Some(new Tuple<Expression, Expression>(_arg2, one));
        }

        public static Expression abs(Expression _arg1)
        {
            Value value2;
            FSharpOption<Value> option = Values.|Value|_|(_arg1);
            if (option != null)
            {
                value2 = option.Value;
                return Values.unpack(ValueModule.abs(value2));
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                if (product.item.TailOrNull != null)
                {
                    FSharpList<Expression> item = product.item;
                    FSharpOption<Value> option2 = Values.|Value|_|(item.HeadOrDefault);
                    if ((option2 != null) && ValueModule.isNegative(option2.Value))
                    {
                        value2 = option2.Value;
                        FSharpList<Expression> list2 = item.TailOrNull;
                        return Expression.NewFunction(MathNet.Symbolics.Function.Abs, multiply(Values.unpack(ValueModule.abs(value2)), Expression.NewProduct(list2)));
                    }
                }
            }
            return Expression.NewFunction(MathNet.Symbolics.Function.Abs, _arg1);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static Expression add(Expression x, Expression y)
        {
            Expression expression;
            Value value2;
            FSharpList<Expression> list4;
            Expression.Sum sum4;
            FSharpList<Expression> list5;
            FSharpOption<Value> option10;
            Expression.Sum sum6;
            FSharpList<Expression> list6;
            if ((x.Tag != 12) && (y.Tag != 12))
            {
                if (ExpressionPatterns.|Zero|_|(x) != null)
                {
                    return y;
                }
                if (ExpressionPatterns.|Zero|_|(y) != null)
                {
                    return x;
                }
                FSharpOption<Value> option3 = Values.|Value|_|(x);
                if (option3 != null)
                {
                    FSharpOption<Value> option4 = Values.|Value|_|(y);
                    if (option4 != null)
                    {
                        value2 = option4.Value;
                        return Values.unpack(ValueModule.sum(option3.Value, value2));
                    }
                }
                switch (x.Tag)
                {
                    case 9:
                        goto Label_0543;

                    case 10:
                        switch (y.Tag)
                        {
                            case 9:
                                goto Label_0543;
                        }
                        goto Label_0549;

                    case 11:
                        switch (y.Tag)
                        {
                            case 9:
                                goto Label_0543;

                            case 10:
                                goto Label_0549;
                        }
                        goto Label_054F;
                }
            }
            else
            {
                return undefined;
            }
            switch (y.Tag)
            {
                case 9:
                    goto Label_0543;

                case 10:
                    goto Label_0549;

                case 11:
                    goto Label_054F;

                default:
                {
                    FSharpOption<Value> option5 = Values.|Value|_|(x);
                    if (option5 == null)
                    {
                        FSharpOption<Value> option6 = Values.|Value|_|(y);
                        if (option6 == null)
                        {
                            FSharpList<Expression> list3;
                            if (x.Tag == 4)
                            {
                                Expression.Sum sum = (Expression.Sum) x;
                                if (sum.item.TailOrNull != null)
                                {
                                    FSharpList<Expression> item = sum.item;
                                    FSharpOption<Value> option7 = Values.|Value|_|(item.HeadOrDefault);
                                    if ((option7 != null) && (y.Tag == 4))
                                    {
                                        Expression.Sum sum2 = (Expression.Sum) y;
                                        if (sum2.item.TailOrNull != null)
                                        {
                                            FSharpList<Expression> list2 = sum2.item;
                                            FSharpOption<Value> option8 = Values.|Value|_|(list2.HeadOrDefault);
                                            if (option8 != null)
                                            {
                                                list3 = list2.TailOrNull;
                                                value2 = option8.Value;
                                                list4 = item.TailOrNull;
                                                return valueAdd@236(ValueModule.sum(option7.Value, value2), merge@218(list4, list3));
                                            }
                                        }
                                    }
                                }
                            }
                            if (x.Tag == 4)
                            {
                                FSharpOption<Value> option9;
                                Expression.Sum sum3 = (Expression.Sum) x;
                                if (sum3.item.TailOrNull == null)
                                {
                                    if (y.Tag != 4)
                                    {
                                        break;
                                    }
                                    sum4 = (Expression.Sum) y;
                                    if (sum4.item.TailOrNull == null)
                                    {
                                        break;
                                    }
                                    list3 = sum4.item;
                                    option9 = Values.|Value|_|(list3.HeadOrDefault);
                                    if (option9 == null)
                                    {
                                        break;
                                    }
                                    value2 = option9.Value;
                                    list5 = list3.TailOrNull;
                                    list4 = sum3.item;
                                }
                                else
                                {
                                    list3 = sum3.item;
                                    option9 = Values.|Value|_|(list3.HeadOrDefault);
                                    if (option9 == null)
                                    {
                                        if (y.Tag != 4)
                                        {
                                            break;
                                        }
                                        sum4 = (Expression.Sum) y;
                                        if (sum4.item.TailOrNull == null)
                                        {
                                            break;
                                        }
                                        list4 = sum4.item;
                                        option10 = Values.|Value|_|(list4.HeadOrDefault);
                                        if (option10 == null)
                                        {
                                            break;
                                        }
                                        value2 = option10.Value;
                                        list5 = list4.TailOrNull;
                                        list4 = sum3.item;
                                    }
                                    else
                                    {
                                        if (y.Tag != 4)
                                        {
                                            break;
                                        }
                                        sum4 = (Expression.Sum) y;
                                        list4 = sum4.item;
                                        list5 = list3.TailOrNull;
                                        value2 = option9.Value;
                                    }
                                }
                                return valueAdd@236(value2, merge@218(list5, list4));
                            }
                            break;
                        }
                        value2 = option6.Value;
                        expression = x;
                    }
                    else
                    {
                        expression = y;
                        value2 = option5.Value;
                    }
                    return valueAdd@236(value2, expression);
                }
            }
            if (x.Tag != 4)
            {
                if (y.Tag != 4)
                {
                    goto Label_0318;
                }
                sum4 = (Expression.Sum) y;
                if (sum4.item.TailOrNull == null)
                {
                    goto Label_0318;
                }
                list4 = sum4.item;
                option10 = Values.|Value|_|(list4.HeadOrDefault);
                if (option10 == null)
                {
                    goto Label_0318;
                }
                value2 = option10.Value;
                list5 = list4.TailOrNull;
                expression = x;
            }
            else
            {
                Expression.Sum sum5;
                sum4 = (Expression.Sum) x;
                if (sum4.item.TailOrNull == null)
                {
                    if (y.Tag != 4)
                    {
                        goto Label_0318;
                    }
                    sum5 = (Expression.Sum) y;
                    if (sum5.item.TailOrNull == null)
                    {
                        goto Label_0318;
                    }
                    list4 = sum5.item;
                    option10 = Values.|Value|_|(list4.HeadOrDefault);
                    if (option10 == null)
                    {
                        goto Label_0318;
                    }
                    value2 = option10.Value;
                    list5 = list4.TailOrNull;
                    expression = x;
                }
                else
                {
                    list4 = sum4.item;
                    option10 = Values.|Value|_|(list4.HeadOrDefault);
                    if (option10 == null)
                    {
                        if (y.Tag == 4)
                        {
                            sum5 = (Expression.Sum) y;
                            if (sum5.item.TailOrNull != null)
                            {
                                list5 = sum5.item;
                                FSharpOption<Value> option11 = Values.|Value|_|(list5.HeadOrDefault);
                                if (option11 != null)
                                {
                                    value2 = option11.Value;
                                    list5 = list5.TailOrNull;
                                    expression = x;
                                    goto Label_02AE;
                                }
                            }
                        }
                        goto Label_0318;
                    }
                    expression = y;
                    list5 = list4.TailOrNull;
                    value2 = option10.Value;
                }
            }
        Label_02AE:
            return valueAdd@236(value2, merge@218(list5, FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty)));
        Label_0318:
            if (x.Tag == 4)
            {
                sum6 = (Expression.Sum) x;
                if (y.Tag == 4)
                {
                    Expression.Sum sum7 = (Expression.Sum) y;
                    list6 = sum7.item;
                    return merge@218(sum6.item, list6);
                }
                expression = y;
                return merge@218(sum6.item, FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty));
            }
            if (y.Tag == 4)
            {
                sum6 = (Expression.Sum) y;
                list6 = sum6.item;
                expression = x;
                return merge@218(FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty), list6);
            }
            expression = y;
            Expression head = x;
            return merge@218(FSharpList<Expression>.Cons(head, FSharpList<Expression>.Empty), FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty));
        Label_0543:
            return complexInfinity;
        Label_0549:
            return infinity;
        Label_054F:
            return negativeInfinity;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static Expression apply(MathNet.Symbolics.Function f, Expression x)
        {
            switch (f.Tag)
            {
                case 1:
                    return ln(x);

                case 2:
                    return exp(x);

                case 3:
                    return sin(x);

                case 4:
                    return cos(x);

                case 5:
                    return tan(x);

                case 6:
                    return Expression.NewFunction(MathNet.Symbolics.Function.Cot, x);

                case 7:
                    return Expression.NewFunction(MathNet.Symbolics.Function.Sec, x);

                case 8:
                    return Expression.NewFunction(MathNet.Symbolics.Function.Csc, x);

                case 9:
                    return Expression.NewFunction(MathNet.Symbolics.Function.Cosh, x);

                case 10:
                    return Expression.NewFunction(MathNet.Symbolics.Function.Sinh, x);

                case 11:
                    return Expression.NewFunction(MathNet.Symbolics.Function.Tanh, x);

                case 12:
                    return Expression.NewFunction(MathNet.Symbolics.Function.Asin, x);

                case 13:
                    return Expression.NewFunction(MathNet.Symbolics.Function.Acos, x);

                case 14:
                    return Expression.NewFunction(MathNet.Symbolics.Function.Atan, x);
            }
            return abs(x);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static a applyN<a>(MathNet.Symbolics.Function f, FSharpList<Expression> xs)
        {
            throw new Exception("not supported yet");
        }

        public static Expression arccos(Expression x) => 
            Expression.NewFunction(MathNet.Symbolics.Function.Acos, x);

        public static Expression arcsin(Expression x) => 
            Expression.NewFunction(MathNet.Symbolics.Function.Asin, x);

        public static Expression arctan(Expression x) => 
            Expression.NewFunction(MathNet.Symbolics.Function.Atan, x);

        internal static bool compare@164(Expression a, Expression b)
        {
            Expression.Product product;
            FSharpList<Expression> list;
            Expression expression;
            Expression.Power power;
            Expression expression2;
            Expression expression3;
            Expression.Sum sum;
            FSharpList<Expression> list2;
            Expression.Function function;
            MathNet.Symbolics.Function function3;
            MathNet.Symbolics.Function function4;
            Expression.FunctionN nn;
        Label_0000:
            switch (a.Tag)
            {
                case 1:
                {
                    Expression.Approximation approximation = (Expression.Approximation) a;
                    switch (b.Tag)
                    {
                        case 1:
                        {
                            Expression.Approximation approximation2 = (Expression.Approximation) b;
                            MathNet.Symbolics.Approximation item = approximation2.item;
                            return ApproximationModule.orderRelation(approximation.item, item);
                        }
                    }
                    return true;
                }
                case 2:
                {
                    Expression.Identifier identifier = (Expression.Identifier) a;
                    switch (b.Tag)
                    {
                        case 1:
                            goto Label_0185;

                        case 2:
                        {
                            Expression.Identifier identifier2 = (Expression.Identifier) b;
                            Symbol y = identifier2.item;
                            return LanguagePrimitives.HashCompare.GenericLessThanIntrinsic<Symbol>(identifier.item, y);
                        }
                        case 3:
                            goto Label_0187;

                        case 4:
                            sum = (Expression.Sum) b;
                            list = sum.item;
                            expression = a;
                            goto Label_0216;

                        case 5:
                            product = (Expression.Product) b;
                            list = product.item;
                            expression = a;
                            goto Label_019D;

                        case 6:
                            power = (Expression.Power) b;
                            expression = power.item1;
                            expression2 = power.item2;
                            expression3 = a;
                            goto Label_01D5;
                    }
                    return true;
                }
                case 3:
                {
                    Expression.Constant constant = (Expression.Constant) a;
                    switch (b.Tag)
                    {
                        case 1:
                            goto Label_0185;

                        case 3:
                        {
                            Expression.Constant constant2 = (Expression.Constant) b;
                            MathNet.Symbolics.Constant constant3 = constant2.item;
                            return LanguagePrimitives.HashCompare.GenericLessThanIntrinsic<MathNet.Symbolics.Constant>(constant.item, constant3);
                        }
                    }
                    return true;
                }
                case 4:
                    sum = (Expression.Sum) a;
                    switch (b.Tag)
                    {
                        case 1:
                            goto Label_0185;

                        case 2:
                            list = sum.item;
                            expression = b;
                            goto Label_02F3;

                        case 3:
                            goto Label_0187;

                        case 4:
                        {
                            Expression.Sum sum2 = (Expression.Sum) b;
                            list = sum2.item;
                            list2 = sum.item;
                            goto Label_0328;
                        }
                        case 5:
                            product = (Expression.Product) b;
                            expression = a;
                            list = product.item;
                            goto Label_019D;

                        case 6:
                            power = (Expression.Power) b;
                            expression3 = a;
                            expression2 = power.item2;
                            expression = power.item1;
                            goto Label_01D5;

                        case 9:
                            list = sum.item;
                            expression = b;
                            goto Label_02F3;

                        case 10:
                            list = sum.item;
                            expression = b;
                            goto Label_02F3;

                        case 11:
                            list = sum.item;
                            expression = b;
                            goto Label_02F3;

                        case 12:
                            list = sum.item;
                            expression = b;
                            goto Label_02F3;
                    }
                    expression = b;
                    list = sum.item;
                    goto Label_02F3;

                case 5:
                    product = (Expression.Product) a;
                    switch (b.Tag)
                    {
                        case 1:
                            goto Label_0185;

                        case 2:
                            list = product.item;
                            expression = b;
                            goto Label_0421;

                        case 3:
                            goto Label_0187;

                        case 4:
                            list = product.item;
                            expression = b;
                            goto Label_0421;

                        case 5:
                        {
                            Expression.Product product2 = (Expression.Product) b;
                            list2 = product.item;
                            list = product2.item;
                            goto Label_0328;
                        }
                        case 6:
                            list = product.item;
                            expression = b;
                            goto Label_0421;

                        case 9:
                            list = product.item;
                            expression = b;
                            goto Label_0421;

                        case 10:
                            list = product.item;
                            expression = b;
                            goto Label_0421;

                        case 11:
                            list = product.item;
                            expression = b;
                            goto Label_0421;

                        case 12:
                            list = product.item;
                            expression = b;
                            goto Label_0421;
                    }
                    expression = b;
                    list = product.item;
                    goto Label_0421;

                case 6:
                    power = (Expression.Power) a;
                    switch (b.Tag)
                    {
                        case 1:
                            goto Label_0185;

                        case 2:
                            expression3 = power.item2;
                            expression2 = power.item1;
                            expression = b;
                            goto Label_0522;

                        case 3:
                            goto Label_0187;

                        case 4:
                            expression3 = power.item2;
                            expression2 = power.item1;
                            expression = b;
                            goto Label_0522;

                        case 5:
                            product = (Expression.Product) b;
                            expression = a;
                            list = product.item;
                            goto Label_019D;

                        case 6:
                        {
                            Expression.Power power2 = (Expression.Power) b;
                            expression = power2.item1;
                            expression2 = power2.item2;
                            expression3 = power.item1;
                            Expression expression4 = power.item2;
                            if (expression3.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
                            {
                                b = expression2;
                                a = expression4;
                            }
                            else
                            {
                                b = expression;
                                a = expression3;
                            }
                            goto Label_0000;
                        }
                        case 9:
                            expression3 = power.item2;
                            expression2 = power.item1;
                            expression = b;
                            goto Label_0522;

                        case 10:
                            expression3 = power.item2;
                            expression2 = power.item1;
                            expression = b;
                            goto Label_0522;

                        case 11:
                            expression3 = power.item2;
                            expression2 = power.item1;
                            expression = b;
                            goto Label_0522;

                        case 12:
                            expression3 = power.item2;
                            expression2 = power.item1;
                            expression = b;
                            goto Label_0522;
                    }
                    expression = b;
                    expression2 = power.item1;
                    expression3 = power.item2;
                    goto Label_0522;

                case 7:
                {
                    function = (Expression.Function) a;
                    switch (b.Tag)
                    {
                        case 1:
                            goto Label_0185;

                        case 2:
                            goto Label_07AD;

                        case 3:
                            goto Label_0187;

                        case 4:
                            sum = (Expression.Sum) b;
                            expression = a;
                            list = sum.item;
                            goto Label_0216;

                        case 5:
                            product = (Expression.Product) b;
                            expression = a;
                            list = product.item;
                            goto Label_019D;

                        case 6:
                            power = (Expression.Power) b;
                            expression3 = a;
                            expression2 = power.item2;
                            expression = power.item1;
                            goto Label_01D5;

                        case 8:
                            nn = (Expression.FunctionN) b;
                            list = nn.item2;
                            function3 = nn.item1;
                            function4 = function.item1;
                            expression = function.item2;
                            if (function4.Equals(function3, LanguagePrimitives.GenericEqualityComparer))
                            {
                                return compareZip@198(FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty), ListModule.Reverse<Expression>(list));
                            }
                            return LanguagePrimitives.HashCompare.GenericLessThanIntrinsic<MathNet.Symbolics.Function>(function4, function3);

                        case 9:
                            goto Label_07AF;

                        case 10:
                            goto Label_07B1;

                        case 11:
                            goto Label_07B3;

                        case 12:
                            goto Label_07B5;
                    }
                    Expression.Function function2 = (Expression.Function) b;
                    function3 = function2.item1;
                    expression = function2.item2;
                    function4 = function.item1;
                    expression2 = function.item2;
                    if (!function4.Equals(function3, LanguagePrimitives.GenericEqualityComparer))
                    {
                        return LanguagePrimitives.HashCompare.GenericLessThanIntrinsic<MathNet.Symbolics.Function>(function4, function3);
                    }
                    b = expression;
                    a = expression2;
                    goto Label_0000;
                }
                case 8:
                {
                    nn = (Expression.FunctionN) a;
                    switch (b.Tag)
                    {
                        case 1:
                            goto Label_0185;

                        case 2:
                            goto Label_07AD;

                        case 3:
                            goto Label_0187;

                        case 4:
                            sum = (Expression.Sum) b;
                            expression = a;
                            list = sum.item;
                            goto Label_0216;

                        case 5:
                            product = (Expression.Product) b;
                            expression = a;
                            list = product.item;
                            goto Label_019D;

                        case 6:
                            power = (Expression.Power) b;
                            expression3 = a;
                            expression2 = power.item2;
                            expression = power.item1;
                            goto Label_01D5;

                        case 7:
                            function = (Expression.Function) b;
                            function3 = function.item1;
                            expression = function.item2;
                            list = nn.item2;
                            function4 = nn.item1;
                            if (function4.Equals(function3, LanguagePrimitives.GenericEqualityComparer))
                            {
                                return compareZip@198(ListModule.Reverse<Expression>(list), FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty));
                            }
                            return LanguagePrimitives.HashCompare.GenericLessThanIntrinsic<MathNet.Symbolics.Function>(function4, function3);

                        case 9:
                            goto Label_07AF;

                        case 10:
                            goto Label_07B1;

                        case 11:
                            goto Label_07B3;

                        case 12:
                            goto Label_07B5;
                    }
                    Expression.FunctionN nn2 = (Expression.FunctionN) b;
                    list = nn2.item2;
                    function3 = nn2.item1;
                    list2 = nn.item2;
                    function4 = nn.item1;
                    if (!function4.Equals(function3, LanguagePrimitives.GenericEqualityComparer))
                    {
                        return LanguagePrimitives.HashCompare.GenericLessThanIntrinsic<MathNet.Symbolics.Function>(function4, function3);
                    }
                    return compareZip@198(ListModule.Reverse<Expression>(list2), ListModule.Reverse<Expression>(list));
                }
                case 9:
                    switch (b.Tag)
                    {
                        case 1:
                            goto Label_0185;

                        case 2:
                            goto Label_07AD;

                        case 3:
                            goto Label_0187;

                        case 4:
                            sum = (Expression.Sum) b;
                            expression = a;
                            list = sum.item;
                            goto Label_0216;

                        case 5:
                            product = (Expression.Product) b;
                            expression = a;
                            list = product.item;
                            goto Label_019D;

                        case 6:
                            power = (Expression.Power) b;
                            expression3 = a;
                            expression2 = power.item2;
                            expression = power.item1;
                            goto Label_01D5;
                    }
                    return true;

                case 10:
                    switch (b.Tag)
                    {
                        case 1:
                            goto Label_0185;

                        case 2:
                            goto Label_07AD;

                        case 3:
                            goto Label_0187;

                        case 4:
                            sum = (Expression.Sum) b;
                            expression = a;
                            list = sum.item;
                            goto Label_0216;

                        case 5:
                            product = (Expression.Product) b;
                            expression = a;
                            list = product.item;
                            goto Label_019D;

                        case 6:
                            power = (Expression.Power) b;
                            expression3 = a;
                            expression2 = power.item2;
                            expression = power.item1;
                            goto Label_01D5;

                        case 9:
                            goto Label_07AF;
                    }
                    return true;

                case 11:
                    switch (b.Tag)
                    {
                        case 1:
                            goto Label_0185;

                        case 2:
                            goto Label_07AD;

                        case 3:
                            goto Label_0187;

                        case 4:
                            sum = (Expression.Sum) b;
                            expression = a;
                            list = sum.item;
                            goto Label_0216;

                        case 5:
                            product = (Expression.Product) b;
                            expression = a;
                            list = product.item;
                            goto Label_019D;

                        case 6:
                            power = (Expression.Power) b;
                            expression3 = a;
                            expression2 = power.item2;
                            expression = power.item1;
                            goto Label_01D5;

                        case 9:
                            goto Label_07AF;

                        case 10:
                            goto Label_07B1;
                    }
                    return true;

                case 12:
                    switch (b.Tag)
                    {
                        case 1:
                            goto Label_0185;

                        case 2:
                            goto Label_07AD;

                        case 3:
                            goto Label_0187;

                        case 4:
                            sum = (Expression.Sum) b;
                            expression = a;
                            list = sum.item;
                            goto Label_0216;

                        case 5:
                            product = (Expression.Product) b;
                            expression = a;
                            list = product.item;
                            goto Label_019D;

                        case 6:
                            power = (Expression.Power) b;
                            expression3 = a;
                            expression2 = power.item2;
                            expression = power.item1;
                            goto Label_01D5;

                        case 9:
                            goto Label_07AF;

                        case 10:
                            goto Label_07B1;

                        case 11:
                            goto Label_07B3;
                    }
                    return false;

                default:
                {
                    Expression.Number number = (Expression.Number) a;
                    switch (b.Tag)
                    {
                        case 0:
                        {
                            Expression.Number number2 = (Expression.Number) b;
                            BigRational rational = number2.item;
                            return LanguagePrimitives.HashCompare.GenericLessThanIntrinsic<BigRational>(number.item, rational);
                        }
                        default:
                            return true;
                    }
                    break;
                }
            }
            return false;
        Label_0185:
            return false;
        Label_0187:
            return false;
        Label_019D:
            return compareZip@198(FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty), ListModule.Reverse<Expression>(list));
        Label_01D5:
            if (!expression3.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
            {
                b = expression;
                a = expression3;
            }
            else
            {
                b = expression2;
                a = one;
            }
            goto Label_0000;
        Label_0216:
            return compareZip@198(FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty), ListModule.Reverse<Expression>(list));
        Label_02F3:
            return compareZip@198(ListModule.Reverse<Expression>(list), FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty));
        Label_0328:
            return compareZip@198(ListModule.Reverse<Expression>(list2), ListModule.Reverse<Expression>(list));
        Label_0421:
            return compareZip@198(ListModule.Reverse<Expression>(list), FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty));
        Label_0522:
            if (!expression2.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
            {
                b = expression;
                a = expression2;
            }
            else
            {
                b = one;
                a = expression3;
            }
            goto Label_0000;
        Label_07AD:
            return false;
        Label_07AF:
            return false;
        Label_07B1:
            return false;
        Label_07B3:
            return false;
        Label_07B5:
            return true;
        }

        internal static bool compareZip@198(FSharpList<Expression> a, FSharpList<Expression> b)
        {
            Expression expression;
            Expression expression2;
            FSharpList<Expression> list3;
            FSharpList<Expression> list4;
        Label_0000:
            if (a.TailOrNull != null)
            {
                FSharpList<Expression> list = a;
                if (b.TailOrNull != null)
                {
                    FSharpList<Expression> list2 = b;
                    expression = list2.HeadOrDefault;
                    if (!list.HeadOrDefault.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
                    {
                        list3 = list2.TailOrNull;
                        expression = list2.HeadOrDefault;
                        list4 = list.TailOrNull;
                        expression2 = list.HeadOrDefault;
                        return compare@164(expression2, expression);
                    }
                }
            }
            if (a.TailOrNull != null)
            {
                list3 = a;
                if (b.TailOrNull == null)
                {
                    goto Label_006C;
                }
                list4 = b;
                FSharpList<Expression> list5 = list4.TailOrNull;
                expression = list4.HeadOrDefault;
                FSharpList<Expression> list6 = list3.TailOrNull;
                expression2 = list3.HeadOrDefault;
                b = list5;
                a = list6;
                goto Label_0000;
            }
            if (b.TailOrNull != null)
            {
                list3 = b;
                list4 = list3.TailOrNull;
                expression = list3.HeadOrDefault;
                return true;
            }
        Label_006C:
            return false;
        }

        public static Expression cos(Expression _arg1)
        {
            BigRational item;
            if (ExpressionPatterns.|Zero|_|(_arg1) != null)
            {
                return one;
            }
            if (_arg1.Tag == 0)
            {
                Expression.Number number = (Expression.Number) _arg1;
                if (number.item.IsNegative)
                {
                    item = number.item;
                    return Expression.NewFunction(MathNet.Symbolics.Function.Cos, Expression.NewNumber(-item));
                }
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                if (product.item.TailOrNull != null)
                {
                    FSharpList<Expression> list = product.item;
                    if (list.HeadOrDefault.Tag == 0)
                    {
                        Expression.Number number2 = (Expression.Number) list.HeadOrDefault;
                        if (number2.item.IsNegative)
                        {
                            item = number2.item;
                            FSharpList<Expression> list2 = list.TailOrNull;
                            return Expression.NewFunction(MathNet.Symbolics.Function.Cos, multiply(Expression.NewNumber(-item), Expression.NewProduct(list2)));
                        }
                    }
                }
            }
            return Expression.NewFunction(MathNet.Symbolics.Function.Cos, _arg1);
        }

        public static Expression cosh(Expression x) => 
            Expression.NewFunction(MathNet.Symbolics.Function.Cosh, x);

        public static Expression cot(Expression x) => 
            Expression.NewFunction(MathNet.Symbolics.Function.Cot, x);

        public static Expression csc(Expression x) => 
            Expression.NewFunction(MathNet.Symbolics.Function.Csc, x);

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static Expression divide(Expression x, Expression y) => 
            multiply(x, invert(y));

        public static Expression exp(Expression _arg1)
        {
            if (ExpressionPatterns.|Zero|_|(_arg1) != null)
            {
                return one;
            }
            return Expression.NewFunction(MathNet.Symbolics.Function.Exp, _arg1);
        }

        public static Expression fromInt32(int x) => 
            Expression.NewNumber(BigRational.FromInt(x));

        public static Expression fromInt64(long x) => 
            Expression.NewNumber(BigRational.FromBigInt(new BigInteger(x)));

        public static Expression fromInteger(BigInteger x) => 
            Expression.NewNumber(BigRational.FromBigInt(x));

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static Expression fromIntegerFraction(BigInteger n, BigInteger d) => 
            Expression.NewNumber(BigRational.FromBigIntFraction(n, d));

        public static Expression fromRational(BigRational x) => 
            Expression.NewNumber(x);

        internal static FSharpList<Expression> gen@219(FSharpList<Expression> acc, FSharpList<Expression> u, FSharpList<Expression> v)
        {
            FSharpList<Expression> list2;
            FSharpList<Expression> list3;
            FSharpOption<Tuple<Value, Expression>> option3;
            Expression expression;
            FSharpList<Expression> list4;
            FSharpList<Expression> list5;
            Value value2;
            FSharpList<Expression> list6;
            Expression expression2;
            Value value3;
            FSharpList<Expression> list7;
            FSharpList<Expression> list8;
        Label_0000:
            if (acc.TailOrNull != null)
            {
                FSharpList<Expression> list = acc;
                if (ExpressionPatterns.|Zero|_|(list.HeadOrDefault) != null)
                {
                    list2 = list.TailOrNull;
                    v = v;
                    u = u;
                    acc = list2;
                    goto Label_0000;
                }
            }
            if (acc.TailOrNull == null)
            {
                goto Label_01A0;
            }
            list2 = acc;
            FSharpOption<Tuple<Value, Expression>> option2 = |Term|_|@211(list2.HeadOrDefault);
            if (option2 == null)
            {
                goto Label_01A0;
            }
            if (u.TailOrNull == null)
            {
                if (v.TailOrNull == null)
                {
                    goto Label_01A0;
                }
                list3 = v;
                option3 = |Term|_|@211(list3.HeadOrDefault);
                if (option3 == null)
                {
                    goto Label_01A0;
                }
                expression = option3.Value.Item2;
                if (!option2.Value.Item2.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
                {
                    goto Label_01A0;
                }
                value3 = option2.Value.Item1;
                expression2 = option2.Value.Item2;
                list6 = list2.TailOrNull;
                value2 = option3.Value.Item1;
                list5 = list3.TailOrNull;
                expression = option3.Value.Item2;
                list4 = u;
            }
            else
            {
                FSharpOption<Tuple<Value, Expression>> option4;
                list3 = u;
                option3 = |Term|_|@211(list3.HeadOrDefault);
                if (option3 == null)
                {
                    if (v.TailOrNull == null)
                    {
                        goto Label_01A0;
                    }
                    list4 = v;
                    option4 = |Term|_|@211(list4.HeadOrDefault);
                    if (option4 == null)
                    {
                        goto Label_01A0;
                    }
                    expression = option4.Value.Item2;
                    if (!option2.Value.Item2.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
                    {
                        goto Label_01A0;
                    }
                    value3 = option2.Value.Item1;
                    expression2 = option2.Value.Item2;
                    list6 = list2.TailOrNull;
                    value2 = option4.Value.Item1;
                    list5 = list4.TailOrNull;
                    expression = option4.Value.Item2;
                    list4 = u;
                }
                else
                {
                    expression = option3.Value.Item2;
                    if (!option2.Value.Item2.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
                    {
                        if (v.TailOrNull != null)
                        {
                            list4 = v;
                            option4 = |Term|_|@211(list4.HeadOrDefault);
                            if (option4 != null)
                            {
                                expression = option4.Value.Item2;
                                if (option2.Value.Item2.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
                                {
                                    value3 = option2.Value.Item1;
                                    expression2 = option2.Value.Item2;
                                    list6 = list2.TailOrNull;
                                    value2 = option4.Value.Item1;
                                    list5 = list4.TailOrNull;
                                    expression = option4.Value.Item2;
                                    list4 = u;
                                    goto Label_00DF;
                                }
                            }
                        }
                        goto Label_01A0;
                    }
                    list4 = v;
                    expression = option3.Value.Item2;
                    list5 = list3.TailOrNull;
                    value2 = option3.Value.Item1;
                    list6 = list2.TailOrNull;
                    expression2 = option2.Value.Item2;
                    value3 = option2.Value.Item1;
                }
            }
        Label_00DF:
            v = list4;
            u = list5;
            acc = FSharpList<Expression>.Cons(multiply(Values.unpack(ValueModule.sum(value3, value2)), expression2), list6);
            goto Label_0000;
        Label_01A0:
            if (u.TailOrNull != null)
            {
                list5 = u;
                FSharpOption<Tuple<Value, Expression>> option5 = |Term|_|@211(list5.HeadOrDefault);
                if ((option5 != null) && (v.TailOrNull != null))
                {
                    list6 = v;
                    FSharpOption<Tuple<Value, Expression>> option6 = |Term|_|@211(list6.HeadOrDefault);
                    if (option6 != null)
                    {
                        expression = option6.Value.Item2;
                        if (option5.Value.Item2.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
                        {
                            expression = option6.Value.Item2;
                            list7 = list6.TailOrNull;
                            value2 = option6.Value.Item1;
                            expression2 = option5.Value.Item2;
                            list8 = list5.TailOrNull;
                            v = list7;
                            u = list8;
                            acc = FSharpList<Expression>.Cons(multiply(Values.unpack(ValueModule.sum(option5.Value.Item1, value2)), expression2), acc);
                            goto Label_0000;
                        }
                    }
                }
            }
            if (u.TailOrNull != null)
            {
                list7 = u;
                if (v.TailOrNull == null)
                {
                    expression = list7.HeadOrDefault;
                    list8 = list7.TailOrNull;
                    goto Label_02A9;
                }
                list8 = v;
                FSharpList<Expression> list9 = list8.TailOrNull;
                expression = list8.HeadOrDefault;
                FSharpList<Expression> list10 = list7.TailOrNull;
                expression2 = list7.HeadOrDefault;
                if (orderRelation(expression2, expression))
                {
                    v = v;
                    u = list10;
                    acc = FSharpList<Expression>.Cons(expression2, acc);
                }
                else
                {
                    v = list9;
                    u = u;
                    acc = FSharpList<Expression>.Cons(expression, acc);
                }
                goto Label_0000;
            }
            if (v.TailOrNull == null)
            {
                return acc;
            }
            list7 = v;
            list8 = list7.TailOrNull;
            expression = list7.HeadOrDefault;
        Label_02A9:
            v = FSharpList<Expression>.Empty;
            u = list8;
            acc = FSharpList<Expression>.Cons(expression, acc);
            goto Label_0000;
        }

        internal static FSharpList<Expression> gen@273-1(FSharpList<Expression> acc, FSharpList<Expression> u, FSharpList<Expression> v)
        {
            FSharpList<Expression> list2;
            FSharpList<Expression> list3;
            FSharpOption<Tuple<Expression, Expression>> option3;
            Expression expression;
            FSharpList<Expression> list4;
            FSharpList<Expression> list5;
            Expression expression2;
            FSharpList<Expression> list6;
            Expression expression3;
            Expression expression4;
            FSharpList<Expression> list7;
            FSharpList<Expression> list8;
        Label_0000:
            if (acc.TailOrNull != null)
            {
                FSharpList<Expression> list = acc;
                if (ExpressionPatterns.|One|_|(list.HeadOrDefault) != null)
                {
                    list2 = list.TailOrNull;
                    v = v;
                    u = u;
                    acc = list2;
                    goto Label_0000;
                }
            }
            if (acc.TailOrNull == null)
            {
                goto Label_019B;
            }
            list2 = acc;
            FSharpOption<Tuple<Expression, Expression>> option2 = |Term|_|@266-1(list2.HeadOrDefault);
            if (option2 == null)
            {
                goto Label_019B;
            }
            if (u.TailOrNull == null)
            {
                if (v.TailOrNull == null)
                {
                    goto Label_019B;
                }
                list3 = v;
                option3 = |Term|_|@266-1(list3.HeadOrDefault);
                if (option3 == null)
                {
                    goto Label_019B;
                }
                expression = option3.Value.Item1;
                if (!option2.Value.Item1.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
                {
                    goto Label_019B;
                }
                expression4 = option2.Value.Item1;
                expression3 = option2.Value.Item2;
                list6 = list2.TailOrNull;
                expression2 = option3.Value.Item1;
                expression = option3.Value.Item2;
                list5 = list3.TailOrNull;
                list4 = u;
            }
            else
            {
                FSharpOption<Tuple<Expression, Expression>> option4;
                list3 = u;
                option3 = |Term|_|@266-1(list3.HeadOrDefault);
                if (option3 == null)
                {
                    if (v.TailOrNull == null)
                    {
                        goto Label_019B;
                    }
                    list4 = v;
                    option4 = |Term|_|@266-1(list4.HeadOrDefault);
                    if (option4 == null)
                    {
                        goto Label_019B;
                    }
                    expression = option4.Value.Item1;
                    if (!option2.Value.Item1.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
                    {
                        goto Label_019B;
                    }
                    expression4 = option2.Value.Item1;
                    expression3 = option2.Value.Item2;
                    list6 = list2.TailOrNull;
                    expression2 = option4.Value.Item1;
                    expression = option4.Value.Item2;
                    list5 = list4.TailOrNull;
                    list4 = u;
                }
                else
                {
                    expression = option3.Value.Item1;
                    if (!option2.Value.Item1.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
                    {
                        if (v.TailOrNull != null)
                        {
                            list4 = v;
                            option4 = |Term|_|@266-1(list4.HeadOrDefault);
                            if (option4 != null)
                            {
                                expression = option4.Value.Item1;
                                if (option2.Value.Item1.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
                                {
                                    expression4 = option2.Value.Item1;
                                    expression3 = option2.Value.Item2;
                                    list6 = list2.TailOrNull;
                                    expression2 = option4.Value.Item1;
                                    expression = option4.Value.Item2;
                                    list5 = list4.TailOrNull;
                                    list4 = u;
                                    goto Label_00DF;
                                }
                            }
                        }
                        goto Label_019B;
                    }
                    list4 = v;
                    list5 = list3.TailOrNull;
                    expression = option3.Value.Item2;
                    expression2 = option3.Value.Item1;
                    list6 = list2.TailOrNull;
                    expression3 = option2.Value.Item2;
                    expression4 = option2.Value.Item1;
                }
            }
        Label_00DF:
            v = list4;
            u = list5;
            acc = FSharpList<Expression>.Cons(pow(expression4, add(expression3, expression)), list6);
            goto Label_0000;
        Label_019B:
            if (u.TailOrNull != null)
            {
                list5 = u;
                FSharpOption<Tuple<Expression, Expression>> option5 = |Term|_|@266-1(list5.HeadOrDefault);
                if ((option5 != null) && (v.TailOrNull != null))
                {
                    list6 = v;
                    FSharpOption<Tuple<Expression, Expression>> option6 = |Term|_|@266-1(list6.HeadOrDefault);
                    if (option6 != null)
                    {
                        expression = option6.Value.Item1;
                        if (option5.Value.Item1.Equals(expression, LanguagePrimitives.GenericEqualityComparer))
                        {
                            list7 = list6.TailOrNull;
                            expression = option6.Value.Item2;
                            expression2 = option6.Value.Item1;
                            list8 = list5.TailOrNull;
                            expression3 = option5.Value.Item2;
                            v = list7;
                            u = list8;
                            acc = FSharpList<Expression>.Cons(pow(option5.Value.Item1, add(expression3, expression)), acc);
                            goto Label_0000;
                        }
                    }
                }
            }
            if (u.TailOrNull == null)
            {
                if (v.TailOrNull == null)
                {
                    return acc;
                }
                list7 = v;
                list8 = list7.TailOrNull;
                v = FSharpList<Expression>.Empty;
                u = list8;
                acc = FSharpList<Expression>.Cons(list7.HeadOrDefault, acc);
            }
            else
            {
                FSharpList<Expression> list9;
                list7 = u;
                if (v.TailOrNull != null)
                {
                    list8 = v;
                    list9 = list8.TailOrNull;
                    expression = list8.HeadOrDefault;
                    FSharpList<Expression> list10 = list7.TailOrNull;
                    expression2 = list7.HeadOrDefault;
                    if (orderRelation(expression2, expression))
                    {
                        v = v;
                        u = list10;
                        acc = FSharpList<Expression>.Cons(expression2, acc);
                    }
                    else
                    {
                        v = list9;
                        u = u;
                        acc = FSharpList<Expression>.Cons(expression, acc);
                    }
                }
                else
                {
                    list8 = v;
                    list9 = list7.TailOrNull;
                    v = list8;
                    u = list9;
                    acc = FSharpList<Expression>.Cons(list7.HeadOrDefault, acc);
                }
            }
            goto Label_0000;
        }

        public static Expression invert(Expression _arg1)
        {
            FSharpOption<Value> option = Values.|Value|_|(_arg1);
            if (option != null)
            {
                Value value2 = option.Value;
                return Values.unpack(ValueModule.invert(value2));
            }
            switch (_arg1.Tag)
            {
                case 5:
                {
                    Expression.Product product = (Expression.Product) _arg1;
                    FSharpList<Expression> item = product.item;
                    return Expression.NewProduct(ListModule.Map<Expression, Expression>(new invert@338(), item));
                }
                case 6:
                {
                    Expression.Power power = (Expression.Power) _arg1;
                    Expression x = power.item1;
                    Expression y = power.item2;
                    return pow(x, multiply(minusOne, y));
                }
            }
            return Expression.NewPower(_arg1, minusOne);
        }

        public static bool isComplexInfinity(Expression _arg1) => 
            (_arg1.Tag == 9);

        public static bool isInfinity(Expression _arg1)
        {
            switch (_arg1.Tag)
            {
                case 9:
                case 10:
                case 11:
                    return true;
            }
            return false;
        }

        public static bool isMinusOne(Expression _arg1) => 
            (ExpressionPatterns.|MinusOne|_|(_arg1) > null);

        public static bool isNegative(Expression _arg1) => 
            (ExpressionPatterns.|Negative|_|(_arg1) > null);

        public static bool isNegativeInfinity(Expression _arg1) => 
            (_arg1.Tag == 11);

        public static bool isOne(Expression _arg1) => 
            (ExpressionPatterns.|One|_|(_arg1) > null);

        public static bool isPositive(Expression _arg1) => 
            (ExpressionPatterns.|Positive|_|(_arg1) > null);

        public static bool isPositiveInfinity(Expression _arg1) => 
            (_arg1.Tag == 10);

        public static bool isZero(Expression _arg1) => 
            (ExpressionPatterns.|Zero|_|(_arg1) > null);

        public static Expression ln(Expression _arg1)
        {
            if (ExpressionPatterns.|One|_|(_arg1) != null)
            {
                return zero;
            }
            return Expression.NewFunction(MathNet.Symbolics.Function.Ln, _arg1);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static Expression log(Expression basis, Expression x)
        {
            Expression expression = ln(x);
            Expression expression2 = ln(basis);
            return multiply(expression, invert(expression2));
        }

        internal static Expression merge@218(FSharpList<Expression> xs, FSharpList<Expression> ys)
        {
            FSharpList<Expression> list = gen@219(FSharpList<Expression>.Empty, xs, ys);
            if (list.TailOrNull == null)
            {
                return zero;
            }
            FSharpList<Expression> list2 = list;
            if (list2.TailOrNull.TailOrNull == null)
            {
                return list2.HeadOrDefault;
            }
            FSharpList<Expression> list3 = list;
            return Expression.NewSum(ListModule.Reverse<Expression>(list3));
        }

        internal static Expression merge@272-1(FSharpList<Expression> xs, FSharpList<Expression> ys)
        {
            FSharpList<Expression> list = gen@273-1(FSharpList<Expression>.Empty, xs, ys);
            if (list.TailOrNull == null)
            {
                return one;
            }
            FSharpList<Expression> list2 = list;
            if (list2.TailOrNull.TailOrNull == null)
            {
                return list2.HeadOrDefault;
            }
            FSharpList<Expression> list3 = list;
            return Expression.NewProduct(ListModule.Reverse<Expression>(list3));
        }

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static Expression multiply(Expression x, Expression y)
        {
            Expression expression;
            Value value2;
            FSharpList<Expression> list4;
            Expression.Product product4;
            FSharpList<Expression> list5;
            FSharpOption<Value> option12;
            Expression.Product product6;
            FSharpList<Expression> list6;
            if ((x.Tag != 12) && (y.Tag != 12))
            {
                if (ExpressionPatterns.|One|_|(x) != null)
                {
                    return y;
                }
                if (ExpressionPatterns.|One|_|(y) != null)
                {
                    return x;
                }
                if ((ExpressionPatterns.|Zero|_|(x) != null) || (ExpressionPatterns.|Zero|_|(y) != null))
                {
                    return zero;
                }
                FSharpOption<Value> option5 = Values.|Value|_|(x);
                if (option5 != null)
                {
                    FSharpOption<Value> option6 = Values.|Value|_|(y);
                    if (option6 != null)
                    {
                        value2 = option6.Value;
                        return Values.unpack(ValueModule.product(option5.Value, value2));
                    }
                }
                switch (x.Tag)
                {
                    case 9:
                        goto Label_0564;

                    case 10:
                        switch (y.Tag)
                        {
                            case 9:
                                goto Label_0564;
                        }
                        goto Label_056A;

                    case 11:
                        switch (y.Tag)
                        {
                            case 9:
                                goto Label_0564;

                            case 10:
                                goto Label_056A;
                        }
                        goto Label_0570;
                }
            }
            else
            {
                return undefined;
            }
            switch (y.Tag)
            {
                case 9:
                    goto Label_0564;

                case 10:
                    goto Label_056A;

                case 11:
                    goto Label_0570;

                default:
                {
                    FSharpOption<Value> option7 = Values.|Value|_|(x);
                    if (option7 == null)
                    {
                        FSharpOption<Value> option8 = Values.|Value|_|(y);
                        if (option8 == null)
                        {
                            FSharpList<Expression> list3;
                            if (x.Tag == 5)
                            {
                                Expression.Product product = (Expression.Product) x;
                                if (product.item.TailOrNull != null)
                                {
                                    FSharpList<Expression> item = product.item;
                                    FSharpOption<Value> option9 = Values.|Value|_|(item.HeadOrDefault);
                                    if ((option9 != null) && (y.Tag == 5))
                                    {
                                        Expression.Product product2 = (Expression.Product) y;
                                        if (product2.item.TailOrNull != null)
                                        {
                                            FSharpList<Expression> list2 = product2.item;
                                            FSharpOption<Value> option10 = Values.|Value|_|(list2.HeadOrDefault);
                                            if (option10 != null)
                                            {
                                                list3 = list2.TailOrNull;
                                                value2 = option10.Value;
                                                list4 = item.TailOrNull;
                                                return valueMul@292(ValueModule.product(option9.Value, value2), merge@272-1(list4, list3));
                                            }
                                        }
                                    }
                                }
                            }
                            if (x.Tag == 5)
                            {
                                FSharpOption<Value> option11;
                                Expression.Product product3 = (Expression.Product) x;
                                if (product3.item.TailOrNull == null)
                                {
                                    if (y.Tag != 5)
                                    {
                                        break;
                                    }
                                    product4 = (Expression.Product) y;
                                    if (product4.item.TailOrNull == null)
                                    {
                                        break;
                                    }
                                    list3 = product4.item;
                                    option11 = Values.|Value|_|(list3.HeadOrDefault);
                                    if (option11 == null)
                                    {
                                        break;
                                    }
                                    value2 = option11.Value;
                                    list5 = list3.TailOrNull;
                                    list4 = product3.item;
                                }
                                else
                                {
                                    list3 = product3.item;
                                    option11 = Values.|Value|_|(list3.HeadOrDefault);
                                    if (option11 == null)
                                    {
                                        if (y.Tag != 5)
                                        {
                                            break;
                                        }
                                        product4 = (Expression.Product) y;
                                        if (product4.item.TailOrNull == null)
                                        {
                                            break;
                                        }
                                        list4 = product4.item;
                                        option12 = Values.|Value|_|(list4.HeadOrDefault);
                                        if (option12 == null)
                                        {
                                            break;
                                        }
                                        value2 = option12.Value;
                                        list5 = list4.TailOrNull;
                                        list4 = product3.item;
                                    }
                                    else
                                    {
                                        if (y.Tag != 5)
                                        {
                                            break;
                                        }
                                        product4 = (Expression.Product) y;
                                        list4 = product4.item;
                                        list5 = list3.TailOrNull;
                                        value2 = option11.Value;
                                    }
                                }
                                return valueMul@292(value2, merge@272-1(list5, list4));
                            }
                            break;
                        }
                        value2 = option8.Value;
                        expression = x;
                    }
                    else
                    {
                        expression = y;
                        value2 = option7.Value;
                    }
                    return valueMul@292(value2, expression);
                }
            }
            if (x.Tag != 5)
            {
                if (y.Tag != 5)
                {
                    goto Label_0339;
                }
                product4 = (Expression.Product) y;
                if (product4.item.TailOrNull == null)
                {
                    goto Label_0339;
                }
                list4 = product4.item;
                option12 = Values.|Value|_|(list4.HeadOrDefault);
                if (option12 == null)
                {
                    goto Label_0339;
                }
                value2 = option12.Value;
                list5 = list4.TailOrNull;
                expression = x;
            }
            else
            {
                Expression.Product product5;
                product4 = (Expression.Product) x;
                if (product4.item.TailOrNull == null)
                {
                    if (y.Tag != 5)
                    {
                        goto Label_0339;
                    }
                    product5 = (Expression.Product) y;
                    if (product5.item.TailOrNull == null)
                    {
                        goto Label_0339;
                    }
                    list4 = product5.item;
                    option12 = Values.|Value|_|(list4.HeadOrDefault);
                    if (option12 == null)
                    {
                        goto Label_0339;
                    }
                    value2 = option12.Value;
                    list5 = list4.TailOrNull;
                    expression = x;
                }
                else
                {
                    list4 = product4.item;
                    option12 = Values.|Value|_|(list4.HeadOrDefault);
                    if (option12 == null)
                    {
                        if (y.Tag == 5)
                        {
                            product5 = (Expression.Product) y;
                            if (product5.item.TailOrNull != null)
                            {
                                list5 = product5.item;
                                FSharpOption<Value> option13 = Values.|Value|_|(list5.HeadOrDefault);
                                if (option13 != null)
                                {
                                    value2 = option13.Value;
                                    list5 = list5.TailOrNull;
                                    expression = x;
                                    goto Label_02CF;
                                }
                            }
                        }
                        goto Label_0339;
                    }
                    expression = y;
                    list5 = list4.TailOrNull;
                    value2 = option12.Value;
                }
            }
        Label_02CF:
            return valueMul@292(value2, merge@272-1(list5, FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty)));
        Label_0339:
            if (x.Tag == 5)
            {
                product6 = (Expression.Product) x;
                if (y.Tag == 5)
                {
                    Expression.Product product7 = (Expression.Product) y;
                    list6 = product7.item;
                    return merge@272-1(product6.item, list6);
                }
                expression = y;
                return merge@272-1(product6.item, FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty));
            }
            if (y.Tag == 5)
            {
                product6 = (Expression.Product) y;
                list6 = product6.item;
                expression = x;
                return merge@272-1(FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty), list6);
            }
            expression = y;
            Expression head = x;
            return merge@272-1(FSharpList<Expression>.Cons(head, FSharpList<Expression>.Empty), FSharpList<Expression>.Cons(expression, FSharpList<Expression>.Empty));
        Label_0564:
            return complexInfinity;
        Label_056A:
            return infinity;
        Label_0570:
            return negativeInfinity;
        }

        public static Expression negate(Expression x) => 
            multiply(minusOne, x);

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        internal static bool orderRelation(Expression x, Expression y) => 
            compare@164(x, y);

        public static a plus<a>(a x) => 
            x;

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static Expression pow(Expression x, Expression y)
        {
            BigRational item;
        Label_0000:
            if ((ExpressionPatterns.|Zero|_|(x) != null) && (ExpressionPatterns.|Zero|_|(y) != null))
            {
                return undefined;
            }
            if (ExpressionPatterns.|Zero|_|(y) != null)
            {
                return one;
            }
            if (ExpressionPatterns.|One|_|(y) != null)
            {
                return x;
            }
            if (ExpressionPatterns.|One|_|(x) != null)
            {
                return one;
            }
            if (x.Tag == 0)
            {
                Expression.Number number = (Expression.Number) x;
                if (y.Tag == 0)
                {
                    Expression.Number number2 = (Expression.Number) y;
                    if (!number2.item.IsInteger)
                    {
                        item = number2.item;
                        BigRational rational2 = number.item;
                        return Expression.NewPower(x, y);
                    }
                }
            }
            FSharpOption<Value> option6 = Values.|Value|_|(x);
            if (option6 != null)
            {
                FSharpOption<Value> option7 = Values.|Value|_|(y);
                if (option7 != null)
                {
                    Value value2 = option7.Value;
                    return Values.unpack(ValueModule.power(option6.Value, value2));
                }
            }
            if (x.Tag == 5)
            {
                Expression.Product product = (Expression.Product) x;
                if (y.Tag == 0)
                {
                    Expression.Number number3 = (Expression.Number) y;
                    if (number3.item.IsInteger)
                    {
                        item = number3.item;
                        FSharpList<Expression> list = product.item;
                        return Expression.NewProduct(ListModule.Map<Expression, Expression>(new pow@328(y), list));
                    }
                }
            }
            if (x.Tag == 6)
            {
                Expression.Power power = (Expression.Power) x;
                if (y.Tag == 0)
                {
                    Expression.Number number4 = (Expression.Number) y;
                    if (number4.item.IsInteger)
                    {
                        Expression expression = power.item1;
                        Expression expression2 = power.item2;
                        item = number4.item;
                        y = multiply(expression2, y);
                        x = expression;
                        goto Label_0000;
                    }
                }
            }
            return Expression.NewPower(x, y);
        }

        public static Expression product(FSharpList<Expression> xs)
        {
            if (xs.TailOrNull == null)
            {
                return one;
            }
            return ListModule.Reduce<Expression>(new product@346(), xs);
        }

        public static Expression productSeq(IEnumerable<Expression> xs) => 
            SeqModule.Fold<Expression, Expression>(new productSeq@347(), one, xs);

        public static Expression real(double floatingPoint) => 
            Values.unpack(ValueModule.real(floatingPoint));

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static Expression root(Expression n, Expression x) => 
            Expression.NewPower(x, Expression.NewPower(n, minusOne));

        public static Expression sec(Expression x) => 
            Expression.NewFunction(MathNet.Symbolics.Function.Sec, x);

        public static Expression sin(Expression _arg1)
        {
            BigRational item;
            if (ExpressionPatterns.|Zero|_|(_arg1) != null)
            {
                return zero;
            }
            if (_arg1.Tag == 0)
            {
                Expression.Number number = (Expression.Number) _arg1;
                if (number.item.IsNegative)
                {
                    item = number.item;
                    return multiply(minusOne, Expression.NewFunction(MathNet.Symbolics.Function.Sin, Expression.NewNumber(-item)));
                }
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                if (product.item.TailOrNull != null)
                {
                    FSharpList<Expression> list = product.item;
                    if (list.HeadOrDefault.Tag == 0)
                    {
                        Expression.Number number2 = (Expression.Number) list.HeadOrDefault;
                        if (number2.item.IsNegative)
                        {
                            item = number2.item;
                            FSharpList<Expression> list2 = list.TailOrNull;
                            return multiply(minusOne, Expression.NewFunction(MathNet.Symbolics.Function.Sin, multiply(Expression.NewNumber(-item), Expression.NewProduct(list2))));
                        }
                    }
                }
            }
            return Expression.NewFunction(MathNet.Symbolics.Function.Sin, _arg1);
        }

        public static Expression sinh(Expression x) => 
            Expression.NewFunction(MathNet.Symbolics.Function.Sinh, x);

        public static Expression sqrt(Expression x) => 
            Expression.NewPower(x, Expression.NewPower(two, minusOne));

        [CompilationArgumentCounts(new int[] { 1, 1 })]
        public static Expression subtract(Expression x, Expression y) => 
            add(x, multiply(minusOne, y));

        public static Expression sum(FSharpList<Expression> xs)
        {
            if (xs.TailOrNull == null)
            {
                return zero;
            }
            return ListModule.Reduce<Expression>(new sum@344(), xs);
        }

        public static Expression sumSeq(IEnumerable<Expression> xs) => 
            SeqModule.Fold<Expression, Expression>(new sumSeq@345(), zero, xs);

        public static Expression symbol(string name) => 
            Expression.NewIdentifier(Symbol.NewSymbol(name));

        public static Expression tan(Expression _arg1)
        {
            BigRational item;
            if (ExpressionPatterns.|Zero|_|(_arg1) != null)
            {
                return zero;
            }
            if (_arg1.Tag == 0)
            {
                Expression.Number number = (Expression.Number) _arg1;
                if (number.item.IsNegative)
                {
                    item = number.item;
                    return multiply(minusOne, Expression.NewFunction(MathNet.Symbolics.Function.Tan, Expression.NewNumber(-item)));
                }
            }
            if (_arg1.Tag == 5)
            {
                Expression.Product product = (Expression.Product) _arg1;
                if (product.item.TailOrNull != null)
                {
                    FSharpList<Expression> list = product.item;
                    if (list.HeadOrDefault.Tag == 0)
                    {
                        Expression.Number number2 = (Expression.Number) list.HeadOrDefault;
                        if (number2.item.IsNegative)
                        {
                            item = number2.item;
                            FSharpList<Expression> list2 = list.TailOrNull;
                            return multiply(minusOne, Expression.NewFunction(MathNet.Symbolics.Function.Tan, multiply(Expression.NewNumber(-item), Expression.NewProduct(list2))));
                        }
                    }
                }
            }
            return Expression.NewFunction(MathNet.Symbolics.Function.Tan, _arg1);
        }

        public static Expression tanh(Expression x) => 
            Expression.NewFunction(MathNet.Symbolics.Function.Tanh, x);

        internal static Expression valueAdd@236(Value v, Expression x)
        {
            FSharpOption<Value> option;
            Value value2;
        Label_0000:
            option = Values.|Value|_|(x);
            if (option == null)
            {
                FSharpList<Expression> list3;
                if (x.Tag == 4)
                {
                    Expression.Sum sum = (Expression.Sum) x;
                    if (sum.item.TailOrNull != null)
                    {
                        FSharpList<Expression> item = sum.item;
                        FSharpOption<Value> option2 = Values.|Value|_|(item.HeadOrDefault);
                        if ((option2 != null) && (item.TailOrNull.TailOrNull == null))
                        {
                            value2 = option2.Value;
                            goto Label_0011;
                        }
                    }
                }
                if (x.Tag == 4)
                {
                    Expression.Sum sum2 = (Expression.Sum) x;
                    if (sum2.item.TailOrNull == null)
                    {
                        return Values.unpack(v);
                    }
                    FSharpList<Expression> list2 = sum2.item;
                    if (list2.TailOrNull.TailOrNull == null)
                    {
                        Expression head = list2.HeadOrDefault;
                        if (ValueModule.isZero(v))
                        {
                            return head;
                        }
                        return Expression.NewSum(FSharpList<Expression>.Cons(Values.unpack(v), FSharpList<Expression>.Cons(head, FSharpList<Expression>.Empty)));
                    }
                    FSharpOption<Value> option3 = Values.|Value|_|(list2.HeadOrDefault);
                    if (option3 != null)
                    {
                        list3 = list2.TailOrNull;
                        x = Expression.NewSum(list3);
                        v = ValueModule.sum(option3.Value, v);
                        goto Label_0000;
                    }
                }
                if (x.Tag == 4)
                {
                    Expression.Sum sum3 = (Expression.Sum) x;
                    list3 = sum3.item;
                    if (ValueModule.isZero(v))
                    {
                        return x;
                    }
                    return Expression.NewSum(FSharpList<Expression>.Cons(Values.unpack(v), list3));
                }
                if (ValueModule.isZero(v))
                {
                    return x;
                }
                return Expression.NewSum(FSharpList<Expression>.Cons(Values.unpack(v), FSharpList<Expression>.Cons(x, FSharpList<Expression>.Empty)));
            }
            value2 = option.Value;
        Label_0011:
            return Values.unpack(ValueModule.sum(v, value2));
        }

        internal static Expression valueMul@292(Value v, Expression x)
        {
            Value value2;
        Label_0000:
            if (ValueModule.isZero(v))
            {
                return zero;
            }
            FSharpOption<Value> option = Values.|Value|_|(x);
            if (option == null)
            {
                FSharpList<Expression> list3;
                if (x.Tag == 5)
                {
                    Expression.Product product = (Expression.Product) x;
                    if (product.item.TailOrNull != null)
                    {
                        FSharpList<Expression> item = product.item;
                        FSharpOption<Value> option2 = Values.|Value|_|(item.HeadOrDefault);
                        if ((option2 != null) && (item.TailOrNull.TailOrNull == null))
                        {
                            value2 = option2.Value;
                            goto Label_0020;
                        }
                    }
                }
                if (x.Tag == 5)
                {
                    Expression.Product product2 = (Expression.Product) x;
                    if (product2.item.TailOrNull == null)
                    {
                        return Values.unpack(v);
                    }
                    FSharpList<Expression> list2 = product2.item;
                    if (list2.TailOrNull.TailOrNull == null)
                    {
                        Expression head = list2.HeadOrDefault;
                        if (ValueModule.isOne(v))
                        {
                            return head;
                        }
                        return Expression.NewProduct(FSharpList<Expression>.Cons(Values.unpack(v), FSharpList<Expression>.Cons(head, FSharpList<Expression>.Empty)));
                    }
                    FSharpOption<Value> option3 = Values.|Value|_|(list2.HeadOrDefault);
                    if (option3 != null)
                    {
                        list3 = list2.TailOrNull;
                        x = Expression.NewProduct(list3);
                        v = ValueModule.product(option3.Value, v);
                        goto Label_0000;
                    }
                }
                if (x.Tag == 5)
                {
                    Expression.Product product3 = (Expression.Product) x;
                    list3 = product3.item;
                    if (ValueModule.isOne(v))
                    {
                        return x;
                    }
                    return Expression.NewProduct(FSharpList<Expression>.Cons(Values.unpack(v), list3));
                }
                if (ValueModule.isOne(v))
                {
                    return x;
                }
                return Expression.NewProduct(FSharpList<Expression>.Cons(Values.unpack(v), FSharpList<Expression>.Cons(x, FSharpList<Expression>.Empty)));
            }
            value2 = option.Value;
        Label_0020:
            return Values.unpack(ValueModule.product(v, value2));
        }

        public static Expression complexInfinity =>
            Expression.ComplexInfinity;

        public static Expression infinity =>
            Expression.PositiveInfinity;

        [CompilationMapping(SourceConstructFlags.Value)]
        public static Expression minusOne =>
            $Expression.minusOne@133;

        public static Expression negativeInfinity =>
            Expression.NegativeInfinity;

        [CompilationMapping(SourceConstructFlags.Value)]
        public static FSharpFunc<int, Expression> number =>
            $Expression.number@151;

        [CompilationMapping(SourceConstructFlags.Value)]
        public static Expression one =>
            $Expression.one@131-2;

        [CompilationMapping(SourceConstructFlags.Value)]
        public static Expression pi =>
            $Expression.pi@134;

        [CompilationMapping(SourceConstructFlags.Value)]
        public static Expression two =>
            $Expression.two@132;

        public static Expression undefined =>
            Expression.Undefined;

        [CompilationMapping(SourceConstructFlags.Value)]
        public static Expression zero =>
            $Expression.zero@130-2;

        [Serializable]
        internal class invert@338 : FSharpFunc<Expression, Expression>
        {
            internal invert@338()
            {
            }

            public override Expression Invoke(Expression _arg1) => 
                MathNet.Symbolics.Operators.invert(_arg1);
        }

        [Serializable]
        internal class number@151 : FSharpFunc<int, Expression>
        {
            internal number@151()
            {
            }

            public override Expression Invoke(int x) => 
                Expression.NewNumber(BigRational.FromInt(x));
        }

        [Serializable]
        internal class pow@328 : FSharpFunc<Expression, Expression>
        {
            public Expression y;

            internal pow@328(Expression y)
            {
                this.y = y;
            }

            public override Expression Invoke(Expression z) => 
                MathNet.Symbolics.Operators.pow(z, this.y);
        }

        [Serializable]
        internal class product@346 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal product@346()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                MathNet.Symbolics.Operators.multiply(x, y);
        }

        [Serializable]
        internal class productSeq@347 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal productSeq@347()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                MathNet.Symbolics.Operators.multiply(x, y);
        }

        [Serializable]
        internal class sum@344 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal sum@344()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                MathNet.Symbolics.Operators.add(x, y);
        }

        [Serializable]
        internal class sumSeq@345 : OptimizedClosures.FSharpFunc<Expression, Expression, Expression>
        {
            internal sumSeq@345()
            {
            }

            public override Expression Invoke(Expression x, Expression y) => 
                MathNet.Symbolics.Operators.add(x, y);
        }
    }
}

