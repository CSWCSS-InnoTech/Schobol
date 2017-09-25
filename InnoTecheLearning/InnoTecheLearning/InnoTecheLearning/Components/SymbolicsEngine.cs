using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

//[assembly: ExportRenderer(typeof(InnoTecheLearning.SymbolicsEngine.WebViewer), typeof(InnoTecheLearning.SymbolicsEngine.WebViewRender))]
namespace InnoTecheLearning
{
    partial class Utils
    {
        public class SymbolicsEngine
#if false
        {
            Jint.Engine _Engine = new Jint.Engine();
            public ValueTask<string> Evaluate(string JavaScript)
            {
                EvaluateCalling(this, EventArgs.Empty);
                var Return = new ValueTask<string>(Task.Run(() => 
                    _Engine.Execute(JavaScript).GetCompletionValue().ToString()));
                Return.GetAwaiter().OnCompleted(() => EvaluateCalled(this, EventArgs.Empty));
                return Return;
            }
#elif __IOS__
        {
            JavaScriptCore.JSContext _Engine = new JavaScriptCore.JSContext();
            public ValueTask<string> Evaluate(string JavaScript) =>
                new ValueTask<string>(Task.Run(() => 
                    _Engine.EvaluateScript(JavaScript).ToString()));
#elif __ANDROID__
        {
            /// <summary>
            /// Turns a string such as "abc" into abc.
            /// If the string is null (literally and reference), then nothing is performed.
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            static string Trim(string s)
            {
                try
                {
                    if (s == "null") return s;
                    return s.Substring(1, s.Length - 2);
                } catch { return s; }
            }
            TaskCompletionSource<Unit> _Ready = new TaskCompletionSource<Unit>();
            Android.Webkit.WebView _Engine;
            public SymbolicsEngine() =>
                Device.BeginInvokeOnMainThread(() =>
                {
                    _Engine = new Android.Webkit.WebView(Forms.Context);
                    _Engine.Settings.JavaScriptEnabled = true;
                    _Ready.SetResult(Unit.Default);
                });
            public async ValueTask<string> Evaluate(string JavaScript)
            {
                var completion = new TaskCompletionSource<string>();
                await _Ready.Task;
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
                    //$"try{{{JavaScript}}}catch(e){{{Error}+(e.message?e.message:e)}}"
                    //https://stackoverflow.com/questions/19788294/how-does-evaluatejavascript-work
                    Device.BeginInvokeOnMainThread(() => _Engine.EvaluateJavascript(JavaScript, new Callback((r) => completion.SetResult(Trim(r)))));
                else
                {
                    var _Interface = new Interface();
                    _Interface.Available += (sender, e) => completion.SetResult(Trim(_Interface.Result));
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _Engine.AddJavascriptInterface(_Interface, "__Interface");
                        //_Engine.LoadData("", "text/html", null); //Must !! Load anything before target url
                        var PutResult = $"{(Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.JellyBean ? "window." : string.Empty)}__Interface.__PutResult";
                        _Engine.LoadUrl($"javascript:eval({EncodeJavascript(JavaScript)})");
                    });
                }
                return await completion.Task;
            }
            class Callback : Java.Lang.Object, Android.Webkit.IValueCallback
            {
                Action<string> callback;
                public Callback(Action<string> callback) => this.callback = callback;
                void Android.Webkit.IValueCallback.OnReceiveValue(Java.Lang.Object value) =>
                    callback(Android.Runtime.Extensions.JavaCast<Java.Lang.String>(value).ToString());
            }
            class Interface : Java.Lang.Object
            {
                string result;
                public string Result => result;
                public event EventHandler Available = (sender, e) => { };

                public Interface() : base() { }

                public Interface(IntPtr handle, Android.Runtime.JniHandleOwnership transfer)
                    : base (handle, transfer)
                { }

                [Java.Interop.Export("__PutResult")]
                [Android.Webkit.JavascriptInterface]
                public string __PutResult(string Result)
                {
                    result = Result;
                    Available(this, EventArgs.Empty);
                    return result;
                }
            }
#else
        {
            Jint.Engine _Engine = new Jint.Engine();
            public ValueTask<string> Evaluate(string JavaScript) =>
                new ValueTask<string>(Task.Run(() => 
                    _Engine.Execute(JavaScript).GetCompletionValue().ToString()));
#endif

            #region Singleton Engine
            public static void Init() =>
                Task.Run(() =>
                {
                    Equals(Current, History); //Runs the static ctor which inits below properties
                    NerdamerPart.Init();
                });

            public static async ValueTask<string> Eval(string Expression, bool DoEvaluate, bool DisplayDecimals)
            {
                var T = Expression.Replace(Cursor, "");
                string Return;
                History.Add(new KeyValuePair<string, string>(T,
                    Return = await (await Current).Evaluate(string.Concat(
                    "try{",
                        "nerdamer('", EncodeJavascript(T, false), "', undefined, 'expand')",
                        DoEvaluate ? ".evaluate()" : string.Empty,
                        DisplayDecimals ? ".text()" : ".toString()",
                    "}catch(e){'", Error, "'+(e.message?e.message:e)}"
                    ))));
                return Return;
            }

            public static ValueTask<SymbolicsEngine> Current { get; } = CreateEngineAsync();
            public static ValueTask<SymbolicsEngine> CreateEngineAsync() => new ValueTask<SymbolicsEngine>(
                Task.Run(async () =>
                {
                    var Return = new SymbolicsEngine();
                    await Return.Evaluate(Resources.GetString("nerdamer.core.js"));
                    await Return.Evaluate(Resources.GetString("Algebra.js"));
                    await Return.Evaluate(Resources.GetString("Calculus.js"));
                    await Return.Evaluate(Resources.GetString("Solve.js"));
                    await Return.Evaluate(Resources.GetString("Extra.js"));
                    await Return.Evaluate("nerdamer.setVar('π', 'pi')");
                    await Return.Evaluate("nerdamer.setFunction('lcm', ['a', 'b'], '(a / gcd(a, b)) * b')");
                    await Return.Evaluate("nerdamer.setFunction('asec', 'x', 'acos(1/x)')");
                    await Return.Evaluate("nerdamer.setFunction('acsc', 'x', 'asin(1/x)')");
                    await Return.Evaluate("nerdamer.setFunction('acot', 'x', 'trunc(((x/abs(x))-1)/2)*pi+acos(1/x)')");
                    //TODO 0.11.x: Support for radians, gradians and turns
                    string TrigRepl(string Name, bool Inverse) =>
                        $"{{var f=Math.{Name};Math.{Name}=function(x){{return f(x{(Inverse ? '/' : '*')}Math.PI{(Inverse ? '*' : '/')}180)}}}}";
                    await Return.Evaluate(TrigRepl("sin", false));
                    await Return.Evaluate(TrigRepl("cos", false));
                    await Return.Evaluate(TrigRepl("tan", false));
                    await Return.Evaluate(TrigRepl("asin", true));
                    await Return.Evaluate(TrigRepl("acos", true));
                    await Return.Evaluate(TrigRepl("atan", true));
                    //await Return.Evaluate("nerdamer.setFunction('sin', 'x', 'sin_(x*π/180)')");
                    //await Return.Evaluate("nerdamer.getCore().PARSER.constants['π'] = 'pi'");
                    //await Return.Evaluate(@"{var f =nerdamer.getCore().PARSER.functions['sin']; }");
                    //await Return.Evaluate("nerdamer.setFunction('lcm', ['a', 'b'], '(a / gcd(a, b)) * b')");
                    return Return;
                }));

            public static ObservableCollection<KeyValuePair<string, string>> History { get; } =
                new ObservableCollection<KeyValuePair<string, string>>() { };

            public static ValueTask<ObservableDictionary<string, string>> Vars { get; } = CreateVarsAsync();
            public static ValueTask<ObservableDictionary<string, string>> CreateVarsAsync() =>
                new ValueTask<ObservableDictionary<string, string>>(
                    Task.Run(async () =>
                    {
                        var Return =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableDictionary<string, string>>
                        (await (await Current).Evaluate("nerdamer.getVars()"));
                        Return.CollectionChanged += async (sender, e) =>
                        {
                            switch (e.Action)
                            {
                                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                                    foreach (KeyValuePair<string, string> Item in e.NewItems)
                                        await (await Current).Evaluate(
                                        $"nerdamer.setVar('{EncodeJavascript(Item.Key)}', '{EncodeJavascript(Item.Value)}')");
                                    break;
                                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                                    foreach (KeyValuePair<string, string> Item in e.OldItems)
                                        await (await Current).Evaluate(
                                        $"nerdamer.setVar('{EncodeJavascript(Item.Key)}', 'delete')");
                                    break;
                                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                                    foreach (KeyValuePair<string, string> Item in e.OldItems)
                                        await (await Current).Evaluate(
                                        $"nerdamer.setVar('{EncodeJavascript(Item.Key)}', 'delete')");
                                    foreach (KeyValuePair<string, string> Item in e.NewItems)
                                        await (await Current).Evaluate(
                                        $"nerdamer.setVar('{EncodeJavascript(Item.Key)}', '{EncodeJavascript(Item.Value)}')");
                                    break;
                                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                                    break;
                                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                                    await (await Current).Evaluate($"nerdamer.clearVars()");
                                    break;
                                default:
                                    break;
                            }
                        };
                        return Return;
                    }));
            #endregion
        }
    }
}