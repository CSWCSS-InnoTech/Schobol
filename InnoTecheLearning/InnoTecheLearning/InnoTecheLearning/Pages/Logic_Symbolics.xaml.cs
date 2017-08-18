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
        //TODO: Add methods from https://help.syncfusion.com/cr/xamarin/calculate
        //Number suffix reference: http://stackoverflow.com/questions/7898310/using-regex-to-balance-match-parenthesis
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
        static readonly (string, string[,]) WhenNorm = ("%", new string[ButtonRows, ButtonColumns]
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
                { "gcd(", "lcm(", "mean(", "mode(", "median(" },
                { "expand(", "divide(", "pfactor(", "fib(", "" },
                { "factor(", "roots(", "coeffs(", "solve(", "solveEquations(" }
            });
        static readonly (string, string[,]) WhenAlpha = ("a", new string[ButtonRows, ButtonColumns]
            {
                { "b", "c", "d", "e", "f" },
                { "g", "h", "i", "j", "k" },
                { "l", "m", "n", "o", "p" },
                { "q", "r", "s", "t", "u" },
                { "v", "w", "x", "y", "z" }
            });
        static readonly (string, string[,]) WhenShiftAlpha = ("A", new string[ButtonRows, ButtonColumns]
            {
                { "B", "C", "D", "E", "F" },
                { "G", "H", "I", "J", "K" },
                { "L", "M", "N", "O", "P" },
                { "Q", "R", "S", "T", "U" },
                { "V", "W", "X", "Y", "Z" }
            });
        static readonly (string, string[,]) WhenAlt = ("atan2(", new string[ButtonRows, ButtonColumns]
            {
                { "sin(", "asin(", "sinh(", "asinh(", "" },
                { "cos(", "acos(", "cosh(", "acosh(", "" },
                { "tan(", "atan(", "tanh(", "atanh(", "" },
                { "", "sinc(", "", "", "" },
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
            EventHandler ModClicked(ButtonModifier Modifier) => (sender, e) =>
            {
                Mod ^= Modifier;
                for (int i = 0; i < ButtonRows; i++)
                    for (int j = 0; j < ButtonColumns; j++)
                        switch (Mod)
                        {
                            case ButtonModifier.Norm:
                                B03.Text = WhenNorm.Item1;
                                Buttons[i, j].Text = WhenNorm.Item2[i, j];
                                break;
                            case ButtonModifier.Shift:
                                B03.Text = WhenShift.Item1;
                                Buttons[i, j].Text = WhenShift.Item2[i, j];
                                break;
                            case ButtonModifier.Alpha:
                                B03.Text = WhenAlpha.Item1;
                                Buttons[i, j].Text = WhenAlpha.Item2[i, j];
                                break;
                            case ButtonModifier.Alt:
                                B03.Text = WhenAlt.Item1;
                                Buttons[i, j].Text = WhenAlt.Item2[i, j];
                                break;
                            case ButtonModifier.Shift | ButtonModifier.Alpha:
                                B03.Text = WhenShiftAlpha.Item1;
                                Buttons[i, j].Text = WhenShiftAlpha.Item2[i, j];
                                break;
                            case ButtonModifier.Shift | ButtonModifier.Alt:
                                B03.Text = WhenShiftAlt.Item1;
                                Buttons[i, j].Text = WhenShiftAlt.Item2[i, j];
                                break;
                            case ButtonModifier.Alt | ButtonModifier.Alpha:
                                B03.Text = WhenAltAlpha.Item1;
                                Buttons[i, j].Text = WhenAltAlpha.Item2[i, j];
                                break;
                            case ButtonModifier.Shift | ButtonModifier.Alt | ButtonModifier.Alpha:
                                B03.Text = "";
                                Buttons[i, j].Text = "";
                                break;
                            default:
                                B03.Text = Buttons[i, j].Text = "";
                                break;
                        }
                if (Mod.HasFlag(Modifier)) (sender as VisualElement)?.TranslateTo(0, 10, 0);
                else (sender as VisualElement)?.TranslateTo(0, 0, 0);
            };
            EventHandler ButtonClicked = (sender, e) =>
            {
                In.Text += (sender as Button)?.Text;
                Mod = ButtonModifier.Norm;
                Shift.TranslateTo(0, 0, 0);
                Alpha.TranslateTo(0, 0, 0);
                Alt.TranslateTo(0, 0, 0);
                for (int i = 0; i < ButtonRows; i++)
                    for (int j = 0; j < ButtonColumns; j++)
                    {
                        B03.Text = WhenNorm.Item1;
                        Buttons[i, j].Text = WhenNorm.Item2[i, j];
                    };
            };
            Shift.Clicked += ModClicked(ButtonModifier.Shift);
            Alpha.Clicked += ModClicked(ButtonModifier.Alpha);
            Alt.Clicked += ModClicked(ButtonModifier.Alt);
            Back.Clicked += (sender, e) => In.Text = In.Text?.Length > 0 ? In.Text.Remove(In.Text.Length - 1) : null;

            B03.Clicked += ButtonClicked;
            for (int i = 0; i < ButtonRows; i++)
                for (int j = 0; j < ButtonColumns; j++)
                    Buttons[i, j].Clicked += ButtonClicked;

            Calculate.Clicked += Calculate_Clicked;
            Expand.Clicked += Expand_Clicked;
            Factorize.Clicked += Factorize_Clicked;

            Display.Clicked += (sender, e) => Display.Text = (DisplayDecimals = !DisplayDecimals) ? "Display Decimals" : "Display Fractions";
            Evaluate.Clicked += (sender, e) => Evaluate.Text = (DoEvaluate = !DoEvaluate) ? "Evaluate Symbols" : "Keep Symbols";

            Out.Focused += (sender, e) => Out.Unfocus();
            OutCopy.Clicked += (sender, e) => Utils.ClipboardText = Out.Text;

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
                Out.Text = (await (await Current).Evaluate(string.Format(Format, In.Text.Replace("'", "\\'").Replace("\\", "\\\\"), (DoEvaluate ? ".evaluate()" : null) + (DisplayDecimals ? ".text()" : ".toString()")))).Trim('"');
            }
            catch (Exception ex)
            {
                Out.Text = Utils.Error + ex.Message;
            }
            //NextEngine();
        }
        async void Calculate_Clicked(object sender, EventArgs e) => await Eval("nerdamer('{0}'){1}");
        async void Expand_Clicked(object sender, EventArgs e) => await Eval("nerdamer.expand('{0}'){1}");
        async void Factorize_Clicked(object sender, EventArgs e) => await Eval("nerdamer.factor('{0}'){1}");
        Task<Engine> Current = CreateEngineAsync();
        //Task<Engine> Next = CreateEngineAsync();
        //void NextEngine() { Current = Next; Next = CreateEngineAsync(); }

        static async Task<Engine> CreateEngineAsync()
        {
            var JSEngine = new Engine();
            await JSEngine.Evaluate(Utils.Resources.GetString("nerdamer.core.js"));
            await JSEngine.Evaluate(Utils.Resources.GetString("Algebra.js"));
            await JSEngine.Evaluate(Utils.Resources.GetString("Calculus.js"));
            await JSEngine.Evaluate(Utils.Resources.GetString("Solve.js"));
            await JSEngine.Evaluate(Utils.Resources.GetString("Extra.js"));
            await JSEngine.Evaluate("nerdamer.setFunction('lcm', ['a', 'b'], '(a / gcd(a, b)) * b')");
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