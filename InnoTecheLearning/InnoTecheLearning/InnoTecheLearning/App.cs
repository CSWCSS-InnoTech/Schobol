#undef DEBUG_RESOURCES

using static InnoTecheLearning.Utils;
using Xamarin.Forms;

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
#if DEBUG_RESOURCES
            MainPage = ResourceView;
#else
            MainPage = new Main();
#endif
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
#if DEBUG_RESOURCES
        public ContentPage ResourceView
        {
            get
            {
               return new ContentPage
               {
                   BackgroundColor = Color.White,
                   Content = new ScrollView { Orientation = ScrollOrientation.Both,
                       Content = new Label { Text = GetResources(), TextColor = Color.Black } }
               };
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
#endif
    }
}
