using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConversationUI : UISystem
{
    [SerializeField] private GameObject _conversationWindow;
    [SerializeField] private GameObject _speakerText;
    [SerializeField] private GameObject _contentsText;

    private List<ConversationInfo> _conversationInfo;
    private List<ConversationInfo> _groupInfo;
    private int _sequenceNumber;

    private void Awake()
    {
        _conversationWindow.SetActive(false);

        List<List<string>> conversationTable = FileRead.Read("ConversationTable");
        if (conversationTable == null)
        {
            Debug.LogError("대화 테이블을 찾을 수 없습니다.");
            return;
        }
        _groupInfo = null;
        _sequenceNumber = 0;

        _conversationInfo = new List<ConversationInfo>();
        for (int i = 0; i < conversationTable.Count; i++)
        {
            int idx = int.Parse(conversationTable[i][0]);
            int g = int.Parse(conversationTable[i][1]);
            int s = int.Parse(conversationTable[i][2]);
            string n = conversationTable[i][3];
            string o = conversationTable[i][5];
            string t = conversationTable[i][3 + (int)UIManager.instance.scriptLanguage];
            ConversationInfo info = new ConversationInfo(idx, g, s, n, o, t);
            _conversationInfo.Add(info);
        }
    }
    private List<ConversationInfo> GetConversationGroup(int g)
    {
        List<ConversationInfo> list = new List<ConversationInfo>();
        List<ConversationInfo> sortedlist = new List<ConversationInfo>();

        for (int i = 0; i < _conversationInfo.Count; i++) 
        {
            if (_conversationInfo[i].group == g) 
            {
                list.Add(_conversationInfo[i]);
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].sequence == i)
            {
                sortedlist.Add(list[i]);
            }
        }

        if (sortedlist.Count == 0) sortedlist = null;
        return sortedlist;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            StartConversation(2);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            ProgressConversation();
        }
    }

    public void StartConversation(int group) 
    {
        //UIManager.instance.gameSystemUI.conversationUI.StartConversation(1);
        _groupInfo = GetConversationGroup(group);
        if (_groupInfo == null) return;

        _conversationWindow.SetActive(true);
        _sequenceNumber = 0;
        _speakerText.GetComponent<TextMeshProUGUI>().text = _groupInfo[0].speakerName;
        _contentsText.GetComponent<TextMeshProUGUI>().text = _groupInfo[0].conversationText;
    }
    public void ProgressConversation() 
    {
        if (_groupInfo == null) return;
        _sequenceNumber++;
        if (_groupInfo.Count <= _sequenceNumber)
        {
            EndConversation();
        }
        else
        {
            _speakerText.GetComponent<TextMeshProUGUI>().text = _groupInfo[_sequenceNumber].speakerName;
            _contentsText.GetComponent<TextMeshProUGUI>().text = _groupInfo[_sequenceNumber].conversationText;
        }
    }
    private void EndConversation() 
    {
        _groupInfo = null;
        _sequenceNumber = 0;
        _conversationWindow.SetActive(false);
    }
}

public class ConversationInfo 
{
    public int index { get; private set; }
    public int group { get; private set; }
    public int sequence { get; private set; }
    public string speakerName { get; private set; }
    public string originalConversationText { get; private set; }
    public string conversationText { get; private set; }

    public ConversationInfo(int i, int g, int s, string name, string originText, string text) 
    {
        index = i;
        group = g;
        sequence = s;
        speakerName = name;
        originalConversationText = originText;
        conversationText = text;
    } 
}
