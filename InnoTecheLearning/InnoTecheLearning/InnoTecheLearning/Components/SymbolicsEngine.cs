using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.
#if __IOS__
    iOS;
#elif __ANDROID__
    Android;
#elif WINDOWS_UWP
    UWP;
#endif

//[assembly: ExportRenderer(typeof(InnoTecheLearning.Utils.SymbolicsEngine.WebViewer), typeof(InnoTecheLearning.Utils.SymbolicsEngine.WebViewRender))]
namespace InnoTecheLearning
{
    partial class Utils
    {
        //NoReturn = No return statement, e.g. var a = 1+1; a
        //WithReturn = Use return statement, e.g. var a = 1+1; return a
        public class SymbolicsEngine
#if false
        {
            public SymbolicsEngine() { }
            WebViewer _Engine = new WebViewer();
            public Task<string> EvaluateNoReturn(string JavaScript) => _Engine.EvaluateJavascript(JavaScript);
            public Task<string> EvaluateWithReturn(string JavaScript) => _Engine.EvaluateJavascript($"(function(){{{JavaScript}}})()");

            internal class WebViewer : WebView
            {
                public WebViewer() : base() { }
                public static BindableProperty EvaluateJavascriptProperty =
                BindableProperty.Create(nameof(EvaluateJavascript), typeof(Func<string, Task<string>>),
                    typeof(WebViewer), null, BindingMode.OneWayToSource);

                public Func<string, Task<string>> EvaluateJavascript
                {
                    get { return (Func<string, Task<string>>)GetValue(EvaluateJavascriptProperty); }
                    set { SetValue(EvaluateJavascriptProperty, value); }
                }
            }
            internal class WebViewRender : WebViewRenderer
            {
                protected override void OnElementChanged(
#if __IOS__
                    VisualElementChangedEventArgs
#else
                    ElementChangedEventArgs<WebView>
#endif
                    e)
                {
                    base.OnElementChanged(e);

                    if (e.NewElement is WebViewer webView)
                        webView.EvaluateJavascript = Eval;
                }
#if __IOS__
                Task<string> Eval(string js) => Task.FromResult(this.EvaluateJavascript(js));
#elif __ANDROID__
                async Task<string> Eval(string js)
                {
                    var reset = new ManualResetEvent(false);
                    var response = string.Empty;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Control?.EvaluateJavascript($"var x={js};", new JavascriptCallback((r) => { response = r; reset.Set(); }));
                    });
                    await Task.Run(() => { reset.WaitOne(); });
                    return response;
                }
                class JavascriptCallback : Java.Lang.Object, Android.Webkit.IValueCallback
                {
                    public JavascriptCallback(Action<string> callback)
                    {
                        _callback = callback;
                    }

                    private Action<string> _callback;
                    public void OnReceiveValue(Java.Lang.Object value)
                    {
                        _callback?.Invoke(Convert.ToString(value));
                    }
                }
#elif WINDOWS_UWP
                Task<string> Eval(string js) => Control.InvokeScriptAsync("eval", new[] { js }).AsTask();
#endif
            }

        }
#elif __IOS__
        {
            JavaScriptCore.JSContext _Engine = new JavaScriptCore.JSContext();
            public Task<string> EvaluateNoReturn(string JavaScript) =>
                Task.Run(() => _Engine.EvaluateScript(JavaScript).ToString());
            public Task<string> EvaluateWithReturn(string JavaScript) =>
                Task.Run(() => _Engine.EvaluateScript($"(function(){{{JavaScript}}})()").ToString());
        }
#elif __ANDROID__
        {
            Android.Webkit.WebView _Engine = new Android.Webkit.WebView(Xamarin.Forms.Forms.Context);
            public SymbolicsEngine() { _Engine.Settings.JavaScriptEnabled = true; }
            public Task<string> EvaluateNoReturn(string JavaScript)
            {
                var CompletionSource = new TaskCompletionSource<string>();
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
                    //https://stackoverflow.com/questions/19788294/how-does-evaluatejavascript-work
                    _Engine.EvaluateJavascript($"(function(){{return ({JavaScript});}})()", new Callback(CompletionSource.SetResult));
                else _Engine.LoadUrl($"javascript:(function(){{return ({JavaScript});}});");
                return CompletionSource.Task;
            }
            public Task<string> EvaluateWithReturn(string JavaScript)
            {
                var CompletionSource = new TaskCompletionSource<string>();
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
                    //https://stackoverflow.com/questions/19788294/how-does-evaluatejavascript-work
                    _Engine.EvaluateJavascript($"(function(){{{JavaScript}}})()", new Callback(CompletionSource.SetResult));
                else _Engine.LoadUrl($"javascript:(function(){{{JavaScript}}});");
                return CompletionSource.Task;
            }
            class Callback : Java.Lang.Object, Android.Webkit.IValueCallback
            {
                Action<string> callback;
                public Callback(Action<string> callback) => this.callback = callback;
                void Android.Webkit.IValueCallback.OnReceiveValue(Java.Lang.Object value) =>
                    callback(Android.Runtime.Extensions.JavaCast<Java.Lang.String>(value).ToString());
            }
        }
#elif WINDOWS_UWP
        {
            Jint.Engine _Engine = new Jint.Engine();
            public Task<string> EvaluateNoReturn(string JavaScript) =>
                Task.Run(() => _Engine.Execute(JavaScript).GetCompletionValue().ToString());
            public Task<string> EvaluateWithReturn(string JavaScript) =>
                Task.Run(() => _Engine.Execute($"(function(){{{JavaScript}}})()").GetCompletionValue().ToString());
        }
#endif
    }
}