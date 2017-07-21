using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MathNet.Symbolics;

namespace InnoTecheLearning.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Logic_Symbolics : ContentPage
	{
		public Logic_Symbolics ()
		{
			InitializeComponent ();

            AExpand.Clicked += Eval(Algebraic.Expand);

		}

        EventHandler Eval(Func<Expression, Expression> Func) => (sender, e) => Out.Text = Func(Infix.ParseOrUndefined(In.Text)).ToString();
    }
}