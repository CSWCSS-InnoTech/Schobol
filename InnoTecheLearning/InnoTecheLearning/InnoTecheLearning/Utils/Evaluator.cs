#pragma warning disable 0618
#if __IOS__ || __ANDROID__ || WINDOWS_UWP
using Jint;
#elif WINDOWS_PHONE_APP || WINDOWS_APP
using ChakraHost.Hosting;
using System;
using System.Collections.ObjectModel;
#endif

namespace InnoTecheLearning
{
    partial class Utils
    {
#if __IOS__ || __ANDROID__ || WINDOWS_UWP
        public class Evaluator
        {
            public static string Eval(string CodeToExecute)
            { return new Engine().Execute(CodeToExecute).GetCompletionValue().ToString(); }
            public static T Eval<T>(string CodeToExecute)
            { return (T)(new Engine().Execute(CodeToExecute).GetCompletionValue().ToObject()); }
        }
#elif WINDOWS_PHONE_APP || WINDOWS_APP
        public static MainViewModel Evaluator { get; } = new MainViewModel();
        public class MainViewModel
        {
            private JavaScriptRuntime Runtime { get; }
            public MainViewModel()
            {
                Runtime = JavaScriptRuntime.Create();
            }

            public string Eval(string CodeToExecute)
            {
                using (new JavaScriptContext.Scope(Runtime.CreateContext()))
                {
                        DefineEcho();
                        var result = JavaScriptContext.RunScript(CodeToExecute);
                        //var numberResult = result.ConvertToNumber();
                        //var doubleResult = numberResult.ToDouble();
                        return result.ToString(); 
                }
            }
            private void DefineCallback
            (JavaScriptValue hostObject, string callbackName, JavaScriptNativeFunction callbackDelegate)
            {
                var propertyId = JavaScriptPropertyId.FromString(callbackName);

                var function = JavaScriptValue.CreateFunction(callbackDelegate);

                hostObject.SetProperty(propertyId, function, true);
            }
            string EchoOut = "";
            private JavaScriptValue Echo(JavaScriptValue callee,
 bool isConstructCall, JavaScriptValue[] arguments,
 ushort argumentCount, IntPtr callbackData)
            {
                for (uint index = 1; index < argumentCount; index++)
                {
                    EchoOut += arguments[index].ConvertToString().ToString();
                }

                return JavaScriptValue.True;
            }

            private JavaScriptNativeFunction EchoDelegate { get; set; }

            private void DefineEcho()
            {
                var globalObject = JavaScriptValue.GlobalObject;

                var hostObject = JavaScriptValue.CreateObject();
                var hostPropertyId = JavaScriptPropertyId.FromString("managedhost");
                globalObject.SetProperty(hostPropertyId, hostObject, true);
                EchoDelegate = Echo;
                DefineCallback(hostObject, "echo", EchoDelegate);
            }
        }
#endif

    }

}
#pragma warning restore 0618