#pragma warning disable 0618
using Microsoft.JScript;
using Microsoft.JScript.Vsa;
namespace InnoTecheLearning
{
    partial class Utils
    {
        public class Evaluator : INeedEngine
        {

            // Methods
            public Evaluator() { init(); }
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
                    vsaEngine = VsaEngine.CreateEngineWithType(typeof(Evaluator).TypeHandle);
                }
                return vsaEngine;
            }
            public void SetEngine(VsaEngine engine1)
            {
                vsaEngine = engine1;
            }
            private VsaEngine vsaEngine { get; set; }
        }

    }

}
#pragma warning restore 0618