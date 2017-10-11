using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using static InnoTecheLearning.Utils.Create;
using Plugin.MediaManager;
using static Plugin.MediaManager.CrossMediaManager;
using Plugin.MediaManager.Abstractions.Enums;

namespace InnoTecheLearning.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Tunes : ContentPage
	{
        MediaManagerImplementation Player1 = new MediaManagerImplementation();
        MediaManagerImplementation Player2 = new MediaManagerImplementation();
        bool StopPlaying = false;
		public Tunes ()
		{
			InitializeComponent ();

            Violin.BackgroundColor = Color.Transparent;
            Violin.Source = ImageSource(ImageFile.Violin);
            Violin.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(_ => DisplayAlert("Violin", "🎻♫♬♩♪♬♩♪♬", "Beautiful"))
            });
            Cello.BackgroundColor = Color.Transparent;
            Cello.Source = ImageSource(ImageFile.Cello);
            Cello.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(_ => DisplayAlert("Cello", "🎻♫♬♩♪♬♩♪♬", "Beautiful"))
            });
            
            var OriginalColor = ViolinG.BackgroundColor;
            var PlayingColor = Color.Orange;
            var AllButtons = new[] { ViolinG, ViolinD, ViolinA, ViolinE, CelloC, CelloG, CelloD, CelloA };
            for(byte i = 0; i < AllButtons.Length; i++)
            {
                var k = i;
                AllButtons[k].Clicked += (sender, e) =>
                {
                    for (byte j = 0; j < AllButtons.Length; j++) AllButtons[j].BackgroundColor = OriginalColor;
                    AllButtons[k].BackgroundColor = PlayingColor;
        
                    Player1.Play(new Utils.SoundFile(Utils.Resources.GetStream($"Sounds.{Utils.GetFileName((Utils.Sounds)k)}")));
                    Task.Run(async () => 
                    {
                        await Task.Delay(500);
                        await Player2?.Play(new Utils.SoundFile(Utils.Resources.GetStream(
                            $"Sounds.{Utils.GetFileName((Utils.Sounds)k)}")));
                        Device.StartTimer(TimeSpan.FromSeconds(1), () => 
                        { Player2?.AudioPlayer.Seek(TimeSpan.Zero); return !StopPlaying; }); });
                    Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                        Player1?.AudioPlayer.Seek(TimeSpan.Zero); return !StopPlaying; });
                };
            }

            Stop.Clicked += (sender, e) =>
            {
                for (byte j = 0; j < AllButtons.Length; j++) AllButtons[j].BackgroundColor = OriginalColor;
                StopPlaying = true;
                Player1?.Stop();
                Player2?.Stop();
            };
            /*
            Volume.ValueChanged += (sender, e) =>
            {
                VolumeLabel.Text = Volume.Value.ToString().PadLeft(3) + "%";
                Current.VolumeManager.CurrentVolume = (float)(Volume.Value / 100);
            };*/
        }

        protected override void OnDisappearing()
        {
            try
            {
                Player1.Dispose();
                Player2.Dispose();
            }
            catch { }
            base.OnDisappearing();
        }
	}
}