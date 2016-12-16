namespace System.Reflection
{
    using System;
    using System.Runtime.InteropServices;

    [AttributeUsage(AttributeTargets.Assembly, Inherited=false), ComVisible(true)]
    public sealed class AssemblyCompanyAttribute : Attribute
    {
        private string m_company;

        public AssemblyCompanyAttribute(string company)
        {
            this.m_company = company;
        }

        public string Company =>
            this.m_company;
    }
}

