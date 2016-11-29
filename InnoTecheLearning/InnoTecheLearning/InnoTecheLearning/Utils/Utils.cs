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
            catch (Exception ex)when(ex is InvalidCastException||ex is Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                Result = default(T);
                return false;
            }
        }

        /// <summary>
        /// Trys to convert an <see cref="object"/> instance to a specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to convert to.</typeparam>
        /// <param name="Object">The <see cref="object"/> instance to convert.</param>
        /// <returns>The result of conversion if successful. If not it will be the default value of the <see cref="Type"/> to convert to.</returns>
        public static T TryCast<T>(dynamic Object)
        {
            try
            {
                return (T)Object;
            }
            catch (Exception ex) when (ex is InvalidCastException || ex is Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                return default(T);
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
#if NETFX_CORE
        public static void Do(global::Windows.Foundation.IAsyncAction Task)
        {
            using (AsyncHelper.AsyncBridge Helper = AsyncHelper.Wait)
                Helper.Run(Task.AsTask());
        }

        public static T Do<T>(global::Windows.Foundation.IAsyncOperation<T> Task)
        {
            return Task.GetAwaiter().GetResult();
        }
#endif
        public static ushort ToUShort(string String)
        {
Retry:      try
            {
                return ushort.Parse(String);
            }
            catch (OverflowException)
            {
                try
                {
                    return ushort.Parse(String.Replace(System.Globalization.NumberFormatInfo.CurrentInfo.
                        NegativeSign, string.Empty).Replace("-", string.Empty).Remove(6));
                }
                catch (OverflowException)
                {
                    return ushort.Parse(String.Replace(System.Globalization.NumberFormatInfo.CurrentInfo.
                        NegativeSign, string.Empty).Replace("-", string.Empty).Remove(5));
                }
            }
            catch (ArgumentNullException)
            { return 0; }
            catch (FormatException)
            {
                for (int i = 0; i < String.Length; i++)
                    if (!char.IsDigit(String[i]))
                    { String = String.Remove(i, 1); i--;}
                goto Retry;
            }
        }
        public static string Evaluate(string Expression, Page Alert = null)
        {
            const string Prefix = @"function Abs (n) {return Math.abs(n); }
function Acos(n) { return Math.acos(n); }
function Asin (n) { return Math.asin(n); }
function Atan (n) { return Math.atan(n); }
function Atan2(y, x){ return Math.atan2(y, x); }
function Ceil(x){ return Math.ceil(x); }
function Cos(x){ return Math.cos(x); }
function Exp(x){ return Math.exp(x); }
function Floor(x){ return Math.floor(x); }
function Log(x){ return Math.log(x); }
function Max(arguments){ return Math.max.apply(this, arguments); }
function Min(arguments){ return Math.min.apply(this, arguments); }
function Pow(x, y){ return Math.pow(x,y); }
function Random(){ return Math.random(); }
function Round(x){ return Math.round(x); }
function Sin(x){ return Math.sin(x); }
function Sqrt(x){ return Math.sqrt(x); }
function Tan(x){ return Math.tan(x); }
function Factorial_(aNumber : int, recursNumber : int ) : double {
   // recursNumber keeps track of the number of iterations so far.
   if (aNumber == 0) {  // If the number is 0, its factorial is 1.
      return 1.;
   } else {
      if(recursNumber > 100) {
         throw(""Too many levels of recursion."");
      } else {  // Otherwise, recurse again.
         return (aNumber* Factorial_(aNumber - 1, recursNumber + 1));
      }
}
}

function Factorial(aNumber : int) : double {
   // Use type annotation to only accept numbers coercible to integers.
   // double is used for the return type to allow very large numbers to be returned.
   if(aNumber< 0) {
      throw(""Cannot take the factorial of a negative number."");
   } else {  // Call the recursive function.
      return  Factorial_(aNumber, 0);
   }
}
var π = Math.PI;
var e = Math.E;
var Root2 = Math.SQRT2;
var Root0_5 = Math.SQRT1_2;
var Ln2 = Math.LN2;
var Ln10 = Math.LN10;
var Log2e = Math.LOG2E;
var Log10e = Math.LOG10E;
"""";
";
            // Ask user to enter expression.
            Evaluator evaluator = new Evaluator();
            try
            {
                return evaluator.Eval(Prefix + Expression);
            }
            catch (Exception ex) when (Alert != null)
            {
                return "ERROR: "+ex.Message;
            }
        }
#if false
        static void Hi()
        {
            Type scriptType = Type.GetTypeFromCLSID(Guid.Parse("0E59F1D5-1FBE-11D0-8FF2-00A0D10038BC"));

            dynamic obj = Activator.CreateInstance(scriptType, false);
            obj.Language = "javascript";

            var res = obj.Eval("a=3; 2*a+32-Math.sin(6)");
        }
#endif
        public static void Try<TException>(Action Try, Action<TException> Catch = null, Func<bool> CatchFilter = null, Action Finally = null)where TException : Exception
        {
            try
            {
                Try();
            }
            catch (TException ex) when (CatchFilter != null && (CatchFilter == null? true : CatchFilter()))
            {
                 Catch(ex); 
            }
            finally
            {
                Finally?.Invoke();
            }

        }
        public static T Try<T, TException>(Func<T> Try, Func<TException,T> Catch = null, Func<bool> CatchFilter = null, Action Finally = null)where TException : Exception
        {
            try
            {
                return Try();
            }
            catch (TException ex) when (CatchFilter != null && (CatchFilter == null ? true : CatchFilter()) )
            {
                return Catch(ex); 
            }
            finally
            {
                Finally?.Invoke();
            }
        }
        public static void Try<TException1, TException2>(Action Try, Action<TException1> Catch1 = null, Func<bool> CatchFilter1 = null, Action<TException2> Catch2 = null, Func<bool> CatchFilter2 = null, Action Finally = null) where TException1 : Exception where TException2 : Exception
        {
            try
            {
                Try();
            }
            catch (TException1 ex) when (CatchFilter1 != null && (CatchFilter1 == null ? true : CatchFilter1()))
            {
                Catch1(ex);
            }
            catch (TException2 ex) when (CatchFilter2 != null && (CatchFilter2 == null ? true : CatchFilter2()))
            {
                Catch2(ex);
            }
            finally
            {
                Finally?.Invoke();
            }

        }
        public static T Try<T, TException1, TException2>(Func<T> Try, Func<TException1, T> Catch1 = null, Func<bool> CatchFilter1 = null, Func<TException2, T> Catch2 = null, Func<bool> CatchFilter2 = null, Action Finally = null) where TException1 : Exception where TException2 : Exception
        {
            try
            {
                return Try();
            }
            catch (TException1 ex) when (CatchFilter1 != null && (CatchFilter1 == null ? true : CatchFilter1()))
            {
                return Catch1(ex);
            }
            catch (TException2 ex) when (CatchFilter2 != null && (CatchFilter2 == null ? true : CatchFilter2()))
            {
                return Catch2(ex);
            }
            finally
            {
                Finally?.Invoke();
            }
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

