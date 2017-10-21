using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InnoTecheLearnUtilities.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Main : ContentPage
	{
		public Main ()
		{
			InitializeComponent ();
            Lingual.Source = Utils.Create.ImageSource(Utils.Create.ImageFile.Translate);
            Logic.Source = Utils.Create.ImageSource(Utils.Create.ImageFile.Calculator);
            Health.Source = Utils.Create.ImageSource(Utils.Create.ImageFile.Sports);
            Tunes.Source = Utils.Create.ImageSource(Utils.Create.ImageFile.MusicTuner);
            Excel.Source = Utils.Create.ImageSource(Utils.Create.ImageFile.MathSolver);
            Facial.Source = Utils.Create.ImageSource(Utils.Create.ImageFile.Facial);
        }
	}
}