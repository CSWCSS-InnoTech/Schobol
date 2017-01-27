using System;

namespace InnoTecheLearning
{
    public partial class Utils
    {
        public static class Immutable
        {
            public static Immutable<T> Create<T>(T Value) => new Immutable<T>(Value);
            public static Immutable<T> Create<T>(Immutable<T> Value) => new Immutable<T>(Value);
        }
        public class Immutable<T> : IDisposable
        {
            public Immutable() : this(default(T)) { }
            public Immutable(Immutable<T> Immutable) : this(Immutable.Inner) { }
            public Immutable(T Mutable) { Inner = Mutable; }
            public static Immutable<T> Default { get { return default(Immutable<T>); } }

            T Inner;
            void IDisposable.Dispose() { }

            public override bool Equals(object obj)
            { if (obj == null) return false; return Inner.Equals(obj); }
            public override int GetHashCode() => Inner.GetHashCode();
            public override string ToString() => Inner.ToString();

            public static implicit operator T(Immutable<T> Immutable) => Immutable.Inner;
            public static explicit operator Immutable<T>(T Mutable) => new Immutable<T>(Mutable);
        }
    }
}