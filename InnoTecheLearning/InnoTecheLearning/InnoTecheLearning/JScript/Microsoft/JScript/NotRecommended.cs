namespace Microsoft.JScript
{
    using System;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public class NotRecommended : Attribute
    {
        private string message;

        public NotRecommended(string message)
        {
            this.message = message;
        }

        public bool IsError =>
            false;

        public string Message =>
            JScriptException.Localize(this.message, null);
    }
}

