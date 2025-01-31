using PassiveSkill;

public class StatUpWhileAction : BaseEffect, IDisplayableEffect
{
    public StatUpWhileAction(StatType statType, int amount) : base(statType, amount)
    {
    }

    public override PassiveEffectType GetEffectType() => PassiveEffectType.StatUpWhileAction;

    public override void OnConditionEnable()
    {
        if (!enable) unit.stat.Add(GetStatType(), GetAmount());
        enable = true;
    }

    public override void OnConditionDisable()
    {
    }
    public void OnTurnFinished(IUnitAction action)
    {
        if (!enable) return;
        enable = false;
        unit.stat.Subtract(GetStatType(), GetAmount());
        unit.RemoveDisplayableEffect(this);
    }

    #region IDISPLAYABLE_EFFECT
    protected override void EffectSetup()
    {
        unit.onFinishAction.AddListener(OnTurnFinished);
    }
    public int GetIndex() => passive.index;
    public int GetStack() => GetAmount();
    public int GetDuration() => 0;

    public bool CanDisplay()
    {
        if (passive is null) return false;
        if (passive.GetConditionType()[0] is ConditionType.Null) return false;
        return enable;
    }
    #endregion
}
