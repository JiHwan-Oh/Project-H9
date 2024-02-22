using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// �÷��̾��� ���� �� ü��, ���߷�, �׼� ����Ʈ�� ǥ���ϴ� UI�� ����� �����ϴ� Ŭ����
/// </summary>
public class PlayerSummaryStatusUI : UIElement
{
    private Unit _player;

    [SerializeField] private GameObject _healthPointUI;
    [SerializeField] private GameObject _actionPointUI;
    [SerializeField] private GameObject _magazineUI;
    [SerializeField] private GameObject _concentrationUI;

    // Start is called before the first frame update
    void Start()
    {
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
    /// ���� UI ������ �����մϴ�.
    /// </summary>
    public void SetCurrentStatusUI()
    {
        Unit _player = FieldSystem.unitSystem.GetPlayer();
        if (_player is null) return;

        _healthPointUI.GetComponent<PlayerHpUI>().SetHpUI();
        _actionPointUI.GetComponent<PlayerApUI>().SetApUI();
        _magazineUI.GetComponent<PlayerMagazineUI>().SetMagazineUI(false);
        _concentrationUI.GetComponent<PlayerConcentrationUI>().SetConcentrationUI();
    }
}
