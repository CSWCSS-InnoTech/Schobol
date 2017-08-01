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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Logic_Symbolics : ContentPage
    {

        public Logic_Symbolics()
        {
            InitializeComponent();

            Evaluate.Clicked += Evaluate_Clicked;
            Expand.Clicked += Expand_Clicked;
            Factorize.Clicked += Factorize_Clicked;
        }

#if true
        async Task Eval(string Format)
        { 
            try
            {
                //Android needs .toString() and trim " while Windows 10 does not
                Out.Text = (await (await Current).EvaluateNoReturn(string.Format(Format, In.Text.Replace("'", "\\'").Replace("\\", "\\\\")))).Trim('"');
            }
            catch (Jint.Runtime.JavaScriptException ex)
            {
                Out.Text = Utils.Error + ex.Message;
            }
            NextEngine();
        }
        async void Evaluate_Clicked(object sender, EventArgs e) => await Eval("nerdamer('{0}').toString()");
        async void Expand_Clicked(object sender, EventArgs e) => await Eval("nerdamer.expand('{0}').toString()");
        async void Factorize_Clicked(object sender, EventArgs e) => await Eval("nerdamer.factor('{0}').toString()");
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
        void Evaluate_Clicked(object sender, EventArgs e) => Eval(x => x, Infix.Format);
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