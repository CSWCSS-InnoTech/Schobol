using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace InnoTecheLearning
{
    partial class Utils
    {
        partial class Create
        {
            public enum ImageFile : int
            {
                Forum,
                Translate,
                VocabBook,
                Calculator,
                Calculator_Free,
                Factorizer,
                Sports,
                MusicTuner,
                MathSolver,
                Cello,
                Violin,
                Heart,
                Dragon,
                Dragon_Dead,
                File_Icon
            }

            public static string[] ImageFileSelection { get; } =
                new[]
                {
                    "forum-message-3.png",
                    "translator-tool-3.png",
                    "book-2.png",
                    "square-root-of-x-mathematical-signs.png",
                    "square-root-of-x-mathematical-signs.png",
                    "mathematical-operation.png",
                    "man-sprinting.png",
                    "treble-clef-2.png",
                    "japanese-dragon.png",
                    "cello-icon.png",
                    "violin-icon.png",
                    "8_bit_heart_stock_by_xquatrox-d4r844m.png",
                    "dragon.jpg",
                    "dragon.fw.png",
                    "folded-paper_318-31112.jpg"
                };
            public static ImageSource ImageSource(ImageFile File) => ImageSource(ImageFileSelection[(int)File]);
            public static ImageSource ImageSource(string FileName) => Xamarin.Forms.ImageSource.FromResource
                (CurrentNamespace + ".Images." + FileName, typeof(Utils));

            public static Image Image(ImageFile File, Action OnTap) => Image(ImageSource(File), OnTap);
            public static Image Image(ImageFile File, Action OnTap, Size Size) => Image(ImageSource(File), OnTap, Size);
            public static Image Image(ImageSource Source, Action OnTap) => Image(Source, OnTap, new Size(50, 50));
            public static Image Image(ImageSource Source, Action OnTap, Size Size)
            {
                Image Image = new Image
                {
                    Source = Source,
                    WidthRequest = Size.Width,
                    HeightRequest = Size.Height
                };
                var Tap = new TapGestureRecognizer { Command = new Command(OnTap) };
                Image.GestureRecognizers.Add(Tap);
                return Image;
            }

            public static Image ImageD/*D = Default (size)*/(ImageFile File, Action OnTap) => ImageD(ImageSource(File), OnTap);
            public static Image ImageD/*D = Default (size)*/(ImageSource Source, Action OnTap)
            {
                Image Image = new Image { Source = Source };
                var Tap = new TapGestureRecognizer { Command = new Command(OnTap) };
                Image.GestureRecognizers.Add(Tap);
                return Image;
            }

            public static StackLayout MainScreenItem(ImageSource Source, Action OnTap, Label Display)
            {
                if(Device.Idiom == TargetIdiom.Desktop) return new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    //WidthRequest = 71.5,
                    Children = {
                        Image(Source: Source, OnTap: OnTap)
                            .With((ref Image x) => {
                                x.HorizontalOptions = LayoutOptions.FillAndExpand;
                                x.VerticalOptions = LayoutOptions.FillAndExpand;
                                x.Aspect = Aspect.AspectFit;
                                var a = x;
                                Device.StartTimer(TimeSpan.FromMilliseconds(10),
                                    () => { a.WidthRequest = ((a.Parent as StackLayout)?.Height - a.Height)
                                        ?? a.WidthRequest; return false; });
                                }
                            ), Display
                            .With((ref Label x) => {
                                x.HorizontalOptions = x.VerticalOptions = LayoutOptions.FillAndExpand;
                                x.HorizontalTextAlignment = TextAlignment.Center;
                                var a = x;
                                Device.StartTimer(TimeSpan.FromMilliseconds(10),
                                    () => { a.WidthRequest = ((a.Parent as StackLayout)?.Height - a.Height)
                                        ?? a.WidthRequest; return false; });
                                }
                            )
                    }
                };
                else return new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                    WidthRequest = 71.5,
                    Children = { Image(Source: Source, OnTap: OnTap), Display }
                };
            }
            [Obsolete("Use Create.Image(ImageSource Source, Action OnTap) instead.\nDeprecated in 0.10.0a46")]
            public static Button ButtonB(FileImageSource Image, EventHandler OnClick)
            { return ButtonB(Image, OnClick, new Size(50, 50)); }
            [Obsolete("Use Create.Image(ImageSource Source, Action OnTap, Size Size) instead.\nDeprecated in 0.10.0a46")]
            public static Button ButtonB(FileImageSource Image, EventHandler OnClick, Size Size)
            {
                Button Button = new Button
                {
                    Image = Image,
                    WidthRequest = Size.Width,
                    HeightRequest = Size.Height
                };
                Button.Clicked += OnClick;
                return Button;
            }

            [Obsolete("Use MainScreenItem(ImageSource Source, Action OnTap, Label Display) instead.\nDeprecated in 0.10.0a46")]
            public static StackLayout MainScreenItemB/*B = Button*/(FileImageSource Image, EventHandler OnClick, Text Display)
            {
                return new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { ButtonB(Image: Image, OnClick: OnClick), Display }
                };
            }
        }
    }
}