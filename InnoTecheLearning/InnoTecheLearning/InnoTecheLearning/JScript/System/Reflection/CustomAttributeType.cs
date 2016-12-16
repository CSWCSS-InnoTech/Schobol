namespace System.Reflection
{
    using System;
    using System.Runtime.InteropServices;

    [Serializable]
    internal struct CustomAttributeType
    {
        private CustomAttributeEncoding m_encodedArrayType;
        private CustomAttributeEncoding m_encodedEnumType;
        private CustomAttributeEncoding m_encodedType;
        private string m_enumName;
        private CustomAttributeEncoding m_padding;

        public CustomAttributeType(CustomAttributeEncoding encodedType, CustomAttributeEncoding encodedArrayType, CustomAttributeEncoding encodedEnumType, string enumName)
        {
            this.m_encodedType = encodedType;
            this.m_encodedArrayType = encodedArrayType;
            this.m_encodedEnumType = encodedEnumType;
            this.m_enumName = enumName;
            this.m_padding = this.m_encodedType;
        }

        public CustomAttributeEncoding EncodedArrayType =>
            this.m_encodedArrayType;

        public CustomAttributeEncoding EncodedEnumType =>
            this.m_encodedEnumType;

        public CustomAttributeEncoding EncodedType =>
            this.m_encodedType;

        [ComVisible(true)]
        public string EnumName =>
            this.m_enumName;
    }
}

