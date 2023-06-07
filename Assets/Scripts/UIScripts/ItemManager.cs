using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemInfo
{
    public int index { get; private set; }
    public int iconNumber { get; private set; }
    public string name { get; private set; }
    public string description { get; private set; }
    public ItemManager.ItemCategory category { get; private set; }
    public ItemInfo(List<string> list) 
    {

    }
}
public class Item
{
    public ItemInfo itemInfo { get; private set; }

    public Item(ItemInfo itemInfo) 
    {
        this.itemInfo = itemInfo;
    }
}
public class ItemManager : Generic.Singleton<ItemManager>
{
    public enum ItemCategory
    {
        Null,
        Weapon,
        Usable,
        Other
    }

    private List<ItemInfo> _itemInformations;
    private List<Item> _items;

    private List<Item> _weaponItems;
    private List<Item> _usableItems;
    private List<Item> _otherItems;

    void Start()
    {
        InitItems();
    }
    void InitItems()
    {
        List<List<string>> _itemTable = SkillRead.Read("ItemTable");
        if (_itemTable == null)
        {
            Debug.Log("item table�� �о���� ���߽��ϴ�.");
            return;
        }

        List<ItemInfo> _itemInformations = new List<ItemInfo>();
        for (int i = 0; i < _itemTable.Count; i++)
        {
            ItemInfo _itemInfo = new ItemInfo(_itemTable[i]);
            _itemInformations.Add(_itemInfo);
        }

        _items = new List<Item>();
        _weaponItems = new List<Item>();
        _usableItems = new List<Item>();
        _otherItems = new List<Item>();
    }
    public void AddItem(int index) 
    {
        int foundIndex = FindItemInfoByIndex(index);
        if (foundIndex == -1)
        {
            Debug.Log("������ ȹ�濡 �����߽��ϴ�.");
            return;
        }

        Item item = new Item(_itemInformations[foundIndex]);
        _items.Add(item);
        if (item.itemInfo.category == ItemCategory.Weapon)
        {
            _weaponItems.Add(item);
        }
        else if (item.itemInfo.category == ItemCategory.Usable)
        {
            _usableItems.Add(item);
        }
        else if (item.itemInfo.category == ItemCategory.Other)
        {
            _otherItems.Add(item);
        }
        else 
        {
            Debug.Log("�� �� ���� ī�װ��� �������Դϴ�.");
        }

    }
    private int FindItemInfoByIndex(int index)
    {
        for (int i = 0; i < _itemInformations.Count; i++)
        {
            if (index == _itemInformations[i].index)
            {
                return i;
            }
        }

        Debug.Log("�ش� �ε����� �������� ã�� ���߽��ϴ�.");
        return -1;
    }

    public void DeleteItem(int index)
    {
    }

    public List<Item> GetAllItems()
    {
        return _items;
    }
    public List<Item> GetWeaponItems()
    {
        return _weaponItems;
    }
    public List<Item> GetUsableItems()
    {
        return _usableItems;
    }
    public List<Item> GetOtherItems()
    {
        return _otherItems;
    }
    public List<Item> GetItem(int index)
    {
        List<Item> findItem = new List<Item>();
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].itemInfo.index == index)
            {
                findItem.Add(_items[i]);
            }
        }

        if (findItem.Count != 0)
        {
            return findItem;
        }
        Debug.Log("�ش� �ε����� �������� ã�� ���߽��ϴ�. �ε���: " + index);
        return null;
    }
}
