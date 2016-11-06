using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace InnoTecheLearning
{
    partial class Utils
    {
        public const string VersionFull = "0.10.0 (Xamarin Update) Alpha 59"; 
        public static Version Version
        { get { return Version.Parse(VersionFull.Remove(VersionFull.IndexOf(' '))); } }
        public static string VersionName
        { get { var NoVersion =  VersionFull.Substring(VersionFull.IndexOf('(') + 1);
                return NoVersion.Remove(')');} }
        public static VersionPhrase VersionState { get {
                return VersionPhrase.Parse(VersionFull.Replace(Version.ToString(), "").Replace(VersionName, ""));} }
        public struct VersionPhrase
        {   VersionStage Stage { get; }
            int Build { get; }
            public VersionPhrase(VersionStage Stage, int Build)
            {   this.Stage = Stage; this.Build = Build; }
            public static VersionPhrase Parse(string Text)
            {   Text = Text.Trim();
                return new VersionPhrase((VersionStage)Enum.Parse(typeof(VersionStage),
                    Text.Replace(' ', '_').TrimStart('(',')').TrimStart().TrimEnd(CharGen('0','9'))),
                    int.Parse(Text.TrimStart(CharGen('\x20','\x7f', CharGen('0','9')))));
            }
            public override string ToString()
            { return Stage.ToString().Replace('_', ' ') + ' ' + Build.ToString(); }
        }
        public enum VersionStage : byte {
            /// <summary>
            /// The app is in alpha phrase.
            /// </summary>
            Alpha = 0,
            /// <summary>
            /// The app is in beta phrase.
            /// </summary>
            Beta,
            /// <summary>
            /// The app is a release candidate.
            /// </summary>
            Release_Candidate,
            /// <summary>
            /// The app is released.
            /// </summary>
            Release
        }
        public static class Constants
        {
            /// <summary>
            /// Carriage return character.
            /// </summary>
            public const char Cr = '\r';
            /// <summary>
            /// Carriage return/linefeed character combination.
            /// </summary>
            public const string CrLf = "\r\n";
            /// <summary>
            /// Linefeed character.
            /// </summary>
            public const char Lf = '\n';
            /// <summary>
            /// The alarm (bell) character.
            /// </summary>
            public const char Alarm = '\a';
            /// <summary>
            /// Backspace character.
            /// </summary>
            public const char Back = '\b';
            /// <summary>
            /// Form feed character. Not used in Microsoft Windows.
            /// </summary>
            public const char FormFeed = '\f';
            /// <summary>
            /// Newline character. Aka CrLf.
            /// </summary>
            public const string NewLine = "\r\n";
            /// <summary>
            /// Null character.
            /// </summary>
            public const char NullChar = '\0';
            /// <summary>
            /// Not the same as a zero-length string (""); used for calling external procedures.
            /// </summary>
            public const string NullString = null;
            /// <summary>
            /// Error number. User-defined error numbers should be greater than this value.
            /// </summary>
            /// <example>For example: Err.Raise(Number) = ObjectError + 1000</example>
            public const int ObjectError = -2147221504;
            /// <summary>
            /// Tab character.
            /// </summary>
            public const char Tab = '\t';
            /// <summary>
            /// Vertical tab character. Not used in Microsoft Windows.
            /// </summary>
            public const char VerticalTab = '\v';
        }
    }
}