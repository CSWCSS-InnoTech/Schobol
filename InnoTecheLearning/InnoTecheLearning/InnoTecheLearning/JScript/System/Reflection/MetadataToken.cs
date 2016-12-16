namespace System.Reflection
{
    using System;
    using System.Globalization;
    using System.Runtime.InteropServices;

    [Serializable, StructLayout(LayoutKind.Sequential)]
    internal struct MetadataToken
    {
        public int Value;
        public static implicit operator int(MetadataToken token) => 
            token.Value;

        public static implicit operator MetadataToken(int token) => 
            new MetadataToken(token);

        public static bool IsTokenOfType(int token, params MetadataTokenType[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if ((token & ((int) 0xff000000L)) == types[i])
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsNullToken(int token) => 
            ((token & 0xffffff) == 0);

        public MetadataToken(int token)
        {
            this.Value = token;
        }

        public bool IsGlobalTypeDefToken =>
            (this.Value == 0x2000001);
        public MetadataTokenType TokenType =>
            (((MetadataTokenType) this.Value) & ((MetadataTokenType) ((int) 0xff000000L)));
        public bool IsTypeRef =>
            (this.TokenType == MetadataTokenType.TypeRef);
        public bool IsTypeDef =>
            (this.TokenType == MetadataTokenType.TypeDef);
        public bool IsFieldDef =>
            (this.TokenType == MetadataTokenType.FieldDef);
        public bool IsMethodDef =>
            (this.TokenType == MetadataTokenType.MethodDef);
        public bool IsMemberRef =>
            (this.TokenType == MetadataTokenType.MemberRef);
        public bool IsEvent =>
            (this.TokenType == MetadataTokenType.Event);
        public bool IsProperty =>
            (this.TokenType == MetadataTokenType.Property);
        public bool IsParamDef =>
            (this.TokenType == MetadataTokenType.ParamDef);
        public bool IsTypeSpec =>
            (this.TokenType == MetadataTokenType.TypeSpec);
        public bool IsMethodSpec =>
            (this.TokenType == MetadataTokenType.MethodSpec);
        public bool IsString =>
            (this.TokenType == MetadataTokenType.String);
        public bool IsSignature =>
            (this.TokenType == MetadataTokenType.Signature);
        public override string ToString() => 
            string.Format(CultureInfo.InvariantCulture, "0x{0:x8}", new object[] { this.Value });
    }
}

