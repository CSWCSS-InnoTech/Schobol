namespace System.Reflection
{
    using System;
    using System.Runtime.InteropServices;

    [ComVisible(true), AttributeUsage(AttributeTargets.Assembly, Inherited=false)]
    public sealed class AssemblyCultureAttribute : Attribute
    {
        private string m_culture;

        public AssemblyCultureAttribute(string culture)
        {
            this.m_culture = culture;
        }

        public string Culture =>
            this.m_culture;
    }
}

