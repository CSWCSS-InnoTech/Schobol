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
            }
            public const string VocabFile = "Vocabs.xml";
            public static readonly string SaveDirectory =
#if WINDOWS_UWP
                Windows.Storage.ApplicationData.Current.LocalFolder.Path
#else
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), AssemblyProduct)
#endif
                ;

            public static string GetSaveLocation(string FileName)
            {
                return Path.Combine(SaveDirectory, FileName);
            }

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
        }
    }
}