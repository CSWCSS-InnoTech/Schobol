using System.Threading.Tasks;
using System.IO;
using static InnoTecheLearning.Utils;
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
using System;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Foundation;
using System.Runtime.InteropServices.WindowsRuntime;
#endif
namespace InnoTecheLearning
{
    partial class Utils
    { public static TempIO Temp { get; } = new TempIO(); }
    public interface ITempIO
    {
        string TempPath { get; }
        string TempFile { get; }
        string GetFile(string FileName);
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
        void SaveStream(string FileName, Stream Stream);
        Stream LoadStream(string FileName);
        Task SaveStreamAsync(string FileName, Stream Stream);
        Task<Stream> LoadStreamAsync(string FileName);
        void Delete(string FileName);
        Task DeleteAsync(string FileName);
    }
    public class TempIO : ITempIO
    {
        protected internal TempIO(){ }
        public string GetFile(string FileName)
        { return Path.Combine(TempPath, FileName); }
        public byte[] ToBytes(Stream Input) // Designed for when Input.Length is untrustable
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = Input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public MemoryStream FromBytes(byte[] Bytes) // Enables MemoryStream.GetBuffer()
        {
            MemoryStream stream = new MemoryStream();
            stream.Write(Bytes, 0, Bytes.Length);
            return stream;
        }
        public void SaveStream(string FileName, Stream Stream)
        {
            SaveBytes(FileName, ToBytes(Stream));
        }
        public Stream LoadStream(string FileName)
        {
            return FromBytes(LoadBytes(FileName));
        }
        public async Task SaveStreamAsync(string FileName, Stream Stream)
        {
            await SaveBytesAsync(FileName, ToBytes(Stream));
        }
        public async Task<Stream> LoadStreamAsync(string FileName)
        {
            return FromBytes(await LoadBytesAsync(FileName));
        }

#if __IOS__ || __ANDROID__
        public string TempPath { get { return Path.GetTempPath(); } }
        public string TempFile { get { return Path.GetTempFileName(); } }
        public void SaveLines(string FileName, string[] Lines)
        {
            var documentsPath = TempPath; //Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, FileName);
            File.WriteAllLines(filePath, Lines);
        }
        public string[] LoadLines(string FileName)
        {
            var documentsPath = TempPath;//Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, FileName);
            return File.ReadAllLines(filePath);
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
            var filePath = Path.Combine(documentsPath, FileName);
            File.WriteAllText(filePath, Text);
        }
        public string LoadText(string FileName)
        {
            var documentsPath = TempPath;//Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, FileName);
            return File.ReadAllText(filePath);
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
            var filePath = Path.Combine(documentsPath, FileName);
            File.WriteAllBytes(filePath, Bytes);
        }
        public byte[] LoadBytes(string FileName)
        {
            var documentsPath = TempPath;//Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, FileName);
            return File.ReadAllBytes(filePath);
        }
        public async Task SaveBytesAsync(string FileName, byte[] Bytes)
        {
            await Task.Run(() => { SaveBytes(FileName, Bytes); });
        }
        public async Task<byte[]> LoadBytesAsync(string FileName)
        {
            return await Task.Run(() => { return LoadBytes(FileName); });
        }
        public void Delete(string FileName)
        {
            File.Delete(Path.Combine(TempPath, FileName));
        }
        public async Task DeleteAsync(string FileName)
        {
           await Task.Run(() => { Delete(FileName); });
        }
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
        public string TempPath { get { return TempFolder.Path; } }
        StorageFolder TempFolder { get { return ApplicationData.Current.TemporaryFolder; } }
        public string TempFile { get
            { // generate unique filename
                var filename = Guid.NewGuid().ToString();
                IAsyncOperation<StorageFile> Task = TempFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                return Task.AsTask().Result.ToString();
            } }
        public void SaveLines(string FileName, string[] Lines)
        {
            Do(SaveLinesAsync(FileName, Lines)); // HACK: to keep Interface return types simple (sorry!)
        }
        public string[] LoadLines(string FileName)
        {
            return Do(LoadLinesAsync(FileName));// HACK: to keep Interface return types simple (sorry!)
        }
        public async Task SaveLinesAsync(string FileName, string[] Lines)
        {
            StorageFolder localFolder = TempFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteLinesAsync(sampleFile, Lines);
        }
        public async Task<string[]> LoadLinesAsync(string FileName)
        {
            StorageFolder storageFolder = TempFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync(FileName);
            string[] Lines = (string[])await FileIO.ReadLinesAsync(sampleFile);
            return Lines;
        }
        public void SaveText(string FileName, string Text)
        {
            Do(SaveTextAsync(FileName, Text));
            //Task Task = SaveTextAsync(FileName, Text);
            //Task.Wait(); // HACK: to keep Interface return types simple (sorry!)
        }
        public string LoadText(string FileName)
        {
            return Do(LoadTextAsync(FileName));// HACK: to keep Interface return types simple (sorry!)
        }
        public async Task SaveTextAsync(string FileName, string Text)
        {
            StorageFolder localFolder = TempFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, Text);
        }
        public async Task<string> LoadTextAsync(string FileName)
        {
            StorageFolder storageFolder = TempFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync(FileName);
            string Text = await FileIO.ReadTextAsync(sampleFile);
            return Text;
        }
        public void SaveBytes(string FileName, byte[] Bytes)
        {
            Do(SaveBytesAsync(FileName, Bytes));// HACK: to keep Interface return types simple (sorry!)
        }
        public byte[] LoadBytes(string FileName)
        {
            return Do(LoadBytesAsync(FileName));// HACK: to keep Interface return types simple (sorry!)
        }
        public async Task SaveBytesAsync(string FileName, byte[] Bytes)
        {
            StorageFolder localFolder = TempFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(sampleFile, Bytes);
        }
        public async Task<byte[]> LoadBytesAsync(string FileName)
        {
            StorageFolder storageFolder = TempFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync(FileName);
            IBuffer buffer = await FileIO.ReadBufferAsync(sampleFile);
            return buffer.ToArray();
        }
        public void Delete(string FileName)
        {
            Do(DeleteAsync(FileName));// HACK: to keep Interface return types simple (sorry!)
        }
        public async Task DeleteAsync(string FileName)
        {
            StorageFolder storageFolder = TempFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync(FileName);
            await sampleFile.DeleteAsync();
        }
#endif
    }
}
