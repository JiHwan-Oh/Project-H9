using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class Item : IItem
{

    public UnityEvent OnItemChanged { get; set; }
    private readonly ItemData _data;
        private readonly List<IItemAttribute> _attributes;
        protected int stackCount;
        public Item(ItemData data)
        {
            _data = data;
            _attributes = new List<IItemAttribute>();
            stackCount = 1;
            OnItemChanged = new UnityEvent();
        }

        public static Item CreateItem(ItemData data)
        {
            //create item by ItemType
            Item item = null;
            
            switch (data.itemType)
            {
                case ItemType.Null:
                    //exception
                    Debug.LogError("ItemType is Null, index is " + data.id);
                    break;
                case ItemType.Etc:
                    item = new EtcItem(data);
                    break;
                case ItemType.Character:
                case ItemType.Revolver:
                case ItemType.Repeater:
                case ItemType.Shotgun:
                    item = new WeaponItem(data);
                    break;
                case ItemType.Heal:
                    item = new HealItem(data);
                    break;
                case ItemType.Damage:
                    item = new DamageItem(data);
                    break;
                case ItemType.Cleanse:
                    item = new CleanseItem(data);
                    break;
                case ItemType.Buff:
                    item = new BuffItem(data);
                    break;
                case ItemType.Debuff:
                    item = new DebuffItem(data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return item;
        }

        public ItemData GetData() => _data;
        public IEnumerable<IItemAttribute> GetAttributes() => _attributes;
        public bool Use(Unit user) => Use(user, Vector3Int.zero);
        public abstract bool Use(Unit user, Vector3Int target);
        
        public int GetStackCount() => stackCount;
        public void SetStackCount(int count)
        {
            stackCount = count;
        }

        public abstract bool TryEquip();

        public bool TrySplit(int count, out IItem newItem)
        {
            newItem = null;
            if (count > 0 && count < stackCount)
            {
                stackCount -= count;
                newItem = (Item)MemberwiseClone();
                ((Item)newItem).stackCount = count;
                return true;
            }
            return false;
        }

        public bool IsImmediate()
        {
            return GetData().itemType is ItemType.Buff or ItemType.Cleanse or ItemType.Heal;
        }

        public bool IsUsable()
        {
            switch (GetData().itemType)
            {
                case ItemType.Null:
                case ItemType.Etc:
                case ItemType.Character:
                case ItemType.Revolver:
                case ItemType.Repeater:
                case ItemType.Shotgun:
                    return false;
                //else : return true;
                default:
                    return true;
            }
        }

    public static Item operator +(Item item1, Item item2)
    {
        //check same id and max storage
        if (item1.GetData().id == item2.GetData().id && item1.stackCount + item2.stackCount <= item1.GetData().itemMaxStorage)
        { 
            item1.stackCount += item2.stackCount;
            return item1;
        }
        Debug.LogError("Cannot stack items");
        return item1;
    }

    public ItemSaveWrapper GetItemSaveData() 
    {
        var sw = new ItemSaveWrapper();
        sw.index = _data.id;
        sw.stack = stackCount;
        return sw;
    }
}

[SerializeField]
public struct ItemSaveWrapper
{
    public int index;
    public int stack;
}