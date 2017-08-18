using System.Collections.Generic;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public class MathSolverStack<T> : Stack<T>
        {
            public MathSolverStack() : base() { }
            public MathSolverStack(int Capacity) : base(Capacity) { }
            public MathSolverStack(IEnumerable<T> Collection) : base(Collection) { }
            
            public new void Push(T Item) { if (Count == 0 || !Item.Equals(Peek())) base.Push(Item); }
            public object Clone() { return MemberwiseClone(); }
        }
    }
}