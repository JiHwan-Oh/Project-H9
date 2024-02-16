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
    /// ĳ������ �нú� ��ų�� ǥ���ϴ� ���
    /// </summary>
    public PassiveSkillListUI passiveSkillListUI { get; private set; }
    /// <summary>
    /// ĳ������ �κ��丮 �� �������� ǥ���ϴ� ���
    /// </summary>
    public ItemUI itemUI { get; private set; }
    public EquipmentUI equipmentUI { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        characterStatUI = GetComponent<CharacterStatUI>();
        passiveSkillListUI = GetComponent<PassiveSkillListUI>();
        itemUI = GetComponent<ItemUI>();
        equipmentUI = GetComponent<EquipmentUI>();
        SetMoneyText();

        uiSubsystems.Add(characterStatUI);
        uiSubsystems.Add(passiveSkillListUI);
        uiSubsystems.Add(itemUI);
        uiSubsystems.Add(equipmentUI);
    }

    public override void ClosePopupWindow()
    {
        itemUI.ClosePopupWindow();
    }

    /// <summary>
    /// �÷��̾��� �������� UI�� ǥ���մϴ�.
    /// </summary>
    public void SetMoneyText()
    {
        //string moneyText = ItemManager.instance.money.ToString();
        //_moneyText.GetComponent<TextMeshProUGUI>().text = "Money: " + moneyText + "$";
    }
}
