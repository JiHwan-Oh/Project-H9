using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScriptLanguage 
{
    NULL,
    Korean,
    English
}
public class SkillScript
{
    public int index { get; private set; }
    public string name { get; private set; }
    private string _description;
    private bool _isSubstituted;
    public SkillScript(int idx, string str, string dsc) 
    {
        index = idx;
        name = str;
        _description = dsc;
        _isSubstituted = false;
    }
    public string GetDescription(int skiilIndex) 
    {
        if (!_isSubstituted)
        {
            _isSubstituted = SubstituteDescriptionValues(skiilIndex);
        }
        if (!_isSubstituted) 
        {
            Debug.LogError("���� ���� ���Կ� �����߽��ϴ�.");
        }
        return _description;
    }
    private bool SubstituteDescriptionValues(int skillIndex) 
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
        _description = result;
        return true;
    }
}
