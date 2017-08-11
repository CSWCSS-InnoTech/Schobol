using System;
using System.Threading.Tasks;
using System.IO;
using static InnoTecheLearning.Utils;
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
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
        [Obsolete("Why not use async instead?")]
        void SaveLines(string FileName, string[] Lines);
        [Obsolete("Why not use async instead?")]
        string[] LoadLines(string FileName);
        ValueTask<Unit> SaveLinesAsync(string FileName, string[] Lines);
        ValueTask<string[]> LoadLinesAsync(string FileName);
        [Obsolete("Why not use async instead?")]
        void SaveText(string FileName, string Text);
        [Obsolete("Why not use async instead?")]
        string LoadText(string FileName);
        ValueTask<Unit> SaveTextAsync(string FileName, string Text);
        ValueTask<string> LoadTextAsync(string FileName);
        [Obsolete("Why not use async instead?")]
        void SaveBytes(string FileName, byte[] Bytes);
        [Obsolete("Why not use async instead?")]
        byte[] LoadBytes(string FileName);
        ValueTask<Unit> SaveBytesAsync(string FileName, byte[] Bytes);
        ValueTask<byte[]> LoadBytesAsync(string FileName);
        [Obsolete("Why not use async instead?")]
        void SaveStream(string FileName, Stream Stream);
        Stream LoadStream(string FileName);
        ValueTask<Unit> SaveStreamAsync(string FileName, Stream Stream);
        ValueTask<Stream> LoadStreamAsync(string FileName);
        [Obsolete("Why not use async instead?")]
        void Delete(string FileName);
        ValueTask<Unit> DeleteAsync(string FileName);
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
        [Obsolete("Why not use async instead?")]
        public void SaveStream(string FileName, Stream Stream)
        {
            SaveBytes(FileName, ToBytes(Stream));
        }
        [Obsolete("Why not use async instead?")]
        public Stream LoadStream(string FileName)
        {
            return FromBytes(LoadBytes(FileName));
        }
        public ValueTask<Unit> SaveStreamAsync(string FileName, Stream Stream)
        {
            return SaveBytesAsync(FileName, ToBytes(Stream));
        }
        public async ValueTask<Stream> LoadStreamAsync(string FileName)
        {
            return FromBytes(await LoadBytesAsync(FileName));
        }

#if true
        public string TempPath { get { return Path.GetTempPath(); } }
        public string TempFile { get { return Path.GetTempFileName(); } }
        [Obsolete("Why not use async instead?")]
        public void SaveLines(string FileName, string[] Lines)
        {
            var documentsPath = TempPath; //Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, FileName);
            File.WriteAllLines(filePath, Lines);
        }
        [Obsolete("Why not use async instead?")]
        public string[] LoadLines(string FileName)
        {
            var documentsPath = TempPath;//Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, FileName);
            return File.ReadAllLines(filePath);
        }
        public async ValueTask<Unit> SaveLinesAsync(string FileName, string[] Lines)
        {
            await Task.Run(() => {
                var documentsPath = TempPath; //Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, FileName);
                File.WriteAllLines(filePath, Lines);
            });
            return Unit.Default;
        }
        public async ValueTask<string[]> LoadLinesAsync(string FileName)
        {
            return await Task.Run(() => {
                var documentsPath = TempPath;//Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, FileName);
                return File.ReadAllLines(filePath);
            });
        }
        [Obsolete("Why not use async instead?")]
        public void SaveText(string FileName, string Text)
        {
            var documentsPath = TempPath; //Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, FileName);
            File.WriteAllText(filePath, Text);
        }
        [Obsolete("Why not use async instead?")]
        public string LoadText(string FileName)
        {
            var documentsPath = TempPath;//Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, FileName);
            return File.ReadAllText(filePath);
        }
        public async ValueTask<Unit> SaveTextAsync(string FileName, string Text)
        {
            await Task.Run( () => {
                var documentsPath = TempPath; //Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, FileName);
                File.WriteAllText(filePath, Text);
            });
            return Unit.Default;
        }
        public async ValueTask<string> LoadTextAsync(string FileName)
        {
           return await Task.Run( () => {
               var documentsPath = TempPath;//Environment.GetFolderPath(Environment.SpecialFolder.Personal);
               var filePath = Path.Combine(documentsPath, FileName);
               return File.ReadAllText(filePath);
           });
        }
        [Obsolete("Why not use async instead?")]
        public void SaveBytes(string FileName, byte[] Bytes)
        {
            var documentsPath = TempPath; //Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, FileName);
            File.WriteAllBytes(filePath, Bytes);
        }
        [Obsolete("Why not use async instead?")]
        public byte[] LoadBytes(string FileName)
        {
            var documentsPath = TempPath;//Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, FileName);
            return File.ReadAllBytes(filePath);
        }
        public async ValueTask<Unit> SaveBytesAsync(string FileName, byte[] Bytes)
        {
            await Task.Run(() => {
                var documentsPath = TempPath; //Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, FileName);
                File.WriteAllBytes(filePath, Bytes);
            });
            return Unit.Default;
        }
        public async ValueTask<byte[]> LoadBytesAsync(string FileName)
        {
            return await Task.Run(() => {
                var documentsPath = TempPath;//Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, FileName);
                return File.ReadAllBytes(filePath);
            });
        }
        [Obsolete("Why not use async instead?")]
        public void Delete(string FileName)
        {
            File.Delete(Path.Combine(TempPath, FileName));
        }
        public async ValueTask<Unit> DeleteAsync(string FileName)
        {
           await Task.Run(() => { File.Delete(Path.Combine(TempPath, FileName)); });
           return Unit.Default;
        }
#elif WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
        public string TempPath { get { return TempFolder.Path; } }
        StorageFolder TempFolder { get { return ApplicationData.Current.TemporaryFolder; } }
        public string TempFile { get
            { // generate unique filename
                var filename = Guid.NewGuid().ToString();
                IAsyncOperation<StorageFile> Task = TempFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                return Task.AsTask().Result.ToString();
            }
        }
        [Obsolete("Why not use async instead?")]
        public void SaveLines(string FileName, string[] Lines)
        {
            SaveLinesAsync(FileName, Lines).Do(); // HACK: to keep Interface return types simple (sorry!)
        }
        [Obsolete("Why not use async instead?")]
        public string[] LoadLines(string FileName)
        {
            return LoadLinesAsync(FileName).Do();// HACK: to keep Interface return types simple (sorry!)
        }
        public async ValueTask<Unit> SaveLinesAsync(string FileName, string[] Lines)
        {
            StorageFolder localFolder = TempFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteLinesAsync(sampleFile, Lines);
            return Unit.Default;
        }
        public async ValueTask<string[]> LoadLinesAsync(string FileName)
        {
            StorageFolder storageFolder = TempFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync(FileName);
            string[] Lines = (string[])await FileIO.ReadLinesAsync(sampleFile);
            return Lines;
        }
        [Obsolete("Why not use async instead?")]
        public void SaveText(string FileName, string Text)
        {
            SaveTextAsync(FileName, Text).Do();
            //Task Task = SaveTextAsync(FileName, Text);
            //Task.Wait(); // HACK: to keep Interface return types simple (sorry!)
        }
        [Obsolete("Why not use async instead?")]
        public string LoadText(string FileName)
        {
            return LoadTextAsync(FileName).Do();// HACK: to keep Interface return types simple (sorry!)
        }
        public async ValueTask<Unit> SaveTextAsync(string FileName, string Text)
        {
            StorageFolder localFolder = TempFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, Text);
            return Unit.Default;
        }
        public async ValueTask<string> LoadTextAsync(string FileName)
        {
            StorageFolder storageFolder = TempFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync(FileName);
            string Text = await FileIO.ReadTextAsync(sampleFile);
            return Text;
        }
        [Obsolete("Why not use async instead?")]
        public void SaveBytes(string FileName, byte[] Bytes)
        {
            SaveBytesAsync(FileName, Bytes).Do();// HACK: to keep Interface return types simple (sorry!)
        }
        [Obsolete("Why not use async instead?")]
        public byte[] LoadBytes(string FileName)
        {
            return LoadBytesAsync(FileName).Do();// HACK: to keep Interface return types simple (sorry!)
        }
        public async ValueTask<Unit> SaveBytesAsync(string FileName, byte[] Bytes)
        {
            StorageFolder localFolder = TempFolder;
            StorageFile sampleFile = await localFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteBytesAsync(sampleFile, Bytes);
            return Unit.Default;
        }
        public async ValueTask<byte[]> LoadBytesAsync(string FileName)
        {
            StorageFolder storageFolder = TempFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync(FileName);
            IBuffer buffer = await FileIO.ReadBufferAsync(sampleFile);
            return buffer.ToArray();
        }
        [Obsolete("Why not use async instead?")]
        public void Delete(string FileName)
        {
            DeleteAsync(FileName).Do();// HACK: to keep Interface return types simple (sorry!)
        }
        public async ValueTask<Unit> DeleteAsync(string FileName)
        {
            StorageFolder storageFolder = TempFolder;
            StorageFile sampleFile = await storageFolder.GetFileAsync(FileName);
            await sampleFile.DeleteAsync();
            return Unit.Default;
        }
#endif
    }
}
