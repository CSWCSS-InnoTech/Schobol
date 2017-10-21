namespace InnoTecheLearnUtilities
{
    partial class Utils
    {
        public enum Expressions : byte
        {
            Space, Ans,
            D0, D1, D2, D3, D4, D5, D6, D7, D8, D9, DPoint, //Decimals
            Addition, Subtraction, Multiplication, Division, Modulus, Increment, Decrement, //Arithmetic
            LParenthese, RParenthese, LBracket, RBracket, LBrace, RBrace, //Parentheses
            BAnd, BLShift, BNot, BOr, BRShift, BXor, UnsignRShift,//Bitwise
            Less, Great, LessEqual, GreatEqual, Equal, NEqual, Identity, NIdentity, //Comparison
            LAnd, LNot, LOr, //Logical
            Abs, Acos, Asin, Atan, Atan2, Ceil, Cos, Exp, Floor, Log, //Math Functions
            Max, Min, Pow, Random, Round, Sin, Sqrt, Tan, Factorial, //Math Functions
            Acosh, Acot, Acoth, Acsc, Acsch, Asec, Asech, Asinh, Atanh, Cbrt, Cosh, Cot, Coth, //Additional Math Functions
            Csc, Csch, Clz32, Imul, Lb, Ln, Sec, Sech, Sign, Sinh, Tanh, Trunc, Deg, Rad, Grad, Turn, //Additional Math Functions
            nPr, nCr, GCD, HCF, LCM, //Additional Math Functions
            π, e, Root2, Root0_5, Ln2, Ln10, Log2e, Log10e, Infinity, NInfinity, NaN, Undefined, //Constants
            Comma, //Continuation
            Assign, AssignAdd, AssignSubtraction, AssignMultiplication, AssignDivision, AssignModulus, //Assignment
            AssignBAnd, AssignBOr, AssignBXor, AssignLShift, AssignRShift, AssignUnsignRShift, //Assignment
            A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, //Variables
        }
        public enum MoreExpressions : byte
        {
#region Expressions
            Space, Ans,
            D0, D1, D2, D3, D4, D5, D6, D7, D8, D9, DPoint, //Decimals
            Addition, Subtraction, Multiplication, Division, Modulus, Increment, Decrement, //Arithmetic
            LParenthese, RParenthese, LBracket, RBracket, LBrace, RBrace, //Parentheses
            BAnd, BLShift, BNot, BOr, BRShift, BXor, UnsignRShift,//Bitwise
            Less, Great, LessEqual, GreatEqual, Equal, NEqual, Identity, NIdentity, //Comparison
            LAnd, LNot, LOr, //Logical
            Abs, Acos, Asin, Atan, Atan2, Ceil, Cos, Exp, Floor, Log, //Math Functions
            Max, Min, Pow, Random, Round, Sin, Sqrt, Tan, Factorial, //Math Functions
            Acosh, Acot, Acoth, Acsc, Acsch, Asec, Asech, Asinh, Atanh, Cbrt, Cosh, Cot, Coth, //Additional Math Functions
            Csc, Csch, Clz32, Imul, Lb, Ln, Sec, Sech, Sign, Sinh, Tanh, Trunc, Deg, Rad, Grad, Turn, //Additional Math Functions
            nPr, nCr, GCD, HCF, LCM, //Additional Math Functions
            π, e, Root2, Root0_5, Ln2, Ln10, Log2e, Log10e, Infinity, NInfinity, NaN, Undefined, //Constants
            Comma, //Continuation
            Assign, AssignAdd, AssignSubtraction, AssignMultiplication, AssignDivision, AssignModulus, //Assignment
            AssignBAnd, AssignBOr, AssignBXor, AssignLShift, AssignRShift, AssignUnsignRShift, //Assignment
            A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, //Variables
            #endregion Hello this is a comment
            InstanceOf, New, Reference, TypeOf, Void, //Types
            Ternary1, Ternary2, Separator, If, Else, Switch, //Control
            Class, Constant, Delete, Enum, Function,//Declaration
            FunctionGet, FunctionSet, Interface, Return, Static, Var,//Declaration
            Comment, CommentStart, CommentEnd, //Comments
            Break, Continue, Do, While, For, In, //Loops
            Super, This, /*References*/ Throw, Try, Catch, Finally, //Exceptions
            Debugger, Import, Package, Print, With //Miscellaneous
        }

        public static void RemoveLast<T>(this System.Collections.Generic.IList<T> List)
        { if(List.Count != 0) List.RemoveAt(List.Count - 1); }
        public static void InsertItemLocation
            (this System.Collections.Generic.IList<Expressions> Expression, int Index, Expressions Item)
        {
            Expression.Insert(ToItemLocation(Expression, Index), Item);
        }
        public static void InsertItemLocation
            (this System.Collections.Generic.IList<MoreExpressions> Expression, int Index, MoreExpressions Item)
        {
            Expression.Insert(ToItemLocation(Expression, Index), Item);
        }
        public static void RemoveItemLocation(this System.Collections.Generic.IList<Expressions> Expression, int Index)
        {
            Expression.RemoveAt(ToItemLocation(Expression, Index));
        }
        public static void RemoveItemLocation(this System.Collections.Generic.IList<MoreExpressions> Expression, int Index)
        {
            Expression.RemoveAt(ToItemLocation(Expression, Index));
        }
        public static int ToItemLocation(this System.Collections.Generic.IList<Expressions> Expression, int StringIndex)
        {
            for (int i = 0, j = 0; i < Expression.Count; i++)
            {
                j += Expression[i].StringLength();
                if (j > StringIndex) return i;
            }
            return -1;
        }
        public static int ToItemLocation(this System.Collections.Generic.IList<MoreExpressions> Expression, int StringIndex)
        {
            for (int i = 0, j = 0; i < Expression.Count; i++)
            {
                j += Expression[i].StringLength();
                if (j > StringIndex) return i;
            }
            return -1;
        }
        public static int ToStringLocation(this System.Collections.Generic.IList<Expressions> Expression, int ItemIndex)
        {
            if (ItemIndex > Expression.Count) return -1;
            int j = 0;
            for (int i = 0; i < ItemIndex; i++) j += Expression[i].StringLength();
            return j;
        }
        public static int ToStringLocation(this System.Collections.Generic.IList<MoreExpressions> Expression, int ItemIndex)
        {
            if (ItemIndex > Expression.Count) return -1;
            int j = 0;
            for (int i = 0; i < ItemIndex; i++) j += Expression[i].StringLength();
            return j;
        }
        public static int StringLength(this Expressions Expression)
        {
            return AsString(Expression).Length;
        }
        public static int StringLength(this MoreExpressions Expression)
        {
            return AsString(Expression).Length;
        }
        public static int StringLength(this System.Collections.Generic.IList<Expressions> Expression)
        {
            return AsString(Expression).Length;
        }
        public static int StringLength(this System.Collections.Generic.IList<MoreExpressions> Expression)
        {
            return AsString(Expression).Length;
        }
        public static string AsString(this System.Collections.Generic.IList<Expressions> Expression)
        {
            string Return = "";
            foreach (Expressions Item in Expression)
                Return += AsString(Item);
            return Return;
        }
        public static string AsString(this System.Collections.Generic.IList<MoreExpressions> Expression)
        {
            string Return = "";
            foreach (MoreExpressions Item in Expression)
                Return += AsString(Item);
            return Return;
        }
        public static string AsString(this Expressions Expression)
        { return AsString((MoreExpressions)Expression); }
        public static string AsString(this MoreExpressions Expression)
        {
            switch (Expression)
            {
                case MoreExpressions.Space:
                    return " ";
                case MoreExpressions.Ans:
                    return "Ans";
                case MoreExpressions.D0:
                    return "0";
                case MoreExpressions.D1:
                    return "1";
                case MoreExpressions.D2:
                    return "2";
                case MoreExpressions.D3:
                    return "3";
                case MoreExpressions.D4:
                    return "4";
                case MoreExpressions.D5:
                    return "5";
                case MoreExpressions.D6:
                    return "6";
                case MoreExpressions.D7:
                    return "7";
                case MoreExpressions.D8:
                    return "8";
                case MoreExpressions.D9:
                    return "9";
                case MoreExpressions.DPoint:
                    return ".";
                case MoreExpressions.Addition:
                    return "+";
                case MoreExpressions.Subtraction:
                    return "-";
                case MoreExpressions.Multiplication:
                    return "*";
                case MoreExpressions.Division:
                    return "/";
                case MoreExpressions.Modulus:
                    return "%";
                case MoreExpressions.Increment:
                    return "++";
                case MoreExpressions.Decrement:
                    return "--";
                case MoreExpressions.LParenthese:
                    return "(";
                case MoreExpressions.RParenthese:
                    return ")";
                case MoreExpressions.LBracket:
                    return "[";
                case MoreExpressions.RBracket:
                    return "]";
                case MoreExpressions.LBrace:
                    return "{";
                case MoreExpressions.RBrace:
                    return "}";
                case MoreExpressions.BAnd:
                    return "&";
                case MoreExpressions.BLShift:
                    return "<<";
                case MoreExpressions.BNot:
                    return "~";
                case MoreExpressions.BOr:
                    return "|";
                case MoreExpressions.BRShift:
                    return ">>";
                case MoreExpressions.BXor:
                    return "^";
                case MoreExpressions.UnsignRShift:
                    return ">>>";
                case MoreExpressions.Less:
                    return "<";
                case MoreExpressions.Great:
                    return ">";
                case MoreExpressions.LessEqual:
                    return "<=";
                case MoreExpressions.GreatEqual:
                    return ">=";
                case MoreExpressions.Equal:
                    return "==";
                case MoreExpressions.NEqual:
                    return "!=";
                case MoreExpressions.Identity:
                    return "===";
                case MoreExpressions.NIdentity:
                    return "!==";
                case MoreExpressions.LAnd:
                    return "&&";
                case MoreExpressions.LNot:
                    return "!";
                case MoreExpressions.LOr:
                    return "||";
                case MoreExpressions.Abs:
                    return "Abs(";
                case MoreExpressions.Acos:
                    return "Acos(";
                case MoreExpressions.Asin:
                    return "Asin(";
                case MoreExpressions.Atan:
                    return "Atan(";
                case MoreExpressions.Atan2:
                    return "Atan2(";
                case MoreExpressions.Ceil:
                    return "Ceil(";
                case MoreExpressions.Cos:
                    return "Cos(";
                case MoreExpressions.Exp:
                    return "Exp(";
                case MoreExpressions.Floor:
                    return "Floor(";
                case MoreExpressions.Log:
                    return "Log(";
                case MoreExpressions.Max:
                    return "Max(";
                case MoreExpressions.Min:
                    return "Min(";
                case MoreExpressions.Pow:
                    return "Pow(";
                case MoreExpressions.Random:
                    return "Random()";
                case MoreExpressions.Round:
                    return "Round(";
                case MoreExpressions.Sin:
                    return "Sin(";
                case MoreExpressions.Sqrt:
                    return "Sqrt(";
                case MoreExpressions.Tan:
                    return "Tan(";
                case MoreExpressions.Factorial:
                    return "Factorial(";
                case MoreExpressions.Acosh:
                    return "Acosh(";
                case MoreExpressions.Acot:
                    return "Acot(";
                case MoreExpressions.Acoth:
                    return "Acoth(";
                case MoreExpressions.Acsc:
                    return "Acsc(";
                case MoreExpressions.Acsch:
                    return "Acsch(";
                case MoreExpressions.Asec:
                    return "Asec(";
                case MoreExpressions.Asech:
                    return "Asech(";
                case MoreExpressions.Asinh:
                    return "Asinh(";
                case MoreExpressions.Atanh:
                    return "Atanh(";
                case MoreExpressions.Cbrt:
                    return "Cbrt(";
                case MoreExpressions.Cosh:
                    return "Cosh(";
                case MoreExpressions.Cot:
                    return "Cot(";
                case MoreExpressions.Coth:
                    return "Coth(";
                case MoreExpressions.Csc:
                    return "Csc(";
                case MoreExpressions.Csch:
                    return "Csch(";
                case MoreExpressions.Clz32:
                    return "Clz32(";
                case MoreExpressions.Imul:
                    return "Imul(";
                case MoreExpressions.Lb:
                    return "Lb(";
                case MoreExpressions.Ln:
                    return "Ln(";
                case MoreExpressions.Sec:
                    return "Sec(";
                case MoreExpressions.Sech:
                    return "Sech(";
                case MoreExpressions.Sign:
                    return "Sign(";
                case MoreExpressions.Sinh:
                    return "Sinh(";
                case MoreExpressions.Tanh:
                    return "Tanh(";
                case MoreExpressions.Trunc:
                    return "Trunc(";
                case MoreExpressions.Deg:
                    return "Deg(";
                case MoreExpressions.Rad:
                    return "Rad(";
                case MoreExpressions.Grad:
                    return "Grad(";
                case MoreExpressions.Turn:
                    return "Turn(";
                case MoreExpressions.nPr:
                    return "nPr(";
                case MoreExpressions.nCr:
                    return "nCr(";
                case MoreExpressions.GCD:
                    return "GCD(";
                case MoreExpressions.HCF:
                    return "HCF(";
                case MoreExpressions.LCM:
                    return "LCM(";
                case MoreExpressions.π:
                    return "π";
                case MoreExpressions.e:
                    return "e";
                case MoreExpressions.Root2:
                    return "Root2";
                case MoreExpressions.Root0_5:
                    return "Root0_5";
                case MoreExpressions.Ln2:
                    return "Ln2";
                case MoreExpressions.Ln10:
                    return "Ln10";
                case MoreExpressions.Log2e:
                    return "Log2e";
                case MoreExpressions.Log10e:
                    return "Log10e";
                case MoreExpressions.Infinity:
                    return "Infinity";
                case MoreExpressions.NInfinity:
                    return "-Infinity";
                case MoreExpressions.NaN:
                    return "NaN";
                case MoreExpressions.Undefined:
                    return "undefined";
                case MoreExpressions.Comma:
                    return ",";
                case MoreExpressions.Assign:
                    return "=";
                case MoreExpressions.AssignAdd:
                    return "+=";
                case MoreExpressions.AssignBAnd:
                    return "&=";
                case MoreExpressions.AssignBOr:
                    return "|=";
                case MoreExpressions.AssignBXor:
                    return "^=";
                case MoreExpressions.AssignDivision:
                    return "/=";
                case MoreExpressions.AssignLShift:
                    return "<<=";
                case MoreExpressions.AssignModulus:
                    return "%=";
                case MoreExpressions.AssignMultiplication:
                    return "*=";
                case MoreExpressions.AssignRShift:
                    return ">>=";
                case MoreExpressions.AssignSubtraction:
                    return "-=";
                case MoreExpressions.AssignUnsignRShift:
                    return ">>>=";
                case MoreExpressions.A:
                    return "A";
                case MoreExpressions.B:
                    return "B";
                case MoreExpressions.C:
                    return "C";
                case MoreExpressions.D:
                    return "D";
                case MoreExpressions.E:
                    return "E";
                case MoreExpressions.F:
                    return "F";
                case MoreExpressions.G:
                    return "G";
                case MoreExpressions.H:
                    return "H";
                case MoreExpressions.I:
                    return "I";
                case MoreExpressions.J:
                    return "J";
                case MoreExpressions.K:
                    return "K";
                case MoreExpressions.L:
                    return "L";
                case MoreExpressions.M:
                    return "M";
                case MoreExpressions.N:
                    return "N";
                case MoreExpressions.O:
                    return "O";
                case MoreExpressions.P:
                    return "P";
                case MoreExpressions.Q:
                    return "Q";
                case MoreExpressions.R:
                    return "R";
                case MoreExpressions.S:
                    return "S";
                case MoreExpressions.T:
                    return "T";
                case MoreExpressions.U:
                    return "U";
                case MoreExpressions.V:
                    return "V";
                case MoreExpressions.W:
                    return "W";
                case MoreExpressions.X:
                    return "X";
                case MoreExpressions.Y:
                    return "Y";
                case MoreExpressions.Z:
                    return "Z";
                //Expressions - MoreExpressions Line
                case MoreExpressions.InstanceOf:
                    return "instanceof(";
                case MoreExpressions.New:
                    return "new";
                case MoreExpressions.Reference:
                    return "&";
                case MoreExpressions.TypeOf:
                    return "typeof(";
                case MoreExpressions.Void:
                    return "void";
                case MoreExpressions.Ternary1:
                    return "?";
                case MoreExpressions.Ternary2:
                    return ":";
                case MoreExpressions.Separator:
                    return ";";
                case MoreExpressions.If:
                    return "if(";
                case MoreExpressions.Else:
                    return "else";
                case MoreExpressions.Switch:
                    return "switch(";
                case MoreExpressions.Class:
                    return "class";
                case MoreExpressions.Constant:
                    return "const";
                case MoreExpressions.Delete:
                    return "delete";
                case MoreExpressions.Enum:
                    return "enum";
                case MoreExpressions.Function:
                    return "function";
                case MoreExpressions.FunctionGet:
                    return "function get";
                case MoreExpressions.FunctionSet:
                    return "function set";
                case MoreExpressions.Interface:
                    return "interface";
                case MoreExpressions.Return:
                    return "return";
                case MoreExpressions.Static:
                    return "static";
                case MoreExpressions.Var:
                    return "var";
                case MoreExpressions.Comment:
                    return "//";
                case MoreExpressions.CommentStart:
                    return "/*";
                case MoreExpressions.CommentEnd:
                    return "*/";
                case MoreExpressions.Break:
                    return "break";
                case MoreExpressions.Continue:
                    return "continue";
                case MoreExpressions.Do:
                    return "do";
                case MoreExpressions.While:
                    return "while(";
                case MoreExpressions.For:
                    return "for(";
                case MoreExpressions.In:
                    return "in";
                case MoreExpressions.Super:
                    return "super";
                case MoreExpressions.This:
                    return "this";
                case MoreExpressions.Throw:
                    return "throw";
                case MoreExpressions.Try:
                    return "try";
                case MoreExpressions.Catch:
                    return "catch";
                case MoreExpressions.Finally:
                    return "finally";
                case MoreExpressions.Debugger:
                    return "debugger";
                case MoreExpressions.Import:
                    return "import";
                case MoreExpressions.Package:
                    return "package";
                case MoreExpressions.Print:
                    return "print(";
                case MoreExpressions.With:
                    return "with(";
                default:
                    throw new System.ArgumentOutOfRangeException("Expression",
                        Expression, "Expression is not a valid MoreExpression object.");
            }
        }
    }
}
