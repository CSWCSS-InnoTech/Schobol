namespace System.Security
{
    using System;
    using System.Collections;
    using System.Globalization;

    [Serializable]
    internal sealed class PermissionTokenKeyComparer : IEqualityComparer
    {
        private Comparer _caseSensitiveComparer;
        private TextInfo _info;

        public PermissionTokenKeyComparer(CultureInfo culture)
        {
            this._caseSensitiveComparer = new Comparer(culture);
            this._info = culture.TextInfo;
        }

        public int Compare(object a, object b)
        {
            string strLeft = a as string;
            string strRight = b as string;
            if ((strLeft == null) || (strRight == null))
            {
                return this._caseSensitiveComparer.Compare(a, b);
            }
            int num = this._caseSensitiveComparer.Compare(a, b);
            if (num == 0)
            {
                return 0;
            }
            if (SecurityManager._IsSameType(strLeft, strRight))
            {
                return 0;
            }
            return num;
        }

        public bool Equals(object a, object b) => 
            ((a == b) || (((a != null) && (b != null)) && (this.Compare(a, b) == 0)));

        public int GetHashCode(object obj)
        {
            string str = obj as string;
            if (str == null)
            {
                return obj.GetHashCode();
            }
            int index = str.IndexOf(',');
            if (index == -1)
            {
                index = str.Length;
            }
            int num2 = 0;
            for (int i = 0; i < index; i++)
            {
                num2 = ((num2 << 7) ^ str[i]) ^ (num2 >> 0x19);
            }
            return num2;
        }
    }
}

