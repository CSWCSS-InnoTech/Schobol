using System;

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Preferences;
using Android.Content;

namespace InnoTecheLearnUtilities.Droid
{
	[Activity (Label = Utils.AssemblyTitle, Theme = "@style/Main", MainLauncher = false, 
        ScreenOrientation = ScreenOrientation.SensorPortrait, HardwareAccelerated = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static Bundle Bundle { get; internal set; }
        public static MainActivity Current { get; private set; }

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            Xamarin.Forms.Forms.Init(this, bundle); //Sometimes Android bypasses the slash activity so be safe
            LoadApplication(new App());
            Current = this;
		}
        public event EventHandler<PreferenceManager.ActivityResultEventArgs> ActivityResult = delegate { };
        
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            ActivityResult(this, new PreferenceManager.ActivityResultEventArgs(true, requestCode, resultCode, data));
        }

       
    }

    [Activity(Label = Utils.AssemblyTitle, Theme = "@style/Splash", MainLauncher = true, NoHistory = true,
        ScreenOrientation = ScreenOrientation.SensorPortrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        System.Threading.ManualResetEvent Inited = new System.Threading.ManualResetEvent(false);
        protected override void OnCreate(Bundle bundle)
        {
            Exceptions.RegisterHandlers();
            Inited.Reset();
            base.OnCreate(bundle);
            MainActivity.Bundle = bundle;
            Utils.Unit.InvokeAsync(() =>
            {
                Xamarin.Forms.Forms.Init(this, MainActivity.Bundle);
                Inited.Set();
                StartActivity(new Intent(BaseContext, typeof(MainActivity)));
            });
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
        }

        protected override void OnPause()
        {
            Inited.WaitOne();
            base.OnPause();
        }
    }
}

