#if __ANDROID__
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Diagnostics;
//using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Uri = System.Uri;
using AndroUri = Android.Net.Uri;
using Environment = Android.OS.Environment;
using AndroLog = Android.Util.Log;
using Exception = System.Exception;

namespace InnoTecheLearning
{
    partial class Utils
    {
        class Updater
        {
            const string ID = "InnoTecheLearning.Updater";
            public string NewestAlpha
            {
                get
                {

                }
            }
            public void CheckUpdate()
            {
                HttpWebRequest Request = new HttpWebRequest(new Uri(@"http://xxxxx/downloads/app.test-signed.apk"));
                //this installs the app onto the device,
                Intent intent = new Intent();
                intent.SetDataAndType(
                    AndroUri.FromFile(
                        new File(Environment.ExternalStorageDirectory.Path + "/app.test-signed.apk")
                    ), "application/vnd.android.package-archive"
                );
                Forms.Context.StartActivity(intent);
            }
            public static HttpStatusCode GetPage(string url)
            {
                try
                {
                    // Creates an HttpWebRequest for the specified URL. 
                    HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    // Sends the HttpWebRequest and waits for a response.
                    using (HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse())
                        return myHttpWebResponse.StatusCode;
                }
                catch (WebException e)
                {
                    AndroLog.Debug(ID, Throwable.FromException(e), "WebException Raised. The following error occured");
                    throw;
                }
                catch (Exception e)
                {
                    AndroLog.Debug(ID, Throwable.FromException(e), "The following Exception was raised");
                    throw;
                }
            }
        }
    }
}
#endif