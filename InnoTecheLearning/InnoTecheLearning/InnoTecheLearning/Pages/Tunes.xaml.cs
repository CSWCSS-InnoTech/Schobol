using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using static InnoTecheLearning.Utils.Create;
using Plugin.MediaManager;
using Plugin.MediaManager.Abstractions.Implementations;
using Plugin.MediaManager.Abstractions;
using Plugin.MediaManager.Abstractions.Enums;

namespace InnoTecheLearning.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Tunes : ContentPage
	{
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
            var AllButtons = new[] { ViolinG, ViolinD, ViolinA, ViolinE, CelloC, CelloG, CelloD, CelloA };
            foreach(var b in AllButtons)
            {
                b.Clicked += (sender, e) =>
                {
                    for (int j = 0; j < AllButtons.Length; j++) AllButtons[j].BackgroundColor = OriginalColor;
                    CrossMediaManager.Current.Play(new MediaFile());
                };
            }
        }
        class A : IMediaFile
        {
            public MediaFileType Type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public ResourceAvailability Availability { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public IMediaFileMetadata Metadata { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public string Url { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
            public bool MetadataExtracted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public event MetadataUpdatedEventHandler MetadataUpdated;
        }
    }
}