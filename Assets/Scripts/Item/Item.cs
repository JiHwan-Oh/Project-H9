using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������: �������� ����ϴ� weaponItem, usableItem, otherItem���� �����ϴ� ���� ���ƺ���.

/// <summary>
/// Item�� �Ӽ��� �����ϰ� �ʱ�ȭ�ϴ� Ŭ����
/// </summary>
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

    /// <summary>
    /// ItemTable���� �� ���� �Է¹޾Ƽ� �������� �ʱ�ȭ�մϴ�.
    /// </summary>
    /// <param name="list"> ItemTable���� ������ �� ���� ���ڿ� </param>
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

/// <summary>
/// �������� �Ӽ��� ���� ���� �� ����� �����ϴ� Ŭ����
/// </summary>
public class Item
{
    public ItemInfo itemInfo { get; private set; }

    public Item(ItemInfo itemInfo)
    {
        this.itemInfo = itemInfo;
    }
}
