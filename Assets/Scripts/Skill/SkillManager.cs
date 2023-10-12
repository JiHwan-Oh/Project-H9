using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScriptLanguage
{
    NULL,
    Korean,
    English
}

/// <summary>
/// ���ӿ��� ���Ǵ� ��ų�� ȹ��, ��� ���� ����� �����ϴ� Ŭ����
/// </summary>
public class SkillManager : Generic.Singleton<SkillManager>
{
    const int REQUIRED_SKILL_POINT = 1;
    public PassiveDatabase passiveDB;
    public ActiveDatabase activeDB;

    private List<Skill> _skills;
    private List<SkillNameScript> _skillNameScripts;
    private List<SkillDescriptionScript> _skillDescriptionScripts;

    private int _skillPoint;

    private new void Awake()
    {
        base.Awake();
        
        InitSkills();
        _skillPoint = 10;    //test
    }

    private void InitSkills()
    {
        List<List<string>> _skillTable = FileRead.Read("SkillTable");
        List<List<string>> _skillName = FileRead.Read("SkillNameScript");
        List<List<string>> _skilldescription = FileRead.Read("SkillTooltipScript");
        if (_skillTable == null || _skillName == null || _skilldescription == null) 
        {
            Debug.Log("skill table�� �о���� ���߽��ϴ�.");
            return;
        }
        if (_skillName.Count != _skilldescription.Count)
        {
            Debug.Log("Script Table�� ������ �����Ͱ� �ֽ��ϴ�.");
            return;
        }

        List<SkillInfo> _skillInformations = new List<SkillInfo>();
        for (int i = 0; i < _skillTable.Count; i++)
        {
            SkillInfo skillInfo = new SkillInfo(_skillTable[i]);
            _skillInformations.Add(skillInfo);
        }

        _skills = new List<Skill>();
        for (int i = 0; i < _skillInformations.Count; i++)
        {
            Skill skill = new Skill(_skillInformations[i]);
            _skills.Add(skill);
        }
        ScriptLanguage language = ScriptLanguage.Korean;

        _skillNameScripts = new List<SkillNameScript>();
        for (int i = 0; i < _skillName.Count; i++)
        {
            SkillNameScript script = new SkillNameScript(i, _skillName[i][(int)language]);
            _skillNameScripts.Add(script);
        }
        _skillDescriptionScripts = new List<SkillDescriptionScript>();
        for (int i = 0; i < _skillName.Count; i++)
        {
            SkillDescriptionScript script = new SkillDescriptionScript(i, _skilldescription[i][(int)language]);
            _skillDescriptionScripts.Add(script);
        }
    }

    /// <summary>
    /// �����ϴ� ��� ��ų���� ��ȯ�մϴ�.
    /// </summary>
    /// <returns> �����ϴ� ��� ��ų���� ���� Skill����Ʈ </returns>
    public List<Skill> GetAllSkills() 
    {
        return _skills;
    }
    /// <summary>
    /// �÷��̾ ������ ��ų�鸸 ��ȯ�մϴ�.
    /// </summary>
    /// <returns> �÷��̾ ������ ��ų���� ���� Skill����Ʈ </returns>
    public List<Skill> GetAllLearnedSkills()
    {
        List<Skill> learnedSkills = new List<Skill>();
        for (int i = 0; i < _skills.Count; i++)
        {
            if (!_skills[i].isLearned) continue;
            learnedSkills.Add(_skills[i]);
        }
        return learnedSkills;
    }
    /// <summary>
    /// �ش� ������ȣ�� ���� ��ų�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="index"> ��ȯ�ϰ��� �ϴ� ��ų�� ������ȣ </param>
    /// <returns> 
    /// �ش� ������ȣ�� ���� ��ų�� �����Ѵٸ� �ش� ��ų�� ��ȯ�մϴ�.
    /// �ƴ϶�� null�� ��ȯ�մϴ�.
    /// </returns>
    public Skill GetSkill(int index) 
    {
        for (int i = 0; i < _skills.Count; i++) 
        {
            if (_skills[i].skillInfo.index == index) 
            {
                return _skills[i];
            }
        }
        Debug.Log("�ش� �ε����� ��ų�� ã�� ���߽��ϴ�. �ε���: " + index);
        return null;
    }
    /// <summary>
    /// ��ų�� �����մϴ�.
    /// </summary>
    /// <param name="index"> �����ϰ��� �ϴ� ��ų�� ������ȣ </param>
    /// <returns>
    /// ��ų ���������� �������� �ʾҰų�, ��ų ����Ʈ�� ������ ��� ��ų�� ������� �ʰ� false�� ��ȯ�մϴ�.
    /// ���������� ��ų�� ������ ��� true�� ��ȯ�մϴ�.
    /// </returns>
    public bool LearnSkill(int index) 
    {
        List<Skill> learnedSkill = GetAllLearnedSkills();
        foreach (Skill skill in learnedSkill) 
        {
            if (skill.skillInfo.index == index) 
            {
                Debug.Log("���� ��ų ���� ����");
                return false;
            }
        }
        for (int i = 0; i < _skills.Count; i++) 
        {
            if (_skills[i].skillInfo.index == index) 
            {
                if (!_skills[i].isLearnable) 
                { 
                    Debug.Log("���� ������ �������� ���� ��ų�Դϴ�.");
                    return false; 
                }
                if (!IsEnoughSkillPoint()) 
                {
                    Debug.Log("��ų ����Ʈ�� �����մϴ�.");
                    return false;
                }

                _skillPoint -= REQUIRED_SKILL_POINT;
                _skills[i].LearnSkill();
                break;
            }
        }
        for (int i = 0; i < _skills.Count; i++)
        {
            _skills[i].UpdateIsLearnable(_skills);
        }

        return true;
    }
    /// <summary>
    /// ���� ������ ��ų ����Ʈ�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns> ���� ��ų ����Ʈ </returns>
    public int GetSkillPoint() 
    {
        return _skillPoint;
    }
    /// <summary>
    /// ���� ��ų ����Ʈ�� ��ų�� ��� ��ŭ ����� �� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>
    /// ���� ��ų ����Ʈ�� �ʿ��� ��ų ����Ʈ���� �۴ٸ� false�� ��ȯ�մϴ�.
    /// �ƴ϶�� true�� ��ȯ�մϴ�.
    /// </returns>
    public bool IsEnoughSkillPoint()
    {
        return REQUIRED_SKILL_POINT <= _skillPoint;
    }
    public string GetSkillName(int skillIndex) 
    {
        Skill skill = GetSkill(skillIndex);
        return _skillNameScripts[skill.skillInfo.nameIndex].name;
    }
    public string GetSkillDescription(int skillIndex)
    {
        Skill skill = GetSkill(skillIndex);
        return _skillDescriptionScripts[skill.skillInfo.tooltipIndex].GetDescription(skillIndex);
    }
}
