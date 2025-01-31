﻿
using System;
using UnityEngine;
using PassiveSkill;

public class StatUpDependedOnCondition : BaseEffect, IDisplayableEffect
{
    public StatUpDependedOnCondition(StatType statType, int amount) : base(statType, amount)
    { }

    protected override void EffectSetup()
    { }

    public override PassiveEffectType GetEffectType()
    {
        return PassiveEffectType.StatUpDependedOnCondition;
    }

    public override void OnConditionEnable()
    {
        if (enable) return;
        enable = true;
        unit.stat.Add(GetStatType(), GetAmount());
    }

    public override void OnConditionDisable()
    {
        if (enable is false) return;
        enable = false;

        unit.stat.Subtract(GetStatType(), GetAmount());
    }

    #region IDISPLAYABLE_EFFECT

    public int GetIndex() => passive.index;
    public int GetStack() => GetAmount();
    public int GetDuration() => IDisplayableEffect.NONE;

    public bool CanDisplay()
    {
        if (passive is null) return false;
        if (passive.GetConditionType()[0] is ConditionType.Null) return false;
        return enable;
    }
    #endregion
}