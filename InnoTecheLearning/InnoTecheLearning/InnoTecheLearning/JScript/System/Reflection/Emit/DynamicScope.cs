namespace System.Reflection.Emit
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Reflection;

    internal class DynamicScope
    {
        internal ArrayList m_tokens = new ArrayList();

        internal DynamicScope()
        {
            this.m_tokens.Add(null);
        }

        internal string GetString(int token) => 
            (this[token] as string);

        public int GetTokenFor(DynamicMethod method) => 
            (this.m_tokens.Add(method) | 0x6000000);

        internal int GetTokenFor(VarArgMethod varArgMethod) => 
            (this.m_tokens.Add(varArgMethod) | 0xa000000);

        public int GetTokenFor(RuntimeFieldHandle field) => 
            (this.m_tokens.Add(field) | 0x4000000);

        public int GetTokenFor(RuntimeMethodHandle method)
        {
            MethodBase methodBase = RuntimeType.GetMethodBase(method);
            if ((methodBase.DeclaringType != null) && methodBase.DeclaringType.IsGenericType)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_MethodDeclaringTypeGenericLcg"), new object[] { methodBase, methodBase.DeclaringType.GetGenericTypeDefinition() }));
            }
            return (this.m_tokens.Add(method) | 0x6000000);
        }

        public int GetTokenFor(RuntimeTypeHandle type) => 
            (this.m_tokens.Add(type) | 0x2000000);

        public int GetTokenFor(string literal) => 
            (this.m_tokens.Add(literal) | 0x70000000);

        public int GetTokenFor(byte[] signature) => 
            (this.m_tokens.Add(signature) | 0x11000000);

        public int GetTokenFor(RuntimeFieldHandle field, RuntimeTypeHandle typeContext) => 
            (this.m_tokens.Add(new GenericFieldInfo(field, typeContext)) | 0x4000000);

        public int GetTokenFor(RuntimeMethodHandle method, RuntimeTypeHandle typeContext) => 
            (this.m_tokens.Add(new GenericMethodInfo(method, typeContext)) | 0x6000000);

        internal byte[] ResolveSignature(int token, int fromMethod)
        {
            if (fromMethod == 0)
            {
                return (byte[]) this[token];
            }
            VarArgMethod method = this[token] as VarArgMethod;
            if (method == null)
            {
                return null;
            }
            return method.m_signature.GetSignature(true);
        }

        internal object this[int token]
        {
            get
            {
                token &= 0xffffff;
                if ((token >= 0) && (token <= this.m_tokens.Count))
                {
                    return this.m_tokens[token];
                }
                return null;
            }
        }
    }
}

