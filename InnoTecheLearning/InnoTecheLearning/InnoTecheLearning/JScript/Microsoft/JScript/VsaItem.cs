namespace Microsoft.JScript
{
    using Microsoft.JScript.Vsa;
    using Microsoft.Vsa;
    using System;
    using System.Security.Permissions;

    public abstract class VsaItem : IVsaItem
    {
        internal string codebase;
        internal VsaEngine engine;
        protected VsaItemFlag flag;
        protected bool isDirty;
        protected string name;
        protected VsaItemType type;

        internal VsaItem(VsaEngine engine, string itemName, VsaItemType type, VsaItemFlag flag)
        {
            this.engine = engine;
            this.type = type;
            this.name = itemName;
            this.flag = flag;
            this.codebase = null;
            this.isDirty = true;
        }

        internal virtual void CheckForErrors()
        {
        }

        internal virtual void Close()
        {
            this.engine = null;
        }

        internal virtual void Compile()
        {
        }

        internal virtual Type GetCompiledType() => 
            null;

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual object GetOption(string name)
        {
            if (this.engine == null)
            {
                throw new VsaException(VsaError.EngineClosed);
            }
            if (string.Compare(name, "codebase", StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw new VsaException(VsaError.OptionNotSupported);
            }
            return this.codebase;
        }

        internal virtual void Remove()
        {
            this.engine = null;
        }

        internal virtual void Reset()
        {
        }

        internal virtual void Run()
        {
        }

        [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
        public virtual void SetOption(string name, object value)
        {
            if (this.engine == null)
            {
                throw new VsaException(VsaError.EngineClosed);
            }
            if (string.Compare(name, "codebase", StringComparison.OrdinalIgnoreCase) != 0)
            {
                throw new VsaException(VsaError.OptionNotSupported);
            }
            this.codebase = (string) value;
            this.isDirty = true;
            this.engine.IsDirty = true;
        }

        public virtual bool IsDirty
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                if (this.engine == null)
                {
                    throw new VsaException(VsaError.EngineClosed);
                }
                return this.isDirty;
            }
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            set
            {
                if (this.engine == null)
                {
                    throw new VsaException(VsaError.EngineClosed);
                }
                this.isDirty = value;
            }
        }

        public VsaItemType ItemType
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                if (this.engine == null)
                {
                    throw new VsaException(VsaError.EngineClosed);
                }
                return this.type;
            }
        }

        public virtual string Name
        {
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            get
            {
                if (this.engine == null)
                {
                    throw new VsaException(VsaError.EngineClosed);
                }
                return this.name;
            }
            [PermissionSet(SecurityAction.LinkDemand, Name="FullTrust")]
            set
            {
                if (this.engine == null)
                {
                    throw new VsaException(VsaError.EngineClosed);
                }
                if (this.name != value)
                {
                    if (!this.engine.IsValidIdentifier(value))
                    {
                        throw new VsaException(VsaError.ItemNameInvalid);
                    }
                    foreach (IVsaItem item in this.engine.Items)
                    {
                        if (item.Name.Equals(value))
                        {
                            throw new VsaException(VsaError.ItemNameInUse);
                        }
                    }
                    this.name = value;
                    this.isDirty = true;
                    this.engine.IsDirty = true;
                }
            }
        }
    }
}

