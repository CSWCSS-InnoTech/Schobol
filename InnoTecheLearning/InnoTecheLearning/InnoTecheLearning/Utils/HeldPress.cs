using System;
using System.Collections.Generic;
using System.Threading;
using Xamarin.Forms;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public static class LongPress
        {
            const uint LongPressMilliseconds = 250;
            const uint TimesToCheck = 10;
            class Extensions
            {
                internal Extensions(EventHandler handler, EventHandler onPress, EventHandler onRelease)
                { Handler = handler; OnPress = onPress; OnRelease = onRelease; }
                internal EventHandler Handler;
                internal bool Pressed = false;
                internal EventHandler OnPress;
                internal EventHandler OnRelease;
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
                        ElementExtensions[button].Pressed = true;
                        uint Counter = 0;
                        Device.StartTimer(TimeSpan.FromMilliseconds(LongPressMilliseconds / TimesToCheck), () =>
                        {
                            if (ElementExtensions[button].Pressed && Counter == TimesToCheck)
                                ElementExtensions[button].Handler(button, EventArgs.Empty);
                            return ElementExtensions[button].Pressed && Counter++ < TimesToCheck;
                        });
                    }, (sender, e) => ElementExtensions[button].Pressed = false));
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