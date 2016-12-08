using System;
using System.Collections.Generic;
using System.Text;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public static class Conversion
        {
            public static double Val(string InputStr)
            {
                int length = 0;
                int num4 = 0;
                int num5 = 0;
                int num6 = 0;
                double num7 = 0;
                if (InputStr == null)
                {
                    length = 0;
                }
                else
                {
                    length = InputStr.Length;
                }
                int num2 = 0;
                while (num2 < length)
                {
                    switch (InputStr[num2])
                    {
                        case '\t':
                        case '\n':
                        case '\r':
                        case ' ':
                            //case ' ':
                            break;

                        case '\v':
                        case '\f':
                            goto Label_004C;

                        default:
                            goto Label_004C;
                    }
                    num2++;
                }
                Label_004C:
                if (num2 >= length)
                {
                    return 0.0;
                }
                char ch = InputStr[num2];
                if (ch == '&')
                {
                    return HexOrOctValue(InputStr, num2 + 1);
                }
                bool flag = false;
                bool flag2 = false;
                bool flag3 = false;
                double y = 0.0;
                ch = InputStr[num2];
                switch (ch)
                {
                    case '-':
                        flag3 = true;
                        num2++;
                        break;

                    case '+':
                        num2++;
                        break;
                }
                while (num2 < length)
                {
                    ch = InputStr[num2];
                    char ch2 = ch;
                    if (((ch2 == '\t') || (ch2 == '\n')) || (((ch2 == '\r') || (ch2 == ' ')) || (ch2 == ' ')))
                    {
                        num2++;
                    }
                    else
                    {
                        if (ch2 == '0')
                        {
                            if ((num4 != 0) || flag)
                            {
                                num7 = ((num7 * 10.0) + ((double)ch)) - 48.0;
                                num2++;
                                num4++;
                            }
                            else
                            {
                                num2++;
                            }
                            continue;
                        }
                        if ((ch2 >= '1') && (ch2 <= '9'))
                        {
                            num7 = ((num7 * 10.0) + ((double)ch)) - 48.0;
                            num2++;
                            num4++;
                        }
                        else
                        {
                            if (ch2 == '.')
                            {
                                num2++;
                                if (flag)
                                {
                                    break;
                                }
                                flag = true;
                                num6 = num4;
                                continue;
                            }
                            if (((ch2 == 'e') || (ch2 == 'E')) || ((ch2 == 'd') || (ch2 == 'D')))
                            {
                                flag2 = true;
                                num2++;
                            }
                            break;
                        }
                    }
                }
                if (flag)
                {
                    num5 = num4 - num6;
                }
                if (!flag2)
                {
                    if (flag && (num5 != 0))
                    {
                        num7 /= Math.Pow(10.0, (double)num5);
                    }
                }
                else
                {
                    bool flag4 = false;
                    bool flag5 = false;
                    while (num2 < length)
                    {
                        ch = InputStr[num2];
                        char ch3 = ch;
                        if (((ch3 == '\t') || (ch3 == '\n')) || (((ch3 == '\r') || (ch3 == ' ')) || (ch3 == ' ')))
                        {
                            num2++;
                        }
                        else if ((ch3 >= '0') && (ch3 <= '9'))
                        {
                            y = ((y * 10.0) + ((double)ch)) - 48.0;
                            num2++;
                        }
                        else
                        {
                            if (ch3 == '+')
                            {
                                if (flag4)
                                {
                                    break;
                                }
                                flag4 = true;
                                num2++;
                                continue;
                            }
                            if ((ch3 != '-') || flag4)
                            {
                                break;
                            }
                            flag4 = true;
                            flag5 = true;
                            num2++;
                        }
                    }
                    if (flag5)
                    {
                        y += num5;
                        num7 *= Math.Pow(10.0, -y);
                    }
                    else
                    {
                        y -= num5;
                        num7 *= Math.Pow(10.0, y);
                    }
                }
                if (double.IsInfinity(num7))
                {
                    throw new NotFiniteNumberException(num7);
                }
                if (flag3)
                {
                    num7 = -num7;
                }
                switch (ch)
                {
                    case '&':
                        if (num5 > 0)
                        {
                            throw new OverflowException();
                        }
                        return (int)Math.Round(num7);

                    case '@':
                        return Convert.ToDouble(new decimal(num7));

                    case '!':
                        return (float)num7;

                    case '%':
                        if (num5 > 0)
                        {
                            throw new OverflowException();
                        }
                        return (short)Math.Round(num7);
                }
                return num7;
            }
            public class NotFiniteNumberException : ArithmeticException
            {
                private double _offendingNumber;

                public NotFiniteNumberException()
                    : base("Number is not finite.")
                {
                    _offendingNumber = 0;
                    HResult = COR_E_NOTFINITENUMBER;
                }

                public NotFiniteNumberException(double offendingNumber)
                    : base()
                {
                    _offendingNumber = offendingNumber;
                    HResult = COR_E_NOTFINITENUMBER;
                }

                public NotFiniteNumberException(String message)
                    : base(message)
                {
                    _offendingNumber = 0;
                    HResult = COR_E_NOTFINITENUMBER;
                }

                public NotFiniteNumberException(String message, double offendingNumber)
                    : base(message)
                {
                    _offendingNumber = offendingNumber;
                    HResult = COR_E_NOTFINITENUMBER;
                }

                public NotFiniteNumberException(String message, Exception innerException)
                    : base(message, innerException)
                {
                    HResult = COR_E_NOTFINITENUMBER;
                }

                public NotFiniteNumberException(String message, double offendingNumber, Exception innerException)
                    : base(message, innerException)
                {
                    _offendingNumber = offendingNumber;
                    HResult = COR_E_NOTFINITENUMBER;
                }

                public double OffendingNumber
                {
                    get { return _offendingNumber; }
                }
                internal const int COR_E_NOTFINITENUMBER = unchecked((int)0x80131528);
            }
            private static double HexOrOctValue(string InputStr, int i)
            {
                long num4 = 0;
                int num5;
                int num2 = 0;
                int length = InputStr.Length;
                char ch = InputStr[i];
                i++;
                if ((ch != 'H') && (ch != 'h'))
                {
                    if ((ch != 'O') && (ch != 'o'))
                    {
                        return 0.0;
                    }
                    while ((i < length) && (num2 < 0x16))
                    {
                        ch = InputStr[i];
                        i++;
                        char ch3 = ch;
                        if ((((ch3 != '\t') && (ch3 != '\n')) && ((ch3 != '\r') && (ch3 != ' '))) && (ch3 != ' '))
                        {
                            if (ch3 == '0')
                            {
                                if (num2 == 0)
                                {
                                    continue;
                                }
                                num5 = 0;
                            }
                            else
                            {
                                if ((ch3 < '1') || (ch3 > '7'))
                                {
                                    break;
                                }
                                num5 = ch - '0';
                            }
                            if (num4 >= 0x1000000000000000L)
                            {
                                num4 = (num4 & 0xfffffffffffffffL) * 8L;
                                num4 |= 0x1000000000000000L;
                            }
                            else
                            {
                                num4 *= 8L;
                            }
                            num4 += num5;
                            num2++;
                        }
                    }
                }
                else
                {
                    while ((i < length) && (num2 < 0x11))
                    {
                        ch = InputStr[i];
                        i++;
                        char ch2 = ch;
                        if ((((ch2 != '\t') && (ch2 != '\n')) && ((ch2 != '\r') && (ch2 != ' '))) && (ch2 != ' '))
                        {
                            if (ch2 == '0')
                            {
                                if (num2 == 0)
                                {
                                    continue;
                                }
                                num5 = 0;
                            }
                            else if ((ch2 >= '1') && (ch2 <= '9'))
                            {
                                num5 = ch - '0';
                            }
                            else if ((ch2 >= 'A') && (ch2 <= 'F'))
                            {
                                num5 = ch - '7';
                            }
                            else
                            {
                                if ((ch2 < 'a') || (ch2 > 'f'))
                                {
                                    break;
                                }
                                num5 = ch - 'W';
                            }
                            if ((num2 == 15) && (num4 > 0x7ffffffffffffffL))
                            {
                                num4 = (num4 & 0x7ffffffffffffffL) * 0x10L;
                                num4 |= -9223372036854775808L;
                            }
                            else
                            {
                                num4 *= 0x10L;
                            }
                            num4 += num5;
                            num2++;
                        }
                    }
                    if (num2 == 0x10)
                    {
                        i++;
                        if (i < length)
                        {
                            ch = InputStr[i];
                        }
                    }
                    if (num2 <= 8)
                    {
                        if ((num2 > 4) || (ch == '&'))
                        {
                            if (num4 > 0x7fffffffL)
                            {
                                num4 = -2147483648L + (num4 & 0x7fffffffL);
                            }
                        }
                        else if (((num2 > 2) || (ch == '%')) && (num4 > 0x7fffL))
                        {
                            num4 = -32768L + (num4 & 0x7fffL);
                        }
                    }
                    switch (ch)
                    {
                        case '%':
                            num4 = (short)num4;
                            break;

                        case '&':
                            num4 = (int)num4;
                            break;
                    }
                    return (double)num4;
                }
                if (num2 == 0x16)
                {
                    i++;
                    if (i < length)
                    {
                        ch = InputStr[i];
                    }
                }
                if (num4 <= 0x100000000L)
                {
                    if ((num4 > 0xffffL) || (ch == '&'))
                    {
                        if (num4 > 0x7fffffffL)
                        {
                            num4 = -2147483648L + (num4 & 0x7fffffffL);
                        }
                    }
                    else if (((num4 > 0xffL) || (ch == '%')) && (num4 > 0x7fffL))
                    {
                        num4 = -32768L + (num4 & 0x7fffL);
                    }
                }
                switch (ch)
                {
                    case '%':
                        num4 = (short)num4;
                        break;

                    case '&':
                        num4 = (int)num4;
                        break;
                }
                return (double)num4;
            }

        }
    }
}