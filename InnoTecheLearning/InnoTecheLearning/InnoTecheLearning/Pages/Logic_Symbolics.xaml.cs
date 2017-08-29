using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventHandler = System.EventHandler;
using EventArgs = System.EventArgs;

using Part = InnoTecheLearning.Utils.NerdamerPart;
using static InnoTecheLearning.Utils.NerdamerPart;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InnoTecheLearning.Pages
{
    [XamlCompilation(XamlCompilationOptions.Skip)]
    public partial class Logic_Symbolics : TabbedPage
    {
        //TODO: Add methods from https://help.syncfusion.com/cr/xamarin/calculate
        //Number suffix reference: http://stackoverflow.com/questions/7898310/using-regex-to-balance-match-parenthesis
        const int ButtonRows = 5;
        const int ButtonColumns = 5;
        #region Button Modifiers
        [System.Flags] enum ButtonModifier : byte
        {
            Norm = 0,
            Shift = 1,
            Alpha = 2,
            Alt = 4
        }
        ButtonModifier _ButtonMod = ButtonModifier.Norm;
        ButtonModifier ButtonMod
        {
            get => _ButtonMod;
            set
            {
                B03.Text = GetMapper(value).Item1.Name;
                for (int i = 0; i < ButtonRows; i++)
                    for (int j = 0; j < ButtonColumns; j++)
                    {
                        Buttons[i, j].Text = GetMapper(value).Item2[i, j].Name;
                    };

                if ((_ButtonMod ^ value).HasFlag(ButtonModifier.Shift))
                    if (value.HasFlag(ButtonModifier.Shift))
                    {
                        Back.Text = "CLR";
                        Shift.TranslateTo(0, 10, 0);
                    }
                    else
                    {
                        Back.Text = "←";
                        Shift.TranslateTo(0, 0, 0);
                    }
                if ((_ButtonMod ^ value).HasFlag(ButtonModifier.Alpha))
                    Alpha.TranslateTo(0, value.HasFlag(ButtonModifier.Alpha) ? 10 : 0, 0);
                if ((_ButtonMod ^ value).HasFlag(ButtonModifier.Alt))
                    Alt.TranslateTo(0, value.HasFlag(ButtonModifier.Alt) ? 10 : 0, 0);
                _ButtonMod = value;
            }
        }
#endregion
        #region Mappers
        static readonly (Part, Part[,]) Invalid = (Empty, Utils.Duplicate(Empty, ButtonRows, ButtonColumns));
        static (Part, Part[,]) GetMapper(ButtonModifier m)
        {
            if (Mappers.TryGetValue(m, out var Result)) return Result;
            else return Invalid;
        }
        static readonly Dictionary<ButtonModifier, (Part, Part[,])> Mappers =
            new Dictionary<ButtonModifier, (Part, Part[,])>
            {
                [ButtonModifier.Norm] = (Percent, new Part[ButtonRows, ButtonColumns]
                {
                    { Comma, Equation, Assign, LeftRound, RightRound },
                    { D7, D8, D9, Exponent, Factorial },
                    { D4, D5, D6, Multiply, Divide },
                    { D1, D2, D3, Add, Subtract },
                    { D0, Decimal, ConstPi, ConstE, ConstI }
                }),
                [ButtonModifier.Shift] = (Fib, new Part[ButtonRows, ButtonColumns]
                {
                    { Log, Log10, Sqrt, LeftSquare, RightSquare },
                    { Floor, Ceil, Round, Trunc, Mod },
                    { Gcd, Lcm, Mean, Mode, Median },
                    { Expand, DivideFunc, PFactor, Min, Max },
                    { Factor, Roots, Coeffs, Solve, SolveEquations }
                }),
                [ButtonModifier.Alpha] = (a, new Part[ButtonRows, ButtonColumns]
                {
                    { b, c, d, e, f },
                    { g, h, i, j, k },
                    { l, m, n, o, p },
                    { q, r, s, t, u },
                    { v, w, x, y, z }
                }),
                [ButtonModifier.Shift | ButtonModifier.Alpha] = (A, new Part[ButtonRows, ButtonColumns]
                {
                    { B, C, D, E, F },
                    { G, H, I, J, K },
                    { L, M, N, O, P },
                    { Q, R, S, T, U },
                    { V, W, Part.X, Part.Y, Z }
                }),
                [ButtonModifier.Alt] = (Atan2, new Part[ButtonRows, ButtonColumns]
                {
                    { Sin, Asin, Sinh, Asinh, Empty },
                    { Cos, Acos, Cosh, Acosh, Empty },
                    { Tan, Atan, Tanh, Atanh, Empty },
                    { Empty, Sinc, Empty, Empty, Empty },
                    { Sum, Product, Diff, Integrate, Defint }
                }),
                [ButtonModifier.Shift | ButtonModifier.Alt] = (Step, new Part[ButtonRows, ButtonColumns]
                {
                    { Sec, Asec, Sech, Asech, Erf },
                    { Csc, Acsc, Csch, Acsch, Rect },
                    { Cot, Acot, Coth, Acoth, Tri },
                    { Si, Ci, Shi, Chi, Ei },
                    { Laplace, Smpvar, Variance, Smpstdev, Stdev }
                }),
                [ButtonModifier.Alt | ButtonModifier.Alpha] = (NotEqual, new Part[ButtonRows, ButtonColumns]
                {
                    { LessThan, LessEqual, Equal, GreaterEqual, GreaterThan },
                    { Empty, Empty, Empty, Empty, Empty },
                    { IMatrix, Empty, Empty, Empty, Empty },
                    { Matrix, Matget, Matset, Invert, Transpose },
                    { Vector, Vecget, Vecset, Cross, Dot }
                })
            };
#endregion
        readonly Button[,] Buttons;
        bool DisplayDecimals = true;
        bool DoEvaluate = true;
        public Logic_Symbolics()
        {
            #region Declarations
            InitializeComponent();
            
            Buttons = new Button[ButtonRows, ButtonColumns]
            {
                { B10, B11, B12, B13, B14 },
                { B20, B21, B22, B23, B24 },
                { B30, B31, B32, B33, B34 },
                { B40, B41, B42, B43, B44 },
                { B50, B51, B52, B53, B54 }
            };
            EventHandler ModClicked(ButtonModifier Modifier) => (sender, e) => ButtonMod ^= Modifier;
            EventHandler ButtonClicked = (sender, e) =>
            {
                var (x, y) = Utils.IndicesOf(Buttons, (Button)sender);
                In.Text += 
                    x == -1 && y == -1 ? GetMapper(ButtonMod).Item1.FullName : GetMapper(ButtonMod).Item2[x, y].FullName;
                ButtonMod = ButtonModifier.Norm;
            };
            EventHandler ButtonLongPressed = (sender, e) => 
            {
                var button = (Button)sender;
                var (x, y) = Utils.IndicesOf(Buttons, button);
                var Part = x == -1 && y == -1 ? GetMapper(ButtonMod).Item1 : GetMapper(ButtonMod).Item2[x, y];
                if (Part.DescriptionContent != null) DisplayAlert(Part.DescriptionTitle, Part.DescriptionContent, "OK");
            };
            #endregion

            #region Wiring up
            In.Completed += Calculate_Clicked;

            Shift.Clicked += ModClicked(ButtonModifier.Shift);
            Alpha.Clicked += ModClicked(ButtonModifier.Alpha);
            Alt.Clicked += ModClicked(ButtonModifier.Alt);
            Back.Clicked += (sender, e) =>
                {
                    In.Text = !ButtonMod.HasFlag(ButtonModifier.Shift) && In.Text?.Length > 0 ?
                       In.Text.Remove(In.Text.Length - 1) : string.Empty;
                    ButtonMod = ButtonModifier.Norm;
                };

            B03.Clicked += ButtonClicked;
            Utils.LongPress.Register(B03, ButtonLongPressed);
            for (int i = 0; i < ButtonRows; i++)
                for (int j = 0; j < ButtonColumns; j++)
                {
                    Buttons[i, j].Clicked += ButtonClicked;
                    Utils.LongPress.Register(Buttons[i, j], ButtonLongPressed);
                }

            Calculate.Clicked += Calculate_Clicked;
            //Utils.LongPress.Register(Calculate, async (sender, e) => Out.Text = await (await Current).Evaluate(In.Text));
            Display.Clicked += (sender, e) => Display.Text = (DisplayDecimals = !DisplayDecimals) ? "Display Decimals" : "Display Fractions";
            Evaluate.Clicked += (sender, e) => Evaluate.Text = (DoEvaluate = !DoEvaluate) ? "Evaluate Symbols" : "Keep Symbols";

            Out.Focused += (sender, e) => Out.Unfocus();
            OutCopy.Clicked += (sender, e) => Utils.ClipboardText = Out.Text;


            /*
            async void Debug_Clicked(object sender, EventArgs e) => await Eval("{0}");
            Debug.Clicked += Debug_Clicked;
            */
            #endregion
        }

        ~Logic_Symbolics()
        {
            Utils.LongPress.UnregisterAll(B03);
            for (int i = 0; i < ButtonRows; i++)
                for (int j = 0; j < ButtonColumns; j++)
                    Utils.LongPress.UnregisterAll(Buttons[i, j]);
        }

        #region Talking to the Engine
        async ValueTask<Utils.Unit> Eval()
        {
            Out.Text = await (await Current).Evaluate(string.Concat(
                "try{",
                    "nerdamer('", Utils.EncodeJavascript(In.Text, false), "')",
                    DoEvaluate ? ".evaluate()" : string.Empty,
                    DisplayDecimals ? ".text()" : ".toString()",
                "}catch(e){'", Utils.Error, "'+(e.message?e.message:e)}"
                ));
            return Utils.Unit.Default;
        }
        async void Calculate_Clicked(object sender, EventArgs e) => await Eval();

        ValueTask<Utils.SymbolicsEngine> Current = CreateEngineAsync();
        static ValueTask<Utils.SymbolicsEngine> CreateEngineAsync() => new ValueTask<Utils.SymbolicsEngine>(
            Task.Run(async () =>
        {
            var Return = new Utils.SymbolicsEngine();
            await Return.Evaluate(Utils.Resources.GetString("nerdamer.core.js"));
            await Return.Evaluate(Utils.Resources.GetString("Algebra.js"));
            await Return.Evaluate(Utils.Resources.GetString("Calculus.js"));
            await Return.Evaluate(Utils.Resources.GetString("Solve.js"));
            await Return.Evaluate(Utils.Resources.GetString("Extra.js"));
            await Return.Evaluate("nerdamer.setFunction('lcm', ['a', 'b'], '(a / gcd(a, b)) * b')");
            return Return;
        }));

        ValueTask<Dictionary<string, string>> History
        {
            get
            {
                async ValueTask<Dictionary<string, string>> Return() => 
                    Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>
                        (await (await Current).Evaluate("nerdamer.getVars()"));
                return Return();
            }
        }
        #endregion
    }
}