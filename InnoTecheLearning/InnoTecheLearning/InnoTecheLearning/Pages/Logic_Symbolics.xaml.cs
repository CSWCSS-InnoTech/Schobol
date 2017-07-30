using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//using MathNet.Symbolics;

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
        async void Evaluate_Clicked(object sender, EventArgs e) => await Eval("nerdamer('{0}').toString()");
        async void Expand_Clicked(object sender, EventArgs e) => await Eval("nerdamer('{0}').toString()");
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
        void Evaluate_Clicked(object sender, EventArgs e)
        {
            switch (MathNet.Symbolics.Infix.Parse(In.Text))
            {
                case MathNet.Symbolics.ParseResult.ParsedExpression Success:
                    Out.Text = Success.Item.ToString();
                    break;
                case MathNet.Symbolics.ParseResult.ParseFailure Fail:
                    Out.Text = Utils.Error + Fail.Item;
                    break;
                default:
                    break;
            }
        }
        void Expand_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        //EventHandler Eval(Func<Expression, Expression> Func) => (sender, e) => Out.Text = Func(Infix.ParseOrUndefined(In.Text)).ToString();
#endif
    }
}