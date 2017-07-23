namespace MathNet.Symbolics
{
    struct Symbol
    {
        string Under { get; set; }
        public static implicit operator string(Symbol s) => s.Under;
        public static implicit operator Symbol(string s) => new Symbol { Under = s };
    }

    enum Function
    {
        Abs,
        Ln, Exp
        , Sin, Cos, Tan
        , Cot, Sec, Csc
        , Cosh, Sinh, Tanh
        , Asin, Acos, Atan
    }

    enum Constant
    {

        E
        , Pi
        , I
    }
}