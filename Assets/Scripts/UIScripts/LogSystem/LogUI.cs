using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogUI : UISystem
{
    [SerializeField]
    private RectTransform _logPanel; // text ���� ���� �� ������, text ���� �ո�ŭ�� ũ�⸦ ���� ����.
    private float _FixedTextedPanelHeight; // "length text 0 ~ n-1" + length text n

    [SerializeField]
    private GameObject _defaultTextPrefab;

    [SerializeField]
    private ScrollRect _scroll;

    private List<TMP_Text> _textCaches = new List<TMP_Text>();
    private List<ContentSizeFitter> _sizeFilterCaches = new List<ContentSizeFitter>();
    private StringBuilder _builder = new StringBuilder();
    private int _curTextlistIndex = -1;
    private float _beforeHeight = 0;

    private readonly int LIMIT_TEXT_LENGTH = 500;

    private void Awake()
    {
        UIManager.instance.onLevelUp.AddListener(ChangedLevel);
        UIManager.instance.onGetExp.AddListener(ChangedExp);
        UIManager.instance.onTSceneChanged.AddListener(TChangeScene);
        //UIManager.instance.onActionChanged.AddListener(ChangedAction); // ���õ� Action�� �ʹ� ���� �и��ϱ����� �ϴ� �ּ�ó��
        UIManager.instance.onTakeDamaged.AddListener(TakeDamaged);
        UIManager.instance.onStartAction.AddListener(StartAction);
        UIManager.instance.onNonHited.AddListener(NonHited);
        //UIManager.instance.onPlayerStatChanged.AddListener(ChangedPlayerStat); // ��� �÷��̾� ���Ⱥ�ȭ �����ϱ� ����� �ϴ� �ּ�ó��
        InstantiateText();
        _FixedTextedPanelHeight = 0;
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }

    public override void OpenUI()
    {
        base.OpenUI();
    }

    private void ChangedExp(int exp)
    {
        _builder.Append($"{exp} ����ġ�� ������ϴ�..\n");
        UpdateText();
    }
    private void ChangedLevel(int level)
    {
        _builder.Append($"������! {level} ������ �Ǿ����ϴ�.\n");
        UpdateText();
    }

    private void StartedTurn(Unit unit)
    {
        _builder.Append($"{unit.unitName}�� {unit.currentRound}��° ����\n");
        UpdateText();
    }

    private void TakeDamaged(Unit unit, int damage, eDamageType.Type type)
    {
        _builder.Append($"{unit.unitName}���� {damage} ����.\n");
        UpdateText();
    }

    private void NonHited(Unit unit)
    {
        _builder.Append($"{unit.unitName}�� ȸ���ߴ�.\n");
        UpdateText();
    }
    
    private void TChangeScene(GameState gameState)
    {
        if (gameState == GameState.Combat)
        {
            _builder.Append("- ���� ���� -\n");
            UpdateText();
        }
    }

    private void StartAction(Unit unit, BaseAction action)
    {
        string actionType = "UnknownAction";
        switch (action.GetActionType())
        {
            case ActionType.Attack: actionType = "����"; break;
            case ActionType.Dynamite: actionType = "���̳ʸ���Ʈ"; break;
            case ActionType.Fanning: actionType = "�д�"; break;
            case ActionType.Hemostasis: actionType = "����"; break;
            case ActionType.Move: actionType = "�̵�"; break;
            case ActionType.Reload: actionType = "������"; break;
            case ActionType.ItemUsing: 
                int itemIdx = ((ItemUsingAction)action).GetItem().GetData().nameIdx;
                ItemScript script = GameManager.instance.itemDatabase.GetItemScript(itemIdx);
                string itemName = script.GetName();
                actionType = $"{itemName} ���";
                break;
            default: break;
        }
        _builder.Append($"{unit.unitName}�� {actionType}.\n");
        UpdateText();
    }

    private void InstantiateText()
    {
        var target = Instantiate(_defaultTextPrefab);
        target.transform.SetParent(_logPanel);
        _textCaches.Add(target.GetComponent<TMP_Text>());
        _sizeFilterCaches.Add(target.GetComponent<ContentSizeFitter>());
        _curTextlistIndex++;
    }

    private void UpdateText()
    {
        _textCaches[_curTextlistIndex].SetText(_builder);
        _sizeFilterCaches[_curTextlistIndex].SetLayoutVertical();
        float newHeight = _beforeHeight + _textCaches[_curTextlistIndex].rectTransform.sizeDelta.y;
        _logPanel.sizeDelta = new Vector2(_logPanel.sizeDelta.x, newHeight);
        _scroll.verticalScrollbar.value = 0; // ��ũ���� ���� �Ʒ��� ���������� ����

        // ũ�Ⱑ ���� ���� �̻��̶��, ������ ����� �ؽ�Ʈ ������Ʈ�� �̸� �����.
        if (LIMIT_TEXT_LENGTH < _textCaches[_curTextlistIndex].text.Length)
        {
            _builder.Clear(); // �̹� ������ �ؽ�Ʈ ������Ʈ�� ��ü���� ���� �����̹Ƿ�, stringBuilder�� ����.
            _beforeHeight = _textCaches[_curTextlistIndex].rectTransform.sizeDelta.y;
            _FixedTextedPanelHeight = _FixedTextedPanelHeight + _textCaches[_curTextlistIndex].rectTransform.sizeDelta.y;
            InstantiateText();
        }
        _logPanel.sizeDelta = new Vector2(_logPanel.sizeDelta.x, _FixedTextedPanelHeight + _textCaches[_curTextlistIndex].rectTransform.sizeDelta.y);
    }
}
