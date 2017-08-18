using System;
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
                Facial
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
                    "boy-smiling.png"
                };
            public static ImageSource ImageSource(ImageFile File) => ImageSource(ImageFileSelection[(int)File]);
            public static ImageSource ImageSource(string FileName) => Log(Xamarin.Forms.ImageSource.FromResource
                (CurrentNamespace + ".Images." + FileName, typeof(Utils)), "ImageSource created: " + FileName);

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

            public static StackLayout MainScreenItem(ImageSource Source, Action OnTap, Label Display)
            {
                return Log(new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                    WidthRequest = 71.5,
                    Scale = 1.5,
                    Children = { Image(Source: Source, OnTap: OnTap), Display }
                }, "MainScreenItem returned: " + Display.Text);
            }
        }
    }
}