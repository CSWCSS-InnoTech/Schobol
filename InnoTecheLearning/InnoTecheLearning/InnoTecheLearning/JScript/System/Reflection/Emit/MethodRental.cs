namespace System.Reflection.Emit
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Permissions;
    using System.Threading;

    [ClassInterface(ClassInterfaceType.None), ComVisible(true), ComDefaultInterface(typeof(_MethodRental)), HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort=true)]
    public sealed class MethodRental : _MethodRental
    {
        public const int JitImmediate = 1;
        public const int JitOnDemand = 0;

        private MethodRental()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining), SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
        public static void SwapMethodBody(Type cls, int methodtoken, IntPtr rgIL, int methodSize, int flags)
        {
            RuntimeType runtimeType;
            if ((methodSize <= 0) || (methodSize >= 0x3f0000))
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_BadSizeForData"), "methodSize");
            }
            if (cls == null)
            {
                throw new ArgumentNullException("cls");
            }
            if (!(cls.Module is ModuleBuilder))
            {
                throw new NotSupportedException(Environment.GetResourceString("NotSupported_NotDynamicModule"));
            }
            if (cls is TypeBuilder)
            {
                TypeBuilder builder = (TypeBuilder) cls;
                if (!builder.m_hasBeenCreated)
                {
                    throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("NotSupported_NotAllTypesAreBaked"), new object[] { builder.Name }));
                }
                runtimeType = builder.m_runtimeType;
            }
            else
            {
                runtimeType = cls as RuntimeType;
            }
            if (runtimeType == null)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_MustBeRuntimeType"), "cls");
            }
            StackCrawlMark lookForMyCaller = StackCrawlMark.LookForMyCaller;
            if (runtimeType.Assembly.m_assemblyData.m_isSynchronized)
            {
                lock (runtimeType.Assembly.m_assemblyData)
                {
                    SwapMethodBodyHelper(runtimeType, methodtoken, rgIL, methodSize, flags, ref lookForMyCaller);
                    return;
                }
            }
            SwapMethodBodyHelper(runtimeType, methodtoken, rgIL, methodSize, flags, ref lookForMyCaller);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void SwapMethodBodyHelper(RuntimeType cls, int methodtoken, IntPtr rgIL, int methodSize, int flags, ref StackCrawlMark stackMark);
        void _MethodRental.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
        {
            throw new NotImplementedException();
        }

        void _MethodRental.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
        {
            throw new NotImplementedException();
        }

        void _MethodRental.GetTypeInfoCount(out uint pcTInfo)
        {
            throw new NotImplementedException();
        }

        void _MethodRental.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
        {
            throw new NotImplementedException();
        }
    }
}

