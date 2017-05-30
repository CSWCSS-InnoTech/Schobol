using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using Android.Content;

namespace InnoTecheLearning.Droid
{
	[Activity (Label = "CSWCSS eLearning App", Icon = "@drawable/icon", MainLauncher = false, 
        ScreenOrientation = ScreenOrientation.SensorPortrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static Bundle Bundle { get; private set; }
        public static MainActivity Current { get; private set; }

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			global::Xamarin.Forms.Forms.Init (this, bundle);
            Bundle = bundle;
			LoadApplication (new App());
            Current = this;
		}
        public event EventHandler<PreferenceManager.ActivityResultEventArgs> ActivityResult = delegate { };
        
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            ActivityResult(this, new PreferenceManager.ActivityResultEventArgs(true, requestCode, resultCode, data));
        }
    }

    [Activity(Label = "CSWCSS eLearning App", Theme = "Splash", MainLauncher = true, NoHistory = true,
        ScreenOrientation = ScreenOrientation.SensorPortrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        static System.Threading.ManualResetEvent _Ready = new System.Threading.ManualResetEvent(false);
        public static void Ready() => _Ready.Set();
        //static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        // Launches the startup task
        protected override async void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            await Utils.Unit.InvokeAsync(_Ready.WaitOne);
        }
    }
}

