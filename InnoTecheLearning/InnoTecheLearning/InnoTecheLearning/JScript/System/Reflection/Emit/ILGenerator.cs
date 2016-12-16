namespace System.Reflection.Emit
{
    using System;
    using System.Diagnostics.SymbolStore;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [ClassInterface(ClassInterfaceType.None), ComVisible(true), ComDefaultInterface(typeof(_ILGenerator))]
    public class ILGenerator : _ILGenerator
    {
        internal const int DefaultExceptionArraySize = 8;
        internal const int DefaultFixupArraySize = 0x40;
        internal const int DefaultLabelArraySize = 0x10;
        internal const int defaultSize = 0x10;
        internal __ExceptionInfo[] m_currExcStack;
        internal int m_currExcStackCount;
        internal int m_exceptionCount;
        internal __ExceptionInfo[] m_exceptions;
        internal int m_fixupCount;
        internal __FixupData[] m_fixupData;
        internal byte[] m_ILStream;
        internal int m_labelCount;
        internal int[] m_labelList;
        internal int m_length;
        internal LineNumberInfo m_LineNumberInfo;
        internal int m_localCount;
        internal SignatureHelper m_localSignature;
        internal int m_maxMidStack;
        internal int m_maxMidStackCur;
        internal int m_maxStackSize;
        internal MethodInfo m_methodBuilder;
        internal int m_RelocFixupCount;
        internal int[] m_RelocFixupList;
        internal int m_RVAFixupCount;
        internal int[] m_RVAFixupList;
        internal ScopeTree m_ScopeTree;
        internal const byte PrefixInstruction = 0xff;

        internal ILGenerator(int size)
        {
            if (size < 0x10)
            {
                this.m_ILStream = new byte[0x10];
            }
            else
            {
                this.m_ILStream = new byte[size];
            }
            this.m_length = 0;
            this.m_labelCount = 0;
            this.m_fixupCount = 0;
            this.m_labelList = null;
            this.m_fixupData = null;
            this.m_exceptions = null;
            this.m_exceptionCount = 0;
            this.m_currExcStack = null;
            this.m_currExcStackCount = 0;
            this.m_RelocFixupList = new int[0x40];
            this.m_RelocFixupCount = 0;
            this.m_RVAFixupList = new int[0x40];
            this.m_RVAFixupCount = 0;
            this.m_ScopeTree = new ScopeTree();
            this.m_LineNumberInfo = new LineNumberInfo();
            this.m_methodBuilder = null;
            this.m_localCount = 0;
            this.m_localSignature = SignatureHelper.GetLocalVarSigHelper(null);
        }

        internal ILGenerator(MethodInfo methodBuilder) : this(methodBuilder, 0x40)
        {
        }

        internal ILGenerator(MethodInfo methodBuilder, int size)
        {
            if (size < 0x10)
            {
                this.m_ILStream = new byte[0x10];
            }
            else
            {
                this.m_ILStream = new byte[size];
            }
            this.m_length = 0;
            this.m_labelCount = 0;
            this.m_fixupCount = 0;
            this.m_labelList = null;
            this.m_fixupData = null;
            this.m_exceptions = null;
            this.m_exceptionCount = 0;
            this.m_currExcStack = null;
            this.m_currExcStackCount = 0;
            this.m_RelocFixupList = new int[0x40];
            this.m_RelocFixupCount = 0;
            this.m_RVAFixupList = new int[0x40];
            this.m_RVAFixupCount = 0;
            this.m_ScopeTree = new ScopeTree();
            this.m_LineNumberInfo = new LineNumberInfo();
            this.m_methodBuilder = methodBuilder;
            this.m_localCount = 0;
            MethodBuilder builder = this.m_methodBuilder as MethodBuilder;
            if (builder == null)
            {
                this.m_localSignature = SignatureHelper.GetLocalVarSigHelper(null);
            }
            else
            {
                this.m_localSignature = SignatureHelper.GetLocalVarSigHelper(builder.GetTypeBuilder().Module);
            }
        }

        internal virtual void AddFixup(Label lbl, int pos, int instSize)
        {
            if (this.m_fixupData == null)
            {
                this.m_fixupData = new __FixupData[0x40];
            }
            if (this.m_fixupCount >= this.m_fixupData.Length)
            {
                this.m_fixupData = EnlargeArray(this.m_fixupData);
            }
            this.m_fixupData[this.m_fixupCount].m_fixupPos = pos;
            this.m_fixupData[this.m_fixupCount].m_fixupLabel = lbl;
            this.m_fixupData[this.m_fixupCount].m_fixupInstSize = instSize;
            this.m_fixupCount++;
        }

        internal virtual byte[] BakeByteArray()
        {
            if (this.m_currExcStackCount != 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_UnclosedExceptionBlock"));
            }
            if (this.m_length == 0)
            {
                return null;
            }
            int length = this.m_length;
            byte[] destinationArray = new byte[length];
            Array.Copy(this.m_ILStream, destinationArray, length);
            for (int i = 0; i < this.m_fixupCount; i++)
            {
                int num2 = this.GetLabelPos(this.m_fixupData[i].m_fixupLabel) - (this.m_fixupData[i].m_fixupPos + this.m_fixupData[i].m_fixupInstSize);
                if (this.m_fixupData[i].m_fixupInstSize == 1)
                {
                    if ((num2 < -128) || (num2 > 0x7f))
                    {
                        throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("NotSupported_IllegalOneByteBranch"), new object[] { this.m_fixupData[i].m_fixupPos, num2 }));
                    }
                    if (num2 < 0)
                    {
                        destinationArray[this.m_fixupData[i].m_fixupPos] = (byte) (0x100 + num2);
                    }
                    else
                    {
                        destinationArray[this.m_fixupData[i].m_fixupPos] = (byte) num2;
                    }
                }
                else
                {
                    this.PutInteger4(num2, this.m_fixupData[i].m_fixupPos, destinationArray);
                }
            }
            return destinationArray;
        }

        public virtual void BeginCatchBlock(Type exceptionType)
        {
            if (this.m_currExcStackCount == 0)
            {
                throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
            }
            __ExceptionInfo info = this.m_currExcStack[this.m_currExcStackCount - 1];
            if (info.GetCurrentState() == 1)
            {
                if (exceptionType != null)
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_ShouldNotSpecifyExceptionType"));
                }
                this.Emit(OpCodes.Endfilter);
            }
            else
            {
                if (exceptionType == null)
                {
                    throw new ArgumentNullException("exceptionType");
                }
                Label endLabel = info.GetEndLabel();
                this.Emit(OpCodes.Leave, endLabel);
            }
            info.MarkCatchAddr(this.m_length, exceptionType);
        }

        public virtual void BeginExceptFilterBlock()
        {
            if (this.m_currExcStackCount == 0)
            {
                throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
            }
            __ExceptionInfo info = this.m_currExcStack[this.m_currExcStackCount - 1];
            Label endLabel = info.GetEndLabel();
            this.Emit(OpCodes.Leave, endLabel);
            info.MarkFilterAddr(this.m_length);
        }

        public virtual Label BeginExceptionBlock()
        {
            if (this.m_exceptions == null)
            {
                this.m_exceptions = new __ExceptionInfo[8];
            }
            if (this.m_currExcStack == null)
            {
                this.m_currExcStack = new __ExceptionInfo[8];
            }
            if (this.m_exceptionCount >= this.m_exceptions.Length)
            {
                this.m_exceptions = EnlargeArray(this.m_exceptions);
            }
            if (this.m_currExcStackCount >= this.m_currExcStack.Length)
            {
                this.m_currExcStack = EnlargeArray(this.m_currExcStack);
            }
            Label endLabel = this.DefineLabel();
            __ExceptionInfo info = new __ExceptionInfo(this.m_length, endLabel);
            this.m_exceptions[this.m_exceptionCount++] = info;
            this.m_currExcStack[this.m_currExcStackCount++] = info;
            return endLabel;
        }

        public virtual void BeginFaultBlock()
        {
            if (this.m_currExcStackCount == 0)
            {
                throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
            }
            __ExceptionInfo info = this.m_currExcStack[this.m_currExcStackCount - 1];
            Label endLabel = info.GetEndLabel();
            this.Emit(OpCodes.Leave, endLabel);
            info.MarkFaultAddr(this.m_length);
        }

        public virtual void BeginFinallyBlock()
        {
            if (this.m_currExcStackCount == 0)
            {
                throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
            }
            __ExceptionInfo info = this.m_currExcStack[this.m_currExcStackCount - 1];
            int currentState = info.GetCurrentState();
            Label endLabel = info.GetEndLabel();
            int endCatchAddr = 0;
            if (currentState != 0)
            {
                this.Emit(OpCodes.Leave, endLabel);
                endCatchAddr = this.m_length;
            }
            this.MarkLabel(endLabel);
            Label lbl = this.DefineLabel();
            info.SetFinallyEndLabel(lbl);
            this.Emit(OpCodes.Leave, lbl);
            if (endCatchAddr == 0)
            {
                endCatchAddr = this.m_length;
            }
            info.MarkFinallyAddr(this.m_length, endCatchAddr);
        }

        public virtual void BeginScope()
        {
            this.m_ScopeTree.AddScopeInfo(ScopeAction.Open, this.m_length);
        }

        internal static int CalculateNumberOfExceptions(__ExceptionInfo[] excp)
        {
            int num = 0;
            if (excp == null)
            {
                return 0;
            }
            for (int i = 0; i < excp.Length; i++)
            {
                num += excp[i].GetNumberOfCatches();
            }
            return num;
        }

        public virtual LocalBuilder DeclareLocal(Type localType) => 
            this.DeclareLocal(localType, false);

        public virtual LocalBuilder DeclareLocal(Type localType, bool pinned)
        {
            MethodBuilder methodBuilder = this.m_methodBuilder as MethodBuilder;
            if (methodBuilder == null)
            {
                throw new NotSupportedException();
            }
            if (methodBuilder.IsTypeCreated())
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_TypeHasBeenCreated"));
            }
            if (localType == null)
            {
                throw new ArgumentNullException("localType");
            }
            if (methodBuilder.m_bIsBaked)
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_MethodBaked"));
            }
            this.m_localSignature.AddArgument(localType, pinned);
            LocalBuilder builder = new LocalBuilder(this.m_localCount, localType, methodBuilder, pinned);
            this.m_localCount++;
            return builder;
        }

        public virtual Label DefineLabel()
        {
            if (this.m_labelList == null)
            {
                this.m_labelList = new int[0x10];
            }
            if (this.m_labelCount >= this.m_labelList.Length)
            {
                this.m_labelList = EnlargeArray(this.m_labelList);
            }
            this.m_labelList[this.m_labelCount] = -1;
            return new Label(this.m_labelCount++);
        }

        public virtual void Emit(OpCode opcode)
        {
            this.EnsureCapacity(3);
            this.InternalEmit(opcode);
        }

        public virtual void Emit(OpCode opcode, byte arg)
        {
            this.EnsureCapacity(4);
            this.InternalEmit(opcode);
            this.m_ILStream[this.m_length++] = arg;
        }

        public virtual unsafe void Emit(OpCode opcode, double arg)
        {
            this.EnsureCapacity(11);
            this.InternalEmit(opcode);
            ulong num = *((ulong*) &arg);
            this.m_ILStream[this.m_length++] = (byte) num;
            this.m_ILStream[this.m_length++] = (byte) (num >> 8);
            this.m_ILStream[this.m_length++] = (byte) (num >> 0x10);
            this.m_ILStream[this.m_length++] = (byte) (num >> 0x18);
            this.m_ILStream[this.m_length++] = (byte) (num >> 0x20);
            this.m_ILStream[this.m_length++] = (byte) (num >> 40);
            this.m_ILStream[this.m_length++] = (byte) (num >> 0x30);
            this.m_ILStream[this.m_length++] = (byte) (num >> 0x38);
        }

        public virtual void Emit(OpCode opcode, short arg)
        {
            this.EnsureCapacity(5);
            this.InternalEmit(opcode);
            this.m_ILStream[this.m_length++] = (byte) arg;
            this.m_ILStream[this.m_length++] = (byte) (arg >> 8);
        }

        public virtual void Emit(OpCode opcode, int arg)
        {
            this.EnsureCapacity(7);
            this.InternalEmit(opcode);
            this.m_length = this.PutInteger4(arg, this.m_length, this.m_ILStream);
        }

        public virtual void Emit(OpCode opcode, long arg)
        {
            this.EnsureCapacity(11);
            this.InternalEmit(opcode);
            this.m_ILStream[this.m_length++] = (byte) arg;
            this.m_ILStream[this.m_length++] = (byte) (arg >> 8);
            this.m_ILStream[this.m_length++] = (byte) (arg >> 0x10);
            this.m_ILStream[this.m_length++] = (byte) (arg >> 0x18);
            this.m_ILStream[this.m_length++] = (byte) (arg >> 0x20);
            this.m_ILStream[this.m_length++] = (byte) (arg >> 40);
            this.m_ILStream[this.m_length++] = (byte) (arg >> 0x30);
            this.m_ILStream[this.m_length++] = (byte) (arg >> 0x38);
        }

        [ComVisible(true)]
        public virtual void Emit(OpCode opcode, ConstructorInfo con)
        {
            int stackchange = 0;
            int methodToken = this.GetMethodToken(con, null);
            this.EnsureCapacity(7);
            this.InternalEmit(opcode);
            if (opcode.m_push == StackBehaviour.Varpush)
            {
                stackchange++;
            }
            if ((opcode.m_pop == StackBehaviour.Varpop) && (con.GetParameterTypes() != null))
            {
                stackchange -= con.GetParameterTypes().Length;
            }
            this.UpdateStackSize(opcode, stackchange);
            this.RecordTokenFixup();
            this.m_length = this.PutInteger4(methodToken, this.m_length, this.m_ILStream);
        }

        public virtual void Emit(OpCode opcode, Label label)
        {
            label.GetLabelValue();
            this.EnsureCapacity(7);
            this.InternalEmit(opcode);
            if (OpCodes.TakesSingleByteArgument(opcode))
            {
                this.AddFixup(label, this.m_length, 1);
                this.m_length++;
            }
            else
            {
                this.AddFixup(label, this.m_length, 4);
                this.m_length += 4;
            }
        }

        public virtual void Emit(OpCode opcode, LocalBuilder local)
        {
            if (local == null)
            {
                throw new ArgumentNullException("local");
            }
            int localIndex = local.GetLocalIndex();
            if (local.GetMethodBuilder() != this.m_methodBuilder)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_UnmatchedMethodForLocal"), "local");
            }
            if (opcode.Equals(OpCodes.Ldloc))
            {
                switch (localIndex)
                {
                    case 0:
                        opcode = OpCodes.Ldloc_0;
                        goto Label_011E;

                    case 1:
                        opcode = OpCodes.Ldloc_1;
                        goto Label_011E;

                    case 2:
                        opcode = OpCodes.Ldloc_2;
                        goto Label_011E;

                    case 3:
                        opcode = OpCodes.Ldloc_3;
                        goto Label_011E;
                }
                if (localIndex <= 0xff)
                {
                    opcode = OpCodes.Ldloc_S;
                }
            }
            else if (opcode.Equals(OpCodes.Stloc))
            {
                switch (localIndex)
                {
                    case 0:
                        opcode = OpCodes.Stloc_0;
                        goto Label_011E;

                    case 1:
                        opcode = OpCodes.Stloc_1;
                        goto Label_011E;

                    case 2:
                        opcode = OpCodes.Stloc_2;
                        goto Label_011E;

                    case 3:
                        opcode = OpCodes.Stloc_3;
                        goto Label_011E;
                }
                if (localIndex <= 0xff)
                {
                    opcode = OpCodes.Stloc_S;
                }
            }
            else if (opcode.Equals(OpCodes.Ldloca) && (localIndex <= 0xff))
            {
                opcode = OpCodes.Ldloca_S;
            }
        Label_011E:
            this.EnsureCapacity(7);
            this.InternalEmit(opcode);
            if (opcode.OperandType != OperandType.InlineNone)
            {
                if (!OpCodes.TakesSingleByteArgument(opcode))
                {
                    this.m_ILStream[this.m_length++] = (byte) localIndex;
                    this.m_ILStream[this.m_length++] = (byte) (localIndex >> 8);
                }
                else
                {
                    if (localIndex > 0xff)
                    {
                        throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_BadInstructionOrIndexOutOfBound"));
                    }
                    this.m_ILStream[this.m_length++] = (byte) localIndex;
                }
            }
        }

        public virtual void Emit(OpCode opcode, SignatureHelper signature)
        {
            int stackchange = 0;
            ModuleBuilder module = (ModuleBuilder) this.m_methodBuilder.Module;
            if (signature == null)
            {
                throw new ArgumentNullException("signature");
            }
            int token = module.GetSignatureToken(signature).Token;
            this.EnsureCapacity(7);
            this.InternalEmit(opcode);
            if (opcode.m_pop == StackBehaviour.Varpop)
            {
                stackchange -= signature.ArgumentCount;
                stackchange--;
                this.UpdateStackSize(opcode, stackchange);
            }
            this.RecordTokenFixup();
            this.m_length = this.PutInteger4(token, this.m_length, this.m_ILStream);
        }

        public virtual void Emit(OpCode opcode, FieldInfo field)
        {
            ModuleBuilder module = (ModuleBuilder) this.m_methodBuilder.Module;
            int token = module.GetFieldToken(field).Token;
            this.EnsureCapacity(7);
            this.InternalEmit(opcode);
            this.RecordTokenFixup();
            this.m_length = this.PutInteger4(token, this.m_length, this.m_ILStream);
        }

        public virtual void Emit(OpCode opcode, MethodInfo meth)
        {
            if ((opcode.Equals(OpCodes.Call) || opcode.Equals(OpCodes.Callvirt)) || opcode.Equals(OpCodes.Newobj))
            {
                this.EmitCall(opcode, meth, null);
            }
            else
            {
                int stackchange = 0;
                if (meth == null)
                {
                    throw new ArgumentNullException("meth");
                }
                int methodToken = this.GetMethodToken(meth, null);
                this.EnsureCapacity(7);
                this.InternalEmit(opcode);
                this.UpdateStackSize(opcode, stackchange);
                this.RecordTokenFixup();
                this.m_length = this.PutInteger4(methodToken, this.m_length, this.m_ILStream);
            }
        }

        [CLSCompliant(false)]
        public void Emit(OpCode opcode, sbyte arg)
        {
            this.EnsureCapacity(4);
            this.InternalEmit(opcode);
            if (arg < 0)
            {
                this.m_ILStream[this.m_length++] = (byte) (0x100 + arg);
            }
            else
            {
                this.m_ILStream[this.m_length++] = (byte) arg;
            }
        }

        public virtual unsafe void Emit(OpCode opcode, float arg)
        {
            this.EnsureCapacity(7);
            this.InternalEmit(opcode);
            uint num = *((uint*) &arg);
            this.m_ILStream[this.m_length++] = (byte) num;
            this.m_ILStream[this.m_length++] = (byte) (num >> 8);
            this.m_ILStream[this.m_length++] = (byte) (num >> 0x10);
            this.m_ILStream[this.m_length++] = (byte) (num >> 0x18);
        }

        public virtual void Emit(OpCode opcode, Type cls)
        {
            int token = 0;
            ModuleBuilder module = (ModuleBuilder) this.m_methodBuilder.Module;
            if (((opcode == OpCodes.Ldtoken) && (cls != null)) && cls.IsGenericTypeDefinition)
            {
                token = module.GetTypeToken(cls).Token;
            }
            else
            {
                token = module.GetTypeTokenInternal(cls).Token;
            }
            this.EnsureCapacity(7);
            this.InternalEmit(opcode);
            this.RecordTokenFixup();
            this.m_length = this.PutInteger4(token, this.m_length, this.m_ILStream);
        }

        public virtual void Emit(OpCode opcode, Label[] labels)
        {
            int length = labels.Length;
            this.EnsureCapacity((length * 4) + 7);
            this.InternalEmit(opcode);
            this.m_length = this.PutInteger4(length, this.m_length, this.m_ILStream);
            int instSize = length * 4;
            for (int i = 0; instSize > 0; i++)
            {
                this.AddFixup(labels[i], this.m_length, instSize);
                this.m_length += 4;
                instSize -= 4;
            }
        }

        public virtual void Emit(OpCode opcode, string str)
        {
            ModuleBuilder module = (ModuleBuilder) this.m_methodBuilder.Module;
            int token = module.GetStringConstant(str).Token;
            this.EnsureCapacity(7);
            this.InternalEmit(opcode);
            this.m_length = this.PutInteger4(token, this.m_length, this.m_ILStream);
        }

        public virtual void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
        {
            int stackchange = 0;
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }
            int methodToken = this.GetMethodToken(methodInfo, optionalParameterTypes);
            this.EnsureCapacity(7);
            this.InternalEmit(opcode);
            if (methodInfo.GetReturnType() != typeof(void))
            {
                stackchange++;
            }
            if (methodInfo.GetParameterTypes() != null)
            {
                stackchange -= methodInfo.GetParameterTypes().Length;
            }
            if ((!(methodInfo is SymbolMethod) && !methodInfo.IsStatic) && !opcode.Equals(OpCodes.Newobj))
            {
                stackchange--;
            }
            if (optionalParameterTypes != null)
            {
                stackchange -= optionalParameterTypes.Length;
            }
            this.UpdateStackSize(opcode, stackchange);
            this.RecordTokenFixup();
            this.m_length = this.PutInteger4(methodToken, this.m_length, this.m_ILStream);
        }

        public virtual void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
        {
            int stackchange = 0;
            int length = 0;
            ModuleBuilder module = (ModuleBuilder) this.m_methodBuilder.Module;
            if (parameterTypes != null)
            {
                length = parameterTypes.Length;
            }
            SignatureHelper sigHelper = SignatureHelper.GetMethodSigHelper(module, unmanagedCallConv, returnType);
            if (parameterTypes != null)
            {
                for (int i = 0; i < length; i++)
                {
                    sigHelper.AddArgument(parameterTypes[i]);
                }
            }
            if (returnType != typeof(void))
            {
                stackchange++;
            }
            if (parameterTypes != null)
            {
                stackchange -= length;
            }
            stackchange--;
            this.UpdateStackSize(opcode, stackchange);
            this.EnsureCapacity(7);
            this.Emit(OpCodes.Calli);
            this.RecordTokenFixup();
            this.m_length = this.PutInteger4(module.GetSignatureToken(sigHelper).Token, this.m_length, this.m_ILStream);
        }

        public virtual void EmitCalli(OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
        {
            int stackchange = 0;
            if ((optionalParameterTypes != null) && ((callingConvention & CallingConventions.VarArgs) == 0))
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAVarArgCallingConvention"));
            }
            ModuleBuilder module = (ModuleBuilder) this.m_methodBuilder.Module;
            SignatureHelper sigHelper = this.GetMemberRefSignature(callingConvention, returnType, parameterTypes, optionalParameterTypes);
            this.EnsureCapacity(7);
            this.Emit(OpCodes.Calli);
            if (returnType != typeof(void))
            {
                stackchange++;
            }
            if (parameterTypes != null)
            {
                stackchange -= parameterTypes.Length;
            }
            if (optionalParameterTypes != null)
            {
                stackchange -= optionalParameterTypes.Length;
            }
            if ((callingConvention & CallingConventions.HasThis) == CallingConventions.HasThis)
            {
                stackchange--;
            }
            stackchange--;
            this.UpdateStackSize(opcode, stackchange);
            this.RecordTokenFixup();
            this.m_length = this.PutInteger4(module.GetSignatureToken(sigHelper).Token, this.m_length, this.m_ILStream);
        }

        public virtual void EmitWriteLine(LocalBuilder localBuilder)
        {
            if (this.m_methodBuilder == null)
            {
                throw new ArgumentException(Environment.GetResourceString("InvalidOperation_BadILGeneratorUsage"));
            }
            MethodInfo method = typeof(Console).GetMethod("get_Out");
            this.Emit(OpCodes.Call, method);
            this.Emit(OpCodes.Ldloc, localBuilder);
            Type[] types = new Type[1];
            object localType = localBuilder.LocalType;
            if ((localType is TypeBuilder) || (localType is EnumBuilder))
            {
                throw new ArgumentException(Environment.GetResourceString("NotSupported_OutputStreamUsingTypeBuilder"));
            }
            types[0] = (Type) localType;
            MethodInfo meth = typeof(TextWriter).GetMethod("WriteLine", types);
            if (meth == null)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmitWriteLineType"), "localBuilder");
            }
            this.Emit(OpCodes.Callvirt, meth);
        }

        public virtual void EmitWriteLine(FieldInfo fld)
        {
            if (fld == null)
            {
                throw new ArgumentNullException("fld");
            }
            MethodInfo method = typeof(Console).GetMethod("get_Out");
            this.Emit(OpCodes.Call, method);
            if ((fld.Attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope)
            {
                this.Emit(OpCodes.Ldsfld, fld);
            }
            else
            {
                this.Emit(OpCodes.Ldarg, (short) 0);
                this.Emit(OpCodes.Ldfld, fld);
            }
            Type[] types = new Type[1];
            object fieldType = fld.FieldType;
            if ((fieldType is TypeBuilder) || (fieldType is EnumBuilder))
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_OutputStreamUsingTypeBuilder"));
            }
            types[0] = (Type) fieldType;
            MethodInfo meth = typeof(TextWriter).GetMethod("WriteLine", types);
            if (meth == null)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmitWriteLineType"), "fld");
            }
            this.Emit(OpCodes.Callvirt, meth);
        }

        public virtual void EmitWriteLine(string value)
        {
            this.Emit(OpCodes.Ldstr, value);
            Type[] types = new Type[] { typeof(string) };
            MethodInfo method = typeof(Console).GetMethod("WriteLine", types);
            this.Emit(OpCodes.Call, method);
        }

        public virtual void EndExceptionBlock()
        {
            if (this.m_currExcStackCount == 0)
            {
                throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
            }
            __ExceptionInfo info = this.m_currExcStack[this.m_currExcStackCount - 1];
            this.m_currExcStack[this.m_currExcStackCount - 1] = null;
            this.m_currExcStackCount--;
            Label endLabel = info.GetEndLabel();
            switch (info.GetCurrentState())
            {
                case 1:
                case 0:
                    throw new InvalidOperationException(Environment.GetResourceString("Argument_BadExceptionCodeGen"));

                case 2:
                    this.Emit(OpCodes.Leave, endLabel);
                    break;

                case 3:
                case 4:
                    this.Emit(OpCodes.Endfinally);
                    break;
            }
            if (this.m_labelList[endLabel.GetLabelValue()] == -1)
            {
                this.MarkLabel(endLabel);
            }
            else
            {
                this.MarkLabel(info.GetFinallyEndLabel());
            }
            info.Done(this.m_length);
        }

        public virtual void EndScope()
        {
            this.m_ScopeTree.AddScopeInfo(ScopeAction.Close, this.m_length);
        }

        internal static byte[] EnlargeArray(byte[] incoming)
        {
            byte[] destinationArray = new byte[incoming.Length * 2];
            Array.Copy(incoming, destinationArray, incoming.Length);
            return destinationArray;
        }

        internal static int[] EnlargeArray(int[] incoming)
        {
            int[] destinationArray = new int[incoming.Length * 2];
            Array.Copy(incoming, destinationArray, incoming.Length);
            return destinationArray;
        }

        internal static __ExceptionInfo[] EnlargeArray(__ExceptionInfo[] incoming)
        {
            __ExceptionInfo[] destinationArray = new __ExceptionInfo[incoming.Length * 2];
            Array.Copy(incoming, destinationArray, incoming.Length);
            return destinationArray;
        }

        internal static __FixupData[] EnlargeArray(__FixupData[] incoming)
        {
            __FixupData[] destinationArray = new __FixupData[incoming.Length * 2];
            Array.Copy(incoming, destinationArray, incoming.Length);
            return destinationArray;
        }

        internal static Type[] EnlargeArray(Type[] incoming)
        {
            Type[] destinationArray = new Type[incoming.Length * 2];
            Array.Copy(incoming, destinationArray, incoming.Length);
            return destinationArray;
        }

        internal static byte[] EnlargeArray(byte[] incoming, int requiredSize)
        {
            byte[] destinationArray = new byte[requiredSize];
            Array.Copy(incoming, destinationArray, incoming.Length);
            return destinationArray;
        }

        internal virtual void EnsureCapacity(int size)
        {
            if ((this.m_length + size) >= this.m_ILStream.Length)
            {
                if ((this.m_length + size) >= (2 * this.m_ILStream.Length))
                {
                    this.m_ILStream = EnlargeArray(this.m_ILStream, this.m_length + size);
                }
                else
                {
                    this.m_ILStream = EnlargeArray(this.m_ILStream);
                }
            }
        }

        internal virtual __ExceptionInfo[] GetExceptions()
        {
            if (this.m_currExcStackCount != 0)
            {
                throw new NotSupportedException(Environment.GetResourceString("Argument_UnclosedExceptionBlock"));
            }
            if (this.m_exceptionCount == 0)
            {
                return null;
            }
            __ExceptionInfo[] destinationArray = new __ExceptionInfo[this.m_exceptionCount];
            Array.Copy(this.m_exceptions, destinationArray, this.m_exceptionCount);
            this.SortExceptions(destinationArray);
            return destinationArray;
        }

        internal virtual int GetLabelPos(Label lbl)
        {
            int labelValue = lbl.GetLabelValue();
            if ((labelValue < 0) || (labelValue >= this.m_labelCount))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_BadLabel"));
            }
            if (this.m_labelList[labelValue] < 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_BadLabelContent"));
            }
            return this.m_labelList[labelValue];
        }

        internal virtual int GetMaxStackSize()
        {
            MethodBuilder methodBuilder = this.m_methodBuilder as MethodBuilder;
            if (methodBuilder == null)
            {
                throw new NotSupportedException();
            }
            return (this.m_maxStackSize + methodBuilder.GetNumberOfExceptions());
        }

        internal virtual SignatureHelper GetMemberRefSignature(CallingConventions call, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes) => 
            this.GetMemberRefSignature(call, returnType, parameterTypes, optionalParameterTypes, 0);

        internal virtual SignatureHelper GetMemberRefSignature(CallingConventions call, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes, int cGenericParameters) => 
            ((ModuleBuilder) this.m_methodBuilder.Module).GetMemberRefSignature(call, returnType, parameterTypes, optionalParameterTypes, cGenericParameters);

        internal virtual int GetMemberRefToken(MethodBase method, Type[] optionalParameterTypes) => 
            ((ModuleBuilder) this.m_methodBuilder.Module).GetMemberRefToken(method, optionalParameterTypes);

        internal virtual int GetMethodToken(MethodBase method, Type[] optionalParameterTypes)
        {
            int handle = 0;
            ModuleBuilder module = (ModuleBuilder) this.m_methodBuilder.Module;
            if (method.IsGenericMethod)
            {
                int num2;
                MethodInfo genericMethodDefinition = method as MethodInfo;
                if (!method.IsGenericMethodDefinition && (method is MethodInfo))
                {
                    genericMethodDefinition = ((MethodInfo) method).GetGenericMethodDefinition();
                }
                if (!genericMethodDefinition.Module.Equals(this.m_methodBuilder.Module) || ((genericMethodDefinition.DeclaringType != null) && genericMethodDefinition.DeclaringType.IsGenericType))
                {
                    handle = this.GetMemberRefToken(genericMethodDefinition, null);
                }
                else
                {
                    handle = module.GetMethodTokenInternal(genericMethodDefinition).Token;
                }
                byte[] signature = SignatureHelper.GetMethodSpecSigHelper(module, method.GetGenericArguments()).InternalGetSignature(out num2);
                return TypeBuilder.InternalDefineMethodSpec(handle, signature, num2, module);
            }
            if (((method.CallingConvention & CallingConventions.VarArgs) == 0) && ((method.DeclaringType == null) || !method.DeclaringType.IsGenericType))
            {
                if (method is MethodInfo)
                {
                    return module.GetMethodTokenInternal(method as MethodInfo).Token;
                }
                return module.GetConstructorToken(method as ConstructorInfo).Token;
            }
            return this.GetMemberRefToken(method, optionalParameterTypes);
        }

        internal virtual int[] GetRVAFixups()
        {
            int[] destinationArray = new int[this.m_RVAFixupCount];
            Array.Copy(this.m_RVAFixupList, destinationArray, this.m_RVAFixupCount);
            return destinationArray;
        }

        internal virtual int[] GetTokenFixups()
        {
            int[] destinationArray = new int[this.m_RelocFixupCount];
            Array.Copy(this.m_RelocFixupList, destinationArray, this.m_RelocFixupCount);
            return destinationArray;
        }

        internal void InternalEmit(OpCode opcode)
        {
            if (opcode.m_size == 1)
            {
                this.m_ILStream[this.m_length++] = opcode.m_s2;
            }
            else
            {
                this.m_ILStream[this.m_length++] = opcode.m_s1;
                this.m_ILStream[this.m_length++] = opcode.m_s2;
            }
            this.UpdateStackSize(opcode, opcode.StackChange());
        }

        public virtual void MarkLabel(Label loc)
        {
            int labelValue = loc.GetLabelValue();
            if ((labelValue < 0) || (labelValue >= this.m_labelList.Length))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidLabel"));
            }
            if (this.m_labelList[labelValue] != -1)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_RedefinedLabel"));
            }
            this.m_labelList[labelValue] = this.m_length;
        }

        public virtual void MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
        {
            if (((startLine == 0) || (startLine < 0)) || ((endLine == 0) || (endLine < 0)))
            {
                throw new ArgumentOutOfRangeException("startLine");
            }
            this.m_LineNumberInfo.AddLineNumberInfo(document, this.m_length, startLine, startColumn, endLine, endColumn);
        }

        internal virtual int PutInteger4(int value, int startPos, byte[] array)
        {
            array[startPos++] = (byte) value;
            array[startPos++] = (byte) (value >> 8);
            array[startPos++] = (byte) (value >> 0x10);
            array[startPos++] = (byte) (value >> 0x18);
            return startPos;
        }

        private void RecordTokenFixup()
        {
            if (this.m_RelocFixupCount >= this.m_RelocFixupList.Length)
            {
                this.m_RelocFixupList = EnlargeArray(this.m_RelocFixupList);
            }
            this.m_RelocFixupList[this.m_RelocFixupCount++] = this.m_length;
        }

        internal virtual void SortExceptions(__ExceptionInfo[] exceptions)
        {
            int length = exceptions.Length;
            for (int i = 0; i < length; i++)
            {
                int index = i;
                for (int j = i + 1; j < length; j++)
                {
                    if (exceptions[index].IsInner(exceptions[j]))
                    {
                        index = j;
                    }
                }
                __ExceptionInfo info = exceptions[i];
                exceptions[i] = exceptions[index];
                exceptions[index] = info;
            }
        }

        void _ILGenerator.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
        {
            throw new NotImplementedException();
        }

        void _ILGenerator.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
        {
            throw new NotImplementedException();
        }

        void _ILGenerator.GetTypeInfoCount(out uint pcTInfo)
        {
            throw new NotImplementedException();
        }

        void _ILGenerator.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
        {
            throw new NotImplementedException();
        }

        public virtual void ThrowException(Type excType)
        {
            if (excType == null)
            {
                throw new ArgumentNullException("excType");
            }
            ModuleBuilder module = (ModuleBuilder) this.m_methodBuilder.Module;
            if (!excType.IsSubclassOf(typeof(Exception)) && (excType != typeof(Exception)))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_NotExceptionType"));
            }
            ConstructorInfo constructor = excType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_MissingDefaultConstructor"));
            }
            this.Emit(OpCodes.Newobj, constructor);
            this.Emit(OpCodes.Throw);
        }

        internal void UpdateStackSize(OpCode opcode, int stackchange)
        {
            this.m_maxMidStackCur += stackchange;
            if (this.m_maxMidStackCur > this.m_maxMidStack)
            {
                this.m_maxMidStack = this.m_maxMidStackCur;
            }
            else if (this.m_maxMidStackCur < 0)
            {
                this.m_maxMidStackCur = 0;
            }
            if (opcode.EndsUncondJmpBlk())
            {
                this.m_maxStackSize += this.m_maxMidStack;
                this.m_maxMidStack = 0;
                this.m_maxMidStackCur = 0;
            }
        }

        public virtual void UsingNamespace(string usingNamespace)
        {
            if (usingNamespace == null)
            {
                throw new ArgumentNullException("usingNamespace");
            }
            if (usingNamespace.Length == 0)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_EmptyName"), "usingNamespace");
            }
            MethodBuilder methodBuilder = this.m_methodBuilder as MethodBuilder;
            if (methodBuilder == null)
            {
                throw new NotSupportedException();
            }
            if (methodBuilder.GetILGenerator().m_ScopeTree.GetCurrentActiveScopeIndex() == -1)
            {
                methodBuilder.m_localSymInfo.AddUsingNamespace(usingNamespace);
            }
            else
            {
                this.m_ScopeTree.AddUsingNamespaceToCurrentScope(usingNamespace);
            }
        }
    }
}

