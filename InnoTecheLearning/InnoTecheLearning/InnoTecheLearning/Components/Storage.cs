using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

            public static void Write(string FileName, object Content)
            {
                using (var File = GetWriteStream(FileName))
                using (var Writer = new StreamWriter(File))
                { Writer.Write(Content); Writer.Flush(); }
            }

            public static void Write(string FileName, System.Collections.IEnumerable Content)
            {
                using(var File = GetWriteStream(FileName))
                using (var Writer = new StreamWriter(File))
                {
                    foreach (var Item in Content) Writer.Write(Item);
                    Writer.Flush();
                }
            }
            public static string Read(string FileName)
            {
                using (var File = GetReadStream(FileName))
                using (var Reader = new StreamReader(File))
                    return Reader.ReadToEnd();
            }
            public static string ReadOrCreate(string FileName)
            {
                using (var File = GetOrCreateReadStream(FileName))
                using (var Reader = new StreamReader(File))
                    return Reader.ReadToEnd();
            }

            public static void SerializedWrite(string FileName, object Content)
            {
                using (var File = GetWriteStream(FileName))
                {
                    new System.Xml.Serialization.XmlSerializer(Content.GetType()).Serialize(File, Content);
                    File.Flush();
                }
            }
            public static void SerializedRead<T>(string FileName, out T Content)
            {
                using (var File = GetReadStream(FileName))
                    Content = (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(File);
            }
            public static void SerializedReadOrDefault<T>(string FileName, out T Content, T Default = default(T))
            {
                try
                {
                    using (var File = GetReadStream(FileName))
                        Content = (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(File);
                }
                catch (Exception)
                {
                    Content = Default;
                }

            }
            public static void SerializedReadOrCreate<T>(string FileName, out T Content)
            {
                using (var File = GetOrCreateReadStream(FileName))
                    Content = (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(File);
            }
            public static void SerializedReadOrCreateOrDefault<T>
                (string FileName, out T Content, T Default = default(T))
            {
                try
                {
                    using (var File = GetOrCreateReadStream(FileName))
                        Content = (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(File);
                }
                catch (Exception)
                {
                    Content = Default;
                }
            }

            public static Stream GetReadStream(string FileName) =>
#if WINDOWS_UWP
                Windows.Storage.StorageFile.GetFileFromPathAsync(GetSaveLocation(FileName)).Do()
                    .OpenStreamForReadAsync().Do()
#else
                new FileStream(GetSaveLocation(FileName), FileMode.Open, FileAccess.Read)
#endif
                ;
            public static Stream GetWriteStream(string FileName) =>
#if WINDOWS_UWP
                Windows.Storage.StorageFile.GetFileFromPathAsync(GetSaveLocation(FileName)).Do()
                    .OpenStreamForWriteAsync().Do()
#else
                new FileStream(GetSaveLocation(FileName), FileMode.Open, FileAccess.Write)
#endif
                ;

            public static Stream GetOrCreateReadStream(string FileName)
#if WINDOWS_UWP
            {
                try { return GetReadStream(FileName); }
                catch (FileNotFoundException) { return CreateReadStream(FileName); }
                catch (AggregateException ex) when (ex.InnerException is FileNotFoundException)
                { return CreateReadStream(FileName); }
            }
#else
                => new FileStream(GetSaveLocation(FileName), FileMode.OpenOrCreate, FileAccess.Read);
#endif


            public static Stream CreateReadStream(string FileName)
#if WINDOWS_UWP
            {
                Windows.Storage.StorageFolder.GetFolderFromPathAsync(SaveDirectory).Do()
                    .CreateFileAsync(FileName).Do();
                return GetReadStream(FileName);
            }
#else
                => new FileStream(GetSaveLocation(FileName), FileMode.CreateNew, FileAccess.Read);
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