/*System.Runtime.Serialization.FormatterServices.GetUninitializedObject((Type)
    System.Runtime.Serialization.FormatterServices.GetUninitializedObject(Type.GetType("System.RuntimeType")))*/
//Hosting process exited with exit code -1073741819.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static InnoTecheLearning.Utils;
using static InnoTecheLearning.Utils.Create;
using static InnoTecheLearning.Utils.OnlineDict.DictionaryResponse;
using static InnoTecheLearning.Utils.StreamPlayer;
using Xamarin.Forms;

namespace InnoTecheLearning
{

#if CHRISTMAS //|| __ANDROID__ 
    using Android.Media;
    public class Media
    {
        private MediaPlayer player;
        public void Start(string Path)
        {
            var player = new MediaPlayer();
            player.Prepared += (s, e) =>
            {
                player.Start();
            };
            player.SetDataSource(Path);
            player.Looping = true;
            player.Prepare();
        }
        public void Pause() => player.Pause();
        public void Stop() => player.Stop();
    }
#endif
    public class Main : ContentPage
    {
        public enum Pages : sbyte
        {
            CloudTest = -2,
            Changelog = -1,
            Main,
            Forum,
            Translate,
            VocabBook,
            Calculator,
            Calculator_Free,
            Factorizer,
            Sports,
            MusicTuner,
            MathSolver
        }
        public new View Content
        {
            get { return base.Content; }
            set
            {
#if false
                var Layout = new RelativeLayout();
                Layout.Children.Add(new Image
                {
                    Aspect = Aspect.Fill,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Source = Image("CNY.jpg")
                },
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    Constraint.RelativeToParent((parent) => { return parent.Height; }));
                Layout.Children.Add(value,
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    Constraint.RelativeToParent((parent) => { return parent.Height; }));
                base.Content = Layout;
#else
                base.Content = value;
#endif
            }
        }
        Pages _Showing;
        Pages Showing
        {
            get { return _Showing; }
            set
            {
                switch (value)
                {
                    case Pages.CloudTest:
                        Region = "CloudTest";
                        Content = CloudTest;
                        break;
                    case Pages.Changelog:
                        Region = "Changelog";
                        Content = ChangelogView(this);
                        break;
                    case Pages.Main:
                        Region = "Main";
                        Content = MainView;
                        break;
                    case Pages.Forum:
                        Region = "Forum";
                        break;
                    case Pages.Translate:
                        Region = "Translate";
                        Content = Translator;
                        break;
                    case Pages.VocabBook:
                        Region = "VocabBook";
                        break;
                    case Pages.Calculator:
                        Region = "Calculator";
                        Content = Calculator;
                        break;
                    case Pages.Calculator_Free:
                        Region = "Calculator_Free";
                        Content = Calculator_Free;
                        break;
                    case Pages.Factorizer:
                        Region = "Factorizer";
                        Content = Factorizer;
                        break;
                    case Pages.Sports:
                        Region = "Sports";
                        Content = Sports;
                        break;
                    case Pages.MusicTuner:
                        Region = "MusicTuner";
                        Content = MusicTuner;
                        break;
                    case Pages.MathSolver:
                        Region = "MathSolver";
                        Content = MathSolver;
                        break;
                    default:
                        Region = "App";
                        break;
                }
                _Showing = value;
            }
        }
        //StreamPlayer _Player;
        public Main()
        {
            // Accomodate iPhone status bar.
            Padding = new Thickness(0, OnPlatform(20, 0, 0), 0, 0);
            BackgroundColor = Color.White;
            //Alert(this, "Main constructor"); 
            Favourites = (ObservableCollection<Result>)Storage.SerializedRead(Storage.VocabFile, Favourites.GetType());
            Showing = Pages.Main;
            //_Player = Create(new StreamPlayerOptions(Utils.Resources.GetStream("Sounds.CNY.wav"), Loop: true));
            //_Player.Play();
            Log("Main page initialized.");
        }
        //~Main() { _Player.Dispose(); }
        protected override bool OnBackButtonPressed()
        {
            if (Showing != Pages.Main)
            {
                Showing = Pages.Main;
                return true;
            }
            else
                return base.OnBackButtonPressed();
        }

        public StackLayout MainView
        {
            get
            {
                return new StackLayout
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Orientation = StackOrientation.Vertical,
                    Children = {
                        Title("CSWCSS eLearning App"),
                        Society,

           MainScreenRow(MainScreenItem(Image(ImageFile.Forum),delegate{
               /*Alert(this,"[2016-11-1 18:00:00] 1E03: Hi\n"+
               "[2016-11-1 18:00:09] 3F43: No one likes you loser\n[2016-11-1 18:00:16] 1E03: 😢😭😢😭😢😭😢😭😢\n"+
               "[2016-11-1 18:00:22] 2E12: Hey don't bully him!\n[2016-11-1 18:00:28] 3F43: Go kill yourself because you"+
               " are a F-ing faggot\n[2016-11-1 18:00:34] 2E12: I am going to rape you\n"+
               "[2016-11-1 18:00:55] 3F43: "+StrDup("😢😭😢😭😢😭😢😭😢",5));*/
               Showing = Pages.CloudTest;
                         }, BoldLabel("Forum\n(⁠C⁠l⁠o⁠u⁠d⁠T⁠e⁠s⁠t⁠)") ),
                         MainScreenItem(Image(ImageFile.Translate), delegate{
                             //Alert(this, "I'm a translator.\nInput: eifj[vguowhfuy9q727969y\nOutput: Gud mornin turists, we spek Inglish");
                         Showing = Pages.Translate; },
                         BoldLabel("Translator") ),
                         MainScreenItem(Image(ImageFile.VocabBook),delegate {Alert(this,"Ida = 捱打，伸張靜儀、儆惡懲奸，\n" +
"      救死扶傷、伸張靜儀、鋤強扶弱、儆惡懲奸、修身齊家、知足常樂"); },BoldLabel("Vocab Book"))),

           MainScreenRow(MainScreenItem(Image(ImageFile.Calculator),delegate {
                            Showing = Pages.Calculator;// Alert(this, "1+1=2");
                             },BoldLabel("Calculator")),
                         MainScreenItem(Image(ImageFile.Calculator_Free),delegate {
                             Showing = Pages.Calculator_Free;//Alert(this, StrDup("1+",100) + "1\n=101");
                             },BoldLabel("Calculator\nFree Mode")),
                         MainScreenItem(Image(ImageFile.Factorizer),delegate {
                             Showing = Pages.Factorizer;//Alert(this,"Factorize 3𝐗²(𝐗−1)²+2𝐗(𝐗−1)³\n = 𝐗(𝐗−1)²(5𝐗−2)");
                             },BoldLabel("Quadratic Factorizer"))),

           MainScreenRow(MainScreenItem(Image(ImageFile.Sports), delegate {
                             Showing = Pages.Sports;//Alert(this,"🏃🏃🏃長天長跑🏃🏃🏃");
                         },BoldLabel("Sports")),
                         MainScreenItem(Image(ImageFile.MusicTuner), delegate {
                             Showing = Pages.MusicTuner;//Alert(this,"🎼♯♩♪♭♫♬🎜🎝♮🎵🎶\n🎹🎻🎷🎺🎸");
                         },BoldLabel("Music Tuner")),
                         MainScreenItem(Image(ImageFile.MathSolver), delegate {
                             Showing = Pages.MathSolver; },BoldLabel("Maths Solver Minigame"))//Alert(this, "🔥🔥🔥🔥🔥🔥🐲🐉");
                         ),

                Button("Changelog", () => { Showing = Pages.Changelog; }),
                VersionDisplay
                    }
                };
            }
        }
        StreamPlayer MusicSound { get; set; }
        public StackLayout MusicTuner
        {
            get
            {
                Label Volume = (Text)"100";
                Slider Vol =
                    Slider((object sender, ValueChangedEventArgs e) =>
                    {
                        Volume.Text = ((int)e.NewValue).ToString().PadLeft(3);
                        if (MusicSound == null || MusicSound._disposedValue) return;
                        MusicSound.Volume = (float)e.NewValue / 100;
                    }, BackColor: Color.Gray);
                Button[] Violin = { MusicTunerPlay("G", Sounds.Violin_G, Vol),
                        MusicTunerPlay("D", Sounds.Violin_D, Vol),
                        MusicTunerPlay("A", Sounds.Violin_A, Vol),
                        MusicTunerPlay("E", Sounds.Violin_E, Vol)};
                for (int i = 0; i < 4; i++)
                    MusicTunerSwitch(Violin, i);

                Button[] Cello = { MusicTunerPlay("'C", Sounds.Cello_C, Vol),
                        MusicTunerPlay("'G", Sounds.Cello_G, Vol),
                        MusicTunerPlay("D", Sounds.Cello_D, Vol),
                        MusicTunerPlay("A", Sounds.Cello_A, Vol)};
                for (int i = 0; i < 4; i++)
                    MusicTunerSwitch(Cello, i);
                return new StackLayout
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Vertical,
                    Children = {
                        Title("CSWCSS Music Tuner"),
                        MainScreenRow(Image(ImageFile.Violin, delegate {Alert(this, "🎻♫♬♩♪♬♩♪♬"); })
                        , (Text)"Violin and Viola"),
                        Row(true, Violin),

                        MainScreenRow(Image(ImageFile.Cello, delegate {Alert(this, "🎻♫♬♩♪♬♩♪♬"); })
                        , (Text)"Cello and Double Bass"),
                        Row(true, Cello),

                        Button("Stop", () => {
                        for (int j = 0; j < 4; j++)
                            { Violin[j].BackgroundColor = Color.Silver; Cello[j].BackgroundColor = Color.Silver; }
                        MusicSound?.Dispose(); /*_Player.Play();*/ }),
                        Row(false, Volume, Vol),
                        Back(this)
                    }
                };
            }
        }
        public void MusicTunerSwitch(Button[] Violin, int i)
        {
            Violin[i].Clicked += delegate
            {
                for (int j = 0; j < 4; j++)
                    Violin[j].BackgroundColor = Color.Silver;
                Violin[i].BackgroundColor = Color.FromHex("#FF7F50"); //Coral (orange)
            };
        }
        public Button MusicTunerPlay(Text Text, Sounds Sound, Slider Vol)
        {
            return Button(Text, () =>
            { /*_Player.Pause();*/
                MusicSound?.Dispose(); MusicSound = Play(Sound, true, (float)Vol.Value);
            });
        }
        public StackLayout CloudTest
        {
            get
            {
                var Display = new
                {
                    ID = "ID:".PadRight(8),
                    Name = "Name:".PadRight(8),
                    Class = "Class:".PadRight(8),
                    Number = "Number:".PadRight(8)
                };

                Entry ID = new Entry
                {
                    Keyboard = Keyboard.Numeric,
                    Placeholder = "Student ID (without beginning s)",
                    PlaceholderColor = Color.Gray,
                    TextColor = Color.Black,
                    Text = "18999"
                };
                Entry E = new Entry
                {
                    Keyboard = Keyboard.Text,
                    Placeholder = "Password",
                    PlaceholderColor = Color.Gray,
                    TextColor = Color.Black,
                    Text = "Y1234567"
                };
                Label L1 = BoldLabel(Display.ID);
                Label L2 = BoldLabel(Display.Name);
                Label L3 = BoldLabel(Display.Class);
                Label L4 = BoldLabel(Display.Number);

                return new StackLayout
                {
                    Children = {ID, E, Button("Test the Cloud",
                    () => { var Response = Login(ToUShort(ID.Text), E.Text);
                    Try(delegate {
                    L1.Text = Display.ID + Response[0];    L2.Text = Display.Name + Response[1];
                    L3.Text = Display.Class + Response[2]; L4.Text = Display.Number + Response[3]; },
                    (IndexOutOfRangeException ex)=> {
                        Alert(this, "Abnornal return value from Cloud: " + '"' + string.Join(",", Response) + '"'); },
                        Catch2:(Exception ex) => { Alert(this, ex.ToString()); }
                    ); }),
                    L1, L2, L3, L4, Back(this)}
                    ,
                    VerticalOptions = LayoutOptions.Center
                };
            }
        }
        string Calculator_Value = "";
        List<Expressions> Calculator_Expression = new List<Expressions>();
        delegate void NoInputDelegate();
        event NoInputDelegate Calculator_Changed;
        AngleMode AngleUnit = 0;
        public StackLayout Calculator
        {
            get
            {
                Entry In = new Entry
                {
                    TextColor = Color.Black,
                    Placeholder = "Expression",
                    PlaceholderColor = Color.Gray,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    BackgroundColor = Color.FromRgb(0xD0, 0xD0, 0xD0),
                    Text = Calculator_Expression.AsString()
                };
                In.TextChanged += delegate
                {
                    if (In.Text != Calculator_Expression.AsString())
                        In.Text = Calculator_Expression.AsString();
                };
                Entry Out = new Entry
                {
                    TextColor = Color.Black,
                    Placeholder = "Result",
                    PlaceholderColor = Color.Gray,
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                Out.TextChanged += Calculator_TextChanged;
                Calculator_Changed += delegate { In.Text = Calculator_Expression.AsString(); };
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
                    Calculator_Expression.RemoveLast();
                    Calculator_Changed();
                }, Color.FromHex("#E91E63")), 3, 0);
                Norm.Children.Add(Button("⎚", () =>
                {
                    Calculator_Expression.Clear();
                    Calculator_Changed();
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
                    Calculator_Value = JSEvaluate(In.Text, this, AngleUnit, Calculator_Modifier);
                    Calculator_TextChanged(Out, new TextChangedEventArgs("", In.Text));
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
                Append(Func.Children, Expressions.Lb, "Lb", 0, 3);
                Append(Func.Children, Expressions.Ln, "Ln", 1, 3);
                Append(Func.Children, Expressions.Log, "Log", 2, 3);
                Append(Func.Children, Expressions.Pow, "Pow", 3, 3);
                Append(Func.Children, Expressions.Max, "Max", 0, 4);
                Append(Func.Children, Expressions.Min, "Min", 1, 4);
                Append(Func.Children, Expressions.Factorial, "Factor", 2, 4);
                Append(Func.Children, Expressions.Sign, "Sign", 3, 4);

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

                StackLayout Return = new StackLayout { Children = { In, new StackLayout(), Norm, new StackLayout(), Out } };
                Grid[] Menus = new Grid[] { Norm, Bin, Func, Trig, Const, Vars };
                Button Mode = new Button { Text = AngleUnit.ToString(), BackgroundColor = Color.FromHex("#02A8F3") };
                //Light Blue
                Mode.Clicked += delegate
                {
                    AngleUnit++; if (AngleUnit > AngleMode.Turn) AngleUnit = AngleMode.Degree;
                    Mode.Text = AngleUnit.ToString();
                };
                var Select = RadioButtons(Color.FromHex("#8AC249"), Color.FromHex("#4CAF50"),
                    i => delegate { if (Return.Children[2] != Menus[i]) Return.Children[2] = Menus[i]; }, 0,
                    nameof(Norm), nameof(Bin), nameof(Func), nameof(Trig), nameof(Const), nameof(Vars));
                Return.Children[1] = Row(false, Select[0], 
                    Scroll(StackOrientation.Horizontal, Select.Skip(1).Concat(new[] { Mode })), Back(this));
                var Modifiers = RadioButtons(Color.FromHex("#8AC249"), Color.FromHex("#4CAF50"),
                    i => delegate
                    {
                        if (Calculator_Modifier != (Modifier)i)
                        {
                            Calculator_Modifier = (Modifier)i;
                            try { Calculator_Value = JSEngine.Invoke("Display", JSEngine.GetCompletionValue(), i).ToString(); }
                            catch (System.Reflection.TargetInvocationException ex)
                            { Calculator_Value = ('ⓧ' + ex.InnerException.Message).Split('\r', '\n', '\f')[0]; }
                            catch (Exception ex) { Calculator_Value = ('ⓧ' + ex.Message).Split('\r', '\n', '\f')[0]; }
                            Out.Text = "";
                        }
                    }, 0,
                    "Norm", "%", "a b / c", "d / c", "° ′ ″", OnPlatform("e√f̅", "e√f̅", "e√̅f", "e√̅f", "e√f̅"), 
                    OnPlatform("g / h √f̅", "g / h √f̅", "g / h √̅f", "g / h √̅f", "g / h √f̅"));
                Return.Children[3] = Row(false, Modifiers[0], Scroll(StackOrientation.Horizontal, Modifiers.Skip(1)));
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
        Modifier Calculator_Modifier;
        #region Append
        public void Append(Grid.IGridList<View> List, Expressions Expression,
            Color BackColor = default(Color), Color TextColor = default(Color))
        {
            List.Add(Button(Expression, (object sender, ExpressionEventArgs e) =>
            { Calculator_Expression.Add(e.Expression); Calculator_Changed(); }, BackColor, TextColor));
        }
        public void Append(Grid.IGridList<View> List, Expressions Expression,
            int Left, int Top, Color BackColor = default(Color), Color TextColor = default(Color))
        {
            List.Add(Button(Expression, (object sender, ExpressionEventArgs e) =>
            { Calculator_Expression.Add(e.Expression); Calculator_Changed(); }, BackColor, TextColor), Left, Top);
        }
        public void Append(Grid.IGridList<View> List, Expressions Expression,
            int Left, int Right, int Top, int Bottom, Color BackColor = default(Color), Color TextColor = default(Color))
        {
            List.Add(Button(Expression, (object sender, ExpressionEventArgs e) =>
            { Calculator_Expression.Add(e.Expression); Calculator_Changed(); }, BackColor, TextColor), Left, Right, Top, Bottom);
        }
        public void Append(Grid.IGridList<View> List, Expressions Expression, Text Name,
            Color BackColor = default(Color), Color TextColor = default(Color))
        {
            List.Add(Button(Expression, (object sender, ExpressionEventArgs e) =>
            { Calculator_Expression.Add(e.Expression); Calculator_Changed(); }, Name, BackColor, TextColor));
        }
        public void Append(Grid.IGridList<View> List, Expressions Expression, Text Name,
            int Left, int Top, Color BackColor = default(Color), Color TextColor = default(Color))
        {
            List.Add(Button(Expression, (object sender, ExpressionEventArgs e) =>
            { Calculator_Expression.Add(e.Expression); Calculator_Changed(); }, Name, BackColor, TextColor), Left, Top);
        }
        public void Append(Grid.IGridList<View> List, Expressions Expression, Text Name,
            int Left, int Right, int Top, int Bottom, Color BackColor = default(Color), Color TextColor = default(Color))
        {
            List.Add(Button(Expression, (object sender, ExpressionEventArgs e) =>
            { Calculator_Expression.Add(e.Expression); Calculator_Changed(); }, Name, BackColor, TextColor), Left, Right, Top, Bottom);
        }
        #endregion
        private void Calculator_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((Entry)sender).Text != Calculator_Value) { ((Entry)sender).Text = Calculator_Value; }
        }
        string Calculator_Free_Value = "";
        public StackLayout Calculator_Free
        {
            get
            {
                Editor Editor = new Editor
                {
                    TextColor = Color.Black,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
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
                    {new ScrollView { Content = Editor,
                        HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand },
                    Row(false, Button("Evaluate", () => { Calculator_Free_Value = JSEvaluate(Editor.Text, this);
                        Calculator_Free_TextChanged(Entry, new TextChangedEventArgs(Entry.Text, Calculator_Free_Value)); }),
                        Back(this)),
                    Entry
                    }
                };
            }
        }

        private void Calculator_Free_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (((Entry)sender).Text != Calculator_Free_Value) { ((Entry)sender).Text = Calculator_Free_Value; }
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
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        Row(false, S1, C1, (Text)(X + "²")),
                        Row(false, S2, C2, (Text)(X + Y)),
                        Row(false, S3, C3, (Text)(Y + "²")),
                        Button("Factorize", () => {Factorizer_Result = Factorize(TryParseDouble(S1.Text + C1.Text, 0d),
                            TryParseDouble(S2.Text + C2.Text, 0d),
                            TryParseDouble(S3.Text + C3.Text, 0d), out System.Numerics.Complex X1, out System.Numerics.Complex X2, X, Y);
                            Factorizer_Root1 = X1.ToABi(); Factorizer_Root2 = X2.ToABi();
                            R1.Text = Factorizer_Root1; R2.Text = Factorizer_Root2; F.Text = Factorizer_Result; }), R1, R2, F,
                        Back(this)
                    }
                };
            }
        }
        Label Sports_Steps = BoldLabel("0", Color.White, Color.FromRgb(0x40, 0x40, 0x40), NamedSize.Large);
        Label Sports_Time = BoldLabel("00:00:00", Color.White, Color.FromRgb(0x40, 0x40, 0x40), NamedSize.Large);
        Label Sports_Distance = BoldLabel("0 m", Color.White, Color.FromRgb(0x40, 0x40, 0x40), NamedSize.Large);
        Label Sports_Now = BoldLabel(DateTime.Now.ToString("HH:mm:ss"), Color.White,
            Color.FromRgb(0x40, 0x40, 0x40), NamedSize.Large);
        IStepCounter Pedometer = new StepCounter(/*(uint Steps, TimeSpan TimePassed, float Distance) =>
            {
                Sports_Steps.Text = Steps.ToString(); Sports_Time.Text = TimePassed.ToString(@"hh\:mm\:ss");
                Sports_Distance.Text = Distance.ToString() + " m"; Sports_Now.Text = DateTime.Now.ToString("HH:mm:ss");
            }*/);
        public StackLayout Sports
        {
            get
            {
                Device.StartTimer(TimeSpan.FromSeconds(1),
                    () =>
                    {
                        Sports_Steps.Text = Pedometer.Steps.ToString();
                        Sports_Distance.Text = Pedometer.Distance.ToString() + " m";
                        Sports_Time.Text = Pedometer.TimePassed.ToString(@"hh\:mm\:ss");
                        Sports_Now.Text = DateTime.Now.ToString("HH:mm:ss");
                        return Showing == Pages.Sports;
                    });
                return new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children = {
                        Button("Start Running", () => { Pedometer.Start(); }, Color.Blue, Color.White),
                        Button("Stop Running", () => { Pedometer.Stop(); }, Color.Red, Color.White),
                        (Text)"Steps",
                        Sports_Steps,
                        (Text)"Elapsed Time",
                        Sports_Time,
                        (Text)"Estimated Distance",
                        Sports_Distance,
                        (Text)"Time Now",
                        Sports_Now,
                        Button("Reset", () => { Pedometer.Reset(); }, Color.Yellow),
                        Back(this)
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
                //Draw.DrawText("AbCdEfGhIjKlMnOpQrStUvWxYz", Size, TColor);
                var Dragon = new Image
                {
                    Source = Image(ImageFile.Dragon),
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
                        Dragon.Source = Image(ImageFile.Dragon_Dead);
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
                        int Segfault = 0;
                        RandomMove: if (++Segfault >= 12) goto Distribute;
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
                        Segfault = 0;
                    }
                    for (int i = 0; i < Questions[Level].Rows; i++)
                        for (int j = 0; j < Questions[Level].Columns; j++)
                            if (Chars[i, j].Text == "") Chars[i, j].Text =
                                 string.Concat(Questions[Level].ExtraChars, Answer).Random().ToString();
                };
                var Continue = Button("Continue",
                    (ref Button sender, EventArgs e) => { sender.IsVisible = false; ++Level; Advance(); });
                Draw.PointerEvent += (sender, e) =>
                {
                    switch (e.Type)
                    {
                        case TouchImage.PointerEventArgs.PointerEventType.Down:
                        case TouchImage.PointerEventArgs.PointerEventType.Move:
                            if (e.PointerDown)
                            {
                                Stack.Push((
                                  (int)(Math.Floor(e.Current.X *
                                  CharGrid.RowDefinitions.Count / CharGrid.Width
#if __ANDROID__
                                  / 3
#endif
                                  )).LowerBound(0).UpperBound(Questions[Level].Rows - 1),
                                  (int)(Math.Floor(e.Current.Y *
                                  CharGrid.ColumnDefinitions.Count / CharGrid.Height
#if __ANDROID__
                                  * 0.5
#elif NETFX_CORE
                                  * 1.5
#endif
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
                                Alert(this, "Correct! The dragon got hurt!", "Yay!", "I'll go on and continue");
                                ++Level; Advance(); }
                            else Alert(this, "You got the answer wrong...\nPlease retry.", "Ooops!", "I'll retry");
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
                    Children = {
                        Title("CSWCSS Maths Solver"),
                        Instruction,
                        Question,
                        Display, CharGrid, Dragon,
                        Row(false, Row(false, Hearts), Continue, Back(this)) }
                };
            }
        }
        SpeechToText TranslatorRecognizer = new SpeechToText("Say something to translate...", SpeechLanguages.English_US);
        ObservableCollection<Result> Favourites = new ObservableCollection<Result>();
        Button TranslatorButton(Result R)
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
                    if (e.OldItems?.Contains(R) == true) B.Text = "★+";
                    if (e.NewItems?.Contains(R) == true) B.Text = "★-";
                };
            return B;
        }
        public StackLayout Translator
        {
            get
            {
                var Input = Entry("", "Enter words...");
                var Recognize = Button("🎤", () =>
                { //http://developer.pearson.com/apis/dictionaries/
                  //Request(Get, "http://api.pearson.com/v2/dictionaries/ldec/entries?headword=" + Input.Text);
                    TranslatorRecognizer.TextChanged +=
                        (sender, e) => Device.BeginInvokeOnMainThread(() => Input.Text = e.Text);
                    TranslatorRecognizer.Start();
                    /*var detailsIntent = new Android.Content.Intent(Android.Speech.RecognizerIntent.ActionGetLanguageDetails);
                    LanguageDetailsChecker checker = new LanguageDetailsChecker();
                    Droid.MainActivity.Current.SendOrderedBroadcast(detailsIntent, null, checker, null,
                            Android.App.Result.Ok, null, null);
                    var a = checker.supportedLanguages;
                    ;*/
                });
                void ViewUpdate(StackLayout Layout, IEnumerable<Result> Results, params Span[] NoResults)
                {
                    Layout.Children.Clear();
                    foreach (var Result in Results)
                    {
                        Layout.Children.Add(
                            Row(false, TranslatorButton(Result),
                                FormattedLabel(
                                    new Span
                                    {
                                        Text = Result.headword.PadRight(27),
                                        ForegroundColor = Color.Black,
                                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                                        FontFamily = FontChinese//"Courier New, Georgia, Serif"
                                    },
                                    new Span
                                    {
                                        Text = Result.part_of_speech.PadRight(13),
                                        ForegroundColor = Color.Gray,
                                        FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                                        FontFamily = FontChinese//"Courier New, Georgia, Serif"
                                    }, new Span
                                    {
                                        Text = Result.senses.Single().translation + "\n",
                                        ForegroundColor = Color.Black,
                                        FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                                        FontFamily = FontChinese
                                    }
                                )
                            )
                        );
                    }
                    if (Layout.Children.Count == 0) Layout.Children.Add(FormattedLabel(NoResults));
                };
                var Formatted = new StackLayout
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Children = {
                        FormattedLabel(
                            new Span
                            {
                                Text = "Enter something and press translate,\nand the results will appear here.",
                                ForegroundColor = Color.Gray,
                                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                                FontFamily = "Courier New, Georgia, Serif"
                            }
                        )
                    }
                };
                var Translate = Button("→", () =>
                {
                    ViewUpdate(Formatted, OnlineDict.ToChinese(Input.Text).results, new Span
                    {
                        Text = "Not found!",
                        ForegroundColor = Color.Red,
                        FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                        FontFamily = "Courier New, Georgia, Serif"
                    });
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
                        Row(false, Recognize, Input, Translate),
                        new ScrollView { Orientation = ScrollOrientation.Both, Content = Formatted }
                    }
                }, 0, 0);
                Grid.Children.Add(new GridSplitter(Color.FromRgb(223, 223, 223)){ VerticalOptions = LayoutOptions.Center }, 0, 1);
                var FavouritesView = new StackLayout();
                void FavouritesUpdate() => ViewUpdate(FavouritesView, Favourites, new Span
                {
                    Text = "There's nothing here...\n",
                    ForegroundColor = Color.Gray,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    FontFamily = "Courier New, Georgia, Serif"
                }, new Span
                {
                    Text = "Press the button on the left of one of the\ntranslated results to add it into the Favourites!",
                    ForegroundColor = Color.Gray,
                    FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                    FontFamily = "Courier New, Georgia, Serif"
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
                return new StackLayout
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children = { Grid, Back(this) }
                };
            }
        }

        ~Main() { Storage.SerializedWrite(Storage.VocabFile, Favourites); }
    }
}