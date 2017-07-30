using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public class SymbolicsEngine
#if __IOS__
        {
            JavaScriptCore.JSContext _Engine = new JavaScriptCore.JSContext();
            public Task<string> Execute(string JavaScript) =>
                Task.Run(() => _Engine.EvaluateScript(JavaScript).ToString());
        }
#elif __ANDROID__
        {
            Android.Webkit.WebView _Engine = new Android.Webkit.WebView(Xamarin.Forms.Forms.Context);
            public Task<string> Execute(string JavaScript)
            {
                var CompletionSource = new TaskCompletionSource<string>();
                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
                    //https://stackoverflow.com/questions/19788294/how-does-evaluatejavascript-work
                    _Engine.EvaluateJavascript($"(function(){{return {JavaScript};}})()", new Callback(CompletionSource.SetResult));
                else throw new PlatformNotSupportedException("Not supported before Android 4.4 KitKat.");
                    //_Engine.LoadUrl($"javascript:(function(){{return {JavaScript};}});");
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
            public Task<string> Execute(string JavaScript) =>
                Task.Run(() => _Engine.Execute(JavaScript).GetCompletionValue().ToString());
        }
#endif
    }
}