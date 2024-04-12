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
                                , KILL_TARGET = 1 << 7 }; // ����Ʈ�� ������ Bit����ũ�� Ȯ�ο�
    public UnityEvent<QuestInfo> OnQuestStarted = new UnityEvent<QuestInfo>();
    public UnityEvent<QuestInfo> OnQuestEnded = new UnityEvent<QuestInfo>();
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
        _moneyReward = moneyReward;
        _expReward = expReward;
        _itemReward = itemReward;
        _skillReward = skillReward;

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
    public void SetProgress(bool isInProgress, int[] curGoalArguments, bool isCleared)
    {
        _isInProgress = isInProgress;
        _curGoalArguments = curGoalArguments;
        _isCleared = isCleared;
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

    public void OnOccurQuestConditionEvented(QuestInfo quest)
    {
        if (!_isInProgress)
        {
            if (QuestEvent(ref _curConditionArguments, ref _conditionArguments, quest.Index))
                StartQuest();
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
                EndQuest();
        }
    }
    #endregion

    #region Add Event
    // ������ ȹ��� ���� KeyValue�� ���� �̺�Ʈ
    // ex. [������ �ε���, ����ؾ��ϴ� ��]
    // Count Event�� �����ұ� �ͱ⵵ ��. Add�� �̷������ ���� ���� ����.
    public void OnAddConditionEvented(int targetIndex, int value)
    {
        if (!_isInProgress)
        {
            if (AddEvent(ref _curConditionArguments, ref _conditionArguments, targetIndex, value))
                StartQuest();
        }
    }

    public void OnAddGoalEvented(int targetIndex, int value)
    {
        if (_isInProgress)
        {
            if (AddEvent(ref _curGoalArguments, ref _goalArguments, targetIndex, value))
                EndQuest();
        }
    }
    #endregion

    #region Renew Event
    // ��� ���ڰ� ���ƾ� �ϴ� Event�� �Ҵ� 
    public void OnRenewConditionEvented(int[] arguments)
    {
        if (!_isInProgress)
        {
            if (RenewEvent(ref _curConditionArguments, ref _conditionArguments, ref arguments))
                StartQuest();
        }
    }

    public void OnRenewGoalEvented(int[] arguments)
    {
        if (_isInProgress)
        {
            if (RenewEvent(ref _curGoalArguments, ref _goalArguments, ref arguments))
                EndQuest();
        }
    }
    #endregion

    // �÷��̾� Move ���ڴ� �� �߰� �� ���ɼ��� �־ ������ �� ��
    // �÷��̾ World�� ���� ����, ���尡 ���� ��(��������) �������� ���ɼ��� �־ �ش� �ε�����.
    public void OnPlayerMovedConditionEvented(Unit player)
    {
        if (_isCleared) return;

        if (!GameManager.instance.CompareState(GameState.World))
            return;
        
        if (!_isInProgress)
        {
            if (OnPlayerEvent(ref _curConditionArguments, ref _conditionArguments, player.hexPosition))
                StartQuest();
        }
    }

    public void OnPlayerMovedGoalEvented(Unit player)
    {
        if (_isCleared) return;

        if (!GameManager.instance.CompareState(GameState.World))
            return;

        if (_isInProgress)
        {
            if (OnPlayerEvent(ref _curGoalArguments, ref _goalArguments, player.hexPosition))
                EndQuest();
        }
    }

    /// ===========================================================================
    private void StartQuest()
    {
        if (_isCleared) return;

        _isInProgress = true;
        Debug.Log($"[{_index}]'{_questName}' ����Ʈ ����, startScript ����, UI���� �ؾߵ�");
        OnQuestStarted.Invoke(this);
    }

    private void EndQuest()
    {
        _isInProgress = false;
        _isCleared = true;
        OnQuestEnded?.Invoke(this);

        var itemDB = GameManager.instance.itemDatabase;
        GameManager.instance.playerInventory.AddGold(_moneyReward);
        if (GameManager.instance.playerInventory.TryAddItem(Item.CreateItem(itemDB.GetItemData(_itemReward))))
        {
            Debug.Log($"����Ʈ �Ϸ� �������� ���� �� �����ϴ�.: itemcode '{_itemReward}'");
        }
        SkillManager.instance.LearnSkill(_skillReward);
        LevelSystem.ReservationExp(_expReward);
    }


    private bool QuestEvent(ref int[] curArgument, ref int[] goalArgument, int value)
    {
        OnChangedProgress?.Invoke();
        if (goalArgument[0] != value)
            return false;
        curArgument[0] = value;
        return true;
    }

    private bool AddEvent(ref int[] curArgument, ref int[] goalArgument, int goalType, int value)
    {
        if (curArgument[0] != goalType)
            return false;

        OnChangedProgress?.Invoke();
        curArgument[1] += value;
        if (curArgument[1] < goalArgument[1])
            return false;

        curArgument[1] = goalArgument[1]; // UI���� ǥ���� ��, ������ �Ѿ�� �ʵ��� ��.
        return true;
    }

    // ownArgument: ���������� �÷��̾��� ��ġ(��)
    // goalArrgument: �÷��̾ ����ϴ� ��ġ
    // curArgument: �÷��̾ ���� ������ ��ġ
    private bool RenewEvent(ref int[] ownArgument, ref int[] goalArgument, ref int[] curArgument)
    {
        bool isSame = true;
        OnChangedProgress?.Invoke();
        for (int i = 0; i < goalArgument.Length; i++)
        {
            ownArgument[i] = curArgument[i];
            if (goalArgument[i] != curArgument[i])
                isSame = false;
        }

        if (!isSame)
            return false;

        return true;
    }

    // �÷��̾ ������ ��ġ �̺�Ʈ
    private bool OnPlayerEvent(ref int[] cur, ref int[] goal, Vector3Int pos)
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