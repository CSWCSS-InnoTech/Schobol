using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

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
#if false && !(WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP)
        public class FileIO
        {
            public FileIO(string FileName, FileMode Mode = FileMode.Create)
            {
                this.FileName = FileName;
                FileStream = new System.IO.IsolatedStorage.IsolatedStorageFileStream(FileName, Mode);
            }
            //var a = new FileImageSourceConverter();
            //var uri = new Image().Source.GetValue(UriImageSource.UriProperty);
            public string FileName { get; }
            public string FilePath { get; }
            public System.IO.IsolatedStorage.IsolatedStorageFileStream FileStream { get; }
            public int Read(byte[] Buffer, int Offset, int Count)
            { return FileStream.Read(Buffer, Offset, Count); }
            public void Write(byte[] Buffer, int Offset, int Count)
            { FileStream.Write(Buffer, Offset, Count); }
            public void Dispose(bool Delete = false)
            {
                FileStream.Dispose();
                if (Delete)
                    File.Delete(FilePath);
            }
            ~FileIO()
            { try { Dispose(); } catch { } }
        }
#endif
        /// <summary>
        /// Returns different values depending on the <see cref="ProjectType"/> <see cref="Xamarin.Forms"/> is working on.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the value to be returned.</typeparam>
        /// <param name="iOS">The value for an Apple <paramref name="iOS"/> OS.</param>
        /// <param name="Android">The value for a Google <paramref name="Android"/> OS.</param>
        /// <param name="Windows">The value for the <paramref name="Windows"/> platform.</param>
        /// <param name="WinPhone">The value for a Microsoft <paramref name="WinPhone"/> OS.</param>
        /// <param name="Default">The value to return if no value was provided for the current OS.</param>
        /// <returns>The value depending on the <see cref="ProjectType"/> <see cref="Xamarin.Forms"/> is working on.</returns>
        public static T OnPlatform<T>(Func<T> iOS = null, Func<T> Android = null,
            Func<T> Windows = null, Func<T> WinPhone = null, Func<T> Default = null)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    if (iOS != null)
                        return (T)iOS.DynamicInvoke();
                    break;
                case Device.Android:
                    if (Android != null)
                        return (T)Android.DynamicInvoke();
                    break;
                case Device.WinPhone:
                    if (WinPhone != null)
                        return (T)WinPhone.DynamicInvoke();
                    break;
                case Device.Windows:
                    if (Windows != null)
                        return (T)Windows.DynamicInvoke();
                    break;
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
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    if (!iOS.Equals(default(T)))
                        return iOS;
                    break;
                case Device.Android:
                    if (!Android.Equals(default(T)))
                        return Android;
                    break;
                case Device.WinPhone:
                    if (!WinPhone.Equals(default(T)))
                        return WinPhone;
                    break;
                case Device.Windows:
                    if (!Windows.Equals(default(T)))
                        return Windows;
                    break;
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
        { get { return "InnoTecheLearning." + OnProject("iOS", "Droid", "UWP", "Windows", "WinPhone"); } }

        public async static Task<T> AlertAsync<T>(T Return, Page Page, Text Message = default(Text),
                                                  string Title = "Alert", string Cancel = "OK")
        {
            await Page.DisplayAlert(Title, Message, Cancel);
            return Return;
        }

        public static T Alert<T>(T Return, Page Page, Text Message = default(Text),
            string Title = "Alert", string Cancel = "OK")
        { return Do(AlertAsync(Return, Page, Title, Message, Cancel)); }

        public async static void Alert(Page Page, Text Message = default(Text),
            string Title = "Alert", string Cancel = "OK")
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
        {
            string Return = "";
            for (int i = 0; i < Count; i++)
                Return += String;
            return Return;
        }
        public static T[] Duplicate<T>(T Item, int Count)
        {
            T[] Return = new T[Count];
            for (int i = 0; i < Count; i++) Return[i] = Item;
            return Return;
        }
        public static T[] Duplicate<T>(Func<T> Creator, int Count)
        {
            T[] Return = new T[Count];
            for (int i = 0; i < Count; i++) Return[i] = Creator();
            return Return;
        }
        /// <summary>
        /// Trys to convert an <see cref="object"/> instance to a specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to convert to.</typeparam>
        /// <param name="Object">The <see cref="object"/> instance to convert.</param>
        /// <param name="Result">The result of conversion if successful.
        /// If not it will be the default value of the <see cref="Type"/> to convert to.</param>
        /// <returns>Whether the conversion has succeeded.</returns>
        public static bool TryCast<T>(object Object, out T Result)
        {
            try
            {
                Result = (T)Object;
                return true;
            }
            catch (Exception)
            //when(ex is InvalidCastException || ex is Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
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
        /// <returns>The result of conversion if successful. If not it will be the default value of
        /// the <see cref="Type"/> to convert to.</returns>
        public static T TryCast<T>(object Object)
        {
            try
            {
                return (T)Object;
            }
            catch (Exception)
            //when (ex is InvalidCastException || ex is Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                return default(T);
            }
        }
        /// <summary>
        /// Trys to convert an <see cref="object"/> instance to a specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to convert to.</typeparam>
        /// <param name="Object">The <see cref="object"/> instance to convert.</param>
        /// <param name="Default">The result of conversion if failed.</param>
        /// <param name="Result">The result of conversion if successful.
        /// If not it will be <paramref name="Default"/>.</param>
        /// <returns>Whether the conversion has succeeded.</returns>
        public static bool TryCast<T>(object Object, T Default, out T Result)
        {
            try
            {
                Result = (T)Object;
                return true;
            }
            catch (Exception)
            //when(ex is InvalidCastException || ex is Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                Result = Default;
                return false;
            }
        }

        /// <summary>
        /// Trys to convert an <see cref="object"/> instance to a specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to convert to.</typeparam>
        /// <param name="Object">The <see cref="object"/> instance to convert.</param>
        /// <param name="Default">The result of conversion if failed.</param>
        /// <returns>The result of conversion if successful. If not it will be <paramref name="Default"/>.</returns>
        public static T TryCast<T>(object Object, T Default)
        {
            try
            {
                return (T)Object;
            }
            catch (Exception)
            //when (ex is InvalidCastException || ex is Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
            {
                return Default;
            }
        }

        public static char[] CharGen(char Start, char End, params char[] Exclude)
        {
            string Return = "";
            for (char i = Start; i < End + 1; i++)
            {
                if (Array.Exists(Exclude, x => x != i))
                    Return += i;
            }
            return Return.ToCharArray();
        }

        public static void DoNothing(params object[] Params)
        { }

        public static T Return<T>(T Return)
        { return Return; }

        public static T Return<T>(T Return, params object[] Params)
        { return Return; }

        public static T Assign<T>(T Value, out T Object)
        { return Object = Value; }

        public static ushort ToUShort(string String)
        {
            Retry: try
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
                    { String = String.Remove(i, 1); i--; }
                goto Retry;
            }
        }

        public static string Eval(string CodeToExecute)
        { return new Jint.Engine().Execute(CodeToExecute).GetCompletionValue().ToString(); }
        public static T Eval<T>(string CodeToExecute)
        { return (T)(new Jint.Engine().Execute(CodeToExecute).GetCompletionValue().ToObject()); }
        private static string JSEvaluteAns = "";
        private static string[] JSVariables = new string[26];
        public enum AngleMode : byte { Degree, Radian, Gradian, Turn }
        public enum Modifier : byte { Normal, Percentage, Mixed_Fraction, Fraction, AngleMeasure, IntSurd, FracSurd }
        public static Jint.Engine JSEngine = new Jint.Engine();
        public static string JSEvaluate(string Expression, Page Alert = null, AngleMode Mode = AngleMode.Radian,
           Modifier Mod = Modifier.Normal, bool TrueFree =  false)
        {
            string GetVars(params string[] Vars)
            {
                var sb = new System.Text.StringBuilder();
                for (int i = 0; i <= 25; i++)
                    sb.Append($"var Var{((char)('A' + i)).ToString()} = \"{Escape(Vars[i])}\";");
                for (int i = 0; i <= 25; i++)
                    sb.Append($"var {((char)('A' + i)).ToString()} = Number(\"{Escape(Vars[i])}\");");
                return sb.ToString();
            }
            void SetVars(Jint.Engine Engine)
            {
                for (int i = 0; i <= 25; i++)
                    JSVariables[i] = Engine.GetValue(((char)('A' + i)).ToString()).ToString();
            }
            string Escape(string Value) => new System.Text.StringBuilder(Value).
                Replace("\\", @"\\").Replace("\b", @"\b").Replace("\f", @"\f").
                Replace("\n", @"\n").Replace("\r", @"\r").Replace("\t", @"\t").Replace("\v", @"\v").
                Replace("\0", @"\0").Replace("'", @"\'").Replace("\"", @"\""").ToString();
            // Ask user to enter expression.
            try
            {
                //TODO: Add methods from https://help.syncfusion.com/cr/xamarin/calculate
                //Number suffix reference: http://stackoverflow.com/questions/7898310/using-regex-to-balance-match-parenthesis
                JSEngine = new Jint.Engine();
                if (TrueFree) return JSEngine.Execute(Expression).GetCompletionValue().ToString();
                JSEngine.Execute(
                    $@"var Prev = ""{Escape(JSEvaluteAns)}"";
var AngleUnit = {(byte)Mode};
var Modifier = {(byte)Mod};" + GetVars(JSVariables)
                + @"
var Ans = Number(Prev);
const global = Function('return this')();
function AngleConvert(Num, Origin, Target){
    switch(Origin) {
        case 0://Degrees
            switch(Target) {
                case 0:
                    return Num;
                    break;
                case 1:
                    return Num * Math.PI / 180;
                    break;
                case 2:
                    return Num * 10 / 9;
                    break;
                case 3:
                    return Num / 360;
                    break;
                default:
                    throw(""Invalid target of angle conversion."");
            } 
            break;
        case 1://Radians
            switch(Target) {
                case 0:
                    return Num / Math.PI * 180;
                    break;
                case 1:
                    return Num;
                    break;
                case 2:
                    return Num * 200 / Math.PI;
                    break;
                case 3:
                    return Num / Math.PI / 2;
                    break;
                default:
                    throw(""Invalid target of angle conversion."");
            } 
            break;
        case 2://Gradians
            switch(Target) {
                case 0:
                    return Num / 10 * 9;
                    break;
                case 1:
                    return Num / 200 * Math.PI;
                    break;
                case 2:
                    return Num;
                    break;
                case 3:
                    return Num / 400;
                    break;
                default:
                    throw(""Invalid target of angle conversion."");
            } 
            break;
        case 3://Turns
            switch(Target) {
                case 0:
                    return Num * 360;
                    break;
                case 1:
                    return Num * 2 * Math.PI;
                    break;
                case 2:
                    return Num * 400;
                    break;
                case 3:
                    return Num;
                    break;
                default:
                    throw(""Invalid target of angle conversion."");
            } 
            break;
        default://What?
            throw(""Invalid origin of angle conversion."");
    } 
}
function Abs (n) { return Math.abs(n); }
function Acos(n) { return AngleConvert(Math.acos(n), 1, AngleUnit); }
function Acosh (n) { return Math.log(n + Math.sqrt(n * n - 1)); }
function Acot (n) { return AngleConvert(Math.PI / 2 - Math.atan(n), 1, AngleUnit); }
function Acoth (n) { return Math.log((n + 1) / (n - 1)) / 2; }
function Acsc (n) { return AngleConvert(Math.asin(1 / n), 1, AngleUnit); }
function Acsch (n) { return Math.log((Math.sqrt(1 + n * n) + 1) / n); }
function Asec (n) { return AngleConvert(Math.acos(1 / n), 1, AngleUnit); }
function Asech (n) { return Math.log((Math.sqrt(1 - n * n) + 1) / n); }
function Asin (n) { return AngleConvert(Math.asin(n), 1, AngleUnit); }
function Asinh (n) { return Math.log(n + Math.sqrt(n * n + 1)); }
function Atan (n) { return AngleConvert(Math.atan(n), 1, AngleUnit); }
function Atan2 (y, x){ return AngleConvert(Math.atan2(y, x), 1, AngleUnit); }
function Atanh (n) { return Math.log((1 + n) / (1 - n)) / 2; }
function Cbrt (x) { return x ? x / Math.abs(x) * Math.pow(Math.abs(x), 1 / 3) : x; }
function Ceil(x) { return Math.ceil(x); }
function Cos(x) { return Math.cos(AngleConvert(x, AngleUnit, 1)); }
function Cosh(x) { return (1 + Math.exp(-2 * x)) / (2 * Math.exp(-x)); }
function Cot(n) { return 1 / AngleConvert(Math.tan(n), 1, AngleUnit); }
function Coth(x) { return (1 + Math.exp(-2 * x)) / (1 - Math.exp(-2 * x)); }
function Csc(n) { return 1 / AngleConvert(Math.sin(n), 1, AngleUnit); }
function Csch(x) { return (2 * Math.exp(-x)) / (1 - Math.exp(-2 * x)); }
function Clz32(x) { return 31 - Math.floor(Math.log(x) * Math.LOG2E); }
function Exp(x) { return Math.exp(x); }
function Expm1(x) { return Math.exp(x) - 1; }
function Floor(x) { return Math.floor(x); }
function Hypot() {
  var y = 0;
  var length = arguments.length;

  for (var i = 0; i < length; i++) {
    if (arguments[i] === Infinity || arguments[i] === -Infinity) {
      return Infinity;
    }
    y += arguments[i] * arguments[i];
  }
  return Math.sqrt(y);
};
function Imul(a, b) { return (a*(b%65536)+a*Math.floor(b/65536))%2147483648; }
function Lb(x) { return Math.log(x) / Math.LN2; }
function Ln(x) { return Math.log(x); }
function Log(x, base) { return Math.log(x) / (base ? Math.log(base) : Math.LN10); }
function Max() {return Math.max.apply(global, arguments);}
function Min() {return Math.min.apply(global, arguments);}
function Pow(x, y) { return Math.pow(x, y); }
function Random() { return Math.random(); }
function Round(x) { return Math.round(x); }
function Sec(n) { return 1 / AngleConvert(Math.cos(n), 1, AngleUnit); }
function Sech(x) { return (2 * Math.exp(-x)) / (1 + Math.exp(-2 * x)); }
function Sign(n){
    // Correctly handles all cases where NaN is appropriate
    n = parseInt(n);
    
    // If it is zero, negative zero, or NaN, then it correctly returns itselfs
    // else it divides itself by the absolute value of itself to acieve its
    // sign. This division proves to be much faster than an if or teranary
    // statement because of the overhead costs of branching, especially in JIT
    // compilers.
    return n ? n/Math.abs(n) : n;
}
function Sin(x) { return Math.sin(AngleConvert(x, AngleUnit, 1)); }
function Sinh(x) { return (1 - Math.exp(-2 * x)) / (2 * Math.exp(-x)); }
function Sqrt(x) { return Math.sqrt(x); }
function Tan(x) { return Math.tan(AngleConvert(x, AngleUnit, 1)); }
function Tanh(x) { return (1 - Math.exp(-2 * x)) / (1 + Math.exp(-2 * x)); }
function Trunc(x) { return Sign(x) == 1 ? Math.floor(x) : Math.ceil(x); }

function Deg(x) { return AngleConvert(x, 0, AngleUnit); }
function Rad(x) { return AngleConvert(x, 1, AngleUnit); }
function Grad(x) { return AngleConvert(x, 2, AngleUnit); }
function Turn(x) { return AngleConvert(x, 3, AngleUnit); }
function Factorial_(aNumber, recursNumber){
   // recursNumber keeps track of the number of iterations so far.
   if (aNumber < 3) {  // If the number is 0, its factorial is 1.
      if (aNumber == 0) return 1.;
      return Number(aNumber);
   } else {
      if(recursNumber > 170) {
         return Infinity;
      } else {  // Otherwise, recurse again.
         return (aNumber* Factorial_(aNumber - 1, recursNumber + 1));
      }
   }
}

function Factorial(aNumber){
   // Use type annotation to only accept numbers coercible to integers.
   // double is used for the return type to allow very large numbers to be returned.
   if(aNumber< 0) {
      return NaN; //throw(""Cannot take the factorial of a negative number."");
   } else {  // Call the recursive function.
      return  Factorial_(aNumber, 0);
   }
}

function nPr(n, r) { if(r > n) return 0; return Factorial(n) / Factorial(n - r);}
function nCr(n, r) { if(r > n) return 0; return Factorial(n) / (Factorial(n - r) * Factorial(r));}

function Display(Text, Modifier) {
    switch(Modifier) {
        case 0: //Normal
            return Text;
        case 1: //%
            return (Text * 100) + '%';
        case 2: //a b / c
            return Mixed(Text);
        case 3: //d / c
            return Fraction(Text);
        case 4: //° ′ ″
            return AngleMeasure(Text);
        case 5: //e√̅f
            return IntSurd(Text);
        case 6: //g / h √̅f
            return FracSurd(Text);
        default: //What?
            throw('Invalid display modifier.');
    }
}

const π = Math.PI;
const e = Math.E;
const Root2 = Math.SQRT2;
const Root0_5 = Math.SQRT1_2;
const Ln2 = Math.LN2;
const Ln10 = Math.LN10;
const Log2e = Math.LOG2E;
const Log10e = Math.LOG10E;
"""";")
#if true
            .SetValue("Fraction", new Func<double, string>((value) => {
                var best_numer = 1.0;
                var best_denom = 1.0;
                var best_err = Math.Abs(value - best_numer / best_denom);
                for (var denom = 1.0; best_err > 0 && denom <= 1e6; denom++)
                {
                    var numer = Math.Round(value * denom);
                    var err = Math.Abs(value - numer / denom);
                    if (err < best_err)
                    {
                        best_numer = numer;
                        best_denom = denom;
                        best_err = err;
                        //Console.WriteLine(best_numer + " / " + best_denom + " = " + (best_numer / best_denom) + " error " + best_err);
                    }
                }
                return best_numer + " / " + best_denom;
            })).SetValue("Mixed", new Func<double, string>((double value) =>
            {
                var best_numer = 1.0;
                var best_denom = 1.0;
                var best_err = Math.Abs(value - best_numer / best_denom);
                for (var denom = 1.0; best_err > 0 && denom <= 1e6; denom++)
                {
                    var numer = Math.Round(value * denom);
                    var err = Math.Abs(value - numer / denom);
                    if (err < best_err)
                    {
                        best_numer = numer;
                        best_denom = denom;
                        best_err = err;
                        //Console.WriteLine(best_numer + " / " + best_denom + " = " + (best_numer / best_denom) + " error " + best_err);
                    }
                }
                return Math.Round(best_numer / best_denom) + " " + best_numer % best_denom + " / " + best_denom;
            })).SetValue("AngleMeasure", new Func<double, string>((double value) =>
            {
                var degree = Math.Floor(value);
                var minute = Math.Floor((value - degree) * 60);
                var second = (value - degree - minute / 60) * 3600;
                return $"{degree}° {minute}′ {second}″";
            })).SetValue("IntSurd", new Func<double, string>((double value) =>
            {
                var Negative = value < 0;
                // A = AVariable, B = Builder, C = Char
                if (value > 5000) throw new ArgumentOutOfRangeException(nameof(value), value, "Value is too large (>5000).");
                var A = value = Math.Round(value * value);
                do { A--; } while (value / (A * A) - Math.Truncate(value / (A * A)) != 0);
                //if (A == 0) throw new ArithmeticException("Cannot find appropiate surd.");
                if (A < 0) A = 1;
                var B = new System.Text.StringBuilder();
                if (Negative) B.Append("-");
                B.Append(A).Append("√");
                foreach(var C in (value / (A * A)).ToString())
                {
#if WINDOWS_UWP
                    B.Append("̅");
                    B.Append(C);
#else
                    B.Append(C);
                    B.Append("̅");
#endif
                }
                return B.ToString();
            })).SetValue("FracSurd", new Func<double, string>((double value) =>
            {
                var Negative = value < 0;
                /*
                void Simplify(int[] numbers)
                {
                    int gcd = GCD(numbers);
                    for (int i = 0; i < numbers.Length; i++)
                        numbers[i] /= gcd;
                }*/
                double GCD(double a, double b)
                {
                    while (b > 0)
                    {
                        var rem = a % b;
                        a = b;
                        b = rem;
                    }
                    return a;
                }
                /*int GCD(int[] args)
                {
                    // using LINQ:
                    return System.Linq.Enumerable.Aggregate(args, (gcd, arg) => GCD(gcd, arg));
                }*/
                value = Math.Abs(value);
                for (int i = 0; i <= 1e6; i++)
                {
                    var SubjectToTest = value / Math.Sqrt(i);
                    for (var denom = 1.0; denom <= 500; denom++)
                    {
                        var numer = Math.Round(SubjectToTest * denom);
                        if (SubjectToTest - numer / denom == 0)
                        {
                            int Square = 1, a = i;
                            for (int b = 2; a > 1; b++)
                                if (a % b == 0)
                                {
                                    int x = 0;
                                    while (a % b == 0)
                                    {
                                        a /= b;
                                        x++;
                                    }
                                    //Console.WriteLine("{0} is a prime factor {1} times!", b, x);
                                    for (int j = 2; j <= x; j += 2) Square *= b;
                                }
                            var LCM = GCD(numer, denom);
                            numer /= LCM;
                            denom /= LCM;
                            var Builder = new System.Text.StringBuilder();
                            if (Negative) Builder.Append("-");
                            Builder.Append(numer.ToString()).Append(" / ").Append(denom).Append(" √");
                            foreach (var Char in (i / Square).ToString())
                            {
#if WINDOWS_UWP
                                Builder.Append("̅");
                                Builder.Append(Char);
#else
                                Builder.Append(Char);
                                Builder.Append("̅");
#endif
                            }
                            return Builder.ToString();
                        }
                    }
                }
                throw new ArithmeticException("Cannot find appropiate fraction and surd.");
            }))
#endif
            .Execute(Expression);
                JSEvaluteAns = JSEngine.GetCompletionValue().ToString();
                SetVars(JSEngine);
            return JSEngine.Invoke("Display", JSEngine.GetCompletionValue(), JSEngine.GetValue("Modifier")).ToString();
            }
            catch (System.Reflection.TargetInvocationException ex) when (Alert != null)
            { return 'ⓧ' + ex.InnerException.Message.Split('\r', '\n', '\f')[0]; }
            catch (Exception ex) when (Alert != null) { return 'ⓧ' + ex.Message.Split('\r', '\n', '\f')[0]; } //⮾ 
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
        public static void Try<TException>(Action Try, Action<TException> Catch = null,
            Func<bool> CatchFilter = null, Action Finally = null) where TException : Exception
        {
            try
            {
                Try();
            }
            catch (TException ex) when (Catch != null && (CatchFilter == null ? true : CatchFilter()))
            {
                Catch(ex);
            }
            finally
            {
                Finally?.Invoke();
            }

        }
        public static T Try<T, TException>(Func<T> Try, Func<TException, T> Catch = null,
            Func<bool> CatchFilter = null, Action Finally = null) where TException : Exception
        {
            try
            {
                return Try();
            }
            catch (TException ex) when (Catch != null && (CatchFilter == null ? true : CatchFilter()))
            {
                return Catch(ex);
            }
            finally
            {
                Finally?.Invoke();
            }
        }
        public static void Try<TException1, TException2>(Action Try, Action<TException1> Catch1 = null,
            Func<bool> CatchFilter1 = null, Action<TException2> Catch2 = null, Func<bool> CatchFilter2 = null,
            Action Finally = null) where TException1 : Exception where TException2 : Exception
        {
            try
            {
                Try();
            }
            catch (TException1 ex) when (Catch1 != null && (CatchFilter1 == null ? true : CatchFilter1()))
            {
                Catch1(ex);
            }
            catch (TException2 ex) when (Catch2 != null && (CatchFilter2 == null ? true : CatchFilter2()))
            {
                Catch2(ex);
            }
            finally
            {
                Finally?.Invoke();
            }

        }
        public static T Try<T, TException1, TException2>(Func<T> Try, Func<TException1, T> Catch1 = null,
            Func<bool> CatchFilter1 = null, Func<TException2, T> Catch2 = null, Func<bool> CatchFilter2 = null,
            Action Finally = null) where TException1 : Exception where TException2 : Exception
        {
            try
            {
                return Try();
            }
            catch (TException1 ex) when (Catch1 != null && (CatchFilter1 == null ? true : CatchFilter1()))
            {
                return Catch1(ex);
            }
            catch (TException2 ex) when (Catch2 != null && (CatchFilter2 == null ? true : CatchFilter2()))
            {
                return Catch2(ex);
            }
            finally
            {
                Finally?.Invoke();
            }
        }
        public static EventHandler<TextChangedEventArgs> TextChanged(Func<string> Value)
        {
            return (object sender, TextChangedEventArgs e) =>
            { if (((Entry)sender).Text != Value()) { ((Entry)sender).Text = Value(); } };
        }
        public static double TryParseDouble(string s, double @default)
        { if (double.TryParse(s, out double d)) { return d; } else { return @default; }; }

        public static byte[] Resample(byte[] samples, int fromSampleRate, int toSampleRate, int quality = 10)
        {
            int srcLength = samples.Length;
            var destLength = (long)samples.Length * toSampleRate / fromSampleRate;
            byte[] _samples = new byte[destLength];
            var dx = srcLength / destLength;

            // fmax : nyqist half of destination sampleRate
            // fmax / fsr = 0.5;
            var fmaxDivSR = 0.5;
            var r_g = 2 * fmaxDivSR;

            // Quality is half the window width
            var wndWidth2 = quality;
            var wndWidth = quality * 2;

            var x = 0;
            int i, j;
            double r_y;
            int tau;
            double r_w;
            double r_a;
            double r_snc;
            for (i = 0; i < destLength; ++i)
            {
                r_y = 0.0;
                for (tau = -wndWidth2; tau < wndWidth2; ++tau)
                {
                    // input sample index
                    j = x + tau;

                    // Hann Window. Scale and calculate sinc
                    r_w = 0.5 - 0.5 * Math.Cos(2 * Math.PI * (0.5 + (j - x) / wndWidth));
                    r_a = 2 * Math.PI * (j - x) * fmaxDivSR;
                    r_snc = 1.0;
                    if (r_a != 0)
                        r_snc = Math.Sin(r_a) / r_a;

                    if ((j >= 0) && (j < srcLength))
                    {
                        r_y += r_g * r_w * r_snc * samples[j];
                    }
                }
                _samples[i] = (byte)r_y;
                x += (int)dx; 
            }

            return _samples;
        }
        public static double[] Resample(double[] samples, int fromSampleRate, int toSampleRate, int quality = 10)
        {
            List<double> _samples = new List<double>();

            int srcLength = samples.Length;
            var destLength = (long)samples.Length * toSampleRate / fromSampleRate;
            var dx = srcLength / destLength;

            // fmax : nyqist half of destination sampleRate
            // fmax / fsr = 0.5;
            var fmaxDivSR = 0.5;
            var r_g = 2 * fmaxDivSR;

            // Quality is half the window width
            var wndWidth2 = quality;
            var wndWidth = quality * 2;

            var x = 0;
            int i, j;
            double r_y;
            int tau;
            double r_w;
            double r_a;
            double r_snc;
            for (i = 0; i < destLength; ++i)
            {
                r_y = 0.0;
                for (tau = -wndWidth2; tau < wndWidth2; ++tau)
                {
                    // input sample index
                    j = x + tau;

                    // Hann Window. Scale and calculate sinc
                    r_w = 0.5 - 0.5 * Math.Cos(2 * Math.PI * (0.5 + (j - x) / wndWidth));
                    r_a = 2 * Math.PI * (j - x) * fmaxDivSR;
                    r_snc = 1.0;
                    if (r_a != 0)
                        r_snc = Math.Sin(r_a) / r_a;

                    if ((j >= 0) && (j < srcLength))
                    {
                        r_y += r_g * r_w * r_snc * samples[j];
                    }
                }
                _samples[i] = r_y;
                x += (int)dx;
            }

            return _samples.ToArray();
        }
        public static IEnumerable<T> ToEnumerable<T>(params T[] Items)
        {
            foreach (T Item in Items)
                yield return Item;
        }

        /// <summary>Formula for computing Luminance out of R G B, which is something close to
        /// luminance = (red * 0.3) + (green * 0.6) + (blue * 0.1).</summary>
        // Original Source: http://stackoverflow.com/questions/20978198/how-to-match-uilabels-textcolor-to-its-background
        public static Color GetTextColor(Color backgroundColor)
        {
            var backgroundColorDelta = ((backgroundColor.R * 0.3) + (backgroundColor.G * 0.6) + (backgroundColor.B * 0.1));

            return (backgroundColorDelta > 0.4f) ? Color.Black : Color.White;
        }

        public static Dictionary<TKey, TValue> NewDictionary<TKey, TValue>(TKey key, TValue value) => 
            new Dictionary<TKey, TValue>() {[key] = value};
        public static void IgnoreEx(Action Action, params Type[] Exceptions)
        {
            try { Action(); }
            catch (Exception e) when (System.Linq.Enumerable.Contains(Exceptions, e.GetType())) { }
        }
        public static T IgnoreEx<T>(Func<T> Action, params Type[] Exceptions)
        {
            try { return Action(); }
            catch (Exception e) when (System.Linq.Enumerable.Contains(Exceptions, e.GetType())) { return default(T); }
        }

        public static async ValueTask<int> Test()
        { await Task.Delay(30);
            return 30;
        }
        ///// <summary>
        ///// An uninitialized Void object.
        ///// </summary>
        //public static object Void
        //{ get { return System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(void)); } }
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
        }
<style type="text/css">
.tg  {border-collapse:collapse;border-spacing:0;}
.tg td{font-family:Arial, sans-serif;font-size:14px;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;}
.tg th{font-family:Arial, sans-serif;font-size:14px;font-weight:normal;padding:10px 5px;border-style:solid;border-width:1px;overflow:hidden;word-break:normal;}
.tg .tg-s6z2{text-align:center}
.tg .tg-baqh{text-align:center;vertical-align:top}
</style>
<table class="tg">
  <tbody><tr>
    <th class="tg-s6z2" colspan="19"></th>
  </tr>
  <tr>
    <td class="tg-baqh" rowspan="6"></td>
    <td class="tg-baqh">π</td>
    <td class="tg-baqh">e</td>
    <td class="tg-baqh" rowspan="6"></td>
    <td class="tg-baqh">Log</td>
    <td class="tg-baqh">Pow</td>
    <td class="tg-baqh">Sin</td>
    <td class="tg-baqh">Asin</td>
    <td class="tg-baqh" rowspan="6"></td>
    <td class="tg-baqh">&lt;</td>
    <td class="tg-baqh">&gt;</td>
    <td class="tg-baqh">&amp;&amp;</td>
    <td class="tg-baqh">&gt;&gt;&gt;</td>
    <td class="tg-baqh" rowspan="6"><br></td>
    <td class="tg-baqh">␣</td>
    <td class="tg-baqh">%</td>
    <td class="tg-baqh">Ans</td>
    <td class="tg-baqh">⌫</td>
    <td class="tg-baqh">⎚</td>
  </tr>
  <tr>
    <td class="tg-s6z2">Root2</td>
    <td class="tg-s6z2">Root0_5</td>
    <td class="tg-s6z2">Rdm</td>
    <td class="tg-s6z2">Exp</td>
    <td class="tg-s6z2">Cos</td>
    <td class="tg-s6z2">Acos</td>
    <td class="tg-s6z2">&lt;=</td>
    <td class="tg-s6z2">&gt;=</td>
    <td class="tg-s6z2">&lt;&lt;</td>
    <td class="tg-s6z2">&gt;&gt;</td>
    <td class="tg-baqh">7</td>
    <td class="tg-s6z2">8</td>
    <td class="tg-s6z2">9</td>
    <td class="tg-s6z2">(</td>
    <td class="tg-s6z2">)</td>
  </tr>
  <tr>
    <td class="tg-s6z2">Ln2</td>
    <td class="tg-s6z2">Ln10</td>
    <td class="tg-s6z2">Max</td>
    <td class="tg-s6z2">Min</td>
    <td class="tg-s6z2">Tan</td>
    <td class="tg-s6z2">Atan</td>
    <td class="tg-s6z2">==</td>
    <td class="tg-s6z2">!=</td>
    <td class="tg-s6z2">++</td>
    <td class="tg-s6z2">--</td>
    <td class="tg-baqh">4</td>
    <td class="tg-s6z2">5</td>
    <td class="tg-s6z2">6</td>
    <td class="tg-s6z2">*</td>
    <td class="tg-s6z2">/</td>
  </tr>
  <tr>
    <td class="tg-s6z2">Log2e</td>
    <td class="tg-s6z2">Log10e</td>
    <td class="tg-s6z2">Sqrt</td>
    <td class="tg-s6z2">Rnd</td>
    <td class="tg-s6z2">Ceil</td>
    <td class="tg-s6z2">Floor</td>
    <td class="tg-s6z2">===</td>
    <td class="tg-s6z2">!==</td>
    <td class="tg-s6z2">~</td>
    <td class="tg-s6z2">&amp;</td>
    <td class="tg-baqh">1</td>
    <td class="tg-s6z2">2</td>
    <td class="tg-s6z2">3</td>
    <td class="tg-s6z2">+</td>
    <td class="tg-s6z2">-</td>
  </tr>
  <tr>
    <td class="tg-s6z2"></td>
    <td class="tg-s6z2"></td>
    <td class="tg-s6z2" colspan="2">,</td>
    
    <td class="tg-s6z2">Abs</td>
    <td class="tg-s6z2">Fct</td>
    <td class="tg-s6z2">!</td>
    <td class="tg-s6z2">||</td>
    <td class="tg-s6z2">^</td>
    <td class="tg-s6z2">|</td>
    <td class="tg-baqh">0</td>
    <td class="tg-s6z2">.</td>
    <td class="tg-s6z2">e</td>
    <td class="tg-s6z2" colspan="2">=</td>
  </tr>
  <tr>
    <td class="tg-s6z2">Const</td>
    <td class="tg-s6z2"></td>
    <td class="tg-s6z2"></td>
    <td class="tg-s6z2">Func</td>
    <td class="tg-s6z2"></td>
    <td class="tg-s6z2"></td>
    <td class="tg-s6z2"></td>
    <td class="tg-s6z2">Bin</td>
    <td class="tg-s6z2"></td>
    <td class="tg-s6z2"></td>
    <td class="tg-baqh"></td>
    <td class="tg-s6z2">Norm</td>
    <td class="tg-s6z2"></td>
    <td class="tg-s6z2"></td>
    <td class="tg-s6z2"></td>
  </tr>
</tbody></table>
         */
    }
}

