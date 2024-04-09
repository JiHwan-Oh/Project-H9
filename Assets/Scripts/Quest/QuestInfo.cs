using System.Collections.Generic;
using UnityEngine;

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
    public enum QUEST_EVENT { NULL, GAME_START, MOVE_TO, QUEST_END, SCRIPT, GET_ITEM, USE_ITEM, KILL_LINK, KILL_TARGET }; // ����Ʈ�� ������ Bit����ũ�� Ȯ�ο�

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
        _expireTurn = expireTurn;
        _goalBit = goalBit;
        _goalArguments = goalArguments;
        _moneyReward = moneyReward;
        _expReward = expReward;
        _itemReward = itemReward;
        _skillReward = skillReward;
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
    public void SetProgress(bool isInProgress, int[] curGoalArguments)
    {
        _isInProgress = isInProgress;
        _curGoalArguments = curGoalArguments;
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

    public void OnAddConditionEvented(int targetIndex, int value)
    {
        if (!_isInProgress)
        {
            if (AddEvent(ref _curConditionArguments, ref _conditionArguments, targetIndex, value))
                StartQuest();
        }
    }

    // �� óġ, ������ ȹ��� ���� KeyValue�� ���� �̺�Ʈ
    // ex. [���ǹ�ȣ, ����óġ�ؾ��ϴ� ��]
    public void OnAddGoalEvented(int targetIndex, int value)
    {
        if (_isInProgress)
        {
            if (AddEvent(ref _curGoalArguments, ref _goalArguments, targetIndex, value))
                EndQuest();
        }
    }

    public void OnRenewConditionEvented(int[] arguments)
    { 
        if (!_isInProgress)
        {
            if (RenewEvent(ref _curConditionArguments, ref _conditionArguments, ref arguments))
                StartQuest();
        }
    }

    // ��ǥ ����ó�� ��� ���ڰ� ���ƾ� �ϴ� Event�� �Ҵ�
    public void OnRenewGoalEvented(int[] arguments)
    {
        if (_isInProgress)
        {
            if (RenewEvent(ref _curGoalArguments, ref _goalArguments, ref arguments))
                EndQuest();
        }
    }

    public void StartQuest()
    {
        _isInProgress = true;
        Debug.Log("����Ʈ ����, startScript ����");
    }

    public void EndQuest() // ������ ������ �޴� ���̽��� ����.
    {
        _isInProgress = false;
        Debug.Log("����Ʈ �Ϸ�, ���� �޴� �ڵ�, endScript ȣ��");
    }

    /// ===========================================================================
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
}
