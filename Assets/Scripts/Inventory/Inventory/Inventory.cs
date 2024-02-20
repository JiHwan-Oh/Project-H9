using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Serialization;

[Serializable]
public class Inventory : IInventory
{
    private int _gold;
    
    [SerializeField]
    private List<IItem> _items = new();

    public void AddItem(IItem item)
    {
        _items.Add(item);
        UIManager.instance.onInventoryChanged.Invoke();
    }

    public void DeleteItem(IItem item)
    {
        _items.Remove(item);
        UIManager.instance.onInventoryChanged.Invoke();
    }

    public IEnumerable<IItem> GetItems()
    {
        return _items;
    }

    public int GetItemCount(IItem item)
    {
        return _items.Count(i => i.Equals(item));
    }

    public int GetItemCount(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= _items.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(itemIndex));
        }
        return _items.Count(i => i.Equals(_items[itemIndex]));
    }
}