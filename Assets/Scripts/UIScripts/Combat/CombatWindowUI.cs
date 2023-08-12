using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

/// <summary>
/// 전투 씬 입장 시 필요한 UI의 여러 기능을 묶어서 관리하는 클래스
/// 이름이 겹쳐서 CombatWindowUI라고 명명했으나, 명칭 일관성을 생각하면 CombatUI가 맞음.
/// </summary>
public class CombatWindowUI : UISystem
{
    /// <summary>
    /// 캐릭터의 행동창 UI의 표시 및 상호작용과 관련된 기능
    /// </summary>
    public CombatActionUI combatActionUI { get; private set; }
    /// <summary>
    /// 현재 장비한 무기의 장탄 수를 표시하는 기능
    /// </summary>
    public MagazineUI magazineUI { get; private set; }
    /// <summary>
    /// 적들의 체력바를 표시하는 기능
    /// </summary>
    public EnemyHpUI enemyHpUI { get; private set; }
    /// <summary>
    /// 적들의 스텟을 표시하는 스텟창과 관련된 기능
    /// </summary>
    public EnemyStatUI enemyStatUI { get; private set; }

    // Start is called before the first frame update
    private new void Awake()
    {
        base.Awake();
        
        combatActionUI = GetComponent<CombatActionUI>();
        magazineUI = GetComponent<MagazineUI>();
        enemyHpUI = GetComponent<EnemyHpUI>();
        enemyStatUI = GetComponent<EnemyStatUI>();
    }

    public override void OpenUI()
    {
        base.OpenUI();
        combatActionUI.OpenUI();
        magazineUI.OpenUI();
        enemyHpUI.OpenUI();
        enemyStatUI.OpenUI();
    }
    public override void CloseUI()
    {
        base.CloseUI();
        combatActionUI.CloseUI();
        magazineUI.CloseUI();
        enemyHpUI.CloseUI();
        enemyStatUI.CloseUI();
    }
    public override void ClosePopupWindow()
    {
        enemyStatUI.CloseEnemyStatUI();
    }

    /// <summary>
    /// 전투 UI를 갱신합니다.
    /// 특정한 액션이 실행될 때, 액션이 종료될 때 실행됩니다.
    /// 플레이어가 특정한 액션을 선택할 때에도 실행됩니다.
    /// </summary>
    public void SetCombatUI()
    {
        if (!GameManager.instance.CompareState(GameState.Combat)) return;
        combatActionUI.SetActionButtons();
        magazineUI.SetMagazineText();
        enemyHpUI.SetEnemyHpBars();
    }
}
