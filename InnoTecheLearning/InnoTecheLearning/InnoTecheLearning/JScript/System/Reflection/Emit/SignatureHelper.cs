namespace System.Reflection.Emit
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    [ComDefaultInterface(typeof(_SignatureHelper)), ComVisible(true), ClassInterface(ClassInterfaceType.None)]
    public sealed class SignatureHelper : _SignatureHelper
    {
        internal const byte ELEMENT_TYPE_ARRAY = 20;
        internal const byte ELEMENT_TYPE_BOOLEAN = 2;
        internal const byte ELEMENT_TYPE_BYREF = 0x10;
        internal const byte ELEMENT_TYPE_CHAR = 3;
        internal const byte ELEMENT_TYPE_CLASS = 0x12;
        internal const byte ELEMENT_TYPE_CMOD_OPT = 0x20;
        internal const byte ELEMENT_TYPE_CMOD_REQD = 0x1f;
        internal const byte ELEMENT_TYPE_END = 0;
        internal const byte ELEMENT_TYPE_FNPTR = 0x1b;
        internal const byte ELEMENT_TYPE_GENERICINST = 0x15;
        internal const byte ELEMENT_TYPE_I = 0x18;
        internal const byte ELEMENT_TYPE_I1 = 4;
        internal const byte ELEMENT_TYPE_I2 = 6;
        internal const byte ELEMENT_TYPE_I4 = 8;
        internal const byte ELEMENT_TYPE_I8 = 10;
        internal const byte ELEMENT_TYPE_INTERNAL = 0x21;
        internal const byte ELEMENT_TYPE_MAX = 0x22;
        internal const byte ELEMENT_TYPE_MVAR = 30;
        internal const byte ELEMENT_TYPE_OBJECT = 0x1c;
        internal const byte ELEMENT_TYPE_PINNED = 0x45;
        internal const byte ELEMENT_TYPE_PTR = 15;
        internal const byte ELEMENT_TYPE_R4 = 12;
        internal const byte ELEMENT_TYPE_R8 = 13;
        internal const byte ELEMENT_TYPE_SENTINEL = 0x41;
        internal const byte ELEMENT_TYPE_STRING = 14;
        internal const byte ELEMENT_TYPE_SZARRAY = 0x1d;
        internal const byte ELEMENT_TYPE_TYPEDBYREF = 0x16;
        internal const byte ELEMENT_TYPE_U = 0x19;
        internal const byte ELEMENT_TYPE_U1 = 5;
        internal const byte ELEMENT_TYPE_U2 = 7;
        internal const byte ELEMENT_TYPE_U4 = 9;
        internal const byte ELEMENT_TYPE_U8 = 11;
        internal const byte ELEMENT_TYPE_VALUETYPE = 0x11;
        internal const byte ELEMENT_TYPE_VAR = 0x13;
        internal const byte ELEMENT_TYPE_VOID = 1;
        internal const int IMAGE_CEE_CS_CALLCONV_DEFAULT = 0;
        internal const int IMAGE_CEE_CS_CALLCONV_FIELD = 6;
        internal const int IMAGE_CEE_CS_CALLCONV_GENERIC = 0x10;
        internal const int IMAGE_CEE_CS_CALLCONV_GENERICINST = 10;
        internal const int IMAGE_CEE_CS_CALLCONV_HASTHIS = 0x20;
        internal const int IMAGE_CEE_CS_CALLCONV_LOCAL_SIG = 7;
        internal const int IMAGE_CEE_CS_CALLCONV_MASK = 15;
        internal const int IMAGE_CEE_CS_CALLCONV_MAX = 11;
        internal const int IMAGE_CEE_CS_CALLCONV_PROPERTY = 8;
        internal const int IMAGE_CEE_CS_CALLCONV_RETPARAM = 0x40;
        internal const int IMAGE_CEE_CS_CALLCONV_UNMGD = 9;
        internal const int IMAGE_CEE_CS_CALLCONV_VARARG = 5;
        internal const int IMAGE_CEE_UNMANAGED_CALLCONV_C = 1;
        internal const int IMAGE_CEE_UNMANAGED_CALLCONV_FASTCALL = 4;
        internal const int IMAGE_CEE_UNMANAGED_CALLCONV_STDCALL = 2;
        internal const int IMAGE_CEE_UNMANAGED_CALLCONV_THISCALL = 3;
        private int m_argCount;
        private int m_currSig;
        private ModuleBuilder m_module;
        private bool m_sigDone;
        private byte[] m_signature;
        private int m_sizeLoc;
        internal const int mdtTypeDef = 0x2000000;
        internal const int mdtTypeRef = 0x1000000;
        internal const int mdtTypeSpec = 0x21000000;
        internal const int NO_SIZE_IN_SIG = -1;

        private SignatureHelper(Module mod, int callingConvention)
        {
            this.Init(mod, callingConvention);
        }

        private SignatureHelper(Module mod, Type type)
        {
            this.Init(mod);
            this.AddOneArgTypeHelper(type);
        }

        private SignatureHelper(Module mod, int callingConvention, Type returnType, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers) : this(mod, callingConvention, 0, returnType, requiredCustomModifiers, optionalCustomModifiers)
        {
        }

        private SignatureHelper(Module mod, int callingConvention, int cGenericParameters, Type returnType, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
        {
            this.Init(mod, callingConvention, cGenericParameters);
            if (callingConvention == 6)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_BadFieldSig"));
            }
            this.AddOneArgTypeHelper(returnType, requiredCustomModifiers, optionalCustomModifiers);
        }

        public void AddArgument(Type clsArgument)
        {
            this.AddArgument(clsArgument, null, null);
        }

        public void AddArgument(Type argument, bool pinned)
        {
            if (argument == null)
            {
                throw new ArgumentNullException("argument");
            }
            this.IncrementArgCounts();
            this.AddOneArgTypeHelper(argument, pinned);
        }

        public void AddArgument(Type argument, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
        {
            if (this.m_sigDone)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_SigIsFinalized"));
            }
            if (argument == null)
            {
                throw new ArgumentNullException("argument");
            }
            if (requiredCustomModifiers != null)
            {
                for (int i = 0; i < requiredCustomModifiers.Length; i++)
                {
                    Type type = requiredCustomModifiers[i];
                    if (type == null)
                    {
                        throw new ArgumentNullException("requiredCustomModifiers");
                    }
                    if (type.HasElementType)
                    {
                        throw new ArgumentException(Environment.GetResourceString("Argument_ArraysInvalid"), "requiredCustomModifiers");
                    }
                    if (type.ContainsGenericParameters)
                    {
                        throw new ArgumentException(Environment.GetResourceString("Argument_GenericsInvalid"), "requiredCustomModifiers");
                    }
                }
            }
            if (optionalCustomModifiers != null)
            {
                for (int j = 0; j < optionalCustomModifiers.Length; j++)
                {
                    Type type2 = optionalCustomModifiers[j];
                    if (type2 == null)
                    {
                        throw new ArgumentNullException("optionalCustomModifiers");
                    }
                    if (type2.HasElementType)
                    {
                        throw new ArgumentException(Environment.GetResourceString("Argument_ArraysInvalid"), "optionalCustomModifiers");
                    }
                    if (type2.ContainsGenericParameters)
                    {
                        throw new ArgumentException(Environment.GetResourceString("Argument_GenericsInvalid"), "optionalCustomModifiers");
                    }
                }
            }
            this.IncrementArgCounts();
            this.AddOneArgTypeHelper(argument, requiredCustomModifiers, optionalCustomModifiers);
        }

        public void AddArguments(Type[] arguments, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
        {
            if ((requiredCustomModifiers != null) && ((arguments == null) || (requiredCustomModifiers.Length != arguments.Length)))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_MismatchedArrays", new object[] { "requiredCustomModifiers", "arguments" }));
            }
            if ((optionalCustomModifiers != null) && ((arguments == null) || (optionalCustomModifiers.Length != arguments.Length)))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_MismatchedArrays", new object[] { "optionalCustomModifiers", "arguments" }));
            }
            if (arguments != null)
            {
                for (int i = 0; i < arguments.Length; i++)
                {
                    this.AddArgument(arguments[i], (requiredCustomModifiers == null) ? null : requiredCustomModifiers[i], (optionalCustomModifiers == null) ? null : optionalCustomModifiers[i]);
                }
            }
        }

        private void AddData(int data)
        {
            if ((this.m_currSig + 4) >= this.m_signature.Length)
            {
                this.m_signature = this.ExpandArray(this.m_signature);
            }
            if (data <= 0x7f)
            {
                this.m_signature[this.m_currSig++] = (byte) (data & 0xff);
            }
            else if (data <= 0x3fff)
            {
                this.m_signature[this.m_currSig++] = (byte) ((data >> 8) | 0x80);
                this.m_signature[this.m_currSig++] = (byte) (data & 0xff);
            }
            else
            {
                if (data > 0x1fffffff)
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_LargeInteger"));
                }
                this.m_signature[this.m_currSig++] = (byte) ((data >> 0x18) | 0xc0);
                this.m_signature[this.m_currSig++] = (byte) ((data >> 0x10) & 0xff);
                this.m_signature[this.m_currSig++] = (byte) ((data >> 8) & 0xff);
                this.m_signature[this.m_currSig++] = (byte) (data & 0xff);
            }
        }

        private void AddData(uint data)
        {
            if ((this.m_currSig + 4) >= this.m_signature.Length)
            {
                this.m_signature = this.ExpandArray(this.m_signature);
            }
            this.m_signature[this.m_currSig++] = (byte) (data & 0xff);
            this.m_signature[this.m_currSig++] = (byte) ((data >> 8) & 0xff);
            this.m_signature[this.m_currSig++] = (byte) ((data >> 0x10) & 0xff);
            this.m_signature[this.m_currSig++] = (byte) ((data >> 0x18) & 0xff);
        }

        private void AddData(ulong data)
        {
            if ((this.m_currSig + 8) >= this.m_signature.Length)
            {
                this.m_signature = this.ExpandArray(this.m_signature);
            }
            this.m_signature[this.m_currSig++] = (byte) (data & ((ulong) 0xffL));
            this.m_signature[this.m_currSig++] = (byte) ((data >> 8) & ((ulong) 0xffL));
            this.m_signature[this.m_currSig++] = (byte) ((data >> 0x10) & ((ulong) 0xffL));
            this.m_signature[this.m_currSig++] = (byte) ((data >> 0x18) & ((ulong) 0xffL));
            this.m_signature[this.m_currSig++] = (byte) ((data >> 0x20) & ((ulong) 0xffL));
            this.m_signature[this.m_currSig++] = (byte) ((data >> 40) & ((ulong) 0xffL));
            this.m_signature[this.m_currSig++] = (byte) ((data >> 0x30) & ((ulong) 0xffL));
            this.m_signature[this.m_currSig++] = (byte) ((data >> 0x38) & ((ulong) 0xffL));
        }

        private void AddElementType(int cvt)
        {
            if ((this.m_currSig + 1) >= this.m_signature.Length)
            {
                this.m_signature = this.ExpandArray(this.m_signature);
            }
            this.m_signature[this.m_currSig++] = (byte) cvt;
        }

        private void AddOneArgTypeHelper(Type clsArgument)
        {
            this.AddOneArgTypeHelperWorker(clsArgument, false);
        }

        private void AddOneArgTypeHelper(Type argument, bool pinned)
        {
            if (pinned)
            {
                this.AddElementType(0x45);
            }
            this.AddOneArgTypeHelper(argument);
        }

        private void AddOneArgTypeHelper(Type clsArgument, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers)
        {
            if (optionalCustomModifiers != null)
            {
                for (int i = 0; i < optionalCustomModifiers.Length; i++)
                {
                    this.AddElementType(0x20);
                    this.AddToken(this.m_module.GetTypeToken(optionalCustomModifiers[i]).Token);
                }
            }
            if (requiredCustomModifiers != null)
            {
                for (int j = 0; j < requiredCustomModifiers.Length; j++)
                {
                    this.AddElementType(0x1f);
                    this.AddToken(this.m_module.GetTypeToken(requiredCustomModifiers[j]).Token);
                }
            }
            this.AddOneArgTypeHelper(clsArgument);
        }

        private void AddOneArgTypeHelperWorker(Type clsArgument, bool lastWasGenericInst)
        {
            if (clsArgument.IsGenericParameter)
            {
                if (clsArgument.DeclaringMethod != null)
                {
                    this.AddData(30);
                }
                else
                {
                    this.AddData(0x13);
                }
                this.AddData(clsArgument.GenericParameterPosition);
            }
            else if (clsArgument.IsGenericType && (!clsArgument.IsGenericTypeDefinition || !lastWasGenericInst))
            {
                this.AddElementType(0x15);
                this.AddOneArgTypeHelperWorker(clsArgument.GetGenericTypeDefinition(), true);
                Type[] genericArguments = clsArgument.GetGenericArguments();
                this.AddData(genericArguments.Length);
                foreach (Type type in genericArguments)
                {
                    this.AddOneArgTypeHelper(type);
                }
            }
            else if (clsArgument is TypeBuilder)
            {
                TypeToken typeToken;
                TypeBuilder builder = (TypeBuilder) clsArgument;
                if (builder.Module.Equals(this.m_module))
                {
                    typeToken = builder.TypeToken;
                }
                else
                {
                    typeToken = this.m_module.GetTypeToken(clsArgument);
                }
                if (clsArgument.IsValueType)
                {
                    this.InternalAddTypeToken(typeToken, 0x11);
                }
                else
                {
                    this.InternalAddTypeToken(typeToken, 0x12);
                }
            }
            else if (clsArgument is EnumBuilder)
            {
                TypeToken token2;
                TypeBuilder typeBuilder = ((EnumBuilder) clsArgument).m_typeBuilder;
                if (typeBuilder.Module.Equals(this.m_module))
                {
                    token2 = typeBuilder.TypeToken;
                }
                else
                {
                    token2 = this.m_module.GetTypeToken(clsArgument);
                }
                if (clsArgument.IsValueType)
                {
                    this.InternalAddTypeToken(token2, 0x11);
                }
                else
                {
                    this.InternalAddTypeToken(token2, 0x12);
                }
            }
            else if (clsArgument.IsByRef)
            {
                this.AddElementType(0x10);
                clsArgument = clsArgument.GetElementType();
                this.AddOneArgTypeHelper(clsArgument);
            }
            else if (clsArgument.IsPointer)
            {
                this.AddElementType(15);
                this.AddOneArgTypeHelper(clsArgument.GetElementType());
            }
            else if (clsArgument.IsArray)
            {
                if (clsArgument.IsSzArray)
                {
                    this.AddElementType(0x1d);
                    this.AddOneArgTypeHelper(clsArgument.GetElementType());
                }
                else
                {
                    this.AddElementType(20);
                    this.AddOneArgTypeHelper(clsArgument.GetElementType());
                    this.AddData(clsArgument.GetArrayRank());
                    this.AddData(0);
                    this.AddData(0);
                }
            }
            else
            {
                RuntimeType cls = clsArgument as RuntimeType;
                int num = (cls != null) ? GetCorElementTypeFromClass(cls) : 0x22;
                if (IsSimpleType(num))
                {
                    this.AddElementType(num);
                }
                else if (clsArgument == typeof(object))
                {
                    this.AddElementType(0x1c);
                }
                else if (clsArgument == typeof(string))
                {
                    this.AddElementType(14);
                }
                else if (this.m_module == null)
                {
                    this.InternalAddRuntimeType(cls);
                }
                else if (clsArgument.IsValueType)
                {
                    this.InternalAddTypeToken(this.m_module.GetTypeToken(clsArgument), 0x11);
                }
                else
                {
                    this.InternalAddTypeToken(this.m_module.GetTypeToken(clsArgument), 0x12);
                }
            }
        }

        public void AddSentinel()
        {
            this.AddElementType(0x41);
        }

        private void AddToken(int token)
        {
            int data = token & 0xffffff;
            int num2 = token & -16777216;
            if (data > 0x3ffffff)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_LargeInteger"));
            }
            data = data << 2;
            switch (num2)
            {
                case 0x1000000:
                    data |= 1;
                    break;

                case 0x21000000:
                    data |= 2;
                    break;
            }
            this.AddData(data);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SignatureHelper))
            {
                return false;
            }
            SignatureHelper helper = (SignatureHelper) obj;
            if ((!helper.m_module.Equals(this.m_module) || (helper.m_currSig != this.m_currSig)) || ((helper.m_sizeLoc != this.m_sizeLoc) || (helper.m_sigDone != this.m_sigDone)))
            {
                return false;
            }
            for (int i = 0; i < this.m_currSig; i++)
            {
                if (this.m_signature[i] != helper.m_signature[i])
                {
                    return false;
                }
            }
            return true;
        }

        private byte[] ExpandArray(byte[] inArray) => 
            this.ExpandArray(inArray, inArray.Length * 2);

        private byte[] ExpandArray(byte[] inArray, int requiredLength)
        {
            if (requiredLength < inArray.Length)
            {
                requiredLength = inArray.Length * 2;
            }
            byte[] destinationArray = new byte[requiredLength];
            Array.Copy(inArray, destinationArray, inArray.Length);
            return destinationArray;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern int GetCorElementTypeFromClass(RuntimeType cls);
        public static SignatureHelper GetFieldSigHelper(Module mod) => 
            new SignatureHelper(mod, 6);

        public override int GetHashCode()
        {
            int num = (this.m_module.GetHashCode() + this.m_currSig) + this.m_sizeLoc;
            if (this.m_sigDone)
            {
                num++;
            }
            for (int i = 0; i < this.m_currSig; i++)
            {
                num += this.m_signature[i].GetHashCode();
            }
            return num;
        }

        public static SignatureHelper GetLocalVarSigHelper() => 
            GetLocalVarSigHelper(null);

        public static SignatureHelper GetLocalVarSigHelper(Module mod) => 
            new SignatureHelper(mod, 7);

        public static SignatureHelper GetMethodSigHelper(CallingConventions callingConvention, Type returnType) => 
            GetMethodSigHelper(null, callingConvention, returnType);

        public static SignatureHelper GetMethodSigHelper(CallingConvention unmanagedCallingConvention, Type returnType) => 
            GetMethodSigHelper(null, unmanagedCallingConvention, returnType);

        public static SignatureHelper GetMethodSigHelper(Module mod, CallingConventions callingConvention, Type returnType) => 
            GetMethodSigHelper(mod, callingConvention, returnType, null, null, null, null, null);

        public static SignatureHelper GetMethodSigHelper(Module mod, CallingConvention unmanagedCallConv, Type returnType)
        {
            int num;
            if (returnType == null)
            {
                returnType = typeof(void);
            }
            if (unmanagedCallConv == CallingConvention.Cdecl)
            {
                num = 1;
            }
            else if ((unmanagedCallConv == CallingConvention.StdCall) || (unmanagedCallConv == CallingConvention.Winapi))
            {
                num = 2;
            }
            else if (unmanagedCallConv == CallingConvention.ThisCall)
            {
                num = 3;
            }
            else
            {
                if (unmanagedCallConv != CallingConvention.FastCall)
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_UnknownUnmanagedCallConv"), "unmanagedCallConv");
                }
                num = 4;
            }
            return new SignatureHelper(mod, num, returnType, null, null);
        }

        public static SignatureHelper GetMethodSigHelper(Module mod, Type returnType, Type[] parameterTypes) => 
            GetMethodSigHelper(mod, CallingConventions.Standard, returnType, null, null, parameterTypes, null, null);

        internal static SignatureHelper GetMethodSigHelper(Module mod, CallingConventions callingConvention, Type returnType, int cGenericParam) => 
            GetMethodSigHelper(mod, callingConvention, cGenericParam, returnType, null, null, null, null, null);

        internal static SignatureHelper GetMethodSigHelper(Module scope, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers) => 
            GetMethodSigHelper(scope, callingConvention, 0, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);

        internal static SignatureHelper GetMethodSigHelper(Module scope, CallingConventions callingConvention, int cGenericParam, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
        {
            if (returnType == null)
            {
                returnType = typeof(void);
            }
            int num = 0;
            if ((callingConvention & CallingConventions.VarArgs) == CallingConventions.VarArgs)
            {
                num = 5;
            }
            if (cGenericParam > 0)
            {
                num |= 0x10;
            }
            if ((callingConvention & CallingConventions.HasThis) == CallingConventions.HasThis)
            {
                num |= 0x20;
            }
            SignatureHelper helper = new SignatureHelper(scope, num, cGenericParam, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers);
            helper.AddArguments(parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
            return helper;
        }

        internal static SignatureHelper GetMethodSpecSigHelper(Module scope, Type[] inst)
        {
            SignatureHelper helper = new SignatureHelper(scope, 10);
            helper.AddData(inst.Length);
            foreach (Type type in inst)
            {
                helper.AddArgument(type);
            }
            return helper;
        }

        public static SignatureHelper GetPropertySigHelper(Module mod, Type returnType, Type[] parameterTypes) => 
            GetPropertySigHelper(mod, returnType, null, null, parameterTypes, null, null);

        public static SignatureHelper GetPropertySigHelper(Module mod, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers) => 
            GetPropertySigHelper(mod, 0, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers, parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);

        public static SignatureHelper GetPropertySigHelper(Module mod, CallingConventions callingConvention, Type returnType, Type[] requiredReturnTypeCustomModifiers, Type[] optionalReturnTypeCustomModifiers, Type[] parameterTypes, Type[][] requiredParameterTypeCustomModifiers, Type[][] optionalParameterTypeCustomModifiers)
        {
            if (returnType == null)
            {
                returnType = typeof(void);
            }
            int num = 8;
            if ((callingConvention & CallingConventions.HasThis) == CallingConventions.HasThis)
            {
                num |= 0x20;
            }
            SignatureHelper helper = new SignatureHelper(mod, num, returnType, requiredReturnTypeCustomModifiers, optionalReturnTypeCustomModifiers);
            helper.AddArguments(parameterTypes, requiredParameterTypeCustomModifiers, optionalParameterTypeCustomModifiers);
            return helper;
        }

        public byte[] GetSignature() => 
            this.GetSignature(false);

        internal byte[] GetSignature(bool appendEndOfSig)
        {
            if (!this.m_sigDone)
            {
                if (appendEndOfSig)
                {
                    this.AddElementType(0);
                }
                this.SetNumberOfSignatureElements(true);
                this.m_sigDone = true;
            }
            if (this.m_signature.Length > this.m_currSig)
            {
                byte[] destinationArray = new byte[this.m_currSig];
                Array.Copy(this.m_signature, destinationArray, this.m_currSig);
                this.m_signature = destinationArray;
            }
            return this.m_signature;
        }

        internal static SignatureHelper GetTypeSigToken(Module mod, Type type)
        {
            if (mod == null)
            {
                throw new ArgumentNullException("module");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            return new SignatureHelper(mod, type);
        }

        private void IncrementArgCounts()
        {
            if (this.m_sizeLoc != -1)
            {
                this.m_argCount++;
            }
        }

        private void Init(Module mod)
        {
            this.m_signature = new byte[0x20];
            this.m_currSig = 0;
            this.m_module = mod as ModuleBuilder;
            this.m_argCount = 0;
            this.m_sigDone = false;
            this.m_sizeLoc = -1;
            if ((this.m_module == null) && (mod != null))
            {
                throw new ArgumentException(Environment.GetResourceString("NotSupported_MustBeModuleBuilder"));
            }
        }

        private void Init(Module mod, int callingConvention)
        {
            this.Init(mod, callingConvention, 0);
        }

        private void Init(Module mod, int callingConvention, int cGenericParam)
        {
            this.Init(mod);
            this.AddData(callingConvention);
            if ((callingConvention == 6) || (callingConvention == 10))
            {
                this.m_sizeLoc = -1;
            }
            else
            {
                if (cGenericParam > 0)
                {
                    this.AddData(cGenericParam);
                }
                this.m_sizeLoc = this.m_currSig++;
            }
        }

        private unsafe void InternalAddRuntimeType(Type type)
        {
            this.AddElementType(0x21);
            void* voidPtr = (void*) type.GetTypeHandleInternal().Value;
            if (sizeof(void*) == 4)
            {
                this.AddData((uint) voidPtr);
            }
            else
            {
                this.AddData((ulong) voidPtr);
            }
        }

        private void InternalAddTypeToken(TypeToken clsToken, int CorType)
        {
            this.AddElementType(CorType);
            this.AddToken(clsToken.Token);
        }

        internal byte[] InternalGetSignature(out int length)
        {
            if (!this.m_sigDone)
            {
                this.m_sigDone = true;
                this.SetNumberOfSignatureElements(false);
            }
            length = this.m_currSig;
            return this.m_signature;
        }

        internal byte[] InternalGetSignatureArray()
        {
            int argCount = this.m_argCount;
            int currSig = this.m_currSig;
            int num3 = currSig;
            if (argCount < 0x7f)
            {
                num3++;
            }
            else if (argCount < 0x3fff)
            {
                num3 += 2;
            }
            else
            {
                num3 += 4;
            }
            byte[] destinationArray = new byte[num3];
            int destinationIndex = 0;
            destinationArray[destinationIndex++] = this.m_signature[0];
            if (argCount <= 0x7f)
            {
                destinationArray[destinationIndex++] = (byte) (argCount & 0xff);
            }
            else if (argCount <= 0x3fff)
            {
                destinationArray[destinationIndex++] = (byte) ((argCount >> 8) | 0x80);
                destinationArray[destinationIndex++] = (byte) (argCount & 0xff);
            }
            else
            {
                if (argCount > 0x1fffffff)
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_LargeInteger"));
                }
                destinationArray[destinationIndex++] = (byte) ((argCount >> 0x18) | 0xc0);
                destinationArray[destinationIndex++] = (byte) ((argCount >> 0x10) & 0xff);
                destinationArray[destinationIndex++] = (byte) ((argCount >> 8) & 0xff);
                destinationArray[destinationIndex++] = (byte) (argCount & 0xff);
            }
            Array.Copy(this.m_signature, 2, destinationArray, destinationIndex, currSig - 2);
            destinationArray[num3 - 1] = 0;
            return destinationArray;
        }

        internal static bool IsSimpleType(int type)
        {
            if ((type > 14) && (((type != 0x16) && (type != 0x18)) && ((type != 0x19) && (type != 0x1c))))
            {
                return false;
            }
            return true;
        }

        private void SetNumberOfSignatureElements(bool forceCopy)
        {
            int currSig = this.m_currSig;
            if (this.m_sizeLoc != -1)
            {
                if ((this.m_argCount < 0x80) && !forceCopy)
                {
                    this.m_signature[this.m_sizeLoc] = (byte) this.m_argCount;
                }
                else
                {
                    int num;
                    if (this.m_argCount < 0x7f)
                    {
                        num = 1;
                    }
                    else if (this.m_argCount < 0x3fff)
                    {
                        num = 2;
                    }
                    else
                    {
                        num = 4;
                    }
                    byte[] destinationArray = new byte[(this.m_currSig + num) - 1];
                    destinationArray[0] = this.m_signature[0];
                    Array.Copy(this.m_signature, (int) (this.m_sizeLoc + 1), destinationArray, (int) (this.m_sizeLoc + num), (int) (currSig - (this.m_sizeLoc + 1)));
                    this.m_signature = destinationArray;
                    this.m_currSig = this.m_sizeLoc;
                    this.AddData(this.m_argCount);
                    this.m_currSig = currSig + (num - 1);
                }
            }
        }

        void _SignatureHelper.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
        {
            throw new NotImplementedException();
        }

        void _SignatureHelper.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
        {
            throw new NotImplementedException();
        }

        void _SignatureHelper.GetTypeInfoCount(out uint pcTInfo)
        {
            throw new NotImplementedException();
        }

        void _SignatureHelper.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Length: " + this.m_currSig + Environment.NewLine);
            if (this.m_sizeLoc != -1)
            {
                builder.Append("Arguments: " + this.m_signature[this.m_sizeLoc] + Environment.NewLine);
            }
            else
            {
                builder.Append("Field Signature" + Environment.NewLine);
            }
            builder.Append("Signature: " + Environment.NewLine);
            for (int i = 0; i <= this.m_currSig; i++)
            {
                builder.Append(this.m_signature[i] + "  ");
            }
            builder.Append(Environment.NewLine);
            return builder.ToString();
        }

        internal int ArgumentCount =>
            this.m_argCount;
    }
}

