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
        public static partial class Create
        {
            public static Button ButtonU/*Uncoloured*/(Text Text, EventHandler OnClick, Color BackColor =
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
            public static Button ButtonU/*Uncoloured*/(Text Text, EventHandler OnClick, Size Size,
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
            public delegate void ButtonOnClick(ref Button sender, EventArgs e);
            public static Button Button(Text Text, Action OnClick,
                 Color BackColor = default(Color), Color TextColor = default(Color)) =>
                Button(Text, (ref Button sender, EventArgs e) => OnClick(), BackColor, TextColor);
            public static Button Button(Text Text, Action OnClick, Size Size,
                 Color BackColor = default(Color), Color TextColor = default(Color)) =>
                Button(Text, (ref Button sender, EventArgs e) => OnClick(), Size,
                    BackColor, TextColor);
            public static Button Button(Text Text, Func<System.Threading.Tasks.Task> OnClickAsync,
                 Color BackColor = default(Color), Color TextColor = default(Color)) =>
                Button(Text, (ref Button sender, EventArgs e) => System.Threading.Tasks.Task.Run(OnClickAsync),
                    BackColor, TextColor);
            public static Button Button(Text Text, Func<System.Threading.Tasks.Task> OnClickAsync, Size Size,
                 Color BackColor = default(Color), Color TextColor = default(Color)) =>
                Button(Text, (ref Button sender, EventArgs e) => System.Threading.Tasks.Task.Run(OnClickAsync), Size, 
                    BackColor, TextColor);
            public static Button Button(Text Text, ButtonOnClick OnClick, Color BackColor =
                default(Color), Color TextColor = default(Color))
            {
                if (BackColor == default(Color))
                    BackColor = Color.Silver;
                if (TextColor == default(Color))
                    TextColor = Color.Black;
                Button Button = new Button { Text = Text, TextColor = TextColor, BackgroundColor = BackColor };
                Button.Clicked += (sender, e) => { OnClick(ref Button, e); };
                return Button;
            }
            public static Button Button(Text Text, ButtonOnClick OnClick, Size Size,
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
                Button.Clicked += (sender, e) => { OnClick(ref Button, e); };
                return Button;
            }
            public class ExpressionEventArgs : EventArgs
            {   public ExpressionEventArgs(Expressions Expression) : base() { this.Expression = Expression; }
                public Expressions Expression { get; } }
            public static Button Button(Expressions Expression, EventHandler<ExpressionEventArgs> OnClick, 
                Color BackColor = default(Color), Color TextColor = default(Color))
            {
                if (BackColor == default(Color))
                    BackColor = Color.Silver;
                if (TextColor == default(Color))
                    TextColor = Color.Black;
                Button Button = new Button { Text = Expression.AsString(),
                    TextColor = TextColor, BackgroundColor = BackColor };
                Button.Clicked += (object sender, EventArgs e)=> { OnClick(sender, new ExpressionEventArgs(Expression)); };
                return Button;
            }
            public static Button Button(Expressions Expression, EventHandler<ExpressionEventArgs> OnClick, 
                Size Size, Color BackColor = default(Color), Color TextColor = default(Color))
            {
                if (BackColor == default(Color))
                    BackColor = Color.Silver;
                if (TextColor == default(Color))
                    TextColor = Color.Black;
                Button Button = new Button
                {
                    Text = Expression.AsString(),
                    TextColor = TextColor,
                    WidthRequest = Size.Width,
                    HeightRequest = Size.Height,
                    BackgroundColor = BackColor
                };
                Button.Clicked += (object sender, EventArgs e) => { OnClick(sender, new ExpressionEventArgs(Expression)); };
                return Button;
            }
            public static Button Button(Expressions Expression, EventHandler<ExpressionEventArgs> OnClick,
                Text Text, Color BackColor = default(Color), Color TextColor = default(Color))
            {
                if (BackColor == default(Color))
                    BackColor = Color.Silver;
                if (TextColor == default(Color))
                    TextColor = Color.Black;
                Button Button = new Button
                {
                    Text = Text,
                    TextColor = TextColor,
                    BackgroundColor = BackColor
                };
                Button.Clicked += (object sender, EventArgs e) => { OnClick(sender, new ExpressionEventArgs(Expression)); };
                return Button;
            }
            public static Button Button(Expressions Expression, EventHandler<ExpressionEventArgs> OnClick,
                Text Text, Size Size, Color BackColor = default(Color), Color TextColor = default(Color))
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
                Button.Clicked += (object sender, EventArgs e) => { OnClick(sender, new ExpressionEventArgs(Expression)); };
                return Button;
            }

            public static StackLayout MainScreenRow(params View[] MainScreenItems)
            {
                StackLayout MenuScreenRow = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Spacing = 50,
                    Children = { }
                };
                foreach (View MenuScreenItem in MainScreenItems)
                    MenuScreenRow.Children.Add(MenuScreenItem);
                return MenuScreenRow;
            }

            public static StackLayout MainScreenRow<T>(params T[] MainScreenItems) where T : View
            {
                StackLayout MenuScreenRow = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.StartAndExpand,
					Spacing = 50,
                    Children = { }
                };
                foreach (T MenuScreenItem in MainScreenItems)
                    MenuScreenRow.Children.Add(MenuScreenItem);
                return MenuScreenRow;
            }
            
            public static Label FormattedLabel(params Span[] Spans)
            {
                var Text = new FormattedString();
                Text.Spans.AddRange(Spans);
                return new Label
                {
                    FormattedText = Text,
                    VerticalTextAlignment = TextAlignment.Start,
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.Fill
                };
            }
            public static Label FormattedLabel(Color BackColor, NamedSize Size, params Span[] Spans)
            {
                var Text = new FormattedString();
                Text.Spans.AddRange(Spans);
                return new Label
                {
                    FormattedText = Text,
                    BackgroundColor = BackColor,
                    VerticalTextAlignment = TextAlignment.Start,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = Device.GetNamedSize(Size, targetElementType: typeof(Label)),
                    HorizontalOptions = LayoutOptions.Fill
                };
            }
            public static Label FormattedLabel(FormattedString Text, Color BackColor = default(Color), NamedSize Size = NamedSize.Default)
            {
                return new Label
                {
                    FormattedText = Text,
                    BackgroundColor = BackColor,
                    VerticalTextAlignment = TextAlignment.Start,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = Device.GetNamedSize(Size, targetElementType: typeof(Label)),
                    HorizontalOptions = LayoutOptions.Fill
                };
            }
            public static Label BoldLabel(Text Text, Color TextColor = default(Color), 
                Color BackColor = default(Color), NamedSize Size = NamedSize.Default)
            {
                if (TextColor == default(Color))
                    TextColor = Color.Black;
                if (TextColor == default(Color))
                    TextColor = Color.Default;
                return new Label
                {
                    Text = Text,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = TextColor,
                    BackgroundColor = BackColor,
                    VerticalTextAlignment = TextAlignment.Start,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = Device.GetNamedSize(Size, targetElementType: typeof(Label)),
                    HorizontalOptions = LayoutOptions.Fill
                };
            }
            public static Label BoldLabel2(Text Text, Color TextColor = default(Color), 
                Color BackColor = default(Color), NamedSize Size = NamedSize.Default)
            {
                if (TextColor == default(Color))
                    TextColor = Color.Black;
                if (TextColor == default(Color))
                    TextColor = Color.Default;
                return new Label
                {
                    FormattedText = Format(Bold(Text)),
                    TextColor = TextColor,
                    BackgroundColor = BackColor,
                    VerticalTextAlignment = TextAlignment.Start,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = Device.GetNamedSize(Size, typeof(Label)),
                    HorizontalOptions = LayoutOptions.Fill
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
            public static Label VersionDisplay
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
            public static Button Back(Page Page, Color BackColor = default(Color), Color TextColor = default(Color))
            {
                if (BackColor == default(Color))
                    BackColor = Color.Silver;
                if (TextColor == default(Color))
                    TextColor = Color.Black;
                Button Return = Button("Back", OnClick: () => Page.SendBackButtonPressed());
                Return.HorizontalOptions = LayoutOptions.End;
                Return.VerticalOptions = LayoutOptions.Fill;
                return Return;
            }/*
            [Obsolete("Not needed. Deprecated in 0.10.0a173")]
            public static Button UpdateAlpha(Page Page, Color BackColor = default(Color), Color TextColor = default(Color))
            {
                if (BackColor == default(Color))
                    BackColor = Color.Silver;
                if (TextColor == default(Color))
                    TextColor = Color.Black;
                Button Return = Button("Check for Alpha", delegate
                {
#if __ANDROID__
                    Android.App.ProgressDialog progress = new Android.App.ProgressDialog(Forms.Context);
                    progress.Indeterminate = true;
                    progress.SetProgressStyle(Android.App.ProgressDialogStyle.Horizontal);
                    progress.SetMessage("Please wait... Loading updater....");
                    progress.SetCancelable(true);
                    progress.Show();
                    progress.SetMessage(new Updater((Updater.UpdateProgress Progress) =>
                    {
                        progress.SetMessage(Progress.ToName());
                        progress.Progress = Progress.Percentage() * 100;
                    }).CheckUpdate().ToString());
                    Do(System.Threading.Tasks.Task.Delay(2000));
                    progress.Dismiss();
#elif WINDOWS_UWP
                    var w = new ProgressDialog(
                        new ProgressDialogConfig { Title = "Please wait... Loading updater...." });
                    w.Show();
                    w.Title = new Updater((Updater.UpdateProgress Progress) =>
                    {
                        w.Title = Progress.ToName();
                        w.PercentComplete = Progress.Percentage();
                    }).CheckUpdate().ToString();
                    Do(System.Threading.Tasks.Task.Delay(2000));
                    w.Hide();
#else
                    Alert(Page, "Only supported on Android and Windows 10. " +
                        "For other versions, please check the github repository manually.");
#endif
                }, Color.Silver);
                Return.HorizontalOptions = LayoutOptions.Start;
                Return.VerticalOptions = LayoutOptions.Fill;
                return Return;
            }*/
            public static StackLayout ChangelogView(Page Page, Color BackColor = default(Color))
            {
                ScrollView Changelog = Create.Changelog;
                if (BackColor == default(Color))
                    BackColor = Color.White;
                return new StackLayout
                {
                    Children = { Changelog, Row(false, /*UpdateAlpha(Page),*/ Back(Page)) },
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

            public static StackLayout Row(bool VerticalExpand, params View[] Items)
            {
                StackLayout MenuScreenRow = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = VerticalExpand ? LayoutOptions.StartAndExpand : LayoutOptions.Center,
                    Children = { }
                };
                foreach (View MenuScreenItem in Items)
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
                foreach (View MenuScreenItem in Items)
                    MenuScreenRow.Children.Add(MenuScreenItem);
                return MenuScreenRow;
            }

            public static StackLayout Row<T>(bool VerticalExpand, IEnumerable<T> Items) where T : View
            {
                StackLayout MenuScreenRow = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = VerticalExpand ? LayoutOptions.StartAndExpand : LayoutOptions.Center,
                    Children = { }
                };
                foreach (T MenuScreenItem in Items)
                    MenuScreenRow.Children.Add(MenuScreenItem);
                return MenuScreenRow;
            }
            public static StackLayout Column<T>(IEnumerable<T> Items) where T : View
            {
                StackLayout MenuScreenRow = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Children = { }
                };
                foreach (T MenuScreenItem in Items)
                    MenuScreenRow.Children.Add(MenuScreenItem);
                return MenuScreenRow;
            }

            public static StackLayout Row<T>(bool VerticalExpand, params T[] Items) where T : View
            {
                StackLayout MenuScreenRow = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = VerticalExpand ? LayoutOptions.StartAndExpand : LayoutOptions.Center,
                    Children = { }
                };
                foreach (T MenuScreenItem in Items)
                    MenuScreenRow.Children.Add(MenuScreenItem);
                return MenuScreenRow;
            }
            public static StackLayout Column<T>(params T[] Items) where T : View
            {
                StackLayout MenuScreenRow = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Children = { }
                };
                foreach (T MenuScreenItem in Items)
                    MenuScreenRow.Children.Add(MenuScreenItem);
                return MenuScreenRow;
            }
            public static ScrollView ButtonStack(params Button[] Buttons)
            {
                StackLayout Return = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                for (int i = 0; i < Buttons.Length - 1; i++)
                    Return.Children.Add(new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Children = { Buttons[i], Buttons[++i] }
                    });
                return new ScrollView
                {
                    Orientation = ScrollOrientation.Vertical,
                    Content = Return,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
            }
            public static ColumnDefinition Column(GridUnitType Unit, double Width) =>
                new ColumnDefinition { Width = new GridLength(Width, Unit) };
            public static RowDefinition Row(GridUnitType Unit, double Height) =>
                new RowDefinition { Height = new GridLength(Height, Unit) };
            public static ColumnDefinitionCollection Columns(GridUnitType Unit, params double[] Widths)
            {
                ColumnDefinitionCollection Return = new ColumnDefinitionCollection();
                foreach (int Width in Widths)
                    Return.Add(new ColumnDefinition { Width = new GridLength(Width, Unit) });
                return Return;
            }
            public static RowDefinitionCollection Rows(GridUnitType Unit, params double[] Heights)
            {
                RowDefinitionCollection Return = new RowDefinitionCollection();
                foreach (int Height in Heights)
                    Return.Add(new RowDefinition { Height = new GridLength(Height, Unit) });
                return Return;
            }
            public static Entry Entry(Text Text, Text Placeholder, Func<string> ReadOnly = null, Keyboard Keyboard = null,
                Color TextColor = default(Color), Color PlaceholderColor = default(Color), Color BackColor = default(Color))
            {
                Entry Return = new Entry
                {
                    Text = Text,
                    Placeholder = Placeholder,
                    Keyboard = Keyboard ?? Keyboard.Default,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    TextColor = TextColor == default(Color) ? Color.Black : TextColor,
                    PlaceholderColor = PlaceholderColor == default(Color) ? Color.Silver : PlaceholderColor,
                    BackgroundColor = BackColor == default(Color) ? Color.Default : BackColor
                };
                if(ReadOnly!= null) Return.TextChanged += TextChanged(ReadOnly);
                return Return;
            }
            public static Version Version(int Major, int Minor, int Build = 0, VersionStage Stage = 0, short Revision = 0) =>
                new Version(Major, Minor, Build, (int)Stage * (1 << 16) + Revision);
            public static Slider Slider(EventHandler<ValueChangedEventArgs> ValueChanged, 
               int Minimum = 0, int Maximum = 100, int Position = 100, Color BackColor = default(Color))
            {
                var Return = new Slider { Minimum = Minimum, Maximum = Maximum, Value = Position,
                    BackgroundColor = BackColor, HorizontalOptions = LayoutOptions.FillAndExpand };
                Return.ValueChanged += ValueChanged;
                return Return;
            }
            public static Button[] RadioButtons(Color Base, Color Selected,
                Func<int, ButtonOnClick> Init, int DefaultIndex = 0, bool AllowDeselect = false, params string[] Names)
            {
                var Modificators = new Button[Names.Length];
                for (int Index = 0; Index < Names.Length; Index++)
                {
                    ref var B = ref Modificators[Index];
                    B = Button(Names[Index], Init(Index), Index == DefaultIndex ? Selected : Base);
                    B.HorizontalOptions = LayoutOptions.FillAndExpand;
                    B.Clicked += (sender, e) =>
                    {
                        var Sender = sender as Button;
                        if (Sender.BackgroundColor == Base)
                        {
                            foreach (var Modify in Modificators)
                                Modify.BackgroundColor = Base;
                            Sender.BackgroundColor = Selected;
                        }
                        else if (AllowDeselect) Sender.BackgroundColor = Base;
                    };
                }
                return Modificators;
            }
            public static ScrollView RadioButtonsView(Color Base, Color Selected,
                Func<int, ButtonOnClick> Init, int DefaultIndex = 0, bool AllowDeselect = false, params string[] Names) =>
                Scroll(StackOrientation.Horizontal, RadioButtons(Base, Selected, Init, DefaultIndex, AllowDeselect, Names));

            public static void AppendScrollStack<T>(ScrollView Base, params T[] Items) where T : View =>
                (Base.Content as StackLayout).Children.AddRange(Items);
            public static void AppendScrollStack<T>(ScrollView Base, IEnumerable<T> Items) where T : View =>
                (Base.Content as StackLayout).Children.AddRange(Items);
            public static void FillGrid(Grid Base, View Item) => 
                Base.Children.Add(Item, 0, Base.RowDefinitions.Count, 0, Base.ColumnDefinitions.Count);
            public static ScrollView Scroll<T>(StackOrientation Orientation, params T[] Views) where T : View
            {
                ScrollView Modificator = new ScrollView
                {
                    Orientation = (ScrollOrientation)Orientation,
                    Content = new StackLayout
                    {
                        Orientation = Orientation
                    }
                };
                if (Orientation == StackOrientation.Horizontal)
                    (Modificator.Content as StackLayout).HorizontalOptions = 
                        Modificator.HorizontalOptions = LayoutOptions.FillAndExpand;
                else
                    (Modificator.Content as StackLayout).VerticalOptions = 
                        Modificator.VerticalOptions = LayoutOptions.FillAndExpand;
                AppendScrollStack(Modificator, Views);
                return Modificator;
            }
            public static ScrollView Scroll<T>(StackOrientation Orientation, IEnumerable<T> IEViews) where T : View
            {
                ScrollView Modificator = new ScrollView
                {
                    Orientation = (ScrollOrientation)Orientation,
                    Content = new StackLayout
                    {
                        Orientation = Orientation,
                    }
                };
                if (Orientation == StackOrientation.Horizontal)
                    (Modificator.Content as StackLayout).HorizontalOptions =
                        Modificator.HorizontalOptions = LayoutOptions.FillAndExpand;
                else
                    (Modificator.Content as StackLayout).VerticalOptions =
                        Modificator.VerticalOptions = LayoutOptions.FillAndExpand;
                AppendScrollStack(Modificator, IEViews);
                return Modificator;
            }
        }
    }
}