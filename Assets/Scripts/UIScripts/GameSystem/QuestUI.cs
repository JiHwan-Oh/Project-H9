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

    void Awake()
    {
        _listPool = new QuestListPool();
        _listPool.Init("Prefab/Quest List UI Element", _listElementContainer.transform, 0);
        _questWindow.SetActive(true);
    }

    public void AddQuestListUI(QuestInfo info) 
    {
        var ui = _listPool.Set();
        if (info.QuestType == 1) ui.Instance.transform.SetAsFirstSibling();
        ui.Instance.GetComponent<QuestListElement>().SetQuestListElement(info);
        Debug.Log(ui);

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
    public void OpenItemTooltip(int index, Vector3 pos) 
    {
        _itemTooltip.GetComponent<InventoryUITooltip>().SetInventoryUITooltip(GameManager.instance.itemDatabase.GetItemData(index), pos);
    }
    public void OpenSkillTooltip(int index, Vector3 pos)
    {
        _skillTooltip.GetComponent<SkillTooltip>().SetSkillTooltip(index, pos);
    }
    public override void ClosePopupWindow()
    {
        _itemTooltip.GetComponent<InventoryUITooltip>().CloseUI();
        _skillTooltip.GetComponent<SkillTooltip>().CloseUI();
        base.ClosePopupWindow();
    }
}
