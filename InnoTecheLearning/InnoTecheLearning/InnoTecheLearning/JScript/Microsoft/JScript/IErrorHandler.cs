namespace Microsoft.JScript
{
    using System;
    using System.Runtime.InteropServices;

    [Guid("E93D012C-56BB-4f32-864F-7C75EDA17B14"), ComVisible(true)]
    public interface IErrorHandler
    {
        bool OnCompilerError(IVsaFullErrorInfo error);
    }
}

