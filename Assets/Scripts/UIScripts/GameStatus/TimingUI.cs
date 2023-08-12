using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ������ �� ���� ī��Ʈ�ϰ�, ���� �߿��� ������ ���� ���� ������ �������� ������� ǥ���ϴ� ����� ������ Ŭ����
/// </summary>
public class TimingUI : UISystem
{
    [SerializeField] private GameObject _turnText;
    [SerializeField] private GameObject _turnOrderUI;
    private readonly Vector3 TURN_ORDER_UI_INIT_POSITION = new Vector3(0, 0, 0);
    private const int TURN_ORDER_UI_INTERVAL = 10;

    public override void OpenUI()
    {
        base.OpenUI();
    }
    public override void CloseUI()
    {
        base.CloseUI();
    }
    /// <summary>
    /// ȭ�� ���� ��ܿ� ���� �� ���� ǥ���Ѵ�.
    /// </summary>
    /// <param name="currentTurn"> ���� �� �� </param>
    public void SetTurnText(int currentTurn) 
    {
        _turnText.GetComponent<TextMeshProUGUI>().text = currentTurn + "��";
    }
    /// <summary>
    /// �� ���� UI�� Ű�ų� ���ϴ�.
    /// ���� ���� �� ������ �ƴ� �� �����ϴ�.
    /// </summary>
    /// <param name="isOn"> UI Ȱ��ȭ ���� </param>
    public void SetTurnOrderUIState(bool isOn) 
    {
        _turnOrderUI.SetActive(isOn);
    }
    /// <summary>
    /// �� ���� UI�� �����մϴ�.
    /// turnSystem���� ����� �� ������ �Է¹޾� UI�� ǥ���մϴ�.
    /// </summary>
    /// <param name="turnOrder"> �� ���� ����Ʈ </param>
    public void SetTurnOrderUI(List<Unit> turnOrder) 
    {
        int index = 0;
        foreach(Unit unit in turnOrder)
        {
            //unit�� index�� �����Ͽ� ������ �ʻ�ȭ sprite�� �ҷ��;� �ϴµ�... ��� ã�ƿ;� ���� ���� ��.
            if (unit is Player)
            {
                _turnOrderUI.transform.GetChild(index).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                _turnOrderUI.transform.GetChild(index).GetComponent<Image>().color = new Color(.5f, .5f, 0.5f, 1);
            }
            index++;
        }
    }

    /// <summary>
    /// �� ���� ��ư�� Ŭ���� �� ����˴ϴ�.
    /// turnSystem�� �÷��̾��� ���� �����϶�� ����� �����ϴ�.
    /// </summary>
    public void OnClickEndTurnButton() 
    {
        if (FieldSystem.turnSystem.turnOwner is Player)
        {
            FieldSystem.turnSystem.EndTurn();
        }
    }
}
