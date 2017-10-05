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
            const string Tab = "    ";
            const string NewLine = "\r\n";
            const char OptionalChar = '\x2060';
            const char Separator = ':';
            //(string 
            public NerdamerPart(string Name, string Friendly = null) : this(Name, null, null, null, null, null, Friendly) { }
            public NerdamerPart(string Name, string Spoken, string Description, string UsageOverride,
                IEnumerable<(string Var, string Description)> Vars, IEnumerable<string> Examples, string Friendly = null, bool Evaluate = true)
            {
                this.Friendly = Friendly ?? Name;
                this.Name = Name;
                this.DescriptionTitle = 
                    $"{Name} {"(" + new[] { (Friendly + "(" == Name ? null : Friendly), Spoken }.Aggregate((prev, current) => string.IsNullOrEmpty(prev) ? current : string.IsNullOrEmpty(current) ? prev : $"{prev}, {current}") + ")"}";
                try                                                   //new[]{"", "2", "3", ""} => "(2, 3)"
                {
                    this.DescriptionContent = 
                    $@"{Description}{NewLine}{NewLine}Usage:{NewLine}{Tab}{UsageOverride ?? (Vars.Aggregate(Name, (prev, current) => current.Var.ElementAtOrDefault(0) == OptionalChar ? $"{prev}){(prev.Contains(NewLine) ? prev.Substring(prev.LastIndexOf(NewLine) + 1) : NewLine + Tab + prev)}{(prev.EndsWith("(") ? string.Empty : ", ")}{current.Var.TrimStart(OptionalChar)}" : $"{prev}{(prev.EndsWith("(") ? string.Empty : ", ")}{current.Var}")) + ")"}{NewLine}Parameters:{Vars?.Aggregate("", (prev, current) => $"{prev}{NewLine}{Tab}{current.Var}{Separator} {current.Description}") ?? $"{NewLine}{Tab}None"}{NewLine}Examples:{Examples.Aggregate("", (prev, current) => $"{prev}{NewLine}{Tab}{current}{Separator} {SymbolicsEngine.Current.RunSynchronously().Evaluate($"nerdamer('{current}'){(Evaluate ? ".evaluate()" : string.Empty)}.text()").RunSynchronously()}")}";
                }
                catch (System.ArgumentNullException)
                {
                    this.DescriptionContent = null;
                }
            }

            public static NerdamerPart FromOperator(string Name, string Spoken, bool Prefix, bool Postfix, string Description, IEnumerable<string> Examples, string UsageOverride = null, string Friendly = null, bool Evaluate = true) =>
                new NerdamerPart(Name, $"The {Spoken} operator", Description, UsageOverride ?? $"{(Postfix ? "x" : string.Empty)}{Name}{(Prefix ? "y" : string.Empty)}",
                    new[] { Postfix ? ("x", "The left hand side expression") : default, Prefix ? ("y", "The right hand side expression") : default }.Where(x => !x.Equals(default)), Examples, Friendly, Evaluate);
            public static NerdamerPart FromLiteral(string Name, string Spoken, string Description, string Friendly = null, IEnumerable<string> Examples = null, bool Evaluate = true) =>
                new NerdamerPart(Name, Spoken, Description, Name, null, Examples ?? new[] { Name, "3" + Name, Name + "/2" }, Friendly, Evaluate);
            public static NerdamerPart FromDigit(char Digit) => FromLiteral(Digit.ToString(), $"The digit {Digit}", $"Denotes the digit with the value {Digit}.");
            public static NerdamerPart FromFunction(string Name, string Spoken, string Description,
                IEnumerable<(string Var, string Description)> Vars, IEnumerable<string> Examples, string UsageOverride = null, string Friendly = null, bool Evaluate = true) =>
                new NerdamerPart(Name + '(', $"The {Spoken} function", Description, null, Vars, Examples, Friendly ?? Name, Evaluate);
            public static string Optional(string VarName) => OptionalChar + VarName;

            public static void Init() => Equals(Empty, Space);

            public string Name { get; }
            public string DescriptionTitle { get; }
            public string DescriptionContent { get; }
            public string Friendly { get; }

            /*
        0   1   2   3   4   5   6   7   8   9   A   B   C   D   E   F
U+00Bx          x²  x³                      x¹                      
U+207x  x⁰  xⁱ          x⁴  x⁵  x⁶  x⁷  x⁸  x⁹  x⁺  x⁻  x⁼  x⁽  x⁾  xⁿ
U+208x  x₀  x₁  x₂  x₃  x₄  x₅  x₆  x₇  x₈  x₉  x₊  x₋  x₌  x₍  x₎  
U+209x  xₐ  xₑ  xₒ  xₓ  xₔ  xₕ  xₖ   xₗ  xₘ  xₙ   xₚ  xₛ  xₜ
ªºⱽⱼ◌ͣ◌ͤ◌ͥ◌ͦ◌ͧ◌ͨ◌ͩ◌ͪ◌ͫ◌ͬ◌ͭ◌ͮ◌ͯ ʰʲʳʷʸˡˢˣᴬᴮᴰᴱᴳᴴᴵᴶᴷᴸᴹᴺᴼᴾᴿᵀᵁᵂᵃᵅᵇᵈᵉᵍᵏᵐᵒᵖᵗᵘᵛᵢᵣᵤᵥᵪᵸᶜᶠᶢᶦᶩᶫᶰᶴᶸᶻ*/
            public static readonly NerdamerPart Empty = new NerdamerPart(string.Empty);
            public static readonly NerdamerPart Space = FromLiteral(" ", "The space bar", "Used for beautifying an expression or denotes implicit multiplication.", "␣", new[] { "3 2", "a     b", "vw x  3   y    z"});
            public static readonly NerdamerPart Percent = FromOperator("%", "percentage", false, true, "Used for denoting percentages, which is equal to dividing by 100.", new[] { "1%", "a%", "1.3%%" });
            public static readonly NerdamerPart Comma = FromOperator(",", "comma", true, true, "Separates a list of values in an argument list or vector.", new[] { "min(2,3,4,5)", "[a,b,4,d]" });
            public static readonly NerdamerPart LeftSquare = new NerdamerPart("[", "The left square bracket", "Denotes the opening of a vector (sequence of expressions).", "[a,b,c,d...]", new[] { ("a,b,c,d...", "Any number of expressions, separated by the comma.")}, new[] { "[1]", "[1,2,3,4]", "[4,3,2]+[2,3,4]", "[7,7,7]/[6,8,9]" });
            public static readonly NerdamerPart RightSquare = new NerdamerPart("]", "The right square bracket", "Denotes the closing of a vector (sequence of expressions).", "[a,b,c,d...]", new[] { ("a,b,c,d...", "Any number of expressions, separated by the comma.") }, new[] { "[a]", "[a,1,c,56]", "[c,s,w]-[c,s,s]", "[h,o,n,g]*[k,o,n,g]" });
            public static readonly NerdamerPart LeftRound = new NerdamerPart("(", "The left round bracket", "Denotes implicit multiplication, the opening of an expression that should be evaluated first, or an argument list to be passed into a function.", $"(e){NewLine}{Tab}f(a,b,c,d...)", new[] { ("e", "Any expression."), ("a,b,c,d...", "Any number of expressions, separated by the comma."), ("f", "Any function that accepts argument lists this long.") }, new[] { "(1)", "(4+5)*7", "(54)(32)", "gcd(1,2)" });
            public static readonly NerdamerPart RightRound = new NerdamerPart(")", "The right round bracket", "Denotes implicit multiplication, the closing of an expression that should be evaluated first, or an argument list to be passed into a function.", $"(e){NewLine}{Tab}f(a,b,c,d...)", new[] { ("e", "Any expression."), ("a,b,c,d...", "Any number of expressions, separated by the comma."), ("f", "Any function that accepts argument lists this long.") }, new[] { "(a)", "(((((((7)))))))", "(c)(d)", "gcd(x,y)" });
            public static readonly NerdamerPart D7 = FromDigit('7');
            public static readonly NerdamerPart D8 = FromDigit('8');
            public static readonly NerdamerPart D9 = FromDigit('9');
            public static readonly NerdamerPart Exponent = FromOperator("^", "exponentiation", true, true, "Raises the left expression to the right-expressionth power. It is right-associative, which means that it is calculated right-to-left.", new[] { "2^3", "(6/5)^3", "5^a*5^b", "x^c/x^d" });
            public static readonly NerdamerPart Factorial = FromOperator("!", "factorial", false, true, "Calculates the factorial (product of all the integers less than or equal to the left expression if it is an integer) or the double factorial (NOT twice the factorial, but product of all the odd integers less than or equal to the left expression if it is an odd integer, and vice versa for even integers).", new[] { "5!", "0!", "(-5)!", "5.3!", "6!!", "(-6)!!"});
            public static readonly NerdamerPart D4 = FromDigit('4');
            public static readonly NerdamerPart D5 = FromDigit('5');
            public static readonly NerdamerPart D6 = FromDigit('6');
            public static readonly NerdamerPart Multiply = FromOperator("*", "multiplication", true, true, "Multiplies two expressions together.", new[] { "2*4*6", "9*55*34", "a*b*7*(5+7)" }, null, "×"); //"∙"
            public static readonly NerdamerPart Divide = FromOperator("/", "division", true, true, "Divides two expressions from each other, or denotes a fraction. For algebraic polynomial long division, use the divide function.", new[] { "7/3", "7/3/4/5", "a/b/c/d", "a/(5+x)" }, null, "÷"); //"⁄"
            public static readonly NerdamerPart D1 = FromDigit('1');
            public static readonly NerdamerPart D2 = FromDigit('2');
            public static readonly NerdamerPart D3 = FromDigit('3');
            public static readonly NerdamerPart Add = FromOperator("+", "addition", true, true, "Adds two expressions together, or denotes a positive expression.", new[] { "1+1", "+4", "5+j+a" }, $"x+y\n{Tab}+y");
            public static readonly NerdamerPart Subtract = FromOperator("-", "subtraction", true, true, "Subtracts two expressions from each other, or denotes a negative expression.", new[] { "1-1", "-4", "-5-j-a" }, $"x-y\n{Tab}-y");
            public static readonly NerdamerPart D0 = FromDigit('0');
            public static readonly NerdamerPart Decimal = new NerdamerPart(".", "The decimal dot", "Separates a number into an integer part and a decimal part.", "a.b", new[] { ("a", "Any combination of 0, 1, 2, 3, 4, 5, 6, 7, 8 and 9."), ("b", "Any combination of 0, 1, 2, 3, 4, 5, 6, 7, 8 and 9.") }, new[] { "1.92", "1.2222222222222", "22222222222222.1", "00012345.6789000" });
            public static readonly NerdamerPart ConstPi = FromLiteral("π", "The mathematical constant pi", "Denotes the length of the circumference of a circle with diameter 1 ≈ 3.14159265358979323846.");
            public static readonly NerdamerPart ConstE = new NerdamerPart("e", "The mathematical constant e / Scientific E-notation", "Denotes the limit of (1 + 1/n)ⁿ as n approaches infinity ≈ 2.71828182845904523536, or denotes the previous number multiplied by ten raised to the following power.", $"e{NewLine}{Tab}nex", new[] { ("n", "Any combination of 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, and the decimal dot (.)."), ("x", "Any combination of 0, 1, 2, 3, 4, 5, 6, 7, 8 and 9.") }, new[] { "e", "3e", "3e3", "34.5e99", "1e1-1e0" });
            public static readonly NerdamerPart ConstI = FromLiteral("i", "The mathematical constant i", "Denotes the imaginary unit = √" + Overline("-1") + ".");
            public static readonly NerdamerPart Degree = FromOperator("°", "degrees", false, true, "Converts an expression in degrees to radians.", new[] { "0°", "1°", "6.4°", "90°", "180°" });
            public static readonly NerdamerPart Equation = FromOperator("=", "equality", true, true, "Denotes that the left and right expressions are equal, and forms an equation. Can be solved using the solve function or the solveEquations function.", new[] { "8=8", "2x=3", "solve(2x=3,x)", "solveEquations([x+y=4,2x+3y=5])" }, Evaluate: false);
            //public static readonly NerdamerPart Assign = new NerdamerPart(":=");
            public static readonly NerdamerPart Log = FromFunction("log", "logarithm", "Calculates the logarithm (a=b^x; b=a^(1/x); x=log(a,b)) with an optional base (defaults to the constant e).", new[] { ("a", "The expression to calculate the logarithm for."), (Optional("b"), "The optional base in which when multiplied by the result, will produce the original expression. Defaults to the constant e.") }, new string[] { "log(9, 3)", "log(e^3)", "log(a^x,a)", "log(2^log(2^3,2),2)" });
            public static readonly NerdamerPart Log10 = FromFunction("log10", "base 10 logarithm", "A convenient shorthand for log(x, 10).", new[] { ("x", "The expression to calculate the base 10 logarithm for.") }, new[] { "log10(10)", "log10(100)", "log10(1e308)" }, null, "log₁₀");
            public static readonly NerdamerPart Min = FromFunction("min", "minimum", "Calculates the minimum value from a set of expressions.", new[] { ("a,b,c,d...", "Any number of expressions, separated by the comma.") }, new[] { "min(1,2,3,π,e)", "min(π,2^2,3.5,10/3)" });
            public static readonly NerdamerPart Max = FromFunction("max", "maximum", "Calculates the maximum value from a set of expressions.", new[] { ("a,b,c,d...", "Any number of expressions, separated by the comma.") }, new[] { "max(sqrt(π)+1.5,e^(11/9),10/3,π)", "max(10, 100/10, log10(10^10), sqrt(100))" });
            public static readonly NerdamerPart Sqrt = new NerdamerPart("sqrt(", "√‾");
            public static readonly NerdamerPart Floor = new NerdamerPart("floor(");
            public static readonly NerdamerPart Ceil = new NerdamerPart("ceil(");
            public static readonly NerdamerPart Round = new NerdamerPart("round(", "rnd");
            public static readonly NerdamerPart Trunc = new NerdamerPart("trunc(");
            public static readonly NerdamerPart Mod = new NerdamerPart("mod(");
            public static readonly NerdamerPart Gcd = new NerdamerPart("gcd(");
            public static readonly NerdamerPart Lcm = new NerdamerPart("lcm(");
            public static readonly NerdamerPart Mean = new NerdamerPart("mean(", "x̄");
            public static readonly NerdamerPart Mode = new NerdamerPart("mode(", "Mo");
            public static readonly NerdamerPart Median = new NerdamerPart("median(", "x̃");
            public static readonly NerdamerPart Expand = new NerdamerPart("expand(", "expd");
            public static readonly NerdamerPart DivideFunc = new NerdamerPart("divide(", "div");
            public static readonly NerdamerPart PFactor = new NerdamerPart("pfactor(", "pfact");
            public static readonly NerdamerPart Fib = new NerdamerPart("fib(");
            public static readonly NerdamerPart Factor = new NerdamerPart("factor(", "fact");
            public static readonly NerdamerPart Roots = new NerdamerPart("roots(");
            public static readonly NerdamerPart Coeffs = new NerdamerPart("coeffs(", "coeff");
            public static readonly NerdamerPart Solve = new NerdamerPart("solve(");
            public static readonly NerdamerPart SolveEquations = new NerdamerPart("solveEquations(", "{");
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
            public static readonly NerdamerPart Atan2 = new NerdamerPart("atan2(", "atan₂");
            public static readonly NerdamerPart Sin = new NerdamerPart("sin(");
            public static readonly NerdamerPart Asin = new NerdamerPart("asin(", "sin⁻¹"); //sin⁻¹
            public static readonly NerdamerPart Sinh = new NerdamerPart("sinh(");
            public static readonly NerdamerPart Asinh = new NerdamerPart("asinh(", "ˢⁱⁿʰ⁻¹");
            public static readonly NerdamerPart Cos = new NerdamerPart("cos(");
            public static readonly NerdamerPart Acos = new NerdamerPart("acos(", "cos⁻¹");
            public static readonly NerdamerPart Cosh = new NerdamerPart("cosh(");
            public static readonly NerdamerPart Acosh = new NerdamerPart("acosh(", "ᶜᵒˢʰ⁻¹");
            public static readonly NerdamerPart Tan = new NerdamerPart("tan(");
            public static readonly NerdamerPart Atan = new NerdamerPart("atan(", "tan⁻¹");
            public static readonly NerdamerPart Tanh = new NerdamerPart("tanh(");
            public static readonly NerdamerPart Atanh = new NerdamerPart("atanh(", "ᵗᵃⁿʰ⁻¹");
            public static readonly NerdamerPart Sinc = new NerdamerPart("sinc(");
            public static readonly NerdamerPart Sum = new NerdamerPart("sum(", "∑");
            public static readonly NerdamerPart Product = new NerdamerPart("product(", "∏");
            public static readonly NerdamerPart Diff = new NerdamerPart("diff(", "d/dx");
            public static readonly NerdamerPart Integrate = new NerdamerPart("integrate(", "∫");
            public static readonly NerdamerPart Defint = new NerdamerPart("defint(", "ₐ∫ᵇ");
            public static readonly NerdamerPart Step = new NerdamerPart("step(");
            public static readonly NerdamerPart Sec = new NerdamerPart("sec(");
            public static readonly NerdamerPart Asec = new NerdamerPart("asec(", "sec⁻¹");
            public static readonly NerdamerPart Sech = new NerdamerPart("sech(");
            public static readonly NerdamerPart Asech = new NerdamerPart("asech(", "ˢᵉᶜʰ⁻¹");
            public static readonly NerdamerPart Erf = new NerdamerPart("erf(");
            public static readonly NerdamerPart Csc = new NerdamerPart("csc(");
            public static readonly NerdamerPart Acsc = new NerdamerPart("acsc(", "csc⁻¹");
            public static readonly NerdamerPart Csch = new NerdamerPart("csch(");
            public static readonly NerdamerPart Acsch = new NerdamerPart("acsch(", "ᶜˢᶜʰ⁻¹");
            public static readonly NerdamerPart Rect = new NerdamerPart("rect(");
            public static readonly NerdamerPart Cot = new NerdamerPart("cot(");
            public static readonly NerdamerPart Acot = new NerdamerPart("acot(", "cot⁻¹");
            public static readonly NerdamerPart Coth = new NerdamerPart("coth(");
            public static readonly NerdamerPart Acoth = new NerdamerPart("acoth(", "ᶜᵒᵗʰ⁻¹");
            public static readonly NerdamerPart Tri = new NerdamerPart("tri(");
            public static readonly NerdamerPart Si = new NerdamerPart("Si(");
            public static readonly NerdamerPart Ci = new NerdamerPart("Ci(");
            public static readonly NerdamerPart Shi = new NerdamerPart("Shi(");
            public static readonly NerdamerPart Chi = new NerdamerPart("Chi(");
            public static readonly NerdamerPart Ei = new NerdamerPart("Ei(");
            public static readonly NerdamerPart Laplace = new NerdamerPart("laplace(", "ℒ");
            public static readonly NerdamerPart Smpvar = new NerdamerPart("smpvar(", "s²");
            public static readonly NerdamerPart Variance = new NerdamerPart("variance(", "σ²");
            public static readonly NerdamerPart Smpstdev = new NerdamerPart("smpstdev(", "s");
            public static readonly NerdamerPart Stdev = new NerdamerPart("stdev(", "σₓ");
            public static readonly NerdamerPart NotEqual = new NerdamerPart("!=", "≠");
            public static readonly NerdamerPart LessThan = new NerdamerPart("<");
            public static readonly NerdamerPart LessEqual = new NerdamerPart("<=", "≤");
            public static readonly NerdamerPart Equal = new NerdamerPart("==", "=");
            public static readonly NerdamerPart GreaterEqual = new NerdamerPart(">=", "≥");
            public static readonly NerdamerPart GreaterThan = new NerdamerPart(">");
            public static readonly NerdamerPart IMatrix = new NerdamerPart("imatrix(", "I");
            public static readonly NerdamerPart Matrix = new NerdamerPart("matrix(", "M");
            public static readonly NerdamerPart Matget = new NerdamerPart("matget(", "Mget");
            public static readonly NerdamerPart Matset = new NerdamerPart("matset(", "Mset");
            public static readonly NerdamerPart Invert = new NerdamerPart("invert(", "M⁻¹");
            public static readonly NerdamerPart Transpose = new NerdamerPart("transpose(", "Mᵀ");
            public static readonly NerdamerPart Vector = new NerdamerPart("vector(", "V");
            public static readonly NerdamerPart Vecget = new NerdamerPart("vecget(", "Vget");
            public static readonly NerdamerPart Vecset = new NerdamerPart("vecset(", "Vset");
            public static readonly NerdamerPart Cross = new NerdamerPart("cross(", "×");
            public static readonly NerdamerPart Dot = new NerdamerPart("dot(", "∘");

            public static readonly NerdamerPart If = new NerdamerPart("IF(");
            public static readonly NerdamerPart Div = new NerdamerPart("div(");
            public static readonly NerdamerPart GammaIncomplete = new NerdamerPart("gamma_incomplete(");
            public static readonly NerdamerPart Parens = new NerdamerPart("parens(", "⸨");
            public static readonly NerdamerPart SetEquation = new NerdamerPart("setEquation(");
            public static readonly NerdamerPart Sign = new NerdamerPart("sign(");
            public static readonly NerdamerPart ZScore = new NerdamerPart("zscore(", "zₓ");


            public static readonly Regex Splitter =
                new Regex(string.Join("|",
                    typeof(NerdamerPart).GetTypeInfo()
                    .DeclaredFields
                    .Where(x => x.FieldType == typeof(NerdamerPart) && x.Name != nameof(Empty)) //No empty matches
                    .Select(x => Regex.Escape(((NerdamerPart)x.GetValue(null)).Name)) //Assuming all fields of type NerdamerPart are static
                    .OrderByDescending(x => x.Length) //Regexes test from left to right; 
                    .Append(".")), RegexOptions.Compiled);                   //e.g. Matches("transpose((i))") => 
                    //No chars left out               //MatchCollection(5) { [transpose(], [(], [i], [)], [)] }
                    //e.g. Matches("transpose(1234河3守a化)") => 
                    //MatchCollection(11) { [transpose(], [1], [2], [3], [4], [河], [3], [守], [a], [化], [)] }
        }
    }
}