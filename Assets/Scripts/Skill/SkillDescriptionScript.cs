using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkillDescriptionScript
{
    public int index { get; private set; }
    private string _description;
    private bool _isSubstituted;
    public SkillDescriptionScript(int idx, string dsc)
    {
        index = idx;
        _description = dsc;
        _isSubstituted = false;
    }
    public string GetDescription(int skiilIndex)
    {
        if (!_isSubstituted)
        {
        }
        if (!_isSubstituted)
        {
            Debug.LogError("���� ���� ���Կ� �����߽��ϴ�.");
        }
        return SubstituteDescriptionValues(skiilIndex);
    }
    private string SubstituteDescriptionValues(int skillIndex)
    {
        string result = "";
        char[] splitChar = { '{', '}' };
        string[] splitString = _description.Split(splitChar);
        bool isSubstitutableValue = false;
        foreach (string str in splitString)
        {
            if (!isSubstitutableValue)
            {
                result += str;
            }
            else
            {
                Debug.Log(skillIndex + " / " + SkillManager.instance.GetSkill(skillIndex).skillInfo.isPassive);
                if (SkillManager.instance.GetSkill(skillIndex).skillInfo.IsPassive())
                {
                    PassiveInfo info = SkillManager.instance.passiveDB.GetPassiveInfo(skillIndex);
                    result += info.effectAmount.ToString();
                    //���� effectAmount�� �迭 ������ �ƴ� 1���� �������� ���ϰ� �ִµ�,
                    //��ȹ ���� ���� ������ ���� ������ ����� ���� �ִٰ� �ؼ� ���� ������ ����Ǹ� �� �κе� �����ؾ� ��.
                }
                else
                {
                    ActiveInfo info = SkillManager.instance.activeDB.GetActiveInfo(skillIndex);
                    result += info.amounts[0].ToString();
                }
                Debug.Log(result);
            }
            isSubstitutableValue = !isSubstitutableValue;
        }
        return result;
    }
}
