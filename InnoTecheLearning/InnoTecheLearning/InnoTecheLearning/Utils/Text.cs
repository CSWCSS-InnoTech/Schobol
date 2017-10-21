using System;
using Xamarin.Forms;

namespace InnoTecheLearnUtilities
{
    public static partial class Utils
    {
        /// <summary>
        /// A piece of ordinary text which is interchangable with a label, span, string and array of chars.
        /// </summary>
        public struct Text : IComparable
        {
            internal static Random Rnd { get; } = new Random();
            public static readonly Text Null = (string)null;
            public static readonly Text Empty = string.Empty;
            public static readonly Text Default = default(Text);
            public static Text Random
            {
                get
                {
                    char[] Chars = new char[Rnd.Next(0, 20)];
                    for (int i = 0; i < Chars.Length; i++)
                    {
                        Chars[i] = (char)Rnd.Next(' ', '~');
                    }
                    return Chars;
                }
            }
            public static Text RandomChar
            {
                get
                {
                    return new Text((char)Rnd.Next(char.MinValue, char.MaxValue));
                }
            }
            public static Text RandomLatin
            {
                get
                {
                    char[] Chars = new char[Rnd.Next(1, 20)];
                    for (int i = 0; i < Chars.Length; i++)
                    {
                        Chars[i] = (char)(Convert.ToBoolean(Rnd.Next(0, 1))?
                            Rnd.Next('A', 'Z'): Rnd.Next('a', 'z'));
                    }
                    return Chars;
                }
            }
            public static Text RandomInteger
            {
                get
                {
                    char[] Chars = new char[Rnd.Next(1, 20)];
                    for (int i = 0; i < Chars.Length; i++)
                    {
                        Chars[i] = (char)Rnd.Next('0', '9');
                    }
                    if (Convert.ToBoolean(Rnd.Next(0, 1))) Chars[0] = '-';
                    return new string(Chars).TrimStart('0');
                }
            }
            public static Text RandomUnicode
            {
                get
                {
                    char[] Chars = new char[Rnd.Next(0, 20)];
                    for (int i = 0; i < Chars.Length; i++)
                    {
                        Chars[i] = (char)Rnd.Next(char.MaxValue);
                    }
                    return Chars;
                }
            }
            public string Value { get; set; }
            public Text(char Text)
            { Value = new string(new[] { Text }); }
            public Text(char[] Text)
            { Value = new string(Text); }
            public Text(string Text)
            { Value = Text; }
            public Text Append(Text Text)
            {
                Value += Text;
                return this;
            }
            public Text Append(char Char)
            {
                Value += Char;
                return this;
            }
            public Text Clear()
            {
                Value = null;
                return this;
            }
            public Text TrimStart()
            {
                Value = Value.TrimStart();
                return this;
            }
            public Text Trim()
            {
                Value = Value.Trim();
                return this;
            }
            public Text TrimEnd()
            {
                Value = Value.TrimEnd();
                return this;
            }
            /// <summary>
            /// Removes a string of characters from the beginning of this <see cref="Text"/>.
            /// </summary>
            /// <returns>The current <see cref="Text"/> object to continue nested instructions.</returns>
            public Text TrimStart(params char[] TrimChars)
            {
                Value = Value.TrimStart(TrimChars);
                return this;
            }
            /// <summary>
            /// Removes a string of characters from the ends of this <see cref="Text"/>.
            /// </summary>
            /// <returns>The current <see cref="Text"/> object to continue nested instructions.</returns>
            public Text Trim(params char[] TrimChars)
            {
                Value = Value.Trim(TrimChars);
                return this;
            }
            /// <summary>
            /// Removes a string of characters from the end of this <see cref="Text"/>.
            /// </summary>
            /// <returns>The current <see cref="Text"/> object to continue nested instructions.</returns>
            public Text TrimEnd(params char[] TrimChars)
            {
                Value = Value.TrimEnd(TrimChars);
                return this;
            }
            public Text ToLower()
            {
                Value = Value.ToLower();
                return this;
            }
            public Text ToUpper()
            {
                Value = Value.ToUpper();
                return this;
            }
            public Text PadLeft(int TotalWidth)
            {
                Value = Value.PadLeft(TotalWidth);
                return this;
            }
            public Text PadRight(int TotalWidth)
            {
                Value = Value.PadRight(TotalWidth);
                return this;
            }
            public bool StartsWith(char Char)
            { return First() == Char; }
            public bool StartsWith(string String)
            { return Value.StartsWith(String); }
            public bool StartsWith(Text Text)
            { return Value.StartsWith(Text); }
            public bool EndsWith(char Char)
            { return Last() == Char; }
            public bool EndsWith(string String)
            { return Value.EndsWith(String); }
            public bool EndsWith(Text Text)
            { return Value.EndsWith(Text); }
            public char First()
            { return Value[0]; }
            public char Last()
            { return Value[Length - 1]; }
            public Text Remove(int StartIndex)
            {
                Value = Value.Remove(StartIndex);
                return this;
            }
            public Text Remove(int StartIndex, int Count)
            {
                Value = Value.Remove(StartIndex, Count);
                return this;
            }
            /// <summary>
            /// Returns a substring of this <see cref="Text"/>.
            /// </summary>
            /// <returns>The current <see cref="Text"/> object to continue nested instructions.</returns>
            public Text Substring(int StartIndex)
            {
                Value = Value.Substring(StartIndex);
                return this;
            }
            public Text Substring(int StartIndex, int Length)
            {
                Value = Value.Substring(StartIndex, Length);
                return this;
            }
            public Text Replace(char OldChar, char NewChar)
            {
                Value = Value.Replace(OldChar, NewChar);
                return this;
            }
            public Text Replace(string OldString, string NewString)
            {
                Value = Value.Replace(OldString, NewString);
                return this;
            }
            public Text Replace(Text OldText, Text NewText)
            {
                Value = Value.Replace(OldText, NewText);
                return this;
            }
            public int Length
            { get { return Value.Length; } }
            public static implicit operator Span(Text Text)
            { return new Span { Text = Text.Value }; }
            public static implicit operator Text(Span Span)
            { return new Text(Span.Text); }
            public static implicit operator string(Text Text)
            { return Text.Value; }
            public static implicit operator Text(string String)
            { return new Text(String); }
            public static implicit operator Label(Text Text)
            { return new Label { Text = Text, TextColor = Color.Black }; }
            public static implicit operator Text(Label Label)
            { return new Text(Label.Text); }
            public static implicit operator char[] (Text Text)
            { return Text.Value.ToCharArray(); }
            public static implicit operator Text(char[] Char)
            { return new Text(new string(Char)); }
            public static explicit operator Button(Text Text)
            { return new Button { Text = Text, TextColor = Color.Black }; }
            public static implicit operator Text(Button Button)
            { return new Text(Button.Text); }
            public static explicit operator SByte(Text Text)
            { return SByte.Parse(Text.Value); }
            public static implicit operator Text(SByte SByte)
            { return new Text(SByte.ToString()); }
            public static implicit operator Byte(Text Text)
            { return Byte.Parse(Text.Value); }
            public static implicit operator Text(Byte Byte)
            { return new Text(Byte.ToString()); }
            public static implicit operator Int16(Text Text)
            { return Int16.Parse(Text.Value); }
            public static implicit operator Text(Int16 Int16)
            { return new Text(Int16.ToString()); }
            public static implicit operator UInt16(Text Text)
            { return UInt16.Parse(Text.Value); }
            public static implicit operator Text(UInt16 UInt16)
            { return new Text(UInt16.ToString()); }
            public static implicit operator Int32(Text Text)
            { return Int32.Parse(Text.Value); }
            public static implicit operator Text(Int32 Int32)
            { return new Text(Int32.ToString()); }
            public static implicit operator UInt32(Text Text)
            { return UInt32.Parse(Text.Value); }
            public static implicit operator Text(UInt32 UInt32)
            { return new Text(UInt32.ToString()); }
            public static implicit operator Int64(Text Text)
            { return Int64.Parse(Text.Value); }
            public static implicit operator Text(Int64 Int64)
            { return new Text(Int64.ToString()); }
            public static implicit operator UInt64(Text Text)
            { return UInt64.Parse(Text.Value); }
            public static implicit operator Text(UInt64 UInt64)
            { return new Text(UInt64.ToString()); }
            public static implicit operator Single(Text Text)
            { return Single.Parse(Text.Value); }
            public static implicit operator Text(Single Single)
            { return new Text(Single.ToString()); }
            public static implicit operator Double(Text Text)
            { return Double.Parse(Text.Value); }
            public static implicit operator Text(Double Double)
            { return new Text(Double.ToString()); }
            public static implicit operator Decimal(Text Text)
            { return Decimal.Parse(Text.Value); }
            public static implicit operator Text(Decimal Decimal)
            { return new Text(Decimal.ToString()); }
            public static implicit operator Boolean(Text Text)
            { return Boolean.Parse(Text.Value); }
            public static implicit operator Text(Boolean Boolean)
            { return new Text(Boolean.ToString()); }
            public static implicit operator DateTime(Text Text)
            { return DateTime.Parse(Text.Value); }
            public static implicit operator Text(DateTime DateTime)
            { return new Text(DateTime.ToString()); }
            public static implicit operator TimeSpan(Text Text)
            { return TimeSpan.Parse(Text.Value); }
            public static implicit operator Text(TimeSpan TimeSpan)
            { return new Text(TimeSpan.ToString()); }
            public static implicit operator IntPtr(Text Text)
            { return new IntPtr(Int32.Parse(Text.Value)); }
            public static implicit operator Text(IntPtr IntPtr)
            { return new Text(IntPtr.ToInt32().ToString()); }
            public static implicit operator UIntPtr(Text Text)
            { return new UIntPtr(UInt32.Parse(Text.Value)); }
            public static implicit operator Text(UIntPtr UIntPtr)
            { return new Text(UIntPtr.ToUInt32().ToString()); }
            public static implicit operator Array(Text Text)
            { return Text.Value.ToCharArray(); }
            public static implicit operator Text(Array Array)
            {
                Text Text = new Text();
                foreach (var Item in Array)
                { char.TryParse(Item.ToString(), out char C); Text.Append(C); };
                return Text;
            }
            public override string ToString()
            { return Value; }
            public int CompareTo(Text Text)
            {
                return Value.CompareTo(Text);
            }
            public int CompareTo(string String)
            {
                return Value.CompareTo(String);
            }
            public int CompareTo(object value)
            {
                if (value == null)
                    return 1;
                Text Convert = new Text();
                if (!(value is string))
                    if (TryCast(value, out Convert))
                    { value = Convert; }
                    else
                        throw new ArgumentException("Value must be convertible to string.", "value");
                return string.Compare(Value, (string)value, StringComparison.CurrentCulture);
            }
        }
    }
}
