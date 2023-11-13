using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FieldSystem : MonoBehaviour
{
    private static FieldSystem _instance;
    
    public static TileSystem tileSystem;
    public static TurnSystem turnSystem;
    public static UnitSystem unitSystem;

    /// <summary>
    /// Stage�� �ε��Ǿ����� �ٷ� ȣ��Ǵ� �̺�Ʈ�Դϴ�.
    /// </summary>
    private static UnityEvent _onStageAwake;
    public static UnityEvent onStageAwake => _onStageAwake ??= new UnityEvent();

    /// <summary>
    /// Stage�� Fade-in�� ������ ���� �� Invoke�Ǵ� �̺�Ʈ�Դϴ�. 
    /// </summary>
    private static UnityEvent _onStageStart;
    public static UnityEvent onStageStart => _onStageStart ??= new UnityEvent();
    
    /// <summary>
    /// CombatScene���� Stage�� �������� ȣ��Ǵ� �̺�Ʈ�Դϴ�. WorldScene���� ȣ����� �ʽ��ϴ�.
    /// </summary>
    private static UnityEvent<bool> _onCombatFinish;
    public static UnityEvent<bool> onCombatFinish => _onCombatFinish ??= new UnityEvent<bool> ();
    
    
    private void Awake()
    {
        Debug.Log("Field System : Awake");
        _instance = this;
        tileSystem = GetComponent<TileSystem>();
        turnSystem = GetComponent<TurnSystem>();
        unitSystem = GetComponent<UnitSystem>();
    }

    private void Start()
    {
        tileSystem.SetUpTilesAndObjects();
        unitSystem.SetUpUnits();
        turnSystem.SetUp();
        StartCoroutine(StartSceneCoroutine());
    }

    private IEnumerator StartSceneCoroutine()
    {
        Debug.Log("onStageAwake");
        onStageAwake.Invoke();
        UIManager.instance.gameSystemUI.turnUI.SetTurnTextUI();

        yield return new WaitUntil(() => LoadingManager.instance.isLoadingNow is false);

        Debug.Log("onStageStart");
        onStageStart.Invoke();
        turnSystem.StartTurn();
    }
}
