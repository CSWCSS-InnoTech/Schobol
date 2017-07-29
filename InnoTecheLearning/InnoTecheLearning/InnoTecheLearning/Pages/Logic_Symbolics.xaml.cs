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


        }

#if WINDOWS_UWP
        async void Evaluate_Clicked(object sender, EventArgs e)
        {
            try
            {
                Out.Text = (await Current).Execute($"nerdamer('{In.Text.Replace("'", "\\'")}').toString()")
                    .GetCompletionValue().ToString();
            }
            catch (Jint.Runtime.JavaScriptException ex)
            {
                Out.Text = Utils.Error + ex.Message;
            }
            NextEngine();
        }
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
        //EventHandler Eval(Func<Expression, Expression> Func) => (sender, e) => Out.Text = Func(Infix.ParseOrUndefined(In.Text)).ToString();
#endif
    }
}