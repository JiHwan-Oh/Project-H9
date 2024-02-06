using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.EventSystems;

/// <summary>
/// ���� �� �÷��̾��� �ൿ�� ������ �� ���̴� �ൿ���ù�ư ������ ����� �����ϴ� Ŭ����
/// </summary>
public class ActionSelectButtonElement : UIElement, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private GameObject _skillImage;
    [SerializeField] private GameObject _highlightEffect;

    [SerializeField] private GameObject _APCostUI;
    [SerializeField] private GameObject _AmmoCostUI;

    [SerializeField] private GameObject _ActionNameUI;

    [SerializeField] private Texture2D _textures; //test Texture. �̵�, ����, ����, �д� ����
    private ActionType[] normalActionType = { ActionType.Move, ActionType.Attack, ActionType.Reload };
    public Sprite[] _sprites;
    public IUnitAction displayedAction { get; private set; }

    private bool _isSelectable;
    private bool _isEnoughCost;
    void Awake()
    {
        //_sprites = Resources.LoadAll<Sprite>("Sprite/" + _textures.name);

        _isSelectable = true;
    }

    /// <summary>
    /// �ൿ���ù�ư�� �����մϴ�.
    /// CombatActionUI���� ��� �ൿ���ù�ư���� �����ϸ鼭 ����˴ϴ�.
    /// </summary>
    /// <param name="action"> �ش� ��ư�� ������ �׼� ���� </param>
    /// <param name="player"> �ش� ��ư�� ������ �÷��̾� ĳ���� ��ü </param>
    public void SetActionSelectButton(IUnitAction action, Player player)
    {
        this.displayedAction = action;

        //Button selectable Setting
        // �÷��̾��� ���� �ƴ� ��� - ���� �Ұ�
        // �÷��̾ �̹� �ൿ�� ���� ���� ��� - ���� �Ұ�
        // �÷��̾ � �ൿ�� �����ߴµ�, �ش� ��ư�� ���õ� ��ư�� �ƴ� ��� - ���� �Ұ�. ��, Idle(Cancel)��ư�� ����.
        // + SetCostIcons() �޼ҵ忡�� Cost�� �����ϴٰ� �ǴܵǸ� - ���� �Ұ�
        _isSelectable = this.displayedAction.IsSelectable();
        IUnitAction playerSelectedAction = player.GetSelectedAction();
        
        bool isPlayerTurn = FieldSystem.turnSystem.turnOwner is Player;
        bool isPlayerSelectAction = (playerSelectedAction.GetActionType() != ActionType.Idle);
        bool isIdleAction = (action.GetActionType() == ActionType.Idle);
        bool isActiveAction = playerSelectedAction.IsActive();
        if ((!isPlayerTurn) || isActiveAction || (isPlayerSelectAction && !isIdleAction))
        {
            _isSelectable = false;
        }

        _isEnoughCost = true;
        SetCostIcons(player.currentActionPoint, player.weapon.currentAmmo);
        if (!_isEnoughCost)
        {
            _isSelectable = false;
        }
        GetComponent<Button>().interactable = _isSelectable;

        //Button Image Setting
        Sprite spr = GetActionSprite(action.GetActionType());
        if (isIdleAction)
        {
            spr = GetActionSprite(playerSelectedAction.GetActionType());
        }
        _skillImage.GetComponent<Image>().sprite = spr;

        Color skillIconColor = Color.gray;
        if (_isSelectable)
        {
            skillIconColor = Color.white;
        }
        _skillImage.GetComponent<Image>().color = skillIconColor;


        //highlight Setting
        Color hLColor = UICustomColor.NormalStateColor;
        bool isRunOutAmmo = ((action.GetActionType() == ActionType.Reload) && (player.weapon.currentAmmo == 0));
        if (isRunOutAmmo) 
        {
            hLColor = UICustomColor.HighlightStateColor;
        }
        if (!_isSelectable) 
        {
            hLColor = UICustomColor.DisableStateColor;
        }
        _highlightEffect.GetComponent<Image>().color = hLColor;

        //Text Setting
        string actionName = action.GetActionType().ToString();
        if (isIdleAction) 
        {
            actionName = playerSelectedAction.GetActionType().ToString();
        }
        _ActionNameUI.GetComponent<TextMeshProUGUI>().text = actionName;
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
        displayedAction = null;
        //Button selectable Setting
        _isSelectable = false;
        _isEnoughCost = false;
        GetComponent<Button>().interactable = _isSelectable;

        //Cost Icon Visible Setting
        _APCostUI.SetActive(false);
        _AmmoCostUI.SetActive(false);

        //Button Image Setting
        _skillImage.GetComponent<Image>().sprite = null;
        _skillImage.GetComponent<Image>().color = Color.gray;

        //Text Setting
        _ActionNameUI.GetComponent<TextMeshProUGUI>().text = "";
    }

    private void SetCostIcons(int playerCurrentAp, int playerCurrentAmmo)
    {
        int apCost = displayedAction.GetCost();
        //MoveAction�� GetCost�� ���� �Ͽ� MoveAction�� �Ҹ��� AP�� ��ȯ�ϴ� ���¶� �ӽù������� �ۼ�
        //if (action.GetActionType() is ActionType.Move) apCost = 1;
        int ammoCost = displayedAction.GetAmmoCost();

        Color32 ApColor = UICustomColor.ActionAPColor;
        Color32 AmmoColor = UICustomColor.ActionAmmoColor;

        SetEachCostIconUI(_APCostUI, apCost, playerCurrentAp, ApColor);
        SetEachCostIconUI(_AmmoCostUI, ammoCost, playerCurrentAmmo, AmmoColor);
    }
    private void SetEachCostIconUI(GameObject icon, int requiredCost, int currentCost, Color initColor)
    {
        //Cost Icon Visible Setting
        icon.SetActive(true);
        if (requiredCost == 0)
        {
            icon.SetActive(false);
        }

        //Cost Icon Color Setting
        icon.GetComponent<Image>().color = initColor;
        if (currentCost < requiredCost)
        {
            icon.GetComponent<Image>().color = Color.gray;
            _isEnoughCost = false;
        }

        //Cost Icon Text Setting
        icon.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = requiredCost.ToString();
    }

    /// <summary>
    /// �ൿ���ù�ư�� Ŭ������ �� �÷��̾�� �ൿ�� �����϶�� ����� ������ ����� �����մϴ�.
    /// </summary>
    public void OnClickActionSeleteButton() 
    {
        if (!GetComponent<Button>().IsInteractable()) return;
        if (FieldSystem.unitSystem.IsCombatFinish(out var none)) return;
        bool isIdleButton = (displayedAction.GetActionType() == ActionType.Idle);
        bool isActiveSelectedAction = (FieldSystem.unitSystem.GetPlayer().GetSelectedAction().IsActive());
        if (isIdleButton && isActiveSelectedAction) return;

        UIManager.instance.combatUI.combatActionUI_legacy.SetSelectedActionButton(gameObject);
        FieldSystem.unitSystem.GetPlayer().SelectAction(displayedAction);
    }

    /// <summary>
    /// �ൿ���ù�ư�� ���콺�����Ǿ������� �����մϴ�.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (displayedAction is not null)
        {
            UIManager.instance.combatUI.combatActionUI_legacy.ShowActionUITooltip(this.gameObject);
        }
    }

    /// <summary>
    /// �ൿ���ù�ư�� ���콺���� ���¿��� ��������� �����մϴ�.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (displayedAction is not null)
        {
            UIManager.instance.combatUI.combatActionUI_legacy.HideActionUITooltip();
        }
    }


    private Sprite GetActionSprite(ActionType actionType) 
    {
        //normal action
        for (int i = 0; i < normalActionType.Length; i++)
        {
            if (normalActionType[i] == actionType)
            {
                return _sprites[i];
;            }
        }

        //skill action �̱���
        if (actionType == ActionType.Fanning)
        {
            return _sprites[3];
        }
        return null;
    }

    public override bool IsInteractable()
    {
        return _isSelectable;
    }
}