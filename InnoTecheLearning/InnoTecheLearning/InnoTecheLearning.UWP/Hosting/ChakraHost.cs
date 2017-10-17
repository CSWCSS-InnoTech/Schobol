using System;
using System.Collections;
using System.Runtime.InteropServices;
using ChakraHost.Hosting;

namespace ChakraHost
{
    public static class ChakraHost
    {
        //public static ChakraHost Current { get; } = new ChakraHost().Init();
        
        private static JavaScriptSourceContext currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
        private static JavaScriptRuntime runtime;
        private static Queue taskQueue = new Queue();
        private static JavaScriptContext context = JavaScriptContext.Invalid;
        private static readonly JavaScriptPromiseContinuationCallback promiseContinuationDelegate = PromiseContinuationCallback;

        static ChakraHost() => InnoTecheLearning.Utils.RunOnMainThread(() => Init());

        private static void PromiseContinuationCallback(JavaScriptValue task, IntPtr callbackState)
        {
            taskQueue.Enqueue(task);
            task.AddRef();
        }

        public static void Init()
        {
            Native.ThrowIfError(Native.JsCreateRuntime(JavaScriptRuntimeAttributes.None, null, out runtime), "failed to create runtime.");

            Native.ThrowIfError(Native.JsCreateContext(runtime, out context), "Failed to create execution context.");

            Native.ThrowIfError(Native.JsSetCurrentContext(context), "Failed to set current context.");

            Native.ThrowIfError(Native.JsSetPromiseContinuationCallback(promiseContinuationDelegate, IntPtr.Zero), "Failed to setup callback for ES6 Promise.");

            //Native.ThrowIfError(Native.JsProjectWinRTNamespace("Windows"), "Failed to project windows namespace.");

            //Native.ThrowIfError(Native.JsStartDebugging(), "Failed to start debugging.");
            //return this;
        }

        static JavaScriptContext GetContext() { Native.JsGetCurrentContext(out var x); return x; }
        public static System.Threading.Tasks.Task<string> RunScript(string script)
        {
            return InnoTecheLearning.Utils.RunOnMainThread(Internal);
            string Internal()
            {
                /*
                try
                {*/

                if (Native.JsRunScript(script, currentSourceContext++, "", out JavaScriptValue result) != JavaScriptErrorCode.NoError)
                {
                    // Get error message and clear exception
                    Native.ThrowIfError(Native.JsGetAndClearException(out JavaScriptValue exception), "Failed to get and clear exception.");

                    Native.ThrowIfError(Native.JsGetPropertyIdFromName("message", out JavaScriptPropertyId messageName), "Failed to get error message id.");

                    Native.ThrowIfError(Native.JsGetProperty(exception, messageName, out JavaScriptValue messageValue), "Failed to get error message.");

                    Native.ThrowIfError(Native.JsStringToPointer(messageValue, out IntPtr message, out UIntPtr length), "Failed to convert error message.");

                    return Marshal.PtrToStringUni(message);
                }

                // Execute promise tasks stored in taskQueue 
                while (taskQueue.Count != 0)
                {
                    JavaScriptValue task = (JavaScriptValue)taskQueue.Dequeue();
                    Native.JsGetGlobalObject(out JavaScriptValue global);
                    JavaScriptValue[] args = new JavaScriptValue[1] { global };
                    Native.JsCallFunction(task, args, 1, out JavaScriptValue promiseResult);
                    task.Release();
                }

                // Convert the return value.
                Native.ThrowIfError(Native.JsConvertValueToString(result, out JavaScriptValue stringResult), "Failed to convert value to string.");
                Native.ThrowIfError(Native.JsStringToPointer(stringResult, out IntPtr returnValue, out UIntPtr stringLength), "Failed to convert return value.");
                /*}
                catch (Exception e)
                {
                    return "chakrahost: fatal error: internal error: " + e.Message;
                }*/

                return Marshal.PtrToStringUni(returnValue);
            }
        }
    }
}