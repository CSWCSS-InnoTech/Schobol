using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using InnoTecheLearning;
using System.Linq;
#if __IOS__
using AVFoundation;
using Foundation;
using Speech;
#elif __ANDROID__
using Android.OS;
using Android.Speech.Tts;
using RecognizerIntent = Android.Speech.RecognizerIntent;
#elif NETFX_CORE
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;
#endif
[assembly: Dependency(typeof(Utils.TextToSpeechImplementation))]

namespace InnoTecheLearning
{
    partial class Utils
    {/// <summary>
     /// Cross-platform access to <see cref="TextToSpeechImplementation.Speak(string)"/>.
     /// </summary>
     /// <param name="Text"></param>
        public static void Speak(string Text)
        { DependencyService.Get<ITextToSpeech>().Speak(Text); }
        /// <summary>
        /// Provides an interface for <see cref="TextToSpeechImplementation"/> and cross-platform Text To Speech.
        /// </summary>
        public interface ITextToSpeech
        {
            void Speak(string text);
        }
        /// <summary>
        /// Provides an interface for <see cref="SpeechToTextImplementation"/> and cross-platform Speech To Text.
        /// </summary>
        public interface ISpeechToText
        {
            void Start();
            void Stop();
            event EventHandler<VoiceRecognitionEventArgs> TextChanged;
        }
        public class VoiceRecognitionEventArgs : EventArgs
        {
            public VoiceRecognitionEventArgs(string Text, bool IsFinal)
            {
                this.Text = Text;
                Final = IsFinal;
            }
            public string Text { get; }
            public bool Final { get; }
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
            public TextToSpeechImplementation() { }

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
        /// <summary>
        /// The platform-specific implementation of <see cref="ISpeechToText"/>.
        /// </summary>
#if __IOS__
        public class SpeechToTextImplementation : ISpeechToText
        {
            public event EventHandler<VoiceRecognitionEventArgs> TextChanged;
        #region Private Variables
            private AVAudioEngine AudioEngine = new AVAudioEngine();
            private SFSpeechRecognizer SpeechRecognizer = new SFSpeechRecognizer();
            private SFSpeechAudioBufferRecognitionRequest LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
            private SFSpeechRecognitionTask RecognitionTask;
        #endregion

            public SpeechToTextImplementation()
            {
            }
            public void InitializeProperties()
            {
                AudioEngine = new AVAudioEngine();
                SpeechRecognizer = new SFSpeechRecognizer();
                LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
            }
            public void Start()
            {
                AskPermission();
            }
            public void Stop()
            {
                CancelRecording();
            }
            void AskPermission()
            {
                // Request user authorization
                SFSpeechRecognizer.RequestAuthorization((SFSpeechRecognizerAuthorizationStatus status) => {
                    // Take action based on status
                    switch (status)
                    {
                        case SFSpeechRecognizerAuthorizationStatus.Authorized:
                            InitializeProperties();
                            StartRecordingSession();
                            break;
                        case SFSpeechRecognizerAuthorizationStatus.Denied:
                            // User has declined speech recognition
                            break;
                        case SFSpeechRecognizerAuthorizationStatus.NotDetermined:
                            // Waiting on approval
                            break;
                        case SFSpeechRecognizerAuthorizationStatus.Restricted:
                            // The device is not permitted
                            break;
                    }
                });
            }
            public void StartRecordingSession()
            {
                // Start recording
                AudioEngine.InputNode.InstallTapOnBus(
            bus: 0,
            bufferSize: 1024,
            format: AudioEngine.InputNode.GetBusOutputFormat(0),
            tapBlock: (buffer, when) => LiveSpeechRequest?.Append(buffer));
                AudioEngine.Prepare();
                AudioEngine.StartAndReturnError(out NSError error);
                // Did recording start?
                if (error != null)
                {
                    // Handle error and retur
                    return;
                }
                try
                {
                    CheckAndStartReconition();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public void CheckAndStartReconition()
            {
                if (RecognitionTask?.State == SFSpeechRecognitionTaskState.Running)
                {
                    CancelRecording();
                }
                StartVoiceRecognition();
            }

            public void StartVoiceRecognition()
            {
                try
                {
                    RecognitionTask = SpeechRecognizer.GetRecognitionTask(LiveSpeechRequest, 
                        (SFSpeechRecognitionResult result, NSError err) => {

                        if (result == null)
                        {
                            CancelRecording();
                            return;
                        }

                        // Was there an error?
                        if (err != null)
                        {
                            CancelRecording();
                            return;
                        }

                        //	 Is this the final translation?
                        if (result != null && result.BestTranscription != null && result.BestTranscription.FormattedString != null)
                        {
                            Console.WriteLine("You said \"{0}\".", result.BestTranscription.FormattedString);
                            OnTextChanged(result.BestTranscription.FormattedString);
                        }
                        if (result.Final)
                        {
                            OnTextChanged(result.BestTranscription.FormattedString, true);
                            CancelRecording();
                            return;
                        }
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public void StopRecording()
            {
                try
                {
                    AudioEngine?.Stop();
                    LiveSpeechRequest?.EndAudio();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public void CancelRecording()
            {
                try
                {
                    AudioEngine?.Stop();
                    RecognitionTask?.Cancel();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            public void OnTextChanged(string text, bool isFinal = false)
            {
                TextChanged?.Invoke(this, new VoiceRecognitionEventArgs(text, isFinal));
            }
        }
#elif __ANDROID__
        public class VoiceButtonRenderer : ISpeechToText
            {
            private bool isRecording;
            private readonly int VOICE = 10;
            public VoiceButtonRenderer()
            {
                isRecording = false;
            }
            public void Start()
            {
                try
                {
                    if (!Forms.Context.PackageManager.HasSystemFeature(Android.Content.PM.PackageManager.FeatureMicrophone))
                    {
                        // no microphone, no recording. Disable the button and output an alert
                        var alert = new Android.App.AlertDialog.Builder(Forms.Context);
                        alert.SetTitle("You don't seem to have a microphone to record with");
                        alert.SetPositiveButton("OK", (sender, e) => {
                            return;
                        });
                        alert.Show().Show();
                    }
                    else
                    {
                        // create the intent and start the activity
                        var voiceIntent = new Android.Content.Intent(RecognizerIntent.ActionRecognizeSpeech);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                        // put a message on the modal dialog
                        voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now!");
                        // if there is more then 1.5s of silence, consider the speech over
                        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                        // you can specify other languages recognised here, for example
                        // voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.German);
                        // if you wish it to recognise the default Locale language and German
                        // if you do use another locale, regional dialects may not be recognised very well
                        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                        Forms.Context.StartActivity(voiceIntent);//, VOICE
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            private void HandleActivityResult(object sender, Android.Preferences.PreferenceManager.ActivityResultEventArgs e)
            {
                if (e.RequestCode == VOICE)
                {
                    if (e.ResultCode == Android.App.Result.Ok)
                    {
                        var matches = e.Data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                        if (matches.Count != 0)
                        {
                            string textInput = matches[0];
                            // limit the output to 500 characters
                            if (textInput.Length > 500)
                                textInput = textInput.Substring(0, 500);
                            sharedButton.OnTextChanged?.Invoke(textInput);
                            //textBox.Text = textInput;
                        }
                        else
                            sharedButton.OnTextChanged?.Invoke("No speech was recognised");
                    }
                }
            }
        }
#elif NETFX_CORE
        public class SpeechToTextImplementation : ISpeechToText
        {
            public SpeechToTextImplementation() { }

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
}