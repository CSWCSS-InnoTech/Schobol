using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using static InnoTecheLearning.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//[assembly: XamlCompilation (XamlCompilationOptions.Compile)]

namespace InnoTecheLearning
{
	public class App : Application
    {
        public App ()
		{
            Region = "App";
            Log("App strated");
            // The root page of your application
            MainPage = new Main();//ResourceView;
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
        public ContentPage ResourceView
        {
            get
            {
               return new ContentPage { BackgroundColor = Color.White,
                    Content = new Label { Text = GetResources(), TextColor = Color.Black } };
            }
        }
        public string GetResources()
        {
            string Return = "";
            foreach (var s in GetType().GetTypeInfo().Assembly.GetManifestResourceNames())
            {
                Return += s + '\n';
            }
            return Return;
        }
    }
}
