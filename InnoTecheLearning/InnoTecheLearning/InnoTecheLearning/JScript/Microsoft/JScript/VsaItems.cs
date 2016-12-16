namespace Microsoft.JScript
{
    using Microsoft.JScript.Vsa;
    using Microsoft.Vsa;
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Security.Permissions;

    public sealed class VsaItems : IVsaItems, IEnumerable
    {
        private VsaEngine engine;
        private bool isClosed;
        private ArrayList items;
        internal int staticCodeBlockCount;

        public VsaItems(VsaEngine engine)
        {
            this.engine = engine;
            this.staticCodeBlockCount = 0;
            this.items = new ArrayList(10);
        }

        public void Close()
        {
            if (this.isClosed)
            {
                throw new VsaException(VsaError.EngineClosed);
            }
            this.TryObtainLock();
            try
            {
                this.isClosed = true;
                foreach (object obj2 in this.items)
                {
                    ((VsaItem) obj2).Close();
                }
                this.items = null;
            }
            finally
            {
                this.ReleaseLock();
                this.engine = null;
            }
        }

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public IVsaItem CreateItem(string name, VsaItemType itemType, VsaItemFlag itemFlag)
        {
            IVsaItem item2;
            if (this.isClosed)
            {
                throw new VsaException(VsaError.EngineClosed);
            }
            if (this.engine.IsRunning)
            {
                throw new VsaException(VsaError.EngineRunning);
            }
            this.TryObtainLock();
            try
            {
                if ((itemType != VsaItemType.Reference) && !this.engine.IsValidIdentifier(name))
                {
                    throw new VsaException(VsaError.ItemNameInvalid);
                }
                foreach (object obj2 in this.items)
                {
                    if (((VsaItem) obj2).Name.Equals(name))
                    {
                        throw new VsaException(VsaError.ItemNameInUse);
                    }
                }
                IVsaItem item = null;
                switch (itemType)
                {
                    case VsaItemType.Reference:
                        if (itemFlag != VsaItemFlag.None)
                        {
                            throw new VsaException(VsaError.ItemFlagNotSupported);
                        }
                        break;

                    case VsaItemType.AppGlobal:
                        if (itemFlag != VsaItemFlag.None)
                        {
                            throw new VsaException(VsaError.ItemFlagNotSupported);
                        }
                        goto Label_00E3;

                    case VsaItemType.Code:
                        if (itemFlag == VsaItemFlag.Class)
                        {
                            throw new VsaException(VsaError.ItemFlagNotSupported);
                        }
                        goto Label_010E;

                    default:
                        goto Label_012A;
                }
                item = new VsaReference(this.engine, name);
                goto Label_012A;
            Label_00E3:
                item = new VsaHostObject(this.engine, name, VsaItemType.AppGlobal);
                ((VsaHostObject) item).isVisible = true;
                goto Label_012A;
            Label_010E:
                item = new VsaStaticCode(this.engine, name, itemFlag);
                this.staticCodeBlockCount++;
            Label_012A:
                if (item != null)
                {
                    this.items.Add(item);
                }
                else
                {
                    throw new VsaException(VsaError.ItemTypeNotSupported);
                }
                this.engine.IsDirty = true;
                item2 = item;
            }
            finally
            {
                this.ReleaseLock();
            }
            return item2;
        }

        public IEnumerator GetEnumerator()
        {
            if (this.isClosed)
            {
                throw new VsaException(VsaError.EngineClosed);
            }
            return this.items.GetEnumerator();
        }

        private void ReleaseLock()
        {
            this.engine.ReleaseLock();
        }

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public void Remove(int itemIndex)
        {
            if (this.isClosed)
            {
                throw new VsaException(VsaError.EngineClosed);
            }
            this.TryObtainLock();
            try
            {
                if ((0 > itemIndex) || (itemIndex >= this.items.Count))
                {
                    throw new VsaException(VsaError.ItemNotFound);
                }
                VsaItem item = (VsaItem) this.items[itemIndex];
                item.Remove();
                this.items.RemoveAt(itemIndex);
                if (item is VsaStaticCode)
                {
                    this.staticCodeBlockCount--;
                }
            }
            finally
            {
                this.ReleaseLock();
            }
        }

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public void Remove(string itemName)
        {
            if (this.isClosed)
            {
                throw new VsaException(VsaError.EngineClosed);
            }
            this.TryObtainLock();
            try
            {
                if (itemName == null)
                {
                    throw new ArgumentNullException("itemName");
                }
                int index = 0;
                int count = this.items.Count;
                while (index < count)
                {
                    IVsaItem item = (IVsaItem) this.items[index];
                    if (item.Name.Equals(itemName))
                    {
                        ((VsaItem) item).Remove();
                        this.items.RemoveAt(index);
                        this.engine.IsDirty = true;
                        if (item is VsaStaticCode)
                        {
                            this.staticCodeBlockCount--;
                        }
                        return;
                    }
                    index++;
                }
                throw new VsaException(VsaError.ItemNotFound);
            }
            finally
            {
                this.ReleaseLock();
            }
        }

        private void TryObtainLock()
        {
            this.engine.TryObtainLock();
        }

        public int Count
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                if (this.isClosed)
                {
                    throw new VsaException(VsaError.EngineClosed);
                }
                if (this.items != null)
                {
                    return this.items.Count;
                }
                return 0;
            }
        }

        public IVsaItem this[int index]
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                if (this.isClosed)
                {
                    throw new VsaException(VsaError.EngineClosed);
                }
                if ((index < 0) || (index >= this.items.Count))
                {
                    throw new VsaException(VsaError.ItemNotFound);
                }
                return (IVsaItem) this.items[index];
            }
        }

        public IVsaItem this[string itemName]
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                if (this.isClosed)
                {
                    throw new VsaException(VsaError.EngineClosed);
                }
                if (itemName != null)
                {
                    int num = 0;
                    int count = this.items.Count;
                    while (num < count)
                    {
                        IVsaItem item = (IVsaItem) this.items[num];
                        if (item.Name.Equals(itemName))
                        {
                            return item;
                        }
                        num++;
                    }
                }
                throw new VsaException(VsaError.ItemNotFound);
            }
        }
    }
}

