using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public struct NerdamerPart
        {
            public NerdamerPart(string Name, string FullName = null)
            {
                this.Name = Name;
                this.FullName = FullName ?? Name; 
                DescriptionContent = DescriptionTitle = null;
            }
            public NerdamerPart(string Name, string DescriptionTitle, 
                string DescriptionContent, string FullName = null)
            {
                this.Name = Name;
                this.FullName = FullName ?? Name;
                this.DescriptionTitle = DescriptionTitle;
                this.DescriptionContent = DescriptionContent;
            }

            public string Name { get; }
            public string DescriptionTitle { get; }
            public string DescriptionContent { get; }
            public string FullName { get; }

            public static readonly NerdamerPart Empty = new NerdamerPart(string.Empty);
            public static readonly NerdamerPart Space = new NerdamerPart(" ");
            public static readonly NerdamerPart Percent = new NerdamerPart("%");
            public static readonly NerdamerPart Comma = new NerdamerPart(",");
            public static readonly NerdamerPart LeftSquare = new NerdamerPart("[");
            public static readonly NerdamerPart RightSquare = new NerdamerPart("]");
            public static readonly NerdamerPart LeftRound = new NerdamerPart("(");
            public static readonly NerdamerPart RightRound = new NerdamerPart(")");
            public static readonly NerdamerPart D7 = new NerdamerPart("7");
            public static readonly NerdamerPart D8 = new NerdamerPart("8");
            public static readonly NerdamerPart D9 = new NerdamerPart("9");
            public static readonly NerdamerPart Exponent = new NerdamerPart("^");
            public static readonly NerdamerPart Factorial = new NerdamerPart("!");
            public static readonly NerdamerPart D4 = new NerdamerPart("4");
            public static readonly NerdamerPart D5 = new NerdamerPart("5");
            public static readonly NerdamerPart D6 = new NerdamerPart("6");
            public static readonly NerdamerPart Multiply = new NerdamerPart("*");
            public static readonly NerdamerPart Divide = new NerdamerPart("/");
            public static readonly NerdamerPart D1 = new NerdamerPart("1");
            public static readonly NerdamerPart D2 = new NerdamerPart("2");
            public static readonly NerdamerPart D3 = new NerdamerPart("3");
            public static readonly NerdamerPart Add = new NerdamerPart("+", "The addition operator", "Input:\n1+1\nOutput:\n2");
            public static readonly NerdamerPart Subtract = new NerdamerPart("-", "The subtraction operator", "Input:\n1-1\nOutput:\n0");
            public static readonly NerdamerPart D0 = new NerdamerPart("0");
            public static readonly NerdamerPart Decimal = new NerdamerPart(".");
            public static readonly NerdamerPart ConstPi = new NerdamerPart("pi");
            public static readonly NerdamerPart ConstE = new NerdamerPart("e");
            public static readonly NerdamerPart ConstI = new NerdamerPart("i");
            public static readonly NerdamerPart Equation = new NerdamerPart("=");
            public static readonly NerdamerPart Assign = new NerdamerPart(":=");
            public static readonly NerdamerPart Log = new NerdamerPart("log(");
            public static readonly NerdamerPart Log10 = new NerdamerPart("log10(");
            public static readonly NerdamerPart Min = new NerdamerPart("min(");
            public static readonly NerdamerPart Max = new NerdamerPart("max(");
            public static readonly NerdamerPart Sqrt = new NerdamerPart("sqrt(");
            public static readonly NerdamerPart Floor = new NerdamerPart("floor(");
            public static readonly NerdamerPart Ceil = new NerdamerPart("ceil(");
            public static readonly NerdamerPart Round = new NerdamerPart("round(");
            public static readonly NerdamerPart Trunc = new NerdamerPart("trunc(");
            public static readonly NerdamerPart Mod = new NerdamerPart("mod(");
            public static readonly NerdamerPart Gcd = new NerdamerPart("gcd(");
            public static readonly NerdamerPart Lcm = new NerdamerPart("lcm(");
            public static readonly NerdamerPart Mean = new NerdamerPart("mean(");
            public static readonly NerdamerPart Mode = new NerdamerPart("mode(");
            public static readonly NerdamerPart Median = new NerdamerPart("median(");
            public static readonly NerdamerPart Expand = new NerdamerPart("expand(");
            public static readonly NerdamerPart DivideFunc = new NerdamerPart("divide(");
            public static readonly NerdamerPart PFactor = new NerdamerPart("pfactor(");
            public static readonly NerdamerPart Fib = new NerdamerPart("fib(");
            public static readonly NerdamerPart Factor = new NerdamerPart("factor(");
            public static readonly NerdamerPart Roots = new NerdamerPart("roots(");
            public static readonly NerdamerPart Coeffs = new NerdamerPart("coeffs(");
            public static readonly NerdamerPart Solve = new NerdamerPart("solve(");
            public static readonly NerdamerPart SolveEquations = new NerdamerPart("solveEquations(");
            public static readonly NerdamerPart a = new NerdamerPart("a");
            public static readonly NerdamerPart b = new NerdamerPart("b");
            public static readonly NerdamerPart c = new NerdamerPart("c");
            public static readonly NerdamerPart d = new NerdamerPart("d");
            public static readonly NerdamerPart e = new NerdamerPart("e");
            public static readonly NerdamerPart f = new NerdamerPart("f");
            public static readonly NerdamerPart g = new NerdamerPart("g");
            public static readonly NerdamerPart h = new NerdamerPart("h");
            public static readonly NerdamerPart i = new NerdamerPart("i");
            public static readonly NerdamerPart j = new NerdamerPart("j");
            public static readonly NerdamerPart k = new NerdamerPart("k");
            public static readonly NerdamerPart l = new NerdamerPart("l");
            public static readonly NerdamerPart m = new NerdamerPart("m");
            public static readonly NerdamerPart n = new NerdamerPart("n");
            public static readonly NerdamerPart o = new NerdamerPart("o");
            public static readonly NerdamerPart p = new NerdamerPart("p");
            public static readonly NerdamerPart q = new NerdamerPart("q");
            public static readonly NerdamerPart r = new NerdamerPart("r");
            public static readonly NerdamerPart s = new NerdamerPart("s");
            public static readonly NerdamerPart t = new NerdamerPart("t");
            public static readonly NerdamerPart u = new NerdamerPart("u");
            public static readonly NerdamerPart v = new NerdamerPart("v");
            public static readonly NerdamerPart w = new NerdamerPart("w");
            public static readonly NerdamerPart x = new NerdamerPart("x");
            public static readonly NerdamerPart y = new NerdamerPart("y");
            public static readonly NerdamerPart z = new NerdamerPart("z");
            public static readonly NerdamerPart A = new NerdamerPart("A");
            public static readonly NerdamerPart B = new NerdamerPart("B");
            public static readonly NerdamerPart C = new NerdamerPart("C");
            public static readonly NerdamerPart D = new NerdamerPart("D");
            public static readonly NerdamerPart E = new NerdamerPart("E");
            public static readonly NerdamerPart F = new NerdamerPart("F");
            public static readonly NerdamerPart G = new NerdamerPart("G");
            public static readonly NerdamerPart H = new NerdamerPart("H");
            public static readonly NerdamerPart I = new NerdamerPart("I");
            public static readonly NerdamerPart J = new NerdamerPart("J");
            public static readonly NerdamerPart K = new NerdamerPart("K");
            public static readonly NerdamerPart L = new NerdamerPart("L");
            public static readonly NerdamerPart M = new NerdamerPart("M");
            public static readonly NerdamerPart N = new NerdamerPart("N");
            public static readonly NerdamerPart O = new NerdamerPart("O");
            public static readonly NerdamerPart P = new NerdamerPart("P");
            public static readonly NerdamerPart Q = new NerdamerPart("Q");
            public static readonly NerdamerPart R = new NerdamerPart("R");
            public static readonly NerdamerPart S = new NerdamerPart("S");
            public static readonly NerdamerPart T = new NerdamerPart("T");
            public static readonly NerdamerPart U = new NerdamerPart("U");
            public static readonly NerdamerPart V = new NerdamerPart("V");
            public static readonly NerdamerPart W = new NerdamerPart("W");
            public static readonly NerdamerPart X = new NerdamerPart("X");
            public static readonly NerdamerPart Y = new NerdamerPart("Y");
            public static readonly NerdamerPart Z = new NerdamerPart("Z");
            public static readonly NerdamerPart Atan2 = new NerdamerPart("atan2(");
            public static readonly NerdamerPart Sin = new NerdamerPart("sin(");
            public static readonly NerdamerPart Asin = new NerdamerPart("asin(");
            public static readonly NerdamerPart Sinh = new NerdamerPart("sinh(");
            public static readonly NerdamerPart Asinh = new NerdamerPart("asinh(");
            public static readonly NerdamerPart Cos = new NerdamerPart("cos(");
            public static readonly NerdamerPart Acos = new NerdamerPart("acos(");
            public static readonly NerdamerPart Cosh = new NerdamerPart("cosh(");
            public static readonly NerdamerPart Acosh = new NerdamerPart("acosh(");
            public static readonly NerdamerPart Tan = new NerdamerPart("tan(");
            public static readonly NerdamerPart Atan = new NerdamerPart("atan(");
            public static readonly NerdamerPart Tanh = new NerdamerPart("tanh(");
            public static readonly NerdamerPart Atanh = new NerdamerPart("atanh(");
            public static readonly NerdamerPart Sinc = new NerdamerPart("sinc(");
            public static readonly NerdamerPart Sum = new NerdamerPart("sum(");
            public static readonly NerdamerPart Product = new NerdamerPart("product(");
            public static readonly NerdamerPart Diff = new NerdamerPart("diff(");
            public static readonly NerdamerPart Integrate = new NerdamerPart("integrate(");
            public static readonly NerdamerPart Defint = new NerdamerPart("defint(");
            public static readonly NerdamerPart Step = new NerdamerPart("step(");
            public static readonly NerdamerPart Sec = new NerdamerPart("sec(");
            public static readonly NerdamerPart Asec = new NerdamerPart("asec(");
            public static readonly NerdamerPart Sech = new NerdamerPart("sech(");
            public static readonly NerdamerPart Asech = new NerdamerPart("asech(");
            public static readonly NerdamerPart Erf = new NerdamerPart("erf(");
            public static readonly NerdamerPart Csc = new NerdamerPart("csc(");
            public static readonly NerdamerPart Acsc = new NerdamerPart("acsc(");
            public static readonly NerdamerPart Csch = new NerdamerPart("csch(");
            public static readonly NerdamerPart Acsch = new NerdamerPart("acsch(");
            public static readonly NerdamerPart Rect = new NerdamerPart("rect(");
            public static readonly NerdamerPart Cot = new NerdamerPart("cot(");
            public static readonly NerdamerPart Acot = new NerdamerPart("acot(");
            public static readonly NerdamerPart Coth = new NerdamerPart("coth(");
            public static readonly NerdamerPart Acoth = new NerdamerPart("acoth(");
            public static readonly NerdamerPart Tri = new NerdamerPart("tri(");
            public static readonly NerdamerPart Si = new NerdamerPart("Si(");
            public static readonly NerdamerPart Ci = new NerdamerPart("Ci(");
            public static readonly NerdamerPart Shi = new NerdamerPart("Shi(");
            public static readonly NerdamerPart Chi = new NerdamerPart("Chi(");
            public static readonly NerdamerPart Ei = new NerdamerPart("Ei(");
            public static readonly NerdamerPart Laplace = new NerdamerPart("laplace(");
            public static readonly NerdamerPart Smpvar = new NerdamerPart("smpvar(");
            public static readonly NerdamerPart Variance = new NerdamerPart("variance(");
            public static readonly NerdamerPart Smpstdev = new NerdamerPart("smpstdev(");
            public static readonly NerdamerPart Stdev = new NerdamerPart("stdev(");
            public static readonly NerdamerPart NotEqual = new NerdamerPart("!=");
            public static readonly NerdamerPart LessThan = new NerdamerPart("<");
            public static readonly NerdamerPart LessEqual = new NerdamerPart("<=");
            public static readonly NerdamerPart Equal = new NerdamerPart("==");
            public static readonly NerdamerPart GreaterEqual = new NerdamerPart(">=");
            public static readonly NerdamerPart GreaterThan = new NerdamerPart(">");
            public static readonly NerdamerPart IMatrix = new NerdamerPart("imatrix(");
            public static readonly NerdamerPart Matrix = new NerdamerPart("matrix(");
            public static readonly NerdamerPart Matget = new NerdamerPart("matget(");
            public static readonly NerdamerPart Matset = new NerdamerPart("matset(");
            public static readonly NerdamerPart Invert = new NerdamerPart("invert(");
            public static readonly NerdamerPart Transpose = new NerdamerPart("transpose(");
            public static readonly NerdamerPart Vector = new NerdamerPart("vector(");
            public static readonly NerdamerPart Vecget = new NerdamerPart("vecget(");
            public static readonly NerdamerPart Vecset = new NerdamerPart("vecset(");
            public static readonly NerdamerPart Cross = new NerdamerPart("cross(");
            public static readonly NerdamerPart Dot = new NerdamerPart("dot(");

            public static readonly Regex Splitter =
                new Regex(string.Join("|",
                    typeof(NerdamerPart).GetTypeInfo()
                    .DeclaredFields
                    .Where(x => x.FieldType == typeof(NerdamerPart) && x.Name != nameof(Empty)) //No empty matches
                    .Select(x => Regex.Escape(((NerdamerPart)x.GetValue(null)).FullName)) //Assuming all fields of type NerdamerPart are static
                    .OrderByDescending(x => x.Length) //Regexes test from left to right; 
                    .Append(".")));                   //e.g. Matches("transpose((i))") => 
                    //No chars left out               //MatchCollection(5) { [transpose(], [(], [i], [)], [)] }
                    //e.g. Matches("transpose(1234河3守a化)") => 
                    //MatchCollection(11) { [transpose(], [1], [2], [3], [4], [河], [3], [守], [a], [化], [)] }
        }
    }
}