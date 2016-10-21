using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Reflection.Emit;

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
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
				button.Text = string.Format ("{0} clicks!", count++);

                Label labelBoldItalic = new Label();
                var fs = new FormattedString();
                fs.Spans.Add(new Span { Text = "bold", Font = Font.SystemFontOfSize(14, FontAttributes.Bold) });
                fs.Spans.Add(new Span { Text = " italic", Font = Font.SystemFontOfSize(14, FontAttributes.Italic) });
                labelBoldItalic.FormattedText = fs;
            };
		}

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


