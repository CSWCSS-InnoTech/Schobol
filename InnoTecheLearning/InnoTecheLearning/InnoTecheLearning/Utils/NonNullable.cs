using System;
using System.Collections.Generic;
using System.Text;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public static class NonNullable {
            public static NonNullable<T> Create<T>(T Value) => new NonNullable<T>(Value);
            public static NonNullable<T> Create<T>(NonNullable<T> Value) => new NonNullable<T>(Value);
            public static NonNullable<T> Create<T>(T Value, T Null) => new NonNullable<T>(Value, Null);
            public static NonNullable<T> Create<T>(T Value, NonNullable<T> Null) => new NonNullable<T>(Value, Null);
        }
        public struct NonNullable<T>
        {
            T Under;
            public NonNullable(T Under) {
                if (Under == null)
                    throw new ArgumentNullException("Under",
                        "Cannot construct a nonnullable from a null value.");
                this.Under = Under;
            }
            public NonNullable(NonNullable<T> Under)
            {
                this.Under = Under;
            }
            public NonNullable(T Under, T Null)
            {
                if (Under == null)
                {
                    if (Null == null) throw new ArgumentNullException("Null",
                           "Cannot construct a nonnullable from a null value.");
                } else
                this.Under = Null;
                this.Under = Under;
            }
            public NonNullable(T Under, NonNullable<T> Null)
            {
                if (Under == null)
                    this.Under = Null;
                this.Under = Under;
            }
            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                return Under.Equals(obj);
            }
            public override int GetHashCode() => Under.GetHashCode();
            public override string ToString() => Under.ToString();
            public static NonNullable<T> Default { get { return default(NonNullable<T>); } }

            public static implicit operator T(NonNullable<T> NonNull) => NonNull.Under;
            public static implicit operator NonNullable<T>(T Nullable) => new NonNullable<T>(Nullable);
        }
    }
}