using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkillDescriptionScript
{
    public int index { get; private set; }
    public string description { get; private set; }
    public List<int> keywordIndex { get; private set; }

    private static string substitutedDescription ="";
    public SkillDescriptionScript(int idx, string str)
    {
        index = idx;
        description = str;
        keywordIndex = new List<int>();
    }
    public string GetDescription(int skiilIndex)
    {
        substitutedDescription = description;
        SubstituteKeyword();
        SubstituteDescriptionValues(skiilIndex);
        if (substitutedDescription[0] == '\"') 
        {
            substitutedDescription = substitutedDescription.Substring(1, substitutedDescription.Length - 2);

        }
        if (keywordIndex.Count != 0)
        {
            UIManager.instance.skillUI.SetKeywordTooltipContents(keywordIndex);
        }
        return substitutedDescription;
    }
    private void SubstituteDescriptionValues(int skillIndex)  //������ ���� ���� �� �տ� ���� ��� ���� ���ɼ� ����.
    {
        string result = "";
        char[] splitChar = { '{', '}' };
        string[] splitString = substitutedDescription.Split(splitChar);
        bool isSubstitutableValue = false;
        foreach (string str in splitString)
        {
            if (!isSubstitutableValue)
            {
                result += str;
            }
            else
            {
                string amountText;
                if (SkillManager.instance.GetSkill(skillIndex).skillInfo.IsPassive())
                {
                    //���� effectAmount�� �迭 ������ �ƴ� 1���� �������� ���ϰ� �ִµ�,
                    //��ȹ ���� ���� ������ ���� ������ ����� ���� �ִٰ� �ؼ� ���� ������ ����Ǹ� �� �κе� �����ؾ� ��.
                    PassiveInfo info = SkillManager.instance.passiveDB.GetPassiveInfo(skillIndex);
                    amountText = info.effectAmount.ToString();
                }
                else
                {
                    ActiveInfo info = SkillManager.instance.activeDB.GetActiveInfo(skillIndex);
                    amountText = info.amounts[int.Parse(str)].ToString();
                }
                string highlightColor = UICustomColor.GetColorHexCode(UICustomColor.PlayerStatColor);
                result += string.Format("<color=#{0}>{1}</color>", highlightColor, amountText);
            }
            isSubstitutableValue = !isSubstitutableValue;
        }
        substitutedDescription = result;
    }
    private void SubstituteKeyword()
    {
        string origin = substitutedDescription;
        string[] split = { "<keyword:", ">" };
        string result = "";
        keywordIndex.Clear();
        while (origin.Contains(split[0]))
        {
            string beforeString = GetSubString(origin, 0, origin.IndexOf(split[0]));
            string middleString = GetSubString(origin, origin.IndexOf(split[0]) + split[0].Length, origin.IndexOf(split[1]));
            string afterString = GetSubString(origin, origin.IndexOf(split[1]) + split[1].Length, origin.Length);
            result += beforeString;
            string highlightColor = UICustomColor.GetColorHexCode(UICustomColor.PlayerStatColor);
            keywordIndex.Add(int.Parse(middleString));
            string keyword = SkillManager.instance.GetSkillKeyword(int.Parse(middleString)).name;
            result += string.Format("<color=#{0}>{1}</color>", highlightColor, keyword);
            origin = afterString;
        }
        substitutedDescription = result + origin;
    }
    private string GetSubString(string origin, int startIndex, int endIndex) 
    {
        int length = endIndex - startIndex;
        return origin.Substring(startIndex, length);
    }
}
