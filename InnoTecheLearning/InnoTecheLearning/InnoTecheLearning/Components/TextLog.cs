using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace InnoTecheLearning
{
    partial class Utils
    {   public static TextLog Logger { get; } = new TextLog(Temp.GetFile("InnoTecheLearning.log"));
        public static string ReadAll()
        { return Logger.ReadAll(); }
        public static string Log(string Message)
        { return Logger.Log(Message); }
        public static string Log(string Message, LogImportance Importance)
        { return Logger.Log(Message, Importance); }
        public static string Log(Exception e)
        { return Logger.Log(e); }
        public static string Log(Exception e, LogImportance Importance)
        { return Logger.Log(e, Importance); }
        public static string Region
        { get { return Logger.Region; } set { Logger.Region = value; } }
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
                    return '⮾';
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
            string ReadAll();
            string Log(string Message);
            string Log(string Message, LogImportance Importance);
            string Log(Exception e);
            string Log(Exception e, LogImportance Importance);
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
            public TextLog(string Path) { this.Path = Path; }
            public TextLog(string Path, string Region) { this.Path = Path; this.Region = Region; }
            public string Log(string Message)
            { return Log(Message, LogImportance.I); }
            public string Log(Exception e)
            { return Log(e, LogImportance.I); }
            public string Log(Exception e, LogImportance Importance)
            { return Log(e.ToString(), Importance); }
            public string Log(string Message, LogImportance Importance)
            {
#if __IOS__|| __ANDROID__
                using (StreamWriter Writer = new StreamWriter(Path, true, Encoding.Unicode))
                { Writer.WriteLine(Format(DateTime.Now, Importance, Region, Message)); Writer.Flush(); }
#elif NETFX_CORE
                global::Windows.Storage.StorageFile File = Do(global::Windows.Storage.StorageFile.GetFileFromPathAsync(Path));
                Do(global::Windows.Storage.FileIO.AppendTextAsync(File, Format(DateTime.Now, Importance, Region, Message),
                    global::Windows.Storage.Streams.UnicodeEncoding.Utf16LE));
#endif
                return Message;
            }
            public string ReadAll()
            {
#if __IOS__ || __ANDROID__
                using (StreamReader Reader = new StreamReader(Path, Encoding.Unicode))
#elif NETFX_CORE
                global::Windows.Storage.StorageFile File = Do(global::Windows.Storage.StorageFile.GetFileFromPathAsync(Path));
                using (Stream Stream = Do(File.OpenStreamForReadAsync()))
                using (StreamReader Reader = new StreamReader(Stream, Encoding.Unicode))
#endif
                return Reader.ReadToEnd(); 
            }
            public string Format(DateTime Time, LogImportance Importance, string Region, string Message)
            { return '[' + Time.ToString(DateTimeFormat) + ']' + Symbol(Importance) + Region + '|' + Message; }
        }
    }
}
