using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public static class LongPress
        {
            const uint LongPressMilliseconds = 250;
            class Extensions
            {
                internal Extensions(EventHandler handler, EventHandler onPress, EventHandler onRelease)
                { Handler = handler; OnPress = onPress; OnRelease = onRelease; }
                internal EventHandler Handler;
                internal EventHandler OnPress;
                internal EventHandler OnRelease;
                internal Stopwatch Stopwatch = new Stopwatch();
            }
            static Dictionary<Button, Extensions> ElementExtensions =
                new Dictionary<Button, Extensions>();
            public static void Register(Button button, EventHandler eventHandler)
            {
                if (ElementExtensions.ContainsKey(button))
                {
                    ElementExtensions[button].Handler += eventHandler;
                }
                else
                {
                    ElementExtensions.Add(button, new Extensions(eventHandler, (sender, e) =>
                    {
                        ElementExtensions[button].Stopwatch.Restart();
                        Device.StartTimer(TimeSpan.FromMilliseconds(LongPressMilliseconds), () =>
                        {
                            if (ElementExtensions[button].Stopwatch.IsRunning &&
                                ElementExtensions[button].Stopwatch.ElapsedMilliseconds >= LongPressMilliseconds)
                            {
                                ElementExtensions[button].Stopwatch.Stop();
                                ElementExtensions[button].Handler(button, EventArgs.Empty);
                            }
                            return false;
                        });
                    }, (sender, e) => ElementExtensions[button].Stopwatch.Stop()));
                    button.Pressed += ElementExtensions[button].OnPress;
                    button.Released += ElementExtensions[button].OnRelease;
                }
            }
            public static void Unregister(Button button, EventHandler eventHandler)
            {
                if (ElementExtensions.ContainsKey(button))
                {
                    ElementExtensions[button].Handler -= eventHandler;
                    if (ElementExtensions[button].Handler == null) UnregisterAll(button);
                }
            }
            public static void UnregisterAll(Button button)
            {
                button.Pressed -= ElementExtensions[button].OnPress;
                button.Released -= ElementExtensions[button].OnRelease;
                ElementExtensions.Remove(button);
            }
        }
    }
}