using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace InnoTecheLearning
{
    partial class Utils
    {   public static TextLog Log { get; } = new TextLog(Temp.GetFile("InnoTecheLearning"+dat));
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
        public interface ITextLog
        {
            string ReadAll();
            string Log(string Message);
            string Log(string Message, LogImportance Importance);
            void Log(Exception e);
            void Log(Exception e, LogImportance Importance);
        }
        /// <summary>
        /// For app logging.
        /// </summary>
        public class TextLog : ITextLog
        {
            string Path;
            public TextLog(string Path) { this.Path = Path; }
#if __IOS__|| __ANDROID__|| WINDOWS_UWP
            string Log(string Message)
            {using (StreamWriter Writer = new StreamWriter(Path, true, Encoding.Unicode))
                { Writer.WriteLine(DateTime.Now.ToString()+Message);Writer.Flush(); }
                return Message;
            }
#endif
        }
    }
}
