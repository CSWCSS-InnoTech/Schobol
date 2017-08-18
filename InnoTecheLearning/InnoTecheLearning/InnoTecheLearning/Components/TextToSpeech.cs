using System;
using Xamarin.Forms;
using InnoTecheLearning;
using System.Linq;
using System.Threading.Tasks;
#if __IOS__
using AVFoundation;
using Foundation;
using Speech;
using UIKit;
#elif __ANDROID__
using Android.OS;
using Android.Speech.Tts;
using static Android.App.AlertDialog;
using RecognizerIntent = Android.Speech.RecognizerIntent;
#elif NETFX_CORE
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;
using Windows.Media.SpeechRecognition;
#endif
[assembly: Dependency(typeof(Utils.TextToSpeechImplementation))]

namespace InnoTecheLearning
{
    partial class Utils
    {/// <summary>
     /// Cross-platform access to <see cref="TextToSpeechImplementation.Speak(string)"/>.
     /// </summary>
     /// <param name="Text"></param>
        public static void Speak(string Text) { DependencyService.Get<ITextToSpeech>().Speak(Text); }
        /// <summary>
        /// Provides an interface for <see cref="TextToSpeechImplementation"/> and cross-platform Text To Speech.
        /// </summary>
        public interface ITextToSpeech
        {
            void Speak(string text);
        }
        /// <summary>
        /// Provides an interface for <see cref="SpeechToText"/> and cross-platform Speech To Text.
        /// </summary>
        public interface ISpeechToText
        {
            ValueTask<Unit> Start();
            ValueTask<Unit> Stop();
            event EventHandler<VoiceRecognitionEventArgs> TextChanged;
            bool IsRecognizing { get; }
            string Text { get; }
            string Prompt { get; set; }
            SpeechLanguages Languages { get; set; }
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
        [Flags]
        public enum SpeechLanguages : byte
        {
            Unspecified = 0b0000000,
            Default = 0b0000001,
            System = 0b0000010,
            English_US = 0b0000100,
            English_UK = 0b0001000,
            Chinese_Simplified = 0b0010000,
            [Obsolete("Not universally supported.")] Chinese_Traditional = 0b0100000,
            [Obsolete("Not universally supported.")] Cantonese = 0b1000000
        }
        /// <summary>
        /// The platform-specific implementation of <see cref="ITextToSpeech"/>.
        /// </summary>
#if __IOS__
        public class TextToSpeechImplementation : ITextToSpeech
        {
            public TextToSpeechImplementation() { }

            public void Speak(string text) => Speak(text, SpeechLanguages.English_US);
            public void Speak(string text, SpeechLanguages lang)
            {
                var speechSynthesizer = new AVSpeechSynthesizer();

                var speechUtterance = new AVSpeechUtterance(text)
                {
                    Rate = AVSpeechUtterance.MaximumSpeechRate / 4,
                    //Voice = AVSpeechSynthesisVoice.FromLanguage(lang.ToLocale().LocaleIdentifier),
                    Volume = 0.5f,
                    PitchMultiplier = 1.0f
                };
                speechSynthesizer.SpeakUtterance(speechUtterance);
            }
        }
#elif __ANDROID__
        public class TextToSpeechImplementation : Java.Lang.Object, ITextToSpeech, TextToSpeech.IOnInitListener
        {
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
                    speaker.Speak(toSpeak, QueueMode.Flush, Droid.MainActivity.Bundle, ID);
                }
            }

            #region IOnInitListener implementation
            public void OnInit(OperationResult status)
            {
                if (status.Equals(OperationResult.Success))
                {
                    speaker.Speak(toSpeak, QueueMode.Flush, Droid.MainActivity.Bundle, ID);
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
        public class SpeechToText : ISpeechToText
        {
            public string Prompt { get; set; }
            public event EventHandler<VoiceRecognitionEventArgs> TextChanged;
            public string Text { get; private set; }
            public bool IsRecognizing { get => RecognitionTask?.State == SFSpeechRecognitionTaskState.Running; }
            public SpeechLanguages Languages { get; set; }
        #region Private Variables
            private AVAudioEngine AudioEngine = new AVAudioEngine();
            private SFSpeechRecognizer SpeechRecognizer = new SFSpeechRecognizer();
            private SFSpeechAudioBufferRecognitionRequest LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
            private SFSpeechRecognitionTask RecognitionTask;
        #endregion

            public SpeechToText(string Prompt = null, SpeechLanguages Langs = SpeechLanguages.Unspecified)
            {
                this.Prompt = Prompt;
                Languages = Langs;
            }
            void InitializeProperties()
            {
                AudioEngine = new AVAudioEngine();
                SpeechRecognizer = Languages == 
                    SpeechLanguages.Unspecified ? new SFSpeechRecognizer() : new SFSpeechRecognizer(Languages.ToLocale().First());
                LiveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
            }
            public ValueTask<Unit> Start() => Unit.InvokeAsync(AskPermission);
            public ValueTask<Unit> Stop() => Unit.InvokeAsync(CancelRecording);
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
                            var alert2 = UIAlertController.Create
                            ("Argh", "Fein", UIAlertControllerStyle.Alert);
                            alert2.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController
                            (alert2, animated: true, completionHandler: null);
                            break;
                        case SFSpeechRecognizerAuthorizationStatus.NotDetermined:
                            // Waiting on approval
                            AskPermission();
                            break;
                        case SFSpeechRecognizerAuthorizationStatus.Restricted:
                            // The device is not permitted
                            var alert = UIAlertController.Create
                            ("Whoops!", "You don't seem to have a microphone to record with", UIAlertControllerStyle.Alert);
                            alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController
                            (alert, animated: true, completionHandler: null);
                            break;
                    }
                });
            }
            void StartRecordingSession()
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
                    Log(ex).Ignore();
                }
            }

            void CheckAndStartReconition()
            {
                if (RecognitionTask?.State == SFSpeechRecognitionTaskState.Running)
                {
                    CancelRecording();
                }
                StartVoiceRecognition();
            }

            void StartVoiceRecognition()
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
                            Log(result.BestTranscription.FormattedString, "You said \"{0}\".");
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
                    Log(ex);
                }
            }

            void StopRecording()
            {
                try
                {
                    AudioEngine?.Stop();
                    LiveSpeechRequest?.EndAudio();
                }
                catch (Exception ex)
                {
                    Log(ex).Ignore();
                }
            }

            void CancelRecording()
            {
                try
                {
                    AudioEngine?.Stop();
                    RecognitionTask?.Cancel();
                }
                catch (Exception ex)
                {
                    Log(ex).Ignore();
                }
            }
            void OnTextChanged(string text, bool isFinal = false)
            {
                TextChanged?.Invoke(this, new VoiceRecognitionEventArgs(Text = text, isFinal));
            }
            ~SpeechToText()
            {
                AudioEngine.Dispose(); SpeechRecognizer.Dispose(); LiveSpeechRequest.Dispose(); RecognitionTask.Dispose();
            }
        }
#elif __ANDROID__
        public class SpeechToText : ISpeechToText
        {
            public string Prompt { get; set; }
            public SpeechLanguages Languages { get; set; }
            private const int VOICE = 10;
            public SpeechToText(string Prompt = null, SpeechLanguages Langs = SpeechLanguages.Unspecified)
            {
                this.Prompt = Prompt;
                Languages = Langs;
            }
            public event EventHandler<VoiceRecognitionEventArgs> TextChanged;
            public string Text { get; private set; }
            public bool IsRecognizing { get; private set; }
            void Alert(string Message, string Title = "Whoops!", string ButtonText = "OK")
            {
                var alert = new Builder(Forms.Context);
                alert.SetTitle(Title);
                alert.SetMessage(Message);
                alert.SetPositiveButton(ButtonText, (sender, e) =>
                {
                    return;
                });
                alert.Show().Show();
            }
            //Android.Content.Intent voiceIntent;
            public ValueTask<Unit> Start() => Start_();
            async ValueTask<Unit> Start_()
            {
                /**
                 * Asking the permission for installing Google Voice Search. 
                 * If permission granted – sent user to Google Play
                 * @param callerActivity – Activity, that initialized installing
                */
                void InstallGoogleVoiceSearch(Android.Content.Context context)
                {

                    // creating a dialog asking user if he want
                    // to install the Voice Search
                    new Builder(context)
                    .SetMessage("For recognition it’s necessary to install \"Google Voice Search\"")    // dialog message
                    .SetTitle("Install Voice Search from Google Play?")    // dialog header
                    .SetPositiveButton("Install",
                    // confirm button
                    // Install Button click handler
                    (sender, e) =>
                    {
                        try
                        {
                            // creating an Intent for opening applications page in Google Play
                            // Voice Search package name: com.google.android.voicesearch
                            var intent = new Android.Content.Intent(
                                Android.Content.Intent.ActionView,
                                Android.Net.Uri.Parse("market://details?id=com.google.android.voicesearch"));
                            // setting flags to avoid going in application history (Activity call stack)
                            intent.SetFlags(Android.Content.ActivityFlags.NoHistory | Android.Content.ActivityFlags.ClearWhenTaskReset);
                            // sending an Intent
                            context.StartActivity(intent);
                        }
                        catch (Exception)
                        {
                            // if something going wrong
                            // doing nothing
                        }
                    })
                    .SetNegativeButton("Cancel", delegate { })    // cancel button
                    .Create()
                    .Show();// showing dialog
                }
                try
                {
                    if (!Forms.Context.PackageManager.HasSystemFeature(Android.Content.PM.PackageManager.FeatureMicrophone))
                        // no microphone, no recording. Disable the button and output an alert
                        Alert("You don't seem to have a microphone to record with");
                    else if (Forms.Context.PackageManager.QueryIntentActivities(
                        new Android.Content.Intent(RecognizerIntent.ActionRecognizeSpeech), 0).Count == 0)
                        InstallGoogleVoiceSearch(Forms.Context);
                    else if (!await InternetAvaliable) Alert("You don't seem to have an Internet connection to analyze your speech");
                    else
                    {
                        // create the intent and start the activity
                        var voiceIntent = new Android.Content.Intent(RecognizerIntent.ActionRecognizeSpeech);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

                        // put a message on the modal dialog
                        voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now!");

                        /*
                        // if there is more then 1.5s of silence, consider the speech over
                        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

                        // you can specify other languages recognised here, for example
                        // voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.German);
                        // if you wish it to recognise the default Locale language and German
                        // if you do use another locale, regional dialects may not be recognised very well

                        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Us);
                        */
                        if (Languages != SpeechLanguages.Unspecified)
                        {
                            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Languages.ToLocale().First().ToLanguageTag());
                            /*voiceIntent.PutExtra("android.speech.extra.EXTRA_ADDITIONAL_LANGUAGES",
                                Languages.ToLocale().Select(loc => loc.ToLanguageTag()).ToArray());*/
                        }
                        IsRecognizing = true;
                        Droid.MainActivity.Current.StartActivityForResult(voiceIntent, VOICE, Droid.MainActivity.Bundle);
                        Droid.MainActivity.Current.ActivityResult += HandleActivityResult;
                        StopAction = () => Droid.MainActivity.Current.FinishActivity(VOICE);
                    }
                }
                catch (Exception ex)
                {
                    Log(ex).Ignore();
                }
                return Unit.Default;
            }
            private Action StopAction = delegate { };
            public ValueTask<Unit> Stop() => Unit.InvokeAsync(StopAction);
            private void HandleActivityResult(object sender, Android.Preferences.PreferenceManager.ActivityResultEventArgs e)
            {
                if (e.RequestCode == VOICE)
                {
                    IsRecognizing = false;
                    if (e.ResultCode == Android.App.Result.Ok && e.Data != null)
                    {
                        var matches = e.Data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                        if (matches.Count != 0)
                        {
                            // limit the output to 500 characters
                            // string textInput = matches[0]; if (textInput.Length > 500) textInput = textInput.Substring(0, 500);
                            TextChanged?.Invoke(sender, new VoiceRecognitionEventArgs(Text = matches[0], true));
                            // textBox.Text = textInput;
                        }
                        else
                        {
                            TextChanged?.Invoke(sender, new VoiceRecognitionEventArgs(Text = "", true));
                            // sharedButton.OnTextChanged?.Invoke("No speech was recognised");
                        }
                    }
                    StopAction = delegate { };
                }
            }

            ~SpeechToText() { Droid.MainActivity.Current.ActivityResult -= HandleActivityResult; StopAction(); }
        }/*
        public class LanguageDetailsChecker : Android.Content.BroadcastReceiver
        {

            private const string TAG = "LanguageDetailsChecker";

            public IList<String> supportedLanguages;

            public String languagePreference;

            public LanguageDetailsChecker()
            {
                supportedLanguages = new List<String>();
            }

            public override void OnReceive(Android.Content.Context context, Android.Content.Intent intent)
            {
                Bundle results = GetResultExtras(true);
                if (results.ContainsKey(RecognizerIntent.ExtraLanguagePreference))
                {
                    languagePreference = results
                            .GetString(RecognizerIntent.ExtraLanguagePreference);
                }
                if (results.ContainsKey(RecognizerIntent.ExtraSupportedLanguages))
                {
                    supportedLanguages = results
                            .GetStringArrayList(RecognizerIntent.ExtraSupportedLanguages);
                }
            }
        }*/
#elif NETFX_CORE
        public class SpeechToText : ISpeechToText
        {
            private SpeechRecognizerState[] ActiveStates = 
                new[] { SpeechRecognizerState.Capturing, SpeechRecognizerState.SoundStarted,
                    SpeechRecognizerState.SpeechDetected, SpeechRecognizerState.Paused };
            public string Prompt { get; set; }
            public SpeechLanguages Languages { get; set; }
            public bool IsRecognizing { get => ActiveStates.Contains(_speechRecognizer?.State ?? SpeechRecognizerState.Idle); }
            public SpeechToText(string Prompt = null, SpeechLanguages Langs = SpeechLanguages.Unspecified)
            {
                this.Prompt = Prompt;
                Languages = Langs;
            }
            public event EventHandler<VoiceRecognitionEventArgs> TextChanged;
            public string Text { get; private set; }
            SpeechRecognizer _speechRecognizer;
            //Windows.UI.Core.CoreDispatcher _coreDispatcher;
            public async ValueTask<Unit> Start()
            {
                async ValueTask<Unit> OpenSettings(string Title, string Content, string Uri)
                {
                    ContentDialog noWifiDialog = new ContentDialog()
                    {
                        Title = Title,
                        Content = Content,
                        PrimaryButtonText = "Shut up",
                        SecondaryButtonText = "Shut up and show me the setting"
                    };
                    if (await noWifiDialog.ShowAsync() == ContentDialogResult.Secondary)
                    {
                        //"http://stackoverflow.com/questions/42391526/exception-the-speech-privacy-policy-" + 
                        //"was-not-accepted-prior-to-attempting-a-spee/43083877#43083877";
                        var success = await Windows.System.Launcher.LaunchUriAsync(new Uri(Uri));

                        if (!success) await new ContentDialog
                        {
                            Title = "Oops! Something went wrong...",
                            Content = "The settings app could not be opened.",
                            PrimaryButtonText = "Shut your mouth up!"
                        }.ShowAsync();
                    }
                    return Unit.Default;
                }
                try
                {
                    _speechRecognizer = Languages ==
                        SpeechLanguages.Unspecified ? new SpeechRecognizer() : new SpeechRecognizer(Languages.ToLocale().First());
                    //_coreDispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;

                    /*var constraintList = new SpeechRecognitionListConstraint(new List<string>() { "Next", "Back" });
                    _speechRecognizer.Constraints.Add(constraintList);*/
                    var result = await _speechRecognizer.CompileConstraintsAsync();
                    if (result.Status != SpeechRecognitionResultStatus.Success) return Unit.Default;
                    _speechRecognizer.UIOptions.AudiblePrompt = Prompt ?? "Say something...";
                    _speechRecognizer.UIOptions.ExampleText = "For example: Lego Mania says \"Cheeseburger\"";
                    _speechRecognizer.UIOptions.IsReadBackEnabled = true;
                    _speechRecognizer.UIOptions.ShowConfirmation = true;
                    _speechRecognizer.HypothesisGenerated += ResultGenerated;
                    _speechRecognizer.StateChanged += Completed;
                    var Result = await _speechRecognizer.RecognizeWithUIAsync();
                }
                catch (System.Runtime.InteropServices.COMException e) when (e.HResult == unchecked((int)0x80045509))
                //privacyPolicyHResult
                //The speech privacy policy was not accepted prior to attempting a speech recognition.
                {
                    await OpenSettings("The speech privacy policy was not accepted",
                         "You need to turn on a button called 'Get to know me'...",
                         "ms-settings:privacy-speechtyping");
                }
                catch (System.Runtime.InteropServices.COMException e) when 
                (e.HResult == unchecked((int)0x8004503a) || e.HResult == unchecked((int)0x800455bc))
                //Corrosponding language pack is not installed.(SPERR_NOT_FOUND) || Language not supported.
                {
                    await OpenSettings($"The {Languages.ToLocale().First().DisplayName} speech service is not installed.",
                           "You need to install the package via 'Add a Language' > Select the language >" +
                           " Select the language again > 'Options' > 'Download' under 'Speech'...",
                           "ms-settings:regionlanguage");
                }
                return Unit.Default;
            }
            public ValueTask<Unit> Stop() => Unit.Await(_speechRecognizer.StopRecognitionAsync());

            private void Completed(SpeechRecognizer sender,
                SpeechRecognizerStateChangedEventArgs args) => 
            TextChanged?.Invoke(this, new VoiceRecognitionEventArgs(Text, true));

            private void ResultGenerated(SpeechRecognizer sender,
                SpeechRecognitionHypothesisGeneratedEventArgs args) => 
            TextChanged?.Invoke(this, new VoiceRecognitionEventArgs(Text = args.Hypothesis.Text, false));

            ~SpeechToText() { _speechRecognizer.Dispose(); }
        }
#endif
    }
}