
//System.Runtime.Serialization.FormatterServices.GetUninitializedObject(((object)typeof(object)).GetType())
//System.Runtime.Serialization.FormatterServices.GetUninitializedObject(Type.GetType("System.RuntimeType"))
/*System.Runtime.Serialization.FormatterServices.GetUninitializedObject((Type)
    System.Runtime.Serialization.FormatterServices.GetUninitializedObject(Type.GetType("System.RuntimeType")))*/
//Hosting process exited with exit code -1073741819.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static InnoTecheLearning.Utils;
using static InnoTecheLearning.Utils.Create;
using Xamarin.Forms;
#if __IOS__
using Xamarin.Forms.Platform.iOS;
#elif __ANDROID__
using Xamarin.Forms.Platform.Android;
#elif WINDOWS_UWP
using Xamarin.Forms.Platform.UWP;
#endif

namespace InnoTecheLearning
{
    
    static class PageIdExtensions
    {
        public static Main.PageId GetId(this Page P) => 
            P.StyleId == AssemblyTitle ? Main.PageId.Main :
                (Main.PageId)Enum.Parse(typeof(Main.PageId),
                    new StringBuilder(P.StyleId)
                    .Replace(' ', '_')
                    .Replace("(", SubLeftBracket)
                    .Replace(")", SubRightBracket)
                    .Remove(0, "eLearn ".Length)
                    .ToString()
                );
        public static string GetTitle(this Main.PageId i) =>
            i == Main.PageId.Main ? AssemblyTitle : 
                new StringBuilder(i.ToString())
                .Replace('_', ' ')
                .Replace(SubLeftBracket, "(")
                .Replace(SubRightBracket, ")")
                .Insert(0, "eLearn ")
                .ToString();
    }
    public class Main : NavigationPage
    {
        public enum PageId : sbyte
        {
            Uninitialised = sbyte.MinValue,
            Console = -3,
            Crashes = -2,
            Changelog = -1,
            Main,
            Lingual,
            Logic_毲Keypad䫎,
            Logic_毲Freeform䫎,
            Logic_毲Factor䫎,
            Logic_毲Symbolics䫎,
            Health,
            Tunes,
            Excel,
            Facial
        }
        bool AnimateRows = true;
        public ValueTask<Unit> Push(View v, PageId Id, string Title = null) =>
            Log(
                Unit.Await(
                    PushAsync(
                        Log(
                            new ContentPage { Content = v, BackgroundColor = Color.White }.With(
                                (ref ContentPage x) =>
                                {
                                    if (Enum.IsDefined(typeof(PageId), Id) && Id != PageId.Uninitialised) x.StyleId = Id.GetTitle();
                                    x.Title = Title ?? x.StyleId ?? string.Empty;
                                }),
                            $"Pushing a page, Page title: {Title}..."
                        )
                    )
                ),
            $"Page pushed. Page title: {Title}");
        public ValueTask<Page> Pop() => new ValueTask<Page>(PopAsync());
        //StreamPlayer _Player;
        public Main()
        {
            Log("Main.ctor()");
            //throw new Java.Lang.Throwable("Can a java throwable be logged?");
            //Wire this up on start in every project: Exceptions.RegisterHandlers();
            //if (Instance != null) throw new InvalidOperationException("Can only have one instance of the main screen.");
            async void AsyncInit()
            {
                Favourites = await Storage.SerializedReadOrCreateOrDefault(Storage.VocabFile, new ObservableCollection<OnlineDict.Entry>());
                Favourites.CollectionChanged += async (object sender, NotifyCollectionChangedEventArgs e) =>
                    await Storage.SerializedWrite(Storage.VocabFile, Favourites);
                await Log("AsyncInit completed.");
            }
            AsyncInit();
            // Accomodate iPhone status bar.
            Padding = new Thickness(0, OnPlatform(20, 0, 0), 0, 0);
            //BarBackgroundColor = Color.Silver;
            //BarTextColor = Color.Black;
            BackgroundColor = Color.White;
            Log("Before pushing main...");
            Push(MainView, PageId.Main);
            Popped += (sender, e) => { MusicSound?.Dispose(); MusicSound = null; };
            AnimateRows = false;
            //_Player = Create(new StreamPlayerOptions(Utils.Resources.GetStream("Sounds.CNY.wav"), Loop: true));
            //_Player.Play();
            Log("Main page initialized.");
        }
        //~Main() { _Player.Dispose(); }
        protected override bool OnBackButtonPressed()
        {
            if (Navigation.NavigationStack.Count > 1)
            {
                Pop();
                return true;
            }
            else
                return base.OnBackButtonPressed();
        }

        public StackLayout MainView
        {
            get
            {
                return Log(new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Children = {
                        Log(Society, "Generating Society Label in MainView"),

                        Log(MainScreenRow(true, AnimateRows,
                            MainScreenItem(
                                ImageSource(ImageFile.Translate),
                                () => Push(Translator, PageId.Lingual),
                                BoldLabel("LINGUAL")
                            ),

                             MainScreenItem(
                                 ImageSource(ImageFile.Calculator),
                                 async () => Switch(
                                     await DisplayActionSheet(
                                     "Choose Logic mode", "Cancel", null,
                                     "Keypad", "Freeform", "Factor", "Symbolics"),

                                     null,
                                     ("Keypad", () => Push(Calculator, PageId.Logic_毲Keypad䫎)),
                                     ("Freeform", () => Push(Calculator_Free, PageId.Logic_毲Freeform䫎)),
                                     ("Factor", () => Push(Factorizer, PageId.Logic_毲Factor䫎)),
                                     ("Symbolics", () => PushAsync(new Pages.Logic_Symbolics()))
                                 ),
                                 BoldLabel("LOGIC")
                            )
                        ), "Generated first row"),

                        Log(MainScreenRow(true, AnimateRows,
                            MainScreenItem(
                                ImageSource(ImageFile.Sports),
                                () => Push(Sports, PageId.Health),
                                BoldLabel("HEALTH")
                            ),

                            MainScreenItem(
                                ImageSource(ImageFile.MusicTuner),
                                () => PushAsync(new Pages.Tunes()),
                                BoldLabel("TUNES"))
                            )
                        , "Generated second row"),

                        MainScreenRow(true, AnimateRows,
                            MainScreenItem(
                                ImageSource(ImageFile.MathSolver),
                                () => Push(MathSolver, PageId.Excel),
                                BoldLabel("EXCEL")
                            ),
                            MainScreenItem(
                                ImageSource(ImageFile.Facial),
                                () => Push(Facial, PageId.Facial),
                                BoldLabel("FACIAL")
                            )
                        ),

                        Button("Changelog", () => Push(
                            Column(Changelog, Row(false,
                                Button("Enter console", () => Push(ConsoleView, PageId.Console)),
                                Button("View crash logs", () => Push(CrashLog, PageId.Crashes))
                           )),
                        PageId.Changelog)),
                        VersionDisplay
                    }
                }, "Generated MainView");
            }
        }
        StreamPlayer MusicSound { get; set; }
        public StackLayout MusicTuner
        {
            get
            {
                void MusicTunerSwitch(Button[] Viola, int i, Button[] OtherRow)
                {
                    Viola[i].Clicked += delegate
                    {
                        for (int j = 0; j < 4; j++)
                            Viola[j].BackgroundColor = Color.Silver;
                        for (int j = 0; j < 4; j++)
                            OtherRow[j].BackgroundColor = Color.Silver;
                        Viola[i].BackgroundColor = Color.FromHex("#FF7F50"); //Coral (orange)
                    };
                }/*
                Button MusicTunerPlay(Text Text, int Frequency, Slider Volum)
                {
                    return Button(Text, () =>
                    { //_Player.Pause();
                        MusicSound?.Dispose(); MusicSound = ToneGenerator.PlayTone(Frequency, 2, (float)Volum.Value);
                    }).With((ref Button x) => x.HorizontalOptions = x.VerticalOptions =  LayoutOptions.FillAndExpand);
                }*/
                Button MusicTunerPlay(Text Text, Sounds Sound, Slider Volum)
                {
                    return Button(Text, () =>
                    { //_Player.Pause();
                        MusicSound?.Dispose(); MusicSound = StreamPlayer.Play(Sound, true, (float)Volum.Value);
                    }).With((ref Button x) => x.HorizontalOptions = x.VerticalOptions = LayoutOptions.FillAndExpand);
                }
                Label Volume = (Text)"100";
                Slider Vol =
                    Slider((object sender, ValueChangedEventArgs e) =>
                    {
                        Volume.Text = ((int)e.NewValue).ToString().PadLeft(4);
                        if (MusicSound == null || MusicSound.Disposed) return;
                        MusicSound.Volume = (float)e.NewValue / 100;
                    }, BackColor: Color.Gray);
                Button[] Violin = { MusicTunerPlay("G", Sounds.Violin_G, Vol), // 196
                        MusicTunerPlay("D", Sounds.Violin_D, Vol), // 294
                        MusicTunerPlay("A", Sounds.Violin_A, Vol), // 440
                        MusicTunerPlay("E", Sounds.Violin_E, Vol)}; // 659
                Button[] Cello = { MusicTunerPlay("'C", Sounds.Cello_C, Vol),// 65
                        MusicTunerPlay("'G", Sounds.Cello_G, Vol),// 98
                        MusicTunerPlay("D", Sounds.Cello_D, Vol),// 147
                        MusicTunerPlay("A", Sounds.Cello_A, Vol)};// 220

                for (int i = 0; i < 4; i++)
                    MusicTunerSwitch(Violin, i, Cello);
                for (int i = 0; i < 4; i++)
                    MusicTunerSwitch(Cello, i, Violin);
                return new StackLayout
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Vertical,
                    Children = {
                        //Title("eLearn Tunes"),

                        MainScreenRow(false, false, Image(ImageFile.Violin, async delegate {await Alert(this, "🎻♫♬♩♪♬♩♪♬"); })
                            .With((ref Image x) =>
                            {
                                x.HorizontalOptions = x.VerticalOptions = LayoutOptions.FillAndExpand;
                                x.Aspect = Aspect.AspectFit;
                            })
                        , ((Label)(Text)"Violin and Viola")
                            .With((ref Label x) => {
                                x.VerticalOptions = LayoutOptions.Center;
                                x.HorizontalOptions = LayoutOptions.FillAndExpand;
                                x.FontAttributes = FontAttributes.Bold;
                            }))
                        .With((ref StackLayout x) => x.HorizontalOptions = x.VerticalOptions = LayoutOptions.FillAndExpand),

                        Row(true, Violin)
                            .With((ref StackLayout x) => x.HorizontalOptions = x.VerticalOptions = LayoutOptions.FillAndExpand),

                        MainScreenRow(false, false, Image(ImageFile.Cello, async delegate {await Alert(this, "🎻♫♬♩♪♬♩♪♬"); })
                            .With((ref Image x) =>
                            {
                                x.HorizontalOptions = x.VerticalOptions = LayoutOptions.FillAndExpand;
                                x.Aspect = Aspect.AspectFit;
                            })
                        , ((Label)(Text)"Cello and Double Bass")
                            .With((ref Label x) => {
                                x.VerticalOptions = LayoutOptions.Center;
                                x.HorizontalOptions = LayoutOptions.FillAndExpand;
                                x.FontAttributes = FontAttributes.Bold;
                            }))
                        .With((ref StackLayout x) => x.HorizontalOptions = x.VerticalOptions = LayoutOptions.FillAndExpand),

                        Row(true, Cello)
                            .With((ref StackLayout x) => x.HorizontalOptions = x.VerticalOptions = LayoutOptions.FillAndExpand),

                        Button("Stop", () => {
                        for (int j = 0; j < 4; j++)
                            { Violin[j].BackgroundColor = Color.Silver; Cello[j].BackgroundColor = Color.Silver; }
                        MusicSound?.Dispose(); /*_Player.Play();*/ })
                            .With((ref Button x) => x.HorizontalOptions = x.VerticalOptions = LayoutOptions.FillAndExpand),

                        Row(false, Volume, Vol),
                        //Back(this)
                    }
                };
            }
        }
        string Calculator_Value_ = "";
        string Calculator_Value { get => Calculator_Value_; set { Calculator_Display_Dirty = true; Calculator_Value_ = value; } }
        int Calculator_Cursor_ = 0;
        int Calculator_Cursor
        {
            get => Calculator_Cursor_.LowerBound(0).UpperBound(Calculator_Expression.Count);
            set
            {
                Calculator_Cursor_ = value;
                Calculator_History[Calculator_HistoryIndex] =
                    (Calculator_History[Calculator_HistoryIndex].In, Calculator_History[Calculator_HistoryIndex].Out, value);
                Calculator_Cursor_Update?.Invoke();
            }
        }
        Action Calculator_Cursor_Update;
        List<Expressions> Calculator_Expression;
        AngleMode Calculator_AngleUnit = 0;
        string Calculator_Display;
        bool Calculator_Display_Dirty = true;
        Modifier Calculator_Modifier_;
        Modifier Calculator_Modifier
        { get => Calculator_Modifier_; set { Calculator_Display_Dirty = true; Calculator_Modifier_ = value; } }
        readonly List<(List<Expressions> In, string Out, int Cursor)> Calculator_History =
            new List<(List<Expressions> In, string Out, int Cursor)> { (new List<Expressions>(), "", 0) };
        int Calculator_HistoryIndex_ = 0;
        int Calculator_HistoryIndex
        {
            get => Calculator_HistoryIndex_.LowerBound(0).UpperBound(Calculator_History.Count);
            set
            {
                Calculator_HistoryIndex_ = value;
                Calculator_HistoryIndex_Update?.Invoke();
            }
        }
        Action Calculator_HistoryIndex_Update;
        #region Append
        void Calculator_StartModify()
        {
            if (Calculator_HistoryIndex != Calculator_History.Count - 1)
            {
                Calculator_History[Calculator_History.Count - 1] =
                    (new List<Expressions>(Calculator_Expression), Calculator_Value, Calculator_Cursor);
                Calculator_HistoryIndex = Calculator_History.Count - 1;
            }
        }
        EventHandler<ExpressionEventArgs> Append_MethodGen(Expressions Expression) =>
            (object sender, ExpressionEventArgs e) =>
                {
                    Calculator_StartModify();
                    Calculator_Expression.Insert(Calculator_Cursor.UpperBound(Calculator_Expression.Count), e.Expression);
                    Calculator_Cursor++;
                };
        public void Append(Grid.IGridList<View> List, Expressions Expression,
            Color BackColor = default, Color TextColor = default) =>
            List.Add(Button(Expression, Append_MethodGen(Expression), BackColor, TextColor));
        public void Append(Grid.IGridList<View> List, Expressions Expression,
            int Left, int Top, Color BackColor = default, Color TextColor = default) =>
            List.Add(Button(Expression, Append_MethodGen(Expression), BackColor, TextColor), Left, Top);
        public void Append(Grid.IGridList<View> List, Expressions Expression,
            int Left, int Right, int Top, int Bottom, Color BackColor = default, Color TextColor = default) =>
            List.Add(Button(Expression, Append_MethodGen(Expression), BackColor, TextColor), Left, Right, Top, Bottom);
        public void Append(Grid.IGridList<View> List, Expressions Expression, Text Name,
            Color BackColor = default, Color TextColor = default) =>
            List.Add(Button(Expression, Append_MethodGen(Expression), Name, BackColor, TextColor));
        public void Append(Grid.IGridList<View> List, Expressions Expression, Text Name,
            int Left, int Top, Color BackColor = default, Color TextColor = default) =>
            List.Add(Button(Expression, Append_MethodGen(Expression), Name, BackColor, TextColor), Left, Top);
        public void Append(Grid.IGridList<View> List, Expressions Expression, Text Name,
            int Left, int Right, int Top, int Bottom, Color BackColor = default, Color TextColor = default) =>
            List.Add(Button(Expression, Append_MethodGen(Expression), Name, BackColor, TextColor), Left, Right, Top, Bottom);
        #endregion
        public StackLayout Calculator
        {
            get
            {
                var MoveLeft = Button("◀", () => Calculator_Cursor--)
                    .With((ref Button x) => { if (Device.Idiom != TargetIdiom.Desktop) x.HorizontalOptions = LayoutOptions.FillAndExpand; });
                var MoveRight = Button("▶", () => Calculator_Cursor++)
                    .With((ref Button x) => { if (Device.Idiom != TargetIdiom.Desktop) x.HorizontalOptions = LayoutOptions.FillAndExpand; });
                var MoveUp = Button("▲", () => Calculator_HistoryIndex--)
                    .With((ref Button x) => { if (Device.Idiom != TargetIdiom.Desktop) x.HorizontalOptions = LayoutOptions.FillAndExpand; });
                var MoveDown = Button("▼", () => Calculator_HistoryIndex++)
                    .With((ref Button x) => { if (Device.Idiom != TargetIdiom.Desktop) x.HorizontalOptions = LayoutOptions.FillAndExpand; });
                Entry In = new Entry
                {
                    TextColor = Color.Black,
                    Placeholder = "Expression",
                    PlaceholderColor = Color.Gray,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromRgb(0xD0, 0xD0, 0xD0)
                };
                Entry Out = new Entry
                {
                    TextColor = Color.Black,
                    Placeholder = "Result",
                    PlaceholderColor = Color.Gray,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                void In_TextChanged()
                {
                    try
                    {
                        var Replacement = Calculator_Expression.AsString()
                            .Insert(Calculator_Expression.ToStringLocation(Calculator_Cursor), Cursor);
                        if (In.Text != Replacement)
                        {
                            In.Text = Replacement;
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        if (In.Text != Cursor)
                        {
                            In.Text = Cursor;
                        }
                    }
                }
                void Out_TextChanged()
                {
                    if (Calculator_Display_Dirty)
                    {
                        Calculator_Display =
                              double.TryParse(Calculator_Value, out var d) ?
                              Try(() => Display(d, Calculator_Modifier), (Exception ex) => d.ToString()) :
                              Calculator_Value;
                        Calculator_Display_Dirty = false;
                    }
                    if (Out.Text != Calculator_Display) Out.Text = Calculator_Display;
                }
                In.TextChanged += (sender, e) => In_TextChanged();
                Out.TextChanged += (sender, e) => Out_TextChanged();
                (Calculator_HistoryIndex_Update =
                    () =>
                    {
                        MoveUp.IsEnabled = Calculator_HistoryIndex != 0;
                        MoveDown.IsEnabled = Calculator_HistoryIndex != Calculator_History.Count - 1;
                        (Calculator_Expression, Calculator_Value, Calculator_Cursor) =
                            Calculator_History[Calculator_HistoryIndex];
                        In_TextChanged();
                        Out_TextChanged();
                    }
                )();
                (Calculator_Cursor_Update = In_TextChanged)();

                Grid Vars, Const, Trig, Func, Bin, Norm = new Grid
                {
                    ColumnDefinitions = Columns(GridUnitType.Star, 1, 1, 1, 1, 1),
                    RowDefinitions = Rows(GridUnitType.Star, 1, 1, 1, 1, 1),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                Append(Norm.Children, Expressions.Space, "␣", 0, 0);
                Append(Norm.Children, Expressions.Modulus, 1, 0);
                Append(Norm.Children, Expressions.Ans, 2, 0);
                Norm.Children.Add(Button("⌫", () =>
                {
                    Calculator_StartModify();
                    if (Calculator_Expression.Count > 0)
                        Calculator_Expression.RemoveAt((--Calculator_Cursor).LowerBound(0));
                    Calculator_Cursor_Update?.Invoke();
                }, Color.FromHex("#E91E63")), 3, 0);
                Norm.Children.Add(Button("⎚", () =>
                {
                    Calculator_StartModify();
                    Calculator_Expression.Clear();
                    Calculator_Cursor_Update?.Invoke();
                }, Color.FromHex("#E91E63")), 4, 0); //Pink
                Append(Norm.Children, Expressions.D7, 0, 1, Color.FromHex("#607D8B"));
                Append(Norm.Children, Expressions.D8, 1, 1, Color.FromHex("#607D8B"));
                Append(Norm.Children, Expressions.D9, 2, 1, Color.FromHex("#607D8B"));
                Append(Norm.Children, Expressions.LParenthese, 3, 1);
                Append(Norm.Children, Expressions.RParenthese, 4, 1);
                Append(Norm.Children, Expressions.D4, 0, 2, Color.FromHex("#607D8B"));
                Append(Norm.Children, Expressions.D5, 1, 2, Color.FromHex("#607D8B"));
                Append(Norm.Children, Expressions.D6, 2, 2, Color.FromHex("#607D8B"));
                Append(Norm.Children, Expressions.Multiplication, 3, 2);
                Append(Norm.Children, Expressions.Division, 4, 2);
                Append(Norm.Children, Expressions.D1, 0, 3, Color.FromHex("#607D8B"));
                Append(Norm.Children, Expressions.D2, 1, 3, Color.FromHex("#607D8B"));
                Append(Norm.Children, Expressions.D3, 2, 3, Color.FromHex("#607D8B"));
                Append(Norm.Children, Expressions.Addition, 3, 3);
                Append(Norm.Children, Expressions.Subtraction, 4, 3);
                Append(Norm.Children, Expressions.D0, 0, 4, Color.FromHex("#607D8B")); //Blue Grey
                Append(Norm.Children, Expressions.DPoint, 1, 4);
                Append(Norm.Children, Expressions.e, 2, 4);
                Norm.Children.Add(Button("=", () =>
                {
                    var ToCalculate = Calculator_Expression.AsString();
                    Calculator_History[Calculator_History.Count - 1] =
                    (
                        Calculator_Expression,
                        string.IsNullOrWhiteSpace(ToCalculate) ? string.Empty :
                            JSEvaluate(ToCalculate, this, Calculator_AngleUnit),
                        Calculator_Cursor
                    );
                    Calculator_HistoryIndex = Calculator_History.Count - 1;
                    Calculator_History.Add((new List<Expressions>(), "", 0));
                }, Color.FromHex("#FFC107")), 3, 5, 4, 5); //Amber

                Bin = new Grid
                {
                    ColumnDefinitions = Columns(GridUnitType.Star, 1, 1, 1, 1),
                    RowDefinitions = Rows(GridUnitType.Star, 1, 1, 1, 1, 1),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                Append(Bin.Children, Expressions.Less, 0, 0);
                Append(Bin.Children, Expressions.Great, 1, 0);
                Append(Bin.Children, Expressions.BXor, 2, 0);
                Append(Bin.Children, Expressions.UnsignRShift, 3, 0);
                Append(Bin.Children, Expressions.LessEqual, 0, 1);
                Append(Bin.Children, Expressions.GreatEqual, 1, 1);
                Append(Bin.Children, Expressions.BLShift, 2, 1);
                Append(Bin.Children, Expressions.BRShift, 3, 1);
                Append(Bin.Children, Expressions.Equal, 0, 2);
                Append(Bin.Children, Expressions.NEqual, 1, 2);
                Append(Bin.Children, Expressions.Increment, 2, 2);
                Append(Bin.Children, Expressions.Decrement, 3, 2);
                Append(Bin.Children, Expressions.Identity, 0, 3);
                Append(Bin.Children, Expressions.NIdentity, 1, 3);
                Append(Bin.Children, Expressions.LAnd, 2, 3);
                Append(Bin.Children, Expressions.BAnd, 3, 3);
                Append(Bin.Children, Expressions.LNot, 0, 4);
                Append(Bin.Children, Expressions.BNot, 1, 4);
                Append(Bin.Children, Expressions.LOr, 2, 4);
                Append(Bin.Children, Expressions.BOr, 3, 4);

                Func = new Grid
                {
                    ColumnDefinitions = Columns(GridUnitType.Star, 1, 1, 1, 1, 1),
                    RowDefinitions = Rows(GridUnitType.Star, 1, 1, 1, 1, 1),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                Append(Func.Children, Expressions.Abs, "Abs", 0, 0);
                Append(Func.Children, Expressions.Clz32, "Clz32", 1, 0);
                Append(Func.Children, Expressions.Sqrt, "Sqrt", 2, 0);
                Append(Func.Children, Expressions.Cbrt, "Cbrt", 3, 0);
                Append(Func.Children, Expressions.nPr, "nPr", 4, 0);
                Append(Func.Children, Expressions.Round, "Round", 0, 1);
                Append(Func.Children, Expressions.Ceil, "Ceil", 1, 1);
                Append(Func.Children, Expressions.Floor, "Floor", 2, 1);
                Append(Func.Children, Expressions.Trunc, "Trunc", 3, 1);
                Append(Func.Children, Expressions.nCr, "nCr", 4, 1);
                Append(Func.Children, Expressions.Exp, "Exp", 0, 2);
                Append(Func.Children, Expressions.Comma, 1, 2);
                Append(Func.Children, Expressions.Imul, "Imul", 2, 2);
                Append(Func.Children, Expressions.Random, "Random", 3, 2);
                Append(Func.Children, Expressions.GCD, "GCD", 4, 2);
                Append(Func.Children, Expressions.Lb, "Lb", 0, 3);
                Append(Func.Children, Expressions.Ln, "Ln", 1, 3);
                Append(Func.Children, Expressions.Log, "Log", 2, 3);
                Append(Func.Children, Expressions.Pow, "Pow", 3, 3);
                Append(Func.Children, Expressions.HCF, "HCF", 4, 3);
                Append(Func.Children, Expressions.Max, "Max", 0, 4);
                Append(Func.Children, Expressions.Min, "Min", 1, 4);
                Append(Func.Children, Expressions.Factorial, "Factor", 2, 4);
                Append(Func.Children, Expressions.Sign, "Sign", 3, 4);
                Append(Func.Children, Expressions.LCM, "LCM", 4, 4);

                Trig = new Grid
                {
                    ColumnDefinitions = Columns(GridUnitType.Star, 1, 1, 1, 1),
                    RowDefinitions = Rows(GridUnitType.Star, 1, 1, 1, 1, 1),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                Append(Trig.Children, Expressions.Deg, "Deg", 0, 0);
                Append(Trig.Children, Expressions.Sin, "Sin", 1, 0);
                Append(Trig.Children, Expressions.Asin, "Asin", 2, 0);
                Append(Trig.Children, Expressions.Sinh, "Sinh", 3, 0);
                Append(Trig.Children, Expressions.Asinh, "Asinh", 4, 0);
                Append(Trig.Children, Expressions.Rad, "Rad", 0, 1);
                Append(Trig.Children, Expressions.Cos, "Cos", 1, 1);
                Append(Trig.Children, Expressions.Acos, "Acos", 2, 1);
                Append(Trig.Children, Expressions.Cosh, "Cosh", 3, 1);
                Append(Trig.Children, Expressions.Acosh, "Acosh", 4, 1);
                Append(Trig.Children, Expressions.Grad, "Grad", 0, 2);
                Append(Trig.Children, Expressions.Tan, "Tan", 1, 2);
                Append(Trig.Children, Expressions.Atan, "Atan", 2, 2);
                Append(Trig.Children, Expressions.Tanh, "Tanh", 3, 2);
                Append(Trig.Children, Expressions.Atanh, "Atanh", 4, 2);
                Append(Trig.Children, Expressions.Turn, "Turn", 0, 3);
                Append(Trig.Children, Expressions.Cot, "Cot", 1, 3);
                Append(Trig.Children, Expressions.Acot, "Acot", 2, 3);
                Append(Trig.Children, Expressions.Coth, "Coth", 3, 3);
                Append(Trig.Children, Expressions.Acoth, "Acoth", 4, 3);
                Append(Trig.Children, Expressions.Atan2, "Atan2", 0, 4);
                Append(Trig.Children, Expressions.Sec, "Sec", 1, 4);
                Append(Trig.Children, Expressions.Asec, "Asec", 2, 4);
                Append(Trig.Children, Expressions.Sech, "Sech", 3, 4);
                Append(Trig.Children, Expressions.Asech, "Asech", 4, 4);
                Append(Trig.Children, Expressions.Comma, 0, 5);
                Append(Trig.Children, Expressions.Csc, "Csc", 1, 5);
                Append(Trig.Children, Expressions.Acsc, "Acsc", 2, 5);
                Append(Trig.Children, Expressions.Csch, "Csch", 3, 5);
                Append(Trig.Children, Expressions.Acsch, "Acsch", 4, 5);

                Const = new Grid
                {
                    ColumnDefinitions = Columns(GridUnitType.Star, 1, 1, 1, 1),
                    RowDefinitions = Rows(GridUnitType.Star, 1, 1, 1),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                Append(Const.Children, Expressions.π, 0, 0);
                Append(Const.Children, Expressions.e, 1, 0);
#if WINDOWS_UWP
                Append(Const.Children, Expressions.Root2, "√̅2", 2, 0);
                Append(Const.Children, Expressions.Root0_5, "√̅0̅.̅5", 3, 0);
#else
                Append(Const.Children, Expressions.Root2, "√2̅", 2, 0);
                Append(Const.Children, Expressions.Root0_5, "√0̅.̅5̅", 3, 0);
#endif
                Append(Const.Children, Expressions.Ln2, "Logₑ2", 0, 1);
                Append(Const.Children, Expressions.Ln10, "Logₑ10", 1, 1);
                Append(Const.Children, Expressions.Log2e, "Log₂e", 2, 1);
                Append(Const.Children, Expressions.Log10e, "Log₁₀e", 3, 1);
                Append(Const.Children, Expressions.Infinity, "∞", 0, 2);
                Append(Const.Children, Expressions.NInfinity, "-∞", 1, 2);
                Append(Const.Children, Expressions.NaN, 2, 2);
                Append(Const.Children, Expressions.Undefined, "UNDEF", 3, 2);

                Vars = new Grid
                {
                    ColumnDefinitions = Columns(GridUnitType.Star, Duplicate(1.0, 6)),
                    RowDefinitions = Rows(GridUnitType.Star, Duplicate(1.0, 7)),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                {
                    int Left, Top;
                    Left = Top = 0;
                    for (var Item = Expressions.Assign; Item <= Expressions.Z; Item++)
                    { Append(Vars.Children, Item, Left, Top); if (++Left >= 6) { Left = 0; Top++; } }
                    Append(Vars.Children, Expressions.Increment, ++Left, Top);
                    Append(Vars.Children, Expressions.Decrement, ++Left, Top);
                }


                StackLayout Return =
                    Device.Idiom == TargetIdiom.Desktop ?
                    new StackLayout
                    {
                        Children =
                        {
                            Row(false, MoveLeft, MoveRight, In, MoveUp, MoveDown),
                            new StackLayout(), Norm, new StackLayout(), Out
                        }
                    } :
                    new StackLayout
                    {
                        Children =
                        {
                            In,
                            Row(false, MoveLeft, MoveRight, MoveUp, MoveDown)
                                .With((ref StackLayout x) => x.HorizontalOptions = LayoutOptions.FillAndExpand),
                            new StackLayout(), Norm, new StackLayout(), Out
                        }
                    };

                Grid[] Menus = new Grid[] { Norm, Bin, Func, Trig, Const, Vars };
                Button Mode = new Button { Text = Calculator_AngleUnit.ToString(), BackgroundColor = Color.FromHex("#02A8F3") };
                //Light Blue
                Mode.Clicked += delegate
                {
                    Calculator_AngleUnit++; if (Calculator_AngleUnit > AngleMode.Turn) Calculator_AngleUnit = AngleMode.Degree;
                    Mode.Text = Calculator_AngleUnit.ToString();
                };
                var Select = RadioButtons(Color.FromHex("#8AC249"), Color.FromHex("#4CAF50"),
                    i => delegate
                    {
                        if (Return.Children[new OnIdiom<int> { Desktop = 2, Phone = 3, Tablet = 3 }] != Menus[i])
                            Return.Children[new OnIdiom<int> { Desktop = 2, Phone = 3, Tablet = 3 }] = Menus[i];
                    }, 0, false,
                    nameof(Norm), nameof(Bin), nameof(Func), nameof(Trig), nameof(Const), nameof(Vars));
                Return.Children[new OnIdiom<int> { Desktop = 1, Phone = 2, Tablet = 2 }] = Row(false, Select[0],
                        Scroll(StackOrientation.Horizontal, Select.Skip(1).Concat(new[] { Mode }))//, Back(this)
                    );
                var Modifiers = RadioButtons(Color.FromHex("#8AC249"), Color.FromHex("#4CAF50"),
                    i => delegate
                    {
                        var Result = Calculator_History[Calculator_HistoryIndex];
                        if (string.IsNullOrWhiteSpace(Calculator_Value)) Calculator_Value = string.Empty;
                        else if (Calculator_Modifier != (Modifier)i)
                        {
                            Calculator_Modifier = (Modifier)i;
                            Calculator_Value = Result.Out.ToString();
                            Out.Text = "";
                        }
                    }, 0, false,
                    "Norm", "%", "a b / c", "d / c", "° ′ ″", OnPlatform("e√f̅", "e√f̅", "e√̅f"),
                    OnPlatform("g / h √f̅", "g / h √f̅", "g / h √̅f"));
                Return.Children[new OnIdiom<int> { Desktop = 3, Phone = 4, Tablet = 4 }] =
                    Row(false, Modifiers[0], Scroll(StackOrientation.Horizontal, Modifiers.Skip(1)));
                return Return;
            } //http://www.goxuni.com/671054-how-to-create-a-custom-color-picker-for-xamarin-forms/
            /*
             
            
                        Button("Const", delegate {Try(delegate { if(Return.Children[2] != Const) Return.Children[2]
                                = Const; },(Exception e)=> { }); }, Color.FromHex("#8AC249")

             { "Default", Color.Default },

{ "Black", Color.FromHex("#212121") },

{ "Blue Grey", Color.FromHex("#607D8B") },
{ "Cyan", Color.FromHex("#00BCD4") },

{ "Dark Purple", Color.FromHex("#673AB7") },
{ "Grey", Color.FromHex("#9E9E9E") },

{ "Light Blue", Color.FromHex("#02A8F3") },
{ "Lime", Color.FromHex("#CDDC39") },

{ "Pink", Color.FromHex("#E91E63") },

{ "Red", Color.FromHex("#D32F2F") },

{ "White", Color.FromHex("#FFFFFF") },

{ "Amber", Color.FromHex("#FFC107") },

{ "Blue", Color.FromHex("#2196F3") },

{ "Brown", Color.FromHex("#795548") },

{ "Dark Orange", Color.FromHex("#FF5722") },
{ "Green", Color.FromHex("#4CAF50") },

{ "Indigo", Color.FromHex("#3F51B5") },

{ "Light Green", Color.FromHex("#8AC249") },
{ "Orange", Color.FromHex("#FF9800") },

{ "Purple", Color.FromHex("#94499D") },

{ "Teal", Color.FromHex("#009587") },

{ "Yellow", Color.FromHex("#FFEB3B") },

*/
        }
        string Calculator_Free_Value = "";
        public StackLayout Calculator_Free
        {
            get
            {
                void Calculator_Free_TextChanged(object sender, TextChangedEventArgs e)
                {
                    if (((Entry)sender).Text != Calculator_Free_Value) { ((Entry)sender).Text = Calculator_Free_Value; }
                }
                Editor Editor = new Editor
                {
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Text = "1 + 1"
                    //BackgroundColor = Color.FromRgb(0xD0, 0xD0, 0xD0) //Light Grey
                };
                Entry Entry = new Entry
                {
                    TextColor = Color.Black,
                    Placeholder = "Result",
                    PlaceholderColor = Color.Gray,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.End
                };
                Entry.TextChanged += Calculator_Free_TextChanged;
                return new StackLayout
                {
                    Children =
                    {
                        //Title("eLearn Logic"),
                        new ScrollView
                        {
                            Content = Editor,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            VerticalOptions = LayoutOptions.FillAndExpand
                        },

                        Row(false, Button("Evaluate", () =>
                        {
                            Calculator_Free_Value =
                                string.IsNullOrWhiteSpace(Editor.Text) ? string.Empty : JSEvaluate(Editor.Text, this);
                            Calculator_Free_TextChanged(Entry, new TextChangedEventArgs(Entry.Text, Calculator_Free_Value));
                        }).With((ref Button x) => x.HorizontalOptions = LayoutOptions.FillAndExpand)
                            //,Back(this)
                        ),
                        Entry
                    }
                };
            }
        }

        string Factorizer_Root1 = "";
        string Factorizer_Root2 = "";
        string Factorizer_Result = "";
        public StackLayout Factorizer
        {
            get
            {
                const string X = "X";
                const string Y = "Y";
                Button S1 = null;
                S1 = Button("+", () => { S1.Text = S1.Text == "+" ? "-" : "+"; });
                Entry C1 = Entry("", "Coefficient", Keyboard: Keyboard.Numeric);
                Button S2 = null;
                S2 = Button("+", () => { S2.Text = S2.Text == "+" ? "-" : "+"; });
                Entry C2 = Entry("", "Coefficient", Keyboard: Keyboard.Numeric);
                Button S3 = null;
                S3 = Button("+", () => { S3.Text = S3.Text == "+" ? "-" : "+"; });
                Entry C3 = Entry("", "Coefficient", Keyboard: Keyboard.Numeric);
                Entry R1 = Entry(Factorizer_Root1, "First Root", delegate { return Factorizer_Root1; });
                Entry R2 = Entry(Factorizer_Root1, "Second Root", delegate { return Factorizer_Root2; });
                Entry F = Entry(Factorizer_Root1, "Factorized Result", delegate { return Factorizer_Result; });
                return new StackLayout
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children = {
                        //Title("eLearn Logic"),
                        Row(false, S1, C1, (Text)(X + "²")),
                        Row(false, S2, C2, (Text)(X + Y)),
                        Row(false, S3, C3, (Text)(Y + "²")),
                        Button("Factorize", () =>
                        {
                            Factorizer_Result =
                                Factorize(
                                    TryParseDouble(C1.Text, 1d) * (S1.Text == "+" ? 1 : -1),
                                    TryParseDouble(C2.Text, 1d) * (S2.Text == "+" ? 1 : -1),
                                    TryParseDouble(C3.Text, 1d) * (S3.Text == "+" ? 1 : -1),
                                    out System.Numerics.Complex X1, out System.Numerics.Complex X2, X, Y
                                );
                            Factorizer_Root1 = X1.ToABi();
                            Factorizer_Root2 = X2.ToABi();
                            R1.Text = Factorizer_Root1;
                            R2.Text = Factorizer_Root2;
                            F.Text = Factorizer_Result;
                        }), R1, R2, F,
                        //Back(this)
                    }
                };
            }
        }
        Label Sports_Steps = BoldLabel("0", Color.White, Color.FromRgb(0x40, 0x40, 0x40), NamedSize.Large);
        Label Sports_Time = BoldLabel("00:00:00", Color.White, Color.FromRgb(0x40, 0x40, 0x40), NamedSize.Large);
        Label Sports_Distance = BoldLabel("0 m", Color.White, Color.FromRgb(0x40, 0x40, 0x40), NamedSize.Large);
        Label Sports_Now = BoldLabel(DateTime.Now.ToString("HH:mm:ss"), Color.White,
            Color.FromRgb(0x40, 0x40, 0x40), NamedSize.Large);
        IStepCounter Pedometer = new StepCounter();
        public StackLayout Sports
        {
            get
            {
                Label Label(string Text) => ((Label)(Text)Text).With((ref Label x) =>
                    { x.FontAttributes = FontAttributes.Bold; x.HorizontalOptions = LayoutOptions.Center; });
                Device.StartTimer(TimeSpan.FromSeconds(1),
                    () =>
                    {
                        Sports_Steps.Text = Pedometer.Steps.ToString();
                        Sports_Distance.Text = Pedometer.Distance.ToString() + " m";
                        Sports_Time.Text = Pedometer.TimePassed.ToString(@"hh\:mm\:ss");
                        Sports_Now.Text = DateTime.Now.ToString("HH:mm:ss");
                        return Navigation.NavigationStack.Last().GetId() == PageId.Health;
                    });
                return new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children = {
                        //Title("eLearn Health"),
                        Button("Start Running", async () => {
                                try { await Pedometer.Start(); }
                                catch(UnauthorizedAccessException)
                                { await Alert(this, "Access to the pedometer is denied.", "Sports"); }
                            }, Color.Blue, Color.White),
                        Button("Stop Running", () => { Pedometer.Stop(); }, Color.Red, Color.White),
                        Label("Steps"),
                        Sports_Steps,
                        Label("Elapsed Time"),
                        Sports_Time,
                        Label("Estimated Distance"),
                        Sports_Distance,
                        Label("Time Now"),
                        Sports_Now,
                        Button("Reset", () => { Pedometer.Reset(); }, Color.Yellow),
                        //Back(this)
                    }
                };
            }
        }
        public StackLayout MathSolver
        {
            get
            {
                var Draw = new TouchImage
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.Black.MultiplyAlpha(1 / 255),
                    CurrentLineColor = Color.Black
                };
                Draw.SetBinding(TouchImage.CurrentLineColorProperty, "CurrentLineColor");
                var Display = new Label
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    TextColor = Color.Black,
                    LineBreakMode = LineBreakMode.NoWrap
                };
                var Dragon = new Image
                {
                    Source = ImageSource(ImageFile.Dragon),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Aspect = Aspect.AspectFit
                };
                var Hearts = Duplicate(() => Image(ImageFile.Heart, () => { }), 5);
                Label Instruction = new Label
                {
                    TextColor = Color.Black,
                    Text = "The dragon is setting fire on everything!",
                    HorizontalTextAlignment = TextAlignment.Center
                };
                Label Question = new Label
                {
                    TextColor = Color.Black,
                    Text = "We must use the power of Mathematics to kill it!",
                    HorizontalTextAlignment = TextAlignment.Center
                };
                var Questions = new[]{new
                {
                    Instruction = "Solve the following.",
                    Question = "(8+54/6-5*3)/2",
                    Answers = new[] { "1" },
                    ExtraChars = "234567890",
                    Rows = 9,
                    Columns = 6
                }, new
                {
                    Instruction = "Find y.",
                    Question = "9(8y)/4=16*5-26",
                    Answers = new[] { "3" },
                    ExtraChars = "124567890",
                    Rows = 9,
                    Columns = 6
                }, new
                {
                    Instruction = "Solve the following. (Use / to indicate fractions.)",
                    Question = "2(1/3+5/6)",
                    Answers = new[] { "7/3" },
                    ExtraChars = "\\124",
                    Rows = 9,
                    Columns = 6
                }, new
                {
                    Instruction = "Factorize the following.",
                    Question = "-ps-2qr-pr-2qs",
                    Answers = new[] { "-(p+2q)(s+r)", "-(2q+p)(s+r)", "-(p+2q)(r+s)", "-(2q+p)(r+s)",
                    "-(s+r)(p+2q)", "-(s+r)(2q+p)", "-(r+s)(p+2q)", "-(r+s)(2q+p)"},
                    ExtraChars = "",
                    Rows = 9,
                    Columns = 4
                }, new
                {
                    Instruction = "Calculate the following. (Don't use a calculator!)",
                    Question = " 1+2+3+...+9998+9999+10000",
                    Answers = new[] { "50005000" },
                    ExtraChars = "",
                    Rows = 9,
                    Columns = 6
                } };
                var Stack = new MathSolverStack<(int X, int Y)>();
                var Chars = new Label[Questions.First().Rows, Questions.First().Columns];
                var CharGrid = new Grid
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    RowDefinitions = Rows(GridUnitType.Star, Duplicate(1.0, Questions.First().Rows)),
                    ColumnDefinitions = Columns(GridUnitType.Star, Duplicate(1.0, Questions.First().Columns)),
                    BackgroundColor = Color.Transparent,
                    IsVisible = false
                };
                int Level = -1;
                string[] Answers = new string[0];
                Random Randomizer = new Random();
                Action Advance = () =>
                {
                    CharGrid.Children.Clear();
                    if (Level > 0) Hearts[Level - 1].IsVisible = false;
                    if (Level >= Questions.Length)
                    {
                        Dragon.Source = ImageSource(ImageFile.Dragon_Dead);
                        Instruction.Text = "You killed the dragon!";
                        Question.Text = "You're a hero!";
                        CharGrid.IsVisible = false;
                        return;
                    }
                    for (int i = 0; i < Questions[Level].Rows; i++)
                        for (int j = 0; j < Questions[Level].Columns; j++)
                        {
                            Chars[i, j] = new Label
                            {
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                VerticalOptions = LayoutOptions.FillAndExpand,
                                HorizontalTextAlignment = TextAlignment.Center,
                                VerticalTextAlignment = TextAlignment.Center,
                                BackgroundColor = Color.Transparent,
                                TextColor = Color.Black,
                                Text = ""
                            };
                            CharGrid.Children.Add(Chars[i, j], i, j);
                        }
                    FillGrid(CharGrid, Draw);
                    CharGrid.IsVisible = true;
                    CharGrid.ForceLayout();
                    Question.Text = Questions[Level].Question;
                    Instruction.Text = Questions[Level].Instruction;
                    Answers = Questions[Level].Answers;
                    var Answer = Answers[Randomizer.Next(Answers.Length)];
                    Distribute: var Location = (X: Randomizer.Next(Questions[Level].Rows - 1),
                    Y: Randomizer.Next(Questions[Level].Columns - 1));
                    for (int i = 0; i < Questions[Level].Rows; i++)
                        for (int j = 0; j < Questions[Level].Columns; j++)
                            Chars[i, j].Text = "";
                    foreach (var Q in Questions[Level].Answers.Random())
                    {
                        Chars[Location.X, Location.Y].Text = Q.ToString();
                        int SegfaultCounter = 0;
                        RandomMove: if (++SegfaultCounter >= 12) goto Distribute;
                        switch (Randomizer.Next(1, 4))
                        {
                            case 1: //N
                                if (Location.Y - 1 > 0 && Chars[Location.X, Location.Y - 1].Text == "") Location.Y--;
                                else goto RandomMove;
                                break;
                            case 2: //W
                                if (Location.X - 1 > 0 && Chars[Location.X - 1, Location.Y].Text == "") Location.X--;
                                else goto RandomMove;
                                break;
                            case 3: //E
                                if (Location.X < Questions[Level].Rows - 1 &&
                                Chars[Location.X + 1, Location.Y].Text == "") Location.X++;
                                else goto RandomMove;
                                break;
                            case 4: //S
                                if (Location.Y < Questions[Level].Columns - 1 &&
                                Chars[Location.X, Location.Y + 1].Text == "") Location.Y++;
                                else goto RandomMove;
                                break;
                            default: //Impossible
                                goto RandomMove;
                        }
                        SegfaultCounter = 0;
                    }
                    for (int i = 0; i < Questions[Level].Rows; i++)
                        for (int j = 0; j < Questions[Level].Columns; j++)
                            if (Chars[i, j].Text == "") Chars[i, j].Text =
                                 string.Concat(Questions[Level].ExtraChars, Answer).Random().ToString();
                };
                var Continue = Button("Continue",
                    (ref Button sender, EventArgs e) => { sender.IsVisible = false; ++Level; Advance(); });
                Draw.PointerEvent += async (sender, e) =>
                {
                    switch (e.Type)
                    {
                        case TouchImage.PointerEventArgs.PointerEventType.Down:
                        case TouchImage.PointerEventArgs.PointerEventType.Move:
                            if (e.PointerDown)
                            {
                                Stack.Push((
                                  (int)(Math.Floor(e.Current.X *
                                  CharGrid.RowDefinitions.Count / CharGrid.Width * RawXMultiplier
                                  )).LowerBound(0).UpperBound(Questions[Level].Rows - 1),
                                  (int)(Math.Floor(e.Current.Y *
                                  CharGrid.ColumnDefinitions.Count / CharGrid.Height * RawYMultiplier
                                  )).LowerBound(0).UpperBound(Questions[Level].Columns - 1)));
                                //Display.Text = ((CharGrid.Width) / e.Current.X).ToString();
                                //Display.Text = ((CharGrid.Height) / e.Current.Y).ToString();
                            }
                            break;
                        case TouchImage.PointerEventArgs.PointerEventType.Up:
                        case TouchImage.PointerEventArgs.PointerEventType.Cancel:
                            if (Answers.Contains(Display.Text))
                            {
                                try { CharGrid.Children.Add(new Label()); } catch (InvalidOperationException) { return; }
                                await Alert(this, "Correct! The dragon got hurt!", "Yay!", "I'll go on and continue");
                                ++Level; Advance(); }
                            else await Alert(this, "You got the answer wrong...\nPlease retry.", "Ooops!", "I'll retry");
                            Stack.Clear();
                            Draw.Clear();
                            break;
                        default:
                            break;
                    }
                    var Sb = new StringBuilder();
                    foreach (var Item in Stack) Sb.Insert(0, Chars[Item.X, Item.Y].Text);
                    Display.Text = Sb.ToString();
                };
                return new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        Instruction,
                        Question,
                        Display, CharGrid, Dragon,
                        Row(false, Row(false, Hearts), Continue
                            )
                    }
                };
            }
        }
        ObservableCollection<OnlineDict.Entry> Favourites = new ObservableCollection<OnlineDict.Entry>();
        public Grid Translator
        {
            get
            {
                Button FavouritesButton(OnlineDict.Entry R)
                {
                    var B = Button(Favourites.Contains(R) ? "★-" : "★+",
                        (ref Button sender, EventArgs e) =>
                        {
                            if (Favourites.Contains(R))
                            {
                                Favourites.Remove(R);
                                sender.Text = "★+";
                            }
                            else
                            {
                                Favourites.Add(R);
                                sender.Text = "★-";
                            }
                        }, TextColor: Color.Yellow);
                    Favourites.CollectionChanged +=
                        (object sender, NotifyCollectionChangedEventArgs e) =>
                        {
                            if (e.OldItems?.Contains(R) == true) Device.BeginInvokeOnMainThread(() => B.Text = "★+");
                            if (e.NewItems?.Contains(R) == true) Device.BeginInvokeOnMainThread(() => B.Text = "★-");
                        };
                    return B;
                }
                var Mode = Button(OnlineDict.ToEnglishMode ? "文→A" : "A→文", (ref Button sender, EventArgs e) => 
                    sender.Text = (OnlineDict.ToEnglishMode = !OnlineDict.ToEnglishMode) ? "文→A" : "A→文");
                var Input = Entry("", "Enter words...");
                void ViewUpdate(StackLayout Layout, IEnumerable<OnlineDict.Entry> Results, params Span[] NoResults)
                {
                    Layout.Children.Clear();
                    foreach (var Result in Results)
                    {
                        Layout.Children.Add(
                            Row(false, FavouritesButton(Result),
                                FormattedLabel(
                                    new Span
                                    {
                                        Text = Result.Headword.PadRight(33),
                                        ForegroundColor = Color.Black,
                                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                                        FontFamily = FontDictionary
                                        },
                                    new Span
                                    {
                                        Text = Result.PoS.PadRight(13),
                                        ForegroundColor = Color.Gray,
                                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                                        FontFamily = FontDictionary
                                    }, new Span
                                    {
                                        Text = Result.Translation + " \n",
                                        ForegroundColor = Color.Black,
                                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                                        FontFamily = FontDictionary
                                    }
                                )
                            )
                        );
                    }
                    if (Layout.Children.Count == 0) Layout.Children.Add(FormattedLabel(NoResults));
                };
                var Formatted = new StackLayout
                {
                    Scale = 1,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Children = {
                        FormattedLabel(
                            new Span
                            {
                                Text = "Enter something and press translate,\nand the results will appear here.",
                                ForegroundColor = Color.Gray,
                                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                                FontFamily = FontDictionary
                            }
                        )
                    }
                };
                var Translate = Button("→", async () =>
                {
                    if (!await InternetAvaliable)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                            await Alert(this, "Cannot connect to the servers. Please check your connection and try again."));
                        return;
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Formatted.Children.Clear();
                        Formatted.Children.Add(FormattedLabel(
                            new Span { Text = "Loading...", FontFamily = FontDictionary, ForegroundColor = Color.Black,
                                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)) }
                        ));
                    });
                    var Results = (await OnlineDict.Convert(Input.Text)).Entries;
                    Device.BeginInvokeOnMainThread(() =>
                        ViewUpdate(Formatted, Results, new Span
                        {
                            Text = "Not found!",
                            ForegroundColor = Color.Red,
                            FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                            FontFamily = FontDictionary
                        })
                    );
                });
                var Grid = new Grid
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    RowDefinitions = { new RowDefinition(), Row(GridUnitType.Auto, 1),
                        new RowDefinition(), Row(GridUnitType.Auto, 1) },
                    RowSpacing = 0
                };
                Grid.Children.Add(new StackLayout
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children = {
                        Row(false, Mode, Input, Translate),
                        new ScrollView { Orientation = ScrollOrientation.Both, Content = Formatted }
                    }
                }, 0, 0);
                Grid.Children.Add(new GridSplitter(Color.FromRgb(223, 223, 223)){ VerticalOptions = LayoutOptions.Center }, 0, 1);
                var FavouritesView = new StackLayout { Scale = 1 };
                void FavouritesUpdate() => ViewUpdate(FavouritesView, Favourites, new Span
                {
                    Text = "There's nothing here...\n",
                    ForegroundColor = Color.Gray,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    FontFamily = FontDictionary
                }, new Span
                {
                    Text = "Press the button on the left of one of the\ntranslated results to add it into the Favourites!",
                    ForegroundColor = Color.Gray,
                    FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                    FontFamily = FontDictionary
                });
                Favourites.CollectionChanged += (_1, _2) => FavouritesUpdate();
                FavouritesUpdate();
                Grid.Children.Add(new ScrollView
                {
                    BackgroundColor = Color.White,
                    Orientation = ScrollOrientation.Both,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Content = FavouritesView
                }, 0, 2);
                return Grid;
            }
        }
        
        string CrashLogCurrent = null;
        public StackLayout CrashLog
        {
            get
            {
                var Crash = Button("Crash me!", OnClick:() => throw new NotSupportedException("Oops, crashing is not supported!"));
                if (Storage.Empty(Storage.CrashDir))
                    return new StackLayout {
                        Children = {
                            ((Label)(Text)"Fortunately, the app has not crashed yet...")
                                .With((ref Label x) => {
                                    x.HorizontalTextAlignment = TextAlignment.Center;
                                    x.HorizontalOptions = x.VerticalOptions = LayoutOptions.FillAndExpand;
                                }),
                            Crash
                        }
                    };

                CrashLogCurrent = CrashLogCurrent ?? Storage.Last(Storage.CrashDir);

                Label File = (Text)CrashLogCurrent;
                File.HorizontalOptions = LayoutOptions.FillAndExpand;

                Label Disp = (Text)"";
                Disp.HorizontalOptions = Disp.VerticalOptions = LayoutOptions.FillAndExpand;
                new Action(async () => Disp.Text = await Storage.Read(Storage.Combine(Storage.CrashDir, CrashLogCurrent)))();

                Button Prev = null;
                Button Next = null;
                Prev = Button("▲", () => {
                    File.Text = CrashLogCurrent = Storage.Before(Storage.CrashDir, CrashLogCurrent);
                    new Action(async () => Disp.Text = await Storage.Read(Storage.Combine(Storage.CrashDir, CrashLogCurrent)))();
                    if (!Storage.HasBefore(Storage.CrashDir, CrashLogCurrent)) Prev.IsEnabled = false;
                    Next.IsEnabled = true;
                });
                Next = Button("▼", () => {
                    File.Text = CrashLogCurrent = Storage.After(Storage.CrashDir, CrashLogCurrent);
                    new Action(async () => Disp.Text = await Storage.Read(Storage.Combine(Storage.CrashDir, CrashLogCurrent)))();
                    if (!Storage.HasAfter(Storage.CrashDir, CrashLogCurrent)) Next.IsEnabled = false;
                    Prev.IsEnabled = true;
                });
                if (!Storage.HasBefore(Storage.CrashDir, CrashLogCurrent)) Prev.IsEnabled = false;
                if (!Storage.HasAfter(Storage.CrashDir, CrashLogCurrent)) Next.IsEnabled = false;

                var Copy = Button("Copy log", () => ClipboardText = Disp.Text)
                    .With((ref Button x) => x.HorizontalOptions = LayoutOptions.FillAndExpand);

                return new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        Row(false, Prev, File, Next), Scroll(ScrollOrientation.Vertical, Disp),
                        Row(false, Copy,
                            Button("Delete", () =>
                            {
                                string NextDisp = Storage.Before(Storage.CrashDir, CrashLogCurrent) ??
                                                  Storage.After(Storage.CrashDir, CrashLogCurrent);
                                Storage.Delete(Storage.Combine(Storage.CrashDir, CrashLogCurrent)).Ignore();
                                if (NextDisp == null) Pop();
                                else
                                {
                                    File.Text = CrashLogCurrent = NextDisp;
                                    new Action(async () => Disp.Text =
                                        await Storage.Read(Storage.Combine(Storage.CrashDir, CrashLogCurrent)))();
                                    Prev.IsEnabled = Storage.HasBefore(Storage.CrashDir, CrashLogCurrent);
                                    Next.IsEnabled = Storage.HasAfter(Storage.CrashDir, CrashLogCurrent);
                                }
                            }), Crash)
                    }
                };
            }
        }
        
        public AbsoluteLayout Facial
        {
            get
            {
                var Return = new AbsoluteLayout();
                Return.HorizontalOptions = Return.VerticalOptions = LayoutOptions.FillAndExpand;
                var cam = new Camera();
                cam.ProcessingPreview += (sender, e) => 
                {
                    for (int i = 0; i < Return.Children.Count; i++)
                        if (Return.Children[i] is BoxView) Return.Children.RemoveAt(i);
                    for (int i = 0; i < e.DetectedFaces.Length; i++)
                        Return.AddPosition(Log(new BoxView
                        {
                            Color = Color.Black
                        }, $"Face Detected: ({e.DetectedFaces[i].Left},{e.DetectedFaces[i].Top})"),
#if __ANDROID__
                        Rectangle.FromLTRB(
                        e.DetectedFaces[i].Left / 2000.0 + 0.5,
                        e.DetectedFaces[i].Top / 2000.0 + 0.5,
                        e.DetectedFaces[i].Right / 2000.0 + 0.5,
                        e.DetectedFaces[i].Bottom / 2000.0 + 0.5), AbsoluteLayoutFlags.All);
#else
                        e.DetectedFaces[i].Left, e.DetectedFaces[i].Top, AbsoluteLayoutFlags.None);
#endif
                    Return.ForceLayout();
                };
                Return.Children.Add(
                    cam.ToView().With((ref View x) =>
                        x.HorizontalOptions = x.VerticalOptions = LayoutOptions.FillAndExpand),
                        new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All
                );
                return Return;
            }
        }
    }
}
