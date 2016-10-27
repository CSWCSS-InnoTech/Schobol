using InnoTecheLearning;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//[assembly: Dependency(typeof(Utils))]

namespace InnoTecheLearning
{/// <summary>
/// A class that provides methods to help run the App.
/// </summary>
    public static class Utils
    {/// <summary>
/// A class that provides methods to help create the UI.
/// </summary>
        public static class Create
        {
            public static Button Button(FileImageSource Image, EventHandler OnClick)
            {
                Button Button = new Button { Image = Image };
                Button.Clicked += OnClick;
                return Button;
            }

            public static Button Button(Text Text, EventHandler OnClick, Color TextColor = default(Color))
            {
                Button Button = new Button { Text = Text };
                Button.Clicked += OnClick;
                return Button;
            }

            public static StackLayout MainScreenItem(FileImageSource Image, EventHandler OnClick, Text Display)
            {
                return new StackLayout
                {
                    VerticalOptions = LayoutOptions.Start,
                    Children = { Button(Image: Image, OnClick: OnClick), Display }
                };
            }

            public static StackLayout MainScreenRow(params StackLayout[] MainScreenItems)
            {
                StackLayout MenuScreenRow = new StackLayout { HorizontalOptions = LayoutOptions.Start, Children = { } };
                foreach (StackLayout MenuScreenItem in MainScreenItems)
                    MenuScreenRow.Children.Add(MenuScreenItem);
                return MenuScreenRow;
            }
            
       public static ImageSource Image(string FileName)
       { return ImageSource.FromResource("InnoTecheLearning.Images." + FileName); }

       public enum ImageFile : int
       {Forum = 1,
       Translate = 2,
       VocabBook = 3,
       MathConverter = 4,
       MathConverter_Duo = 5,
       Factorizer = 6,
       Sports = 7,
       MusicTuner = 8,
       MathSolver = 9}

       public static FileImageSource Image(ImageFile File)
       {   Text ActualFile;
           switch (File)
           {
               case ImageFile.Forum:
                   ActualFile = "forum-message-3.png";
                   break;
             /*case ImageFile.Translate:
                   break;
               case ImageFile.VocabBook:
                   break;
               case ImageFile.MathConverter:
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
           return (string)ActualFile;
       }
        }

        /// <summary>
        /// Making formatted text is #Ez.
        /// </summary>
        /// <param name="Spans">Obviously you will need text to format.</param>
        /// <returns></returns>
        public static FormattedString Format(params Span[] Spans)
        {
            var fs = new FormattedString();
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
        /// A piece of ordinary text which is interchangable with a label, span, string and array of chars.
        /// </summary>
        public struct Text : IComparable
        {
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
            { return new Label { Text = Text }; }
            public static implicit operator Text(Label Label)
            { return new Text(Label.Text); }
            public static implicit operator char[] (Text Text)
            { return Text.Value.ToCharArray(); }
            public static implicit operator Text(char[] Char)
            { return new Text(new string(Char)); }
            public static explicit operator Button(Text Text)
            { return new Button{ Text = Text};}
            public static implicit operator Text(Button Button)
            { return new Text(Button.Text);}
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
                {
                    return 1;
                }
                Text Convert = new Text();
                if (value is string) { }
                else if (TryCast(value, out Convert))
                { value = Convert; }
                else{
                        throw new ArgumentException("Value must be convertible to string.", "value");
                    }

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

