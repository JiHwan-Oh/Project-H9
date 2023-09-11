using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FanningAction : BaseAction
{
    //todo : 애니메이션 확정되면 다 Frame단위 Int로 변경
    private const float TURNING_TIME = 0.5f;
    private const float SHOT_TIME = 0.1f;
    private const float COOL_OFF_TIME = 0.5f;
    
    private Unit _target;
    private int _shotCount;

    public override ActionType GetActionType()
    {
        return ActionType.Panning;
    }

    public override void SetTarget(Vector3Int targetPos)
    {
        _target = FieldSystem.unitSystem.GetUnit(targetPos);
        Debug.Log("Panning Target : " + _target);
    }

    public override int GetCost()
    {
        return 4;
    }

    public override int GetAmmoCost()
    {
        return unit.weapon.currentAmmo;
    }

    public override bool CanExecuteImmediately()
    {
        return false;
    }

    public override bool IsSelectable()
    {
        if (unit.weapon.currentAmmo < 0) return false;
        if (unit.hasAttacked) return false;

        return true;
    }

    public override bool CanExecute()
    {
        if (_target is null) return false;
        if (IsThereWallBetweenUnitAndThis(_target.hexPosition)) return false;
        
        return true;
    }

    public override void Execute(Action onActionComplete)
    {
        _shotCount = unit.weapon.currentAmmo;
        StartAction(onActionComplete);
    }
    
    private bool IsThereWallBetweenUnitAndThis(Vector3Int targetPos)
    {
        return !FieldSystem.tileSystem.RayThroughCheck(unit.hexPosition, targetPos);
    }

    private const float ROTATION_SPEED = 10f;
    protected override IEnumerator ExecuteCoroutine()
    {
        unit.animator.SetTrigger(PANNING);
        
        Transform tsf;
        Vector3 aimDirection = (Hex.Hex2World(_target.hexPosition) - (tsf = transform).position).normalized;
        aimDirection.y = 0;

        float totalTime = 0;
        while ((totalTime += Time.deltaTime) > TURNING_TIME)
        {
            transform.forward = Vector3.Lerp(tsf.forward, aimDirection, Time.deltaTime * ROTATION_SPEED);
            yield return null;
        }
        transform.forward = aimDirection;

        for (int i = 0; i < _shotCount; i++)
        {
            yield return new WaitForSeconds(SHOT_TIME);

            unit.weapon.currentAmmo--;

            bool hit = unit.weapon.GetFinalHitRate(_target) - 0.1f > UnityEngine.Random.value;
            Debug.Log(hit ? "뱅" : "빗나감");
            if (VFXHelper.TryGetGunFireFXInfo(unit.weapon.GetWeaponType(), out var fxKey, out var fxTime))
            {
                var gunpointPos = unit.weapon.weaponModel.GetGunpointPosition();
                VFXManager.instance.TryInstantiate(fxKey, fxTime, gunpointPos);
            }

            if (VFXHelper.TryGetTraceOfBulletFXKey(unit.weapon.GetWeaponType(), out var fxBulletLine, out var traceTime))
            {
                var startPos = unit.weapon.weaponModel.GetGunpointPosition();
                var destPos = _target.transform.position + Vector3.up;
                if (!hit) destPos += new Vector3(UnityEngine.Random.value * 2 - 1, UnityEngine.Random.value * 2 - 1, UnityEngine.Random.value * 2 - 1);
                VFXManager.instance.TryLineRender(fxBulletLine, traceTime, startPos, destPos);
            }

            if (hit)
            {
                unit.weapon.Attack(_target, out var isHeadShot);
                if (VFXHelper.TryGetBloodingFXKey(unit.weapon.GetWeaponType(), out var fxBloodingKey, out var bloodingTime))
                {
                    var startPos = _target.transform.position + Vector3.up;
                    VFXManager.instance.TryInstantiate(fxBloodingKey, bloodingTime, startPos);
                }
            }
        }

        yield return new WaitForSeconds(COOL_OFF_TIME);
        
        unit.hasAttacked = true;
        unit.animator.SetTrigger(IDLE);
        FinishAction();
    }
}
