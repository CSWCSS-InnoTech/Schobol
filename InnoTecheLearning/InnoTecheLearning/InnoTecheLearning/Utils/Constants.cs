using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace InnoTecheLearning
{
    partial class Utils
    {
        public const string VersionFull = "0.10.0 (Xamarin Update) Alpha 63"; 
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
            public const string Trash =
@"游BB金句：星期二過後就係星期三
- 在非洲，每六十秒，就有一分鐘過去。
- 每呼吸60秒，就減少一分鐘的壽命。
- 張開你的眼睛！否則，你將什麼都看不見。
- 誰能想的到，這名16歲少女，在四年前，只是一名12歲少女。";
        }
        public const string Shorts =
"不時見到 WhatsApp 對話，有人打 LOL、er、LMK，但係一個都睇唔明，而且風氣吹到去周圍都係，"+
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
}