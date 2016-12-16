namespace System.Reflection.Emit
{
    using System;
    using System.Diagnostics.SymbolStore;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal class DynamicILGenerator : ILGenerator
    {
        private int m_methodSigToken;
        internal DynamicScope m_scope;

        internal DynamicILGenerator(DynamicMethod method, byte[] methodSignature, int size) : base(method, size)
        {
            this.m_scope = new DynamicScope();
            this.m_methodSigToken = this.m_scope.GetTokenFor(methodSignature);
        }

        private int AddSignature(byte[] sig) => 
            (this.m_scope.GetTokenFor(sig) | 0x11000000);

        private int AddStringLiteral(string s) => 
            (this.m_scope.GetTokenFor(s) | 0x70000000);

        public override void BeginCatchBlock(Type exceptionType)
        {
            if (base.m_currExcStackCount == 0)
            {
                throw new NotSupportedException(Environment.GetResourceString("Argument_NotInExceptionBlock"));
            }
            __ExceptionInfo info = base.m_currExcStack[base.m_currExcStackCount - 1];
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
                if (exceptionType.GetType() != typeof(RuntimeType))
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"));
                }
                Label endLabel = info.GetEndLabel();
                this.Emit(OpCodes.Leave, endLabel);
                base.UpdateStackSize(OpCodes.Nop, 1);
            }
            info.MarkCatchAddr(base.m_length, exceptionType);
            info.m_filterAddr[info.m_currentCatch - 1] = this.m_scope.GetTokenFor(exceptionType.TypeHandle);
        }

        public override void BeginExceptFilterBlock()
        {
            throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
        }

        public override Label BeginExceptionBlock() => 
            base.BeginExceptionBlock();

        public override void BeginFaultBlock()
        {
            throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
        }

        public override void BeginFinallyBlock()
        {
            base.BeginFinallyBlock();
        }

        public override void BeginScope()
        {
            throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
        }

        public override LocalBuilder DeclareLocal(Type localType, bool pinned)
        {
            if (localType == null)
            {
                throw new ArgumentNullException("localType");
            }
            if (localType.GetType() != typeof(RuntimeType))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"));
            }
            LocalBuilder builder = new LocalBuilder(base.m_localCount, localType, base.m_methodBuilder);
            base.m_localSignature.AddArgument(localType, pinned);
            base.m_localCount++;
            return builder;
        }

        [ComVisible(true)]
        public override void Emit(OpCode opcode, ConstructorInfo con)
        {
            if ((con == null) || !(con is RuntimeConstructorInfo))
            {
                throw new ArgumentNullException("con");
            }
            if ((con.DeclaringType != null) && (con.DeclaringType.IsGenericType || con.DeclaringType.IsArray))
            {
                this.Emit(opcode, con.MethodHandle, con.DeclaringType.TypeHandle);
            }
            else
            {
                this.Emit(opcode, con.MethodHandle);
            }
        }

        public override void Emit(OpCode opcode, SignatureHelper signature)
        {
            int stackchange = 0;
            if (signature == null)
            {
                throw new ArgumentNullException("signature");
            }
            this.EnsureCapacity(7);
            base.InternalEmit(opcode);
            if (opcode.m_pop == StackBehaviour.Varpop)
            {
                stackchange -= signature.ArgumentCount;
                stackchange--;
                base.UpdateStackSize(opcode, stackchange);
            }
            int num2 = this.AddSignature(signature.GetSignature(true));
            base.m_length = this.PutInteger4(num2, base.m_length, base.m_ILStream);
        }

        public override void Emit(OpCode opcode, FieldInfo field)
        {
            if (field == null)
            {
                throw new ArgumentNullException("field");
            }
            if (!(field is RuntimeFieldInfo))
            {
                throw new ArgumentNullException("field");
            }
            if (field.DeclaringType == null)
            {
                this.Emit(opcode, field.FieldHandle);
            }
            else
            {
                this.Emit(opcode, field.FieldHandle, field.DeclaringType.GetTypeHandleInternal());
            }
        }

        public override void Emit(OpCode opcode, MethodInfo meth)
        {
            if (meth == null)
            {
                throw new ArgumentNullException("meth");
            }
            int stackchange = 0;
            int tokenFor = 0;
            DynamicMethod method = meth as DynamicMethod;
            if (method == null)
            {
                if (!(meth is RuntimeMethodInfo))
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeMethodInfo"), "meth");
                }
                if ((meth.DeclaringType != null) && (meth.DeclaringType.IsGenericType || meth.DeclaringType.IsArray))
                {
                    tokenFor = this.m_scope.GetTokenFor(meth.MethodHandle, meth.DeclaringType.TypeHandle);
                }
                else
                {
                    tokenFor = this.m_scope.GetTokenFor(meth.MethodHandle);
                }
            }
            else
            {
                if ((opcode.Equals(OpCodes.Ldtoken) || opcode.Equals(OpCodes.Ldftn)) || opcode.Equals(OpCodes.Ldvirtftn))
                {
                    throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOpCodeOnDynamicMethod"));
                }
                tokenFor = this.m_scope.GetTokenFor(method);
            }
            this.EnsureCapacity(7);
            base.InternalEmit(opcode);
            if ((opcode.m_push == StackBehaviour.Varpush) && (meth.ReturnType != typeof(void)))
            {
                stackchange++;
            }
            if (opcode.m_pop == StackBehaviour.Varpop)
            {
                stackchange -= meth.GetParametersNoCopy().Length;
            }
            if ((!meth.IsStatic && !opcode.Equals(OpCodes.Newobj)) && (!opcode.Equals(OpCodes.Ldtoken) && !opcode.Equals(OpCodes.Ldftn)))
            {
                stackchange--;
            }
            base.UpdateStackSize(opcode, stackchange);
            base.m_length = this.PutInteger4(tokenFor, base.m_length, base.m_ILStream);
        }

        public void Emit(OpCode opcode, RuntimeFieldHandle fieldHandle)
        {
            if (fieldHandle.IsNullHandle())
            {
                throw new ArgumentNullException("fieldHandle");
            }
            int tokenFor = this.m_scope.GetTokenFor(fieldHandle);
            this.EnsureCapacity(7);
            base.InternalEmit(opcode);
            base.m_length = this.PutInteger4(tokenFor, base.m_length, base.m_ILStream);
        }

        public void Emit(OpCode opcode, RuntimeMethodHandle meth)
        {
            if (meth.IsNullHandle())
            {
                throw new ArgumentNullException("meth");
            }
            int tokenFor = this.m_scope.GetTokenFor(meth);
            this.EnsureCapacity(7);
            base.InternalEmit(opcode);
            base.UpdateStackSize(opcode, 1);
            base.m_length = this.PutInteger4(tokenFor, base.m_length, base.m_ILStream);
        }

        public void Emit(OpCode opcode, RuntimeTypeHandle typeHandle)
        {
            if (typeHandle.IsNullHandle())
            {
                throw new ArgumentNullException("typeHandle");
            }
            int tokenFor = this.m_scope.GetTokenFor(typeHandle);
            this.EnsureCapacity(7);
            base.InternalEmit(opcode);
            base.m_length = this.PutInteger4(tokenFor, base.m_length, base.m_ILStream);
        }

        public override void Emit(OpCode opcode, string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            int num = this.AddStringLiteral(str) | 0x70000000;
            this.EnsureCapacity(7);
            base.InternalEmit(opcode);
            base.m_length = this.PutInteger4(num, base.m_length, base.m_ILStream);
        }

        public override void Emit(OpCode opcode, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            this.Emit(opcode, type.TypeHandle);
        }

        public void Emit(OpCode opcode, RuntimeFieldHandle fieldHandle, RuntimeTypeHandle typeContext)
        {
            if (fieldHandle.IsNullHandle())
            {
                throw new ArgumentNullException("fieldHandle");
            }
            int tokenFor = this.m_scope.GetTokenFor(fieldHandle, typeContext);
            this.EnsureCapacity(7);
            base.InternalEmit(opcode);
            base.m_length = this.PutInteger4(tokenFor, base.m_length, base.m_ILStream);
        }

        public void Emit(OpCode opcode, RuntimeMethodHandle meth, RuntimeTypeHandle typeContext)
        {
            if (meth.IsNullHandle())
            {
                throw new ArgumentNullException("meth");
            }
            if (typeContext.IsNullHandle())
            {
                throw new ArgumentNullException("typeContext");
            }
            int tokenFor = this.m_scope.GetTokenFor(meth, typeContext);
            this.EnsureCapacity(7);
            base.InternalEmit(opcode);
            base.UpdateStackSize(opcode, 1);
            base.m_length = this.PutInteger4(tokenFor, base.m_length, base.m_ILStream);
        }

        public override void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[] optionalParameterTypes)
        {
            int stackchange = 0;
            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }
            if (methodInfo.ContainsGenericParameters)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_GenericsInvalid"), "methodInfo");
            }
            if ((methodInfo.DeclaringType != null) && methodInfo.DeclaringType.ContainsGenericParameters)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_GenericsInvalid"), "methodInfo");
            }
            int memberRefToken = this.GetMemberRefToken(methodInfo, optionalParameterTypes);
            this.EnsureCapacity(7);
            base.InternalEmit(opcode);
            if (methodInfo.ReturnType != typeof(void))
            {
                stackchange++;
            }
            stackchange -= methodInfo.GetParameterTypes().Length;
            if ((!(methodInfo is SymbolMethod) && !methodInfo.IsStatic) && !opcode.Equals(OpCodes.Newobj))
            {
                stackchange--;
            }
            if (optionalParameterTypes != null)
            {
                stackchange -= optionalParameterTypes.Length;
            }
            base.UpdateStackSize(opcode, stackchange);
            base.m_length = this.PutInteger4(memberRefToken, base.m_length, base.m_ILStream);
        }

        public override void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type returnType, Type[] parameterTypes)
        {
            int stackchange = 0;
            int length = 0;
            if (parameterTypes != null)
            {
                length = parameterTypes.Length;
            }
            SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(unmanagedCallConv, returnType);
            if (parameterTypes != null)
            {
                for (int i = 0; i < length; i++)
                {
                    methodSigHelper.AddArgument(parameterTypes[i]);
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
            base.UpdateStackSize(opcode, stackchange);
            this.EnsureCapacity(7);
            this.Emit(OpCodes.Calli);
            int num4 = this.AddSignature(methodSigHelper.GetSignature(true));
            base.m_length = this.PutInteger4(num4, base.m_length, base.m_ILStream);
        }

        public override void EmitCalli(OpCode opcode, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
        {
            int stackchange = 0;
            if ((optionalParameterTypes != null) && ((callingConvention & CallingConventions.VarArgs) == 0))
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAVarArgCallingConvention"));
            }
            SignatureHelper helper = this.GetMemberRefSignature(callingConvention, returnType, parameterTypes, optionalParameterTypes);
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
            base.UpdateStackSize(opcode, stackchange);
            int num2 = this.AddSignature(helper.GetSignature(true));
            base.m_length = this.PutInteger4(num2, base.m_length, base.m_ILStream);
        }

        public override void EndExceptionBlock()
        {
            base.EndExceptionBlock();
        }

        public override void EndScope()
        {
            throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
        }

        internal unsafe RuntimeMethodHandle GetCallableMethod(void* module) => 
            new RuntimeMethodHandle(ModuleHandle.GetDynamicMethod(module, base.m_methodBuilder.Name, (byte[]) this.m_scope[this.m_methodSigToken], new DynamicResolver(this)));

        internal override int GetMaxStackSize() => 
            base.m_maxStackSize;

        internal override SignatureHelper GetMemberRefSignature(CallingConventions call, Type returnType, Type[] parameterTypes, Type[] optionalParameterTypes)
        {
            int length;
            int num2;
            if (parameterTypes == null)
            {
                length = 0;
            }
            else
            {
                length = parameterTypes.Length;
            }
            SignatureHelper methodSigHelper = SignatureHelper.GetMethodSigHelper(call, returnType);
            for (num2 = 0; num2 < length; num2++)
            {
                methodSigHelper.AddArgument(parameterTypes[num2]);
            }
            if ((optionalParameterTypes != null) && (optionalParameterTypes.Length != 0))
            {
                methodSigHelper.AddSentinel();
                for (num2 = 0; num2 < optionalParameterTypes.Length; num2++)
                {
                    methodSigHelper.AddArgument(optionalParameterTypes[num2]);
                }
            }
            return methodSigHelper;
        }

        internal override int GetMemberRefToken(MethodBase methodInfo, Type[] optionalParameterTypes)
        {
            Type[] typeArray;
            if ((optionalParameterTypes != null) && ((methodInfo.CallingConvention & CallingConventions.VarArgs) == 0))
            {
                throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NotAVarArgCallingConvention"));
            }
            if (!((methodInfo is RuntimeMethodInfo) || (methodInfo is DynamicMethod)))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeMethodInfo"), "methodInfo");
            }
            ParameterInfo[] parametersNoCopy = methodInfo.GetParametersNoCopy();
            if ((parametersNoCopy != null) && (parametersNoCopy.Length != 0))
            {
                typeArray = new Type[parametersNoCopy.Length];
                for (int i = 0; i < parametersNoCopy.Length; i++)
                {
                    typeArray[i] = parametersNoCopy[i].ParameterType;
                }
            }
            else
            {
                typeArray = null;
            }
            SignatureHelper signature = this.GetMemberRefSignature(methodInfo.CallingConvention, methodInfo.GetReturnType(), typeArray, optionalParameterTypes);
            return this.m_scope.GetTokenFor(new VarArgMethod(methodInfo as MethodInfo, signature));
        }

        public override void MarkSequencePoint(ISymbolDocumentWriter document, int startLine, int startColumn, int endLine, int endColumn)
        {
            throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
        }

        public override void UsingNamespace(string ns)
        {
            throw new NotSupportedException(Environment.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod"));
        }
    }
}

