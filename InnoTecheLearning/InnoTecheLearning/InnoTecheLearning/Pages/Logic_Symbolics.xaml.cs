using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine = InnoTecheLearning.Utils.SymbolicsEngine;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#if false
using MathNet.Symbolics;
#endif

namespace InnoTecheLearning.Pages
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class Logic_Symbolics : ContentPage
    {
        const int ButtonRows = 5;
        const int ButtonColumns = 5;
        [Flags] enum ButtonModifier : byte
        {
            Norm = 0,
            Shift = 1,
            Alpha = 2,
            Alt = 4
        }
        ButtonModifier Mod = ButtonModifier.Norm;
        static readonly (string, string[,]) WhenNorm = ("", new string[ButtonRows, ButtonColumns]
            {
                { ",", "[", "]", "(", ")" },
                { "7", "8", "9", "^", "!" },
                { "4", "5", "6", "*", "/" },
                { "1", "2", "3", "+", "-" },
                { "0", ".", "pi", "e", "i" }
            });
        static readonly (string, string[,]) WhenShift = ("", new string[ButtonRows, ButtonColumns]
            {
                { "log(", "log10(", "min(", "max(", "sqrt(" },
                { "floor(", "ceil(", "round(", "trunc(", "mod(" },
                { "", "", "", "", "" },
                { "expand(", "divide(", "pfactor(", "fib(", "" },
                { "factor(", "roots(", "coeffs(", "solve(", "solveEquations(" }
            });
        static readonly (string, string[,]) WhenAlpha = ("z", new string[ButtonRows, ButtonColumns]
            {
                { "a", "b", "c", "d", "e" },
                { "f", "g", "h", "i", "j" },
                { "k", "l", "m", "n", "o" },
                { "p", "q", "r", "s", "t" },
                { "u", "v", "w", "x", "y" }
            });
        static readonly (string, string[,]) WhenShiftAlpha = ("Z", new string[ButtonRows, ButtonColumns]
            {
                { "A", "B", "C", "D", "E" },
                { "F", "G", "H", "I", "J" },
                { "K", "L", "M", "N", "O" },
                { "P", "Q", "R", "S", "T" },
                { "U", "V", "W", "X", "Y" }
            });
        static readonly (string, string[,]) WhenAlt = ("sinc(", new string[ButtonRows, ButtonColumns]
            {
                { "sin(", "asin(", "sinh(", "asinh(", "mean(" },
                { "cos(", "acos(", "cosh(", "acosh(", "mode(" },
                { "tan(", "atan(", "tanh(", "atanh(", "median(" },
                { "", "atan2(", "gcd(", "lcm(", "" },
                { "sum(", "product(", "diff(", "integrate(", "defint(" }
            });
        static readonly (string, string[,]) WhenShiftAlt = ("step(", new string[ButtonRows, ButtonColumns]
            {
                { "sec(", "asec(", "sech(", "asech(", "erf(" },
                { "csc(", "acsc(", "csch(", "acsch(", "rect(" },
                { "cot(", "acot(", "coth(", "acoth(", "tri(" },
                { "Si(", "Ci(", "Shi(", "Chi(", "Ei(" },
                { "laplace(", "smpvar(", "variance(", "smpstdev(", "stdev(" }
            });
        static readonly (string, string[,]) WhenAltAlpha = ("!=", new string[ButtonRows, ButtonColumns]
            {
                { "<", "<=" , "==", ">=", ">" },
                { "", "" , "", "", "" },
                { "imatrix(", "" , "", "", "" },
                { "matrix(", "matget(" , "matset(", "invert(", "transpose(" },
                { "vector(", "vecget(" , "vecset(", "cross(", "dot(" }
            });
        bool DisplayDecimals = true;
        bool DoEvaluate = true;
        public Logic_Symbolics()
        {
            InitializeComponent();

            var Buttons = new Button[ButtonRows, ButtonColumns]
            {
                { B10, B11, B12, B13, B14 },
                { B20, B21, B22, B23, B24 },
                { B30, B31, B32, B33, B34 },
                { B40, B41, B42, B43, B44 },
                { B50, B51, B52, B53, B54 }
            };

            Shift.Clicked += (sender, e) => 
            {
                Mod ^= ButtonModifier.Shift;
                if (Mod.HasFlag(ButtonModifier.Shift)) Shift.TranslateTo(0, 0, 0);
                else Shift.TranslateTo(0, 10, 0);
            };
            Alpha.Clicked += (sender, e) =>
            {
                Mod ^= ButtonModifier.Alpha;
                if (Mod.HasFlag(ButtonModifier.Alpha)) Alpha.TranslateTo(0, 0, 0);
                else Alpha.TranslateTo(0, 10, 0);
            };
            Alt.Clicked += (sender, e) =>
            {
                Mod ^= ButtonModifier.Alt;
                if (Mod.HasFlag(ButtonModifier.Alt)) Alt.TranslateTo(0, 0, 0);
                else Shift.TranslateTo(0, 10, 0);
            };
            for (int i = 0; i < ButtonRows; i++)
                for (int j = 0; j < ButtonColumns; j++)
                    Buttons[i, j].Clicked += (sender, e) =>
                    {
                        switch (Mod)
                        {
                            case ButtonModifier.Norm:
                                break;
                            case ButtonModifier.Shift:
                                break;
                            case ButtonModifier.Alpha:
                                break;
                            case ButtonModifier.Alt:
                                break;
                            case ButtonModifier.Shift | ButtonModifier.Alpha:
                                break;
                            case ButtonModifier.Shift | ButtonModifier.Alt:
                                break;
                            case ButtonModifier.Alpha | ButtonModifier.Alt:
                                break;
                            default:
                                break;
                        }
                    };

            Calculate.Clicked += Calculate_Clicked;
            Expand.Clicked += Expand_Clicked;
            Factorize.Clicked += Factorize_Clicked;

            Display.Clicked += (sender, e) => Display.Text = (DisplayDecimals = !DisplayDecimals) ? "Display Decimals" : "Display Fractions";
            Evaluate.Clicked += (sender, e) => Evaluate.Text = (DoEvaluate = !DoEvaluate) ? "Evaluate Symbols" : "Keep Symbols";

            Out.Focused += (sender, e) => Out.Unfocus();
            OutCopy.Clicked += (sender, e) => Utils.SetClipboardText(Out.Text);

            /*
            async void Debug_Clicked(object sender, EventArgs e) => await Eval("{0}");
            Debug.Clicked += Debug_Clicked;
            */
        }

#if true
        async Task Eval(string Format)
        { 
            try
            {
                //Android needs .toString() and trim " while Windows 10 does not
                Out.Text = (await (await Current).EvaluateNoReturn(string.Format(Format, In.Text.Replace("'", "\\'").Replace("\\", "\\\\"), (DoEvaluate ? ".evaluate()" : null) + (DisplayDecimals ? ".text()" : ".toString()")))).Trim('"');
            }
            catch (Exception ex)
            {
                Out.Text = Utils.Error + ex.Message;
            }
            NextEngine();
        }
        async void Calculate_Clicked(object sender, EventArgs e) => await Eval("nerdamer('{0}'){1}");
        async void Expand_Clicked(object sender, EventArgs e) => await Eval("nerdamer.expand('{0}'){1}");
        async void Factorize_Clicked(object sender, EventArgs e) => await Eval("nerdamer.factor('{0}'){1}");
        Task<Engine> Current = CreateEngineAsync();
        Task<Engine> Next = CreateEngineAsync();
        void NextEngine() { Current = Next; Next = CreateEngineAsync(); }

        static async Task<Engine> CreateEngineAsync()
        {
            var JSEngine = new Engine();
            await JSEngine.EvaluateNoReturn(Utils.Resources.GetString("nerdamer.core.js"));
            await JSEngine.EvaluateNoReturn(Utils.Resources.GetString("Algebra.js"));
            await JSEngine.EvaluateNoReturn(Utils.Resources.GetString("Calculus.js"));
            await JSEngine.EvaluateNoReturn(Utils.Resources.GetString("Solve.js"));
            await JSEngine.EvaluateNoReturn(Utils.Resources.GetString("Extra.js"));
            return JSEngine;
        }
#else
        void Calculate_Clicked(object sender, EventArgs e) => Eval(x => x, Infix.Format);
        void Expand_Clicked(object sender, EventArgs e) => Eval(Algebraic.Expand, Infix.Format);
        void Factorize_Clicked(object sender, EventArgs e) => Eval(MathNet.Symbolics.Algebraic.Factors, 
            x => x.Select(y => $"({Infix.Format(y)})").Aggregate((y, z) => $"{y}*{z}"));
        void Eval<T>(Func<Expression, T> Func, Func<T, string> Formatter)
        {
            switch (Infix.Parse(In.Text))
            {
                case ParseResult.ParsedExpression Success:
                    Out.Text = Formatter(Func(Success.Item));
                    break;
                case ParseResult.ParseFailure Fail:
                    Out.Text = Utils.Error + Fail.Item;
                    break;
                default:
                    break;
            }
        }
#endif
    }
}