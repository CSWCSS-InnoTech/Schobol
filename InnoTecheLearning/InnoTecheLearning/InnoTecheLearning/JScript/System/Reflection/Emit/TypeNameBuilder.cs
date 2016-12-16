namespace System.Reflection.Emit
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class TypeNameBuilder
    {
        private IntPtr m_typeNameBuilder;

        private TypeNameBuilder(IntPtr typeNameBuilder)
        {
            this.m_typeNameBuilder = typeNameBuilder;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _AddArray(IntPtr tnb, int rank);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _AddAssemblySpec(IntPtr tnb, string assemblySpec);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _AddByRef(IntPtr tnb);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _AddName(IntPtr tnb, string name);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _AddPointer(IntPtr tnb);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _AddSzArray(IntPtr tnb);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _Clear(IntPtr tnb);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _CloseGenericArgument(IntPtr tnb);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _CloseGenericArguments(IntPtr tnb);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern IntPtr _CreateTypeNameBuilder();
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _OpenGenericArgument(IntPtr tnb);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _OpenGenericArguments(IntPtr tnb);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void _ReleaseTypeNameBuilder(IntPtr pAQN);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern string _ToString(IntPtr tnb);
        private void AddArray(int rank)
        {
            _AddArray(this.m_typeNameBuilder, rank);
        }

        private void AddAssemblySpec(string assemblySpec)
        {
            _AddAssemblySpec(this.m_typeNameBuilder, assemblySpec);
        }

        private void AddByRef()
        {
            _AddByRef(this.m_typeNameBuilder);
        }

        private void AddElementType(Type elementType)
        {
            if (elementType.HasElementType)
            {
                this.AddElementType(elementType.GetElementType());
            }
            if (elementType.IsPointer)
            {
                this.AddPointer();
            }
            else if (elementType.IsByRef)
            {
                this.AddByRef();
            }
            else if (elementType.IsSzArray)
            {
                this.AddSzArray();
            }
            else if (elementType.IsArray)
            {
                this.AddArray(elementType.GetArrayRank());
            }
        }

        private void AddName(string name)
        {
            _AddName(this.m_typeNameBuilder, name);
        }

        private void AddPointer()
        {
            _AddPointer(this.m_typeNameBuilder);
        }

        private void AddSzArray()
        {
            _AddSzArray(this.m_typeNameBuilder);
        }

        private void Clear()
        {
            _Clear(this.m_typeNameBuilder);
        }

        private void CloseGenericArgument()
        {
            _CloseGenericArgument(this.m_typeNameBuilder);
        }

        private void CloseGenericArguments()
        {
            _CloseGenericArguments(this.m_typeNameBuilder);
        }

        private void ConstructAssemblyQualifiedNameWorker(Type type, Format format)
        {
            Type elementType = type;
            while (elementType.HasElementType)
            {
                elementType = elementType.GetElementType();
            }
            List<Type> list = new List<Type>();
            for (Type type3 = elementType; type3 != null; type3 = type3.IsGenericParameter ? null : type3.DeclaringType)
            {
                list.Add(type3);
            }
            for (int i = list.Count - 1; i >= 0; i--)
            {
                Type type4 = list[i];
                string name = type4.Name;
                if (((i == (list.Count - 1)) && (type4.Namespace != null)) && (type4.Namespace.Length != 0))
                {
                    name = type4.Namespace + "." + name;
                }
                this.AddName(name);
            }
            if (elementType.IsGenericType && (!elementType.IsGenericTypeDefinition || (format == Format.ToString)))
            {
                Type[] genericArguments = elementType.GetGenericArguments();
                this.OpenGenericArguments();
                for (int j = 0; j < genericArguments.Length; j++)
                {
                    Format format2 = (format == Format.FullName) ? Format.AssemblyQualifiedName : format;
                    this.OpenGenericArgument();
                    this.ConstructAssemblyQualifiedNameWorker(genericArguments[j], format2);
                    this.CloseGenericArgument();
                }
                this.CloseGenericArguments();
            }
            this.AddElementType(type);
            if (format == Format.AssemblyQualifiedName)
            {
                this.AddAssemblySpec(type.Module.Assembly.FullName);
            }
        }

        internal void Dispose()
        {
            _ReleaseTypeNameBuilder(this.m_typeNameBuilder);
        }

        private void OpenGenericArgument()
        {
            _OpenGenericArgument(this.m_typeNameBuilder);
        }

        private void OpenGenericArguments()
        {
            _OpenGenericArguments(this.m_typeNameBuilder);
        }

        public override string ToString() => 
            _ToString(this.m_typeNameBuilder);

        internal static string ToString(Type type, Format format)
        {
            if (((format == Format.FullName) || (format == Format.AssemblyQualifiedName)) && (!type.IsGenericTypeDefinition && type.ContainsGenericParameters))
            {
                return null;
            }
            TypeNameBuilder builder = new TypeNameBuilder(_CreateTypeNameBuilder());
            builder.Clear();
            builder.ConstructAssemblyQualifiedNameWorker(type, format);
            string str = builder.ToString();
            builder.Dispose();
            return str;
        }

        internal enum Format
        {
            ToString,
            FullName,
            AssemblyQualifiedName
        }
    }
}

