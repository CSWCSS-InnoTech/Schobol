using System.IO;
namespace InnoTecheLearnUtilities
{
    partial class Utils
    {
        public static ITempIO Temp { get; } = new TempIO();
        public interface ITempIO
        {
            string TempPath { get; }
            string TempFile { get; }
            string GetFile(string FileName);
            void SaveLines(string FileName, string[] Lines);
            string[] LoadLines(string FileName);
            void SaveText(string FileName, string Text);
            string LoadText(string FileName);
            void SaveBytes(string FileName, byte[] Bytes);
            byte[] LoadBytes(string FileName);
            void SaveStream(string FileName, Stream Stream);
            Stream LoadStream(string FileName);
            void Delete(string FileName);
        }
        public class TempIO : ITempIO
        {
            protected internal TempIO() { }
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

            public string TempPath { get { return Path.GetTempPath(); } }
            public string TempFile { get { return Path.GetTempFileName(); } }
            public void SaveLines(string FileName, string[] Lines)
            {
                var documentsPath = TempPath; 
                var filePath = Path.Combine(documentsPath, FileName);
                File.WriteAllLines(filePath, Lines);
            }
            public string[] LoadLines(string FileName)
            {
                var documentsPath = TempPath;
                var filePath = Path.Combine(documentsPath, FileName);
                return File.ReadAllLines(filePath);
            }
            public void SaveText(string FileName, string Text)
            {
                var documentsPath = TempPath; 
                var filePath = Path.Combine(documentsPath, FileName);
                File.WriteAllText(filePath, Text);
            }
            public string LoadText(string FileName)
            {
                var documentsPath = TempPath;
                var filePath = Path.Combine(documentsPath, FileName);
                return File.ReadAllText(filePath);
            }
            public void SaveBytes(string FileName, byte[] Bytes)
            {
                var documentsPath = TempPath; 
                var filePath = Path.Combine(documentsPath, FileName);
                File.WriteAllBytes(filePath, Bytes);
            }
            public byte[] LoadBytes(string FileName)
            {
                var documentsPath = TempPath;
                var filePath = Path.Combine(documentsPath, FileName);
                return File.ReadAllBytes(filePath);
            }
            public void Delete(string FileName)
            {
                File.Delete(Path.Combine(TempPath, FileName));
            }
        }
    }
}