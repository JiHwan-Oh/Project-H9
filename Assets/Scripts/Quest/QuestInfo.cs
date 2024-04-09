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
    public UnityEvent<int> OnQuestEnded = new UnityEvent<int>();

    private int _index;
    private int _questType;
    private string _questName;
    private string _questTooltip;
    private int _startScript;
    private int _endScript;

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

    public QuestInfo(int index
                    , int questType
                    , string questName
                    , string questTooltip
                    , int startScript
                    , int endScript
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
        _startScript = startScript;
        _endScript = endScript;
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

    public void OnOccurQuestConditionEvented(int targetIndex)
    {
        if (!_isInProgress)
        {
            if (QuestEvent(ref _curConditionArguments, ref _conditionArguments, targetIndex))
                StartQuest();
        }
    }

    #region Add Event
    // �� óġ, ������ ȹ��� ���� KeyValue�� ���� �̺�Ʈ
    // ex. [���ǹ�ȣ, ����óġ�ؾ��ϴ� ��]
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
    }

    private void EndQuest() // ������ ������ �޴� ���̽��� ����.
    {
        _isInProgress = false;
        _isCleared = true;
        Debug.Log($"[{_index}]'{_questName}'����Ʈ �Ϸ�, ���� �޴� �ڵ�, endScript ȣ��, UI���� �ؾߵ�");
        OnQuestEnded?.Invoke(_index);
    }


    private bool QuestEvent(ref int[] curArgument, ref int[] goalArgument, int value)
    {
        if (goalArgument[0] != value)
            return false;
        curArgument[0] = value;
        return true;
    }

    private bool AddEvent(ref int[] curArgument, ref int[] goalArgument, int goalType, int value)
    {
        if (curArgument[0] != goalType)
            return false;

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

    private bool OnPlayerEvent(ref int[] cur, ref int[] goal, Vector3Int pos)
    {
        cur[0] = pos.x;
        cur[1] = pos.y;
        cur[2] = pos.z;

        for (int i = 0; i < goal.Length; i++)
        {
            if (cur[i] != goal[i])
                return false;
        }

        return true;
    }
}