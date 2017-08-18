using System;
using System.Collections.Generic;
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

            public static StackLayout MainScreenRow(bool Dropdown, bool Animate, params View[] MainScreenItems) =>
                MainScreenRow<View>(Dropdown, Animate, MainScreenItems);

            public static StackLayout MainScreenRow<T>(bool Dropdown, bool Animate, params T[] MainScreenItems) where T : View
            {
                Log("Generating MainScreenRow...");
                StackLayout MenuScreenRow = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
					//Spacing = 50,
                    Children = { }
                };
                foreach (T MenuScreenItem in MainScreenItems)
                    MenuScreenRow.Children.Add(MenuScreenItem);
                Log("Added all items into MainScreenRow");
                if(Dropdown)
                    Device.StartTimer(Device.Idiom != TargetIdiom.Desktop && Animate ? Seconds(1) : Milliseconds(1),
                    () => { MenuScreenRow.Spacing = MainScreenItems[0].Width;
                    MenuScreenRow.TranslateTo(0, MainScreenItems[0].Height / 2, 1000, Easing.BounceOut); return false; });
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
                    return Return;
                }
            }
            public static StackLayout ConsoleView
            {
                get
                {
                    var Out = new Label { BindingContext = Console, Text = Console.OutText, TextColor = Color.Black };
                    Out.SetBinding(Label.TextProperty, new Binding(nameof(Console.OutText), BindingMode.OneWay));
                    Out.HorizontalOptions = Out.VerticalOptions = LayoutOptions.FillAndExpand;
                    var In = Entry(string.Empty, "Enter command (\"help\" for help)");
                    In.HorizontalOptions = LayoutOptions.FillAndExpand;
                    return new StackLayout
                    {
                        Children =
                        {
                            Scroll(ScrollOrientation.Vertical, Out),
                            Row(false, In, Button("→", () => Unit.Invoke(() => Console.Execute(In.Text))))
                        }
                    };
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
            public static StackLayout ChangelogView(Main Instance, Color BackColor = default(Color))
            {
                ScrollView Changelog = Create.Changelog;
                if (BackColor == default(Color))
                    BackColor = Color.White;
                return new StackLayout
                {
                    Children =
                    {
                        Changelog,
                        Row(false, 
                        Button("View crash logs", () => Instance.Push(Instance.CrashLog, Main.PageId.Crashes))
                            .With((ref Button x) => x.HorizontalOptions = LayoutOptions.FillAndExpand)
                        )
                        /*, Row(false, UpdateAlpha(Page), Back(Page))*/
                    },
                    BackgroundColor = BackColor,
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill
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
                        FormattedText = Format((Text)"Developed by Hadrian Tang Wai To of the\n", Bold("Innovative Technology Society of CSWCSS"))
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
            public static ScrollView Scroll(StackLayout Stack) =>
                new ScrollView { Orientation = (ScrollOrientation)Stack.Orientation, Content = Stack }
                .With((ref ScrollView x) =>
                {
                    switch (Stack.Orientation)
                    {
                        case StackOrientation.Vertical:
                            x.VerticalOptions = LayoutOptions.FillAndExpand;
                            break;
                        case StackOrientation.Horizontal:
                            x.HorizontalOptions = LayoutOptions.FillAndExpand;
                            break;
                    }
                });
            public static ScrollView Scroll(ScrollOrientation Orientation, View View) =>
                new ScrollView { Orientation = Orientation, Content = View }
                .With((ref ScrollView x) =>
                {
                    switch (Orientation)
                    {
                        case ScrollOrientation.Vertical:
                            x.VerticalOptions = LayoutOptions.FillAndExpand;
                            break;
                        case ScrollOrientation.Horizontal:
                            x.HorizontalOptions = LayoutOptions.FillAndExpand;
                            break;
                        case ScrollOrientation.Both:
                            x.HorizontalOptions = x.VerticalOptions = LayoutOptions.FillAndExpand;
                            break;
                    }
                });
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