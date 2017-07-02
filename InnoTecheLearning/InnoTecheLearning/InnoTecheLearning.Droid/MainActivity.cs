﻿using System;

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
	[Activity (Label = "CSWCSS eLearn Utilities", Theme = "@style/Main", MainLauncher = false, 
        ScreenOrientation = ScreenOrientation.SensorPortrait, HardwareAccelerated = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static Bundle Bundle { get; internal set; }
        public static MainActivity Current { get; private set; }

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
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

    [Activity(Label = "CSWCSS eLearning App", Theme = "@style/Splash", MainLauncher = true, NoHistory = true,
        ScreenOrientation = ScreenOrientation.SensorPortrait,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class SplashActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        //static System.Threading.ManualResetEvent _Ready = new System.Threading.ManualResetEvent(false);
        //public static void Ready() => _Ready.Set();
        //static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Exceptions.RegisterHandlers();
            MainActivity.Bundle = bundle;
            Utils.Unit.InvokeAsync(() =>
            {
                global::Xamarin.Forms.Forms.Init(this, MainActivity.Bundle);
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
            //if (!Xamarin.Forms.Forms.IsInitialized) Xamarin.Forms.Forms.Init(this, MainActivity.Bundle);
            base.OnPause();
        }
    }
}

