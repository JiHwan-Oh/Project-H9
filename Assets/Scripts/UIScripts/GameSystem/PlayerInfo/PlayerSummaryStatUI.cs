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

    [SerializeField] private GameObject _actionPointText;
    [SerializeField] private GameObject _healthPointText;
    [SerializeField] private GameObject _ConcentrationText;

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
        
        _playerStat = _player.GetStat();
        
        _healthPointText.GetComponent<TextMeshProUGUI>().text = _playerStat.curHp.ToString() + " / " + _playerStat.maxHp.ToString();
        _ConcentrationText.GetComponent<TextMeshProUGUI>().text = _playerStat.concentration.ToString();
        _actionPointText.GetComponent<TextMeshProUGUI>().text = _player.currentActionPoint.ToString() + " / " + _playerStat.actionPoint.ToString();
        if (_player.currentActionPoint == 0)
        {
            _actionPointText.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            _actionPointText.GetComponent<TextMeshProUGUI>().color = Color.white;
        }

        GetComponent<PlayerHpUI>().SetHpUI();
    }
}
