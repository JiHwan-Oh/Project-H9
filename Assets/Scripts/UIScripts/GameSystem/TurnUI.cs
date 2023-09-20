using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ������ �� ���� ī��Ʈ�ϰ�, ���� �߿��� ������ ���� ���� ������ �������� ������� ǥ���ϴ� ����� ������ Ŭ����
/// </summary>
public class TurnUI : UISystem
{
    [SerializeField] private GameObject _turnText;
    [SerializeField] private GameObject _endTurnButton;

    private bool _isInteractable = true;
    private bool _isHighlight = false;

    private void Update()
    {
        SetEndTurnButton();
        if (Input.GetKeyDown(KeyCode.BackQuote)) 
        {
            OnClickEndTurnButton();
        }
    }

    /// <summary>
    /// ȭ�� ���� ��ܿ� ���� �� ���� ǥ���Ѵ�.
    /// </summary>
    /// <param name="currentTurn"> ���� �� �� </param>
    public void SetTurnTextUI() 
    {
        int currentTurn = 0;
        if (GameManager.instance.CompareState(GameState.Combat))
        {
            currentTurn = FieldSystem.unitSystem.GetPlayer().currentRound;
        }
        else 
        {
            currentTurn = FieldSystem.turnSystem.turnNumber;
        }
        _turnText.GetComponent<TextMeshProUGUI>().text = "Turn " + currentTurn;
    }
    public void SetEndTurnButton() 
    {
        Color color = Color.white;
        if (!IsButtonInteractable()) color = Color.black;
        else if (IsButtonHighlighted()) color = Color.yellow;
        _endTurnButton.GetComponent<Image>().color = color;
    }

    /// <summary>
    /// �� ���� ��ư�� Ŭ���� �� ����˴ϴ�.
    /// turnSystem�� �÷��̾��� ���� �����϶�� ����� �����ϴ�.
    /// </summary>
    public void OnClickEndTurnButton() 
    {
        Debug.Log("adsf");
        if (IsButtonInteractable())
        {
            FieldSystem.turnSystem.EndTurn();
        }
    }

    private bool IsButtonInteractable()
    {
        bool isPlayerTurn = (FieldSystem.turnSystem.turnOwner is Player);
        bool isActiveAction = (FieldSystem.unitSystem.GetPlayer().GetSelectedAction().IsActive());
        bool isCombatFinished = (GameManager.instance.CompareState(GameState.Combat) && FieldSystem.unitSystem.IsCombatFinish());
        if (isPlayerTurn && !isActiveAction && !isCombatFinished) 
        {
            return true;
        }
        return false;
    }
    private bool IsButtonHighlighted() 
    {
        if (GameManager.instance.CompareState(GameState.World))
        {
            Debug.Log("world");
            return (FieldSystem.unitSystem.GetPlayer().currentActionPoint <= 0);
        }
        else
        {
            return !UIManager.instance.combatUI.combatActionUI.IsThereSeletableButton();
        }
    }
}
