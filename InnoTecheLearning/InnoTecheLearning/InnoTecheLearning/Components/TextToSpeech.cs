using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using InnoTecheLearning;
using System.Linq;
#if __IOS__
using AVFoundation;
#elif __ANDROID__
using Android.OS;
using Android.Speech.Tts;
#elif NETFX_CORE
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;
#endif
[assembly: Dependency(typeof(TextToSpeechImplementation))]

namespace InnoTecheLearning
{   /// <summary>
    /// Cross-platform access to <see cref="TextToSpeechImplementation"/>.
    /// </summary>
    public static class Speech
    {   /// <summary>
        /// Cross-platform access to <see cref="TextToSpeechImplementation.Speak(string)"/>.
        /// </summary>
        /// <param name="Text"></param>
        public static void Speak(string Text)
        { DependencyService.Get<ITextToSpeech>().Speak(Text); }
    }
    /// <summary>
    /// Provides an interface for <see cref="TextToSpeechImplementation"/> and cross-platform <see cref="TextToSpeech"/>.
    /// </summary>
    public interface ITextToSpeech
    {
        void Speak(string text);
    }
    /// <summary>
    /// The platform-specific implementation of <see cref="ITextToSpeech"/>.
    /// </summary>
#if __IOS__
    public class TextToSpeechImplementation : ITextToSpeech
    {
        public TextToSpeechImplementation() { }

        public void Speak(string text)
        {
            var speechSynthesizer = new AVSpeechSynthesizer();

            var speechUtterance = new AVSpeechUtterance(text)
            {
                Rate = AVSpeechUtterance.MaximumSpeechRate / 4,
                Voice = AVSpeechSynthesisVoice.FromLanguage("en-US"),
                Volume = 0.5f,
                PitchMultiplier = 1.0f
            };

            speechSynthesizer.SpeakUtterance(speechUtterance);
        }
    }
#elif __ANDROID__
    public class TextToSpeechImplementation : Java.Lang.Object, ITextToSpeech, TextToSpeech.IOnInitListener
    {
        public static Bundle Bundle { get; set; }
        public const string ID = "utteranceId";
        TextToSpeech speaker;
        string toSpeak;

        public TextToSpeechImplementation() { }

        public void Speak(string text)
        {
            toSpeak = text;
            if (speaker == null)
            {
                speaker = new TextToSpeech(Forms.Context/* useful for many Android SDK features */, this);
            }
            else
            {
                speaker.Speak(toSpeak, QueueMode.Flush, Bundle, ID);
            }
        }

        #region IOnInitListener implementation
        public void OnInit(OperationResult status)
        {
            if (status.Equals(OperationResult.Success))
            {
                speaker.Speak(toSpeak, QueueMode.Flush, Bundle, ID);
            }
        }
        #endregion

      /*private List<string> AvaliableLanguages
        {  get{ var langAvailable = new List<string> { "Default" };
                var localesAvailable = Java.Util.Locale.GetAvailableLocales().ToList();
                foreach (var locale in localesAvailable)
                {
                    var res = speaker.IsLanguageAvailable(locale);
                    switch (res)
                    {
                        case LanguageAvailableResult.Available:
                            langAvailable.Add(locale.DisplayLanguage);
                            break;
                        case LanguageAvailableResult.CountryAvailable:
                            langAvailable.Add(locale.DisplayLanguage);
                            break;
                        case LanguageAvailableResult.CountryVarAvailable:
                            langAvailable.Add(locale.DisplayLanguage);
                            break;
                    }
                }
                return langAvailable.OrderBy(t => t).Distinct().ToList();}}*/
    }
#elif NETFX_CORE
    public class TextToSpeechImplementation : ITextToSpeech
    {
        public TextToSpeechImplementation() {}
    
        public async void Speak(string text)
        {
            var mediaElement = new MediaElement();
            var synth = new SpeechSynthesizer();
            var stream = await synth.SynthesizeTextToStreamAsync(text);

            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }
    }
#endif
}
