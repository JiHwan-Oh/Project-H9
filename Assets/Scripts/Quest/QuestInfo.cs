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
    private int _index;
    private int _questType;
    private string _questName;
    private string _questTooltip;
    private int _startScript;
    private int _endScript;

    // conditions
    private string _getCondition;
    private int[] _getConditionArguments;
    private int _expireTurn;
    private int _goalType;
    private int[] _goalArguments;
    
    // rewards
    private int _moneyReward;
    private int _expReward;
    private int _itemReward;
    private int _skillReward;

    // current infos (not table)
    private bool _isInProgress = false;
    private int _curTurn;
    private int[] _curGoalArguments;

    public QuestInfo(int index
                    , int questType
                    , string questName
                    , string questTooltip
                    , int startScript
                    , int endScript
                    , string getCondition
                    , int expireTurn
                    , int goalType
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
        _getCondition = getCondition;
        _expireTurn = expireTurn;
        _goalType = goalType;
        _goalArguments = goalArguments;
        _moneyReward = moneyReward;
        _expReward = expReward;
        _itemReward = itemReward;
        _skillReward = skillReward;
    }

    // ���� ����, �ε� �� ���� ��Ȳ�� �����ϱ� ����.
    public void SetProgress(bool isInProgress, int[] curGoalArguments)
    {
        _isInProgress = isInProgress;
        _curGoalArguments = curGoalArguments;
    }

    // �� óġ, ������ ȹ��� ���� ���ڰ� 1���� �����ϴ� Event�� �Ҵ�
    public void OnAddEvented(int i)
    {
        _curGoalArguments[0] += i;
        if (_curGoalArguments[0] < _goalArguments[0])
            return;

        _curGoalArguments[0] = _goalArguments[0]; // UI���� ǥ���� ��, ������ �Ѿ�� �ʵ��� ��.
        EndQuest();
    }

    // ��ǥ ����ó�� ��� ���ڰ� ���ƾ� �ϴ� Event�� �Ҵ�
    public void OnRenewEvented(int[] arguments)
    {
        for (int i = 0; i < arguments.Length; i++)
        {
            if (_goalArguments[i] != arguments[i])
                return;
        }

        _curGoalArguments = arguments;
        EndQuest();
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
}
