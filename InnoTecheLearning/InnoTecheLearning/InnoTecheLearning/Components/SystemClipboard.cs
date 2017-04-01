﻿using System;
using System.Collections.Generic;
using System.Text;

namespace InnoTecheLearning
{
    partial class Utils
    {

#if __IOS__
        public static UIKit.UIPasteboard ClipBoard { get; } = UIKit.UIPasteboard.General;
#elif __ANDROID__
        public static Android.Content.ClipboardManager ClipBoard { get; } =
            (Android.Content.ClipboardManager)
            Xamarin.Forms.Forms.Context.GetSystemService(Android.Content.Context.ClipboardService);
#elif NETFX_CORE
        public static SystemClipboard Clipboard { get; } = new SystemClipboard();
        public sealed class SystemClipboard {
            public void Clear() => Windows​.ApplicationModel​.DataTransfer.Clipboard.Clear();
            public event EventHandler<object> ContentChanged {
                add => Windows​.ApplicationModel​.DataTransfer.Clipboard.ContentChanged += value;
                remove => Windows​.ApplicationModel​.DataTransfer.Clipboard.ContentChanged -= value;
            }
            public void Flush() => Windows​.ApplicationModel​.DataTransfer.Clipboard.Flush();
            public Windows​.ApplicationModel​.DataTransfer.DataPackageView GetContent() => 
                Windows​.ApplicationModel​.DataTransfer.Clipboard.GetContent();
            public void SetContent(Windows​.ApplicationModel​.DataTransfer.DataPackage content) =>
                Windows​.ApplicationModel​.DataTransfer.Clipboard.SetContent(content);
        }
#endif
    }
}