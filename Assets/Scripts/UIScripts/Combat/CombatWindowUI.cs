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
    public CombatActionUI_Legacy combatActionUI_legacy { get; private set; }
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
    public TurnOrderUI turnOrderUI { get; private set; }
    public CombatResultUI combatResultUI { get; private set; }
    public BuffUI buffUI { get; private set; }

    // Start is called before the first frame update
    private void Awake()
    {

        combatActionUI_legacy = GetComponent<CombatActionUI_Legacy>();
        combatActionUI = GetComponent<CombatActionUI>();
        magazineUI = GetComponent<MagazineUI>();
        enemyHpUI = GetComponent<EnemyHpUI>();
        enemyStatUI = GetComponent<EnemyStatUI>();
        startTurnTextUI = GetComponent<StartTurnTextUI>();
        turnOrderUI = GetComponent<TurnOrderUI>();
        combatResultUI = GetComponent<CombatResultUI>();
        buffUI = GetComponent<BuffUI>();

        uiSubsystems.Add(combatActionUI_legacy);
        uiSubsystems.Add(combatActionUI);
        uiSubsystems.Add(magazineUI);
        uiSubsystems.Add(enemyHpUI);
        uiSubsystems.Add(enemyStatUI);
        uiSubsystems.Add(startTurnTextUI);
        uiSubsystems.Add(turnOrderUI);
        uiSubsystems.Add(combatResultUI);
        uiSubsystems.Add(buffUI);
    }

    public override void ClosePopupWindow()
    {
        enemyStatUI.ClosePopupWindow();
    }

    /// <summary>
    /// ���� UI�� �����մϴ�.
    /// Ư���� �׼��� ����� ��, �׼��� ����� �� ����˴ϴ�.
    /// �÷��̾ Ư���� �׼��� ������ ������ ����˴ϴ�.
    /// </summary>
    //public void SetCombatUI()
    //{
    //    _isInCombat = GameManager.instance.CompareState(GameState.Combat);

    //    if (!_isInCombat) return;

    //    combatActionUI.SetActionButtons();
    //    magazineUI.SetMagazineText();
    //    enemyHpUI.SetEnemyHpBars();
    //}

}
