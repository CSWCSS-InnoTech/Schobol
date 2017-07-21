namespace MathNet.Symbolics
{
    using MathNet.Numerics;
    using Microsoft.FSharp.Collections;
    using Microsoft.FSharp.Core;
    using System;
    using System.Collections;
    using System.Diagnostics;
    using System.Numerics;
    using System.Runtime.CompilerServices;

    [Serializable, StructuralEquality, NoComparison, DebuggerDisplay("{__DebugDisplay(),nq}"), CompilationMapping(SourceConstructFlags.SumType)]
    public class Expression : IEquatable<Expression>, IStructuralEquatable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal readonly int _tag;
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly Expression _unique_ComplexInfinity = new Expression(9);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly Expression _unique_NegativeInfinity = new Expression(11);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly Expression _unique_PositiveInfinity = new Expression(10);
        [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
        internal static readonly Expression _unique_Undefined = new Expression(12);

        [CompilerGenerated]
        internal Expression(int _tag)
        {
            this._tag = _tag;
        }

        [CompilerGenerated]
        internal object __DebugDisplay() => 
            ExtraTopLevelOperators.PrintFormatToString<FSharpFunc<Expression, string>>(new PrintfFormat<FSharpFunc<Expression, string>, Unit, string, string, string>("%+0.8A")).Invoke(this);

        public static Expression Abs(Expression x) => 
            MathNet.Symbolics.Operators.Abs(x);

        public static Expression Apply(MathNet.Symbolics.Function f, Expression x) => 
            MathNet.Symbolics.Operators.Apply(f, x);

        public static a ApplyN<a>(MathNet.Symbolics.Function f, FSharpList<Expression> xs)
        {
            throw new Exception("not supported yet");
        }

        public static Expression ArcCos(Expression x) => 
            NewFunction(MathNet.Symbolics.Function.Acos, x);

        public static Expression ArcSin(Expression x) => 
            NewFunction(MathNet.Symbolics.Function.Asin, x);

        public static Expression ArcTan(Expression x) => 
            NewFunction(MathNet.Symbolics.Function.Atan, x);

        public static Expression Cos(Expression x) => 
            MathNet.Symbolics.Operators.cos(x);

        public static Expression Cosh(Expression x) => 
            NewFunction(MathNet.Symbolics.Function.Cosh, x);

        public static Expression Cot(Expression x) => 
            NewFunction(MathNet.Symbolics.Function.Cot, x);

        public static Expression Csc(Expression x) => 
            NewFunction(MathNet.Symbolics.Function.Csc, x);

        [CompilerGenerated]
        public bool Equals(Expression obj)
        {
            var @this = this;
        Label_0001:
            if (@this == null)
            {
                return (obj == null);
            }
            if (obj == null)
            {
                return false;
            }
            int num = @this._tag;
            int num2 = obj._tag;
            if (num != num2)
            {
                return false;
            }
            switch (@this.Tag)
            {
                case 0:
                {
                    Number number = (Number) @this;
                    Number number2 = (Number) obj;
                    return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic<BigRational>(number.item, number2.item);
                }
                case 1:
                {
                    Approximation approximation = (Approximation) @this;
                    Approximation approximation2 = (Approximation) obj;
                    return approximation.item.Equals(approximation2.item);
                }
                case 2:
                {
                    Identifier identifier = (Identifier) @this;
                    Identifier identifier2 = (Identifier) obj;
                    return identifier.item.Equals(identifier2.item);
                }
                case 3:
                {
                    Constant constant = (Constant) @this;
                    Constant constant2 = (Constant) obj;
                    return constant.item.Equals(constant2.item);
                }
                case 4:
                {
                    Sum sum = (Sum) @this;
                    Sum sum2 = (Sum) obj;
                    return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic<FSharpList<Expression>>(sum.item, sum2.item);
                }
                case 5:
                {
                    Product product = (Product) @this;
                    Product product2 = (Product) obj;
                    return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic<FSharpList<Expression>>(product.item, product2.item);
                }
                case 6:
                {
                    Power power = (Power) @this;
                    Power power2 = (Power) obj;
                    if (!power.item1.Equals(power2.item1))
                    {
                        return false;
                    }
                    obj = power2.item2;
                    @this = power.item2;
                    goto Label_0001;
                }
                case 7:
                {
                    Function function = (Function) @this;
                    Function function2 = (Function) obj;
                    if (!function.item1.Equals(function2.item1))
                    {
                        return false;
                    }
                    obj = function2.item2;
                    @this = function.item2;
                    goto Label_0001;
                }
                case 8:
                {
                    FunctionN nn = (FunctionN) @this;
                    FunctionN nn2 = (FunctionN) obj;
                    if (!nn.item1.Equals(nn2.item1))
                    {
                        return false;
                    }
                    return LanguagePrimitives.HashCompare.GenericEqualityERIntrinsic<FSharpList<Expression>>(nn.item2, nn2.item2);
                }
            }
            return true;
        }

        [CompilerGenerated]
        public sealed override bool Equals(object obj)
        {
            Expression expression = obj as Expression;
            return ((expression != null) && this.Equals(expression));
        }

        [CompilerGenerated]
        public bool Equals(object obj, IEqualityComparer comp)
        {
            var @this = this;
            FSharpList<Expression> list;
            FSharpList<Expression> list2;
            Expression expression2;
            Expression expression3;
            MathNet.Symbolics.Function function3;
            MathNet.Symbolics.Function function4;
        Label_0001:
            if (@this == null)
            {
                return (obj == null);
            }
            Expression expression = obj as Expression;
            if (expression == null)
            {
                return false;
            }
            int num = @this._tag;
            int num2 = expression._tag;
            if (num != num2)
            {
                return false;
            }
            switch (@this.Tag)
            {
                case 0:
                {
                    Number number = (Number) @this;
                    Number number2 = (Number) expression;
                    return LanguagePrimitives.HashCompare.GenericEqualityWithComparerIntrinsic<BigRational>(comp, number.item, number2.item);
                }
                case 1:
                {
                    Approximation approximation = (Approximation) @this;
                    Approximation approximation2 = (Approximation) expression;
                    MathNet.Symbolics.Approximation item = approximation.item;
                    MathNet.Symbolics.Approximation approximation4 = approximation2.item;
                    return item.Equals(approximation4, comp);
                }
                case 2:
                {
                    Identifier identifier = (Identifier) @this;
                    Identifier identifier2 = (Identifier) expression;
                    MathNet.Symbolics.Symbol symbol = identifier.item;
                    MathNet.Symbolics.Symbol symbol2 = identifier2.item;
                    return symbol.Equals(symbol2, comp);
                }
                case 3:
                {
                    Constant constant = (Constant) @this;
                    Constant constant2 = (Constant) expression;
                    MathNet.Symbolics.Constant constant3 = constant.item;
                    MathNet.Symbolics.Constant constant4 = constant2.item;
                    return constant3.Equals(constant4, comp);
                }
                case 4:
                {
                    Sum sum = (Sum) @this;
                    Sum sum2 = (Sum) expression;
                    list = sum.item;
                    list2 = sum2.item;
                    return list.Equals(list2, comp);
                }
                case 5:
                {
                    Product product = (Product) @this;
                    Product product2 = (Product) expression;
                    list = product.item;
                    list2 = product2.item;
                    return list.Equals(list2, comp);
                }
                case 6:
                {
                    Power power = (Power) @this;
                    Power power2 = (Power) expression;
                    expression2 = power.item1;
                    expression3 = power2.item1;
                    if (!expression2.Equals(expression3, comp))
                    {
                        return false;
                    }
                    expression2 = power.item2;
                    expression3 = power2.item2;
                    comp = comp;
                    obj = expression3;
                    @this = expression2;
                    goto Label_0001;
                }
                case 7:
                {
                    Function function = (Function) @this;
                    Function function2 = (Function) expression;
                    function3 = function.item1;
                    function4 = function2.item1;
                    if (!function3.Equals(function4, comp))
                    {
                        return false;
                    }
                    expression2 = function.item2;
                    expression3 = function2.item2;
                    comp = comp;
                    obj = expression3;
                    @this = expression2;
                    goto Label_0001;
                }
                case 8:
                {
                    FunctionN nn = (FunctionN) @this;
                    FunctionN nn2 = (FunctionN) expression;
                    function3 = nn.item1;
                    function4 = nn2.item1;
                    if (!function3.Equals(function4, comp))
                    {
                        return false;
                    }
                    list = nn.item2;
                    list2 = nn2.item2;
                    return list.Equals(list2, comp);
                }
            }
            return true;
        }

        public static Expression Exp(Expression x) => 
            MathNet.Symbolics.Operators.Exp(x);

        public static Expression FromInt32(int x) => 
            NewNumber(BigRational.FromInt(x));

        public static Expression FromInt64(long x) => 
            NewNumber(BigRational.FromBigInt(new BigInteger(x)));

        public static Expression FromInteger(BigInteger x) => 
            NewNumber(BigRational.FromBigInt(x));

        public static Expression FromIntegerFraction(BigInteger n, BigInteger d) => 
            NewNumber(BigRational.FromBigIntFraction(n, d));

        public static Expression FromRational(BigRational x) => 
            NewNumber(x);

        [CompilerGenerated]
        public sealed override int GetHashCode() => 
            this.GetHashCode(LanguagePrimitives.GenericEqualityComparer);

        [CompilerGenerated]
        public sealed override int GetHashCode(IEqualityComparer comp)
        {
            if (this == null)
            {
                return 0;
            }
            int num = 0;
            switch (this.Tag)
            {
                case 1:
                {
                    Approximation approximation = (Approximation) this;
                    num = 1;
                    return (-1640531527 + (approximation.item.GetHashCode(comp) + ((num << 6) + (num >> 2))));
                }
                case 2:
                {
                    Identifier identifier = (Identifier) this;
                    num = 2;
                    return (-1640531527 + (identifier.item.GetHashCode(comp) + ((num << 6) + (num >> 2))));
                }
                case 3:
                {
                    Constant constant = (Constant) this;
                    num = 3;
                    return (-1640531527 + (constant.item.GetHashCode(comp) + ((num << 6) + (num >> 2))));
                }
                case 4:
                {
                    Sum sum = (Sum) this;
                    num = 4;
                    return (-1640531527 + (sum.item.GetHashCode(comp) + ((num << 6) + (num >> 2))));
                }
                case 5:
                {
                    Product product = (Product) this;
                    num = 5;
                    return (-1640531527 + (product.item.GetHashCode(comp) + ((num << 6) + (num >> 2))));
                }
                case 6:
                {
                    Power power = (Power) this;
                    num = 6;
                    num = -1640531527 + (power.item2.GetHashCode(comp) + ((num << 6) + (num >> 2)));
                    return (-1640531527 + (power.item1.GetHashCode(comp) + ((num << 6) + (num >> 2))));
                }
                case 7:
                {
                    Function function = (Function) this;
                    num = 7;
                    num = -1640531527 + (function.item2.GetHashCode(comp) + ((num << 6) + (num >> 2)));
                    return (-1640531527 + (function.item1.GetHashCode(comp) + ((num << 6) + (num >> 2))));
                }
                case 8:
                {
                    FunctionN nn = (FunctionN) this;
                    num = 8;
                    num = -1640531527 + (nn.item2.GetHashCode(comp) + ((num << 6) + (num >> 2)));
                    return (-1640531527 + (nn.item1.GetHashCode(comp) + ((num << 6) + (num >> 2))));
                }
                case 9:
                    return 9;

                case 10:
                    return 10;

                case 11:
                    return 11;

                case 12:
                    return 12;
            }
            Number number = (Number) this;
            num = 0;
            return (-1640531527 + (LanguagePrimitives.HashCompare.GenericHashWithComparerIntrinsic<BigRational>(comp, number.item) + ((num << 6) + (num >> 2))));
        }

        public static Expression Invert(Expression x) => 
            MathNet.Symbolics.Operators.invert(x);

        public static Expression Ln(Expression x) => 
            MathNet.Symbolics.Operators.ln(x);

        public static Expression Log(Expression basis, Expression x) => 
            MathNet.Symbolics.Operators.log(basis, x);

        [CompilationMapping(SourceConstructFlags.UnionCase, 1)]
        public static Expression NewApproximation(MathNet.Symbolics.Approximation item) => 
            new Approximation(item);

        [CompilationMapping(SourceConstructFlags.UnionCase, 3)]
        public static Expression NewConstant(MathNet.Symbolics.Constant item) => 
            new Constant(item);

        [CompilationMapping(SourceConstructFlags.UnionCase, 7)]
        public static Expression NewFunction(MathNet.Symbolics.Function item1, Expression item2) => 
            new Function(item1, item2);

        [CompilationMapping(SourceConstructFlags.UnionCase, 8)]
        public static Expression NewFunctionN(MathNet.Symbolics.Function item1, FSharpList<Expression> item2) => 
            new FunctionN(item1, item2);

        [CompilationMapping(SourceConstructFlags.UnionCase, 2)]
        public static Expression NewIdentifier(MathNet.Symbolics.Symbol item) => 
            new Identifier(item);

        [CompilationMapping(SourceConstructFlags.UnionCase, 0)]
        public static Expression NewNumber(BigRational item) => 
            new Number(item);

        [CompilationMapping(SourceConstructFlags.UnionCase, 6)]
        public static Expression NewPower(Expression item1, Expression item2) => 
            new Power(item1, item2);

        [CompilationMapping(SourceConstructFlags.UnionCase, 5)]
        public static Expression NewProduct(FSharpList<Expression> item) => 
            new Product(item);

        [CompilationMapping(SourceConstructFlags.UnionCase, 4)]
        public static Expression NewSum(FSharpList<Expression> item) => 
            new Sum(item);

        public static Expression operator +(Expression x, Expression y) => 
            MathNet.Symbolics.Operators.add(x, y);

        public static Expression operator +(Expression x, double y) => 
            MathNet.Symbolics.Operators.add(x, Values.unpack(ValueModule.real(y)));

        public static Expression operator +(Expression x, int y) => 
            MathNet.Symbolics.Operators.add(x, MathNet.Symbolics.Operators.number.Invoke(y));

        public static Expression operator +(double x, Expression y) => 
            MathNet.Symbolics.Operators.add(Values.unpack(ValueModule.real(x)), y);

        public static Expression operator +(int x, Expression y) => 
            MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.number.Invoke(x), y);

        public static Expression operator /(Expression x, Expression y) => 
            MathNet.Symbolics.Operators.multiply(x, MathNet.Symbolics.Operators.invert(y));

        public static Expression operator /(Expression x, double y)
        {
            Expression expression = Values.unpack(ValueModule.real(y));
            return MathNet.Symbolics.Operators.multiply(x, MathNet.Symbolics.Operators.invert(expression));
        }

        public static Expression operator /(Expression x, int y)
        {
            Expression expression = MathNet.Symbolics.Operators.number.Invoke(y);
            return MathNet.Symbolics.Operators.multiply(x, MathNet.Symbolics.Operators.invert(expression));
        }

        public static Expression operator /(double x, Expression y) => 
            MathNet.Symbolics.Operators.multiply(Values.unpack(ValueModule.real(x)), MathNet.Symbolics.Operators.invert(y));

        public static Expression operator /(int x, Expression y) => 
            MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(x), MathNet.Symbolics.Operators.invert(y));

        public static implicit operator Expression(BigRational x) => 
            NewNumber(x);

        public static implicit operator Expression(double x) => 
            Values.unpack(ValueModule.real(x));

        public static implicit operator Expression(int x) => 
            NewNumber(BigRational.FromInt(x));

        public static implicit operator Expression(long x) => 
            NewNumber(BigRational.FromBigInt(new BigInteger(x)));

        public static implicit operator Expression(BigInteger x) => 
            NewNumber(BigRational.FromBigInt(x));

        public static Expression operator *(Expression x, Expression y) => 
            MathNet.Symbolics.Operators.multiply(x, y);

        public static Expression operator *(Expression x, double y) => 
            MathNet.Symbolics.Operators.multiply(x, Values.unpack(ValueModule.real(y)));

        public static Expression operator *(Expression x, int y) => 
            MathNet.Symbolics.Operators.multiply(x, MathNet.Symbolics.Operators.number.Invoke(y));

        public static Expression operator *(double x, Expression y) => 
            MathNet.Symbolics.Operators.multiply(Values.unpack(ValueModule.real(x)), y);

        public static Expression operator *(int x, Expression y) => 
            MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.number.Invoke(x), y);

        public static Expression operator -(Expression x, Expression y) => 
            MathNet.Symbolics.Operators.add(x, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, y));

        public static Expression operator -(Expression x, double y)
        {
            Expression expression = Values.unpack(ValueModule.real(y));
            return MathNet.Symbolics.Operators.add(x, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression));
        }

        public static Expression operator -(Expression x, int y)
        {
            Expression expression = MathNet.Symbolics.Operators.number.Invoke(y);
            return MathNet.Symbolics.Operators.add(x, MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, expression));
        }

        public static Expression operator -(double x, Expression y) => 
            MathNet.Symbolics.Operators.add(Values.unpack(ValueModule.real(x)), MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, y));

        public static Expression operator -(int x, Expression y) => 
            MathNet.Symbolics.Operators.add(MathNet.Symbolics.Operators.number.Invoke(x), MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, y));

        public static Expression operator -(Expression x) => 
            MathNet.Symbolics.Operators.multiply(MathNet.Symbolics.Operators.minusOne, x);

        public static Expression operator +(Expression x) => 
            x;

        public static Expression Pow(Expression x, Expression y) => 
            MathNet.Symbolics.Operators.pow(x, y);

        public static Expression Pow(Expression x, int y) => 
            MathNet.Symbolics.Operators.pow(x, MathNet.Symbolics.Operators.number.Invoke(y));

        public static Expression Real(double floatingPoint) => 
            Values.unpack(ValueModule.real(floatingPoint));

        public static Expression Root(Expression n, Expression x) => 
            NewPower(x, NewPower(n, MathNet.Symbolics.Operators.minusOne));

        public static Expression Sec(Expression x) => 
            NewFunction(MathNet.Symbolics.Function.Sec, x);

        public static Expression Sin(Expression x) => 
            MathNet.Symbolics.Operators.sin(x);

        public static Expression Sinh(Expression x) => 
            NewFunction(MathNet.Symbolics.Function.Sinh, x);

        public static Expression Sqrt(Expression x) => 
            NewPower(x, NewPower(MathNet.Symbolics.Operators.two, MathNet.Symbolics.Operators.minusOne));

        public static Expression Symbol(string name) => 
            NewIdentifier(MathNet.Symbolics.Symbol.NewSymbol(name));

        public static Expression Tan(Expression x) => 
            MathNet.Symbolics.Operators.tan(x);

        public static Expression Tanh(Expression x) => 
            NewFunction(MathNet.Symbolics.Function.Tanh, x);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Expression ComplexInfinity =>
            _unique_ComplexInfinity;

        public static Expression E =>
            NewConstant(MathNet.Symbolics.Constant.E);

        public static Expression I =>
            NewConstant(MathNet.Symbolics.Constant.I);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsApproximation =>
            (this.Tag == 1);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsComplexInfinity =>
            (this.Tag == 9);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsConstant =>
            (this.Tag == 3);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsFunction =>
            (this.Tag == 7);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsFunctionN =>
            (this.Tag == 8);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsIdentifier =>
            (this.Tag == 2);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsNegativeInfinity =>
            (this.Tag == 11);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsNumber =>
            (this.Tag == 0);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsPositiveInfinity =>
            (this.Tag == 10);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsPower =>
            (this.Tag == 6);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsProduct =>
            (this.Tag == 5);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsSum =>
            (this.Tag == 4);

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsUndefined =>
            (this.Tag == 12);

        public static Expression MinusOne =>
            MathNet.Symbolics.Operators.minusOne;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Expression NegativeInfinity =>
            _unique_NegativeInfinity;

        public static Expression One =>
            MathNet.Symbolics.Operators.one;

        public static Expression Pi =>
            MathNet.Symbolics.Operators.pi;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Expression PositiveInfinity =>
            _unique_PositiveInfinity;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int Tag =>
            this._tag;

        public static Expression Two =>
            MathNet.Symbolics.Operators.two;

        [CompilerGenerated, DebuggerNonUserCode, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Expression Undefined =>
            _unique_Undefined;

        public static Expression Zero =>
            MathNet.Symbolics.Operators.zero;

        [Serializable, DebuggerTypeProxy(typeof(Expression.Approximation@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Approximation : Expression
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly MathNet.Symbolics.Approximation item;

            [CompilerGenerated]
            internal Approximation(MathNet.Symbolics.Approximation item) : base(1)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 1, 0), CompilerGenerated]
            public MathNet.Symbolics.Approximation Item =>
                this.item;
        }

        internal class Approximation@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal Expression.Approximation _obj;

            [CompilerGenerated]
            public Approximation@DebugTypeProxy(Expression.Approximation obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 1, 0), CompilerGenerated]
            public MathNet.Symbolics.Approximation Item =>
                this._obj.item;
        }

        [Serializable, DebuggerTypeProxy(typeof(Expression.Constant@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Constant : Expression
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly MathNet.Symbolics.Constant item;

            [CompilerGenerated]
            internal Constant(MathNet.Symbolics.Constant item) : base(3)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 3, 0), CompilerGenerated]
            public MathNet.Symbolics.Constant Item =>
                this.item;
        }

        internal class Constant@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal Expression.Constant _obj;

            [CompilerGenerated]
            public Constant@DebugTypeProxy(Expression.Constant obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 3, 0), CompilerGenerated]
            public MathNet.Symbolics.Constant Item =>
                this._obj.item;
        }

        [Serializable, DebuggerTypeProxy(typeof(Expression.Function@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Function : Expression
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly MathNet.Symbolics.Function item1;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly Expression item2;

            [CompilerGenerated]
            internal Function(MathNet.Symbolics.Function item1, Expression item2) : base(7)
            {
                this.item1 = item1;
                this.item2 = item2;
            }

            [CompilationMapping(SourceConstructFlags.Field, 7, 0), CompilerGenerated]
            public MathNet.Symbolics.Function Item1 =>
                this.item1;

            [CompilationMapping(SourceConstructFlags.Field, 7, 1), CompilerGenerated]
            public Expression Item2 =>
                this.item2;
        }

        internal class Function@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal Expression.Function _obj;

            [CompilerGenerated]
            public Function@DebugTypeProxy(Expression.Function obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 7, 0), CompilerGenerated]
            public MathNet.Symbolics.Function Item1 =>
                this._obj.item1;

            [CompilationMapping(SourceConstructFlags.Field, 7, 1), CompilerGenerated]
            public Expression Item2 =>
                this._obj.item2;
        }

        [Serializable, DebuggerTypeProxy(typeof(Expression.FunctionN@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class FunctionN : Expression
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly MathNet.Symbolics.Function item1;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly FSharpList<Expression> item2;

            [CompilerGenerated]
            internal FunctionN(MathNet.Symbolics.Function item1, FSharpList<Expression> item2) : base(8)
            {
                this.item1 = item1;
                this.item2 = item2;
            }

            [CompilationMapping(SourceConstructFlags.Field, 8, 0), CompilerGenerated]
            public MathNet.Symbolics.Function Item1 =>
                this.item1;

            [CompilationMapping(SourceConstructFlags.Field, 8, 1), CompilerGenerated]
            public FSharpList<Expression> Item2 =>
                this.item2;
        }

        internal class FunctionN@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal Expression.FunctionN _obj;

            [CompilerGenerated]
            public FunctionN@DebugTypeProxy(Expression.FunctionN obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 8, 0), CompilerGenerated]
            public MathNet.Symbolics.Function Item1 =>
                this._obj.item1;

            [CompilationMapping(SourceConstructFlags.Field, 8, 1), CompilerGenerated]
            public FSharpList<Expression> Item2 =>
                this._obj.item2;
        }

        [Serializable, DebuggerTypeProxy(typeof(Expression.Identifier@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Identifier : Expression
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly Symbol item;

            [CompilerGenerated]
            internal Identifier(Symbol item) : base(2)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 2, 0), CompilerGenerated]
            public Symbol Item =>
                this.item;
        }

        internal class Identifier@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal Expression.Identifier _obj;

            [CompilerGenerated]
            public Identifier@DebugTypeProxy(Expression.Identifier obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 2, 0), CompilerGenerated]
            public Symbol Item =>
                this._obj.item;
        }

        [Serializable, DebuggerTypeProxy(typeof(Expression.Number@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Number : Expression
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly BigRational item;

            [CompilerGenerated]
            internal Number(BigRational item) : base(0)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 0, 0), CompilerGenerated]
            public BigRational Item =>
                this.item;
        }

        internal class Number@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal Expression.Number _obj;

            [CompilerGenerated]
            public Number@DebugTypeProxy(Expression.Number obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 0, 0), CompilerGenerated]
            public BigRational Item =>
                this._obj.item;
        }

        [Serializable, DebuggerTypeProxy(typeof(Expression.Power@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Power : Expression
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly Expression item1;
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly Expression item2;

            [CompilerGenerated]
            internal Power(Expression item1, Expression item2) : base(6)
            {
                this.item1 = item1;
                this.item2 = item2;
            }

            [CompilationMapping(SourceConstructFlags.Field, 6, 0), CompilerGenerated]
            public Expression Item1 =>
                this.item1;

            [CompilationMapping(SourceConstructFlags.Field, 6, 1), CompilerGenerated]
            public Expression Item2 =>
                this.item2;
        }

        internal class Power@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal Expression.Power _obj;

            [CompilerGenerated]
            public Power@DebugTypeProxy(Expression.Power obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 6, 0), CompilerGenerated]
            public Expression Item1 =>
                this._obj.item1;

            [CompilationMapping(SourceConstructFlags.Field, 6, 1), CompilerGenerated]
            public Expression Item2 =>
                this._obj.item2;
        }

        [Serializable, DebuggerTypeProxy(typeof(Expression.Product@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Product : Expression
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly FSharpList<Expression> item;

            [CompilerGenerated]
            internal Product(FSharpList<Expression> item) : base(5)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 5, 0), CompilerGenerated]
            public FSharpList<Expression> Item =>
                this.item;
        }

        internal class Product@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal Expression.Product _obj;

            [CompilerGenerated]
            public Product@DebugTypeProxy(Expression.Product obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 5, 0), CompilerGenerated]
            public FSharpList<Expression> Item =>
                this._obj.item;
        }

        [Serializable, DebuggerTypeProxy(typeof(Expression.Sum@DebugTypeProxy)), DebuggerDisplay("{__DebugDisplay(),nq}")]
        public class Sum : Expression
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal readonly FSharpList<Expression> item;

            [CompilerGenerated]
            internal Sum(FSharpList<Expression> item) : base(4)
            {
                this.item = item;
            }

            [CompilationMapping(SourceConstructFlags.Field, 4, 0), CompilerGenerated]
            public FSharpList<Expression> Item =>
                this.item;
        }

        internal class Sum@DebugTypeProxy
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never), CompilerGenerated]
            internal Expression.Sum _obj;

            [CompilerGenerated]
            public Sum@DebugTypeProxy(Expression.Sum obj)
            {
                this._obj = obj;
            }

            [CompilationMapping(SourceConstructFlags.Field, 4, 0), CompilerGenerated]
            public FSharpList<Expression> Item =>
                this._obj.item;
        }

        public static class Tags
        {
            public const int Approximation = 1;
            public const int ComplexInfinity = 9;
            public const int Constant = 3;
            public const int Function = 7;
            public const int FunctionN = 8;
            public const int Identifier = 2;
            public const int NegativeInfinity = 11;
            public const int Number = 0;
            public const int PositiveInfinity = 10;
            public const int Power = 6;
            public const int Product = 5;
            public const int Sum = 4;
            public const int Undefined = 12;
        }
    }
}

