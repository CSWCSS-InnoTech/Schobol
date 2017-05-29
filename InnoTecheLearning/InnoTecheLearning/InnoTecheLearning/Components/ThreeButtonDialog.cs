using System;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public static class ThreeButtonDialog
        {
            // Define your own ContentDialogResult enum
            public enum ThreeButtonDialogResult
            {
                Nothing,
                Yes,
                No,
                Cancel
            }

            public static void Show(string Title, string Message, string Button1, Action Button1Clicked,
                string Button2, Action Button2Clicked, string Button3, Action Button3Clicked)
            {
#if __IOS__
                var alert = UIKit.UIAlertController.Create(Title, Message, UIKit.UIAlertControllerStyle.Alert);
                alert.PreferredAction =
                    UIKit.UIAlertAction.Create(Button1, UIKit.UIAlertActionStyle.Default, _ => Button1Clicked());
                alert.AddAction(UIKit.UIAlertAction.Create(Button2, UIKit.UIAlertActionStyle.Default, _ => Button2Clicked()));
                alert.AddAction(UIKit.UIAlertAction.Create(Button3, UIKit.UIAlertActionStyle.Default, _ => Button3Clicked()));
                UIKit.UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController
                            (alert, animated: true, completionHandler: null);
#elif __ANDROID__
                new Android.App.AlertDialog.Builder(Xamarin.Forms.Forms.Context)
                    .SetTitle(Title)
                    .SetMessage(Message)
                    .SetPositiveButton(Button1, (_, __) => Button1Clicked())
                    .SetNeutralButton(Button2, (_, __) => Button2Clicked())
                    .SetNegativeButton(Button3, (_, __) => Button3Clicked())
                    .Create()
                    .Show();
#elif WINDOWS_UWP
                new UWP.ThreeButtonDialog(Title, Message, Button1, Button1Clicked, Button2, Button2Clicked, Button3, Button3Clicked)
                    .ShowAsync().Ignore();
#endif
            }
        }
    }
}