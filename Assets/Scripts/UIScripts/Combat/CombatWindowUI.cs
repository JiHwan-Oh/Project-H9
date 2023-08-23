using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

/// <summary>
/// ���� �� ���� �� �ʿ��� UI�� ���� ����� ��� �����ϴ� Ŭ����
/// �̸��� ���ļ� CombatWindowUI��� ���������, ��Ī �ϰ����� �����ϸ� CombatUI�� ����.
/// </summary>
public class CombatWindowUI : UISystem
{
    /// <summary>
    /// ĳ������ �ൿâ UI�� ǥ�� �� ��ȣ�ۿ�� ���õ� ���
    /// </summary>
    public CombatActionUI combatActionUI { get; private set; }
    /// <summary>
    /// ���� ����� ������ ��ź ���� ǥ���ϴ� ���
    /// </summary>
    public MagazineUI magazineUI { get; private set; }
    /// <summary>
    /// ������ ü�¹ٸ� ǥ���ϴ� ���
    /// </summary>
    public EnemyHpUI enemyHpUI { get; private set; }
    /// <summary>
    /// ������ ������ ǥ���ϴ� ����â�� ���õ� ���
    /// </summary>
    public EnemyStatUI enemyStatUI { get; private set; }
    /// <summary>
    /// ���� ���۵Ǵ� �� ���ο� ���缭 ȭ�� �߾ӿ� �ؽ�Ʈ�� ����ϴ� ���
    /// </summary>
    public StartTurnTextUI startTurnTextUI { get; private set; }

    // Start is called before the first frame update
    private new void Awake()
    {
        base.Awake();
        
        combatActionUI = GetComponent<CombatActionUI>();
        magazineUI = GetComponent<MagazineUI>();
        enemyHpUI = GetComponent<EnemyHpUI>();
        enemyStatUI = GetComponent<EnemyStatUI>();
        startTurnTextUI = GetComponent<StartTurnTextUI>();
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
    /// ���� UI�� �����մϴ�.
    /// Ư���� �׼��� ����� ��, �׼��� ����� �� ����˴ϴ�.
    /// �÷��̾ Ư���� �׼��� ������ ������ ����˴ϴ�.
    /// </summary>
    public void SetCombatUI()
    {
        if (!GameManager.instance.CompareState(GameState.Combat)) return;
        combatActionUI.SetActionButtons();
        magazineUI.SetMagazineText();
        enemyHpUI.SetEnemyHpBars();
    }
}
