//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// �÷��̾ ���� ���� �����۵��� �������� �з��� �°� �ѹ��� �����ϴ� Ŭ����
///// </summary>
//public class Inventory
//{
//    public List<Item> items { get; private set; }

//    public List<Item> weaponItems { get; private set; }
//    public List<Item> usableItems { get; private set; }
//    public List<Item> otherItems { get; private set; }

//    public Inventory()
//    {
//        items = new List<Item>();
//        weaponItems = new List<Item>();
//        usableItems = new List<Item>();
//        otherItems = new List<Item>();
//    }

//    /// <summary>
//    /// �κ��丮�� �������� �߰��մϴ�.
//    /// </summary>
//    /// <param name="item"> �߰��� ������ </param>
//    public void AddItem(Item item)
//    {
//        items.Add(item);
//        if (item.itemInfo.category == ItemInfo.ItemCategory.Weapon)
//        {
//            weaponItems.Add(item);
//        }
//        else if (item.itemInfo.category == ItemInfo.ItemCategory.Usable)
//        {
//            usableItems.Add(item);
//        }
//        else if (item.itemInfo.category == ItemInfo.ItemCategory.Other)
//        {
//            otherItems.Add(item);
//        }
//        else
//        {
//            Debug.Log("�� �� ���� ī�װ��� �������Դϴ�.");
//        }
//    }

//    /// <summary>
//    /// �κ��丮���� �������� �����մϴ�.
//    /// </summary>
//    /// <param name="index"> ������ ������ </param>
//    public void DeleteItem(Item item) 
//    {
//        DeleteItemAtEachCategory(items, item.itemInfo.index);
//        if (item.itemInfo.category == ItemInfo.ItemCategory.Weapon)
//        {
//            Debug.Log("w");
//            DeleteItemAtEachCategory(weaponItems, item.itemInfo.index);
//        }
//        else if (item.itemInfo.category == ItemInfo.ItemCategory.Usable)
//        {
//            Debug.Log("u");
//            DeleteItemAtEachCategory(usableItems, item.itemInfo.index);
//        }
//        else if (item.itemInfo.category == ItemInfo.ItemCategory.Other)
//        {
//            Debug.Log("o");
//            DeleteItemAtEachCategory(otherItems, item.itemInfo.index);
//        }
//        else
//        {
//            Debug.Log("�� �� ���� ī�װ��� �������Դϴ�.");
//        }
//    }

//    /// <summary>
//    /// �ϳ��� ������ �з����� �������� �����մϴ�.
//    /// </summary>
//    /// <param name="items"> ������ �������� �з� </param>
//    /// <param name="index"> ������ �������� ������ȣ </param>
//    private void DeleteItemAtEachCategory(List<Item> items, int index)
//    {
//        foreach (Item item in items)
//        {
//            if (item.itemInfo.index == index)
//            {
//                ItemInfo.ItemCategory itemCategory = item.itemInfo.category;
//                items.Remove(item);
//                return;
//            }
//        }
//        Debug.Log("����Ʈ�� �������� �ʴ� �������� ���� �õ��߽��ϴ�.");
//    }
//}

///// <summary>
///// ���ӿ��� ���Ǵ� �����۰� �������� ȹ��, �̵� ���� ����� �����ϴ� Ŭ����
///// </summary>
//public class ItemManager : Generic.Singleton<ItemManager>
//{
//    private List<ItemInfo> _itemInformations;
//    private Inventory _inventory;
//    public int money { get; private set; }

//    public GameObject inst;

//    private new void Awake()
//    {
//        base.Awake();

//        InitItemInfo();
//        _inventory = new Inventory();
//        money = 10;
//    }
//    private void InitItemInfo()
//    {
//        List<List<string>> _itemTable = FileRead.Read("ItemTable");
//        if (_itemTable == null)
//        {
//            Debug.Log("item table�� �о���� ���߽��ϴ�.");
//            return;
//        }

//        _itemInformations = new List<ItemInfo>();
//        for (int i = 0; i < _itemTable.Count; i++)
//        {
//            ItemInfo _itemInfo = new ItemInfo(_itemTable[i]);
//            _itemInformations.Add(_itemInfo);
//        }
//    }

//    /// <summary>
//    /// �Էµ� ������ȣ�� ������ ���̺��� �о�� ������ �߿� �����ϴ��� Ȯ���մϴ�.
//    /// </summary>
//    /// <param name="index"></param>
//    /// <returns>
//    /// �����Ѵٸ� �ش� �������� ItemInfo�� ��ȯ�մϴ�.
//    /// �������� �ʴ´ٸ� null�� ��ȯ�մϴ�.
//    /// </returns>
//    private ItemInfo FindItemInfoByIndex(int index)
//    {
//        for (int i = 0; i < _itemInformations.Count; i++)
//        {
//            if (index == _itemInformations[i].index)
//            {
//                return _itemInformations[i];
//            }
//        }

//        Debug.Log("�ش� �ε����� �������� ã�� ���߽��ϴ�.");
//        return null;
//    }

//    /// <summary>
//    /// �÷��̾��� �κ��丮�� �������� �߰��մϴ�.
//    /// </summary>
//    /// <param name="index"> �߰��� �������� ������ȣ </param>
//    public void AddItem(int index) 
//    {
//        ItemInfo fountItemInfo = FindItemInfoByIndex(index);
//        if (fountItemInfo == null)
//        {
//            Debug.Log("������ ȹ�濡 �����߽��ϴ�.");
//            return;
//        }

//        Item item = new Item(fountItemInfo);
//        _inventory.AddItem(item);


//    }

//    /// <summary>
//    /// �÷��̾��� �κ��丮���� �������� �����մϴ�.
//    /// </summary>
//    /// <param name="index"> ������ �������� ������ȣ </param>
//    public void DeleteItem(int index)
//    {
//        ItemInfo fountItemInfo = FindItemInfoByIndex(index);
//        if (fountItemInfo == null)
//        {
//            Debug.Log("������ ���ſ� �����߽��ϴ�. item index: " + index);
//            return;
//        }

//        Item item = new Item(fountItemInfo);
//        _inventory.DeleteItem(item);
//    }

//    /// <summary>
//    /// �÷��̾��� �κ��丮���� �������� ã�Ƽ� ��ȯ�մϴ�.
//    /// </summary>
//    /// <param name="index"> ã���� �ϴ� �������� ������ȣ </param>
//    /// <returns>
//    /// �������� �κ��丮�� ������ ��� �ش� �����۵��� �迭�� ��ȯ�մϴ�.
//    /// �������� ���� ��� null�� ��ȯ�մϴ�.
//    /// </returns>
//    public List<Item> GetItem(int index)
//    {
//        List<Item> findItems = new List<Item>();
//        for (int i = 0; i < _inventory.items.Count; i++)
//        {
//            if (_inventory.items[i].itemInfo.index == index)
//            {
//                findItems.Add(_inventory.items[i]);
//            }
//        }

//        if (findItems.Count != 0)
//        {
//            return findItems;
//        }
//        Debug.Log("�ش� �ε����� �������� ã�� ���߽��ϴ�. �ε���: " + index);
//        return null;
//    }

//    /// <summary>
//    /// �÷��̾��� �κ��丮�� ��ȯ�մϴ�.
//    /// </summary>
//    /// <returns>
//    /// �÷��̾��� �κ��丮�� ������ ��� �κ��丮�� ��ȯ�մϴ�.
//    /// �������� ���� ��� null�� ��ȯ�մϴ�. - ���� �߻�
//    /// </returns>
//    public Inventory GetInventory()
//    {
//        //Debug.Log("inventory test");
//        //Debug.Log("��ü ������ ����:" + _inventory.items.Count);
//        //foreach (Item item in _inventory.items)
//        //{
//        //    Debug.Log(item.itemInfo.index);
//        //}
//        //Debug.Log("���� ������ ����:" + _inventory.weaponItems.Count);
//        //foreach (Item item in _inventory.weaponItems)
//        //{
//        //    Debug.Log(item.itemInfo.index);
//        //}
//        //Debug.Log("�Һ� ������ ����:" + _inventory.usableItems.Count);
//        //foreach (Item item in _inventory.usableItems)
//        //{
//        //    Debug.Log(item.itemInfo.index);
//        //}
//        //Debug.Log("��Ÿ ������ ����:" + _inventory.otherItems.Count);
//        //foreach (Item item in _inventory.otherItems)
//        //{
//        //    Debug.Log(item.itemInfo.index);
//        //}

//        if (_inventory == null) 
//        {
//            Debug.LogError("�κ��丮�� �������� �ʽ��ϴ�.");
//            return null;
//        }
//        return _inventory;
//    }

//    /// <summary>
//    /// �ش� ������ȣ�� ������ ������ ��ȯ�մϴ�.
//    /// </summary>
//    /// <param name="index"> ã���� �ϴ� ������ ������ ������ȣ </param>
//    /// <returns>
//    /// ItemTable���� �о�� ������ ������ �ش� ������ȣ�� ������ ������ ������ ��� ������ ������ ��ȯ�մϴ�.
//    /// �������� ���� ��� null�� ��ȯ�մϴ�. - ���� �߻�
//    /// </returns>
//    public ItemInfo GetItemInfo(int index) 
//    {
//        foreach (ItemInfo info in _itemInformations) 
//        {
//            if (info.index == index) 
//            {
//                return info;
//            }
//        }
//        Debug.LogError("�ش� �ε����� ������ ������ ã�� �� �����ϴ�. index: " + index);
//        return null;
//    }

//    /// <summary>
//    /// �÷��̾��� �������� ����մϴ�. - ������
//    /// ����ϰ��� �ϴ� �������� �з��� �´� �Լ��� ȣ��˴ϴ�.
//    /// </summary>
//    /// <param name="index"> ����ϰ��� �ϴ� ������ ������ȣ </param>
//    public void UseItem(int index)
//    {
//        List<Item> item = GetItem(index);
//        if (item == null) 
//        {
//            Debug.LogError("����ϰ��� �ϴ� �������� �÷��̾�� �������� �ʽ��ϴ�.");
//            return;
//        }

//        if (item[0].itemInfo.category == ItemInfo.ItemCategory.Weapon) 
//        {
//            EquipWeaponItem(item[0]);
//        }
//        else if (item[0].itemInfo.category == ItemInfo.ItemCategory.Usable)
//        {
//            UseUsableItem(item[0]);
//        }
//        else
//        {
//        }
//    }
//    private void EquipWeaponItem(Item item) 
//    {
//        //Item currentWeapon = GetCurrnetWeapon();
//        //EquipWeapon(item);
//        //AddItem(currentWeapon);
//    }
//    private void UseUsableItem(Item item)
//    {
//    }

//    /// <summary>
//    /// �������� �Ǹ��մϴ�. �÷��̾��� �κ��丮���� �ش� ������ 1���� �����ϰ� �Ǹ��� �������� ��ġ��ŭ �Ӵϰ� �����մϴ�.
//    /// </summary>
//    /// <param name="index"> �Ǹ��ϰ��� �ϴ� �������� ������ȣ </param>
//    public void SellItem(int index)
//    {
//        GetInventory();
//        List<Item> item = GetItem(index);
//        money += item[0].itemInfo.price;
//        DeleteItem(index);
//    }

//    /// <summary>
//    /// �������� �����մϴ�. �÷��̾��� �����ۿ��� �ش� ������ 1���� �����˴ϴ�.
//    /// </summary>
//    /// <param name="index"> �����ϰ��� �ϴ� �������� ������ȣ </param>
//    public void DiscardItem(int index)
//    {
//        DeleteItem(index);
//    }
//}
