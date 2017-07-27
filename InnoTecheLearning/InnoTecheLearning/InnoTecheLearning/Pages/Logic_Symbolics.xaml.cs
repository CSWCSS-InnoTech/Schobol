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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Logic_Symbolics : ContentPage
    {
        Task<Jint.Engine> Current = CreateEngineAsync();
        Task<Jint.Engine> Next = CreateEngineAsync();

        public Logic_Symbolics()
        {
            InitializeComponent();

            Evaluate.Clicked += async (sender, e) =>
            {
                try
                {
                    Out.Text = (await Current).Execute($"nerdamer(\"{In.Text.Replace("\"", "\\\"")}\")")
                        .GetCompletionValue().ToString();
                }
                catch (Jint.Runtime.JavaScriptException ex)
                {
                    Out.Text = Utils.Error + ex.Message;
                }
                NextEngine();
            };

        }

        public void NextEngine() { Current = Next; Next = CreateEngineAsync(); }

        public static Task<Jint.Engine> CreateEngineAsync() => Task.Run(() =>
        {
            var JSEngine = new Jint.Engine();
            JSEngine.Execute(Utils.Resources.GetString("nerdamer.core.js"));
            JSEngine.Execute(Utils.Resources.GetString("Algebra.js"));
            JSEngine.Execute(Utils.Resources.GetString("Calculus.js"));
            JSEngine.Execute(Utils.Resources.GetString("Solve.js"));
            JSEngine.Execute(Utils.Resources.GetString("Extra.js"));
            return JSEngine;
        });
        //EventHandler Eval(Func<Expression, Expression> Func) => (sender, e) => Out.Text = Func(Infix.ParseOrUndefined(In.Text)).ToString();
    }
}