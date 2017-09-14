using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public static T RunSynchronously<T>(this ValueTask<T> Task, T Default = default)
        {
            T Result = Default;
            using (System.Threading.AutoResetEvent Wait = new System.Threading.AutoResetEvent(false))
            {
                using (AsyncHelper.AsyncBridge Helper = AsyncHelper.Wait)
                    Helper.Run(Task, (ValueTask<T> CallBack) => { Result = CallBack.Result; Wait.Set(); });
                Wait.WaitOne();
            }
            return Result;
        }

        public static void AddRange<T>(this ICollection<T> ic, IEnumerable<T> ie) { foreach (T obj in ie) ic.Add(obj); }

        public static (int x, int y) IndicesOf<T>(this T[,] matrix, T value)
        {
            int w = matrix.GetLength(0); // width
            int h = matrix.GetLength(1); // height

            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    if (matrix[x, y].Equals(value))
                        return (x, y);
                }
            }

            return (-1, -1);
        }

        public static byte[] ReadFully(this Stream input, bool reset = false)
        {
            long pos = input.Position;
            try
            {
                if (reset && input.CanSeek) input.Seek(0, SeekOrigin.Begin);
                if (input is MemoryStream)
                {
                    return ((MemoryStream)input).ToArray();
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        input.CopyTo(ms);
                        return ms.ToArray();
                    }
                }
            }
            finally
            {
                if (reset && input.CanSeek) input.Seek(pos, SeekOrigin.Begin);
            }
        }

        public static string ToHex(this byte[] data, string prefix = "")
        {
            char[] lookup = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            int i = 0, p = prefix.Length, l = data.Length;
            char[] c = new char[l * 2 + p];
            byte d;
            for (; i < p; ++i) c[i] = prefix[i];
            i = -1;
            --l;
            --p;
            while (i < l)
            {
                d = data[++i];
                c[++p] = lookup[d >> 4];
                c[++p] = lookup[d & 0xF];
            }
            return new string(c, 0, c.Length);
        }
        public static byte[] FromHex(this string str, int offset = 0, int step = 0, int tail = 0)
        {
            byte[] b = new byte[(str.Length - offset - tail + step) / (2 + step)];
            byte c1, c2;
            int l = str.Length - tail;
            int s = step + 1;
            for (int y = 0, x = offset; x < l; ++y, x += s)
            {
                c1 = (byte)str[x];
                if (c1 > 0x60) c1 -= 0x57;
                else if (c1 > 0x40) c1 -= 0x37;
                else c1 -= 0x30;
                c2 = (byte)str[++x];
                if (c2 > 0x60) c2 -= 0x57;
                else if (c2 > 0x40) c2 -= 0x37;
                else c2 -= 0x30;
                b[y] = (byte)((c1 << 4) + c2);
            }
            return b;
        }
        
        /// <summary>
        /// Adds a <see cref="View"/> to an <see cref="AbsoluteLayout"/>.
        /// </summary>
        /// <param name="Layout">The <see cref="AbsoluteLayout"/> to add the <see cref="View"/> into.</param>
        /// <param name="View">The <see cref="View"/> to add into the <see cref="AbsoluteLayout"/>.</param>
        /// <param name="RelativeX">Relative to the <see cref="AbsoluteLayout"/>, between 0.0 and 1.0</param>
        /// <param name="RelativeY">Relative to the <see cref="AbsoluteLayout"/>, between 0.0 and 1.0</param>
        /// <param name="Flags">Options to layout in the <see cref="AbsoluteLayout"/>.</param>
        public static void AddPosition(this AbsoluteLayout Layout, View View, double RelativeX, double RelativeY,
            AbsoluteLayoutFlags Flags = AbsoluteLayoutFlags.PositionProportional)
        {
            // PositionProportional flag maps the range (0.0, 1.0) to
            // the range "flush [left|top]" to "flush [right|bottom]"
            AbsoluteLayout.SetLayoutFlags(View, Flags);
            AbsoluteLayout.SetLayoutBounds(View, new Rectangle(RelativeX, RelativeY,
                AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            Layout.Children.Add(View);
        }
        /// <summary>
        /// Adds a <see cref="View"/> to an <see cref="AbsoluteLayout"/>.
        /// </summary>
        /// <param name="Layout">The <see cref="AbsoluteLayout"/> to add the <see cref="View"/> into.</param>
        /// <param name="View">The <see cref="View"/> to add into the <see cref="AbsoluteLayout"/>.</param>
        /// <param name="RelativeX">Relative to the <see cref="AbsoluteLayout"/>, between 0.0 and 1.0</param>
        /// <param name="RelativeY">Relative to the <see cref="AbsoluteLayout"/>, between 0.0 and 1.0</param>
        /// <param name="RelativeWidth">Relative to the <see cref="AbsoluteLayout"/>, between 0.0 and 1.0</param>
        /// <param name="RelativeHeight">Relative to the <see cref="AbsoluteLayout"/>, between 0.0 and 1.0</param>
        /// <param name="Flags">Options to layout in the <see cref="AbsoluteLayout"/>.</param>
        public static void AddPosition(this AbsoluteLayout Layout, View View, double RelativeX, double RelativeY,
            double RelativeWidth, double RelativeHeight,
            AbsoluteLayoutFlags Flags = AbsoluteLayoutFlags.All)
        {
            // PositionProportional flag maps the range (0.0, 1.0) to
            // the range "flush [left|top]" to "flush [right|bottom]"
            AbsoluteLayout.SetLayoutFlags(View, Flags);
            AbsoluteLayout.SetLayoutBounds(View, new Rectangle(RelativeX, RelativeY, RelativeWidth, RelativeHeight));
            Layout.Children.Add(View);
        }
        /// <summary>
        /// Adds a <see cref="View"/> to an <see cref="AbsoluteLayout"/>.
        /// </summary>
        /// <param name="Layout">The <see cref="AbsoluteLayout"/> to add the <see cref="View"/> into.</param>
        /// <param name="View">The <see cref="View"/> to add into the <see cref="AbsoluteLayout"/>.</param>
        /// <param name="Relative">Relative to the <see cref="AbsoluteLayout"/>, between 0.0 and 1.0</param>
        /// <param name="Flags">Options to layout in the <see cref="AbsoluteLayout"/>.</param>
        public static void AddPosition(this AbsoluteLayout Layout, View View, Rectangle Relative,
            AbsoluteLayoutFlags Flags = AbsoluteLayoutFlags.All)
        {
            // PositionProportional flag maps the range (0.0, 1.0) to
            // the range "flush [left|top]" to "flush [right|bottom]"
            AbsoluteLayout.SetLayoutFlags(View, Flags);
            AbsoluteLayout.SetLayoutBounds(View, Relative);
            Layout.Children.Add(View);
        }
        /// <summary>
        /// Adds a <see cref="View"/> to an <see cref="RelativeLayout"/>, and fills the <see cref="RelativeLayout"/>.
        /// </summary>
        /// <param name="Layout">The <see cref="RelativeLayout"/> to add the <see cref="View"/> into.</param>
        /// <param name="View">The <see cref="View"/> to add into the <see cref="AbsoluteLayout"/>.</param>
        public static void Add(this RelativeLayout Layout, View View)
        {
            Layout.Children.Add(View,
                        Constraint.Constant(0),
                        Constraint.Constant(0),
                        Constraint.RelativeToParent((parent) => { return parent.Width; }),
                        Constraint.RelativeToParent((parent) => { return parent.Height; }));
        }
        /// <summary>
        /// Casting as a method.
        /// </summary>
        /// <typeparam name="T"><see cref="Type"/> to <see cref="Convert"/> to.</typeparam>
        /// <param name="Object"><see cref="Object"/> to <see cref="Convert"/> from.</param>
        /// <returns>The casted <see cref="Object"/>.</returns>
        public static T Cast<T>(this object Object) => (T)Object;

#if NETFX_CORE
        public static global::Windows.UI.Color ToWindows(this Color color)
        {
            return global::Windows.UI.Color.FromArgb(Convert.ToByte(color.A * 255),
                Convert.ToByte(color.R * 255), Convert.ToByte(color.G * 255), Convert.ToByte(color.B * 255));
        }
        public static Color ToColor(this global::Windows.UI.Color Color)
        { return new Color(Color.R / 255, Color.G / 255, Color.B / 255, Color.A / 255); }
        public static Color ToColor(this global::Windows.UI.Xaml.Media.Brush Brush)
        { return ToColor(((global::Windows.UI.Xaml.Media.SolidColorBrush)Brush).Color); }
#endif

        public static int LowerBound(this int Num, int Bound) => (Num < Bound) ? Bound : Num;
        public static int UpperBound(this int Num, int Bound) => (Num > Bound) ? Bound : Num;
        public static double LowerBound(this double Num, double Bound) => (Num < Bound) ? Bound : Num;
        public static double UpperBound(this double Num, double Bound) => (Num > Bound) ? Bound : Num;

        public static Dictionary<TKey, TValue> Append<TKey, TValue>
            (this Dictionary<TKey, TValue> Dict, TKey key, TValue value)
        { Dict.Add(key, value); return Dict; }

        public static T Random<T>(this IEnumerable<T> IE) =>
            System.Linq.Enumerable.Count(IE) == 0 ? default :
            System.Linq.Enumerable.ElementAt(IE, Text.Rnd.Next(System.Linq.Enumerable.Count(IE)));

        public static void Ignore(this object Instance) => Instance.ToString();
#if false
#if __IOS__
        public static IEnumerable<Foundation.NSLocale> ToLocale(this SpeechLanguages Langs)
        {
            if (Langs.HasFlag(SpeechLanguages.Default))
                yield return Foundation.NSLocale.CurrentLocale;
            else if (Langs.HasFlag(SpeechLanguages.System))
                yield return Foundation.NSLocale.SystemLocale;
            else if (Langs.HasFlag(SpeechLanguages.English_US))
                yield return Foundation.NSLocale.FromLocaleIdentifier("en_US");
            else if (Langs.HasFlag(SpeechLanguages.English_UK))
                yield return Foundation.NSLocale.FromLocaleIdentifier("en_GB");
            else if (Langs.HasFlag(SpeechLanguages.Chinese_Simplified))
                yield return Foundation.NSLocale.FromLocaleIdentifier("zh_CN");
#pragma warning disable 618 //Justification: Intended to use (disable 612 for Obsolete without message)
            else if (Langs.HasFlag(SpeechLanguages.Chinese_Traditional))
                yield return Foundation.NSLocale.FromLocaleIdentifier("zh_HK");
            else if (Langs.HasFlag(SpeechLanguages.Cantonese))
                yield return Foundation.NSLocale.FromLocaleIdentifier("yue_HK");
#pragma warning restore 618
            else
                throw new ArgumentException($"Invalid language. Value: {Langs}", nameof(Langs));
        }
#elif __ANDROID__
        public static IEnumerable<Java.Util.Locale> ToLocale(this SpeechLanguages Langs)
        {
            if (Langs.HasFlag(SpeechLanguages.Default))
                yield return Java.Util.Locale.Default;
            else if (Langs.HasFlag(SpeechLanguages.System))
                yield return Java.Util.Locale.Root;
            else if (Langs.HasFlag(SpeechLanguages.English_US))
                yield return Java.Util.Locale.Us;
            else if (Langs.HasFlag(SpeechLanguages.English_UK))
                yield return Java.Util.Locale.Uk;
            else if (Langs.HasFlag(SpeechLanguages.Chinese_Simplified))
                yield return Java.Util.Locale.SimplifiedChinese;
#pragma warning disable 618 //Justification: Intended to use (disable 612 for Obsolete without message)
            else if (Langs.HasFlag(SpeechLanguages.Chinese_Traditional))
                yield return Java.Util.Locale.TraditionalChinese;
            else if (Langs.HasFlag(SpeechLanguages.Cantonese))
                yield return new Java.Util.Locale("zh", "HK");
#pragma warning restore 618
            else
                throw new ArgumentException($"Invalid language. Value: {Langs}", nameof(Langs));
        }
#else
        public static IEnumerable<Windows.Globalization.Language> ToLocale(this SpeechLanguages Langs)
        {
            if (Langs.HasFlag(SpeechLanguages.Default))
                yield return new Windows.Globalization.Language(Windows.System.UserProfile.GlobalizationPreferences.Languages[0]);
            else if (Langs.HasFlag(SpeechLanguages.System))
                yield return Windows.Media.SpeechRecognition.SpeechRecognizer.SystemSpeechLanguage;
            else if (Langs.HasFlag(SpeechLanguages.English_US))
                yield return new Windows.Globalization.Language("en-US");
            else if (Langs.HasFlag(SpeechLanguages.English_UK))
                yield return new Windows.Globalization.Language("en-UK");
            else if (Langs.HasFlag(SpeechLanguages.Chinese_Simplified))
                yield return new Windows.Globalization.Language("zh-CN");
#pragma warning disable 618 //Justification: Intended to use (disable 612 for Obsolete without message)
            else if (Langs.HasFlag(SpeechLanguages.Chinese_Traditional))
                yield return new Windows.Globalization.Language("zh-HK");
            else if (Langs.HasFlag(SpeechLanguages.Cantonese))
                yield return new Windows.Globalization.Language("yue-HK");
#pragma warning restore 618
            else
                throw new ArgumentException($"Invalid language. Value: {Langs}", nameof(Langs));
        }
#endif
        public static IEnumerable<string> ToIdentiifiers(this SpeechLanguages Langs)
        {
            if (Langs.HasFlag(SpeechLanguages.Default))
                foreach (var Item in System.Linq.Enumerable.Select(Langs.ToLocale(), L => L.
#if __IOS__
                LocaleIdentifier
#elif __ANDROID__
                ToLanguageTag()
#else
                LanguageTag
#endif
                )) yield return Item;
            else if (Langs.HasFlag(SpeechLanguages.System))
                foreach (var Item in System.Linq.Enumerable.Select(Langs.ToLocale(), L => L.
#if __IOS__
                LocaleIdentifier
#elif __ANDROID__
                ToLanguageTag()
#else
                LanguageTag
#endif
                )) yield return Item;
            else if (Langs.HasFlag(SpeechLanguages.English_US))
                yield return "en-US";
            else if (Langs.HasFlag(SpeechLanguages.English_UK))
                yield return "en-UK";
            else if (Langs.HasFlag(SpeechLanguages.Chinese_Simplified))
                yield return "zh-CN";
#pragma warning disable 618 //Justification: Intended to use (disable 612 for Obsolete without message)
            else if (Langs.HasFlag(SpeechLanguages.Chinese_Traditional))
                yield return "zh-HK";
            else if (Langs.HasFlag(SpeechLanguages.Cantonese))
                yield return "yue-HK";
#pragma warning restore 618
            else
                throw new ArgumentException($"Invalid language. Value: {Langs}", nameof(Langs));
        }
        public static IEnumerable<string> ToGeneralIdentiifiers(this SpeechLanguages Langs)
        {
            foreach (var Item in Langs.ToIdentiifiers())
                yield return Item.Split('_', '-')[0];
        }
#endif
        public static T DebugWrite<T>(this T Object, string Format = "{0}\n", string Category = null)
        { System.Diagnostics.Debug.Write(string.Format(Format, Object), Category); return Object; }

        public static async ValueTask<Stream> GetStream(this ImageSource Source)
        {
            switch (Source)
            {
                case FileImageSource File:
                    return await Storage.GetReadStream(File.File);
                case StreamImageSource Stream:
                    return await Stream.Stream?.Invoke(System.Threading.CancellationToken.None);
                case UriImageSource Uri:
                    switch (Uri.Uri.Scheme)
                    {
                        case "http":
                        case "https":
                        case "ftp":
                        case "file":
                            return (await System.Net.WebRequest.Create(Uri.Uri).GetResponseAsync()).GetResponseStream();
#if WINDOWS_UWP
                        case "ms-appdata":
                        case "ms-appx":
                            return await (await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(Uri.Uri)
                                ).OpenStreamForReadAsync();
#endif
                    }
                    break;
            }
            throw new ArgumentException(
                "Source is either an UriImageSource with unknown scheme or an unknown class derivating from ImageSource."
                , nameof(Source));
        }

        public delegate T WithDelegate<T>(ref T Instance);
        public delegate void WithDelegateVoid<T>(ref T Instance);
        public delegate TIgnore WithDelegateIgnore<T, TIgnore>(ref T Instance);
        public static T With<T>(this T Instance, WithDelegate<T> Chain) => Chain(ref Instance);
        public static T With<T>(this T Instance, WithDelegateVoid<T> Action) { Action(ref Instance); return Instance; }
        public static T With<T, TIgnore>(this T Instance, WithDelegateIgnore<T, TIgnore> Function)
        { Function(ref Instance); return Instance; }

        public static bool IsInteger(this float d) => d == Math.Truncate(d);
        public static bool IsInteger(this double d) => d == Math.Truncate(d);
        public static bool NearInteger(this float d, int maxDifference = 11) =>
            HasMinimalDifference(d, Math.Round(d), maxDifference);
        public static bool NearInteger(this double d, int maxDifference = 22) =>
            HasMinimalDifference(d, Math.Round(d), maxDifference);
        public static bool IsInteger(this decimal d) => d == Math.Truncate(d);

        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> ie, T Item) => new PrependIterator<T>(ie, Item);
        class PrependIterator<T> : IEnumerable<T>, IEnumerator<T>
        {
            public PrependIterator(IEnumerable<T> ie, T Item)
            {
                UnderIterator =
                    (Under = ie ?? throw new ArgumentNullException(nameof(ie)))
                    .GetEnumerator() ?? throw new NullReferenceException($"{nameof(ie)}.GetEnumerator() returned null.");
                Prepend = Item;
            }
            IEnumerable<T> Under;
            IEnumerator<T> UnderIterator;
            T Prepend;
            bool Prepended = false;

            object System.Collections.IEnumerator.Current => Current;
            public T Current { get; private set; }

            public void Dispose() => UnderIterator.Dispose();

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
            public IEnumerator<T> GetEnumerator() => this;

            public bool MoveNext()
            {
                if (Prepended)
                {
                    bool Return = UnderIterator.MoveNext();
                    if (Return) Current = UnderIterator.Current;
                    return Return;
                }
                else
                {
                    Current = Prepend;
                    Prepended = true;
                    return true;
                }
            }

            public void Reset() { UnderIterator.Reset(); Prepended = false; }
        }
        
        public static IEnumerable<T> Append<T>(this IEnumerable<T> ie, T Item) => new AppendIterator<T>(ie, Item);
        class AppendIterator<T> : IEnumerable<T>, IEnumerator<T>
        {
            public AppendIterator(IEnumerable<T> ie, T Item)
            {
                UnderIterator =
                    (Under = ie ?? throw new ArgumentNullException(nameof(ie)))
                    .GetEnumerator() ?? throw new NullReferenceException($"{nameof(ie)}.GetEnumerator() returned null.");
                Append = Item;
            }
            IEnumerable<T> Under;
            IEnumerator<T> UnderIterator;
            T Append;
            bool Ending;

            object System.Collections.IEnumerator.Current => Current;
            public T Current { get; private set; }

            public void Dispose() => UnderIterator.Dispose();

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
            public IEnumerator<T> GetEnumerator() => this;

            public bool MoveNext()
            {
                if (Ending) return false;
                bool Return = UnderIterator.MoveNext();
                if (Return)
                {
                    Current = UnderIterator.Current;
                    return true;
                }
                else
                {
                    Current = Append;
                    Ending = true;
                    return true;
                }
            }

            public void Reset() { UnderIterator.Reset(); Ending = false; }

        }
    }
}