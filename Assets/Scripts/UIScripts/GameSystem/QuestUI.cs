using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ ����Ʈ�� ǥ���ϴ� ����� ������ Ŭ����
/// </summary>
public class QuestUI : UISystem
{
    [SerializeField] private GameObject _questWindow;
    [SerializeField] private GameObject _listElementContainer;
    [SerializeField] private GameObject _itemTooltip;
    [SerializeField] private GameObject _skillTooltip;

    private QuestListPool _listPool = null;

    void Start()
    {
        _questWindow.SetActive(true);
    }
    public void SetQuestListUI()
    {
        //var quests = getQuests()

        //_listPool.Reset();

        //var mainQuestUI = _listPool.Set();
        //var mainQuest = null;
        //for (int i = 0; i < quests.Count; i++) 
        //{
        //    if (quests[i].isMainQuest) 
        //    {
        //        mainQuest = quests[i];
        //        continue;
        //    }
        //    var ui = _listPool.Set();
        //    ui.Instance.GetComponent<QuestListElement>().SetQuestListElement(quests[i]);
        //}
        //if (mainQuest == null) 
        //{
        //    Debug.LogError("���� ����Ʈ�� �������� �ʾ� UI�� ǥ���� �� �����ϴ�.");
        //    return;
        //}
        //mainQuestUI.Instance.GetComponent<QuestListElement>().SetQuestListElement(mainQuest);
    }

    public void AddQuestListUI() 
    {

    }
    public void DeleteQuestListUI(int idx) 
    {
        QuestListElement listElement = _listPool.Find(idx);
        if (listElement == null) 
        {
            Debug.Log("�ش� �ε����� ����Ʈ�� Ȱ��ȭ�Ǿ����� �ʽ��ϴ�. �ε���: " + idx);
            return;
        }

        listElement.CompleteQuestUI();
    }

    public void ClickQuestButton()
    {
        _questWindow.SetActive(!_questWindow.activeSelf);
    }
}
