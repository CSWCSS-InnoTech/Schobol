using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

//[assembly: ExportRenderer(typeof(InnoTecheLearning.Utils.SymbolicsEngine.WebViewer), typeof(InnoTecheLearning.Utils.SymbolicsEngine.WebViewRender))]
namespace InnoTecheLearning
{
    partial class Utils
    {
        public class SymbolicsEngine
#if false
        {
            Jint.Engine _Engine = new Jint.Engine();
            public Task<string> Evaluate(string JavaScript) =>
                Task.Run(() => _Engine.Execute(JavaScript).GetCompletionValue().ToString());
        }
#elif __IOS__
        {
            JavaScriptCore.JSContext _Engine = new JavaScriptCore.JSContext();
            public Task<string> Evaluate(string JavaScript) =>
                Task.Run(() => _Engine.EvaluateScript(JavaScript).ToString());
        }
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
        }
#else
        {
            Jint.Engine _Engine = new Jint.Engine();
            public Task<string> Evaluate(string JavaScript) =>
                Task.Run(() => _Engine.Execute(JavaScript).GetCompletionValue().ToString());
        }
#endif
    }
}