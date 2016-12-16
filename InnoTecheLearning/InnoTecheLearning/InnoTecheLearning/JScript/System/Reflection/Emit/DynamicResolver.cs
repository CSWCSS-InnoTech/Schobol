namespace System.Reflection.Emit
{
    using System;
    using System.Reflection;
    using System.Threading;

    internal class DynamicResolver : Resolver
    {
        private byte[] m_code;
        private byte[] m_exceptionHeader;
        private __ExceptionInfo[] m_exceptions;
        private byte[] m_localSignature;
        private DynamicMethod m_method;
        private int m_methodToken;
        private DynamicScope m_scope;
        private int m_stackSize;

        internal DynamicResolver(DynamicILGenerator ilGenerator)
        {
            this.m_stackSize = ilGenerator.GetMaxStackSize();
            this.m_exceptions = ilGenerator.GetExceptions();
            this.m_code = ilGenerator.BakeByteArray();
            this.m_localSignature = ilGenerator.m_localSignature.InternalGetSignatureArray();
            this.m_scope = ilGenerator.m_scope;
            this.m_method = (DynamicMethod) ilGenerator.m_methodBuilder;
            this.m_method.m_resolver = this;
        }

        internal DynamicResolver(DynamicILInfo dynamicILInfo)
        {
            this.m_stackSize = dynamicILInfo.MaxStackSize;
            this.m_code = dynamicILInfo.Code;
            this.m_localSignature = dynamicILInfo.LocalSignature;
            this.m_exceptionHeader = dynamicILInfo.Exceptions;
            this.m_scope = dynamicILInfo.DynamicScope;
            this.m_method = dynamicILInfo.DynamicMethod;
            this.m_method.m_resolver = this;
        }

        ~DynamicResolver()
        {
            DynamicMethod method = this.m_method;
            if ((method != null) && !method.m_method.IsNullHandle())
            {
                DestroyScout scout = null;
                try
                {
                    scout = new DestroyScout();
                }
                catch
                {
                    if (!Environment.HasShutdownStarted && !AppDomain.CurrentDomain.IsFinalizingForUnload())
                    {
                        GC.ReRegisterForFinalize(this);
                    }
                    return;
                }
                scout.m_method = method.m_method;
            }
        }

        internal override byte[] GetCodeInfo(ref int stackSize, ref int initLocals, ref int EHCount)
        {
            stackSize = this.m_stackSize;
            if ((this.m_exceptionHeader != null) && (this.m_exceptionHeader.Length != 0))
            {
                if (this.m_exceptionHeader.Length < 4)
                {
                    throw new FormatException();
                }
                byte num = this.m_exceptionHeader[0];
                if ((num & 0x40) != 0)
                {
                    byte[] buffer = new byte[4];
                    for (int i = 0; i < 3; i++)
                    {
                        buffer[i] = this.m_exceptionHeader[i + 1];
                    }
                    EHCount = (BitConverter.ToInt32(buffer, 0) - 4) / 0x18;
                }
                else
                {
                    EHCount = (this.m_exceptionHeader[1] - 2) / 12;
                }
            }
            else
            {
                EHCount = ILGenerator.CalculateNumberOfExceptions(this.m_exceptions);
            }
            initLocals = this.m_method.InitLocals ? 1 : 0;
            return this.m_code;
        }

        internal override MethodInfo GetDynamicMethod() => 
            this.m_method.GetMethodInfo();

        internal override unsafe void GetEHInfo(int excNumber, void* exc)
        {
            Resolver.CORINFO_EH_CLAUSE* corinfo_eh_clausePtr = (Resolver.CORINFO_EH_CLAUSE*) exc;
            for (int i = 0; i < this.m_exceptions.Length; i++)
            {
                int numberOfCatches = this.m_exceptions[i].GetNumberOfCatches();
                if (excNumber < numberOfCatches)
                {
                    corinfo_eh_clausePtr->Flags = this.m_exceptions[i].GetExceptionTypes()[excNumber];
                    corinfo_eh_clausePtr->Flags |= 0x20000000;
                    corinfo_eh_clausePtr->TryOffset = this.m_exceptions[i].GetStartAddress();
                    if ((corinfo_eh_clausePtr->Flags & 2) != 2)
                    {
                        corinfo_eh_clausePtr->TryLength = this.m_exceptions[i].GetEndAddress() - corinfo_eh_clausePtr->TryOffset;
                    }
                    else
                    {
                        corinfo_eh_clausePtr->TryLength = this.m_exceptions[i].GetFinallyEndAddress() - corinfo_eh_clausePtr->TryOffset;
                    }
                    corinfo_eh_clausePtr->HandlerOffset = this.m_exceptions[i].GetCatchAddresses()[excNumber];
                    corinfo_eh_clausePtr->HandlerLength = this.m_exceptions[i].GetCatchEndAddresses()[excNumber] - corinfo_eh_clausePtr->HandlerOffset;
                    corinfo_eh_clausePtr->ClassTokenOrFilterOffset = this.m_exceptions[i].GetFilterAddresses()[excNumber];
                    return;
                }
                excNumber -= numberOfCatches;
            }
        }

        internal override void GetJitContext(ref int securityControlFlags, ref RuntimeTypeHandle typeOwner)
        {
            SecurityControlFlags flags = SecurityControlFlags.Default;
            if (this.m_method.m_restrictedSkipVisibility)
            {
                flags |= SecurityControlFlags.RestrictedSkipVisibilityChecks;
            }
            else if (this.m_method.m_skipVisibility)
            {
                flags |= SecurityControlFlags.SkipVisibilityChecks;
            }
            typeOwner = (this.m_method.m_typeOwner != null) ? this.m_method.m_typeOwner.TypeHandle : RuntimeTypeHandle.EmptyHandle;
            if (this.m_method.m_creationContext != null)
            {
                flags |= SecurityControlFlags.HasCreationContext;
                if (this.m_method.m_creationContext.CanSkipEvaluation)
                {
                    flags |= SecurityControlFlags.CanSkipCSEvaluation;
                }
            }
            securityControlFlags = (int) flags;
        }

        internal override byte[] GetLocalsSignature() => 
            this.m_localSignature;

        private int GetMethodToken()
        {
            if (this.IsValidToken(this.m_methodToken) == 0)
            {
                int tokenFor = this.m_scope.GetTokenFor(this.m_method.GetMethodDescriptor());
                Interlocked.CompareExchange(ref this.m_methodToken, tokenFor, 0);
            }
            return this.m_methodToken;
        }

        internal override byte[] GetRawEHInfo() => 
            this.m_exceptionHeader;

        internal override CompressedStack GetSecurityContext() => 
            this.m_method.m_creationContext;

        internal override string GetStringLiteral(int token) => 
            this.m_scope.GetString(token);

        internal override int IsValidToken(int token)
        {
            if (this.m_scope[token] == null)
            {
                return 0;
            }
            return 1;
        }

        internal override int ParentToken(int token)
        {
            RuntimeTypeHandle emptyHandle = RuntimeTypeHandle.EmptyHandle;
            object obj2 = this.m_scope[token];
            if (obj2 is RuntimeMethodHandle)
            {
                emptyHandle = ((RuntimeMethodHandle) obj2).GetDeclaringType();
            }
            else if (obj2 is RuntimeFieldHandle)
            {
                emptyHandle = ((RuntimeFieldHandle) obj2).GetApproxDeclaringType();
            }
            else if (obj2 is DynamicMethod)
            {
                DynamicMethod method = (DynamicMethod) obj2;
                emptyHandle = method.m_method.GetDeclaringType();
            }
            else if (obj2 is GenericMethodInfo)
            {
                GenericMethodInfo info = (GenericMethodInfo) obj2;
                emptyHandle = info.m_context;
            }
            else if (obj2 is GenericFieldInfo)
            {
                GenericFieldInfo info2 = (GenericFieldInfo) obj2;
                emptyHandle = info2.m_context;
            }
            else if (obj2 is VarArgMethod)
            {
                VarArgMethod method2 = (VarArgMethod) obj2;
                DynamicMethod method3 = method2.m_method as DynamicMethod;
                if (method3 != null)
                {
                    emptyHandle = method3.GetMethodDescriptor().GetDeclaringType();
                }
                else if (method2.m_method.DeclaringType == null)
                {
                    emptyHandle = method2.m_method.MethodHandle.GetDeclaringType();
                }
                else
                {
                    emptyHandle = method2.m_method.DeclaringType.TypeHandle;
                }
            }
            if (emptyHandle.IsNullHandle())
            {
                return -1;
            }
            return this.m_scope.GetTokenFor(emptyHandle);
        }

        internal override byte[] ResolveSignature(int token, int fromMethod) => 
            this.m_scope.ResolveSignature(token, fromMethod);

        internal override unsafe void* ResolveToken(int token)
        {
            object obj2 = this.m_scope[token];
            if (obj2 is RuntimeTypeHandle)
            {
                RuntimeTypeHandle handle = (RuntimeTypeHandle) obj2;
                return (void*) handle.Value;
            }
            if (obj2 is RuntimeMethodHandle)
            {
                RuntimeMethodHandle handle2 = (RuntimeMethodHandle) obj2;
                return (void*) handle2.Value;
            }
            if (obj2 is RuntimeFieldHandle)
            {
                RuntimeFieldHandle handle3 = (RuntimeFieldHandle) obj2;
                return (void*) handle3.Value;
            }
            if (obj2 is DynamicMethod)
            {
                DynamicMethod method = (DynamicMethod) obj2;
                return (void*) method.GetMethodDescriptor().Value;
            }
            if (obj2 is GenericMethodInfo)
            {
                GenericMethodInfo info = (GenericMethodInfo) obj2;
                return (void*) info.m_method.Value;
            }
            if (obj2 is GenericFieldInfo)
            {
                GenericFieldInfo info2 = (GenericFieldInfo) obj2;
                return (void*) info2.m_field.Value;
            }
            if (!(obj2 is VarArgMethod))
            {
                return null;
            }
            VarArgMethod method2 = (VarArgMethod) obj2;
            DynamicMethod method3 = method2.m_method as DynamicMethod;
            if (method3 == null)
            {
                return (void*) method2.m_method.MethodHandle.Value;
            }
            return (void*) method3.GetMethodDescriptor().Value;
        }

        private class DestroyScout
        {
            internal RuntimeMethodHandle m_method;

            ~DestroyScout()
            {
                if (!this.m_method.IsNullHandle())
                {
                    if (this.m_method.GetResolver() != null)
                    {
                        if (!Environment.HasShutdownStarted && !AppDomain.CurrentDomain.IsFinalizingForUnload())
                        {
                            GC.ReRegisterForFinalize(this);
                        }
                    }
                    else
                    {
                        this.m_method.Destroy();
                    }
                }
            }
        }

        [Flags]
        internal enum SecurityControlFlags
        {
            CanSkipCSEvaluation = 8,
            Default = 0,
            HasCreationContext = 4,
            RestrictedSkipVisibilityChecks = 2,
            SkipVisibilityChecks = 1
        }
    }
}

