using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                string Prepend(char c, string s) => string.IsNullOrEmpty(s) ? s : c + s;
                string Append(string s, char c) => string.IsNullOrEmpty(s) ? s : s + c;
                this.Friendly = Friendly ?? Name;
                this.Name = Name;
                this.DescriptionTitle =
                    $"{(Name.Length > 1 ? Name.TrimEnd('(') : Name)} {Append(Prepend('(', new[] { (Friendly + "(" == Name ? null : Friendly), Spoken }.Aggregate((prev, current) => string.IsNullOrEmpty(prev) ? current : string.IsNullOrEmpty(current) ? prev : $"{prev}, {current}")), ')')}";
                //new[]{"", "2", "3", ""} => "(2, 3)"

                this.DescriptionContent = new System.Lazy<Task<string>>(async () =>
                {
                    try
                    {
                        return $"{Description}{NewLine}{NewLine}Usage:{NewLine}{Tab}" +
                        (UsageOverride ?? Append(Vars?.Aggregate(Name, (prev, current) => current.Var.FirstOrDefault() == OptionalChar ? $"{prev}){(prev.Contains(NewLine) ? prev.Substring(prev.LastIndexOf(NewLine, System.StringComparison.Ordinal) + 1) : NewLine + Tab + prev)}{(prev.EndsWith("(", System.StringComparison.Ordinal) ? string.Empty : ", ")}{current.Var.TrimStart(OptionalChar)}" : $"{prev}{(prev.EndsWith("(", System.StringComparison.Ordinal) ? string.Empty : ", ")}{current.Var}"), ')') ?? "None") +
                        $"{NewLine}Parameters:" + 
                        (Vars?.Aggregate("", (prev, current) => $"{prev}{NewLine}{Tab}{current.Var}{Separator} {current.Description}") ?? $"{NewLine}{Tab}None") +
                        $"{NewLine}Examples:" +
                        (Examples?.Zip(await Task.WhenAll(Examples.Select(async x => await (await SymbolicsEngine.Current).Evaluate($"nerdamer('{x}'){(Evaluate ? ".evaluate()" : string.Empty)}.text()"))), (In, Out) => (In, Out)).Aggregate("", (prev, current) => $"{prev}{NewLine}{Tab}{current.In}{Separator} {current.Out}") ?? $"{NewLine}{Tab}None");
                    }
                    catch (System.ArgumentNullException)
                    {
                        return null;
                    }
                });
            }
            
            public static NerdamerPart FromOperator(string Name, string Spoken, bool Prefix, bool Postfix, string Description, IEnumerable<string> Examples, string UsageOverride = null, string Friendly = null, bool Evaluate = true) =>
                new NerdamerPart(Name, $"The {Spoken} operator", Description, UsageOverride ?? $"{(Postfix ? "x" : string.Empty)}{Name}{(Prefix ? "y" : string.Empty)}",
                    new[] { Postfix ? ("x", "The left hand side expression") : default, Prefix ? ("y", "The right hand side expression") : default }.Where(x => !x.Equals(default)), Examples, Friendly, Evaluate);
            public static NerdamerPart FromLiteral(string Name, string Spoken, string Description, string Friendly = null, IEnumerable<string> Examples = null, bool Evaluate = true) =>
                new NerdamerPart(Name, Spoken, Description, Name, null, Examples ?? new[] { Name, "3" + Name, Name + "/2" }, Friendly, Evaluate);
            public static NerdamerPart FromDigit(char Digit) => FromLiteral(Digit.ToString(), $"The digit {Digit}", $"Denotes the digit with the value {Digit}.");
            public static NerdamerPart FromFunction(string Name, string Spoken, string Description,
                IEnumerable<(string Var, string Description)> Vars, IEnumerable<string> Examples, string Friendly = null, bool Evaluate = true) =>
                new NerdamerPart(Name + '(', $"The {Spoken} function", Description, null, Vars, Examples, Friendly ?? Name, Evaluate);
            public static NerdamerPart FromVariable(char Variable) => FromLiteral(Variable.ToString(), $"The variable {Variable}", $"Denotes the variable {Variable}. Note that {char.ToUpperInvariant(Variable)} is different from {char.ToLowerInvariant(Variable)}.");

            public static string Optional(string VarName) => OptionalChar + VarName;
            public static string ForPhone(string Phone, string Else) => Xamarin.Forms.Device.Idiom == Xamarin.Forms.TargetIdiom.Phone ? Phone : Else;

            public static void Init() => Equals(Empty, Space);

            public string Name { get; }
            public string DescriptionTitle { get; }
            public System.Lazy<Task<string>> DescriptionContent { get; }
            public string Friendly { get; }

            /*
        0   1   2   3   4   5   6   7   8   9   A   B   C   D   E   F
U+00Bx          x²  x³                      x¹                      
U+207x  x⁰  xⁱ          x⁴  x⁵  x⁶  x⁷  x⁸  x⁹  x⁺  x⁻  x⁼  x⁽  x⁾  xⁿ
U+208x  x₀  x₁  x₂  x₃  x₄  x₅  x₆  x₇  x₈  x₉  x₊  x₋  x₌  x₍  x₎  
U+209x  xₐ  xₑ  xₒ  xₓ  xₔ  xₕ  xₖ   xₗ  xₘ  xₙ   xₚ  xₛ  xₜ
ªºⱽⱼ◌ͣ◌ͤ◌ͥ◌ͦ◌ͧ◌ͨ◌ͩ◌ͪ◌ͫ◌ͬ◌ͭ◌ͮ◌ͯ ʰʲʳʷʸˡˢˣᴬᴮᴰᴱᴳᴴᴵᴶᴷᴸᴹᴺᴼᴾᴿᵀᵁᵂᵃᵅᵇᵈᵉᵍᵏᵐᵒᵖᵗᵘᵛᵢᵣᵤᵥᵪᵸᶜᶠᶢᶦᶩᶫᶰᶴᶸᶻ*/
            public static readonly NerdamerPart Empty = new NerdamerPart(string.Empty, "Empty", "Is an empty slot waiting to be filled with a use.", null, null, null);
            public static readonly NerdamerPart Space = FromLiteral(" ", "The space bar", "Beautifies an expression or denotes implicit multiplication.", "␣", new[] { "3 2", "a     b", "vw x  3   y    z"});
            public static readonly NerdamerPart Percent = FromOperator("%", "percentage", false, true, "Denotes percentages, which is equal to dividing by 100.", new[] { "1%", "a%", "1.3%%" });
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
#warning After https://github.com/jiggzson/nerdamer/issues/213 has been closed, mark this as evaluate
            public static readonly NerdamerPart Equation = FromOperator("=", "equality", true, true, "Denotes that the left and right expressions are equal, and forms an equation. Can be solved using the solve function or the solveEquations function.", new[] { "8=8", "2x=3", "solve(2x=3,x)", "solveEquations([x+y=4,2x+3y=5])" }, Evaluate: false);
            //public static readonly NerdamerPart Assign = new NerdamerPart(":=");
            public static readonly NerdamerPart Log = FromFunction("log", "logarithm", "Calculates the logarithm (a=b^x; b=a^(1/x); x=log(a,b)) with an optional base (defaults to the constant e).", new[] { ("a", "The antilogarithm (expression to calculate the logarithm for)."), (Optional("b"), "The optional base in which when multiplied by the result, will produce the original expression. Defaults to the constant e.") }, new string[] { "log(9, 3)", "log(e^3)", "log(a^x,a)", "log(2^log(2^3,2),2)" });
            public static readonly NerdamerPart Log10 = FromFunction("log10", "base 10 logarithm", "Is a convenient shorthand for log(x, 10).", new[] { ("x", "The expression to calculate the base 10 logarithm for.") }, new[] { "log10(10)", "log10(100)", "log10(1e308)" }, "log₁₀");
            public static readonly NerdamerPart Min = FromFunction("min", "minimum", "Calculates the minimum value from a set of expressions.", new[] { ("a,b,c,d...", "Any number of expressions, separated by the comma.") }, new[] { "min(1,2,3,π,e)", "min(π,2^2,3.5,10/3)" });
            public static readonly NerdamerPart Max = FromFunction("max", "maximum", "Calculates the maximum value from a set of expressions.", new[] { ("a,b,c,d...", "Any number of expressions, separated by the comma.") }, new[] { "max(sqrt(π)+1.5,e^(11/9),10/3,π)", "max(10, 100/10, log10(10^10), sqrt(100))" });
            public static readonly NerdamerPart Sqrt = FromFunction("sqrt", "square root", "Calculates the square root of an expression.", new[] { ("x", "The expression to find the square root for.") }, new[] { "sqrt(2)", "sqrt(3)", "sqrt(1.5)", "sqrt(-23)" }, "√‾");
            public static readonly NerdamerPart Floor = FromFunction("floor", "floor", "Finds the largest integer that is less than or equal to an expression.", new[] { ("x", "The expression to find the floor for.") }, new[] { "floor(7)", "floor(7.3)", "floor(7.7)", "floor(-7.3)", "floor(-7.7)", "floor(i)" }, "⌊x⌋");
            public static readonly NerdamerPart Ceil = FromFunction("ceil", "ceiling", "Finds the smallest integer that is greater than or equal to an expression.", new[] { ("x", "The expression to find the ceiling for.") }, new[] { "ceil(7)", "ceil(7.3)", "ceil(7.7)", "ceil(-7.3)", "ceil(-7.7)", "ceil(i)" }, "⌈x⌉");
#warning After https://github.com/jiggzson/nerdamer/issues/291 has been closed, mark this as evaluate
            public static readonly NerdamerPart Round = FromFunction("round", "round", "Finds the nearest integer or decimal with the specified decimal places of an expression. Rounds half up.", new[] { ("x", "The expression to round."), (Optional("d"), "The optional number of decimal places that the result should have. Defaults to 0.") }, new[] { "round(2.4)", "round(-2.6)", "round(-2.5)", "round(2.51001,1)", "round(-2.51001,2)", "round(2.50001,3)" }, "rnd", false);
#warning To do: Open an issue of implementing the second argument (number of decimal digits) in trunc (jiggzson is a bit mad right now)
            public static readonly NerdamerPart Trunc = FromFunction("trunc", "truncate", "Discards all decimal places of an expression.", new[] { ("x", "The expression to truncate decimal places from.") }, new[] { "round(-2.4)", "round(2.6)", "round(-2.5)" });
            public static readonly NerdamerPart Mod = FromFunction("mod", "modulo", "Calculates the remainder after an expression is divided by another.", new[] { ("x", "The dividend of the division."), ("y", "The divisor of the division.") }, new[] { "mod(1,2)", "mod(-1,2)", "mod(1,-2)", "mod(2.6,1.3)", "mod(1.10,1.09)" });
#warning To do: Open an issue of implementing variable argument list of lcm and gcd
            public static readonly NerdamerPart Gcd = FromFunction("gcd", "greatest common divisor", "Calculates the greatest expression that can divide the two expressions with no remainder.", new[] { ("x", "One of the two expressions to calculate the gcd for."), ("y", "Another one of the two expressions to calculate the gcd for.") }, new[] { "gcd(3,4)", "gcd(15,6)", "gcd(x^2,x)", "gcd(x^2+2x+1,x^2+3x+2)", "gcd(18x^5+40x^2+75x^4+98x^3,12x^5+122x^3+60x^2+70x^4)" });
            public static readonly NerdamerPart Lcm = FromFunction("lcm", "least common multiple", "Calculates the smallest expression that can be divided by the two provided expressions.", new[] { ("x", "One of the two expressions to calculate the lcm for."), ("y", "Another one of the two expressions to calculate the lcm for.") }, new[] { "lcm(3,4)", "lcm(15,6)", "lcm(x^2,x)", "lcm(x+2,3x^2+10x+8)", "lcm(2x^3,x^4)" });
            public static readonly NerdamerPart Mean = FromFunction("mean", "sample mean", "Calculates the arithmetic mean (average) of a set of values.", new[] { ("a,b,c,d...", "The set of values to calculate the mean for, or one vector containing the values.") }, new[] { "mean(-9.9,-5,0,1,22,4.32,2.1)", "mean(1.5,sqrt(2)+sqrt(3),-5,2/3,3/2)", "mean([1,2,3,4,5,6,7,a,b,c,d,e])" }, "x̄");
            public static readonly NerdamerPart Mode = FromFunction("mode", "mode", "Calculates the mode (the most frequent value) of a set of values.", new[] { ("a,b,c,d...", "The set of values to calculate the mode for, or one vector containing the values.") }, new[] { "mode(-9.9,-5,0,1,22,4.32,2.1)", "mode(1.5,sqrt(2)+sqrt(3),-5,2/3,3/2)", "mode([1,2,3,4,5,6,7,a,b,c,d,e])" }, "Mo");
            public static readonly NerdamerPart Median = FromFunction("median", "median", "Calculates the median (middle value after ordering) of a set of values.", new[] { ("a,b,c,d...", "The set of values to calculate the median for, or one vector containing the values.") }, new[] { "median(-9.9,-5,0,1,22,4.32,2.1)", "median(1.5,sqrt(2)+sqrt(3),-5,2/3,3/2)", "median([1,2,3,4,5,6,7,a,b,c,d,e])" }, "x̃");
            public static readonly NerdamerPart Expand = FromFunction("expand", "expand", "Expands an expression to enable easier addition and subtraction of expressions.", new[] { ("x", "The expression to expand.") }, new[] { "expand((x+2)(x-5)(x-3))", "expand((x+2)(3x^2-4x)(3+6x))", "expand((x+y+z)(-x+y+z)(x-y+z)(x+y-z))" }, "expd");
            public static readonly NerdamerPart DivideFunc = FromFunction("divide", "algebraic long division", "Divides two polynomials. While the / operator only does simple division, this factors the arguments and divides them.", new[] { ("x", "The dividend of the division."), ("y", "The divisor of the division.") }, new[] { "divide(x^2+4x-45,x^2+x-30)", "divide(x^3+3x^2y+3x y^2+y^3,x^2+2x y+y^2)" }, "div");
            public static readonly NerdamerPart PFactor = FromFunction("pfactor", "prime factorization", "Factorizes a rational number into a product of primes. NOTE: Make sure 'Keep Symbols' is set. When 'Evaluate Symbols' is set, the result is converted into the original number.", new[] { ("x", "The rational number to do prime factorization on.") },new[] { "pfactor(31)", "pfactor(60)", "pfactor(-70.8)", "pfactor(15!)" }, "pfact", false);
            public static readonly NerdamerPart Fib = FromFunction("fib", "fibonacci", "Calculates the number in the fibonacci sequence in the specified position. fib(1) = 1; fib(2) = 1; fib(n) = fib(n-1)+fib(n-2)", new[] { ("x", "The position in the fibonacci sequence.") }, new[] { "fib(1)", "fib(0)", "fib(10)", "fib(-6)", "fib(2.3)" });
            public static readonly NerdamerPart Factor = FromFunction("factor", "factorization", "Factorizes an expression to enable easier multiplication and division of expressions.", new[] { ("x", "The expression to factorize.") }, new[] { "factor(30-x-6x^2+x^3)", "factor(18x^5-15x^4-12x^3)", "factor(-x^4-y^4-z^4+2x^2y^2+2x^2z^2+2y^2z^2)" }, "fact");
            public static readonly NerdamerPart Roots = FromFunction("roots", "polynomial roots", "Finds the roots of a univariate (one variable only) polynomial such that the polynomial equates to 0.", new[] { ("x", "The polynomial to find roots for.") }, new[] { "roots(x^2-3*x-10)", "roots(x^2+1)", "roots(x^3-1)" });
            public static readonly NerdamerPart Coeffs = FromFunction("coeffs", "polynomial coefficients", "Gets the coefficients of a polynomial. The coefficients will be places in the index of their power plus one. So constants are in the first place, x^2 are in the 3rd place, etc. Errors if the expression given is not a polynomial. Holes are filled with zeros.", new[] { ("p", "The polynomial to find the coefficients for."), ("x", "The respective variable in which coefficients are get.") }, new[] { "coeffs(3x^2+1,x)", "coeffs(a x^2+b x+c,x)", "coeffs(a x+x^2/b,x)" }, "coeff", false);
            public static readonly NerdamerPart Solve = FromFunction("solve", "single equation solve", "Attempts to solve an equation. If no solutions are found then an empty vector is returned. It can solve for multivariate (multiple variable) polynomials up to the third degree. After which it can solve numerically for polynomials up to the the 100th degree. If it's a univariate (single variable)  equation it will attempt to solve it using numerical methods.", new[] { ("e", "The equation to solve. Can also be a polynomial which will be interpreted as equating to 0."), ("x", "The variable to solve for.") }, new[] { "solve(x^3+1, x)", "solve(x^2+2*x+1, x)", "solve(3*(x+a)*(x-b), x)", "solve(cos(x), x)" });
            public static readonly NerdamerPart SolveEquations = FromFunction("solveEquations", "multiple equations solve", "Solves a system of linear equations. The array returned is in the format [variable1, value1, variable2, value2, etc]. NOTE: Must use with 'Keep Symbols'.", new[] { ( "e", "An equation or a vector of equations to solve."), (Optional("x"), "The optional variable to solve for. Defaults to x.") }, new[] { "solveEquations([x+y=1, 2*x=6, 4*z+y=6])", "solveEquations(cos(x)+cos(3*x)=1, x)", "solveEquations(x^2+8+y=x+6, x)" }, "slvEqs", false);
            public static readonly NerdamerPart a = FromVariable('a');
            public static readonly NerdamerPart b = FromVariable('b');
            public static readonly NerdamerPart c = FromVariable('c');
            public static readonly NerdamerPart d = FromVariable('d');
            public static readonly NerdamerPart e = FromVariable('e');
            public static readonly NerdamerPart f = FromVariable('f');
            public static readonly NerdamerPart g = FromVariable('g');
            public static readonly NerdamerPart h = FromVariable('h');
            public static readonly NerdamerPart i = FromVariable('i');
            public static readonly NerdamerPart j = FromVariable('j');
            public static readonly NerdamerPart k = FromVariable('k');
            public static readonly NerdamerPart l = FromVariable('l');
            public static readonly NerdamerPart m = FromVariable('m');
            public static readonly NerdamerPart n = FromVariable('n');
            public static readonly NerdamerPart o = FromVariable('o');
            public static readonly NerdamerPart p = FromVariable('p');
            public static readonly NerdamerPart q = FromVariable('q');
            public static readonly NerdamerPart r = FromVariable('r');
            public static readonly NerdamerPart s = FromVariable('s');
            public static readonly NerdamerPart t = FromVariable('t');
            public static readonly NerdamerPart u = FromVariable('u');
            public static readonly NerdamerPart v = FromVariable('v');
            public static readonly NerdamerPart w = FromVariable('w');
            public static readonly NerdamerPart x = FromVariable('x');
            public static readonly NerdamerPart y = FromVariable('y');
            public static readonly NerdamerPart z = FromVariable('z');
            public static readonly NerdamerPart A = FromVariable('A');
            public static readonly NerdamerPart B = FromVariable('B');
            public static readonly NerdamerPart C = FromVariable('C');
            public static readonly NerdamerPart D = FromVariable('D');
            public static readonly NerdamerPart E = FromVariable('E');
            public static readonly NerdamerPart F = FromVariable('F');
            public static readonly NerdamerPart G = FromVariable('G');
            public static readonly NerdamerPart H = FromVariable('H');
            public static readonly NerdamerPart I = FromVariable('I');
            public static readonly NerdamerPart J = FromVariable('J');
            public static readonly NerdamerPart K = FromVariable('K');
            public static readonly NerdamerPart L = FromVariable('L');
            public static readonly NerdamerPart M = FromVariable('M');
            public static readonly NerdamerPart N = FromVariable('N');
            public static readonly NerdamerPart O = FromVariable('O');
            public static readonly NerdamerPart P = FromVariable('P');
            public static readonly NerdamerPart Q = FromVariable('Q');
            public static readonly NerdamerPart R = FromVariable('R');
            public static readonly NerdamerPart S = FromVariable('S');
            public static readonly NerdamerPart T = FromVariable('T');
            public static readonly NerdamerPart U = FromVariable('U');
            public static readonly NerdamerPart V = FromVariable('V');
            public static readonly NerdamerPart W = FromVariable('W');
            public static readonly NerdamerPart X = FromVariable('X');
            public static readonly NerdamerPart Y = FromVariable('Y');
            public static readonly NerdamerPart Z = FromVariable('Z');
            public static readonly NerdamerPart Atan2 = FromFunction("atan2", "2-argument arctangent", "Calculates a number between -pi and pi representing the angle theta of an (x, y) point. This is the counterclockwise angle centred at the origin, measured in radians, between the positive X axis, and the point (x, y). The y-coordinate is passed first because this function is sililar to atan(y/x) but calculates approppriately for different sign combinations of x and y, and/or where x is 0.", new[] { ("y", "The y-coordinate of the point to calculate."), ("x", "The x-coordinate of the point to calculate.") }, new[] { "atan2(2.1,0)", "atan2(-2.1,0)", "atan2(2.1,-2.1)", "atan2(0,-2.1)" }, "atan₂");
            public static readonly NerdamerPart Sin = FromFunction("sin", "sine", "Calculates the ratio of the length of the side that is opposite for the specified angle to the length of the longest side of the triangle (the hypotenuse) in a right-angled triangle.", new[] { ("θ", "The angle to find the sine ratio for.") }, new[] { "sin(0)", "sin(π)", "sin(2π)", "sin(90°)", "sin(30°)" });
            public static readonly NerdamerPart Asin = FromFunction("asin", "arcus sine", $"Is the inverse sine function. Calculates the corrosponding angle in radians from a sine ratio,{NewLine}e.g. sin(θ) = x; θ = asin(x).", new[] { ("x", "The sine ratio to find the corrosponding angle for.") }, new[] { "asin(0)", "asin(1)", "asin(0.5)", "asin(sqrt(2)/2)", "asin(sqrt(3)/2)" }, "sin⁻¹"); //
            public static readonly NerdamerPart Sinh = FromFunction("sinh", "hyperbolic sine", $"Calculates the hyperbolic sine ratio of the specified hyperbolic angle.{NewLine}sinh(x) = (1-e^(-2x))/(2e^(-x))", new[] { ("x", "The hyperbolic angle to find the hyperbolic sine ratio for.") }, new[] { "sinh(0)", "sinh(π)", "sinh(90°)", "sinh(30°)" });
            public static readonly NerdamerPart Asinh = FromFunction("asinh", "area hyperbolic sine", $"Calculates the corrosponding hyperbolic angle from a hyperbolic sine ratio.{NewLine}asinh(z) = log(z+sqrt(z^2+1))", new[] { ("z", "The hyperbolic sine ratio to find the hyperbolic angle for.") }, new[] { "asinh(0)", "asinh(1)", "asinh(sinh(3))" }, ForPhone("ˢⁱⁿʰ⁻¹", "sinh⁻¹"));
            public static readonly NerdamerPart Cos = FromFunction("cos", "cosine (sine complement)", "Calculates the ratio of the length of the adjacent side to the length of the hypotenuse, so called because it is the sine of the complementary or co-angle, the other non-right angle in a right-angled triangle.", new[] { ("θ", "The angle to find the cosine ratio for.") }, new[] { "cos(0)", "cos(π)", "cos(2π)", "cos(90°)", "cos(30°)" });
            public static readonly NerdamerPart Acos = FromFunction("acos", "arcus cosine", $"Is the inverse cosine function. Calculates the corrosponding angle in radians from a cosine ratio,{NewLine}e.g. cos(θ) = x; θ = acos(x).", new[] { ("x", "The sine ratio to find the corrosponding angle for.") }, new[] { "acos(0)", "acos(1)", "acos(0.5)", "acos(sqrt(2)/2)", "acos(sqrt(3)/2)" }, "cos⁻¹");
            public static readonly NerdamerPart Cosh = FromFunction("cosh", "hyperbolic cosine", $"Calculates the hyperbolic cosine ratio of the specified hyperbolic angle.{NewLine}cosh(x) = (1+e^(-2x))/(2e^(-x))", new[] { ("x", "The hyperbolic angle to find the hyperbolic cosine ratio for.") }, new[] { "cosh(0)", "cosh(π)", "cosh(90°)", "cosh(30°)" });
            public static readonly NerdamerPart Acosh = FromFunction("acosh", "area hyperbolic cosine", $"Calculates the corrosponding hyperbolic angle from a hyperbolic cosine ratio.{NewLine}acosh(z) = log(z+sqrt(z^2-1))", new[] { ("z", "The hyperbolic cosine ratio to find the hyperbolic angle for.") }, new[] { "acosh(0)", "acosh(1)", "acosh(cosh(3))" }, ForPhone("ᶜᵒˢʰ⁻¹", "cosh⁻¹"));
            public static readonly NerdamerPart Tan = FromFunction("tan", "tangent", "Calculates the ratio of the length of the opposite side to the length of the adjacent side in a right-angled triangle.", new[] { ("θ", "The angle to find the tangent ratio for.") }, new[] { "tan(0)", "tan(π)", "tan(2π)", "tan(90°)", "tan(30°)" });
            public static readonly NerdamerPart Atan = FromFunction("atan", "arcus tangent", $"Is the inverse tangent function. Calculates the corrosponding angle in radians from a tangent ratio,{NewLine}e.g. tan(θ) = x; θ = atan(x).", new[] { ("x", "The tangent ratio to find the corrosponding angle for.") }, new[] { "atan(0)", "atan(1)", "atan(0.5)", "atan(sqrt(2)/2)", "atan(sqrt(3)/2)" }, "tan⁻¹");
            public static readonly NerdamerPart Tanh = FromFunction("tanh", "hyperbolic tangent", $"Calculates the hyperbolic tangent ratio of the specified hyperbolic angle.{NewLine}tanh(x) = (1-e^(-2x))/(1+e^(-2x))", new[] { ("x", "The hyperbolic angle to find the hyperbolic tangent ratio for.") }, new[] { "tanh(0)", "tanh(π)", "tanh(90°)", "tanh(30°)" });
            public static readonly NerdamerPart Atanh = FromFunction("atanh", "area hyperbolic tangent", $"Calculates the corrosponding hyperbolic angle from a hyperbolic tangent ratio.{NewLine}atanh(z) = 1/2log((1+z)/(1-z))", new[] { ("z", "The hyperbolic tangent ratio to find the hyperbolic angle for.") }, new[] { "atanh(0)", "atanh(1)", "atanh(tanh(3))" }, ForPhone("ᵗᵃⁿʰ⁻¹", "tanh⁻¹"));
            public static readonly NerdamerPart Sinc = FromFunction("sinc", "unnormalized sine cardinal", "Gives 1 when x is 0 and sin(x)/x otherwise where x is the argument.", new[] { ("x", "The value to calculate the unnormalized sine cardinal for.") }, new[] { "sinc(0)", "sinc(π)", "sinc(π/2)" });
            public static readonly NerdamerPart Sum = FromFunction("sum", "summation", "Adds an expression for (j-i) times with a given index variable being assigned with (i+k), where i is the given starting value, j is the given ending value, and k is the current number of times that the addition inside this function has been performed.", new[] { ("e", "The expression to add for each time of addition."), ("x", "The index variable being assigned."), ("i", "The starting number of the index variable."), ("j", "The ending number of the index variable.") }, new[] { "sum(x+1,x,1,5)", "sum(x+y,x,1,5)" }, "∑");
            public static readonly NerdamerPart Product = FromFunction("product", "product", "Multiplies an expression for (j-i) times with a given index variable being assigned with (i+k), where i is the given starting value, j is the given ending value, and k is the current number of times that the multiplication inside this function has been performed.", new[] { ("e", "The expression to multiply for each time of multiplication."), ("x", "The index variable being assigned."), ("i", "The starting number of the index variable."), ("j", "The ending number of the index variable.") }, new[] { "product(x+1,x,1,5)", "product(x+y,x,1,5)" }, "∏");
            public static readonly NerdamerPart Diff = FromFunction("diff", "differentiate", "Gets the derivative of a function with respect to a given variable. It measures the sensitivity to change of the function (output value) with respect to a change in that variable (input value).", new[] { ("e", "The expression to calculate the derivative for."), ("x", "The variable with respect to differentiate."), (Optional("n"), "The optional value for the number of derivative. Defaults to 1.") }, new[] { "diff(cos(x)sin(x),x)", "diff([x^2,cos(x),1],x,2)", "diff(x^3+a x^3+x^2,x,2)", "diff((x^2+1)tan(x),x)" }, "d/dx");
            public static readonly NerdamerPart Integrate = FromFunction("integrate", "integrate", "");//, "Calculates the " "∫");
            public static readonly NerdamerPart Defint = new NerdamerPart("defint(", "ₐ∫ᵇ");
            public static readonly NerdamerPart Step = new NerdamerPart("step(");
            public static readonly NerdamerPart Sec = new NerdamerPart("sec(");
            public static readonly NerdamerPart Asec = new NerdamerPart("asec(", "sec⁻¹");
            public static readonly NerdamerPart Sech = new NerdamerPart("sech(");
            public static readonly NerdamerPart Asech = new NerdamerPart("asech(", ForPhone("ˢᵉᶜʰ⁻¹", "sech⁻¹"));
            public static readonly NerdamerPart Erf = new NerdamerPart("erf(");
            public static readonly NerdamerPart Csc = new NerdamerPart("csc(");
            public static readonly NerdamerPart Acsc = new NerdamerPart("acsc(", "csc⁻¹");
            public static readonly NerdamerPart Csch = new NerdamerPart("csch(");
            public static readonly NerdamerPart Acsch = new NerdamerPart("acsch(", ForPhone("ᶜˢᶜʰ⁻¹", "csch⁻¹"));
            public static readonly NerdamerPart Rect = new NerdamerPart("rect(");
            public static readonly NerdamerPart Cot = new NerdamerPart("cot(");
            public static readonly NerdamerPart Acot = new NerdamerPart("acot(", "cot⁻¹");
            public static readonly NerdamerPart Coth = new NerdamerPart("coth(");
            public static readonly NerdamerPart Acoth = new NerdamerPart("acoth(", ForPhone("ᶜᵒᵗʰ⁻¹", "coth⁻¹"));
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
                    .OrderByDescending(x => x.Length)      //Regexes test from left to right; 
                    .Append(".")), RegexOptions.Compiled); //e.g. Matches("transpose((i))") => 
                    //No chars left out                    //MatchCollection(5) { [transpose(], [(], [i], [)], [)] }
                    //e.g. Matches("transpose(1234河3守a化)") => 
                    //MatchCollection(11) { [transpose(], [1], [2], [3], [4], [河], [3], [守], [a], [化], [)] }
        }
    }
}
