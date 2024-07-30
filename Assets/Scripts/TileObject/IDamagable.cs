using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    void TakeDamage(Damage damageContext);
    Vector3Int GetHex();
    int GetCurrentHp();
    int GetMaxHp();
    
    UnityEvent<int, int> OnHpChanged { get; }
    int GetHitRateModifier(Unit attacker);
}