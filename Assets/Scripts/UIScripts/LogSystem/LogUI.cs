using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogUI : UISystem
{
    [SerializeField]
    private RectTransform _logView; // on/off �� ���� �Ѵ� â

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
    private readonly string LOCALIZATION_PATH = "LogLocalizationTable";
    private Dictionary<int, string> localization;

    private const int LIMIT_TEXT_LENGTH = 500;

    private void Awake()
    {
        FileRead.ParseLocalization(in LOCALIZATION_PATH, out localization);

        UIManager.instance.onLevelUp.AddListener(ChangedLevel);
        UIManager.instance.onGetExp.AddListener(ChangedExp);
        UIManager.instance.onTSceneChanged.AddListener(TChangeScene);
        UIManager.instance.onTakeDamaged.AddListener(TakeDamaged);
        UIManager.instance.onStartAction.AddListener(StartAction);
        UIManager.instance.onNonHited.AddListener(NonHited);
        PlayerEvents.OnProcessedWorldTurn.AddListener(ProcessedWorldTurn);
        UIManager.instance.onStartedCombatTurn.AddListener(StartedCombatTurn);
        //UIManager.instance.onPlayerStatChanged.AddListener(ChangedPlayerStat); // ��� �÷��̾� ���Ⱥ�ȭ �����ϱ� ����� �ϴ� �ּ�ó��
        InstantiateText();
        _FixedTextedPanelHeight = 0;
    }


    public override void CloseUI()
    {
        base.CloseUI();
        _logView.gameObject.SetActive(false);
    }

    public override void OpenUI()
    {
        base.OpenUI();
        _logView.gameObject.SetActive(true);
    }

    public void ViewToggle()
    {
        if (_logView.gameObject.activeSelf) CloseUI();
        else OpenUI();
    }

    private void ChangedExp(int exp)
    {
        var message = localization[1].Replace("{exp}", exp.ToString());
        _builder.Append($"{message}\n");
        UpdateText();
    }
    private void ChangedLevel(int level)
    {
        var message = localization[2].Replace("{level}", level.ToString());
        _builder.Append($"{message}\n");
        UpdateText();
    }

    private void ProcessedWorldTurn(int turn)
    {
        var message = localization[3].Replace("{turn}", turn.ToString());
        _builder.Append($"{message}\n");
        UpdateText();
    }

    private void StartedCombatTurn(Unit unit)
    {
        var message = localization[4].Replace("{unitName}", unit.unitName.ToString());
        _builder.Append($"{message}\n");
        UpdateText();
    }

    private void TakeDamaged(Unit unit, int damage, eDamageType.Type type)
    {
        var message = localization[5].Replace("{unitName}", unit.unitName.ToString());
        message = message.Replace("{damage}", damage.ToString());
        _builder.Append($"{message}\n");
        UpdateText();
    }

    private void NonHited(Unit unit)
    {
        var message = localization[6].Replace("{unitName}", unit.unitName.ToString());
        _builder.Append($"{message}\n");
        UpdateText();
    }
    
    private void TChangeScene(GameState gameState)
    {
        if (gameState == GameState.Combat)
        {
            var message = localization[7];
            _builder.Append($"{message}\n");
            UpdateText();
        }
    }

    private void StartAction(Unit unit, BaseAction action)
    {
        string actionType = localization[11];
        switch (action.GetActionType())
        {
            case ActionType.Attack: actionType = localization[12]; break;
            case ActionType.Dynamite: actionType = localization[13]; break;
            case ActionType.Fanning: actionType = localization[14]; break;
            case ActionType.Hemostasis: actionType = localization[15]; break;
            case ActionType.Move: actionType = localization[16]; break;
            case ActionType.Reload: actionType = localization[17]; break;
            case ActionType.ItemUsing: 
                int itemIdx = ((ItemUsingAction)action).GetItem().GetData().nameIdx;
                ItemScript script = GameManager.instance.itemDatabase.GetItemScript(itemIdx);
                string itemName = script.GetName();
                var itemMessage = localization[18].Replace("{itemName}", itemName.ToString());
                actionType = itemMessage;
                break;
            default: break;
        }
        var message = localization[10].Replace("{unitName}", unit.unitName.ToString());
        message = message.Replace("{action}", actionType.ToString());
        _builder.Append($"{message}\n");
        UpdateText();
    }

    private void InstantiateText()
    {
        var target = Instantiate(_defaultTextPrefab);
        target.transform.SetParent(_logPanel);
        target.transform.localScale = Vector3.one; // ��Ȥ canvas�� scale�� ���ϴ� �� ������ �����ص�. scale ��ȯ��ų �� ������ �� �� ����.
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
