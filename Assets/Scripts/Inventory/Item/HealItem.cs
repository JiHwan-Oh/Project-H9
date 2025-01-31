using UnityEngine;

public class HealItem : Item
{
    public HealItem(ItemData data) : base(data)
    { }
    
    public override bool Use(Unit user, Vector3Int target)
    {
        user.stat.Recover(StatType.CurHp, GetData().itemEffectAmount, out var appliedValue);

        stackCount--;
        
        UIManager.instance.onHealed.Invoke(user, appliedValue, Damage.Type.Heal);
        GameManager.instance.playerInventory.CollectZeroItem();
        return true;
    }

    public override bool TryEquip()
    {
        return false;
    }
}