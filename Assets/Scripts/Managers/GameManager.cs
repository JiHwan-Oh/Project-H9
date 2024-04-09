using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum GameState
{
    Combat,
    World,
    Editor
}
public class GameManager : Generic.Singleton<GameManager>
{
    private const string COMBAT_SCENE_NAME = "CombatScene";

    public Inventory playerInventory = new Inventory();
    [SerializeField]
    public ItemDatabase itemDatabase;
    public WeaponDatabase weaponDatabase;
    public List<QuestInfo> Quests;
    
    #region ITEM_TEST
    public void AddItem(int id)
    {
        if (itemDatabase == null)
        {
            Debug.LogError("ItemDatabase is not initialized");
            return;
        }

        ItemData itemData = itemDatabase.GetItemData(id);

        if (itemData == null)
        {
            Debug.LogError($"No item data found for id {id}");
            return;
        }

        Item item = Item.CreateItem(itemData);

        playerInventory.TryAddItem(item);

        Debug.Log("Added item to inventory");
    }//a
    #endregion

    private HashSet<Vector3Int> _discoveredWorldTileSet;
    
    [SerializeField]
    private GameState _currentState = GameState.World;

    [SerializeField] private CombatStageData _stageData;
    [SerializeField]
    private int _currentLinkIndex = -1;

    [Header("Player Info")]
    public Vector3Int playerWorldPos;
    public UnitStat playerStat;
    [SerializeField] private int _playerWeaponIndex;
    public int PlayerWeaponIndex
    {
        get => _playerWeaponIndex;
        set
        {
            _playerWeaponIndex = value;
            Weapon weapon = weaponDatabase.Clone(value);
            onPlayerWeaponChanged.Invoke(weapon); 
        }
    }
    public GameObject playerModel;
    public List<int> playerPassiveIndexList;
    public List<int> playerActiveIndexList;
    
    public UnityEvent<Weapon> onPlayerWeaponChanged = new UnityEvent<Weapon>();

    #region LEVEL

    [Header("Level system")]
    public int level = 1;
    public int curExp = 0;
    private int maxExp => level * 100;
    private const int LEVEL_UP_REWARD_SKILL_POINT = 1;
    public void GetExp(int exp)
    {
        curExp += exp;
        while (curExp >= maxExp)
        {
            LevelUp();
        }
        UIManager.instance.onPlayerStatChanged.Invoke();
        UIManager.instance.onGetExp.Invoke(exp);
    }
    private void LevelUp()
    {
        if (maxExp > curExp) return;

        curExp -= maxExp;
        level++;
        playerStat.Recover(StatType.CurHp, playerStat.GetStat(StatType.MaxHp), out var appliedValue);
        SkillManager.instance.AddSkillPoint(LEVEL_UP_REWARD_SKILL_POINT);
        if (level % 3 == 0)
        {
            UIManager.instance.gameSystemUI.playerStatLevelUpUI.OpenPlayerStatLevelUpUI();
            UIManager.instance.onLevelUp.Invoke(level);
        }
    }
    public int GetMaxExp()
    {
        return maxExp;
    }
    #endregion

    [Header("World Scene Name")]
    public string worldSceneName;

    [Header("World Info")]
    public int worldAp;
    public int worldTurn;

    public bool backToWorldTrigger = false;

    private UnityEvent OnGameStarted = new UnityEvent();
    private UnityEvent<int> OnNotifiedQuestEvent = new UnityEvent<int>();

    public void StartCombat(int stageIndex, int linkIndex)
    {
        //Save World Data
        worldAp = FieldSystem.unitSystem.GetPlayer().currentActionPoint;
        worldTurn = FieldSystem.turnSystem.turnNumber;

        playerWorldPos = FieldSystem.unitSystem.GetPlayer().hexPosition;
        ChangeState(GameState.Combat);
        _currentLinkIndex = linkIndex;
        _stageData = Resources.Load<CombatStageData>($"Map Data/Stage {stageIndex}");
        LoadingManager.instance.LoadingScene(COMBAT_SCENE_NAME);
    }
    
    public int GetLinkIndex()
    {
        return _currentLinkIndex;
    }
    
    public CombatStageData GetStageData()
    {
        return _stageData;
    }

    public void FinishCombat()
    {
        ChangeState(GameState.World);

        backToWorldTrigger = true;
        LoadingManager.instance.LoadingScene(worldSceneName);
    }

    public void SetEditor()
    {
        ChangeState(GameState.Editor);
    }

    public bool CompareState(GameState state)
    {
        return _currentState == state;
    }

    private void ChangeState(GameState state)
    {
        if (CompareState(state)) return;

        _currentState = state;
    }

    public void AddPlayerSkillListElement(SkillInfo skillInfo)
    {
        List<int> list = null;
        if (skillInfo.IsPassive())
        {
            list = playerPassiveIndexList;
        }
        else
        {
            list = playerActiveIndexList;
        }

        int previousSkillPositionIndex = -1;
        if (skillInfo.IsActive())
        {
            ActionType addedActionType = SkillManager.instance.activeDB.GetActiveInfo(skillInfo.index).action;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                ActionType actionType = SkillManager.instance.activeDB.GetActiveInfo(list[i]).action;
                if (addedActionType == actionType) 
                {
                    list.RemoveAt(i);
                    previousSkillPositionIndex = i;
                }
            }
        }
        if (previousSkillPositionIndex == -1)
        {
            list.Add(skillInfo.index);
        }
        else
        {
            list.Insert(previousSkillPositionIndex, skillInfo.index);
        }
    }
    
    public bool IsPioneeredWorldTile(Vector3Int tilePos)
    {
        return _discoveredWorldTileSet.Contains(tilePos);
    }
    
    public List<Vector3Int> GetPioneeredWorldTileList()
    {
        return new List<Vector3Int>(_discoveredWorldTileSet);
    }

    public void AddPioneeredWorldTile(Vector3Int tilePos)
    {
        if (_discoveredWorldTileSet.Contains(tilePos)) return;
        
        _discoveredWorldTileSet.Add(tilePos);
    }
    
    private new void Awake()
    {
        base.Awake();
        
        _discoveredWorldTileSet = new ();
        var qi = new QuestInit();
        Quests = qi.GetQuests();
    }

    private void Start()
    {
        // 게임 시작시 시작하는 퀘스트 연결
        foreach (var quest in Quests)
        {
            if (quest.HasConditionFlag(QuestInfo.QUEST_EVENT.GAME_START))
                OnGameStarted.AddListener(quest.OnOccurConditionEvented);
        }

        // 퀘스트 완료시 Invoke 함수 호출
        // 퀘스트 완료시 시작하는 퀘스트 연결
        foreach (var quest in Quests)
        {
            quest.OnQuestEnded.AddListener(InvokeQuestEnd);
            if (quest.HasConditionFlag(QuestInfo.QUEST_EVENT.QUEST_END))
                OnNotifiedQuestEvent.AddListener(quest.OnOccurQuestConditionEvented);
        }

        // 퀘스트 조건의 MOVE_TO 호출
        // 퀘스트 완료시 MOVE_TO 연결
        foreach (var quest in Quests)
        {
            if (quest.HasConditionFlag(QuestInfo.QUEST_EVENT.MOVE_TO))
                FieldSystem.unitSystem.GetPlayer().onMoved.AddListener(quest.OnPlayerMovedConditionEvented);
            if (quest.HasGoalFlag(QuestInfo.QUEST_EVENT.MOVE_TO))
                FieldSystem.unitSystem.GetPlayer().onMoved.AddListener(quest.OnPlayerMovedGoalEvented);
        }

        OnGameStarted?.Invoke();
    }

    public void Update()
    {
        var deltaTime = Time.deltaTime;
        Service.OnUpdated(deltaTime);
        
        #region ITEM_TEST

        if (Input.GetKeyDown(KeyCode.F1))
        {
            //add all item
            var allItems = itemDatabase.GetAllItemData();

            for (int i = 1; i < allItems.Capacity; i++)
            {
                ItemData itemData = allItems[i];
                //Debug.Log(itemData.id);
                AddItem(itemData.id);
            }
        }
        #endregion
    }

    private void InvokeQuestEnd(int questIndex)
    {
        OnNotifiedQuestEvent?.Invoke(questIndex);
    }
}
