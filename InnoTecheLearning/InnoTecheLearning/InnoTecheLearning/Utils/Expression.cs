namespace InnoTecheLearning
{
    partial class Utils
    {
        public enum Expressions : byte
        {
            Space,
            D0, D1, D2, D3, D4, D5, D6, D7, D8, D9, DPoint, //Decimals
            Addition, Subtraction, Multiplication, Division, Modulus, Increment, Decrement, //Arithmetic
            LParenthese, RParenthese, LBracket, RBracket, LBrace, RBrace, //Parentheses
            BAnd, BLShift, BNot, BOr, BRShift, BXor, UnsignRShift,//Bitwise
            Less, Great, LessEqual, GreatEqual, Equal, NEqual, Identity, NIdentity, //Comparison
            LAnd, LNot, LOr, //Logical
            Abs, Acos, Asin, Atan, Atan2, Ceil, Cos, Exp, Floor, Log, //Math Functions
            Max, Min, Pow, Random, Round, Sin, Sqrt, Tan, Factorial, //Math Functions
            π, e, Root2, Root0_5, Ln2, Ln10, Log2e, Log10e //Constants
        }
        public enum MoreExpressions : byte
        {
            #region Expressions
            Space,
            D0, D1, D2, D3, D4, D5, D6, D7, D8, D9, DPoint, //Decimals
            Addition, Subtraction, Multiplication, Division, Modulus, Increment, Decrement, //Arithmetic
            LParenthese, RParenthese, LBracket, RBracket, LBrace, RBrace, //Parentheses
            BAnd, BLShift, BNot, BOr, BRShift, BXor, UnsignRShift,//Bitwise
            Less, Great, LessEqual, GreatEqual, Equal, NEqual, Identity, NIdentity, //Comparison
            LAnd, LNot, LOr, //Logical
            Abs, Acos, Asin, Atan, Atan2, Ceil, Cos, Exp, Floor, Log, //Math Functions
            Max, Min, Pow, Random, Round, Sin, Sqrt, Tan, Factorial, //Math Functions
            π, e, Root2, Root0_5, Ln2, Ln10, Log2e, Log10e //Constants
            , //Continuation
            #endregion Hello this is a comment
            Assign, AssignAdd, AssignBAnd, AssignBOr, AssignBXor, //Assignment
            AssignDivision, AssignLShift, AssignModulus, AssignMultiplication, //Assignment
            AssignRShift, AssignSubtraction, AssignUnsignRShift, //Assignment
            InstanceOf, New, Reference, TypeOf, Void, //Types
            Comma, Ternary1, Ternary2, Separator, If, Else, Switch, //Control
            Class, Constant, Delete, Enum, Function,//Declaration
            FunctionGet, FunctionSet, Interface, Return, Static, Var,//Declaration
            Comment, CommentStart, CommentEnd, //Comments
            Break, Continue, Do, While, For, In, //Loops
            Super, This, /*References*/ Throw, Try, Catch, Finally, //Exceptions
            Debugger, Import, Package, Print, With //Miscellaneous
        }
        public static string ToString(Expressions Expression)
        { return ToString((MoreExpressions)Expression); }
        public static string ToString(MoreExpressions Expression)
        {
            switch (Expression)
            {
                case MoreExpressions.Space:
                    return " ";
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
                    return "Atan2";
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
                //Expressions - MoreExpressions Line
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
                case MoreExpressions.Comma:
                    return ",";
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
