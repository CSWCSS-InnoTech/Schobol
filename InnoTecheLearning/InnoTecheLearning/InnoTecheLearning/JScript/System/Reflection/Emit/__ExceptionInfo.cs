namespace System.Reflection.Emit
{
    using System;

    internal class __ExceptionInfo
    {
        internal const int Fault = 4;
        internal const int Filter = 1;
        internal const int Finally = 2;
        internal int[] m_catchAddr;
        internal Type[] m_catchClass;
        internal int[] m_catchEndAddr;
        internal int m_currentCatch;
        private int m_currentState;
        internal int m_endAddr;
        internal int m_endFinally;
        internal Label m_endLabel;
        internal int[] m_filterAddr;
        internal Label m_finallyEndLabel;
        internal int m_startAddr;
        internal int[] m_type;
        internal const int None = 0;
        internal const int PreserveStack = 4;
        internal const int State_Catch = 2;
        internal const int State_Done = 5;
        internal const int State_Fault = 4;
        internal const int State_Filter = 1;
        internal const int State_Finally = 3;
        internal const int State_Try = 0;

        private __ExceptionInfo()
        {
            this.m_startAddr = 0;
            this.m_filterAddr = null;
            this.m_catchAddr = null;
            this.m_catchEndAddr = null;
            this.m_endAddr = 0;
            this.m_currentCatch = 0;
            this.m_type = null;
            this.m_endFinally = -1;
            this.m_currentState = 0;
        }

        internal __ExceptionInfo(int startAddr, Label endLabel)
        {
            this.m_startAddr = startAddr;
            this.m_endAddr = -1;
            this.m_filterAddr = new int[4];
            this.m_catchAddr = new int[4];
            this.m_catchEndAddr = new int[4];
            this.m_catchClass = new Type[4];
            this.m_currentCatch = 0;
            this.m_endLabel = endLabel;
            this.m_type = new int[4];
            this.m_endFinally = -1;
            this.m_currentState = 0;
        }

        internal virtual void Done(int endAddr)
        {
            this.m_catchEndAddr[this.m_currentCatch - 1] = endAddr;
            this.m_currentState = 5;
        }

        internal virtual int[] GetCatchAddresses() => 
            this.m_catchAddr;

        internal virtual Type[] GetCatchClass() => 
            this.m_catchClass;

        internal virtual int[] GetCatchEndAddresses() => 
            this.m_catchEndAddr;

        internal virtual int GetCurrentState() => 
            this.m_currentState;

        internal virtual int GetEndAddress() => 
            this.m_endAddr;

        internal virtual Label GetEndLabel() => 
            this.m_endLabel;

        internal virtual int[] GetExceptionTypes() => 
            this.m_type;

        internal virtual int[] GetFilterAddresses() => 
            this.m_filterAddr;

        internal virtual int GetFinallyEndAddress() => 
            this.m_endFinally;

        internal virtual Label GetFinallyEndLabel() => 
            this.m_finallyEndLabel;

        internal virtual int GetNumberOfCatches() => 
            this.m_currentCatch;

        internal virtual int GetStartAddress() => 
            this.m_startAddr;

        internal bool IsInner(__ExceptionInfo exc)
        {
            int index = exc.m_currentCatch - 1;
            int num2 = this.m_currentCatch - 1;
            return ((exc.m_catchEndAddr[index] < this.m_catchEndAddr[num2]) || ((exc.m_catchEndAddr[index] == this.m_catchEndAddr[num2]) && (exc.GetEndAddress() > this.GetEndAddress())));
        }

        internal virtual void MarkCatchAddr(int catchAddr, Type catchException)
        {
            this.m_currentState = 2;
            this.MarkHelper(catchAddr, catchAddr, catchException, 0);
        }

        internal virtual void MarkFaultAddr(int faultAddr)
        {
            this.m_currentState = 4;
            this.MarkHelper(faultAddr, faultAddr, null, 4);
        }

        internal virtual void MarkFilterAddr(int filterAddr)
        {
            this.m_currentState = 1;
            this.MarkHelper(filterAddr, filterAddr, null, 1);
        }

        internal virtual void MarkFinallyAddr(int finallyAddr, int endCatchAddr)
        {
            if (this.m_endFinally != -1)
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_TooManyFinallyClause"));
            }
            this.m_currentState = 3;
            this.m_endFinally = finallyAddr;
            this.MarkHelper(finallyAddr, endCatchAddr, null, 2);
        }

        private void MarkHelper(int catchorfilterAddr, int catchEndAddr, Type catchClass, int type)
        {
            if (this.m_currentCatch >= this.m_catchAddr.Length)
            {
                this.m_filterAddr = ILGenerator.EnlargeArray(this.m_filterAddr);
                this.m_catchAddr = ILGenerator.EnlargeArray(this.m_catchAddr);
                this.m_catchEndAddr = ILGenerator.EnlargeArray(this.m_catchEndAddr);
                this.m_catchClass = ILGenerator.EnlargeArray(this.m_catchClass);
                this.m_type = ILGenerator.EnlargeArray(this.m_type);
            }
            if (type == 1)
            {
                this.m_type[this.m_currentCatch] = type;
                this.m_filterAddr[this.m_currentCatch] = catchorfilterAddr;
                this.m_catchAddr[this.m_currentCatch] = -1;
                if (this.m_currentCatch > 0)
                {
                    this.m_catchEndAddr[this.m_currentCatch - 1] = catchorfilterAddr;
                }
            }
            else
            {
                this.m_catchClass[this.m_currentCatch] = catchClass;
                if (this.m_type[this.m_currentCatch] != 1)
                {
                    this.m_type[this.m_currentCatch] = type;
                }
                this.m_catchAddr[this.m_currentCatch] = catchorfilterAddr;
                if ((this.m_currentCatch > 0) && (this.m_type[this.m_currentCatch] != 1))
                {
                    this.m_catchEndAddr[this.m_currentCatch - 1] = catchEndAddr;
                }
                this.m_catchEndAddr[this.m_currentCatch] = -1;
                this.m_currentCatch++;
            }
            if (this.m_endAddr == -1)
            {
                this.m_endAddr = catchorfilterAddr;
            }
        }

        internal virtual void SetFinallyEndLabel(Label lbl)
        {
            this.m_finallyEndLabel = lbl;
        }
    }
}

