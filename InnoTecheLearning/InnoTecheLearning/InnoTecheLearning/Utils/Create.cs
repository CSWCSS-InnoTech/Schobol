using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace InnoTecheLearning
{
    public static partial class Utils
    {
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
                Button Button = new Button
                {
                    Image = (FileImageSource)Image,
                    WidthRequest = Size.Width,
                    HeightRequest = Size.Height
                };
                Button.Clicked += OnClick;
                return Button;
            }

            public static Button Button(Text Text, EventHandler OnClick, Color TextColor = default(Color))
            {
                return Button(Text, OnClick, TextColor, new Size(50, 50));
            }
            public static Button Button(Text Text, EventHandler OnClick, Color TextColor, Size Size)
            {
                Button Button = new Button
                {
                    Text = Text,
                    TextColor = TextColor,
                    WidthRequest = Size.Width,
                    HeightRequest = Size.Height
                };
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

            public static StackLayout MainScreenItem(ImageSource Source, Action OnTap, Label Display)
            {
                return new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                    Children = { Image(Source: Source, OnTap: OnTap), Display }
                };
            }

            public static StackLayout MainScreenRow(params StackLayout[] MainScreenItems)
            {
                StackLayout MenuScreenRow = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.StartAndExpand,
					Spacing = 50,
                    Children = { }
                };
                foreach (StackLayout MenuScreenItem in MainScreenItems)
                    MenuScreenRow.Children.Add(MenuScreenItem);
                return MenuScreenRow;
            }

            public static ImageSource Image(string FileName)
            {
                return ImageSource.FromResource(CurrentNamespace + ".Images." + FileName);
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
                    case ImageFile.MathConverter:
                        ActualFile = "square-root-of-x-mathematical-signs.png";
                        break;
                    case ImageFile.MathConverter_Duo:
                        ActualFile = "square-root-of-x-mathematical-signs.png";
                        break;
                    case ImageFile.Factorizer:
                        ActualFile = "mathematical-operation.png";
                        break;
                    case ImageFile.Sports:
                        ActualFile = "man-sprinting.png";
                        break;
                    case ImageFile.MusicTuner:
                        ActualFile = "treble-clef-2.png";
                        break;
                    case ImageFile.MathSolver:
                        ActualFile = "japanese-dragon.png";
                        break;
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
            public static Label BoldLabel(Text Text)
            {   return new Label
                {   Text = Text,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.Black,
                    VerticalTextAlignment = TextAlignment.Start,
                    HorizontalTextAlignment = TextAlignment.Center}; }
            public static Label BoldLabel2(Text Text)
            {   return new Label
                {   FormattedText = Format(Bold(Text)),
                    TextColor = Color.Black,
                    VerticalTextAlignment = TextAlignment.Start,
                    HorizontalTextAlignment = TextAlignment.Center}; }
        }
    }
}