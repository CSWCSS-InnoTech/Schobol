using InnoTecheLearning;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//[assembly: Dependency(typeof(Utils))]

namespace InnoTecheLearning
{/// <summary>
/// A class that provides methods to help run the App.
/// </summary>
    public static partial class Utils
    {   /// <summary>
        /// Which project is the app built in?
        /// </summary>
        public static ProjectType Project
        {
            get
            {
#if __ANDROID__
                return ProjectType.Android;
#elif __IOS__
                return ProjectType.iOS;
#elif WINDOWS_UWP
                return ProjectType.UWP10;
#elif WINDOWS_APP
                return ProjectType.Win81;
#elif WINDOWS_PHONE_APP
                return ProjectType.WinPhone81;
#else
                return ProjectType.Undefined;
#endif
            }
        }
        /// <summary>
        /// All project types.
        /// </summary>
        public enum ProjectType : sbyte
        {
            Undefined = -1,
            iOS,
            Android,
            UWP10,
            WinPhone81,
            Win81
        }
#if !(WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP)
        public class FileIO
        {
            public FileIO(string FileName,FileMode Mode = FileMode.Create)
            {
                this.FileName = FileName;
                FileStream = new System.IO.IsolatedStorage.IsolatedStorageFileStream(FileName, Mode);
            }
            //var a = new FileImageSourceConverter();
            //var uri = new Image().Source.GetValue(UriImageSource.UriProperty);
            public string FileName { get; }
            public string FilePath { get; }
            public System.IO.IsolatedStorage.IsolatedStorageFileStream FileStream { get; }
            public int Read(byte[] Buffer,int Offset, int Count)
            { return FileStream.Read(Buffer, Offset, Count); }
            public void Write(byte[] Buffer, int Offset, int Count)
            { FileStream.Write(Buffer, Offset, Count); }
            public void Dispose(bool Delete = false)
            { FileStream.Dispose();
              if (Delete)
                  File.Delete(FilePath); }
            ~FileIO()
            { try { Dispose(); } catch { } }
        }
#endif
        /// <summary>
        /// Returns different values depending on the <see cref="ProjectType"/> <see cref="Xamarin.Forms"/> is working on.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the value to be returned.></typeparam>
        /// <param name="iOS">The value for an Apple <paramref name="iOS"/> OS.</param>
        /// <param name="Android">The value for a Google <paramref name="Android"/> OS.</param>
        /// <param name="Windows">The value for the <paramref name="Windows"/> platform.</param>
        /// <param name="WinPhone">The value for a Microsoft <paramref name="WinPhone"/> OS.</param>
        /// <param name="Default">The value to return if no value was provided for the current OS.</param>
        /// <returns>The value depending on the <see cref="ProjectType"/> <see cref="Xamarin.Forms"/> is working on.</returns>
        public static T OnPlatform<T>(Func<T> iOS = null, Func<T> Android = null,
            Func<T> Windows = null, Func<T> WinPhone = null, Func<T> Default = null)
        {
            switch (Device.OS)
            {
                case TargetPlatform.iOS:
                    if(iOS != null)
                        return (T)iOS.DynamicInvoke();
                    break;
                case TargetPlatform.Android:
                    if(Android != null)
                        return (T)Android.DynamicInvoke();
                    break;
                case TargetPlatform.WinPhone:
                    if(WinPhone != null)
                        return (T)WinPhone.DynamicInvoke();
                    break;
                case TargetPlatform.Windows:
                    if(Windows != null)
                        return (T)Windows.DynamicInvoke();
                    break;
                case TargetPlatform.Other:
                default:
                    break;
            }
            return Default == null ? default(T) : (T)Default.DynamicInvoke();
        }
    /// <summary>
    /// Returns different values depending on the <see cref="ProjectType"/> <see cref="Xamarin.Forms"/> is working on.
    /// </summary>
    /// <typeparam name="T">The <see cref="Type"/> of the value to be returned.></typeparam>
    /// <param name="iOS">The value for an Apple <paramref name="iOS"/> OS.</param>
    /// <param name="Android">The value for a Google <paramref name="Android"/> OS.</param>
    /// <param name="Windows">The value for the <paramref name="Windows"/> platform.</param>
    /// <param name="WinPhone">The value for a Microsoft <paramref name="WinPhone"/> OS.</param>
    /// <param name="Default">The value to return if no value was provided for the current OS.</param>
    /// <returns>The value depending on the <see cref="ProjectType"/> <see cref="Xamarin.Forms"/> is working on.</returns>
    public static T OnPlatform<T>(T iOS = default(T), T Android = default(T), T Windows = default(T),
                                  T WinPhone = default(T), T Default = default(T))
    {
        switch (Device.OS)
        {
            case TargetPlatform.iOS:
                if (!iOS.Equals(default(T)))
                    return iOS;
                break;
            case TargetPlatform.Android:
                if (!Android.Equals(default(T)))
                    return Android;
                break;
            case TargetPlatform.WinPhone:
                if (!WinPhone.Equals(default(T)))
                    return WinPhone;
                break;
            case TargetPlatform.Windows:
                if (!Windows.Equals(default(T)))
                    return Windows;
                break;
            case TargetPlatform.Other:
            default:
                break;
        }
        return Default;
    }
        public static T OnProject<T>(Func<T> iOS = null, Func<T> Android = null, Func<T> UWP10 = null,
            Func<T> Win81 = null, Func<T> WinPhone81 = null, Func<T> Default = null)
        {
            switch (Project)
            {
                case ProjectType.iOS:
                    if (iOS != null)
                        return (T)iOS.DynamicInvoke();
                    break;
                case ProjectType.Android:
                    if (Android != null)
                        return (T)Android.DynamicInvoke();
                    break;
                case ProjectType.UWP10:
                    if (UWP10 != null)
                        return (T)UWP10.DynamicInvoke();
                    break;
                case ProjectType.Win81:
                    if (Win81 != null)
                        return (T)Win81.DynamicInvoke();
                    break;
                case ProjectType.WinPhone81:
                    if (WinPhone81 != null)
                        return (T)WinPhone81.DynamicInvoke();
                    break;
                case ProjectType.Undefined:
                default:
                    break;
            }
            return Default == null ? default(T) : (T)Default.DynamicInvoke();
        }
        public static T OnProject<T>(T iOS = default(T), T Android = default(T), T UWP10 = default(T),
                                      T Win81 = default(T), T WinPhone81 = default(T), T Default = default(T))
        {
            switch (Project)
            {
                case ProjectType.iOS:
                    if (!iOS.Equals(default(T)))
                        return iOS;
                    break;
                case ProjectType.Android:
                    if (!Android.Equals(default(T)))
                        return Android;
                    break;
                case ProjectType.UWP10:
                    if (!UWP10.Equals(default(T)))
                        return UWP10;
                    break;
                case ProjectType.Win81:
                    if (!Win81.Equals(default(T)))
                        return Win81;
                    break;
                case ProjectType.WinPhone81:
                    if (!WinPhone81.Equals(default(T)))
                        return WinPhone81;
                    break;
                case ProjectType.Undefined:
                default:
                    break;
            }
            return Default;
        }

        public static string CurrentNamespace
        {   get {return "InnoTecheLearning." + OnProject("iOS", "Droid", "UWP", "Windows", "WinPhone");}}

        public async static Task<T> AlertAsync<T>(T Return,Page Page, Text Message = default(Text),
                                                  string Title = "Alert", string Cancel = "OK")
        {   await Page.DisplayAlert(Title, Message, Cancel);
            return Return; }

        public static T Alert<T>(T Return, Page Page,  Text Message = default(Text), string Title = "Alert", string Cancel = "OK")
        {  return Do(AlertAsync(Return, Page, Title, Message, Cancel)); }

        public async static void Alert(Page Page, Text Message = default(Text), string Title = "Alert", string Cancel = "OK")
        {
            await Page.DisplayAlert(Title, Message, Cancel);
        }

        /// <summary>
        /// Making formatted text is #Ez.
        /// </summary>
        /// <param name="Spans">Obviously you will need text to format.</param>
        /// <returns></returns>
        public static FormattedString Format(params Span[] Spans)
        {
            FormattedString fs = new FormattedString();
            foreach (Span Span in Spans)
            {
                fs.Spans.Add(Span);
            }
            /*fs.Spans.Add(new Span { Text = "First ", ForegroundColor = Color.Red, FontSize = 14 });
            fs.Spans.Add(new Span { Text = " second ", ForegroundColor = Color.Blue, FontSize = 28 });
            fs.Spans.Add(new Span { Text = " third.", ForegroundColor = Color.Yellow, FontSize = 14 });*/
            return fs;
        }

        /// <summary>
        /// Returns bolded <see cref="Text"/>.
        /// </summary>
        /// <param name="Text"><see cref="Text"/> to make bold.</param>
        /// <returns></returns>
        public static Span Bold(Text Text)
        { return new Span { Text = Text, FontAttributes = FontAttributes.Bold }; }
        /// <summary>
        /// Returns a <see cref="string"/> consisting of the specified
        /// <see cref="char"/> repeated the specified number of times.
        /// </summary>
        /// <param name="Char">The <see cref="char"/> that you want to duplicate. </param>
        /// <param name="Count">Number of times to duplicate the <see cref="char"/>.</param>
        /// <returns>Returns a <see cref="string"/> consisting of the specified
        /// <see cref="char"/> repeated the specified number of times. </returns>
        public static string StrDup(char Char, int Count)
        { return new string(Char, Count); }
        /// <summary>
        /// Returns a <see cref="string"/> consisting of the specified
        /// <see cref="string"/> repeated the specified number of times.
        /// </summary>
        /// <param name="String">The <see cref="string"/> that you want to duplicate. </param>
        /// <param name="Count">Number of times to duplicate the <see cref="string"/>.</param>
        /// <returns>Returns a <see cref="string"/> consisting of the specified
        /// <see cref="string"/> repeated the specified number of times. </returns>
        public static string StrDup(string String, int Count)
        { string Return = "";
          for (int i = 0; i < Count; i++)
                Return += String;
          return Return;}
        /// <summary>
        /// Trys to convert an <see cref="object"/> instance to a specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to convert to.</typeparam>
        /// <param name="Object">The <see cref="object"/> instance to convert.</param>
        /// <param name="Result">The result of conversion if successful. If not it will be the default value of the <see cref="Type"/> to convert to.</param>
        /// <returns>Whether the conversion has succeeded.</returns>
        public static bool TryCast<T>(dynamic Object, out T Result)
        {
            try
            {
                Result = (T)Object;
                return true;
            }
            catch (InvalidCastException)
            {
                Result = default(T);
                return false;
            }
        }

        public static char[] CharGen(char Start, char End, params char[] Exclude)
        {   string Return = "";
            for (char i = Start; i < End+1; i++)
            {
                if (Array.Exists(Exclude,x=>x!=i))
                Return += i;
            }
            return Return.ToCharArray();
        }

        public static void DoNothing(params dynamic[] Params)
        { }

        public static T Return<T>(T Return)
        { return Return; }

        public static T Assign<T>(T Value, out T Object)
        { return Object = Value; }

        public static T Do<T>(Task<T> Task)
        {
            return Task.GetAwaiter().GetResult();
        }

        public static void Do(Task Task)
        {
            using (AsyncHelper.AsyncBridge Helper = AsyncHelper.Wait)
                Helper.Run(Task);
        } 
        /*
        public string TransformForCurrentPlatform(string url)
        {
            string result = ArgumentValidator.AssertNotNull(url, "url");

            if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
            {
                const string filePrefix = "file:///";

                if (url.StartsWith(filePrefix))
                {
                    result = url.Substring(filePrefix.Length);
                }

                result = result.Replace("/", "_").Replace("\\", "_");

                if (result.StartsWith("_") && result.Length > 1)
                {
                    result = result.Substring(1);
                }
            }
            else if (Device.OS == TargetPlatform.WinPhone)
            {
                if (url.StartsWith("/") && url.Length > 1)
                {
                    result = result.Substring(1);
                }
            }

            return result;
        }
        [ContentProperty("Source")]
        public class ImageResourceExtension : IMarkupExtension
        {
            public string Source { get; set; }

            public object ProvideValue(IServiceProvider serviceProvider)
            {
                if (Source == null)
                {
                    return null;
                }

                ImageSource imageSource = null;

                var transformer = Dependency.Resolve<IImageUrlTransformer, ImageUrlTransformer>(true);
                string url = transformer.TransformForCurrentPlatform(Source);

                if (Device.OS == TargetPlatform.Android)
                {
                    imageSource = ImageSource.FromFile(url);
                }
                else if (Device.OS == TargetPlatform.iOS)
                {
                    imageSource = ImageSource.FromFile(url);
                }
                else if (Device.OS == TargetPlatform.WinPhone)
                {
    #if WINDOWS_PHONE
        if (url.StartsWith("/") && url.Length > 1)
        {
            url = url.Substring(1);
        }

        var stream = System.Windows.Application.GetResourceStream(new Uri(url, UriKind.Relative));

        if (stream != null)
        {
            imageSource = ImageSource.FromStream(() => stream.Stream);
        }
        else
        {
            ILog log;
            if (Dependency.TryResolve<ILog>(out log))
            {
               log.Debug("Unable to located create ImageSource using URL: " + url);
            }
        }
    #endif
                }

                if (imageSource == null)
                {
                    imageSource = ImageSource.FromFile(url);
                }

                return imageSource;
            }
        }*/
    }
}

