using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// ĳ���� ���� â�� ���� ����� ��� �����ϴ� Ŭ����
/// </summary>
public class CharacterUI : UISystem
{
    /// <summary>
    /// ĳ������ ���� �� ���� ������ ǥ���ϴ� ���
    /// </summary>
    public CharacterStatUI characterStatUI { get; private set; }
    /// <summary>
    /// ĳ���Ͱ� ������ ��ų�� ĳ���� â�� ǥ���ϴ� ���
    /// </summary>
    public LearnedSkillUI learnedSkillUI { get; private set; }
    /// <summary>
    /// ĳ������ �κ��丮 �� �������� ǥ���ϴ� ���
    /// </summary>
    public ItemListUI itemListUI { get; private set; }

    [SerializeField] private GameObject _moneyText;

    // Start is called before the first frame update
    void Start()
    {
        characterStatUI = GetComponent<CharacterStatUI>();
        learnedSkillUI = GetComponent<LearnedSkillUI>();
        itemListUI = GetComponent<ItemListUI>();
        SetMoneyText();
    }
    public override void OpenUI()
    {
        base.OpenUI();
        characterStatUI.OpenUI();
        learnedSkillUI.OpenUI();
        itemListUI.OpenUI();
    }
    public override void CloseUI()
    {
        base.CloseUI();
        characterStatUI.CloseUI();
        learnedSkillUI.CloseUI();
        itemListUI.CloseUI();
    }
    public override void ClosePopupWindow()
    {
        itemListUI.ClosePopupWindow();
    }

    /// <summary>
    /// �÷��̾��� �������� UI�� ǥ���մϴ�.
    /// ������ �Ŵ������� �������� ������ �� �۵��մϴ�.
    /// </summary>
    public void SetMoneyText()
    {
        string moneyText = ItemManager.instance.money.ToString();
        _moneyText.GetComponent<TextMeshProUGUI>().text = "Money: " + moneyText + "$";
    }
}
