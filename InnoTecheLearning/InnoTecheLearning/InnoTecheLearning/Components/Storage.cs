using System;
using System.IO;
using System.Threading.Tasks;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public static class Storage
        {
            static Storage()
            {
#if !WINDOWS_UWP
                if (!Directory.Exists(SaveDirectory)) Directory.CreateDirectory(SaveDirectory);
#endif
                if (!Directory.Exists(CrashDir)) Directory.CreateDirectory(GetSaveLocation(CrashDir));
            }
            public const string VocabFile = "Vocabs.xml";
            public const string CrashDir = "Crashes";
            public static readonly string SaveDirectory =
#if WINDOWS_UWP
                Windows.Storage.ApplicationData.Current.LocalFolder.Path
#else
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AssemblyProduct)
#endif
                ;

            public static string Combine(params string[] Paths) => Path.Combine(Paths);
            public static string GetSaveLocation(string FileName)
            {
                return Path.Combine(SaveDirectory, FileName);
            }
            public static string GetSaveLocation(params string[] FileFolders)
            {
                return Path.Combine(System.Linq.Enumerable.ToArray(FileFolders.Prepend(SaveDirectory)));
            }

            public static void CreateSync(string FileName) => File.Create(GetSaveLocation(FileName)).Dispose();
            public static void DeleteSync(string FileName) => File.Delete(GetSaveLocation(FileName));
            public static string ReadSync(string FileName) => File.ReadAllText(GetSaveLocation(FileName));
            public static void WriteSync(string FileName, object o) => File.WriteAllText(GetSaveLocation(FileName), o.ToString());
            public static ValueTask<Unit> Delete(string FileName) => Unit.InvokeAsync(() => File.Delete(GetSaveLocation(FileName)));

            public static async ValueTask<Unit> Write(string FileName, object Content)
            {
                using (var File = await GetWriteStream(FileName))
                using (var Writer = new StreamWriter(File))
                { Writer.Write(Content); Writer.Flush(); }
                return Unit.Default;
            }

            public static async ValueTask<Unit> Write(string FileName, System.Collections.IEnumerable Content)
            {
                using(var File = await GetWriteStream(FileName))
                using (var Writer = new StreamWriter(File))
                {
                    foreach (var Item in Content) Writer.Write(Item);
                    Writer.Flush();
                }
                return Unit.Default;
            }
            public static async ValueTask<string> Read(string FileName)
            {
                using (var File = await GetReadStream(FileName))
                using (var Reader = new StreamReader(File))
                    return Reader.ReadToEnd();
            }
            public static async ValueTask<string> ReadOrCreate(string FileName)
            {
                using (var File = await GetOrCreateReadStream(FileName))
                using (var Reader = new StreamReader(File))
                    return Reader.ReadToEnd();
            }

            public static async ValueTask<Unit> SerializedWrite(string FileName, object Content)
            {
                using (var File = await GetWriteStream(FileName))
                {
                    new System.Xml.Serialization.XmlSerializer(Content.GetType()).Serialize(File, Content);
                    File.Flush();
                }
                return Unit.Default;
            }
            public static async ValueTask<T> SerializedRead<T>(string FileName)
            {
                using (var File = await GetReadStream(FileName))
                    return (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(File);
            }
            public static async ValueTask<T> SerializedReadOrDefault<T>(string FileName, T Default = default(T))
            {
                try
                {
                    using (var File = await GetReadStream(FileName))
                        return (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(File);
                }
                catch (Exception)
                {
                    return Default;
                }
            }
            public static async ValueTask<T> SerializedReadOrCreate<T>(string FileName)
            {
                using (var File = await GetOrCreateReadStream(FileName))
                    return (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(File);
            }
            public static async ValueTask<T> SerializedReadOrCreateOrDefault<T>
                (string FileName, T Default = default(T))
            {
                try
                {
                    using (var File = await GetOrCreateReadStream(FileName))
                        return (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(File);
                }
                catch (Exception)
                {
                    return Default;
                }
            }

            public static async ValueTask<Stream> GetReadStream(string FileName) =>
#if WINDOWS_UWP
                await (await Windows.Storage.StorageFile.GetFileFromPathAsync(GetSaveLocation(FileName)))
                    .OpenStreamForReadAsync()
#else
                await new ValueTask<Stream>(() => new FileStream(GetSaveLocation(FileName), FileMode.Open, FileAccess.Read))
#endif
                ;
            public static async ValueTask<Stream> GetWriteStream(string FileName) =>
#if WINDOWS_UWP
                await (await Windows.Storage.StorageFile.GetFileFromPathAsync(GetSaveLocation(FileName)))
                    .OpenStreamForWriteAsync()
#else
                await new ValueTask<Stream>(() => new FileStream(GetSaveLocation(FileName), FileMode.Open, FileAccess.Write))
#endif
                ;

            public static async ValueTask<Stream> GetOrCreateReadStream(string FileName)
#if WINDOWS_UWP
            {
                try { return await GetReadStream(FileName); }
                catch (FileNotFoundException) { return await CreateReadStream(FileName); }
                catch (AggregateException ex) when (ex.InnerException is FileNotFoundException)
                { return await CreateReadStream(FileName); }
            }
#else
                => await new ValueTask<Stream>(() => new FileStream(GetSaveLocation(FileName), FileMode.OpenOrCreate, FileAccess.Read));
#endif


            public static async ValueTask<Stream> CreateReadStream(string FileName)
#if WINDOWS_UWP
            {
                await (await Windows.Storage.StorageFolder.GetFolderFromPathAsync(SaveDirectory))
                    .CreateFileAsync(FileName);
                return await GetReadStream(FileName);
            }
#else
                => await new ValueTask<Stream>(() => new FileStream(GetSaveLocation(FileName), FileMode.CreateNew, FileAccess.Read));
#endif

            /*
            public static Stream CreateReadStream(string FileName) =>
#if WINDOWS_UWP
                Windows.Storage.StorageFile.CreateStreamedFileFromUriAsync(
                    FileName, new Uri(GetSaveLocation(FileName)), 
                    Windows.Storage.Streams.RandomAccessStreamReference.CreateFromStream(
                        Create.ImageSource(Create.ImageFile.File_Icon).GetStream().AsRandomAccessStream()
                    )
                ).Do().OpenStreamForReadAsync().Do()
#else
                new FileStream(GetSaveLocation(FileName), FileMode.Open, FileAccess.Read)
#endif
                ;
            public static Stream CreateWriteStream(string FileName) =>
#if WINDOWS_UWP
                Windows.Storage.StorageFile.CreateStreamedFileFromUriAsync(
                    FileName, new Uri(GetSaveLocation(FileName)),
                    Windows.Storage.Streams.RandomAccessStreamReference.CreateFromStream(
                        Create.ImageSource(Create.ImageFile.File_Icon).GetStream().AsRandomAccessStream()
                    )
                ).Do().OpenStreamForWriteAsync().Do()
#else
                new FileStream(GetSaveLocation(FileName), FileMode.OpenOrCreate, FileAccess.Write)
#endif
                ;
*/
            public static bool Empty(string Directory) => new DirectoryInfo(GetSaveLocation(Directory)).GetFiles().Length == 0;
            public static bool Any(string Directory) => new DirectoryInfo(GetSaveLocation(Directory)).GetFiles().Length != 0;
            public static bool HasBefore(string Directory, string FileName) => Before(Directory, FileName) != null;
            public static bool HasAfter(string Directory, string FileName) => After(Directory, FileName) != null;
            public static string Before(string Directory, string FileName)
            {
                var Files = System.Linq.Enumerable.ToList(new DirectoryInfo(GetSaveLocation(Directory)).GetFiles());
                var Index = Files.FindIndex(f => f.Name == FileName);
                return Index == -1 || Index == 0 ? null : Files[Index - 1].Name;
            }
            public static string After(string Directory, string FileName)
            {
                var Files = System.Linq.Enumerable.ToList(new DirectoryInfo(GetSaveLocation(Directory)).GetFiles());
                var Index = Files.FindIndex(f => f.Name == FileName);
                return Index == -1 || Index == Files.Count - 1 ? null : Files[Index + 1].Name;
            }
            public static string First(string Directory)
            {
                var Files = new DirectoryInfo(GetSaveLocation(Directory)).GetFiles();
                return Files[0].Name;
            }
            public static string Last(string Directory)
            {
                var Files = new DirectoryInfo(GetSaveLocation(Directory)).GetFiles();
                return Files[Files.Length - 1].Name;
            }
        }
    }
}