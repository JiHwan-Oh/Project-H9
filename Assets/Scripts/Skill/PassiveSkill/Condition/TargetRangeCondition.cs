﻿using UnityEngine;

namespace PassiveSkill
{
    public class LessTargetRangeCondition : BaseCondition 
    {
        public override ConditionType GetConditionType() => ConditionType.TargetHighHp;

        protected override void ConditionSetup()
        {
            unit.onStartShoot.AddListener(SetTarget);
            unit.onFinishShoot.AddListener(TargetOff);
        }

        public LessTargetRangeCondition(float amt) : base(amt)
        { }

        private void SetTarget(Unit target)
        {
            var dist = Hex.Distance(unit.hexPosition, target.hexPosition);
            
            if(dist <= amount) passive.FullfillCondition(this);
            else passive.NotFullfillCondition(this);
        }

        private void TargetOff(Unit target, int damage, bool none, bool __)
        {
            passive.NotFullfillCondition(this);
        }
    }
    
    public class MoreTargetRangeCondition : BaseCondition
    {
        public override ConditionType GetConditionType() => ConditionType.TargetHpIs;

        protected override void ConditionSetup()
        {
            unit.onStartShoot.AddListener(SetTarget);
            unit.onFinishShoot.AddListener(TargetOff);
        }

        public MoreTargetRangeCondition(float amt) : base(amt)
        { }

        private void SetTarget(Unit target)
        {
            var dist = Hex.Distance(unit.hexPosition, target.hexPosition);
            
            if(dist >= (int)amount) passive.FullfillCondition(this);
            else passive.NotFullfillCondition(this);
        }

        private void TargetOff(Unit target, int damage, bool none, bool __)
        {
            passive.NotFullfillCondition(this);
        }
    }
    
    public class SameTargetRangeCondition : BaseCondition
    {
        public SameTargetRangeCondition(float amt) : base(amt)
        { }
        
        public override ConditionType GetConditionType() => ConditionType.TargetLowHp;

        protected override void ConditionSetup()
        {
            unit.onStartShoot.AddListener(SetTarget);
            unit.onFinishShoot.AddListener(TargetOff);
        }

        private void SetTarget(Unit target)
        {
            var dist = Hex.Distance(unit.hexPosition, target.hexPosition);
            
            if(dist == (int)amount) passive.FullfillCondition(this);
            else passive.NotFullfillCondition(this);
        }

        private void TargetOff(Unit target, int damage, bool none, bool __)
        {
            passive.NotFullfillCondition(this);
        }
    }
    public class TargetOnSweetSpotCondition : BaseCondition
    {
        public TargetOnSweetSpotCondition(float amt) : base(amt)
        { }

        public override ConditionType GetConditionType() => ConditionType.TargetOnSweetSpot;

        protected override void ConditionSetup()
        {
            unit.onStartShoot.AddListener(SetTarget);
            unit.onFinishShoot.AddListener(TargetOff);
            unit.onKill.AddListener((u) => { Debug.LogError(u); });
        }

        private void SetTarget(Unit target)
        {
            var dist = Hex.Distance(unit.hexPosition, target.hexPosition);
            if (unit.weapon is not Repeater repeater) return;

            if (dist == repeater.GetSweetSpot()) passive.FullfillCondition(this);
            else passive.NotFullfillCondition(this);
        }

        private void TargetOff(Unit target, int damage, bool none, bool __)
        {
            passive.NotFullfillCondition(this);
        }
    }
}