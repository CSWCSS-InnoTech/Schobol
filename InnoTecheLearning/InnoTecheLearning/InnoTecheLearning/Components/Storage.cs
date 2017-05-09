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
            public const string VocabFile = "Vocabs.txt";

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
                using (var File = new FileStream(GetSaveLocation(FileName), FileMode.OpenOrCreate, FileAccess.Write))
                using (var Writer = new StreamWriter(File))
                { Writer.Write(Content); Writer.Flush(); }
            }

            public static void Write(string FileName, System.Collections.IEnumerable Content)
            {
                using (var File = new FileStream(GetSaveLocation(FileName), FileMode.OpenOrCreate, FileAccess.Write))
                using (var Writer = new StreamWriter(File))
                {
                    foreach (var Item in Content) Writer.Write(Item);
                    Writer.Flush();
                }
            }

            public static string Read(string FileName)
            {
                using (var File = new FileStream(GetSaveLocation(FileName), FileMode.Open, FileAccess.Read))
                using (var Reader = new StreamReader(File))
                    return Reader.ReadToEnd();
            }

            public static void SerializedWrite(string FileName, object Content)
            {
                var xs = new System.Xml.Serialization.XmlSerializer(Content.GetType());
                using (var File = new FileStream(GetSaveLocation(FileName), FileMode.OpenOrCreate, FileAccess.Write))
                { xs.Serialize(File, Content); File.Flush(); }
            }
            public static object SerializedRead(string FileName, Type ObjectType)
            {
                var xs = new System.Xml.Serialization.XmlSerializer(ObjectType);
                using (var File = new FileStream(GetSaveLocation(FileName), FileMode.Open, FileAccess.Read))
                    return xs.Deserialize(File);
            }
        }
    }
}