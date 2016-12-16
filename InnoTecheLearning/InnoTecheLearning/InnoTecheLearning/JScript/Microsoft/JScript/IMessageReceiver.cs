namespace Microsoft.JScript
{
    using System;
    using System.Runtime.InteropServices;

    [Guid("F062C7FB-53BF-4f0d-B0F6-D66C5948E63F"), ComVisible(true)]
    public interface IMessageReceiver
    {
        void Message(string strValue);
    }
}

