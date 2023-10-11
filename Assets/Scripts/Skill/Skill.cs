using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Skill�� �Ӽ��� �����ϰ� �ʱ�ȭ�ϴ� Ŭ����
/// </summary>
public class SkillInfo
{
    enum SkillCategory
    {
        Null,
        Character,
        Revoler,
        Repeater,
        Shotgun
    }
    enum SkillActiveOrPassive
    {
        Null,
        PassiveType,
        ActiveType,
    }
    enum Stat
    {
        Null,
        HP,
        Concentration,
        Speed,
        FinalHitRate,
        Sight_Distance,
        Final_Mobility,
        FinalDamagePercent,
        FinalDamageInteger,
        HPRecovery,
        ActiveSkillDamageInteger,
        SkillDistance,
        SkillUseCount,
        UpHitRate,
        UpDistance,
        SkillDamageInteger
    }
    enum SkillActiveType
    {
        Null,
        Damage,
        Explode,
        Heal
    }

    public int index { get; private set; }
    public int section { get; private set; }
    public int isPassive { get; private set; }
    public int nameIndex { get; private set; }
    public int tooltipIndex { get; private set; }
    public int iconIndex { get; private set; }
    public int[] precedenceIndex { get; private set; }

    //public int repeatCount { get; private set; }
    //public int[] stat { get; private set; }
    //public int activeType { get; private set; }
    //public int upgradeSkill { get; private set; }
    //public int[] amount { get; private set; }
    //public int range { get; private set; }
    //public int width { get; private set; }

    /// <summary>
    /// SkillTable���� �� ���� �Է¹޾Ƽ� �������� �ʱ�ȭ�մϴ�.
    /// </summary>
    /// <param name="list"> SkillTable���� ������ �� ���� ���ڿ� </param>
    public SkillInfo(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].Equals("") || list[i].Equals("null"))
            {
                list[i] = "0";
            }
        }

        index = int.Parse(list[0]);
        section = int.Parse(list[1]);
        isPassive = int.Parse(list[2]);
        nameIndex = int.Parse(list[3]);
        tooltipIndex = int.Parse(list[4]);
        iconIndex = int.Parse(list[5]);
        precedenceIndex = InitIntArrayValue(list[6]);
    }
    private int[] InitIntArrayValue(string str)
    {
        const char SPLIT_CHAR = '$';
        if (str.Equals("0")) return new int[] { 0 };

        string[] splitString = str.Split(SPLIT_CHAR);

        int[] result = new int[splitString.Length];

        for (int i = 0; i < splitString.Length; i++)
        {
            result[i] = int.Parse(splitString[i]);
        }

        return result;
    }

    public bool IsActive()
    {
        Debug.Log("isPassiveTest");
        Debug.Log((int)SkillActiveOrPassive.ActiveType);
        if (isPassive == (int)SkillActiveOrPassive.ActiveType)
        {
            return true;
        }
        return false;
    }
    public bool IsPassive() 
    {
        return !IsActive();
    }
}

/// <summary>
/// ��ų�� �Ӽ��� ���� ���� �� ����� �����ϴ� Ŭ����
/// </summary>
public class Skill
{
    public SkillInfo skillInfo { get; private set; }

    public bool isLearned { get; private set; }
    public bool isLearnable { get; private set; }
    public int skillLevel { get; private set; }
    public bool[] isLearnedPrecedeSkill { get; private set; }

    public Skill(SkillInfo info)
    {
        skillInfo = info;

        isLearned = false;
        InitIsLearnable();
        skillLevel = 0;
        InitIsLearnedPrecedeSkill();
    }

    /// <summary>
    /// isLearnable ������ �ʱ�ȭ�մϴ�.
    /// ��ų�� ���� ��ų�� �������� ���� ��� true, ������ ��� false�� �ʱ�ȭ�մϴ�.
    /// </summary>
    private void InitIsLearnable()
    {
        isLearnable = (skillInfo.precedenceIndex[0] == 0);
    }
    private void InitIsLearnedPrecedeSkill()
    {
        isLearnedPrecedeSkill = new bool[skillInfo.precedenceIndex.Length];
        for (int i = 0; i < isLearnedPrecedeSkill.Length; i++)
        {
            isLearnedPrecedeSkill[i] = false;
        }
        if (skillInfo.precedenceIndex[0] == 0)
        {
            isLearnedPrecedeSkill[0] = true;
        }
    }
    /// <summary>
    /// isLearnable ������ ���¸� �����մϴ�.
    /// ��ų�� �ִ�ġ���� ��� ���¶�� false�� �����մϴ�.
    /// �׷��� �ʰ�, ���� ��ų���� ��� ��� ���¶�� true, �ƴ϶�� false�� �����մϴ�.
    /// </summary>
    /// <param name="skills"> �÷��̾��� ��ų ����Ʈ </param>
    public void UpdateIsLearnable(List<Skill> skills)
    {
        //if (skillLevel >= skillInfo.repeatCount)
        //{
        //    isLearnable = false;
        //    return;
        //}

        CheckPrecedenceSkill(skills);
        isLearnable = IsLearnedAllPrecedenceSkills();
    }
    private void CheckPrecedenceSkill(List<Skill> skills)
    {
        for (int i = 0; i < skills.Count; i++)
        {
            for (int j = 0; j < skillInfo.precedenceIndex.Length; j++)
            {
                bool isPrecedenceSkill = (skills[i].skillInfo.index == skillInfo.precedenceIndex[j]);
                if (isPrecedenceSkill)
                {
                    isLearnedPrecedeSkill[j] = skills[i].isLearned;
                }
            }
        }
    }
    private bool IsLearnedAllPrecedenceSkills()
    {
        for (int i = 0; i < isLearnedPrecedeSkill.Length; i++)
        {
            if (!isLearnedPrecedeSkill[i]) return false;
        }
        return true;
    }

    /// <summary>
    /// �ش� ��ų�� ���ϴ�.
    /// </summary>
    public void LearnSkill()
    {
        //if (skillLevel >= skillInfo.repeatCount) 
        //{
        //    Debug.LogError("��ų ���� �������� ���");
        //    return; 
        //}

        skillLevel++;
        isLearned = true;
        //if (skillLevel >= skillInfo.repeatCount)
        //{
        //    isLearnable = false;
        //}
        isLearnable = false;
    }
}

