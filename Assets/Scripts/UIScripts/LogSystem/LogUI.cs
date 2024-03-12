using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogUI : UISystem
{
    [SerializeField]
    private RectTransform _logPanel;

    [SerializeField]
    private GameObject _defaultTextPrefab;

    [SerializeField]
    private ScrollRect _scroll;

    private List<TMP_Text> _textCaches= new List<TMP_Text>();
    private List<ContentSizeFitter> _sizeFilterCaches = new List<ContentSizeFitter>();
    private StringBuilder _builder = new StringBuilder();
    private int _curTextlistIndex = -1;
    private float _beforeHeight = 0;
    
    private readonly int LIMIT_TEXT_LENGTH = 500;

    private void Awake()
    {
        UIManager.instance.onPlayerStatChanged.AddListener(ChangedPlayerStat);
        UIManager.instance.onTurnChanged.AddListener(ChangedTurn);
        UIManager.instance.onActionChanged.AddListener(ChangedAction);
        InstantiateText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangedPlayerStat();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangedTurn();
        }
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }

    public override void OpenUI()
    {
        base.OpenUI();
    }

    private void ChangedPlayerStat()
    {
        _builder.Append("�÷��̾� ������ ��ȭ�߽��ϴ�.\n");
        UpdateText();
    }

    private void ChangedTurn()
    {
        _builder.Append("���� ��ȭ�߽��ϴ�.\n");
        UpdateText();
    }

    private void ChangedAction()
    {
        _builder.Append("�׼��� ��ȭ�߽��ϴ�.\n");
        UpdateText();
    }

    private void InstantiateText()
    {
        _builder.Append("<color:red>���ο� �����Դϴ�.</color>\n");
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
            InstantiateText();
        }
    }
}
