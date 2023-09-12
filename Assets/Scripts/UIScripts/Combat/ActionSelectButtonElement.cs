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
    public IUnitAction _action { get; private set; }

    [SerializeField] private GameObject _skillImage;
    [SerializeField] private GameObject _highlightEffect;
    private Color32 grayColor = new Color32(0, 0, 0, 200);
    private Color32 redColor = new Color32(240, 64, 0, 200);
    private Color32 yellowColor = new Color32(240, 240, 0, 200);

    [SerializeField] private GameObject _APCostUI;
    [SerializeField] private GameObject _AmmoCostUI;
    private Color _APCostUIInitColor;
    private Color _AmmoCostUIInitColor;

    [SerializeField] private GameObject _ActionNameUI;

    [SerializeField] private Texture2D _textures; //test Texture. �̵�, ����, ����, �д� ����
    private ActionType[] normalActionType = { ActionType.Move, ActionType.Attack, ActionType.Reload };
    private Sprite[] _sprites;

    private bool _isSelectable;
    void Awake()
    {
        _sprites = Resources.LoadAll<Sprite>("Sprite/" + _textures.name);
        GetComponent<Image>().sprite = _sprites[0];

        _isSelectable = true;

        //_APCostUIInitColor = _APCostUI.GetComponent<Image>().color;
        //_AmmoCostUIInitColor = _AmmoCostUI.GetComponent<Image>().color;
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
        IUnitAction playerSelectedAction = player.GetSelectedAction();
        bool isPlayerTurn = FieldSystem.turnSystem.turnOwner is Player;
        bool isPlayerSelectAction = (playerSelectedAction.GetActionType() != ActionType.Idle);
        bool isSelectedAction = (playerSelectedAction.GetActionType() == _action.GetActionType());
        bool isIdleAction = (action.GetActionType() == ActionType.Idle);
        bool isActiveAction = playerSelectedAction.IsActive();
        if ((!isPlayerTurn) || (isPlayerSelectAction && !isSelectedAction && !isIdleAction) || isActiveAction)
        {
            _isSelectable = false;
        }

        SetCostIcons(player.currentActionPoint, player.weapon.currentAmmo);
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
        Color hLColor = redColor;
        bool isRunOutAmmo = ((action.GetActionType() == ActionType.Reload) && (player.weapon.currentAmmo == 0));
        if (isRunOutAmmo) 
        {
            hLColor = yellowColor;
        }
        if (!_isSelectable) 
        {
            hLColor = grayColor;
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
        _action = null;
        //Button selectable Setting
        _isSelectable = false;
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
        int apCost = _action.GetCost();
        //MoveAction�� GetCost�� ���� �Ͽ� MoveAction�� �Ҹ��� AP�� ��ȯ�ϴ� ���¶� �ӽù������� �ۼ�
        if (_action.GetActionType() is ActionType.Move) apCost = 1;
        int ammoCost = _action.GetAmmoCost();

        _APCostUIInitColor = new Color32(0, 224, 128, 255);
        _AmmoCostUIInitColor = new Color32(255, 128, 0, 255);

        SetEachCostIconUI(_APCostUI, apCost, playerCurrentAp, _APCostUIInitColor);
        SetEachCostIconUI(_AmmoCostUI, ammoCost, playerCurrentAmmo, _AmmoCostUIInitColor);
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
            _isSelectable = false;
        }

        //Cost Icon Text Setting
        icon.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = requiredCost.ToString();
    }

    /// <summary>
    /// �ൿ���ù�ư�� Ŭ������ �� �÷��̾�� �ൿ�� �����϶�� ����� ������ ����� �����մϴ�.
    /// </summary>
    public void OnClickActionSeleteButton() 
    {
        bool isIdleButton = (_action.GetActionType() == ActionType.Idle);
        bool isActiveSelectedAction = (FieldSystem.unitSystem.GetPlayer().GetSelectedAction().IsActive());
        if (isIdleButton && isActiveSelectedAction) return;

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
        if (actionType == ActionType.Panning)
        {
            return _sprites[3];
        }
        return null;
    }
}
