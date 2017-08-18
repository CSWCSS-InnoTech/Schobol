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
#if __IOS__
        {
            JavaScriptCore.JSContext _Engine = new JavaScriptCore.JSContext();
            public Task<string> Evaluate(string JavaScript) =>
                Task.Run(() => _Engine.EvaluateScript(JavaScript).ToString());
        }
#elif __ANDROID__
        {
            Android.Webkit.WebView _Engine = new Android.Webkit.WebView(Forms.Context);
            public SymbolicsEngine() => _Engine.Settings.JavaScriptEnabled = true;
            public async Task<string> Evaluate(string JavaScript)
            {
                var reset = new ManualResetEvent(false);
                var response = string.Empty;
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
                    //https://stackoverflow.com/questions/19788294/how-does-evaluatejavascript-work
                    Device.BeginInvokeOnMainThread(() => _Engine.EvaluateJavascript($"try{{{JavaScript}}}catch(e){{{Error}+(e.message?e.message:e)}}", new Callback((r) => { response = r; reset.Set(); })));
                else
                {
                    var _Interface = new Interface();
                    _Interface.Available += (sender, e) => { response = _Interface.Result; reset.Set(); };
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _Engine.AddJavascriptInterface(_Interface, "__Interface");
                        //_Engine.LoadData("", "text/html", null); //Must !! Load anything before target url
                        _Engine.LoadUrl($"javascript:{(Android.OS.Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.JellyBean ? "window." : string.Empty)}__Interface.__PutResult(eval(\"try{{{JavaScript.Replace("\\", "\\\\").Replace("\"", "\\\"")}}}catch(e){{{Error}+(e.message?e.message:e)}}\"))");
                    });
                }
                await Task.Run(() => reset.WaitOne());
                return response;
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
#elif WINDOWS_UWP
        {
            Jint.Engine _Engine = new Jint.Engine();
            public Task<string> Evaluate(string JavaScript) =>
                Task.Run(() => _Engine.Execute(JavaScript).GetCompletionValue().ToString());
        }
#endif
    }
}