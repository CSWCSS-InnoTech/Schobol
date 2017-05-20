using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace InnoTecheLearning
{
    partial class Utils
    {   public static TextLog Logger { get; } = new TextLog(Temp.GetFile("InnoTecheLearning.log"));
        public static ValueTask<string> ReadAll()
        { return Logger.ReadAll(); }//""; }
        public static ValueTask<string> Log(string Message)
        { return Logger.Log(Message); }//""; }
        public static ValueTask<string> Log(string Message, LogImportance Importance)
        { return Logger.Log(Message, Importance); }//""; }
        public static ValueTask<string> Log(Exception e)
        { return Logger.Log(e); }//""; }
        public static ValueTask<string> Log(Exception e, LogImportance Importance)
        { return Logger.Log(e, Importance); }//""; }
        public static string Region
        { get { return Logger.Region; } set { Logger.Region = value; } }//""; 
        public enum LogImportance : byte
        {
            /// <summary>
            /// The message is critical.
            /// </summary>
            C,
            /// <summary>
            /// The message is erring.
            /// </summary>
            E,
            /// <summary>
            /// The message is warning.
            /// </summary>
            W,
            /// <summary>
            /// The message is informational.
            /// </summary>
            I,
            /// <summary>
            /// The message is verbose.
            /// </summary>
            V
        }
        public static char Symbol(LogImportance Importance)
        {
            switch (Importance)
            {
                case LogImportance.C:
                    return '❗';
                case LogImportance.E:
                    return 'ⓧ'; //⮾
                case LogImportance.W:
                    return '⚠';
                case LogImportance.I:
                    return 'ⓘ';
                case LogImportance.V:
                    return '#';
                default:
                    throw new ArgumentOutOfRangeException("Importance", Importance, "Importance is out of range.");
            }
        }
        public interface ITextLog
        {
            ValueTask<string> ReadAll();
            ValueTask<string> Log(string Message);
            ValueTask<string> Log(string Message, LogImportance Importance);
            ValueTask<string> Log(Exception e);
            ValueTask<string> Log(Exception e, LogImportance Importance);
            string Format(DateTime Time, LogImportance Importance, string Region, string Message);
        }
        /// <summary>
        /// For app logging.
        /// </summary>
        public class TextLog : ITextLog
        {
            public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.FFF";
            string Path;
            public string Region { get; set; }
            public async void Init(string Path)
            {
 #if __IOS__|| __ANDROID__
                if (!File.Exists(Path)) File.Create(Path).Dispose();
#elif NETFX_CORE
                Path = Path.Replace('/', '\\');
                try
                {
                global::Windows.Storage.StorageFile File = await global::Windows.Storage.StorageFile.GetFileFromPathAsync(Path);
                }
                catch (Exception)
                {
                    await (await global::Windows.Storage.StorageFolder.GetFolderFromPathAsync(System.IO.Path.GetDirectoryName(Path)))
                        .CreateFileAsync(System.IO.Path.GetFileName(Path));  
                }
#endif
                this.Path = Path;
            }
            public TextLog(string Path) { Init(Path); }
            public TextLog(string Path, string Region) : this(Path) { this.Region = Region; }
            public ValueTask<string> Log(string Message)
            { return Log(Message, LogImportance.I); }
            public ValueTask<string> Log(Exception e)
            { return Log(e, LogImportance.E); }
            public ValueTask<string> Log(Exception e, LogImportance Importance)
            { return Log(e.ToString(), Importance); }
            public async ValueTask<string> Log(string Message, LogImportance Importance)
            {
#if __IOS__|| __ANDROID__
                using (StreamWriter Writer = new StreamWriter(Path, true, Encoding.Unicode))
                { Writer.WriteLine(Format(DateTime.Now, Importance, Region, Message)); Writer.Flush(); }
#elif NETFX_CORE
                global::Windows.Storage.StorageFile File = await global::Windows.Storage.StorageFile.GetFileFromPathAsync(Path);
                await global::Windows.Storage.FileIO.AppendTextAsync(File, Format(DateTime.Now, Importance, Region, Message),
                    global::Windows.Storage.Streams.UnicodeEncoding.Utf16LE);
#endif
                return Message;
            }
            public async ValueTask<string> ReadAll()
            {
#if __IOS__ || __ANDROID__
                using (StreamReader Reader = new StreamReader(Path, Encoding.Unicode))
#elif NETFX_CORE
                global::Windows.Storage.StorageFile File = await global::Windows.Storage.StorageFile.GetFileFromPathAsync(Path);
                using (Stream Stream = await File.OpenStreamForReadAsync())
                using (StreamReader Reader = new StreamReader(Stream, Encoding.Unicode))
#endif
                return Reader.ReadToEnd(); 
            }
            public string Format(DateTime Time, LogImportance Importance, string Region, string Message)
            { return '[' + Time.ToString(DateTimeFormat) + ']' + Symbol(Importance) + Region + '|' + Message; }
        }
    }
}