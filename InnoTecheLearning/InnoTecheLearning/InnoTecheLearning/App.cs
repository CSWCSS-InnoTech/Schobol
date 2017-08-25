#undef DEBUG_WRITE
#define DEBUG_INTERACTIVE

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
#if DEBUG_WRITE
            MainPage = ResourceView;
#elif DEBUG_INTERACTIVE
            MainPage = InteractiveView;
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
#if DEBUG_WRITE
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
            var x = 1073741826.0;
            return ((int)(uint)x).ToString();
            return Jint.Runtime.TypeConverter.ToInt32(new Jint.Native.JsValue(1073741826.0)).ToString();
            string Return = "";
            foreach (var s in System.Reflection.IntrospectionExtensions.GetTypeInfo(GetType())
                .Assembly.GetManifestResourceNames())
            {
                Return += s + '\n';
            }
            return Return;
        }
#elif DEBUG_INTERACTIVE
        public ContentPage InteractiveView
        {
            get
            {
                Android.Webkit.WebView _View = new Android.Webkit.WebView(Forms.Context);
                var In = new Editor();
                var Execute = new Button
                {
                    Text = "Execute"
                };
                var Out = new Entry();
                return new ContentPage
                {
                    Content = new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        Content = new Label { Text = GetResources(), TextColor = Color.Black }
                    }
                };
            }
        }
#endif
    }
}
