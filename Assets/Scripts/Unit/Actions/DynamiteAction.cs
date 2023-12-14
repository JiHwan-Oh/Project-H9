﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynamiteAction : BaseAction
{
    public override ActionType GetActionType()
    {
        return ActionType.Dynamite;
    }
    
    public override void SetTarget(Vector3Int targetPos)
    {
        _center = targetPos;
        _targets = FieldSystem.unitSystem.GetUnitListInRange(targetPos, radius);
    }

    public override bool CanExecuteImmediately()
    {
        return false;
    }

    public override bool IsSelectable()
    {
        return GetCost() <= unit.currentActionPoint;
    }

    public override bool CanExecute()
    {
        if (FieldSystem.tileSystem.GetTile(_center) is null)
        {
            Debug.LogWarning("center tile is null");
            return false;
        }

        if (range < Hex.Distance(unit.hexPosition, _center))
        {
            Debug.LogWarning("Too Far to throw bomb");
            return false;
        }

        return true;
    }

    public int GetExplosionRange() => radius;
    public int GetThrowingRange() => Mathf.Clamp(range, 0, unit.stat.sightRange);

    protected override IEnumerator ExecuteCoroutine()
    {
        Explode();
        yield break;
    }

    #region PRIVATE

    private Vector3Int _center;
    private List<Unit> _targets;

    private void Explode()
    {
        foreach(var target in _targets)
        {
            target.TakeDamage(damage, unit);
            target.TryAddStatus(new Burning(damage, 10, unit));  //for test
        }
    }

    #endregion
}