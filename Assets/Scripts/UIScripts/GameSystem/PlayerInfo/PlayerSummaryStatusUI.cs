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

    [SerializeField] private TMP_Text _hpTitleText;
    [SerializeField] private TMP_Text _concentrationTitleText;
    [SerializeField] private TMP_Text _apTitleText;

    public int expectedHpUsage = 0;
    public int expectedApUsage = 0;
    public int expectedMagUsage = 0;
    public int expectedConcenUsage = 0;

    // Start is called before the first frame update
    void Start()
    {
        // todo: onActionChanged ����
        UIManager.instance.onActionChanged.AddListener(InitExpectedValues);
        
        FieldSystem.onStageAwake.AddListener(StageStart);
        
        
        InitExpectedValues();
        SetCurrentStatusUI();

        _hpTitleText.text = UIManager.instance.UILocalization[26];
        _concentrationTitleText.text = UIManager.instance.UILocalization[29];
        _apTitleText.text = UIManager.instance.UILocalization[27];
    }

    void StageStart()
    {
        FieldSystem.turnSystem.onTurnChanged.AddListener(InitExpectedValues);
    }
    
    private void InitExpectedValues() 
    {
        expectedHpUsage = 0;
        expectedApUsage = 0;
        expectedMagUsage = 0;
        expectedConcenUsage = 0;
        SetCurrentStatusUI();
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
