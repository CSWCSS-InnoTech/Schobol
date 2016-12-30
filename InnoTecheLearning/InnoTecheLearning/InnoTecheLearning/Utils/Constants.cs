using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
namespace InnoTecheLearning
{
    partial class Utils
    {
        public static Version Version { get { return Create.Version(0, 10, 0, VersionStage.Alpha, 113); } }
        public const string VersionName = "Xamarin Update";

        public static VersionStage VersionState { get { return (VersionStage)Version.MajorRevision; } }
        public static string VersionFull
        {
            get
            {
                return Version.ToString(3) + (string.IsNullOrEmpty(VersionName) ? "" : $" ({VersionName})") +
                       (VersionState > VersionStage.Undefined && VersionState < VersionStage.Release ?
                       " " + VersionState.ToString().Replace('_', ' ') + " " + Version.MinorRevision : "");
            }
        }
        public static string VersionShort
        {
            get
            {
                return Version.ToString(3) +
                       (VersionState > VersionStage.Undefined && VersionState < VersionStage.Release ?
                       (char)((int)VersionState + 'a' - 1) + Version.MinorRevision.ToString() : "");
            }
        }

        public static VersionStage GetVersionState(this Version Version) { return (VersionStage)Version.MajorRevision; }
        public static string ToShort(this Version Version)
        {
            return Version.ToString(3) +
                   (VersionState > VersionStage.Undefined && VersionState < VersionStage.Release ?
                   (char)((int)VersionState + 'a' - 1) + Version.MinorRevision.ToString() : "");
        }
        public static VersionStage GetVersionState(this ModifiableVersion Version) { return (VersionStage)Version.MajorRevision; }
        public static string ToShort(this ModifiableVersion Version)
        {
            return Version.ToString(3) +
                   (VersionState > VersionStage.Undefined && VersionState < VersionStage.Release ?
                   (char)((int)VersionState + 'a' - 1) + Version.MinorRevision.ToString() : "");
        }

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
            /// <summary>
            /// Full block character. Not used in Microsoft Windows.
            /// </summary>
            public const char FullBlock = '█';
            public const string Trash =
@"游BB金句：星期二過後就係星期三
- 在非洲，每六十秒，就有一分鐘過去。
- 每呼吸60秒，就減少一分鐘的壽命。
- 張開你的眼睛！否則，你將什麼都看不見。
- 誰能想的到，這名16歲少女，在四年前，只是一名12歲少女。";
            public const string Shorts =
"不時見到 WhatsApp 對話，有人打 LOL、er、LMK，但係一個都睇唔明，而且風氣吹到去周圍都係，" +
@"個個都覆你 OMW、BOT ，睇唔明好蝕底。短短幾個英文字母係咩意思？唔好再咁老套，以下係常見的英文WhatsApp潮語，以後同朋友溝通更容易！

常用口語

AFAIK = As far as I know = 就我所知
AFK = Away from keyboard = 暫時離開
AKA = As known as = 也就是
BBT = Be back tomorrow = 明天回來
BRB = Be right back = 馬上回來
BOT = Back on topic = 回到主題
FYI = For your information = 給你的資訊
GL = Good luck = 祝你好運
grats = Congratulations = 恭喜
IDK = I don’t know = 我不知道
IMO = In my opinion 在我看來
LMK = Let me know = 話我知
NP = No problem = 沒問題
ORLY = Oh really = 真的嗎
OMW = On my way = 在路上
PPL = people = 大家
RTG = Ready to go = 準備出發
TBH = To be honest = 老實說
TTYL = Talk to you later = 遲啲傾
LOL = Laugh out loud = 笑到好大聲
ROFL = Roll on floor laughing = 笑到碌地
OMG = Oh my god = 我的天

其他

ASAP = As soon as possible = 越快越好
ATM = At the moment = 現在
B-DAY = Birthday = 生日
EA = Each = 每個
HAV = Have = 有
NPNT = No picture / photo no truth 無圖無真相
PM = Private message = 私人留言
RDY/RTG = Ready / ready to go = 準備好
RIP = Rest in peace = 安息
TMR = Tomorrow = 明日

助語詞

ahaha = 啊哈哈
er = 呃……
hm = 嗯……
oops = 弊啦！
oh = 喔
yeah/ yup = 同意";
        }


        // A Version object contains four hierarchical numeric components: major, minor,
        // build and revision.  Build and revision may be unspecified, which is represented 
        // internally as a -1.  By definition, an unspecified component matches anything 
        // (both unspecified and specified), and an unspecified component is "less than" any
        // specified component.

#if __ANDROID__
        [Serializable]
#endif
        [System.Runtime.InteropServices.ComVisible(true)]
        public sealed class ModifiableVersion :
#if __ANDROID__
            ICloneable,
#endif
            IComparable
            , IComparable<ModifiableVersion>, IEquatable<ModifiableVersion>
        {
            public static implicit operator Version(ModifiableVersion MV)
            {
                return MV._Revision == -1 ? MV._Build == -1 ? new Version(MV._Major, MV._Minor) :
                      new Version(MV._Major, MV._Minor, MV._Build) :
                      new Version(MV._Major, MV._Minor, MV._Build, MV._Revision);
            }
            public static implicit operator ModifiableVersion(Version V)
            {
                return V.Revision == -1 ? V.Build == -1 ? new ModifiableVersion(V.Major, V.Minor) :
                      new ModifiableVersion(V.Major, V.Minor, V.Build) :
                      new ModifiableVersion(V.Major, V.Minor, V.Build, V.Revision);
            }
            // AssemblyName depends on the order staying the same
            private int _Major;
            private int _Minor;
            private int _Build = -1;
            private int _Revision = -1;
            private static readonly char[] SeparatorsArray = new char[] { '.' };

            public ModifiableVersion(int major, int minor, int build, int revision)
            {
                if (major < 0)
                    throw new ArgumentOutOfRangeException("major", major, "major is less than zero.");

                if (minor < 0)
                    throw new ArgumentOutOfRangeException("minor", minor, "minor is less than zero.");

                if (build < 0)
                    throw new ArgumentOutOfRangeException("build", build, "build is less than zero.");

                if (revision < 0)
                    throw new ArgumentOutOfRangeException("revision", revision, "revision is less than zero.");
                Contract.EndContractBlock();

                _Major = major;
                _Minor = minor;
                _Build = build;
                _Revision = revision;
            }

            public ModifiableVersion(int major, int minor, int build)
            {
                if (major < 0)
                    throw new ArgumentOutOfRangeException("major", major, "major is less than zero.");

                if (minor < 0)
                    throw new ArgumentOutOfRangeException("minor", minor, "minor is less than zero.");

                if (build < 0)
                    throw new ArgumentOutOfRangeException("build", build, "build is less than zero.");

                Contract.EndContractBlock();

                _Major = major;
                _Minor = minor;
                _Build = build;
            }

            public ModifiableVersion(int major, int minor)
            {
                if (major < 0)
                    throw new ArgumentOutOfRangeException("major", major, "major is less than zero.");

                if (minor < 0)
                    throw new ArgumentOutOfRangeException("minor", minor, "minor is less than zero.");
                Contract.EndContractBlock();

                _Major = major;
                _Minor = minor;
            }

            public ModifiableVersion(String version)
            {
                Version v = ModifiableVersion.Parse(version);
                _Major = v.Major;
                _Minor = v.Minor;
                _Build = v.Build;
                _Revision = v.Revision;
            }

#if FEATURE_LEGACYNETCF
        //required for Mango AppCompat
        [System.Runtime.CompilerServices.FriendAccessAllowed]
#endif
            public ModifiableVersion()
            {
                _Major = 0;
                _Minor = 0;
            }

            // Properties for setting and getting version numbers
            public int Major
            {
                get { return _Major; }
                set { _Major = value; }
            }

            public int Minor
            {
                get { return _Minor; }
                set { _Minor = value; }
            }

            public int Build
            {
                get { return _Build; }
                set { _Build = value; }
            }

            public int Revision
            {
                get { return _Revision; }
                set { _Revision = value; }
            }

            public short MajorRevision
            {
                get { return (short)(_Revision >> 16); }
                set { _Revision = value * (1 << 16) + MinorRevision; }
            }

            public short MinorRevision
            {
                get { return (short)(_Revision & 0xFFFF); }
                set { _Revision = MajorRevision * (1 << 16) + value; }
            }

            public Object Clone()
            {
                ModifiableVersion v = new ModifiableVersion();
                v._Major = _Major;
                v._Minor = _Minor;
                v._Build = _Build;
                v._Revision = _Revision;
                return (v);
            }

            public int CompareTo(Object version)
            {
                if (version == null)
                {
#if FEATURE_LEGACYNETCF
                if (CompatibilitySwitches.IsAppEarlierThanWindowsPhone8) {
                    throw new ArgumentOutOfRangeException();
                } else {
#endif
                    return 1;
#if FEATURE_LEGACYNETCF
                }
#endif
                }

                ModifiableVersion v = version as ModifiableVersion;
                if (v == null)
                {
#if FEATURE_LEGACYNETCF
                if (CompatibilitySwitches.IsAppEarlierThanWindowsPhone8) {
                    throw new InvalidCastException(Environment.GetResourceString("Arg_MustBeVersion"));
                } else {
#endif
                    throw new ArgumentException("version must be a ModifiableVersion.", "version");
#if FEATURE_LEGACYNETCF
                }
#endif
                }

                if (this._Major != v._Major)
                    if (this._Major > v._Major)
                        return 1;
                    else
                        return -1;

                if (this._Minor != v._Minor)
                    if (this._Minor > v._Minor)
                        return 1;
                    else
                        return -1;

                if (this._Build != v._Build)
                    if (this._Build > v._Build)
                        return 1;
                    else
                        return -1;

                if (this._Revision != v._Revision)
                    if (this._Revision > v._Revision)
                        return 1;
                    else
                        return -1;

                return 0;
            }

            public int CompareTo(ModifiableVersion value)
            {
                if (value == null)
                    return 1;

                if (this._Major != value._Major)
                    if (this._Major > value._Major)
                        return 1;
                    else
                        return -1;

                if (this._Minor != value._Minor)
                    if (this._Minor > value._Minor)
                        return 1;
                    else
                        return -1;

                if (this._Build != value._Build)
                    if (this._Build > value._Build)
                        return 1;
                    else
                        return -1;

                if (this._Revision != value._Revision)
                    if (this._Revision > value._Revision)
                        return 1;
                    else
                        return -1;

                return 0;
            }

            public override bool Equals(Object obj)
            {
                ModifiableVersion v = obj as ModifiableVersion;
                if (v == null)
                    return false;

                // check that major, minor, build & revision numbers match
                if ((this._Major != v._Major) ||
                    (this._Minor != v._Minor) ||
                    (this._Build != v._Build) ||
                    (this._Revision != v._Revision))
                    return false;

                return true;
            }

            public bool Equals(ModifiableVersion obj)
            {
                if (obj == null)
                    return false;

                // check that major, minor, build & revision numbers match
                if ((this._Major != obj._Major) ||
                    (this._Minor != obj._Minor) ||
                    (this._Build != obj._Build) ||
                    (this._Revision != obj._Revision))
                    return false;

                return true;
            }

            public override int GetHashCode()
            {
                // Let's assume that most version numbers will be pretty small and just
                // OR some lower order bits together.

                int accumulator = 0;

                accumulator |= (this._Major & 0x0000000F) << 28;
                accumulator |= (this._Minor & 0x000000FF) << 20;
                accumulator |= (this._Build & 0x000000FF) << 12;
                accumulator |= (this._Revision & 0x00000FFF);

                return accumulator;
            }

            public override String ToString()
            {
                if (_Build == -1) return (ToString(2));
                if (_Revision == -1) return (ToString(3));
                return (ToString(4));
            }

            public String ToString(int fieldCount)
            {
                StringBuilder sb;
                switch (fieldCount)
                {
                    case 0:
                        return (String.Empty);
                    case 1:
                        return (_Major.ToString());
                    case 2:
                        sb = StringBuilderCache.Acquire();
                        AppendPositiveNumber(_Major, sb);
                        sb.Append('.');
                        AppendPositiveNumber(_Minor, sb);
                        return StringBuilderCache.GetStringAndRelease(sb);
                    default:
                        if (_Build == -1)
                            throw new ArgumentException("fieldCount must be between 0 to 2", "fieldCount");

                        if (fieldCount == 3)
                        {
                            sb = StringBuilderCache.Acquire();
                            AppendPositiveNumber(_Major, sb);
                            sb.Append('.');
                            AppendPositiveNumber(_Minor, sb);
                            sb.Append('.');
                            AppendPositiveNumber(_Build, sb);
                            return StringBuilderCache.GetStringAndRelease(sb);
                        }

                        if (_Revision == -1)
                            throw new ArgumentException("fieldCount must be between 0 to 3", "fieldCount");

                        if (fieldCount == 4)
                        {
                            sb = StringBuilderCache.Acquire();
                            AppendPositiveNumber(_Major, sb);
                            sb.Append('.');
                            AppendPositiveNumber(_Minor, sb);
                            sb.Append('.');
                            AppendPositiveNumber(_Build, sb);
                            sb.Append('.');
                            AppendPositiveNumber(_Revision, sb);
                            return StringBuilderCache.GetStringAndRelease(sb);
                        }

                        throw new ArgumentException("fieldCount must be between 0 to 4", "fieldCount");
                }
            }

            //
            // AppendPositiveNumber is an optimization to append a number to a StringBuilder object without
            // doing any boxing and not even creating intermediate string.
            // Note: as we always have positive numbers then it is safe to convert the number to string 
            // regardless of the current culture as we’ll not have any punctuation marks in the number
            //
            private const int ZERO_CHAR_VALUE = (int)'0';
            private static void AppendPositiveNumber(int num, StringBuilder sb)
            {
                Contract.Assert(num >= 0, "AppendPositiveNumber expect positive numbers");

                int index = sb.Length;
                int reminder;

                do
                {
                    reminder = num % 10;
                    num = num / 10;
                    sb.Insert(index, (char)(ZERO_CHAR_VALUE + reminder));
                } while (num > 0);
            }

            public static Version Parse(string input)
            {
                if (input == null)
                {
                    throw new ArgumentNullException("input");
                }
                Contract.EndContractBlock();

                VersionResult r = new VersionResult();
                r.Init("input", true);
                if (!TryParseVersion(input, ref r))
                {
                    throw r.GetVersionParseException();
                }
                return r.m_parsedVersion;
            }

            public static bool TryParse(string input, out Version result)
            {
                VersionResult r = new VersionResult();
                r.Init("input", false);
                bool b = TryParseVersion(input, ref r);
                result = r.m_parsedVersion;
                return b;
            }

            private static bool TryParseVersion(string version, ref VersionResult result)
            {
                int major, minor, build, revision;

                if ((Object)version == null)
                {
                    result.SetFailure(ParseFailureKind.ArgumentNullException);
                    return false;
                }

                string[] parsedComponents = version.Split(SeparatorsArray);
                int parsedComponentsLength = parsedComponents.Length;
                if ((parsedComponentsLength < 2) || (parsedComponentsLength > 4))
                {
                    result.SetFailure(ParseFailureKind.ArgumentException);
                    return false;
                }

                if (!TryParseComponent(parsedComponents[0], "version", ref result, out major))
                {
                    return false;
                }

                if (!TryParseComponent(parsedComponents[1], "version", ref result, out minor))
                {
                    return false;
                }

                parsedComponentsLength -= 2;

                if (parsedComponentsLength > 0)
                {
                    if (!TryParseComponent(parsedComponents[2], "build", ref result, out build))
                    {
                        return false;
                    }

                    parsedComponentsLength--;

                    if (parsedComponentsLength > 0)
                    {
                        if (!TryParseComponent(parsedComponents[3], "revision", ref result, out revision))
                        {
                            return false;
                        }
                        else
                        {
                            result.m_parsedVersion = new Version(major, minor, build, revision);
                        }
                    }
                    else
                    {
                        result.m_parsedVersion = new Version(major, minor, build);
                    }
                }
                else
                {
                    result.m_parsedVersion = new Version(major, minor);
                }

                return true;
            }

            private static bool TryParseComponent(string component, string componentName, ref VersionResult result, out int parsedComponent)
            {
                if (!Int32.TryParse(component, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedComponent))
                {
                    result.SetFailure(ParseFailureKind.FormatException, component);
                    return false;
                }

                if (parsedComponent < 0)
                {
                    result.SetFailure(ParseFailureKind.ArgumentOutOfRangeException, componentName);
                    return false;
                }

                return true;
            }

            public static bool operator ==(ModifiableVersion v1, ModifiableVersion v2)
            {
                if (Object.ReferenceEquals(v1, null))
                {
                    return Object.ReferenceEquals(v2, null);
                }

                return v1.Equals(v2);
            }

            public static bool operator !=(ModifiableVersion v1, ModifiableVersion v2)
            {
                return !(v1 == v2);
            }

            public static bool operator <(ModifiableVersion v1, ModifiableVersion v2)
            {
                if ((Object)v1 == null)
                    throw new ArgumentNullException("v1");
                Contract.EndContractBlock();
                return (v1.CompareTo(v2) < 0);
            }

            public static bool operator <=(ModifiableVersion v1, ModifiableVersion v2)
            {
                if ((Object)v1 == null)
                    throw new ArgumentNullException("v1");
                Contract.EndContractBlock();
                return (v1.CompareTo(v2) <= 0);
            }

            public static bool operator >(ModifiableVersion v1, ModifiableVersion v2)
            {
                return (v2 < v1);
            }

            public static bool operator >=(ModifiableVersion v1, ModifiableVersion v2)
            {
                return (v2 <= v1);
            }

            internal enum ParseFailureKind
            {
                ArgumentNullException,
                ArgumentException,
                ArgumentOutOfRangeException,
                FormatException
            }

            internal struct VersionResult
            {
                internal Version m_parsedVersion;
                internal ParseFailureKind m_failure;
                internal string m_exceptionArgument;
                internal string m_argumentName;
                internal bool m_canThrow;

                internal void Init(string argumentName, bool canThrow)
                {
                    m_canThrow = canThrow;
                    m_argumentName = argumentName;
                }

                internal void SetFailure(ParseFailureKind failure)
                {
                    SetFailure(failure, String.Empty);
                }

                internal void SetFailure(ParseFailureKind failure, string argument)
                {
                    m_failure = failure;
                    m_exceptionArgument = argument;
                    if (m_canThrow)
                    {
                        throw GetVersionParseException();
                    }
                }

                internal Exception GetVersionParseException()
                {
                    switch (m_failure)
                    {
                        case ParseFailureKind.ArgumentNullException:
                            return new ArgumentNullException(m_argumentName);
                        case ParseFailureKind.ArgumentException:
                            return new ArgumentException("Input must be convertible to ModifiableVersion.");
                        case ParseFailureKind.ArgumentOutOfRangeException:
                            return new ArgumentOutOfRangeException(m_exceptionArgument, "Input is out of ModifiableVersion's range.");
                        case ParseFailureKind.FormatException:
                            // Regenerate the FormatException as would be thrown by Int32.Parse()
                            try
                            {
                                Int32.Parse(m_exceptionArgument, CultureInfo.InvariantCulture);
                            }
                            catch (FormatException e)
                            {
                                return e;
                            }
                            catch (OverflowException e)
                            {
                                return e;
                            }
                            Contract.Assert(false, "Int32.Parse() did not throw exception but TryParse failed: " + m_exceptionArgument);
                            return new FormatException("String format is invalid.");
                        default:
                            Contract.Assert(false, "Unmatched case in Version.GetVersionParseException() for value: " + m_failure);
                            return new ArgumentException("String must be convertible to ModifiableVersion.");
                    }
                }

            }
            internal static class StringBuilderCache
            {
                // The value 360 was chosen in discussion with performance experts as a compromise between using
                // as litle memory (per thread) as possible and still covering a large part of short-lived
                // StringBuilder creations on the startup path of VS designers.
                private const int MAX_BUILDER_SIZE = 360;

                [ThreadStatic]
                private static StringBuilder CachedInstance;

                public static StringBuilder Acquire(int capacity = 16)
                {
                    if (capacity <= MAX_BUILDER_SIZE)
                    {
                        StringBuilder sb = StringBuilderCache.CachedInstance;
                        if (sb != null)
                        {
                            // Avoid stringbuilder block fragmentation by getting a new StringBuilder
                            // when the requested size is larger than the current capacity
                            if (capacity <= sb.Capacity)
                            {
                                StringBuilderCache.CachedInstance = null;
                                sb.Clear();
                                return sb;
                            }
                        }
                    }
                    return new StringBuilder(capacity);
                }

                public static void Release(StringBuilder sb)
                {
                    if (sb.Capacity <= MAX_BUILDER_SIZE)
                    {
                        StringBuilderCache.CachedInstance = sb;
                    }
                }

                public static string GetStringAndRelease(StringBuilder sb)
                {
                    string result = sb.ToString();
                    Release(sb);
                    return result;
                }
            }
        }
    }
}