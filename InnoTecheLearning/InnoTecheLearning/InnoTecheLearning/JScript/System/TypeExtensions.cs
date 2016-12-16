using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace System
{
    public static class SystemExtensions
    {
        public static bool IsPrimitive(this Type T)
        {
            if (T == typeof(bool) ||
                T == typeof(byte) ||
                T == typeof(sbyte) ||
                T == typeof(short) ||
                T == typeof(ushort) ||
                T == typeof(int) ||
                T == typeof(uint) ||
                T == typeof(long) ||
                T == typeof(ulong) ||
                T == typeof(IntPtr) ||
                T == typeof(UIntPtr) ||
                T == typeof(char) ||
                T == typeof(double) ||
                T == typeof(float))
                return true;
            return false;
        }
        public static bool IsEnum(this Type T)
        {
            if (T == typeof(Enum))
                return true;
            return false;
        }
        public static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                     "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }
    }
}
