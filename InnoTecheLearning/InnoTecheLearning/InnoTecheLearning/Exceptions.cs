using System;
using System.Collections.Generic;
using System.Text;

namespace InnoTecheLearning
{
    public static class Exceptions
    {

        public static void RegisterHandlers()
        {
#if WINDOWS_UWP
            Windows.UI.Xaml.Application.Current.UnhandledException += HandleExceptions;
#else
            AppDomain.CurrentDomain.UnhandledException += HandleExceptions;
#endif
        }
        public static void HandleExceptions(object sender,
#if WINDOWS_UWP
            Windows.UI.Xaml.UnhandledExceptionEventArgs
#else
            System.UnhandledExceptionEventArgs
#endif
            e)
        {
#if WINDOWS_UWP
            var ex = e.Exception;
#else
            var ex = (Exception)e.ExceptionObject;
#endif
            var d = DateTime.Now;
            var file = $"crash-{d.Year}-{d.Month}-{d.Day}_{d.Hour}.{d.Minute}.{d.Second}.txt";
            Utils.Storage.CreateSync(Utils.Storage.Combine(Utils.Storage.CrashDir, file));
            Utils.Storage.WriteSync(Utils.Storage.Combine(Utils.Storage.CrashDir, file), ex);
        }
    }
}
