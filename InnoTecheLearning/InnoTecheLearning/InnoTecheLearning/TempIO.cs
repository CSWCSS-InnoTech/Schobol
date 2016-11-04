using System.Threading.Tasks;
#if WINDOWS_APP || WINDOWS_PHONE_APP
using System;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
#endif
namespace InnoTecheLearning
{
    public interface ITempIO
    {
        void SaveLines(string FileName, string[] Lines);
        string[] LoadLines(string FileName);
        Task SaveLinesAsync(string FileName, string[] Lines);
        Task<string[]> LoadLinesAsync(string FileName);
        void SaveText(string FileName, string Text);
        string LoadText(string FileName);
        Task SaveTextAsync(string FileName, string Text);
        Task<string> LoadTextAsync(string FileName);
        void SaveBytes(string FileName, byte[] Bytes);
        byte[] LoadBytes(string FileName);
        Task SaveBytesAsync(string FileName, byte[] Bytes);
        Task<byte[]> LoadBytesAsync(string FileName);
    }
    class TempIO : ITempIO
    {
#if __IOS__ || __ANDROID__ || WINDOWS_UWP
        static string TempPath { get { return System.IO.Path.GetTempPath(); } }
        public void SaveLines(string FileName, string[] Lines)
        {
            var documentsPath = TempPath; //Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = System.IO.Path.Combine(documentsPath, FileName);
            System.IO.File.WriteAllLines(filePath, Lines);
        }
        public string[] LoadLines(string FileName)
        {
            var documentsPath = TempPath;//Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = System.IO.Path.Combine(documentsPath, FileName);
            return System.IO.File.ReadAllLines(filePath);
        }
        public async Task SaveLinesAsync(string FileName, string[] Lines)
        {
            await Task.Run(() => { SaveLines(FileName, Lines); });
        }
        public async Task<string[]> LoadLinesAsync(string FileName)
        {
            return await Task.Run(() => { return LoadLines(FileName); });
        }
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
        public async Task SaveTextAsync(string FileName, string Text)
        {
            await Task.Run( () => { SaveText(FileName, Text); });
        }
        public async Task<string> LoadTextAsync(string FileName)
        {
           return await Task.Run( () => {return LoadText(FileName); });
        }
        public void SaveBytes(string FileName, byte[] Bytes)
        {
            var documentsPath = TempPath; //Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = System.IO.Path.Combine(documentsPath, FileName);
            System.IO.File.WriteAllBytes(filePath, Bytes);
        }
        public byte[] LoadBytes(string FileName)
        {
            var documentsPath = TempPath;//Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = System.IO.Path.Combine(documentsPath, FileName);
            return System.IO.File.ReadAllBytes(filePath);
        }
        public async Task SaveBytesAsync(string FileName, byte[] Bytes)
        {
            await Task.Run(() => { SaveBytes(FileName, Bytes); });
        }
        public async Task<byte[]> LoadBytesAsync(string FileName)
        {
            return await Task.Run(() => { return LoadBytes(FileName); });
        }
#elif WINDOWS_APP || WINDOWS_PHONE_APP
        static StorageFolder TempPath { get { return ApplicationData.Current.TemporaryFolder; } }
        public void SaveLines(string FileName, string[] Lines)
        {
            Task Task = SaveLinesAsync(FileName, Lines);
            Task.Wait(); // HACK: to keep Interface return types simple (sorry!)
        }
        public string[] LoadLines(string FileName)
        {
            Task<string[]> Task = LoadLinesAsync(FileName);
            Task.Wait(); // HACK: to keep Interface return types simple (sorry!)
            return Task.Result;
        }
        public async Task SaveLinesAsync(string FileName, string[] Lines)
        {
            StorageFolder localFolder = TempPath;
            StorageFile sampleFile = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteLinesAsync(sampleFile, Lines);
        }
        public async Task<string[]> LoadLinesAsync(string FileName)
        {
            StorageFolder storageFolder = TempPath;
            StorageFile sampleFile = await storageFolder.GetFileAsync(FileName);
            string[] Lines = (string[])await FileIO.ReadLinesAsync(sampleFile);
            return Lines;
        }
        public void SaveText(string FileName, string Text)
        {
            Task Task = SaveTextAsync(FileName, Text);
            Task.Wait(); // HACK: to keep Interface return types simple (sorry!)
        }
        public string LoadText(string FileName)
        {
            Task<string> Task = LoadTextAsync(FileName);
            Task.Wait(); // HACK: to keep Interface return types simple (sorry!)
            return Task.Result;
        }
        public async Task SaveTextAsync(string FileName, string Text)
        {
            StorageFolder localFolder = TempPath;
            StorageFile sampleFile = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, Text);
        }
        public async Task<string> LoadTextAsync(string FileName)
        {
            StorageFolder storageFolder = TempPath;
            StorageFile sampleFile = await storageFolder.GetFileAsync(FileName);
            string Text = await FileIO.ReadTextAsync(sampleFile);
            return Text;
        }
        public void SaveBytes(string FileName, byte[] Bytes)
        {
            Task Task = SaveBytesAsync(FileName, Bytes);
            Task.Wait(); // HACK: to keep Interface return types simple (sorry!)
        }
        public byte[] LoadBytes(string FileName)
        {
            Task<byte[]> Task = LoadBytesAsync(FileName);
            Task.Wait(); // HACK: to keep Interface return types simple (sorry!)
            return Task.Result;
        }
        public async Task SaveBytesAsync(string FileName, byte[] Bytes)
        {
            StorageFolder localFolder = TempPath;
            StorageFile sampleFile = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(sampleFile, Bytes);
        }
        public async Task<byte[]> LoadBytesAsync(string FileName)
        {
            StorageFolder storageFolder = TempPath;
            StorageFile sampleFile = await storageFolder.GetFileAsync(FileName);
            IBuffer buffer = await FileIO.ReadBufferAsync(sampleFile);
            return buffer.ToArray();
        }
#endif
    }
}
