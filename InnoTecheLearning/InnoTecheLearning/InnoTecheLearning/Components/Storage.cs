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
            public const string VocabFile = "Vocabs.xml";

            public static string GetSaveLocation(string FileName) =>
#if WINDOWS_UWP
                    "ms-appdata:///local/" + FileName
#else
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    AssemblyProduct, FileName)
#endif
                ;

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
            public static void SerializedReadOrCreate<T>(string FileName, out T Content)
            {
                using (var File = GetOrCreateReadStream(FileName))
                    Content = (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(File);
            }

            public static Stream GetReadStream(string FileName) =>
#if WINDOWS_UWP
                Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri(GetSaveLocation(FileName))).Do()
                    .OpenStreamForReadAsync().Do()
#else
                new FileStream(GetSaveLocation(FileName), FileMode.Open, FileAccess.Read)
#endif
                ;
            public static Stream GetWriteStream(string FileName) =>
#if WINDOWS_UWP
                Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri(GetSaveLocation(FileName))).Do()
                    .OpenStreamForWriteAsync().Do()
#else
                new FileStream(GetSaveLocation(FileName), FileMode.OpenOrCreate, FileAccess.Write)
#endif
                ;

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

            public static Stream GetOrCreateReadStream(string FileName)
            {
                try { return GetReadStream(FileName); }
                catch (FileNotFoundException) { return CreateReadStream(FileName); }
                catch (AggregateException ex) when (ex.InnerException is FileNotFoundException)
                { return CreateReadStream(FileName); }
            }
        }
    }
}