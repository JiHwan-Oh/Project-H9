using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// ������ ����Ʈ�� ǥ���ϴ� ����� ������ Ŭ����
/// </summary>
public class QuestUI : UISystem
{
    [SerializeField] private GameObject _questText;
    [SerializeField] private GameObject _questWindow;
    [SerializeField] private GameObject _listElementContainer;
    [SerializeField] private GameObject _itemTooltip;
    [SerializeField] private GameObject _skillTooltip;
    [SerializeField] private GameObject _popupWindow;
    [SerializeField] private GameObject _popupText;

    private WorldCamera worldCamera;

    private QuestListPool _listPool = null;

    private List<QuestInfo> _currentProgressingQuests = new();

    void Awake()
    {
        _popupWindow.SetActive(false);
        _listPool = new QuestListPool();
        _listPool.Init("Prefab/Quest List UI Element", _listElementContainer.transform, 0);
        _questWindow.SetActive(true);

        worldCamera = CameraManager.instance.worldCamera;

        UIManager.instance.onTSceneChanged.AddListener((s) =>
        {
            if (s == GameState.World)
            {
                _questWindow.SetActive(true);
            }
            else if (s == GameState.Combat)
            {
                _questWindow.SetActive(false);
            }
        });
    }
    private void Start()
    {
        _questText.GetComponent<TextMeshProUGUI>().text = UIManager.instance.UILocalization[200];
    }

    public void AddQuestListUI(QuestInfo info) 
    {
        var ui = _listPool.Set();
        if (info.Pin.Length > 0) 
        {
            Vector3Int pinPos = new Vector3Int(info.Pin[0], info.Pin[1], info.Pin[2]);
            UIManager.instance.gameSystemUI.pinUI.SetPinUI(pinPos);
            Tile target = FieldSystem.tileSystem.GetTile(pinPos);
            if (target is null)
            {
                Debug.LogError("�ش� Hex ��ǥ�� Tile�� �������� �ʽ��ϴ�. Hex ��ǥ: " + pinPos);
                return;
            }
            Vector3 _targetPos = target.gameObject.transform.position;
            worldCamera.SetPosition(_targetPos);
        }
        ui.Instance.GetComponent<QuestListElement>().SetQuestListElement(info, out string popupText);
        _listPool.Sort();
        StartCoroutine(StartQuestUI(popupText));
        _currentProgressingQuests.Add(info);
    }
    public void DeleteQuestListUI(QuestInfo info) 
    {
        QuestListElement listElement = _listPool.Find(info.Index);
        if (listElement == null) 
        {
            Debug.Log("�ش� �ε����� ����Ʈ�� Ȱ��ȭ�Ǿ����� �ʽ��ϴ�. �ε���: " + info.Index);
            return;
        }

        _listPool.Sort();

        string popupText;
        if (info.CurTurn == 0)
        {
            listElement.FailQuestUI(out popupText);
        }
        else
        {
            listElement.CompleteQuestUI(out popupText);
        }
        StartCoroutine(EndQuestUI(popupText));
        _currentProgressingQuests.Remove(info);
        GameManager.instance.user.ClearedQuests.Add(info.Index);
    }
    IEnumerator StartQuestUI(string popupText)
    {
        while (true)
        {
            _popupText.GetComponent<TextMeshProUGUI>().text = popupText;
            _popupWindow.SetActive(true);

            yield return new WaitForSeconds(1.5f);
            _popupWindow.SetActive(false);
            yield break;
        }
    }
    IEnumerator EndQuestUI(string popupText) 
    {
        while (true)
        {
            _popupText.GetComponent<TextMeshProUGUI>().text = popupText;
            _popupWindow.SetActive(true);
            Player player = FieldSystem.unitSystem.GetPlayer();
            if (player == null)
            {
                yield return new WaitForSeconds(2.0f);
                continue;
            }
            var actions = player.GetUnitActionArray();
            foreach (var a in actions)
            {
                if (a is IdleAction)
                {
                    player.SelectAction(a);
                }
            }

            yield return new WaitForSeconds(1.5f);
            _popupWindow.SetActive(false);
            yield return new WaitForSeconds(.5f);

            player = FieldSystem.unitSystem.GetPlayer();
            if (player == null) 
            {
                yield break;
            } 
            foreach (var a in actions)
            {
                if (a is MoveAction)
                {
                    player.SelectAction(a);
                }
            }
            
            
            yield break;
        }
    }

    public void ClickQuestButton()
    {
        _questWindow.SetActive(!_questWindow.activeSelf);
    }
    public void OpenItemTooltip(int index, Vector3 pos) 
    {
        pos = ScreenOverCorrector.GetCorrectedUIPosition(GetComponent<Canvas>(), pos, _itemTooltip);
        _itemTooltip.GetComponent<InventoryUITooltip>().SetInventoryUITooltip(GameManager.instance.itemDatabase.GetItemData(index), pos, true);
    }
    public void OpenSkillTooltip(int index, Vector3 pos)
    {
        pos = ScreenOverCorrector.GetCorrectedUIPosition(GetComponent<Canvas>(), pos, _skillTooltip);
        _skillTooltip.GetComponent<SkillTooltip>().SetSkillTooltip(index, pos);
    }
    public override void ClosePopupWindow()
    {
        if(_itemTooltip.activeSelf) _itemTooltip.GetComponent<InventoryUITooltip>().CloseUI();
        if (_skillTooltip.activeSelf) _skillTooltip.GetComponent<SkillTooltip>().CloseUI();
        base.ClosePopupWindow();
    }

    public List<QuestInfo> GetCurrentProgressingQuests => _currentProgressingQuests;
}
