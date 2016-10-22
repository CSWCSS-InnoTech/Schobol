using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Text;

namespace eLearning.Droid
{
	[Activity (Label = "CSWCSS eLearning App", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button Button = FindViewById<Button> (Resource.Id.myButton);
			Button.Click += delegate {
				Button.Text = string.Format ("{0} clicks!", count++);
            };
            TextView Title = FindViewById<TextView>(Resource.Id.Title);
            Title.TextFormatted = HTMLMarkup(Resource.String.app_name);
            Title.TextAlignment = TextAlignment.Center;
            TextView InnoTech = FindViewById<TextView>(Resource.Id.InnoTech);
            InnoTech.TextFormatted = HTMLMarkup(Resource.String.innotech);
            InnoTech.TextAlignment = TextAlignment.Center;
		}

        public ISpanned HTMLMarkup(int ResourceId)
        { return Html.FromHtml(Resources.GetString(ResourceId).ToString()); }

        public string GetVersionName()
        {
            return Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0).VersionName;
        }
        public int GetVersionCode()
        {
            return Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Application.Context.ApplicationContext.PackageName, 0).VersionCode;
        }
    }
}


