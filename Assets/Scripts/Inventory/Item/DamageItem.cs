using UnityEngine;

/// <summary>
/// Use �Լ��� ȣ���� ��, Target ��ġ ���� Unit�� ã�� ItemEffectAmount��ŭ�� �������� �����ϴ�.
/// </summary>
public class DamageItem : Item
{
    public DamageItem(ItemData data) : base(data)
    {
    }

    public override bool Use(Unit user, Vector3Int target)
    {
        var unit = FieldSystem.unitSystem.GetUnit(target);
        if (unit == null) return false;
        
        unit.TakeDamage(GetData().itemEffectAmount, user);
        stackCount--;
        return true;
    }

    public override bool TryEquip()
    {
        return false;
    }
}