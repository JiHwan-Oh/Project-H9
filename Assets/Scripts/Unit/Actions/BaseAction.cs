using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseAction : MonoBehaviour, IUnitAction
{
    public abstract ActionType GetActionType();
    public abstract bool CanExecute(Vector3Int target);
    public abstract void Execute(Vector3Int targetPos, Action _onActionComplete);
    public abstract int GetCost();

    public UnityEvent onActionStarted;
    public UnityEvent onActionFinished;

    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;
    public void Setup(Unit unit)
    {
        this.unit = unit;
    }
    public Unit GetUnit() => unit;
    public bool IsActive() => isActive;

    protected void StartAction(Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        unit.activeUnitAction = this;
        isActive = true;
        
        onActionStarted.Invoke();
    }

    protected void FinishAction()
    {
        isActive = false;
        //unit.UnitActionFinish(this);
        onActionComplete();

        onActionFinished.Invoke();
    }
}