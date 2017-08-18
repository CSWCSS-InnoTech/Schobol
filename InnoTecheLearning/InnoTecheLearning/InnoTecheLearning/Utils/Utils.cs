using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InnoTecheLearning
{/// <summary>
/// A class that provides methods to help run the App.
/// </summary>
    public static partial class Utils
    {   
        public const string CurrentNamespace =
            "InnoTecheLearning." +
#if __IOS__
            "iOS"
#elif __ANDROID__
            "Droid"
#elif WINDOWS_UWP
            "UWP"
#elif WINDOWS_APP
            "Windows"
#elif WINDOWS_PHONE_APP
            "WinPhone"
#endif
                ;

        public static T OnPlatform<T>(T iOS, T Android, T UWP) =>
#if __IOS__
            iOS
#elif __ANDROID__
            Android
#elif WINDOWS_UWP
            UWP
#endif
            ;
        public async static ValueTask<T> Alert<T>(T Return, Page Page, string Message = "",
                                                  string Title = "Alert", string Cancel = "OK")
        {
            await Page.DisplayAlert(Title, Message, Cancel);
            return Return;
        }

        public static ValueTask<Unit> Alert(Page Page, string Message = "",
            string Title = "Alert", string Cancel = "OK") => Unit.Await(Page.DisplayAlert(Title, Message, Cancel));
        public async static ValueTask<bool> AlertChoose(Page Page, string Message = "",
            string Title = "Alert", string Accept = "OK", string Cancel = "Cancel") =>
            await Page.DisplayAlert(Message, Title, Accept, Cancel);

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
            return fs;
        }

        /// <summary>
        /// Returns bolded <see cref="Text"/>.
        /// </summary>
        /// <param name="Text"><see cref="Text"/> to make bold.</param>
        /// <returns></returns>
        public static Span Bold(string Text)
        { return new Span { Text = Text, FontAttributes = FontAttributes.Bold }; }
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
        
        public delegate double MathFunc0();
        public delegate double MathFunc(double x);
        public delegate double MathFunc2(double x, double y);
        //public delegate double MathFuncArgs(params double[] arguments);
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
            bool TrueFree = false)
        {
            void GetVars(Jint.Engine Engine, params string[] Vars)
            {
                for (int i = 0; i <= 25; i++)
                    Engine.SetValue($"Var{((char)('A' + i)).ToString()}", Vars[i]);
                for (int i = 0; i <= 25; i++)
                    Engine.SetValue(((char)('A' + i)).ToString(), TryParseDouble(Vars[i], double.NaN));
            }
            void SetVars(Jint.Engine Engine)
            {
                for (int i = 0; i <= 25; i++)
                    JSVariables[i] = Engine.GetValue(((char)('A' + i)).ToString()).ToString();
            }
            double AngleConvert(double Num, AngleMode Origin, AngleMode Target)
            {
                switch (Origin)
                {
                    case AngleMode.Degree:
                        switch (Target)
                        {
                            case AngleMode.Degree:
                                return Num;
                            case AngleMode.Radian:
                                return Num * Math.PI / 180;
                            case AngleMode.Gradian:
                                return Num * 10 / 9;
                            case AngleMode.Turn:
                                return Num / 360;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(Target), Target, "Invalid target of angle conversion.");
                        }
                    case AngleMode.Radian:
                        switch (Target)
                        {
                            case AngleMode.Degree:
                                return Num / Math.PI * 180;
                            case AngleMode.Radian:
                                return Num;
                            case AngleMode.Gradian:
                                return Num * 200 / Math.PI;
                            case AngleMode.Turn:
                                return Num / Math.PI / 2;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(Target), Target, "Invalid target of angle conversion.");
                        }
                    case AngleMode.Gradian:
                        switch (Target)
                        {
                            case AngleMode.Degree:
                                return Num / 10 * 9;
                            case AngleMode.Radian:
                                return Num / 200 * Math.PI;
                            case AngleMode.Gradian:
                                return Num;
                            case AngleMode.Turn:
                                return Num / 400;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(Target), Target, "Invalid target of angle conversion.");
                        }
                    case AngleMode.Turn:
                        switch (Target)
                        {
                            case AngleMode.Degree:
                                return Num * 360;
                            case AngleMode.Radian:
                                return Num * 2 * Math.PI;
                            case AngleMode.Gradian:
                                return Num * 400;
                            case AngleMode.Turn:
                                return Num;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(Target), Target, "Invalid target of angle conversion.");
                        }
                    default://What?
                        throw new ArgumentOutOfRangeException(nameof(Origin), Origin, "Invalid origin of angle conversion.");
                }
            }
            double Factorial(double x)
            {
                // Use type annotation to only accept numbers coercible to integers.
                // double is used for the return type to allow very large numbers to be returned.
                if (x < 0)
                {
                    return double.NaN; //throw(""Cannot take the factorial of a negative number."");
                }
                else
                {  // Call the recursive function.
                    return Factorial_(x, 0);
                }
                double Factorial_(double aNumber, byte recursNumber)
                {
                    // recursNumber keeps track of the number of iterations so far.
                    if (aNumber < 3)
                    {  // If the number is 0, its factorial is 1.
                        if (aNumber == 0) return 1.0;
                        return aNumber;
                    }
                    else
                    {
                        if (recursNumber > 170)
                        {
                            return double.PositiveInfinity;
                        }
                        else
                        {  // Otherwise, recurse again.
                            return (aNumber * Factorial_(aNumber - 1, (byte)(recursNumber + 1)));
                        }
                    }
                }
            }
            double GCD(double a, double b)
            {
                while (b != 0)
                {
                    var temp = b;
                    b = a % b;
                    a = temp;
                }
                return a;
            }
            // Ask user to enter expression.
            try
            {
                JSEngine = new Jint.Engine();
                if (TrueFree) return JSEngine.Execute(Expression).GetCompletionValue().ToString();
                GetVars(JSEngine, JSVariables);
                JSEngine.SetValue("Prev", JSEvaluteAns)
                .SetValue("Ans", TryParseDouble(JSEvaluteAns, double.NaN))
                .SetValue("global", JSEngine.Global)
.Execute(@"
function Hypot()
{                    
    var y = 0.0;
    var length = arguments.length;

    for (var i = 0; i < length; i++)
    {
        if (arguments[i] === Infinity || arguments[i] === -Infinity)
                return Infinity;
        y += arguments[i] * arguments[i];
    }
    return Math.sqrt(y);
};
function Max() { return Math.max.apply(global, arguments); }
function Min() { return Math.min.apply(global, arguments); }
"""";")
                .SetValue("AngleConvert", new Func<double, AngleMode, AngleMode, double>(AngleConvert))
                .SetValue("Abs", new MathFunc(Math.Abs))
                .SetValue("Acos", new MathFunc(x => AngleConvert(Math.Acos(x), AngleMode.Radian, Mode)))
                .SetValue("Acosh", new MathFunc(x => Math.Log(x + Math.Sqrt(x * x - 1))))
                .SetValue("Acot", new MathFunc(x => AngleConvert(Math.PI / 2 - Math.Atan(x), AngleMode.Radian, Mode)))
                .SetValue("Acoth", new MathFunc(x => Math.Log((x + 1) / (x - 1)) / 2))
                .SetValue("Acsc", new MathFunc(x => AngleConvert(Math.Asin(1 / x), AngleMode.Radian, Mode)))
                .SetValue("Acsch", new MathFunc(x => Math.Log((Math.Sqrt(1 + x * x) + 1) / x)))
                .SetValue("Asec", new MathFunc(x => AngleConvert(Math.Acos(1 / x), AngleMode.Radian, Mode)))
                .SetValue("Asech", new MathFunc(x => Math.Log((Math.Sqrt(1 - x * x) + 1) / x)))
                .SetValue("Asin", new MathFunc(x => AngleConvert(Math.Asin(x), AngleMode.Radian, Mode)))
                .SetValue("Asinh", new MathFunc(x => Math.Log(x + Math.Sqrt(x * x + 1))))
                .SetValue("Atan", new MathFunc(x => AngleConvert(Math.Atan(x), AngleMode.Radian, Mode)))
                .SetValue("Atan2", new MathFunc2((y, x) => AngleConvert(Math.Atan2(y, x), AngleMode.Radian, Mode)))
                .SetValue("Atanh", new MathFunc(x => Math.Log((1 + x) / (1 - x)) / 2))
                .SetValue("Cbrt", new MathFunc(x => 
                    double.IsInfinity(x) || double.IsNaN(x) ? x : x / Math.Abs(x) * Math.Pow(Math.Abs(x), 1 / 3)))
                .SetValue("Ceil", new MathFunc(Math.Ceiling))
                .SetValue("Cos", new MathFunc(x => AngleConvert(Math.Cos(x), AngleMode.Radian, Mode)))
                .SetValue("Cosh", new MathFunc(Math.Cosh))
                .SetValue("Cot", new MathFunc(x => 1 / Math.Tan(AngleConvert(x, Mode, AngleMode.Radian))))
                .SetValue("Coth", new MathFunc(x => (1 + Math.Exp(-2 * x)) / (1 - Math.Exp(-2 * x))))
                .SetValue("Csc", new MathFunc(x => 1 / Math.Sin(AngleConvert(x, Mode, AngleMode.Radian))))
                .SetValue("Csch", new MathFunc(x => (2 * Math.Exp(-x)) / (1 - Math.Exp(-2 * x))))
                .SetValue("Clz32", new MathFunc(x => 31 - Math.Floor(Math.Log(x, 2))))
                .SetValue("Exp", new MathFunc(Math.Exp))
                .SetValue("Floor", new MathFunc(Math.Floor))
                .SetValue("Imul", new MathFunc2((x, y) => (x * (y % 65536) + x * Math.Floor(y / 65536)) % 2147483648))
                .SetValue("Lb", new MathFunc(x => Math.Log(x, 2)))
                .SetValue("Ln", new MathFunc(Math.Log))
                .SetValue("Log", new MathFunc2((x, @base) =>
                    Math.Log(x, double.IsNaN(@base) || double.IsInfinity(@base) ? Math.Log(10) : Math.Log(@base))))
                //.SetValue("Max_", new MathFuncArgs(System.Linq.Enumerable.Max))
                //.SetValue("Min_", new MathFuncArgs(System.Linq.Enumerable.Min))
                .SetValue("Pow", new MathFunc2(Math.Pow))
                .SetValue("Round", new MathFunc(Math.Round))
                .SetValue("Random", new MathFunc0(new Random().NextDouble))
                .SetValue("Sec", new MathFunc(x => 1 / Math.Cos(AngleConvert(x, Mode, AngleMode.Radian))))
                .SetValue("Sech", new MathFunc(x => (2 * Math.Exp(-x)) / (1 + Math.Exp(-2 * x))))
                .SetValue("Sign", new MathFunc(x => Math.Sign(x)))
                .SetValue("Sin", new MathFunc(x => Math.Sin(AngleConvert(x, Mode, AngleMode.Radian))))
                .SetValue("Sinh", new MathFunc(Math.Sinh))
                .SetValue("Sqrt", new MathFunc(Math.Sqrt))
                .SetValue("Tan", new MathFunc(x => Math.Tan(AngleConvert(x, Mode, AngleMode.Radian))))
                .SetValue("Tanh", new MathFunc(Math.Tanh))
                .SetValue("Trunc", new MathFunc(Math.Truncate))
                .SetValue("Deg", new MathFunc(x => AngleConvert(x, AngleMode.Degree, Mode)))
                .SetValue("Rad", new MathFunc(x => AngleConvert(x, AngleMode.Radian, Mode)))
                .SetValue("Grad", new MathFunc(x => AngleConvert(x, AngleMode.Gradian, Mode)))
                .SetValue("Turn", new MathFunc(x => AngleConvert(x, AngleMode.Turn, Mode)))
                .SetValue("Factorial", new MathFunc(Factorial))
                .SetValue("nPr", new MathFunc2((n, r) => r > n ? 0 : Factorial(n) / Factorial(n - r)))
                .SetValue("nCr", new MathFunc2((n, r) => r > n ? 0 : Factorial(n) / (Factorial(n - r) * Factorial(r))))
                .SetValue("GCD", new MathFunc2(GCD))
                .SetValue("HCF", new MathFunc2(GCD))
                .SetValue("LCM", new MathFunc2((x, y) => (x / GCD(x, y)) * y))

                .SetValue("π", Math.PI)
                .SetValue("e", Math.E)
                .SetValue("Root2", Math.Sqrt(2))
                .SetValue("Root0_5", Math.Sqrt(0.5))
                .SetValue("Ln2", Math.Log(2))
                .SetValue("Ln10", Math.Log(10))
                .SetValue("Log2e", Math.Log(Math.E, 2))
                .SetValue("Log10e", Math.Log10(Math.E))
                .Execute(Expression);
                JSEvaluteAns = JSEngine.GetCompletionValue().ToString();
                SetVars(JSEngine);
                return JSEngine.GetCompletionValue().ToString();
            }
            catch (System.Reflection.TargetInvocationException ex) when (Alert != null)
            { return Error + ex.InnerException.Message.Split('\r', '\n', '\f')[0]; }
            catch (Exception ex) when (Alert != null) { return Error + ex.Message.Split('\r', '\n', '\f')[0]; }
        }
        public static string Display(double value, Modifier mod)
        {
            switch (mod)
            {
                case Modifier.Normal:
                    return value.ToString();
                case Modifier.Percentage:
                    return (value * 100).ToString("F99").TrimEnd('0').TrimEnd('.') + "%";
                case Modifier.Mixed_Fraction:
                    for (var denom = 1.0; denom <= 1e6; denom++)
                    {
                        var numer = Math.Round(value * denom);
                        if (HasMinimalDifference(value, numer / denom))
                            return $"{Math.Round(numer / denom)} + {numer % denom} / {denom}";
                    }
                    throw new ArithmeticException("Cannot find appropriate fraction.");
                    /*
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
                            //Console.WriteLine(best_numer + " / " + best_denom +
                            //    " = " + (best_numer / best_denom) + " error " + best_err);
                        }
                    }
                    return Math.Round(best_numer / best_denom) + " " + best_numer % best_denom + " / " + best_denom;*/
                case Modifier.Fraction:
                    for (var denom = 1.0; denom <= 1e6; denom++)
                    {
                        var numer = Math.Round(value * denom);
                        if (HasMinimalDifference(value, numer / denom))
                            return $"{numer} / {denom}";
                    }
                    throw new ArithmeticException("Cannot find appropriate fraction.");
                    /*
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
                            //Console.WriteLine(best_numer + " / " + best_denom +
                            //    " = " + (best_numer / best_denom) + " error " + best_err);
                        }
                    }
                    return best_numer + " / " + best_denom;*/
                case Modifier.AngleMeasure:
                    var degree = Math.Floor(value);
                    var minute = Math.Floor((value - degree) * 60);
                    var second = ((value - degree - minute / 60) * 3600).ToString("F99").TrimEnd('0').TrimEnd('.');
                    return $"{degree}° {minute}′ {second}″";
                case Modifier.IntSurd:
                    if (double.IsInfinity(value) || double.IsNaN(value))
                        throw new ArithmeticException(nameof(value) + " is not finite.");
                    if (value.NearInteger()) return value.ToString() + OnPlatform("√1̅", "√1̅", "√̅1");
                    var Negative = value < 0;
                    // A = AVariable, B = Builder, C = Char
                    if (value > 5000 || value < -5000)
                        throw new ArgumentOutOfRangeException(nameof(value), value,
                            nameof(value) + "'s absolute value is too large (>5000).");
                    double A = Math.Round(value * value), squa = A;
                    do { A--; } while (squa / (A * A) - Math.Truncate(squa / (A * A)) != 0);
                    if (A == -1 || !HasMinimalDifference(A * Math.Sqrt(squa / (A * A)), value))
                        throw new ArithmeticException("Cannot find appropriate surd.");
                    var B = new System.Text.StringBuilder();
                    if (Negative) B.Append("-");
                    B.Append(A).Append("√");
                    foreach (var C in (squa / (A * A)).ToString())
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
                case Modifier.FracSurd:
                    if (double.IsInfinity(value) || double.IsNaN(value))
                        throw new ArithmeticException(nameof(value) + " is not finite.");
                    bool Minus = value < 0;
                    if (Minus) value = -value;
                    for (int Surd = 1; Surd <= 1000; Surd++)
                        for (int Denom = 1; Denom <= 500; Denom++)
                        {
                            var Numer = value / Math.Sqrt(Surd) * Denom;
                            if (Numer.NearInteger())
                            {
                                Numer = Math.Round(Numer);
                                int GCF(int a, int b)
                                {
                                    while (b != 0)
                                    {
                                        int temp = b;
                                        b = a % b;
                                        a = temp;
                                    }
                                    return a;
                                }
                                (int Squared, int Remaining) SimplifySurd(int a)
                                {
                                    int Number = a, b = 0, Squared = 1;
                                    for (b = 2; a > 1; b++)
                                        if (a % b == 0)
                                        {
                                            int x = 0;
                                            while (a % b == 0)
                                            {
                                                a /= b;
                                                x++;
                                            }
                                            //Console.WriteLine("{0} is a prime factor {1} times!", b, x);
                                            for (int c = 2; c <= x; c += 2) Squared *= b;
                                        }
                                    return (Squared, Number / (Squared * Squared));
                                }

                                var Simplified = SimplifySurd(Surd);
                                Numer *= Simplified.Squared;
                                Surd = Simplified.Remaining;
                                var Common = GCF((int)Math.Round(Numer), Denom);
                                Numer /= Common;
                                Denom /= Common;

                                var Builder = new System.Text.StringBuilder();
                                if (Minus) Builder.Append("-");
                                Builder.Append(Numer).Append(" / ").Append(Denom).Append(" √");
                                foreach (var C in (Surd).ToString())
                                {
#if WINDOWS_UWP
                                    Builder.Append("̅");
                                    Builder.Append(C);
#else
                                Builder.Append(C);
                                Builder.Append("̅");
#endif
                                }
                                return Builder.ToString();
                            }
                        }
                    throw new ArithmeticException("Cannot find appropriate fraction and surd.");
                default:
                    throw new ArgumentOutOfRangeException(nameof(mod), mod, $"{mod} is not a valid {nameof(Modifier)}");
            }
        }

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
        
        /// <summary>Formula for computing Luminance out of R G B, which is something close to
        /// luminance = (red * 0.3) + (green * 0.6) + (blue * 0.1).</summary>
        // Original Source: http://stackoverflow.com/questions/20978198/how-to-match-uilabels-textcolor-to-its-background
        public static Color GetTextColor(Color backgroundColor)
        {
            var backgroundColorDelta = ((backgroundColor.R * 0.3) + (backgroundColor.G * 0.6) + (backgroundColor.B * 0.1));

            return (backgroundColorDelta > 0.4f) ? Color.Black : Color.White;
        }
        
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

        public static ValueTask<bool> InternetAvaliable
        {
            get
            {
                async ValueTask<bool> InternalAsync()
                {
                    try
                    {
                        await System.Net.Dns.GetHostEntryAsync("www.google.com");
                        return true;
                    }
                    catch (System.Net.Sockets.SocketException) { return false; }
                    catch (AggregateException ex) when (ex.InnerException is System.Net.Sockets.SocketException)
                    {
                        return false;
                    }
                }
                return InternalAsync();
            }
        }
        public static int FloatToInt32Bits(float value)
        {
            int result = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
            if (((result & 0x7F800000) == 0x7F800000) && (result & 0x80000000) != 0)
                result = 0x7fc00000;
            return result;
        }
        public static float Int32BitsToFloat(int value) => BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
        public static bool HasMinimalDifference(float value1, float value2, int units = 11)
        {
            int iValue1 = FloatToInt32Bits(value1);
            int iValue2 = FloatToInt32Bits(value2);

            // If the signs are different, return false except for +0 and -0.
            if ((iValue1 >> 31) != (iValue2 >> 31)) return value1 == value2;

            int diff = Math.Abs(iValue1 - iValue2);

            return diff <= units;
        }
        public static bool HasMinimalDifference(double value1, double value2, int units = 22)
        {
            long lValue1 = BitConverter.DoubleToInt64Bits(value1);
            long lValue2 = BitConverter.DoubleToInt64Bits(value2);

            // If the signs are different, return false except for +0 and -0.
            if ((lValue1 >> 63) != (lValue2 >> 63)) return value1 == value2;

            long diff = Math.Abs(lValue1 - lValue2);

            return diff <= units;
        }
        public static TimeSpan Seconds(double s) => TimeSpan.FromSeconds(s);
        public static TimeSpan Milliseconds(double s) => TimeSpan.FromMilliseconds(s);
        
        public static double[][] MatrixCreate(int rows, int cols)
        {
            // creates a matrix initialized to all 0.0s  
            // do error checking here?  
            double[][] result = new double[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new double[cols];
            // auto init to 0.0  
            return result;
        }



        /// <summary>
        /// Constructs an object through reflection with a possibly non-public constructor.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="args">Arguments to pass into the constructor.</param>
        /// <exception cref="MissingMethodException">Thrown when no matching constructor is found.</exception>
        /// <returns>The constructed object.</returns>
        public static T CreateInstanceInternalCtor<T>(params object[] args)
        {
            var type = typeof(T);
            foreach (var ctor in System.Linq.Enumerable.Where(
                System.Reflection.IntrospectionExtensions.GetTypeInfo(type).DeclaredConstructors, x => x.Name == ".ctor"))
                if (System.Linq.Enumerable.SequenceEqual(
                    System.Linq.Enumerable.Select(ctor.GetParameters(), x => x.ParameterType),
                    System.Linq.Enumerable.Select(args, x => x.GetType()))) return (T)ctor.Invoke(args);
            throw new MissingMethodException("No matching constructor found.");
        }
    }
}