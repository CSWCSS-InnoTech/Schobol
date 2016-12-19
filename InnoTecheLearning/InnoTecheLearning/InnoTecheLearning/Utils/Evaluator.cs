#pragma warning disable 0618
#if __IOS__ || __ANDROID__
using Microsoft.JScript;
using Microsoft.JScript.Vsa;
#elif NETFX_CORE
using ChakraHost.Hosting;
using System;
using System.Collections.ObjectModel;
#endif

namespace InnoTecheLearning
{
    partial class Utils
    {
#if __IOS__ || __ANDROID__
        public class JSEvaluator : INeedEngine
        {

            // Methods
            public JSEvaluator() { init(); }
            private void init() { }
            [JSFunction(JSFunctionAttributeEnum.HasStackFrame)]
            public virtual string Eval(string expr)
            {
                string str = default(string);
                JSLocalField[] fields = new JSLocalField[] { new JSLocalField("expr", typeof(string).TypeHandle, 0), new JSLocalField("return value", typeof(string).TypeHandle, 1) };
                StackFrame.PushStackFrameForMethod(this, fields, ((INeedEngine)this).GetEngine());
                try
                {
                    ((StackFrame)GetEngine().ScriptObjectStackTop()).localVars[0] = expr;
                    ((StackFrame)GetEngine().ScriptObjectStackTop()).localVars[1] = str;
                    expr = Convert.ToString(((StackFrame)GetEngine().ScriptObjectStackTop()).localVars[0], true);
                    str = Convert.ToString(((StackFrame)GetEngine().ScriptObjectStackTop()).localVars[1], true);
                    str = Convert.ToString(Microsoft.JScript.Eval.JScriptEvaluate(expr, GetEngine()), true);
                    ((StackFrame)GetEngine().ScriptObjectStackTop()).localVars[0] = expr;
                    ((StackFrame)GetEngine().ScriptObjectStackTop()).localVars[1] = str;
                }
                finally
                {
                    ((INeedEngine)this).GetEngine().PopScriptObject();
                }
                return str;
            }
            public VsaEngine GetEngine()
            {
                if (vsaEngine == null)
                {
                    vsaEngine = VsaEngine.CreateEngineWithType(typeof(JSEvaluator).TypeHandle);
                }
                return vsaEngine;
            }
            public void SetEngine(VsaEngine engine1)
            {
                vsaEngine = engine1;
            }
            private VsaEngine vsaEngine { get; set; }
        }
#elif NETFX_CORE
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