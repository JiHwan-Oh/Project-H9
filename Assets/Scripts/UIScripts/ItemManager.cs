using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemInfo
{
    public enum ItemCategory
    {
        Null,
        Weapon,
        Usable,
        Other
    }
    public int index { get; private set; }
    public int iconNumber { get; private set; }
    public string name { get; private set; }
    public string description { get; private set; }
    public ItemCategory category { get; private set; }
    public int price { get; private set; }
    public ItemInfo(List<string> list) 
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals(""))
            {
                list[i] = "0";
            }
        }

        index = int.Parse(list[0]);
        iconNumber = int.Parse(list[1]);
        name = list[2];
        description = list[3];
        category = (ItemCategory)int.Parse(list[4]);
        price = 5;//test
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

public class Inventory
{
    public List<Item> items { get; private set; }

    public List<Item> weaponItems { get; private set; }
    public List<Item> usableItems { get; private set; }
    public List<Item> otherItems { get; private set; }

    public Inventory()
    {
        items = new List<Item>();
        weaponItems = new List<Item>();
        usableItems = new List<Item>();
        otherItems = new List<Item>();
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        if (item.itemInfo.category == ItemInfo.ItemCategory.Weapon)
        {
            weaponItems.Add(item);
        }
        else if (item.itemInfo.category == ItemInfo.ItemCategory.Usable)
        {
            usableItems.Add(item);
        }
        else if (item.itemInfo.category == ItemInfo.ItemCategory.Other)
        {
            otherItems.Add(item);
        }
        else
        {
            Debug.Log("�� �� ���� ī�װ��� �������Դϴ�.");
        }
    }
    public void DeleteItem(int index) 
    {
        ItemInfo.ItemCategory itemCategory;
        itemCategory = DeleteItemAtEachCategory(items, index);
        if (itemCategory == ItemInfo.ItemCategory.Weapon)
        {
            Debug.Log("w");
            DeleteItemAtEachCategory(weaponItems, index);
        }
        else if (itemCategory == ItemInfo.ItemCategory.Usable)
        {
            Debug.Log("u");
            DeleteItemAtEachCategory(usableItems, index);
        }
        else if (itemCategory == ItemInfo.ItemCategory.Other)
        {
            Debug.Log("o");
            DeleteItemAtEachCategory(otherItems, index);
        }
        else
        {
            Debug.Log("�� �� ���� ī�װ��� �������Դϴ�.");
        }
    }
    private ItemInfo.ItemCategory DeleteItemAtEachCategory(List<Item> items, int index)
    {
        foreach (Item item in items)
        {
            if (item.itemInfo.index == index)
            {
                ItemInfo.ItemCategory itemCategory = item.itemInfo.category;
                items.Remove(item);
                return itemCategory;
            }
        }
        Debug.Log("����Ʈ�� �������� �ʴ� �������� ���� �õ��߽��ϴ�.");
        return ItemInfo.ItemCategory.Null;
    }
}
public class ItemManager : Generic.Singleton<ItemManager>
{

    private List<ItemInfo> _itemInformations;
    private Inventory _inventory;
    public int money { get; private set; }

    void Start()
    {
        InitItems();
        money = 10;
    }
    void InitItems()
    {
        List<List<string>> _itemTable = FileRead.Read("ItemTable");
        if (_itemTable == null)
        {
            Debug.Log("item table�� �о���� ���߽��ϴ�.");
            return;
        }

        _itemInformations = new List<ItemInfo>();
        for (int i = 0; i < _itemTable.Count; i++)
        {
            ItemInfo _itemInfo = new ItemInfo(_itemTable[i]);
            _itemInformations.Add(_itemInfo);
        }

        _inventory = new Inventory();
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
    public void AddItem(int index) 
    {
        int foundIndex = FindItemInfoByIndex(index);
        if (foundIndex == -1)
        {
            Debug.Log("������ ȹ�濡 �����߽��ϴ�.");
            return;
        }

        Item item = new Item(_itemInformations[foundIndex]);
        _inventory.AddItem(item);


    }
    public void DeleteItem(int index)
    {
        int foundIndex = FindItemInfoByIndex(index);
        if (foundIndex == -1)
        {
            Debug.Log("������ ���ſ� �����߽��ϴ�.");
            return;
        }

        _inventory.DeleteItem(index);
    }
    public List<Item> GetItem(int index)
    {
        List<Item> findItems = new List<Item>();
        for (int i = 0; i < _inventory.items.Count; i++)
        {
            if (_inventory.items[i].itemInfo.index == index)
            {
                findItems.Add(_inventory.items[i]);
            }
        }

        if (findItems.Count != 0)
        {
            return findItems;
        }
        Debug.Log("�ش� �ε����� �������� ã�� ���߽��ϴ�. �ε���: " + index);
        return null;
    }

    public Inventory GetInventory()
    {
        //Debug.Log("inventory test");
        //Debug.Log("��ü ������ ����:" + _inventory.items.Count);
        //foreach (Item item in _inventory.items)
        //{
        //    Debug.Log(item.itemInfo.index);
        //}
        //Debug.Log("���� ������ ����:" + _inventory.weaponItems.Count);
        //foreach (Item item in _inventory.weaponItems)
        //{
        //    Debug.Log(item.itemInfo.index);
        //}
        //Debug.Log("�Һ� ������ ����:" + _inventory.usableItems.Count);
        //foreach (Item item in _inventory.usableItems)
        //{
        //    Debug.Log(item.itemInfo.index);
        //}
        //Debug.Log("��Ÿ ������ ����:" + _inventory.otherItems.Count);
        //foreach (Item item in _inventory.otherItems)
        //{
        //    Debug.Log(item.itemInfo.index);
        //}
        return _inventory;
    }
    public ItemInfo GetItemInfo(int index) 
    {
        foreach (ItemInfo info in _itemInformations) 
        {
            if (info.index == index) 
            {
                return info;
            }
        }
        Debug.Log("�ش� �ε����� ������ ������ ã�� �� �����ϴ�. index: " + index);
        return null;
    }
    public void UseItem(int index)
    {
        List<Item> item = GetItem(index);
        if (item[0].itemInfo.category == ItemInfo.ItemCategory.Weapon) 
        {
            EquipWeaponItem(item[0]);
        }
        else if (item[0].itemInfo.category == ItemInfo.ItemCategory.Usable)
        {
            UseUsableItem(item[0]);
        }
        else
        {
        }
    }
    private void EquipWeaponItem(Item item) 
    {
        //Item currentWeapon = GetCurrnetWeapon();
        //EquipWeapon(item);
        //AddItem(currentWeapon);
    }
    private void UseUsableItem(Item item)
    {
    }
    public void SellItem(int index)
    {
        GetInventory();
        List<Item> item = GetItem(index);
        money += item[0].itemInfo.price;
        DeleteItem(index);
    }
    public void DiscardItem(int index)
    {
        DeleteItem(index);
    }
}
