using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public static TextLog Logger { get => DebugLog.Default; }
        /*public static string ReadAll()
        { return Logger.ReadAll(); }*/
        public static string Log(string Message) => Logger.Log(Message);
        public static string Log(string Message, LogImportance Importance) => Logger.Log(Message, Importance);
        public static string Log(string Message, string Format) => Logger.Log(Message, Format);
        public static string Log(string Message, string Format, LogImportance Importance) =>
            Logger.Log(Message, Format, Importance);
        public static string Log(Exception e) => Logger.Log(e);
        public static string Log(Exception e, LogImportance Importance) => Logger.Log(e, Importance);

        public static T Log<T>(T Object) => Logger.Log(Object);
        public static T Log<T>(T Object, LogImportance Importance) => Logger.Log(Object, Importance);
        public static T Log<T>(T Object, string Format) => Logger.Log(Object, Format);
        public static T Log<T>(T Object, string Format, LogImportance Importance) => Logger.Log(Object, Format, Importance);

        public static T Log<T>(T Object, Func<T, string> Display) => Logger.Log(Object, Display(Object));
        public static T Log<T>(T Object, Func<T, string> Display, LogImportance Importance) =>
            Logger.Log(Object, Display(Object), Importance);

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

            public abstract string ReadAll();
            public virtual string Log(string Message)
            { return Log(Message, LogImportance.I); }
            public virtual string Log(string Message, string Format)
            { return Log(string.Format(Format, Message), LogImportance.I); }
            public virtual string Log(Exception e)
            { return Log(e, LogImportance.E); }
            public virtual string Log(Exception e, string Format)
            { return Log(string.Format(Format, e), LogImportance.I); }
            public virtual string Log(Exception e, LogImportance Importance)
            { return Log(e.ToString(), Importance); }
            public virtual string Log(Exception e, string Format, LogImportance Importance)
            { return Log(string.Format(Format, e), Importance); }
            public abstract string Log(string Message, LogImportance Importance);
            public virtual string Log(string Message, string Format, LogImportance Importance)
            { return Log(string.Format(Format, Message), Importance); }


            public virtual T Log<T>(T Object)
            { Log(Object.ToString()); return Object; }
            public virtual T Log<T>(T Object, LogImportance Importance)
            { Log(Object.ToString(), Importance); return Object; }
            public virtual T Log<T>(T Object, string Format)
            { Log(string.Format(Format, Object)); return Object; }
            public virtual T Log<T>(T Object, string Format, LogImportance Importance)
            { Log(string.Format(Format, Object), Importance); return Object; }

            public virtual string Format(DateTime Time, LogImportance Importance, string Region, string Message = null) =>
                $"[{Time.ToString(DateTimeFormat)}]{Symbol(Importance)}{Region}{(Message == null ? string.Empty : $"|{Message}")}";
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
            public FileLog(string Path) => this.Path = Path;
            public FileLog(string Path, string Region) : this(Path) => this.Region = Region;
            string Path_;
            public string Path
            {
                get => Path_;
                set
                {
                    try { if (!File.Exists(value)) File.Create(value).Dispose(); } catch { }
                    Path_ = value;
                }
            }
            public override string Log(string Message, LogImportance Importance)
            {
                using(FileStream Stream = new FileStream(Path, FileMode.Append))
                using (StreamWriter Writer = new StreamWriter(Stream, Encoding.Unicode))
                { Writer.WriteLine(Format(DateTime.Now, Importance, Region, Message)); Writer.Flush(); }
                return Message;
            }
            public override string ReadAll()
            {
                using (FileStream Stream = new FileStream(Path, FileMode.OpenOrCreate))
                using (StreamReader Reader = new StreamReader(Stream, Encoding.Unicode))
                    return Reader.ReadToEnd();
            }
        }
        public class DebugLog : TextLog
        {
            public override string Log(string Message, LogImportance Importance = LogImportance.I)
            {
                System.Diagnostics.Debug.WriteLine(Message, Format(DateTime.Now, Importance, Region));
                return Message;
            }

            public override string ReadAll() => throw new NotSupportedException("Cannot read logs back from output.");

            public static DebugLog Default { get; } = new DebugLog();
        }
    }
}