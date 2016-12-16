namespace System.Reflection
{
    using System;
    using System.Runtime.InteropServices;

    [ComVisible(true)]
    public class ManifestResourceInfo
    {
        private Assembly _containingAssembly;
        private string _containingFileName;
        private System.Reflection.ResourceLocation _resourceLocation;

        internal ManifestResourceInfo(Assembly containingAssembly, string containingFileName, System.Reflection.ResourceLocation resourceLocation)
        {
            this._containingAssembly = containingAssembly;
            this._containingFileName = containingFileName;
            this._resourceLocation = resourceLocation;
        }

        public virtual string FileName =>
            this._containingFileName;

        public virtual Assembly ReferencedAssembly =>
            this._containingAssembly;

        public virtual System.Reflection.ResourceLocation ResourceLocation =>
            this._resourceLocation;
    }
}

