namespace Microsoft.JScript
{
    using Microsoft.JScript.Vsa;
    using System;

    public sealed class Namespace
    {
        internal VsaEngine engine;
        private string name;

        private Namespace(string name, VsaEngine engine)
        {
            this.name = name;
            this.engine = engine;
        }

        public static Namespace GetNamespace(string name, VsaEngine engine) => 
            new Namespace(name, engine);

        internal Type GetType(string typeName) => 
            this.engine.GetType(typeName);

        internal string Name =>
            this.name;
    }
}

