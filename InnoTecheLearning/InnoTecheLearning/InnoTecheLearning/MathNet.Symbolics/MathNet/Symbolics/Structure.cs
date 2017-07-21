namespace MathNet.Symbolics
{
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    [RequireQualifiedAccess, CompilationMapping(SourceConstructFlags.Module)]
    public static class Structure
    {
        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("collect")]
        public static FSharpList<T> Collect<T>(FSharpFunc<Expression, FSharpOption<T>> chooser, Expression x)
        {
            FSharpFunc<Expression, FSharpOption<T>> func = chooser;
            return impl@89<T>(chooser, FSharpList<T>.Empty, x);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("collectAll")]
        public static FSharpList<T> CollectAll<T>(FSharpFunc<Expression, FSharpOption<T>> chooser, Expression x)
        {
            FSharpFunc<Expression, FSharpOption<T>> func = chooser;
            return impl@110-4<T>(chooser, FSharpList<T>.Empty, x);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("collectAllDistinct")]
        public static FSharpList<a> CollectAllDistinct<a>(FSharpFunc<Expression, FSharpOption<a>> chooser, Expression x) => 
            SeqModule.ToList<a>(SeqModule.Distinct<a>((IEnumerable<a>) CollectAll<a>(chooser, x)));

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("collectAllPredicate")]
        public static FSharpList<Expression> CollectAllPredicate(FSharpFunc<Expression, bool> predicate, Expression x)
        {
            FSharpFunc<Expression, bool> func = predicate;
            return impl@118-6(predicate, FSharpList<Expression>.Empty, x);
        }

        [CompilationSourceName("collectApproximations")]
        public static FSharpList<Expression> CollectApproximations(Expression x) => 
            SortList(CollectDistinct<Expression>(new CollectApproximations@157(), x));

        [CompilationSourceName("collectApproximationValues")]
        public static FSharpList<MathNet.Symbolics.Approximation> CollectApproximationValues(Expression x) => 
            ApproximationModule.SortList(CollectDistinct<MathNet.Symbolics.Approximation>(new CollectApproximationValues@162(), x));

        [CompilationSourceName("collectConstants")]
        public static FSharpList<Expression> CollectConstants(Expression x) => 
            SortList(CollectDistinct<Expression>(new CollectConstants@167(), x));

        [CompilationSourceName("collectConstantValues")]
        public static FSharpList<MathNet.Symbolics.Constant> CollectConstantValues(Expression x) => 
            ListModule.Sort<MathNet.Symbolics.Constant>(CollectDistinct<MathNet.Symbolics.Constant>(new CollectConstantValues@172(), x));

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("collectDistinct")]
        public static FSharpList<a> CollectDistinct<a>(FSharpFunc<Expression, FSharpOption<a>> chooser, Expression x) => 
            SeqModule.ToList<a>(SeqModule.Distinct<a>((IEnumerable<a>) Collect<a>(chooser, x)));

        [CompilationSourceName("collectFunctions")]
        public static FSharpList<Expression> CollectFunctions(Expression x) => 
            SortList(CollectAllDistinct<Expression>(new CollectFunctions@177(), x));

        [CompilationSourceName("collectFunctionTypes")]
        public static FSharpList<MathNet.Symbolics.Function> CollectFunctionTypes(Expression x) => 
            ListModule.Sort<MathNet.Symbolics.Function>(CollectAllDistinct<MathNet.Symbolics.Function>(new CollectFunctionTypes@182(), x));

        [CompilationSourceName("collectIdentifiers")]
        public static FSharpList<Expression> CollectIdentifiers(Expression x) => 
            SortList(CollectDistinct<Expression>(new CollectIdentifiers@137(), x));

        [CompilationSourceName("collectIdentifierSymbols")]
        public static FSharpList<Symbol> CollectIdentifierSymbols(Expression x) => 
            ListModule.Sort<Symbol>(CollectDistinct<Symbol>(new CollectIdentifierSymbols@142(), x));

        [CompilationSourceName("collectNumbers")]
        public static FSharpList<Expression> CollectNumbers(Expression x) => 
            SortList(CollectDistinct<Expression>(new CollectNumbers@147(), x));

        [CompilationSourceName("collectNumberValues")]
        public static FSharpList<BigRational> CollectNumberValues(Expression x) => 
            ListModule.Sort<BigRational>(CollectDistinct<BigRational>(new CollectNumberValues@152(), x));

        [CompilationSourceName("collectPowers")]
        public static FSharpList<Expression> CollectPowers(Expression x) => 
            SortList(CollectAllDistinct<Expression>(new CollectPowers@197(), x));

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("collectPredicate")]
        public static FSharpList<Expression> CollectPredicate(FSharpFunc<Expression, bool> predicate, Expression x)
        {
            FSharpFunc<Expression, bool> func = predicate;
            return impl@97-2(predicate, FSharpList<Expression>.Empty, x);
        }

        [CompilationSourceName("collectProducts")]
        public static FSharpList<Expression> CollectProducts(Expression x) => 
            SortList(CollectAllDistinct<Expression>(new CollectProducts@192(), x));

        [CompilationSourceName("collectSums")]
        public static FSharpList<Expression> CollectSums(Expression x) => 
            SortList(CollectAllDistinct<Expression>(new CollectSums@187(), x));

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("fold")]
        public static a Fold<a>(FSharpFunc<a, FSharpFunc<Expression, a>> f, a s, Expression _arg1)
        {
            FSharpList<Expression> item;
            switch (_arg1.Tag)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 9:
                case 10:
                case 11:
                    return s;

                case 5:
                    item = ((Expression.Product) _arg1).item;
                    break;

                case 6:
                {
                    Expression.Power power = (Expression.Power) _arg1;
                    Expression head = power.item1;
                    Expression expression2 = power.item2;
                    return ListModule.Fold<Expression, a>(f, s, FSharpList<Expression>.Cons(head, FSharpList<Expression>.Cons(expression2, FSharpList<Expression>.Empty)));
                }
                case 7:
                {
                    Expression.Function function = (Expression.Function) _arg1;
                    return FSharpFunc<a, Expression>.InvokeFast<a>(f, s, function.item2);
                }
                case 8:
                    item = ((Expression.FunctionN) _arg1).item2;
                    break;

                case 12:
                    return s;

                default:
                    item = ((Expression.Sum) _arg1).item;
                    break;
            }
            return ListModule.Fold<Expression, a>(f, s, item);
        }

        internal static FSharpList<T> impl@110-4<T>(FSharpFunc<Expression, FSharpOption<T>> chooser, FSharpList<T> acc, Expression x)
        {
            FSharpFunc<FSharpList<T>, FSharpFunc<Expression, FSharpList<T>>> f = new impl@110-5<T>(chooser);
            FSharpOption<T> option = chooser.Invoke(x);
            if (option == null)
            {
                return Fold<FSharpList<T>>(f, acc, x);
            }
            T head = option.Value;
            return Fold<FSharpList<T>>(f, FSharpList<T>.Cons(head, acc), x);
        }

        internal static FSharpList<Expression> impl@118-6(FSharpFunc<Expression, bool> predicate, FSharpList<Expression> acc, Expression x)
        {
            FSharpFunc<FSharpList<Expression>, FSharpFunc<Expression, FSharpList<Expression>>> f = new impl@118-7(predicate);
            if (predicate.Invoke(x))
            {
                return Fold<FSharpList<Expression>>(f, FSharpList<Expression>.Cons(x, acc), x);
            }
            return Fold<FSharpList<Expression>>(f, acc, x);
        }

        internal static FSharpList<T> impl@89<T>(FSharpFunc<Expression, FSharpOption<T>> chooser, FSharpList<T> acc, Expression x)
        {
            FSharpFunc<FSharpList<T>, FSharpFunc<Expression, FSharpList<T>>> f = new impl@89-1<T>(chooser);
            FSharpOption<T> option = chooser.Invoke(x);
            if (option == null)
            {
                return Fold<FSharpList<T>>(f, acc, x);
            }
            FSharpOption<T> option2 = option;
            return FSharpList<T>.Cons(option2.Value, acc);
        }

        internal static FSharpList<Expression> impl@97-2(FSharpFunc<Expression, bool> predicate, FSharpList<Expression> acc, Expression x)
        {
            FSharpFunc<FSharpList<Expression>, FSharpFunc<Expression, FSharpList<Expression>>> f = new impl@97-3(predicate);
            if (predicate.Invoke(x))
            {
                return FSharpList<Expression>.Cons(x, acc);
            }
            return Fold<FSharpList<Expression>>(f, acc, x);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("freeOf")]
        public static bool IsFreeOf(Expression symbol, Expression x)
        {
            FSharpList<Expression> item;
        Label_0000:
            if (symbol.Equals(x, LanguagePrimitives.GenericEqualityComparer))
            {
                return false;
            }
            switch (x.Tag)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 9:
                case 10:
                case 11:
                    return true;

                case 5:
                    item = ((Expression.Product) x).item;
                    break;

                case 6:
                {
                    Expression.Power power = (Expression.Power) x;
                    Expression expression = power.item1;
                    Expression expression2 = power.item2;
                    if (!IsFreeOf(symbol, expression))
                    {
                        return false;
                    }
                    x = expression2;
                    symbol = symbol;
                    goto Label_0000;
                }
                case 7:
                {
                    Expression.Function function = (Expression.Function) x;
                    x = function.item2;
                    symbol = symbol;
                    goto Label_0000;
                }
                case 8:
                    item = ((Expression.FunctionN) x).item2;
                    break;

                case 12:
                    return true;

                default:
                    item = ((Expression.Sum) x).item;
                    break;
            }
            return ListModule.ForAll<Expression>(new IsFreeOf@32(symbol), item);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("freeOfSet")]
        public static bool IsFreeOfSet(HashSet<Expression> symbols, Expression x)
        {
            FSharpList<Expression> item;
        Label_0000:
            if (symbols.Contains(x))
            {
                return false;
            }
            switch (x.Tag)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 9:
                case 10:
                case 11:
                    return true;

                case 5:
                    item = ((Expression.Product) x).item;
                    break;

                case 6:
                {
                    Expression.Power power = (Expression.Power) x;
                    Expression expression = power.item1;
                    Expression expression2 = power.item2;
                    if (!IsFreeOfSet(symbols, expression))
                    {
                        return false;
                    }
                    x = expression2;
                    symbols = symbols;
                    goto Label_0000;
                }
                case 7:
                {
                    Expression.Function function = (Expression.Function) x;
                    x = function.item2;
                    symbols = symbols;
                    goto Label_0000;
                }
                case 8:
                    item = ((Expression.FunctionN) x).item2;
                    break;

                case 12:
                    return true;

                default:
                    item = ((Expression.Sum) x).item;
                    break;
            }
            return ListModule.ForAll<Expression>(new IsFreeOfSet@42(symbols), item);
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("map")]
        public static Expression Map(FSharpFunc<Expression, Expression> f, Expression _arg1)
        {
            FSharpList<Expression> item;
            Expression expression;
            switch (_arg1.Tag)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 9:
                case 10:
                case 11:
                case 12:
                    return _arg1;

                case 4:
                {
                    Expression.Sum sum = (Expression.Sum) _arg1;
                    item = sum.item;
                    return MathNet.Symbolics.Operators.sum(ListModule.Map<Expression, Expression>(f, item));
                }
                case 5:
                {
                    Expression.Product product = (Expression.Product) _arg1;
                    item = product.item;
                    return MathNet.Symbolics.Operators.product(ListModule.Map<Expression, Expression>(f, item));
                }
                case 6:
                {
                    Expression.Power power = (Expression.Power) _arg1;
                    expression = power.item1;
                    Expression func = power.item2;
                    return MathNet.Symbolics.Operators.pow(f.Invoke(expression), f.Invoke(func));
                }
                case 7:
                {
                    Expression.Function function = (Expression.Function) _arg1;
                    expression = function.item2;
                    return MathNet.Symbolics.Operators.apply(function.item1, f.Invoke(expression));
                }
                case 8:
                {
                    Expression.FunctionN nn = (Expression.FunctionN) _arg1;
                    item = nn.item2;
                    FSharpList<Expression> list2 = ListModule.Map<Expression, Expression>(f, item);
                    throw new Exception("not supported yet");
                }
            }
            return _arg1;
        }

        [CompilationSourceName("numberOfOperands")]
        public static int NumberOfOperands(Expression _arg1)
        {
            FSharpList<Expression> item;
            switch (_arg1.Tag)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 9:
                case 10:
                case 11:
                    return 0;

                case 5:
                    item = ((Expression.Product) _arg1).item;
                    break;

                case 6:
                    return 2;

                case 7:
                    return 1;

                case 8:
                {
                    Expression.FunctionN nn = (Expression.FunctionN) _arg1;
                    return nn.item2.Length;
                }
                case 12:
                    return 0;

                default:
                    item = ((Expression.Sum) _arg1).item;
                    break;
            }
            return item.Length;
        }

        [CompilationArgumentCounts(new int[] { 1, 1 }), CompilationSourceName("operand")]
        public static Expression Operand(int i, Expression _arg1)
        {
            Expression.Power power;
            FSharpList<Expression> item;
            switch (_arg1.Tag)
            {
                case 4:
                    item = ((Expression.Sum) _arg1).item;
                    goto Label_0090;

                case 5:
                    item = ((Expression.Product) _arg1).item;
                    goto Label_0090;

                case 6:
                    power = (Expression.Power) _arg1;
                    if (i != 0)
                    {
                        break;
                    }
                    return power.item1;

                case 8:
                    item = ((Expression.FunctionN) _arg1).item2;
                    goto Label_0090;
            }
            if (_arg1.Tag == 6)
            {
                power = (Expression.Power) _arg1;
                if (i == 1)
                {
                    return power.item2;
                }
            }
            if (_arg1.Tag == 7)
            {
                Expression.Function function = (Expression.Function) _arg1;
                if (i == 0)
                {
                    return function.item2;
                }
            }
            throw new Exception("no such operand");
        Label_0090:
            return ListModule.Get<Expression>(item, i);
        }

        [CompilationSourceName("sortList")]
        public static FSharpList<Expression> SortList(FSharpList<Expression> list) => 
            ListModule.SortWith<Expression>(new SortList@80-1(), list);

        [CompilationArgumentCounts(new int[] { 1, 1, 1 }), CompilationSourceName("substitute")]
        public static Expression Substitute(Expression y, Expression r, Expression x)
        {
            FSharpList<Expression> item;
            Expression expression;
            if (y.Equals(x, LanguagePrimitives.GenericEqualityComparer))
            {
                return r;
            }
            switch (x.Tag)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 9:
                case 10:
                case 11:
                    return x;

                case 5:
                {
                    Expression.Product product = (Expression.Product) x;
                    item = product.item;
                    return MathNet.Symbolics.Operators.product(ListModule.Map<Expression, Expression>(new Substitute@53-1(y, r), item));
                }
                case 6:
                {
                    Expression.Power power = (Expression.Power) x;
                    expression = power.item1;
                    Expression expression2 = power.item2;
                    return MathNet.Symbolics.Operators.pow(Substitute(y, r, expression), Substitute(y, r, expression2));
                }
                case 7:
                {
                    Expression.Function function = (Expression.Function) x;
                    expression = function.item2;
                    return MathNet.Symbolics.Operators.apply(function.item1, Substitute(y, r, expression));
                }
                case 8:
                {
                    Expression.FunctionN nn = (Expression.FunctionN) x;
                    item = nn.item2;
                    FSharpList<Expression> list2 = ListModule.Map<Expression, Expression>(new Substitute@56-2(y, r), item);
                    throw new Exception("not supported yet");
                }
                case 12:
                    return x;
            }
            Expression.Sum sum = (Expression.Sum) x;
            item = sum.item;
            return MathNet.Symbolics.Operators.sum(ListModule.Map<Expression, Expression>(new Substitute@52(y, r), item));
        }

        [Serializable]
        internal class CollectApproximations@157 : FSharpFunc<Expression, FSharpOption<Expression>>
        {
            internal CollectApproximations@157()
            {
            }

            public override FSharpOption<Expression> Invoke(Expression _arg1)
            {
                if (_arg1.Tag == 1)
                {
                    return FSharpOption<Expression>.Some(_arg1);
                }
                return null;
            }
        }

        [Serializable]
        internal class CollectApproximationValues@162 : FSharpFunc<Expression, FSharpOption<MathNet.Symbolics.Approximation>>
        {
            internal CollectApproximationValues@162()
            {
            }

            public override FSharpOption<MathNet.Symbolics.Approximation> Invoke(Expression _arg1)
            {
                if (_arg1.Tag == 1)
                {
                    Expression.Approximation approximation = (Expression.Approximation) _arg1;
                    return FSharpOption<MathNet.Symbolics.Approximation>.Some(approximation.item);
                }
                return null;
            }
        }

        [Serializable]
        internal class CollectConstants@167 : FSharpFunc<Expression, FSharpOption<Expression>>
        {
            internal CollectConstants@167()
            {
            }

            public override FSharpOption<Expression> Invoke(Expression _arg1)
            {
                if (_arg1.Tag == 3)
                {
                    return FSharpOption<Expression>.Some(_arg1);
                }
                return null;
            }
        }

        [Serializable]
        internal class CollectConstantValues@172 : FSharpFunc<Expression, FSharpOption<MathNet.Symbolics.Constant>>
        {
            internal CollectConstantValues@172()
            {
            }

            public override FSharpOption<MathNet.Symbolics.Constant> Invoke(Expression _arg1)
            {
                if (_arg1.Tag == 3)
                {
                    Expression.Constant constant = (Expression.Constant) _arg1;
                    return FSharpOption<MathNet.Symbolics.Constant>.Some(constant.item);
                }
                return null;
            }
        }

        [Serializable]
        internal class CollectFunctions@177 : FSharpFunc<Expression, FSharpOption<Expression>>
        {
            internal CollectFunctions@177()
            {
            }

            public override FSharpOption<Expression> Invoke(Expression _arg1)
            {
                Expression expression;
                switch (_arg1.Tag)
                {
                    case 7:
                        expression = _arg1;
                        break;

                    case 8:
                        expression = _arg1;
                        break;

                    default:
                        return null;
                }
                return FSharpOption<Expression>.Some(expression);
            }
        }

        [Serializable]
        internal class CollectFunctionTypes@182 : FSharpFunc<Expression, FSharpOption<MathNet.Symbolics.Function>>
        {
            internal CollectFunctionTypes@182()
            {
            }

            public override FSharpOption<MathNet.Symbolics.Function> Invoke(Expression _arg1)
            {
                MathNet.Symbolics.Function function;
                switch (_arg1.Tag)
                {
                    case 7:
                        function = ((Expression.Function) _arg1).item1;
                        break;

                    case 8:
                        function = ((Expression.FunctionN) _arg1).item1;
                        break;

                    default:
                        return null;
                }
                return FSharpOption<MathNet.Symbolics.Function>.Some(function);
            }
        }

        [Serializable]
        internal class CollectIdentifiers@137 : FSharpFunc<Expression, FSharpOption<Expression>>
        {
            internal CollectIdentifiers@137()
            {
            }

            public override FSharpOption<Expression> Invoke(Expression _arg1)
            {
                if (_arg1.Tag == 2)
                {
                    return FSharpOption<Expression>.Some(_arg1);
                }
                return null;
            }
        }

        [Serializable]
        internal class CollectIdentifierSymbols@142 : FSharpFunc<Expression, FSharpOption<Symbol>>
        {
            internal CollectIdentifierSymbols@142()
            {
            }

            public override FSharpOption<Symbol> Invoke(Expression _arg1)
            {
                if (_arg1.Tag == 2)
                {
                    Expression.Identifier identifier = (Expression.Identifier) _arg1;
                    return FSharpOption<Symbol>.Some(identifier.item);
                }
                return null;
            }
        }

        [Serializable]
        internal class CollectNumbers@147 : FSharpFunc<Expression, FSharpOption<Expression>>
        {
            internal CollectNumbers@147()
            {
            }

            public override FSharpOption<Expression> Invoke(Expression _arg1)
            {
                if (_arg1.Tag == 0)
                {
                    return FSharpOption<Expression>.Some(_arg1);
                }
                return null;
            }
        }

        [Serializable]
        internal class CollectNumberValues@152 : FSharpFunc<Expression, FSharpOption<BigRational>>
        {
            internal CollectNumberValues@152()
            {
            }

            public override FSharpOption<BigRational> Invoke(Expression _arg1)
            {
                if (_arg1.Tag == 0)
                {
                    Expression.Number number = (Expression.Number) _arg1;
                    return FSharpOption<BigRational>.Some(number.item);
                }
                return null;
            }
        }

        [Serializable]
        internal class CollectPowers@197 : FSharpFunc<Expression, FSharpOption<Expression>>
        {
            internal CollectPowers@197()
            {
            }

            public override FSharpOption<Expression> Invoke(Expression _arg1)
            {
                if (_arg1.Tag == 6)
                {
                    return FSharpOption<Expression>.Some(_arg1);
                }
                return null;
            }
        }

        [Serializable]
        internal class CollectProducts@192 : FSharpFunc<Expression, FSharpOption<Expression>>
        {
            internal CollectProducts@192()
            {
            }

            public override FSharpOption<Expression> Invoke(Expression _arg1)
            {
                if (_arg1.Tag == 5)
                {
                    return FSharpOption<Expression>.Some(_arg1);
                }
                return null;
            }
        }

        [Serializable]
        internal class CollectSums@187 : FSharpFunc<Expression, FSharpOption<Expression>>
        {
            internal CollectSums@187()
            {
            }

            public override FSharpOption<Expression> Invoke(Expression _arg1)
            {
                if (_arg1.Tag == 4)
                {
                    return FSharpOption<Expression>.Some(_arg1);
                }
                return null;
            }
        }

        [Serializable]
        internal class impl@110-5<T> : OptimizedClosures.FSharpFunc<FSharpList<T>, Expression, FSharpList<T>>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public FSharpFunc<Expression, FSharpOption<T>> chooser;

            internal impl@110-5(FSharpFunc<Expression, FSharpOption<T>> chooser)
            {
                this.chooser = chooser;
            }

            public override FSharpList<T> Invoke(FSharpList<T> acc, Expression x) => 
                Structure.impl@110-4<T>(this.chooser, acc, x);
        }

        [Serializable]
        internal class impl@118-7 : OptimizedClosures.FSharpFunc<FSharpList<Expression>, Expression, FSharpList<Expression>>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public FSharpFunc<Expression, bool> predicate;

            internal impl@118-7(FSharpFunc<Expression, bool> predicate)
            {
                this.predicate = predicate;
            }

            public override FSharpList<Expression> Invoke(FSharpList<Expression> acc, Expression x) => 
                Structure.impl@118-6(this.predicate, acc, x);
        }

        [Serializable]
        internal class impl@89-1<T> : OptimizedClosures.FSharpFunc<FSharpList<T>, Expression, FSharpList<T>>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public FSharpFunc<Expression, FSharpOption<T>> chooser;

            internal impl@89-1(FSharpFunc<Expression, FSharpOption<T>> chooser)
            {
                this.chooser = chooser;
            }

            public override FSharpList<T> Invoke(FSharpList<T> acc, Expression x) => 
                Structure.impl@89<T>(this.chooser, acc, x);
        }

        [Serializable]
        internal class impl@97-3 : OptimizedClosures.FSharpFunc<FSharpList<Expression>, Expression, FSharpList<Expression>>
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            public FSharpFunc<Expression, bool> predicate;

            internal impl@97-3(FSharpFunc<Expression, bool> predicate)
            {
                this.predicate = predicate;
            }

            public override FSharpList<Expression> Invoke(FSharpList<Expression> acc, Expression x) => 
                Structure.impl@97-2(this.predicate, acc, x);
        }

        [Serializable]
        internal class IsFreeOf@32 : FSharpFunc<Expression, bool>
        {
            public Expression symbol;

            internal IsFreeOf@32(Expression symbol)
            {
                this.symbol = symbol;
            }

            public override bool Invoke(Expression x) => 
                Structure.IsFreeOf(this.symbol, x);
        }

        [Serializable]
        internal class IsFreeOfSet@42 : FSharpFunc<Expression, bool>
        {
            public HashSet<Expression> symbols;

            internal IsFreeOfSet@42(HashSet<Expression> symbols)
            {
                this.symbols = symbols;
            }

            public override bool Invoke(Expression x) => 
                Structure.IsFreeOfSet(this.symbols, x);
        }

        [Serializable]
        internal class SortList@80-1 : OptimizedClosures.FSharpFunc<Expression, Expression, int>
        {
            internal SortList@80-1()
            {
            }

            public override int Invoke(Expression a, Expression b)
            {
                if (a.Equals(b, LanguagePrimitives.GenericEqualityComparer))
                {
                    return 0;
                }
                if (MathNet.Symbolics.Operators.orderRelation(a, b))
                {
                    return -1;
                }
                return 1;
            }
        }

        [Serializable]
        internal class Substitute@52 : FSharpFunc<Expression, Expression>
        {
            public Expression r;
            public Expression y;

            internal Substitute@52(Expression y, Expression r)
            {
                this.y = y;
                this.r = r;
            }

            public override Expression Invoke(Expression x) => 
                Structure.Substitute(this.y, this.r, x);
        }

        [Serializable]
        internal class Substitute@53-1 : FSharpFunc<Expression, Expression>
        {
            public Expression r;
            public Expression y;

            internal Substitute@53-1(Expression y, Expression r)
            {
                this.y = y;
                this.r = r;
            }

            public override Expression Invoke(Expression x) => 
                Structure.Substitute(this.y, this.r, x);
        }

        [Serializable]
        internal class Substitute@56-2 : FSharpFunc<Expression, Expression>
        {
            public Expression r;
            public Expression y;

            internal Substitute@56-2(Expression y, Expression r)
            {
                this.y = y;
                this.r = r;
            }

            public override Expression Invoke(Expression x) => 
                Structure.Substitute(this.y, this.r, x);
        }
    }
}

