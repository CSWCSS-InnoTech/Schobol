using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Engine = InnoTecheLearning.Utils.SymbolicsEngine;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#if !WINDOWS_UWP
using MathNet.Symbolics;
#endif

namespace InnoTecheLearning.Pages
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class Logic_Symbolics : ContentPage
    {
        static readonly string[,] WhenNorm = new string[,]
            {
                { ",", "[", "]", "(", ")" },
                { "7", "8", "9", "^", "!" },
                { "4", "5", "6", "*", "/" },
                { "1", "2", "3", "+", "-" },
                { "0", ".", "pi", "e", "i" }
            };
        static readonly string[,] WhenShift = new string[,]
            {
                { "log(", "log10(", "min(", "max(", "sqrt(" },
                { "floor(", "ceil(", "round(", ""/*trunc(*/, ""/**/ },
                { "Si(", "Ci(", "Shi(", "Chi(", "Ei(" },
                { "sinc(", "step(", "mod(", ""/**/, ""/**/ },
                { "fib(", "erf(", "pfactor(", "rect(", "tri(" }
            };
        static readonly string[,] WhenAlpha = new string[,]
            {
                { "a", "b", "c", "d", "e" },
                { "f", "g", "h", "i", "j" },
                { "k", "l", "m", "n", "o" },
                { "p", "q", "r", "s", "t" },
                { "u", "v", "w", "x", "y" }
            };
        static readonly string[,] WhenShiftAlpha = new string[,]
            {
                { "A", "B", "C", "D", "E" },
                { "F", "G", "H", "I", "J" },
                { "K", "L", "M", "N", "O" },
                { "P", "Q", "R", "S", "T" },
                { "U", "V", "W", "X", "Y" }
            };
        static readonly string[,] WhenTrig = new string[,]
            {
                { "sin(", "asin(", "sinh(", "asinh(", "cot(" },
                { "cos(", "acos(", "cosh(", "acosh(", "acot(" },
                { "tan(", "atan(", "tanh(", "atanh(", "atan2(" },
                { "sec(", "asec(", "sech(", "asech(", "coth(" },
                { "csc(", "acsc(", "csch(", "acsch(", "acoth(" }
            };
        bool DisplayDecimals = true;
        bool DoEvaluate = true;
        public Logic_Symbolics()
        {
            InitializeComponent();

            Calculate.Clicked += Calculate_Clicked;
            Expand.Clicked += Expand_Clicked;
            Factorize.Clicked += Factorize_Clicked;

            Display.Clicked += (sender, e) => Display.Text = (DisplayDecimals = !DisplayDecimals) ? "Display Decimals" : "Display Fractions";
            Evaluate.Clicked += (sender, e) => Evaluate.Text = (DoEvaluate = !DoEvaluate) ? "Evaluate Symbols" : "Keep Symbols";

            Out.Focused += (sender, e) => Out.Unfocus();
            OutCopy.Clicked += (sender, e) => Utils.SetClipboardText(Out.Text);

            async void Debug_Clicked(object sender, EventArgs e) => await Eval("{0}");
            Debug.Clicked += Debug_Clicked;
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