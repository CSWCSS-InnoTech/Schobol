using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Logic_Symbolics()
        {
            InitializeComponent();

            Evaluate.Clicked += Evaluate_Clicked;
            Expand.Clicked += Expand_Clicked;
            Factorize.Clicked += Factorize_Clicked;
        }

#if WINDOWS_UWP
        async Task Eval(string Format)
        { 
            try
            {
                Out.Text = (await Current).Execute(string.Format(Format, In.Text.Replace("'", "\\'")))
                    .GetCompletionValue().ToString();
            }
            catch (Jint.Runtime.JavaScriptException ex)
            {
                Out.Text = Utils.Error + ex.Message;
            }
            NextEngine();
        }
        async void Evaluate_Clicked(object sender, EventArgs e) => await Eval("nerdamer('{0}')");
        async void Expand_Clicked(object sender, EventArgs e) => await Eval("nerdamer.expand('{0}')");
        async void Factorize_Clicked(object sender, EventArgs e) => await Eval("nerdamer.factor('{0}')");
        Task<Jint.Engine> Current = CreateEngineAsync();
        Task<Jint.Engine> Next = CreateEngineAsync();
        void NextEngine() { Current = Next; Next = CreateEngineAsync(); }

        static Task<Jint.Engine> CreateEngineAsync() => Task.Run(() =>
        {
            var JSEngine = new Jint.Engine();
            JSEngine.Execute(Utils.Resources.GetString("nerdamer.core.js"));
            JSEngine.Execute(Utils.Resources.GetString("Algebra.js"));
            JSEngine.Execute(Utils.Resources.GetString("Calculus.js"));
            JSEngine.Execute(Utils.Resources.GetString("Solve.js"));
            JSEngine.Execute(Utils.Resources.GetString("Extra.js"));
            return JSEngine;
        });
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