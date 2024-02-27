using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CombatActionButtonElement : UIElement, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _actionButton;
    [SerializeField] private GameObject _actionButtonActiveEffect;
    [SerializeField] private GameObject _actionButtonIcon;
    [SerializeField] private GameObject _actionButtonNumber;

    public CombatActionType actionType = CombatActionType.Null;
    public string buttonName { get; private set; }
    public IUnitAction buttonAction { get; private set; }

    private int _buttonIndex = 0;
    private bool _isSelectable = true;

    // Start is called before the first frame update
    void Start()
    {
        buttonAction = null;
        if (actionType != CombatActionType.Null) 
        {
            //_actionButtonIcon.GetComponent<Image>().sprite = ;
            _actionButtonNumber.GetComponent<TextMeshProUGUI>().text = ((int)actionType).ToString();
            buttonName = actionType.ToString();
        }
    }

    public void SetcombatActionButton(CombatActionType actionType, int btnNumber, IUnitAction action) 
    {
        this.actionType = actionType;
        _buttonIndex = btnNumber;
        _actionButtonNumber.GetComponent<TextMeshProUGUI>().text = (btnNumber + 1).ToString();
        buttonName = action.GetActionType().ToString();
        buttonAction = action;
        SetInteractable();
    }

    public void OnClickCombatSelectButton()
    {
        if (IsInteractable())
        {
            UIManager.instance.combatUI.combatActionUI.SelectAction(actionType, _buttonIndex);
        }
    }
    public override bool IsInteractable()
    {
        return _isSelectable;
    }
    public void SetInteractable()
    {
        if (buttonAction is null) return;

        //Button selectable Setting
        // �÷��̾��� ���� �ƴ� ��� - ���� �Ұ�
        // �÷��̾ �̹� �ൿ�� ���� ���� ��� - ���� �Ұ�
        // �÷��̾ � �ൿ�� �����ߴµ�, �ش� ��ư�� ���õ� ��ư�� �ƴ� ��� - ���� �Ұ�. ��, Idle(Cancel)��ư�� ����.
        // Cost�� �����ϴٰ� �ǴܵǸ� - ���� �Ұ�
        _isSelectable = this.buttonAction.IsSelectable();
        Player player = FieldSystem.unitSystem.GetPlayer();
        if (player is null) return;
        IUnitAction playerSelectedAction = player.GetSelectedAction();

        bool isPlayerTurn = FieldSystem.turnSystem.turnOwner is Player;
        bool isPlayerAlreadyActiveAction = playerSelectedAction.IsActive();

        bool isEnoughApCost = (buttonAction.GetCost() <= player.currentActionPoint);
        bool isEnoughAmmoCost = (buttonAction.GetAmmoCost() <= player.weapon.currentAmmo);
        bool isEnoughCost = isEnoughApCost && isEnoughAmmoCost;
        if ((!isPlayerTurn) || isPlayerAlreadyActiveAction || !isEnoughCost)
        {
            _isSelectable = false;
        }
        _actionButtonActiveEffect.SetActive(!_isSelectable);
    }
    public void SetInteractable(bool isInteractable)
    {
        if (buttonAction is not null) return;
        _isSelectable = isInteractable;
        _actionButtonActiveEffect.SetActive(!_isSelectable);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.instance.combatUI.combatActionUI.ShowActionUITooltip(gameObject);
        if (buttonAction is not null)
        {
            UIManager.instance.combatUI.combatActionUI.ShowRequiredCost(buttonAction);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.instance.combatUI.combatActionUI.HideActionUITooltip();
        if (buttonAction is not null)
        {
            UIManager.instance.combatUI.combatActionUI.ClearRequiredCost();
        }
    }
}
