namespace Microsoft.JScript
{
    using Microsoft.Vsa;
    using System;
    using System.Runtime.InteropServices;

    [Guid("ED4BAE22-2F3C-419a-B487-CF869E716B95"), ComVisible(true)]
    public interface IVsaScriptScope : IVsaItem
    {
        IVsaScriptScope Parent { get; }
        IVsaItem AddItem(string itemName, VsaItemType type);
        IVsaItem GetItem(string itemName);
        void RemoveItem(string itemName);
        void RemoveItem(IVsaItem item);
        int GetItemCount();
        IVsaItem GetItemAtIndex(int index);
        void RemoveItemAtIndex(int index);
        object GetObject();
        IVsaItem CreateDynamicItem(string itemName, VsaItemType type);
    }
}

