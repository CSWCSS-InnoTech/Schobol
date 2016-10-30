using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]

namespace InnoTecheLearning
{
	public class App : Application
    {
        public static Size ScreenSize { get; set; }
        public App ()
		{
            // The root page of your application
            MainPage = new Main();
            MainPage.DisplayAlert(GetResources(),"Alert", "OK");
		}

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}

        public string GetResources()
        {
            string Return = "";
            foreach (var s in
       typeof(Main).Assembly.GetManifestResourceNames())
            {
                Return += s;
            }
            return Return;
        }
	}
}
