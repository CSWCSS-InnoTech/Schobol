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
	[Activity (Label = "CSWCSS eLearning App", Icon = "@drawable/icon", MainLauncher = true,
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
}

