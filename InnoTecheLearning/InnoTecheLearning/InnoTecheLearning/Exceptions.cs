using System;

namespace InnoTecheLearning
{
    public static class Exceptions
    {
#if __IOS__
        public delegate void NSUncaughtExceptionHandler(IntPtr exception);

        [System.Runtime.InteropServices.DllImport("/System/Library/Frameworks/Foundation.framework/Foundation")]
        private static extern void NSSetUncaughtExceptionHandler(IntPtr handler);

        class NSException : Foundation.NSException
        { public NSException(IntPtr handle) : base(handle) { } }
#endif
        public static void RegisterHandlers()
        {
#if WINDOWS_UWP
            Windows.UI.Xaml.Application.Current.UnhandledException += HandleExceptions;
#elif __ANDROID__
            Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += HandleExceptions;
#elif __IOS__
            NSSetUncaughtExceptionHandler(
                System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate
                (new NSUncaughtExceptionHandler(HandleExceptions)));
#else
            AppDomain.CurrentDomain.UnhandledException += HandleExceptions;
#endif
        }

#if __IOS__
        [ObjCRuntime.MonoPInvokeCallback(typeof(NSUncaughtExceptionHandler))]
#endif
        public static void HandleExceptions(
#if !__IOS__
            object sender,
#endif
#if WINDOWS_UWP
            Windows.UI.Xaml.UnhandledExceptionEventArgs
#elif __ANDROID__
            Android.Runtime.RaiseThrowableEventArgs
#elif __IOS__
            IntPtr
#else
            System.UnhandledExceptionEventArgs
#endif
            e)
        {
            var ex =
#if WINDOWS_UWP || __ANDROID__
            e.Exception
#elif __IOS__
            new NSException(e)
#else
            (Exception)e.ExceptionObject
#endif
                ;
            var d = DateTime.Now;
            var file = $"crash-{d.Year.ToString("D4")}-{d.Month.ToString("D2")}-{d.Day.ToString("D2")}_" +
                $"{d.Hour.ToString("D2")}.{d.Minute.ToString("D2")}.{d.Second.ToString("D2")}.txt";
            Utils.Log(ex);
            Utils.Storage.CreateSync(Utils.Storage.Combine(Utils.Storage.CrashDir, file));
            Utils.Storage.WriteSync(Utils.Storage.Combine(Utils.Storage.CrashDir, file), ex);
        }
    }
}
