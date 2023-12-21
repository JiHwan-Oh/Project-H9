using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �÷��̾��� ���� �� ü��, ���߷�, �׼� ����Ʈ�� ǥ���ϴ� UI�� ����� �����ϴ� Ŭ����
/// </summary>
public class PlayerSummaryStatUI : UISystem
{
    private UnitStat _playerStat;
    private Unit _player;

    [SerializeField] private GameObject _healthPointUI;
    [SerializeField] private GameObject _ConcentrationUI;
    [SerializeField] private GameObject _actionPointUI;

    // Start is called before the first frame update
    void Start()
    {
        UIManager.instance.onPlayerStatChanged.AddListener(() => SetCurrentStatusUI());
        UIManager.instance.onTurnChanged.AddListener(() => SetCurrentStatusUI());
        UIManager.instance.onActionChanged.AddListener(() => SetCurrentStatusUI());
        SetCurrentStatusUI();
    }
    private void Update()
    {
        //for test
        //SetCurrentStatusUI();
        //������ ���� ��ȭ ������ ������ �����...? Unit�� stat������ protected�̰�... UnityEvent�� ������ ���� ����.
        //stat ������ set�� �̿��ؼ� �ص� �Ǳ� �ϴµ� ���� ������ �� �ƴ϶� ���ɽ�����. ���� �����丵 �� ���.
    }
    public override void OpenUI()
    {
        base.OpenUI();
    }
    public override void CloseUI()
    {
        base.CloseUI();
    }

    /// <summary>
    /// ���� UI ����(�÷��̾� ü��, ���߷�, �׼� ����Ʈ)�� �����մϴ�.
    /// ����� �� �����Ӹ��� ȣ��˴ϴ�.
    /// </summary>
    public void SetCurrentStatusUI()
    {
        _player = FieldSystem.unitSystem.GetPlayer();
        if (_player is null) return;
        
        _playerStat = _player.stat;

        _healthPointUI.GetComponent<PlayerSummaryStatElement>().SetPlayerSummaryUI("Health Point", _playerStat.curHp.ToString() + " / " + _playerStat.GetStat(StatType.MaxHp).ToString());
        _ConcentrationUI.GetComponent<PlayerSummaryStatElement>().SetPlayerSummaryUI("Concentration", _playerStat.concentration.ToString());
        _actionPointUI.GetComponent<PlayerSummaryStatElement>().SetPlayerSummaryUI("Action Point", _playerStat.GetStat(StatType.CurActionPoint).ToString() + " / " + _playerStat.GetStat(StatType.MaxActionPoint).ToString());

        GetComponent<PlayerHpUI>().SetHpUI();
    }
}
