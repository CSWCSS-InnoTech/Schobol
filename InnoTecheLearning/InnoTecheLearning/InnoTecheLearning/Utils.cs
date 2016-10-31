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
    public static class Utils
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
        public class IO
        {
            public IO(string FileName,FileMode Mode = FileMode.Create)
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
            ~IO()
            { try { Dispose(); } catch { } }
        }
#endif
        /// <summary>
        /// A class that provides methods to help create the UI.
        /// </summary>
        public static class Create
        {
            public static Button Button(FileImageSource Image, EventHandler OnClick)
            {
                return Button(Image, OnClick, new Size(50, 50));
            }
            public static Button Button(FileImageSource Image, EventHandler OnClick, Size Size)
            {
                Button Button = new Button { Image = (FileImageSource)Image,
                    WidthRequest = Size.Width, HeightRequest = Size.Height };
                Button.Clicked += OnClick;
                return Button;
            }

            public static Button Button(Text Text, EventHandler OnClick, Color TextColor = default(Color))
            {
                return Button(Text, OnClick, TextColor, new Size(50, 50));
            }
            public static Button Button(Text Text, EventHandler OnClick, Color TextColor, Size Size)
            {
                Button Button = new Button { Text = Text, TextColor = TextColor,
                    WidthRequest = Size.Width, HeightRequest = Size.Height };
                Button.Clicked += OnClick;
                return Button;
            }

            public static StackLayout MainScreenItemB(FileImageSource Image, EventHandler OnClick, Text Display)
            {
                return new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children = { Button(Image: Image, OnClick: OnClick), Display }
                };
            }

            public static StackLayout MainScreenItem(ImageSource Source, Action OnTap, Text Display)
            {
                return new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Children = { Image(Source: Source, OnTap: OnTap), Display }
                };
            }

            public static StackLayout MainScreenRow(params StackLayout[] MainScreenItems)
            {
                StackLayout MenuScreenRow = new StackLayout {Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand, Children = { } };
                foreach (StackLayout MenuScreenItem in MainScreenItems)
                    MenuScreenRow.Children.Add(MenuScreenItem);
                return MenuScreenRow;
            }

            public static ImageSource Image(string FileName)
            { return ImageSource.FromResource(CurrentNamespace +".Images." + FileName);
            }

            public enum ImageFile : int
            {
                Forum = 1,
                Translate = 2,
                VocabBook = 3,
                MathConverter = 4,
                MathConverter_Duo = 5,
                Factorizer = 6,
                Sports = 7,
                MusicTuner = 8,
                MathSolver = 9
            }


            public static ImageSource Image(ImageFile File)
            {
                string ActualFile = "";
                switch (File)
                {
                    case ImageFile.Forum:
                        ActualFile = "forum-message-3.png";
                        break;
                    case ImageFile.Translate:
                        ActualFile = "translator-tool-3.png";
                        break;
                      case ImageFile.VocabBook:
                        ActualFile = "book-2.png";
                          break;
                      /*case ImageFile.MathConverter:
                          break;
                      case ImageFile.MathConverter_Duo:
                          break;
                      case ImageFile.Factorizer:
                          break;
                      case ImageFile.Sports:
                          break;
                      case ImageFile.MusicTuner:
                          break;
                      case ImageFile.MathSolver:
                          break;*/
                    default:
                        ActualFile = "";
                        break;
                }
                return Image(ActualFile);
               ;
            }

            public static Image Image(ImageSource Source, Action OnTap)
            {
                return Image(Source, OnTap, new Size(50, 50));
            }
            public static Image Image(ImageSource Source, Action OnTap, Size Size)
            {
                Image Image = new Image
                {
                    Source = Source,
                    WidthRequest = Size.Width,
                    HeightRequest = Size.Height
                };
                var Tap = new TapGestureRecognizer();
                Tap.Command = new Command(OnTap);
                Image.GestureRecognizers.Add(Tap);
                return Image;
            }

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
        {   Task<T> Task = AlertAsync(Return, Page, Title, Message, Cancel);
            Task.Wait();
            return Return;
        }

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
        /// Returns bolded text.
        /// </summary>
        /// <param name="Text">Text to make bold.</param>
        /// <returns></returns>
        public static Span Bold(Text Text)
        { return new Span { Text = Text, FontAttributes = FontAttributes.Bold }; }

        /// <summary>
        /// Trys to convert an <see cref="object"/> instance to a specified <see cref="Type"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to convert to.</typeparam>
        /// <param name="Object">The <see cref="object"/> instance to convert.</param>
        /// <param name="Result">The result of conversion if successful. If not it will be the default value of the <see cref="Type"/> to convert to.</param>
        /// <returns>Whether the conversion has succeeded.</returns>
        public static bool TryCast<T>(object Object, out T Result)
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
        /// <summary>
        /// A piece of ordinary text which is interchangable with a label, span, string and array of chars.
        /// </summary>
        public struct Text : IComparable
        {
            public static Text Null { get { return (string)null; } }
            public static Text Empty { get { return ""; } }
            public string Value { get; set; }
            public Text(string Text)
            { Value = Text; }
            public Text Append(char Char)
            {
                Value += Char;
                return this;
            }
            public Text Clear()
            {
                Value = null;
                return this;
            }
            public Text TrimStart()
            {
                Value = Value.TrimStart();
                return this;
            }
            public Text Trim()
            {
                Value = Value.Trim();
                return this;
            }
            public Text TrimEnd()
            {
                Value = Value.TrimEnd();
                return this;
            }
            /// <summary>
            /// Removes a string of characters from the beginning of this <see cref="Text"/>.
            /// </summary>
            /// <returns>The current <see cref="Text"/> object to continue nested instructions.</returns>
            public Text TrimStart(params char[] TrimChars)
            {
                Value = Value.TrimStart(TrimChars);
                return this;
            }
            /// <summary>
            /// Removes a string of characters from the ends of this <see cref="Text"/>.
            /// </summary>
            /// <returns>The current <see cref="Text"/> object to continue nested instructions.</returns>
            public Text Trim(params char[] TrimChars)
            {
                Value = Value.Trim(TrimChars);
                return this;
            }
            /// <summary>
            /// Removes a string of characters from the end of this <see cref="Text"/>.
            /// </summary>
            /// <returns>The current <see cref="Text"/> object to continue nested instructions.</returns>
            public Text TrimEnd(params char[] TrimChars)
            {
                Value = Value.TrimEnd(TrimChars);
                return this;
            }
            public Text ToLower()
            {
                Value = Value.ToLower();
                return this;
            }
            public Text ToUpper()
            {
                Value = Value.ToUpper();
                return this;
            }
            public Text PadLeft(int TotalWidth)
            {
                Value = Value.PadLeft(TotalWidth);
                return this;
            }
            public Text PadRight(int TotalWidth)
            {
                Value = Value.PadRight(TotalWidth);
                return this;
            }
            public bool StartsWith(char Char)
            { return First() == Char; }
            public bool StartsWith(string String)
            { return Value.StartsWith(String); }
            public bool EndsWith(char Char)
            { return Last() == Char; }
            public bool EndsWith(string String)
            { return Value.EndsWith(String); }
            public char First()
            { return Value[0]; }
            public char Last()
            { return Value[Length - 1]; }
            public Text Remove(int StartIndex)
            {
                Value = Value.Remove(StartIndex);
                return this;
            }
            public Text Remove(int StartIndex, int Count)
            {
                Value = Value.Remove(StartIndex, Count);
                return this;
            }
            /// <summary>
            /// Returns a substring of this <see cref="Text"/>.
            /// </summary>
            /// <returns>The current <see cref="Text"/> object to continue nested instructions.</returns>
            public Text Substring(int StartIndex)
            {
                Value = Value.Substring(StartIndex);
                return this;
            }
            public Text Substring(int StartIndex, int Length)
            {
                Value = Value.Substring(StartIndex, Length);
                return this;
            }
            public Text Replace(char OldChar, char NewChar)
            {
                Value = Value.Replace(OldChar, NewChar);
                return this;
            }
            public Text Replace(string OldString, string NewString)
            {
                Value = Value.Replace(OldString, NewString);
                return this;
            }
            public int Length
            { get { return Value.Length; } }
            public static implicit operator Span(Text Text)
            { return new Span { Text = Text.Value }; }
            public static implicit operator Text(Span Span)
            { return new Text(Span.Text); }
            public static implicit operator string(Text Text)
            { return Text.Value; }
            public static implicit operator Text(string String)
            { return new Text(String); }
            public static implicit operator Label(Text Text)
            { return new Label { Text = Text, TextColor = Color.Black }; }
            public static implicit operator Text(Label Label)
            { return new Text(Label.Text); }
            public static implicit operator char[] (Text Text)
            { return Text.Value.ToCharArray(); }
            public static implicit operator Text(char[] Char)
            { return new Text(new string(Char)); }
            public static explicit operator Button(Text Text)
            { return new Button { Text = Text,TextColor = Color.Black }; }
            public static implicit operator Text(Button Button)
            { return new Text(Button.Text); }
            public static explicit operator SByte(Text Text)
            { return SByte.Parse(Text.Value); }
            public static implicit operator Text(SByte SByte)
            { return new Text(SByte.ToString()); }
            public static implicit operator Byte(Text Text)
            { return Byte.Parse(Text.Value); }
            public static implicit operator Text(Byte Byte)
            { return new Text(Byte.ToString()); }
            public static implicit operator Int16(Text Text)
            { return Int16.Parse(Text.Value); }
            public static implicit operator Text(Int16 Int16)
            { return new Text(Int16.ToString()); }
            public static implicit operator UInt16(Text Text)
            { return UInt16.Parse(Text.Value); }
            public static implicit operator Text(UInt16 UInt16)
            { return new Text(UInt16.ToString()); }
            public static implicit operator Int32(Text Text)
            { return Int32.Parse(Text.Value); }
            public static implicit operator Text(Int32 Int32)
            { return new Text(Int32.ToString()); }
            public static implicit operator UInt32(Text Text)
            { return UInt32.Parse(Text.Value); }
            public static implicit operator Text(UInt32 UInt32)
            { return new Text(UInt32.ToString()); }
            public static implicit operator Int64(Text Text)
            { return Int64.Parse(Text.Value); }
            public static implicit operator Text(Int64 Int64)
            { return new Text(Int64.ToString()); }
            public static implicit operator UInt64(Text Text)
            { return UInt64.Parse(Text.Value); }
            public static implicit operator Text(UInt64 UInt64)
            { return new Text(UInt64.ToString()); }
            public static implicit operator Single(Text Text)
            { return Single.Parse(Text.Value); }
            public static implicit operator Text(Single Single)
            { return new Text(Single.ToString()); }
            public static implicit operator Double(Text Text)
            { return Double.Parse(Text.Value); }
            public static implicit operator Text(Double Double)
            { return new Text(Double.ToString()); }
            public static implicit operator Decimal(Text Text)
            { return Decimal.Parse(Text.Value); }
            public static implicit operator Text(Decimal Decimal)
            { return new Text(Decimal.ToString()); }
            public static implicit operator Boolean(Text Text)
            { return Boolean.Parse(Text.Value); }
            public static implicit operator Text(Boolean Boolean)
            { return new Text(Boolean.ToString()); }
            public static implicit operator DateTime(Text Text)
            { return DateTime.Parse(Text.Value); }
            public static implicit operator Text(DateTime DateTime)
            { return new Text(DateTime.ToString()); }
            public static implicit operator TimeSpan(Text Text)
            { return TimeSpan.Parse(Text.Value); }
            public static implicit operator Text(TimeSpan TimeSpan)
            { return new Text(TimeSpan.ToString()); }
            public static implicit operator IntPtr(Text Text)
            { return new IntPtr(Int32.Parse(Text.Value)); }
            public static implicit operator Text(IntPtr IntPtr)
            { return new Text(IntPtr.ToInt32().ToString()); }
            public static implicit operator UIntPtr(Text Text)
            { return new UIntPtr(UInt32.Parse(Text.Value)); }
            public static implicit operator Text(UIntPtr UIntPtr)
            { return new Text(UIntPtr.ToUInt32().ToString()); }
            public static implicit operator Array(Text Text)
            { return Text.Value.ToCharArray(); }
            public static implicit operator Text(Array Array)
            {
                Text Text = new Text();
                foreach (var Item in Array)
                { Text.Append((char)Item); };
                return Text;
            }
            public override string ToString()
            { return Value; }
            public int CompareTo(string String)
            {
                return Value.CompareTo(String);
            }
            public int CompareTo(object value)
            {
                if (value == null)
                    return 1;
                Text Convert = new Text();
                if (!(value is string))
                    if (TryCast(value, out Convert))
                    { value = Convert; }
                    else
                        throw new ArgumentException("Value must be convertible to string.", "value");
                return string.Compare(Value, (string)value, StringComparison.CurrentCulture);
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

