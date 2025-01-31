using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitModel : MonoBehaviour
{
    #region Triggers
    private static readonly int IDLE = Animator.StringToHash("Idle");
    private static readonly int START_TURN = Animator.StringToHash("StartTurn");
    private static readonly int SHOOT = Animator.StringToHash("Shoot");
    private static readonly int RELOAD = Animator.StringToHash("Reload");
    private static readonly int MOVE = Animator.StringToHash("Move");
    private static readonly int GET_HIT1 = Animator.StringToHash("GetHit1");
    private static readonly int GET_HIT2 = Animator.StringToHash("GetHit2");
    private static readonly int DIE = Animator.StringToHash("Die");
    private static readonly int FANNING = Animator.StringToHash("Fanning");
    private static readonly int DYNAMITE = Animator.StringToHash("Dynamite");
    
    private static readonly int FRACTURED = Animator.StringToHash("Injured");
    #endregion
    
    [Header("Transform")]
    public Transform root;
    public Transform hand;
    public Transform chest;
    public Transform waist;
    public Transform triggerFinger;
    public Transform head;
    
    [Space(10 )]
    public WeaponModel weaponModel;
    public Animator animator;
    public Unit unit;
    
    private SkinnedMeshRenderer _visual;

    [SerializeField] private bool isWesternFrontierAsset = true;

    public void Setup(Unit unit)
    {
        if (TryGetComponent(out animator) is false)
        {
            Debug.LogError("Animator is null");
            throw new Exception();
            return;
        }

        this.unit = unit;
        _visual = transform.GetComponentInChildren<SkinnedMeshRenderer>();
        
        _deadFlag = false;
        ResetStatusEffectTrigger();
        
        #region EVENTS

        unit.onHit.AddListener(OnHit);
        unit.onTurnStart.AddListener(OnStartTurn);
        unit.onStatusEffectChanged.AddListener(OnStatusEffectChanged);
        unit.onMoved.AddListener((a) => transform.localPosition = Vector3.zero);

        #endregion
    }

    public void SetupWeaponModel(Weapon weapon)
    {
        if(weaponModel != null)
            Destroy(weaponModel.gameObject);
        
        if (GameManager.instance.CompareState(GameState.Combat))
        {
            weaponModel = Instantiate(weapon.model, hand).GetComponent<WeaponModel>();
            weaponModel.SetHandPosRot(isWesternFrontierAsset);
            SetAnimator(weapon.GetWeaponType());
        }

        else
        {
            weaponModel = Instantiate(weapon.model, waist).GetComponent<WeaponModel>();
            weaponModel.SetWaistPosRot(isWesternFrontierAsset);
            SetAnimator(ItemType.Null);
        }
    }

    public Vector3 GetGunpointPosition()
    {
        return weaponModel.GetGunpointPosition();
    }

    public bool isVisible
    {
        //get method for visual
        get => _visual.enabled;
        //set method for visual and weapon
        set
        {
            _visual.enabled = value;
            SetWeaponVisible();
        }
    }
    #region PRIVATE
    private void SetAnimator(ItemType type)
    {
        animator.runtimeAnimatorController =
            (RuntimeAnimatorController)Resources.Load("Animator/" 
                                                      + (type is ItemType.Null ? "Standing" : type) +
                                                      " Animator Controller");
        
        animator.SetTrigger(IDLE);
        animator.ResetTrigger(IDLE);
    }


    private bool _deadFlag = false;

    private void OnHit(Unit attacker, int damage)
    {
        if (_deadFlag) return;
        
        animator.SetTrigger(GET_HIT1);
        //if hp is 0, die
        if (unit.hp <= 0)
        {
            animator.SetBool(DIE, true);
            _deadFlag = true;
        }
    }

    private void OnStartTurn(Unit owner)
    {
        animator.SetTrigger(START_TURN);
    }
    
    #region STATUS EFFECT FX

    private bool _bleedingTrigger;
    private bool _stunTrigger;
    private bool _fireTrigger;
    
    private readonly Dictionary<StatusEffectType, GameObject> _statusEffectFX = new ();

    private void ResetStatusEffectTrigger()
    {
        _bleedingTrigger = false;
        _stunTrigger = false;
        _fireTrigger = false;
    }
    
    private void OnStatusEffectChanged()
    {
        if (unit.HasStatusEffect(StatusEffectType.UnArmed))
        {
            SetAnimator(ItemType.Character);
        }
        else
        {
            SetAnimator(unit.weapon.GetWeaponType());
        }
        SetWeaponVisible();
        
        animator.SetFloat(FRACTURED, unit.HasStatusEffect(StatusEffectType.Fracture) ? 1.0f : 0.0f);
    
        #region VFX
        //save regacy trigger states
        bool bleedingBefore = _bleedingTrigger;
        bool stunBefore = _stunTrigger;
        bool burningBefore = _fireTrigger;
        
        //reset all trigger states
        bool bleedingNow = unit.HasStatusEffect(StatusEffectType.Bleeding);
        bool stunNow = unit.HasStatusEffect(StatusEffectType.Stun);
        bool fireNow = unit.HasStatusEffect(StatusEffectType.Burning);

        //check if trigger changed false to true, create fx
        //else, remove fx
        if (bleedingNow is true && bleedingBefore is false)
        {
            _bleedingTrigger = true;
            if (VFXHelper.TryGetStatusEffectFXKey(StatusEffectType.Bleeding, out var fxKey, out var time))
            {
                VFXManager.instance.TryInstantiate(fxKey, time, waist.position, parent : waist);
                //insert in dictionary
                _statusEffectFX.Add(StatusEffectType.Bleeding, VFXManager.instance.GetLastInstantiated());
            }
        }
        else if (bleedingNow is false && bleedingBefore is true)
        {
            _bleedingTrigger = false;
            
            //find fx and remove
            foreach (var fx in _statusEffectFX)
            {
                if (fx.Key == StatusEffectType.Bleeding)
                {
                    StartCoroutine(ParticleDestroyCoroutine(0.5f, fx.Value));
                    _statusEffectFX.Remove(fx.Key);
                    break;
                }
            }
        }
        
        //check if trigger changed false to true, create fx
        //else, remove fx
        if (stunNow is true && stunBefore is false)
        {
            _stunTrigger = true;
            if (VFXHelper.TryGetStatusEffectFXKey(StatusEffectType.Stun, out var fxKey, out var time))
            {
                VFXManager.instance.TryInstantiate(fxKey, time, head.position + Vector3.up, parent:transform);
                //insert in dictionary
                _statusEffectFX.Add(StatusEffectType.Stun, VFXManager.instance.GetLastInstantiated());
            }
        }
        else if (stunNow is false && stunBefore is true)
        {
            _stunTrigger = false;
            
            //find fx and remove
            foreach (var fx in _statusEffectFX)
            {
                if (fx.Key == StatusEffectType.Stun)
                {
                    StartCoroutine(ParticleDestroyCoroutine(0.5f, fx.Value));
                    _statusEffectFX.Remove(fx.Key);
                    break;
                }
            }
        }
        
        //check if trigger changed false to true, create fx
        //else, remove fx
        if (fireNow is true && burningBefore is false)
        {
            _fireTrigger = true;
            if (VFXHelper.TryGetStatusEffectFXKey(StatusEffectType.Burning, out var fxKey, out var time))
            {
                VFXManager.instance.TryInstantiate(fxKey, time, transform.position, parent : transform);
                //insert in dictionary
                _statusEffectFX.Add(StatusEffectType.Burning, VFXManager.instance.GetLastInstantiated());
            }
        }
        else if (fireNow is false && burningBefore is true)
        {
            _fireTrigger = false;
            
            //find fx and remove
            foreach (var fx in _statusEffectFX)
            {
                if (fx.Key == StatusEffectType.Burning)
                {
                    StartCoroutine(ParticleDestroyCoroutine(0.5f, fx.Value));
                    _statusEffectFX.Remove(fx.Key);
                    break;
                }
            }
        }

        IEnumerator ParticleDestroyCoroutine(float time, GameObject fx)
        {
            //while 'time', scale lerp to 0
            float timer = 0;
            while (timer < time)
            {
                timer += Time.deltaTime;
                fx.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / time);
                yield return null;
            }
            
            Destroy(fx);
        }

        #endregion
    }
    
    #endregion

    private void SetWeaponVisible()
    {
        //if unarmed, set weapon invisible
        if (unit.HasStatusEffect(StatusEffectType.UnArmed))
        {
            weaponModel.isVisible = false;
            return;
        }
        
        //else, follow model's visible
        weaponModel.isVisible = isVisible;
    }
    #endregion
    
    #if UNITY_EDITOR
    
    [ContextMenu("Setup")]
    private void Setup()
    {
        RemoveDisabled();
        SetTransform();
    }

    public void RemoveDisabled()
    {
        //find disabled object and remove
        var list = new List<Transform>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf is false)
            {
                list.Add(child);
            }
        }
        
        foreach (var child in list)
        {
            DestroyImmediate(child.gameObject, true);
        }
    }
    
    public void SetTransform()
    {
        //set all transform

        //root
        root = TryFind("Root", transform, out var tsf) ? tsf : null;
        hand = TryFind("IndexFinger_01 1", transform, out tsf) ? tsf : null;
        chest = TryFind("Spine_03", transform, out tsf) ? tsf : null;
        waist = TryFind("Hips", transform, out tsf) ? tsf : null;
        triggerFinger = TryFind("IndexFinger_04", transform, out tsf) ? tsf : null;
        head = TryFind("Head", transform, out tsf) ? tsf : null;
    }
    
    private static bool TryFind(string path, Transform current, out Transform target)
    {
        //using recursion, find transform
        if (current.name == path)
        {
            target = current;
            return true;
        }
        
        foreach (Transform child in current)
        {
            if (TryFind(path, child, out target))
            {
                return true;
            }
        }
        
        target = null;
        return false;
    }
    #endif

    
    /// <summary>
    /// Animation Event를 받아 해당 UnitAction에 전달합니다. Animation Event는 각 Animation Clip에 설정되어 있습니다.
    /// </summary>
    /// <param name="eventStringArgument"></param>
    public void GetAnimationEvent(string eventStringArgument)
    {
        if (AnimationEventNames.IsEventName(eventStringArgument) is false)
        {
            Debug.LogError("Invalid event name: " + eventStringArgument);
            return;
        }
        
        unit.GetSelectedAction()?.TossAnimationEvent(eventStringArgument);
    }
}
