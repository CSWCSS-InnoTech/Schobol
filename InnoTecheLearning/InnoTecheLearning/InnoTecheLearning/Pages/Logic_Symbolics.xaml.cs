#undef DEBUG_SYMBOLICS

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EventHandler = System.EventHandler;
using EventArgs = System.EventArgs;

using Part = InnoTecheLearning.Utils.NerdamerPart;
using static InnoTecheLearning.Utils.NerdamerPart;
using static InnoTecheLearning.Utils.SymbolicsEngine;

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
                B03.Text = GetMapper(value).Item1.Friendly;
                for (int i = 0; i < ButtonRows; i++)
                    for (int j = 0; j < ButtonColumns; j++)
                    {
                        Buttons[i, j].Text = GetMapper(value).Item2[i, j].Friendly;
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
                //Unassigned: Assign
                [ButtonModifier.Norm] = (Percent, new Part[ButtonRows, ButtonColumns]
                {
                    { Comma, Equation, Space, LeftRound, RightRound },
                    { D7, D8, D9, Exponent, Factorial },
                    { D4, D5, D6, Multiply, Divide },
                    { D1, D2, D3, Add, Subtract },
                    { D0, Decimal, ConstPi, ConstE, ConstI }
                }),
                [ButtonModifier.Shift] = (Fib, new Part[ButtonRows, ButtonColumns]
                {
                    { Sin, Cos, Tan, LeftSquare, RightSquare },
                    { Asin, Acos, Atan, DivideFunc, Mod },
                    { Log, Log10, Sqrt, Gcd, Lcm },
                    { Expand, PFactor, Mean, Mode, Median },
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
                    { Sinh, Cosh, Tanh, Min, Max },
                    { Asinh, Acosh, Atanh, Empty, Empty },
                    { Empty, Empty, Empty, Empty, Empty },
                    { Sinc, Floor, Ceil, Round, Trunc },
                    { Sum, Product, Diff, Integrate, Defint }
                }),
                [ButtonModifier.Shift | ButtonModifier.Alt] = (Si, new Part[ButtonRows, ButtonColumns]
                {
                    { Sec, Csc, Cot, Ci, Erf },
                    { Asec, Acsc, Acot, Shi, Step },
                    { Sech, Csch, Coth, Chi, Tri },
                    { Asech, Acsch, Acoth, Ei, Rect },
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
            #region Field Initializers
            InitializeComponent();

            Buttons = new Button[ButtonRows, ButtonColumns]
            {
                { B10, B11, B12, B13, B14 },
                { B20, B21, B22, B23, B24 },
                { B30, B31, B32, B33, B34 },
                { B40, B41, B42, B43, B44 },
                { B50, B51, B52, B53, B54 }
            };
            #endregion
            #region Declarations
            EventHandler ModClicked(ButtonModifier Modifier) => (sender, e) => ButtonMod ^= Modifier;
            int? IndexNextToCursor(bool Before, int? CursorIndexOverride = null)
            {
                var T = In.Text;
                var Index = CursorIndexOverride ?? T.IndexOf(Utils.Cursor);
                if (Index == -1)
                {
                    return null;
                }
                var Splitted = Splitter.Matches(T);
                int LengthIterated = 0;
                foreach (System.Text.RegularExpressions.Match M in Splitted)
                {
                    if (Before && LengthIterated + M.Length >= Index)
                    {
                        return LengthIterated;
                    }
                    else if (!Before && LengthIterated > Index)
                    {
                        return LengthIterated + M.Length;
                    }
                    LengthIterated += M.Length;
                }
                return Index;
            }
            EventHandler CursorMovePressed(bool MoveLeft) => (sender, e) => 
            {
                var OldIndex = In.Text.IndexOf(Utils.Cursor);
                var NewIndex = IndexNextToCursor(MoveLeft);
                if (NewIndex == null)
                {
                    In.Text = In.Text.Insert(MoveLeft ? In.Text.Length : 0, Utils.Cursor);
                }
                else if (MoveLeft)
                {
                    In.Text = In.Text.Insert(NewIndex.Value, Utils.Cursor).Remove(OldIndex + 1, 1);
                    return;
                }
                else
                {
                    In.Text = In.Text.Insert(NewIndex.Value, Utils.Cursor).Remove(OldIndex, 1);
                    return;
                }
            };
            EventHandler ButtonClicked = (sender, e) =>
            {
                var (x, y) = Utils.IndicesOf(Buttons, (Button)sender);
                var ToAdd = x == -1 && y == -1 ? GetMapper(ButtonMod).Item1.Name : GetMapper(ButtonMod).Item2[x, y].Name;
                if (In.Text.Contains(Utils.Cursor)) In.Text = In.Text.Insert(In.Text.IndexOf(Utils.Cursor), ToAdd);
                else In.Text += ToAdd;
                ButtonMod = ButtonModifier.Norm;
            };
            EventHandler ButtonLongPressed = (sender, e) =>
            {
                var button = (Button)sender;
                var (x, y) = Utils.IndicesOf(Buttons, button);
                var Part = x == -1 && y == -1 ? GetMapper(ButtonMod).Item1 : GetMapper(ButtonMod).Item2[x, y];
                if (Part.DescriptionContent != null) DisplayAlert(Part.DescriptionTitle, Part.DescriptionContent, "OK");
            };
            async void Calculate_Clicked(object sender, EventArgs e) => Out.Text = await Eval(In.Text, DoEvaluate, DisplayDecimals);
            #endregion

            #region Wiring up
            Left.Clicked += CursorMovePressed(true);
            In.Completed += Calculate_Clicked;
            Right.Clicked += CursorMovePressed(false);

            Shift.Clicked += ModClicked(ButtonModifier.Shift);
            Alpha.Clicked += ModClicked(ButtonModifier.Alpha);
            Alt.Clicked += ModClicked(ButtonModifier.Alt);
            Back.Clicked += (sender, e) =>
                {
                    var Index = IndexNextToCursor(true);
                    In.Text = !ButtonMod.HasFlag(ButtonModifier.Shift) && In.Text?.Length > 0 ?
                            Index == null ? In.Text.Remove(IndexNextToCursor(true, In.Text.Length).Value) : 
                            In.Text.Remove(Index.Value, In.Text.IndexOf(Utils.Cursor) - Index.Value)
                        : string.Empty;
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
#if DEBUG_SYMBOLICS
            Utils.LongPress.Register(Calculate, async (sender, e) => Out.Text = await (await Current).Evaluate(In.Text));
#endif
            Display.Clicked += (sender, e) => Display.Text = (DisplayDecimals = !DisplayDecimals) ? "Display Decimals" : "Display Fractions";
            Evaluate.Clicked += (sender, e) => Evaluate.Text = (DoEvaluate = !DoEvaluate) ? "Evaluate Symbols" : "Keep Symbols";

            Out.Focused += (sender, e) => Out.Unfocus();
            OutCopy.Clicked += (sender, e) => Utils.ClipboardText = Out.Text;

#if DEBUG_SYMBOLICS
            async void Debug_Clicked(object sender, EventArgs e) => await Eval("{0}");
            Debug.Clicked += Debug_Clicked;
#endif
            
            HistoryView.ItemsSource = History;
            HistoryView.ItemTapped += async (sender, e) =>
            {
                if (!string.IsNullOrEmpty(In.Text))
                    if (!await DisplayAlert("Discard calculation",
                        "The current calculation will be discarded.", "Discard", "Cancel")) return;
                HistoryView.SelectedItem = null;
                In.Text = ((KeyValuePair<string, string>)e.Item).Key;
                Out.Text = string.Empty;
                CurrentPage = Calculator;
            };

            ClearHistory.Clicked += (sender, e) => History.Clear();
            #endregion
        }

        ~Logic_Symbolics()
        {
            Utils.LongPress.UnregisterAll(B03);
            for (int i = 0; i < ButtonRows; i++)
                for (int j = 0; j < ButtonColumns; j++)
                    Utils.LongPress.UnregisterAll(Buttons[i, j]);
        }
    }
}