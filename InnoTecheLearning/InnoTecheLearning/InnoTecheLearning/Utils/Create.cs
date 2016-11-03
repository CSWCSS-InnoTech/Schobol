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
            [Obsolete("Use Create.Image(ImageSource Source, Action OnTap) instead.\nDeprecated in 0.10.0a46")]
            public static Button Button(FileImageSource Image, EventHandler OnClick)
            {   return Button(Image, OnClick, new Size(50, 50));}
            [Obsolete("Use Create.Image(ImageSource Source, Action OnTap, Size Size) instead.\nDeprecated in 0.10.0a46")]
            public static Button Button(FileImageSource Image, EventHandler OnClick, Size Size)
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

            public static Button Button(Text Text, EventHandler OnClick,Color BackColor =
                default(Color), Color TextColor = default(Color))
            {
                if (BackColor == default(Color))
                    BackColor = Color.Silver;
                if (TextColor == default(Color))
                    TextColor = Color.Black;
                Button Button = new Button{Text = Text, TextColor = TextColor, BackgroundColor = BackColor};
                Button.Clicked += OnClick;
                return Button;
            }
            public static Button Button(Text Text, EventHandler OnClick, Size Size,
                 Color BackColor = default(Color), Color TextColor = default(Color))
            {
                if (BackColor == default(Color))
                    BackColor = Color.Silver;
                if (TextColor == default(Color))
                    TextColor = Color.Black;
                Button Button = new Button
                {
                    Text = Text,
                    TextColor = TextColor,
                    WidthRequest = Size.Width,
                    HeightRequest = Size.Height,
                    BackgroundColor = BackColor
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
                    WidthRequest = 70,
                    Children = { Image(Source: Source, OnTap: OnTap), Display }
                };
            }

            public static StackLayout MainScreenRow(params StackLayout[] MainScreenItems)
            {
                StackLayout MenuScreenRow = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
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
                MathSolver = 9,
                Cello = 10,
                Violin = 11
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
                    case ImageFile.Cello:
                        ActualFile = "cello-icon.png";
                        break;
                    case ImageFile.Violin:
                        ActualFile = "violin-icon.png";
                        break;
                    default:
                        ActualFile = "";
                        break;
                }
                return Image(ActualFile);
                ;
            }
            public static Image Image(ImageFile File, Action OnTap)
            { return Image(Image(File), OnTap); }
            public static Image Image(ImageFile File, Action OnTap, Size Size)
            { return Image(Image(File), OnTap, Size); }
            public static Image ImageD/*D = Default (size)*/(ImageFile File, Action OnTap)
            { return ImageD(Image(File), OnTap); }
            public static Image ImageD/*D = Default (size)*/(ImageSource Source, Action OnTap)
            {
                Image Image = new Image{Source = Source};
                var Tap = new TapGestureRecognizer();
                Tap.Command = new Command(OnTap);
                Image.GestureRecognizers.Add(Tap);
                return Image;
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
            public static Label BoldLabel(Text Text, Color TextColor = default(Color))
            {
                if (TextColor == default(Color))
                    TextColor = Color.Black;
                return new Label
                {
                    Text = Text,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = TextColor,
                    VerticalTextAlignment = TextAlignment.Start,
                    HorizontalTextAlignment = TextAlignment.Center
                };
            }
            public static Label BoldLabel2(Text Text, Color TextColor = default(Color))
            {
                if (TextColor == default(Color))
                    TextColor = Color.Black;
                return new Label
                {
                    FormattedText = Format(Bold(Text)),
                    TextColor = TextColor,
                    VerticalTextAlignment = TextAlignment.Start,
                    HorizontalTextAlignment = TextAlignment.Center
                };
            }
            public static ScrollView Changelog
            {
                get
                {
                    var Return = new ScrollView
                    {
                        Content = new Label
                        {
                            Text = Resources.GetString("Change.log"),
                            TextColor = Color.Black,
                            LineBreakMode = LineBreakMode.WordWrap,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            HorizontalOptions = LayoutOptions.FillAndExpand
                        },
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand
                    };
                    /*Return.SizeChanged += (object sender, EventArgs e) =>
                    {
                        var View = (View)sender;
                        if (View.Width <= 0 || View.Height <= 0) return;
                        Return.WidthRequest = View.Width; Return.HeightRequest = View.Height;
                    };*/
                    return Return;
                }
            }
            public static Label Version
            {
                get
                {
                    return new Label
                    {
                        Text = "Version: " + VersionFull,
                        HorizontalTextAlignment = TextAlignment.End,
                        VerticalTextAlignment = TextAlignment.Start,
                        LineBreakMode = LineBreakMode.NoWrap,
                        TextColor = Color.Black
                    };
                }
            }
            public static StackLayout ChangelogView(Page Page, Color BackColor = default(Color))
            {
                Button Back = Button("Back", delegate { Page.SendBackButtonPressed(); }, Color.Silver);
                ScrollView Changelog = Create.Changelog;
                if (BackColor == default(Color))
                    BackColor = Color.White;
                return new StackLayout
                {
                    Children = { Changelog, Back },
                    BackgroundColor = BackColor,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill
                };
            }
            public static Label Title(Text Text)
            {
                return new Label
                {
                    FontSize = 25,
                    BackgroundColor = Color.FromUint(4285098345),
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.White,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.Start,
                    Text = Text
                };
            }
            public static Label Society
            {
                get
                {
                    return new Label
                    {
                        HorizontalTextAlignment = TextAlignment.Center,
                        TextColor = Color.Black,
                        FormattedText = Format((Text)"Developed by the\n", Bold("Innovative Technology Society of CSWCSS"))
                    };
                }
            }

            public static StackLayout Row(params View[] Items)
            {
                StackLayout MenuScreenRow = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Children = { }
                };
                foreach (StackLayout MenuScreenItem in Items)
                    MenuScreenRow.Children.Add(MenuScreenItem);
                return MenuScreenRow;
            }
            public static StackLayout Column(params View[] Items)
            {
                StackLayout MenuScreenRow = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Children = { }
                };
                foreach (StackLayout MenuScreenItem in Items)
                    MenuScreenRow.Children.Add(MenuScreenItem);
                return MenuScreenRow;
            }
        }
    }
}