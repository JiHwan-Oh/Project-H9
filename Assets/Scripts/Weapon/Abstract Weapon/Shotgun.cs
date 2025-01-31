using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    private Vector3Int _targetHex;

    public Shotgun(WeaponData data) : base(data)
    {
    }

    public override ItemType GetWeaponType() => ItemType.Shotgun;
    public override float GetDistancePenalty() => 2;
    public override int GetRange()
    {
        return weaponRange + magazine.GetNextBullet().data.range + unitStat.shotgunAdditionalDamage;
    }

    public override void Attack(IDamageable target, out bool isCritical)
    {
        Debug.Log("Weapon attack Call" + " : " + nameIndex);

        isCritical = Random.value * 100 < unitStat.criticalChance + criticalChance + magazine.GetNextBullet().data.criticalChance;
        if (isCritical)
        {
            CriticalAttack(target);
        }
        else
        {
            NonCriticalAttack(target);
        }
    }

    public override int GetFinalDamage()
    {
        float baseDamage = (weaponDamage + magazine.GetNextBullet().data.damage + unitStat.shotgunAdditionalDamage);

        int range = GetRange();
        int distance = Hex.Distance(unit.hexPosition, _targetHex);

        int value = range - distance;
        int damage = Mathf.RoundToInt(baseDamage * Mathf.Pow(2, value));
        
        return damage;
    }

    public override int GetFinalCriticalDamage()
    {
        float dmg = GetFinalDamage();
        dmg += dmg * ((unitStat.shotgunCriticalDamage + criticalDamage + +magazine.GetNextBullet().data.criticalDamage) * 0.01f);

        return Mathf.RoundToInt(dmg);
    }

    public override float GetFinalHitRate(IDamageable target)
    {
        int range = GetRange();
        int distance = Hex.Distance(unit.hexPosition, target.GetHex());
        
        _targetHex = target.GetHex();

        float finalHitRate = distance <= range ? 100 : 0;

#if UNITY_EDITOR
        UIManager.instance.debugUI.SetDebugUI
            (finalHitRate, unit, (Unit)target, distance, weaponRange,
                unitStat.revolverAdditionalRange,
                GetDistancePenalty() *
                (distance > range ? REVOLVER_OVER_RANGE_PENALTY : 1));
#endif

        return finalHitRate;
    }

    private void NonCriticalAttack(IDamageable target)
    {
        int damage = GetFinalDamage();
        target.TakeDamage(damage, unit);
    }

    private void CriticalAttack(IDamageable target)
    {
        int damage = GetFinalCriticalDamage();
        target.TakeDamage(damage, unit, Damage.Type.Critical);
    }
}
