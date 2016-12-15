#if __ANDROID__ || WINDOWS_UWP
using Callback = System.Action<InnoTecheLearning.Utils.Updater.UpdateProgress>;
using Exception = System.Exception;
using System.Net;
#if __ANDROID__
using Android.Content;
using Xamarin.Forms;
using AndroUri = Android.Net.Uri;
using Environment = Android.OS.Environment;
using AndroLog = Android.Util.Log;
using Throwable = Java.Lang.Throwable;
using File = Java.IO.File;
using Path = System.IO.Path;
#elif WINDOWS_UWP
using Windows.Foundation;
using Windows.ApplicationModel;
using Windows.Management.Deployment;
using Uri = System.Uri;
#endif

namespace InnoTecheLearning
{
    partial class Utils
    {
        interface IUpdater
        { string NewestAlpha { get; } UpdateState CheckUpdate(); HttpStatusCode GetPage(string url); }
        public enum UpdateState : byte
        { Undefined, Success, Newest, NoNetwork, DownloadException, InstallException }
        public static string ToName(this Updater.UpdateProgress Progress)
        {
            if (Progress >= Updater.UpdateProgress.Downloading_0_Percent &&
                Progress <= Updater.UpdateProgress.Downloading_100_Percent)
                return $"Downloading ({Progress - Updater.UpdateProgress.Downloading_0_Percent}%)";
            return Progress.ToString().Replace('_', ' ');
        }
        public static byte Percentage(this Updater.UpdateProgress Progress)
        {
            if (Progress <= Updater.UpdateProgress.Downloading_0_Percent) return 0;
            if (Progress >= Updater.UpdateProgress.Downloading_100_Percent) return 100;
            return Progress - Updater.UpdateProgress.Downloading_0_Percent;
        }
        public class Updater : IUpdater
        {
            public bool Is404(string url) { return GetPage(url) == HttpStatusCode.NotFound; }
            public event Callback Progress;
            public Updater(Callback Action)
            { Progress += Action; }

            public enum UpdateProgress : byte
            {
                Checking_Internet = 0,
                Getting_Downloads_Folder = 1,
                Checking_Newest_Update = 2,
                Downloading = 3,
                #region Downloading Percent
                Downloading_0_Percent = 100,
                Downloading_1_Percent = 101,
                Downloading_2_Percent = 102,
                Downloading_3_Percent = 103,
                Downloading_4_Percent = 104,
                Downloading_5_Percent = 105,
                Downloading_6_Percent = 106,
                Downloading_7_Percent = 107,
                Downloading_8_Percent = 108,
                Downloading_9_Percent = 109,
                Downloading_10_Percent = 110,
                Downloading_11_Percent = 111,
                Downloading_12_Percent = 112,
                Downloading_13_Percent = 113,
                Downloading_14_Percent = 114,
                Downloading_15_Percent = 115,
                Downloading_16_Percent = 116,
                Downloading_17_Percent = 117,
                Downloading_18_Percent = 118,
                Downloading_19_Percent = 119,
                Downloading_20_Percent = 120,
                Downloading_21_Percent = 121,
                Downloading_22_Percent = 122,
                Downloading_23_Percent = 123,
                Downloading_24_Percent = 124,
                Downloading_25_Percent = 125,
                Downloading_26_Percent = 126,
                Downloading_27_Percent = 127,
                Downloading_28_Percent = 128,
                Downloading_29_Percent = 129,
                Downloading_30_Percent = 130,
                Downloading_31_Percent = 131,
                Downloading_32_Percent = 132,
                Downloading_33_Percent = 133,
                Downloading_34_Percent = 134,
                Downloading_35_Percent = 135,
                Downloading_36_Percent = 136,
                Downloading_37_Percent = 137,
                Downloading_38_Percent = 138,
                Downloading_39_Percent = 139,
                Downloading_40_Percent = 140,
                Downloading_41_Percent = 141,
                Downloading_42_Percent = 142,
                Downloading_43_Percent = 143,
                Downloading_44_Percent = 144,
                Downloading_45_Percent = 145,
                Downloading_46_Percent = 146,
                Downloading_47_Percent = 147,
                Downloading_48_Percent = 148,
                Downloading_49_Percent = 149,
                Downloading_50_Percent = 150,
                Downloading_51_Percent = 151,
                Downloading_52_Percent = 152,
                Downloading_53_Percent = 153,
                Downloading_54_Percent = 154,
                Downloading_55_Percent = 155,
                Downloading_56_Percent = 156,
                Downloading_57_Percent = 157,
                Downloading_58_Percent = 158,
                Downloading_59_Percent = 159,
                Downloading_60_Percent = 160,
                Downloading_61_Percent = 161,
                Downloading_62_Percent = 162,
                Downloading_63_Percent = 163,
                Downloading_64_Percent = 164,
                Downloading_65_Percent = 165,
                Downloading_66_Percent = 166,
                Downloading_67_Percent = 167,
                Downloading_68_Percent = 168,
                Downloading_69_Percent = 169,
                Downloading_70_Percent = 170,
                Downloading_71_Percent = 171,
                Downloading_72_Percent = 172,
                Downloading_73_Percent = 173,
                Downloading_74_Percent = 174,
                Downloading_75_Percent = 175,
                Downloading_76_Percent = 176,
                Downloading_77_Percent = 177,
                Downloading_78_Percent = 178,
                Downloading_79_Percent = 179,
                Downloading_80_Percent = 180,
                Downloading_81_Percent = 181,
                Downloading_82_Percent = 182,
                Downloading_83_Percent = 183,
                Downloading_84_Percent = 184,
                Downloading_85_Percent = 185,
                Downloading_86_Percent = 186,
                Downloading_87_Percent = 187,
                Downloading_88_Percent = 188,
                Downloading_89_Percent = 189,
                Downloading_90_Percent = 190,
                Downloading_91_Percent = 191,
                Downloading_92_Percent = 192,
                Downloading_93_Percent = 193,
                Downloading_94_Percent = 194,
                Downloading_95_Percent = 195,
                Downloading_96_Percent = 196,
                Downloading_97_Percent = 197,
                Downloading_98_Percent = 198,
                Downloading_99_Percent = 199,
                Downloading_100_Percent = 200,
                #endregion
                Installing = 255
            }
#if __ANDROID__
            const string Url = @"https://github.com/Happypig375/InnoTech-eLearning/blob/Versions/Android/";
            const string Download = @"https://github.com/Happypig375/InnoTech-eLearning/raw/Versions/Android/";
            const string ID = "InnoTecheLearning.Updater";
            public string NewestAlpha
            {
                get
                {
                    ModifiableVersion TestFor = Version;
                    while (!Is404(Url + TestFor.ToShort() + ".apk"))
                        TestFor.Major++;
                    TestFor.Major--;
                    while (!Is404(Url + TestFor.ToShort() + ".apk"))
                        TestFor.Minor++;
                    TestFor.Minor--;
                    while (!Is404(Url + TestFor.ToShort() + ".apk"))
                        TestFor.Build++;
                    TestFor.Build--;
                    while (!Is404(Url + TestFor.ToShort() + ".apk"))
                        TestFor.MajorRevision++;
                    TestFor.MajorRevision--;
                    while (!Is404(Url + TestFor.ToShort() + ".apk"))
                        TestFor.MinorRevision++;
                    TestFor.MinorRevision--;
                    return TestFor.ToShort();
                }
            }
            public UpdateState CheckUpdate()
            {
                Progress?.Invoke(UpdateProgress.Checking_Internet);
                if (!IsInternetAvailable) return UpdateState.NoNetwork;

                Progress?.Invoke(UpdateProgress.Getting_Downloads_Folder);
                string LocalDownload = Environment.GetExternalStoragePublicDirectory
                    (Environment.DirectoryDownloads).AbsolutePath;
                string DownloadLocation = "";
                try
                {
                    Progress?.Invoke(UpdateProgress.Checking_Newest_Update);
                    string NewestAlpha = this.NewestAlpha;
                    if (NewestAlpha == VersionShort) return UpdateState.Newest;
                    NewestAlpha += ".apk";
                    Progress?.Invoke(UpdateProgress.Downloading);
                    WebClient Client = new WebClient();
                    Client.DownloadProgressChanged += (object sender, DownloadProgressChangedEventArgs e) => 
                    { Progress((UpdateProgress)((byte)UpdateProgress.Downloading_0_Percent + e.ProgressPercentage)); };
                    Do(Client.DownloadFileTaskAsync(Download + NewestAlpha,
                        DownloadLocation = Path.Combine(LocalDownload, NewestAlpha)));
                }
                catch (Exception)
                {
                    return UpdateState.DownloadException;
                }
                try
                {
                    Progress?.Invoke(UpdateProgress.Installing);
                    //this installs the app onto the device,
                    Intent intent = new Intent();
                    intent.SetDataAndType(
                        AndroUri.FromFile(new File(DownloadLocation)), "application/vnd.android.package-archive"
                    );
                    Forms.Context.StartActivity(intent);
                    return UpdateState.Success;
                }
                catch (Exception)
                {
                    return UpdateState.InstallException;
                }
            }

            public HttpStatusCode GetPage(string url)
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
            public bool IsInternetAvailable
            {
                get
                {
                    try
                    {
                        Dns.GetHostEntry("www.github.com");
                        return true;
                    }
                    catch (System.Net.Sockets.SocketException)
                    { return false; }
                }
            }
#elif WINDOWS_UWP
            const string Url = @"https://github.com/Happypig375/InnoTech-eLearning/blob/Versions/UWP/";
            const string Download = @"https://github.com/Happypig375/InnoTech-eLearning/raw/Versions/UWP/";
            const string ID = "InnoTecheLearning.Updater";
            public string NewestAlpha
            {
                get
                {
                    ModifiableVersion TestFor = Version;
                    while (!Is404(Url + TestFor.ToShort() + ".appxbundle"))
                        TestFor.Major++;
                    TestFor.Major--;
                    while (!Is404(Url + TestFor.ToShort() + ".appxbundle"))
                        TestFor.Minor++;
                    TestFor.Minor--;
                    while (!Is404(Url + TestFor.ToShort() + ".appxbundle"))
                        TestFor.Build++;
                    TestFor.Build--;
                    while (!Is404(Url + TestFor.ToShort() + ".appxbundle"))
                        TestFor.MajorRevision++;
                    TestFor.MajorRevision--;
                    while (!Is404(Url + TestFor.ToShort() + ".appxbundle"))
                        TestFor.MinorRevision++;
                    TestFor.MinorRevision--;
                    return TestFor.ToShort();
                }
            }
            public UpdateState CheckUpdate()
            {
                Progress?.Invoke(UpdateProgress.Checking_Internet);
                if (!IsInternetAvailable) return UpdateState.NoNetwork;
                try
                {
                    Progress?.Invoke(UpdateProgress.Checking_Newest_Update);
                    string NewestAlpha = this.NewestAlpha;
                    if (NewestAlpha == VersionShort) return UpdateState.Newest;
                    NewestAlpha += ".appxbundle";
                    Uri updatePackage = new Uri(Download + NewestAlpha);

                    //this installs the app onto the device,
                    string packageFamilyName = Package.Current.Id.FamilyName;
                    string packageLocation = updatePackage.ToString();
                    PackageManager packagemanager = new PackageManager();
                    Progress?.Invoke(UpdateProgress.Downloading);
                    var Action = packagemanager.UpdatePackageAsync(new Uri(packageLocation), 
                        null, DeploymentOptions.ForceApplicationShutdown);
                    Action.Progress += (IAsyncOperationWithProgress<DeploymentResult, DeploymentProgress> result,
                        DeploymentProgress progress) => {
                            Progress?.Invoke((UpdateProgress)
                                ((byte)UpdateProgress.Downloading_0_Percent + progress.percentage)); };
                    return UpdateState.Success;
                }
                catch (Exception)
                {
                    return UpdateState.InstallException;
                }
            }
            public HttpStatusCode GetPage(string url)
            {
                // Creates an HttpWebRequest for the specified URL. 
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                // Sends the HttpWebRequest and waits for a response.
                using (HttpWebResponse myHttpWebResponse = (HttpWebResponse)Do(myHttpWebRequest.GetResponseAsync()))
                    return myHttpWebResponse.StatusCode;
            }
            public bool IsInternetAvailable
            {
                get
                {
                    try
                    {
                        Do(Dns.GetHostEntryAsync("www.github.com"));
                        return true;
                    }
                    catch (System.Net.Sockets.SocketException)
                    { return false; }
                }
            }
#endif
        }
    }
}
#endif