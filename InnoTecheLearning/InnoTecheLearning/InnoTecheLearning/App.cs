#undef DEBUG_WRITE
#undef DEBUG_INTERACTIVE

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
                var In = new Editor { VerticalOptions = LayoutOptions.FillAndExpand };
                var Execute = new Button
                { Text = "Execute", HorizontalOptions = LayoutOptions.FillAndExpand };
                var Reset = new Button { Text = "Reset" };
                var Out = new Entry();
                Execute.Clicked += async (sender, e) =>
                { Out.ClearValue(Entry.TextProperty); Out.Text = await this.Execute(In.Text); };
                Reset.Clicked += async (sender, e) => await this.Reset();
                return new ContentPage
                {
                    Content = new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        Children =
                        {
                            In,
                            new StackLayout
                            { Orientation = StackOrientation.Horizontal, Children = { Execute, Reset } },
                            Out
                        }
                    }
                };
            }
        }

        Android.Webkit.WebView _View = new Android.Webkit.WebView(Forms.Context);
        public System.Threading.Tasks.Task<string> Execute(string Input)
        {
            _View.Settings.JavaScriptEnabled = true;
            var Completion = new System.Threading.Tasks.TaskCompletionSource<string>();
            Device.BeginInvokeOnMainThread(() =>
                _View.EvaluateJavascript(Input, new Callback(s => Completion.SetResult(s))));
            return Completion.Task;
        }
        public System.Threading.Tasks.Task Reset()
        { _View = new Android.Webkit.WebView(Forms.Context); return System.Threading.Tasks.Task.CompletedTask; }
        class Callback : Java.Lang.Object, Android.Webkit.IValueCallback
        {
            System.Action<string> _Callback;
            public Callback(System.Action<string> Callback) => _Callback = Callback;
            public void OnReceiveValue(Java.Lang.Object value) => 
                _Callback(Android.Runtime.Extensions.JavaCast<Java.Lang.String>(value).ToString());
        }
#endif
    }
}
