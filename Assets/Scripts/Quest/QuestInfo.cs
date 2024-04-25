using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 1. CSV Init : ��� ����Ʈ ���� �о����
/// 2. Localization : Tooltip, Name �� ������ ��� �о����
/// 3. Async : ���� ���� �����Ϳ� �����ϴ�, ����Ʈ ���� ��Ȳ ����ȭ
/// 4. GetEvent : ��� ������ ����Ʈ�� Start�� ����
/// 5. GoalEvent : ���� �������� ����Ʈ�� Event�� ����
/// - GoalEvent���� �Ϸ� ������ �ȴٸ� EndQuest�� ����
/// </summary>
public class QuestInfo
{
    public enum QUEST_EVENT {   NULL
                                , GAME_START = 1 << 0
                                , MOVE_TO   = 1 << 1
                                , QUEST_END = 1 << 2
                                , SCRIPT    = 1 << 3
                                , GET_ITEM  = 1 << 4
                                , USE_ITEM  = 1 << 5
                                , KILL_LINK = 1 << 6
                                , KILL_UNIT = 1 << 7 
                                , LINK_IN_SIGHT = 1 << 8 
                                , TILE_IN_SIGHT = 1 << 9 }; // ����Ʈ�� ������ Bit����ũ�� Ȯ�ο�
    public UnityEvent<QuestInfo> OnQuestStarted = new UnityEvent<QuestInfo>();
    public UnityEvent<QuestInfo> OnQuestEnded = new UnityEvent<QuestInfo>(); // ����Ʈ �������ο� ���þ��� ����
    public UnityEvent OnChangedProgress = new UnityEvent();

    private int _index;
    private int _questType;
    private string _questName;
    private string _questTooltip;
    private int _startConversation;
    private int _endConversation;

    // conditions
    private QUEST_EVENT _conditionBit;
    private int[] _conditionArguments;
    private int _expireTurn;
    private QUEST_EVENT _goalBit;
    private int[] _goalArguments;

    private int[] _pinTile;
    private int[] _createLink;

    // rewards
    private int _moneyReward;
    private int _expReward;
    private int _itemReward;
    private int _skillReward;

    // current infos (not table)
    private bool _isInProgress = false;
    private bool _isCleared = false;
    private int _curTurn;
    private int[] _curConditionArguments;
    private int[] _curGoalArguments;

    public int Index { get => _index; }
    public int QuestType { get => _questType; }
    public string QuestName { get => _questName; }
    public string QuestTooltip { get => _questTooltip; }
    public int StartConversation { get => _startConversation; }
    public int EndConversation { get => _endConversation; }
    public int ExpireTurn { get => _expireTurn; }
    public QUEST_EVENT GOAL_TYPE { get => _goalBit; }
    public int[] GoalArg{ get => _goalArguments; }
    public int[] CurArg { get => _curGoalArguments; }
    public int[] Pin { get => _pinTile; }
    public int CurTurn { get => _curTurn; } // ExpireTurn���� �����Ͽ� 0���� ���� ���� ��. ex. {CurTurn}�� ����!
    public int MoneyReward { get => _moneyReward; }
    public int ExpReward { get => _expReward; }
    public int ItemReward { get => _itemReward; }
    public int SKillReward { get => _skillReward; }

    public QuestInfo(int index
                    , int questType
                    , string questName
                    , string questTooltip
                    , int startConversation
                    , int endConversation
                    , QUEST_EVENT conditionBit
                    , int[] conditionArgument
                    , int expireTurn
                    , QUEST_EVENT goalBit
                    , int[] goalArguments
                    , int[] pinTile
                    , int[] createLink
                    , int moneyReward
                    , int expReward
                    , int itemReward
                    , int skillReward)
    {
        _index = index;
        _questType = questType;
        _questName = questName;
        _questTooltip = questTooltip;
        _startConversation = startConversation;
        _endConversation = endConversation;
        _conditionBit = conditionBit;
        _conditionArguments = conditionArgument;
        _expireTurn = expireTurn;
        _goalBit = goalBit;
        _goalArguments = goalArguments;
        _pinTile = pinTile;
        _createLink = createLink;
        _moneyReward = moneyReward;
        _expReward = expReward;
        _itemReward = itemReward;
        _skillReward = skillReward;

        _curTurn = _expireTurn;
        _curConditionArguments = new int[_conditionArguments.Length];
        _curGoalArguments = new int[_goalArguments.Length];
    }

    public bool HasConditionFlag(QUEST_EVENT target)
    {
        if (_conditionBit.HasFlag(target)) return true;
        return false;
    }
    public bool HasGoalFlag(QUEST_EVENT target)
    {
        if (_goalBit.HasFlag(target)) return true;
        return false;
    }

    // ���� ����, �ε� �� ���� ��Ȳ�� �����ϱ� ����.
    public void SetProgress(bool isInProgress, int[] curGoalArguments, bool isCleared, int curTurn)
    {
        _isInProgress = isInProgress;
        _curGoalArguments = curGoalArguments;
        _isCleared = isCleared;
        _curTurn = curTurn;
    }

    // Game Start ó�� ���� ���� 1ȸ������ ������ �̺�Ʈ.
    // Game start �ܿ� �� ���� ���µ�, Game start ��ü�� �ӽ÷� �ص� ���̶� ���� �͵� ��� ��.
    public void OnOccurConditionEvented()
    {
        if (!_isInProgress)
        {
            StartQuest();
        }
    }

    public void OnAccordedConditionEvented(int index)
    {
        if (!_isInProgress)
        {
            if (AccordEvent(ref _curConditionArguments, ref _conditionArguments, index))
                StartQuest();
        }
    }
    public void OnAccordedGoalEvented(int index)
    {
        if (_isInProgress)
        {
            if (AccordEvent(ref _curGoalArguments, ref _goalArguments, index))
                SuccessQuest();
        }
    }

    #region Count Event
    // ����, ���� ó�� �̺�Ʈ������ �Ͼ���� �� ���� ī��Ʈ�ϴ� �̺�Ʈ
    // ex. [������ �ε���, ����ؾ��ϴ� ��]
    public void OnCountConditionEvented(int targetIndex)
    {
        if (!_isInProgress)
        {
            if (CountEvent(ref _curConditionArguments, ref _conditionArguments, targetIndex))
                StartQuest();
        }
    }

    public void OnCountGoalEvented(int targetIndex)
    {
        if (_isInProgress)
        {
            if (CountEvent(ref _curGoalArguments, ref _goalArguments, targetIndex))
                SuccessQuest();
        }
    }
    #endregion

    // �÷��̾� Move ���ڴ� �� �߰� �� ���ɼ��� �־ ������ �� ��
    // �÷��̾ World�� ���� ����, ���尡 ���� ��(��������) �������� ���ɼ��� �־ �ش� �ε�����.
    public void OnPositionMovedConditionEvented(Vector3Int position)
    {
        if (_isCleared) return;

        if (!GameManager.instance.CompareState(GameState.World))
            return;
        
        if (!_isInProgress)
        {
            if (OnPositionEvent(ref _curConditionArguments, ref _conditionArguments, position))
                StartQuest();
        }
    }

    public void OnPositionMovedGoalEvented(Vector3Int position)
    {
        if (_isCleared) return;

        if (!GameManager.instance.CompareState(GameState.World))
            return;

        if (_isInProgress)
        {
            if (OnPositionEvent(ref _curGoalArguments, ref _goalArguments, position))
                SuccessQuest();
        }
    }
    
    public void ProgressExpireTurn()
    {
        _curTurn--;
        if (_curTurn == 0)
        {
            FailQuest();
        }
    }

    /// ===========================================================================
    private void StartQuest()
    {
        if (_isCleared) return;

        _isInProgress = true;
        OnQuestStarted.Invoke(this);     
        PlayerEvents.OnStartedQuest.Invoke(this);
    }

    private void SuccessQuest()
    {
        _isInProgress = false;
        _isCleared = true;

        var itemDB = GameManager.instance.itemDatabase;
        if (_moneyReward != 0)
            GameManager.instance.playerInventory.AddGold(_moneyReward);
        if (_itemReward != 0)
            if (GameManager.instance.playerInventory.TryAddItem(Item.CreateItem(itemDB.GetItemData(_itemReward))))
            {
                Debug.Log($"����Ʈ �Ϸ� �������� ���� �� �����ϴ�.: itemcode '{_itemReward}'");
            }
        if (_skillReward != 0)
            SkillManager.instance.LearnSkill(_skillReward);
        if (_expReward != 0)
        {
            LevelSystem.GetExpImmediately(_expReward);
        }
        PlayerEvents.OnSuccessQuest.Invoke(this);
        OnQuestEnded?.Invoke(this);
    }

    private void FailQuest()
    {
        _isInProgress = false;
        _isCleared = true;
        PlayerEvents.OnFailedQuest.Invoke(this);
        OnQuestEnded?.Invoke(this);
    }

    private bool AccordEvent(ref int[] curArgument, ref int[] goalArgument, int value)
    {
        OnChangedProgress?.Invoke();
        if (goalArgument[0] != value)
            return false;
        curArgument[0] = value;
        return true;
    }

    // �÷��̾ ������ ��ġ �̺�Ʈ
    private bool OnPositionEvent(ref int[] cur, ref int[] goal, Vector3Int pos)
    {
        cur[0] = pos.x;
        cur[1] = pos.y;
        cur[2] = pos.z;

        OnChangedProgress?.Invoke();
        for (int i = 0; i < goal.Length; i++)
        {
            if (cur[i] != goal[i])
                return false;
        }

        return true;
    }

    private bool CountEvent(ref int[] curArgument, ref int[] goalArgument, int goalType)
    {
        if (curArgument[0] != goalType)
            return false;

        OnChangedProgress?.Invoke();
        curArgument[1] += 1;
        if (curArgument[1] < goalArgument[1])
            return false;
        return true;
    }
}