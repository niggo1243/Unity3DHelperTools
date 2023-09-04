using System.Collections.Generic;

namespace NikosAssets.Helpers.ScriptableObjectWrappers
{
    public abstract class BaseSOWList<ListItemType> : BaseSOW<List<ListItemType>>
    {
        public virtual void SetListItemAt(ListItemType listItem, int index) => GetValue()[index] = listItem;
        
        public virtual ListItemType GetListItemAt(int index) => GetValue()[index];

        public virtual void RemoveAt(int index) => GetValue().RemoveAt(index);
        
        public virtual bool Remove(ListItemType listItem) => GetValue().Remove(listItem);

        public virtual void AddItem(ListItemType listItem) => GetValue().Add(listItem);

        public virtual void AddExclusiveItem(ListItemType listItem)
        {
            if (!GetValue().Contains(listItem)) AddItem(listItem);
        }
    }
}
