#if WINDOWS_APP || WINDOWS_PHONE_APP
using System;
using System.Threading.Tasks;
using Windows.Storage;
#endif
namespace InnoTecheLearning
{
    class TempIO
    {
#if __IOS__ || __ANDROID__ || WINDOWS_UWP
        static string TempPath { get { return System.IO.Path.GetTempPath(); } }
        public void SaveText(string FileName, string Text)
        {
            var documentsPath = TempPath; //Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = System.IO.Path.Combine(documentsPath, FileName);
            System.IO.File.WriteAllText(filePath, Text);
        }
        public string LoadText(string FileName)
        {
            var documentsPath = TempPath;//Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = System.IO.Path.Combine(documentsPath, FileName);
            return System.IO.File.ReadAllText(filePath);
        }
#elif WINDOWS_APP || WINDOWS_PHONE_APP
        static StorageFolder TempPath { get { return ApplicationData.Current.TemporaryFolder; } }
        public async Task SaveText(string FileName, string Text)
        {
            StorageFolder localFolder = TempPath;
            StorageFile sampleFile = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, Text);
        }
        public async Task<string> LoadText(string FileName)
        {
            StorageFolder storageFolder = TempPath;
            StorageFile sampleFile = await storageFolder.GetFileAsync(FileName);
            string Text = await FileIO.ReadTextAsync(sampleFile);
            return Text;
        }
#endif
    }
}
