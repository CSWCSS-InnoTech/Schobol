namespace System.Security.Util
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security.Policy;

    internal static class Config
    {
        private static string m_machineConfig;
        private static string m_userConfig;

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern string _GetMachineDirectory();
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern string _GetUserDirectory();
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void AddCacheEntry(ConfigId id, int numKey, char[] key, byte[] data);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool GetCacheEntry(ConfigId id, int numKey, char[] key, out byte[] data);
        private static void GetFileLocales()
        {
            if (m_machineConfig == null)
            {
                m_machineConfig = _GetMachineDirectory();
            }
            if (m_userConfig == null)
            {
                m_userConfig = _GetUserDirectory();
            }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool RecoverData(ConfigId id);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void ResetCacheData(ConfigId id);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool SaveDataByte(string path, byte[] data, int offset, int length);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern void SetQuickCache(ConfigId id, QuickCacheEntryType quickCacheFlags);
        [MethodImpl(MethodImplOptions.InternalCall)]
        internal static extern bool WriteToEventLog(string message);

        internal static string MachineDirectory
        {
            get
            {
                GetFileLocales();
                return m_machineConfig;
            }
        }

        internal static string UserDirectory
        {
            get
            {
                GetFileLocales();
                return m_userConfig;
            }
        }
    }
}

