namespace System.Reflection.Emit
{
    using System;
    using System.Globalization;
    using System.IO;

    [Serializable]
    internal class ModuleBuilderData
    {
        [NonSerialized]
        internal ResWriterData m_embeddedRes;
        internal bool m_fGlobalBeenCreated;
        internal bool m_fHasGlobal;
        [NonSerialized]
        internal TypeBuilder m_globalTypeBuilder;
        internal bool m_isSaved;
        internal bool m_isTransient;
        [NonSerialized]
        internal ModuleBuilder m_module;
        internal byte[] m_resourceBytes;
        internal string m_strFileName;
        internal string m_strModuleName;
        internal string m_strResourceFileName;
        internal int m_tkFile;
        internal const string MULTI_BYTE_VALUE_CLASS = "$ArrayType$";

        internal ModuleBuilderData(ModuleBuilder module, string strModuleName, string strFileName)
        {
            this.Init(module, strModuleName, strFileName);
        }

        internal virtual void Init(ModuleBuilder module, string strModuleName, string strFileName)
        {
            this.m_fGlobalBeenCreated = false;
            this.m_fHasGlobal = false;
            this.m_globalTypeBuilder = new TypeBuilder(module);
            this.m_module = module;
            this.m_strModuleName = strModuleName;
            this.m_tkFile = 0;
            this.m_isSaved = false;
            this.m_embeddedRes = null;
            this.m_strResourceFileName = null;
            this.m_resourceBytes = null;
            if (strFileName == null)
            {
                this.m_strFileName = strModuleName;
                this.m_isTransient = true;
            }
            else
            {
                switch (Path.GetExtension(strFileName))
                {
                    case null:
                    case string.Empty:
                        throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_NoModuleFileExtension"), new object[] { strFileName }));
                }
                this.m_strFileName = strFileName;
                this.m_isTransient = false;
            }
            this.m_module.InternalSetModuleProps(this.m_strModuleName);
        }

        internal virtual bool IsTransient() => 
            this.m_isTransient;
    }
}

