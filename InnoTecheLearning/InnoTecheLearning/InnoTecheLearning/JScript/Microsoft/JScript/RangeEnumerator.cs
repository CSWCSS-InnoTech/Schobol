namespace Microsoft.JScript
{
    using System;
    using System.Collections;

    internal class RangeEnumerator : IEnumerator
    {
        private int curr;
        private int start;
        private int stop;

        internal RangeEnumerator(int start, int stop)
        {
            this.curr = start - 1;
            this.start = start;
            this.stop = stop;
        }

        public virtual bool MoveNext() => 
            (++this.curr <= this.stop);

        public virtual void Reset()
        {
            this.curr = this.start;
        }

        public virtual object Current =>
            this.curr;
    }
}

