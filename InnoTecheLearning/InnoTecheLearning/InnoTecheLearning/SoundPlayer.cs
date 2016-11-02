using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace InnoTecheLearning
{
    interface ISoundPlayer
    { }
    class SoundPlayer : ISoundPlayer
    {
#if __IOS__
#elif __ANDROID__
#elif NETFX_CORE
        MediaElement _mediaElement;
        public async Task Play(string FileName)
        {
            Windows.Storage.StorageFolder folder =
            await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(folderName);
            Windows.Storage.StorageFile file = await folder.GetFileAsync(fileName);
            Windows.Storage.Streams.IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

            _mediaElement = new MediaElement
            {
                IsMuted = false,
                Position = new TimeSpan(0, 0, 0),
                Volume = 1
            };
            _mediaElement.SetSource(stream, file.ContentType);

            await new Task(() =>
            {
                _mediaElement.Play();
            });
        }
#endif
    }
}
