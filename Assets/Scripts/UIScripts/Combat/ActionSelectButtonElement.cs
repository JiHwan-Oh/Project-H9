using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// ���� �� �÷��̾��� �ൿ�� ������ �� ���̴� �ൿ���ù�ư ������ ����� �����ϴ� Ŭ����
/// </summary>
public class ActionSelectButtonElement : UIElement, IPointerEnterHandler, IPointerExitHandler
{
    public IUnitAction _action { get; private set; }
    private CombatActionUI _combatActionUI;

    private GameObject APCostUI;
    private GameObject AmmoCostUI;
    private Color APCostUIInitColor;
    private Color AmmoCostUIInitColor;

    private bool _isSelectable;

    void SetUp()
    {
        APCostUI = gameObject.transform.GetChild(0).gameObject;
        AmmoCostUI = gameObject.transform.GetChild(1).gameObject;

        APCostUIInitColor = APCostUI.GetComponent<Image>().color;
        AmmoCostUIInitColor = AmmoCostUI.GetComponent<Image>().color;

        _isSelectable = true;
    }
    void Awake()
    {
        SetUp();
    }

    /// <summary>
    /// �ൿ���ù�ư�� �����մϴ�.
    /// CombatActionUI���� ��� �ൿ���ù�ư���� �����ϸ鼭 ����˴ϴ�.
    /// </summary>
    /// <param name="action"> �ش� ��ư�� ������ �׼� ���� </param>
    /// <param name="player"> �ش� ��ư�� ������ �÷��̾� ĳ���� ��ü </param>
    public void SetActionSelectButton(IUnitAction action, Player player)
    {
        _action = action;

        //Button selectable Setting
        _isSelectable = _action.IsSelectable();
        bool isPlayerTurn = FieldSystem.turnSystem.turnOwner is Player;
        bool isPlayerSelectAction = (player.GetSelectedAction().GetActionType() != ActionType.Idle);
        bool isSelectedAction = (player.GetSelectedAction().GetActionType() == _action.GetActionType());
        bool isIdleAction = (action.GetActionType() == ActionType.Idle);
        if (!isPlayerTurn || (isPlayerSelectAction && !isSelectedAction && !isIdleAction))
        {
            _isSelectable = false;
        }

        SetCostIcons(player.currentActionPoint, player.weapon.currentAmmo);
        GetComponent<Button>().interactable = _isSelectable;

        //Button Color Setting
        GetComponent<Image>().color = Color.white;
        bool isRunOutAmmo = ((action.GetActionType() == ActionType.Reload) && (player.weapon.currentAmmo == 0));
        if (isRunOutAmmo) 
        {
            GetComponent<Image>().color = Color.yellow;
        }

    }

    /// <summary>
    /// �ൿ���ù�ư�� ������ ��Ȱ��ȭ��ŵ�ϴ�.
    /// ��ȣ�ۿ��� ���� �ڽ�Ʈ�� ��ų �̹����� ǥ������ �ʽ��ϴ�.
    /// �÷��̾ ��ų�� ��ġ���� �ʾƼ� �ൿâ UI�� �� ������ �ְų�,
    /// �̹� �ش� �ൿ�� �����Ͽ� Idle �׼��� �ش� �ൿ���ù�ư�� ��ü�߰ų�,
    /// �׼��� ���� ���� ����� Idle �׼��� �ش� ���°� �˴ϴ�.
    /// </summary>
    public void OffActionSelectButton()
    {
        _action = null;
        //Button selectable Setting
        _isSelectable = false;
        GetComponent<Button>().interactable = _isSelectable;

        //Cost Icon Visible Setting
        APCostUI.SetActive(false);
        AmmoCostUI.SetActive(false);

        //Button Color Setting
        GetComponent<Image>().color = new Color(1, 1, 1);
    }

    private void SetCostIcons(int playerCurrentAp, int playerCurrentAmmo)
    {
        int apCost = _action.GetCost();
        if (_action.GetActionType() is ActionType.Move) apCost = 1; //Delete later
        //int ammoCost = _action.GetAmmoCost();
        int ammoCost = GetAmmoCost();   //Delete later

        SetEachCostIconUI(APCostUI, apCost, playerCurrentAp, APCostUIInitColor);
        SetEachCostIconUI(AmmoCostUI, ammoCost, playerCurrentAmmo, AmmoCostUIInitColor);
    }
    private void SetEachCostIconUI(GameObject ui, int requiredCost, int currentCost, Color initColor)
    {
        //Cost Icon Visible Setting
        ui.SetActive(true);
        if (requiredCost == 0)
        {
            ui.SetActive(false);
        }

        //Cost Icon Color Setting
        ui.GetComponent<Image>().color = initColor;
        if (currentCost < requiredCost)
        {
            ui.GetComponent<Image>().color = Color.gray;
            _isSelectable = false;
        }

        //Cost Icon Text Setting
        ui.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = requiredCost.ToString();
    }

    //Delete later
    private int GetAmmoCost() 
    {
        if (_action.GetActionType() == ActionType.Attack) return 1;
        return 0;
    }

    /// <summary>
    /// �ൿ���ù�ư�� Ŭ������ �� �÷��̾�� �ൿ�� �����϶�� ����� ������ ����� �����մϴ�.
    /// </summary>
    public void OnClickActionSeleteButton() 
    {
        if (_action.GetActionType() == ActionType.Idle &&
        FieldSystem.unitSystem.GetPlayer().GetSelectedAction().IsActive()) return;

        FieldSystem.unitSystem.GetPlayer().SelectAction(_action);
    }

    /// <summary>
    /// �ൿ���ù�ư�� ���콺�����Ǿ������� �����մϴ�.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_action is not null)
        {
            UIManager.instance.combatUI.combatActionUI.ShowActionUITooltip(this.gameObject);
        }
    }

    /// <summary>
    /// �ൿ���ù�ư�� ���콺���� ���¿��� ��������� �����մϴ�.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_action is not null)
        {
            UIManager.instance.combatUI.combatActionUI.HideActionUITooltip();
        }
    }
}
