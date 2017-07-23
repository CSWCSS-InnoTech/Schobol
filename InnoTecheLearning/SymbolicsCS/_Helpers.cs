using System;
using System.Collections.Generic;
using System.Text;

namespace MathNet.Symbolics
{
    public abstract class TaggedUnion
    { 
        protected readonly int index; // this is the tag; it could be implemented with an enum for more clarity
        protected readonly object val;
        public int Index => index;
        protected TaggedUnion(object val, int index)
        {
            this.val = val;
            this.index = index;
        }
    }
    public class TaggedUnion<T1, T2> : TaggedUnion
    {

        public T1 Val1
        {
            get
            {
                if (!(index == 1)) throw new InvalidOperationException();
                return (T1)val;
            }
        }

        public T2 Val2
        {
            get
            {
                if (!(index == 2)) throw new InvalidOperationException();
                return (T2)val;
            }
        }

        protected TaggedUnion(object val, int index) : base(val, index) { }
        public TaggedUnion(T1 val) : base(val, 1) { }
        public TaggedUnion(T2 val) : base(val, 2) { }

        public static implicit operator TaggedUnion<T1, T2>(T1 val) => new TaggedUnion<T1, T2>(val);
        public static implicit operator TaggedUnion<T1, T2>(T2 val) => new TaggedUnion<T1, T2>(val);
    }

    public class TaggedUnion<T1, T2, T3> : TaggedUnion<T1, T2>
    {
        public T3 Val3
        {
            get
            {
                if (!(index == 3)) throw new InvalidOperationException();
                return (T3)val;
            }
        }
        protected TaggedUnion(object val, int index) : base(val, index) { }
        public TaggedUnion(T1 val) : base(val, 1) { }
        public TaggedUnion(T2 val) : base(val, 2) { }
        public TaggedUnion(T3 val) : base(val, 3) { }
        public static implicit operator TaggedUnion<T1, T2, T3>(T1 val) => new TaggedUnion<T1, T2, T3>(val);
        public static implicit operator TaggedUnion<T1, T2, T3>(T2 val) => new TaggedUnion<T1, T2, T3>(val);
        public static implicit operator TaggedUnion<T1, T2, T3>(T3 val) => new TaggedUnion<T1, T2, T3>(val);
    }

    public class TaggedUnion<T1, T2, T3, T4> : TaggedUnion<T1, T2, T3>
    {
        public T4 Val4
        {
            get
            {
                if (!(index == 4)) throw new InvalidOperationException();
                return (T4)val;
            }
        }
        protected TaggedUnion(object val, int index) : base(val, index) { }
        public TaggedUnion(T1 val) : base(val, 1) { }
        public TaggedUnion(T2 val) : base(val, 2) { }
        public TaggedUnion(T3 val) : base(val, 3) { }
        public TaggedUnion(T4 val) : base(val, 4) { }
        public static implicit operator TaggedUnion<T1, T2, T3, T4>(T1 val) => 
            new TaggedUnion<T1, T2, T3, T4>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4>(T2 val) =>
            new TaggedUnion<T1, T2, T3, T4>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4>(T3 val) =>
            new TaggedUnion<T1, T2, T3, T4>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4>(T4 val) =>
            new TaggedUnion<T1, T2, T3, T4>(val);
    }

    public class TaggedUnion<T1, T2, T3, T4, T5> : TaggedUnion<T1, T2, T3, T4>
    {
        public T5 Val5
        {
            get
            {
                if (!(index == 5)) throw new InvalidOperationException();
                return (T5)val;
            }
        }
        protected TaggedUnion(object val, int index) : base(val, index) { }
        public TaggedUnion(T1 val) : base(val, 1) { }
        public TaggedUnion(T2 val) : base(val, 2) { }
        public TaggedUnion(T3 val) : base(val, 3) { }
        public TaggedUnion(T4 val) : base(val, 4) { }
        public TaggedUnion(T5 val) : base(val, 5) { }
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5>(T1 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5>(T2 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5>(T3 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5>(T4 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5>(T5 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5>(val);
    }

    public class TaggedUnion<T1, T2, T3, T4, T5, T6> : TaggedUnion<T1, T2, T3, T4, T5>
    {
        public T6 Val6
        {
            get
            {
                if (!(index == 6)) throw new InvalidOperationException();
                return (T6)val;
            }
        }
        protected TaggedUnion(object val, int index) : base(val, index) { }
        public TaggedUnion(T1 val) : base(val, 1) { }
        public TaggedUnion(T2 val) : base(val, 2) { }
        public TaggedUnion(T3 val) : base(val, 3) { }
        public TaggedUnion(T4 val) : base(val, 4) { }
        public TaggedUnion(T5 val) : base(val, 5) { }
        public TaggedUnion(T6 val) : base(val, 6) { }
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6>(T1 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6>(T2 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6>(T3 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6>(T4 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6>(T5 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6>(T6 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6>(val);
    }

    public class TaggedUnion<T1, T2, T3, T4, T5, T6, T7> : TaggedUnion<T1, T2, T3, T4, T5, T6>
    {
        public T7 Val7
        {
            get
            {
                if (!(index == 7)) throw new InvalidOperationException();
                return (T7)val;
            }
        }
        protected TaggedUnion(object val, int index) : base(val, index) { }
        public TaggedUnion(T1 val) : base(val, 1) { }
        public TaggedUnion(T2 val) : base(val, 2) { }
        public TaggedUnion(T3 val) : base(val, 3) { }
        public TaggedUnion(T4 val) : base(val, 4) { }
        public TaggedUnion(T5 val) : base(val, 5) { }
        public TaggedUnion(T6 val) : base(val, 6) { }
        public TaggedUnion(T7 val) : base(val, 7) { }
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7>(T1 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7>(T2 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7>(T3 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7>(T4 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7>(T5 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7>(T6 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7>(T7 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7>(val);
    }

    public class TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8> : TaggedUnion<T1, T2, T3, T4, T5, T6, T7>
    {
        public T8 Val8
        {
            get
            {
                if (!(index == 8)) throw new InvalidOperationException();
                return (T8)val;
            }
        }
        protected TaggedUnion(object val, int index) : base(val, index) { }
        public TaggedUnion(T1 val) : base(val, 1) { }
        public TaggedUnion(T2 val) : base(val, 2) { }
        public TaggedUnion(T3 val) : base(val, 3) { }
        public TaggedUnion(T4 val) : base(val, 4) { }
        public TaggedUnion(T5 val) : base(val, 5) { }
        public TaggedUnion(T6 val) : base(val, 6) { }
        public TaggedUnion(T7 val) : base(val, 7) { }
        public TaggedUnion(T8 val) : base(val, 8) { }
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T1 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T2 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T3 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T4 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T5 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T6 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T7 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(val);
        public static implicit operator TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T8 val) =>
            new TaggedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(val);
    }
}
