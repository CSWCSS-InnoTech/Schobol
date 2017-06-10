using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public static TextLog Logger { get => DebugLog.Default; }
        /*public static ValueTask<string> ReadAll()
        { return Logger.ReadAll(); }*/
        public static ValueTask<string> Log(string Message) => Logger.Log(Message);
        public static ValueTask<string> Log(string Message, LogImportance Importance) => Logger.Log(Message, Importance);
        public static ValueTask<string> Log(string Message, string Format) => Logger.Log(Message, Format);
        public static ValueTask<string> Log(string Message, string Format, LogImportance Importance) => 
            Logger.Log(Message, Format, Importance);
        public static ValueTask<string> Log(Exception e) => Logger.Log(e);
        public static ValueTask<string> Log(Exception e, LogImportance Importance) => Logger.Log(e, Importance);

        public static T Log<T>(T Object) => Logger.Log(Object);
        public static T Log<T>(T Object, LogImportance Importance) => Logger.Log(Object, Importance);
        public static T Log<T>(T Object, string Format) => Logger.Log(Object, Format);
        public static T Log<T>(T Object, string Format, LogImportance Importance) => Logger.Log(Object, Format, Importance);

        public static string Region { get { return Logger.Region; } set { Logger.Region = value; } }
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
        public abstract class TextLog
        {
            public virtual string Region { get; set; }
            public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.FFF";

            public abstract ValueTask<string> ReadAll();
            public virtual ValueTask<string> Log(string Message)
            { return Log(Message, LogImportance.I); }
            public virtual ValueTask<string> Log(string Message, string Format)
            { return Log(string.Format(Format, Message), LogImportance.I); }
            public virtual ValueTask<string> Log(Exception e)
            { return Log(e, LogImportance.E); }
            public virtual ValueTask<string> Log(Exception e, string Format)
            { return Log(string.Format(Format, e), LogImportance.I); }
            public virtual ValueTask<string> Log(Exception e, LogImportance Importance)
            { return Log(e.ToString(), Importance); }
            public virtual ValueTask<string> Log(Exception e, string Format, LogImportance Importance)
            { return Log(string.Format(Format, e), Importance); }
            public abstract ValueTask<string> Log(string Message, LogImportance Importance);
            public virtual ValueTask<string> Log(string Message, string Format, LogImportance Importance)
            { return Log(string.Format(Format, Message), Importance); }


            public virtual T Log<T>(T Object)
            { Log(Object.ToString()); return Object; }
            public virtual T Log<T>(T Object, LogImportance Importance)
            { Log(Object.ToString(), Importance); return Object; }
            public virtual T Log<T>(T Object, string Format)
            { Log(string.Format(Format, Object)); return Object; }
            public virtual T Log<T>(T Object, string Format, LogImportance Importance)
            { Log(string.Format(Format, Object), Importance); return Object; }

            public virtual string Format(DateTime Time, LogImportance Importance, string Region, string Message)
            { return '[' + Time.ToString(DateTimeFormat) + ']' + Symbol(Importance) + Region + '|' + Message; }
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
        }
        /// <summary>
        /// For app logging.
        /// </summary>
        public class FileLog : TextLog
        {
            public static FileLog Default => new FileLog(Temp.GetFile("InnoTecheLearning.log"));
            public FileLog(string Path) { Init(Path); }
            public FileLog(string Path, string Region) : this(Path) { this.Region = Region; }
            string Path;
            public async void Init(string Path)
            {
#if __IOS__ || __ANDROID__
                await Unit.InvokeAsync(() => { if (!File.Exists(Path)) File.Create(Path).Dispose(); });
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
            public override async ValueTask<string> Log(string Message, LogImportance Importance)
            {
#if __IOS__|| __ANDROID__
                using (StreamWriter Writer = new StreamWriter(Path, true, Encoding.Unicode))
                { await Writer.WriteLineAsync(Format(DateTime.Now, Importance, Region, Message)); Writer.Flush(); }
#elif NETFX_CORE
                global::Windows.Storage.StorageFile File = await global::Windows.Storage.StorageFile.GetFileFromPathAsync(Path);
                await global::Windows.Storage.FileIO.AppendTextAsync(File, Format(DateTime.Now, Importance, Region, Message),
                    global::Windows.Storage.Streams.UnicodeEncoding.Utf16LE);
#endif
                return Message;
            }
            public override async ValueTask<string> ReadAll()
            {
#if __IOS__ || __ANDROID__
                using (StreamReader Reader = new StreamReader(Path, Encoding.Unicode))
#elif NETFX_CORE
                global::Windows.Storage.StorageFile File = await global::Windows.Storage.StorageFile.GetFileFromPathAsync(Path);
                using (Stream Stream = await File.OpenStreamForReadAsync())
                using (StreamReader Reader = new StreamReader(Stream, Encoding.Unicode))
#endif
                return await Reader.ReadToEndAsync(); 
            }
        }
        public class DebugLog : TextLog
        {
            public override ValueTask<string> Log(string Message, LogImportance Importance)
            {
                System.Diagnostics.Debug.WriteLine(Message, Format(DateTime.Now, Importance, Region, Message));
                return new ValueTask<string>(Message);
            }

            public override ValueTask<string> ReadAll() => throw new NotSupportedException("Cannot read logs back from output.");

            public static DebugLog Default { get; } = new DebugLog();
        }
    }
}