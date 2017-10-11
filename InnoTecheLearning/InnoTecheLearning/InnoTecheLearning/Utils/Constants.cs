using System;

namespace InnoTecheLearning
{
    partial class Utils
    {
#region Version
        public const string VersionFull = "0.11.0 Alpha 9"; //0.10.0 (Xamarin Update) Beta 2
        public const string VersionAssembly = "0.11.0";
        public const string VersionAssemblyFile = "0.11";
        public const string VersionAssemblyInfo = VersionFull;

#region VersionFunctions
        public static string VersionShort
        { get => (VersionFull.Contains("(") ? 
                     (VersionFull.Remove(VersionFull.IndexOf('(') - 1, VersionFull.IndexOf(')') - VersionFull.IndexOf('(') + 2)) :
                     VersionFull)
                 .Replace(" Alpha ", "a").Replace(" Beta ", "b").Replace(" Release Candidate ", "c"); }
        public static Version Version
        {
            get
            {
                var VersionDecomposition = VersionShort.Split('.');
                var IndexOfStage = VersionDecomposition[2].IndexOfAny(new[] { 'a', 'b', 'c' });
                return CreateVersion(int.Parse(VersionDecomposition[0]), int.Parse(VersionDecomposition[1]),
                    int.Parse(IndexOfStage == -1 ? VersionDecomposition[2] : VersionDecomposition[2].Remove(IndexOfStage)),
                    IndexOfStage == -1 ? VersionStage.Release : (VersionStage)(VersionDecomposition[2][IndexOfStage] - 'a'),
                    IndexOfStage == -1 ? (short)0 : short.Parse(VersionDecomposition[2].Substring(IndexOfStage + 1)));
            }
        }
        public static string VersionName
        { get => VersionFull.Contains("(") ? 
                 VersionFull.Substring(VersionFull.IndexOf('(') + 1, VersionFull.IndexOf(')') - VersionFull.IndexOf('(') - 1) :
                 string.Empty; }
        public static VersionStage VersionState { get => (VersionStage)Version.MajorRevision; }
        public static Version CreateVersion(int Major, int Minor, int Build = 0, VersionStage Stage = 0, short Revision = 0) =>
            new Version(Major, Minor, Build, (int)Stage * (1 << 16) + Revision);

        public static VersionStage GetVersionState(this Version Version) { return (VersionStage)Version.MajorRevision; }
        public static string ToShort(this Version Version) =>
            Version.ToString(3) +
            (Version.GetVersionState() > VersionStage.Undefined && Version.GetVersionState() < VersionStage.Release ?
            (char)((int)Version.GetVersionState() + 'a' - 1) + Version.MinorRevision.ToString() : "");
        public enum VersionStage : byte
        {
            /// <summary>
            /// The stage of the app is undefined.
            /// </summary>
            Undefined,
            /// <summary>
            /// The app is in alpha phrase.
            /// </summary>
            Alpha,
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
        #endregion
#endregion

#region AssemblyInfo
        public const string AssemblyTitle = "CSWCSS eLearn Utilities";
        public const string AssemblyDescription = "The eLearn Utilities App is an app empowering " +
            "students to learn more efficiently through digitalised learning.";
        public const string AssemblyConfiguration = "";
        public const string AssemblyCompany = "Innovative Technology Society of CSWCSS";
        public const string AssemblyProduct = "InnoTecheLearning"; //Plz no change, affects Storage I/O
        public const string AssemblyCopyright = "Copyright © Innovative Technology Society of CSWCSS 2017";
        public const string AssemblyTrademark = "";
        public const string AssemblyCulture = "";
        public const bool ComVisible = false;
        public const string ComGuid = "72bdc44f-c588-44f3-b6df-9aace7daafdd";
        #endregion

        #region Numbers
        public const float RawXMultiplier = 1;

        public const float RawYMultiplier = 1.5f;
        #endregion

#region Fonts
        public const int ColourIcon = 0x090170;

        public const string FontDictionary =
#if __IOS__
            "KAIU.TTF"
#elif __ANDROID__
            "monospace" //"KAIU.TTF#標楷體"
#elif WINDOWS_UWP
            "Assets/Fonts/KAIU.TTF#標楷體"
#endif
            ; //"Courier New, Georgia, Serif"
#endregion

#region Character Substitutes
        public const string SubLeftBracket = "毲";
        public const string SubRightBracket = "䫎";
        #endregion

        public const char Error = 'ⓧ'; //⮾ 
        public const string Cursor = "‸";

    }
}