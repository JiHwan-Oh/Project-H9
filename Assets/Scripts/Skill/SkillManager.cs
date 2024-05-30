using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ���ӿ��� ���Ǵ� ��ų�� ȹ��, ��� ���� ����� �����ϴ� Ŭ����
/// </summary>
public class SkillManager : Generic.Singleton<SkillManager>
{
#if UNITY_EDITOR
    private const int REQUIRED_SKILL_POINT = 0;
    private const int INITIAL_SKILL_POINT = 10;
#else
    private const int REQUIRED_SKILL_POINT = 1;
    private const int INITIAL_SKILL_POINT = 0;
#endif
    public PassiveDatabase passiveDB;
    public ActiveDatabase activeDB;

    private ScriptLanguage _language = ScriptLanguage.Korean;
    private List<Skill> _skills;
    private List<SkillNameScript> _skillNameScripts;
    private List<SkillDescriptionScript> _skillDescriptionScripts;
    private List<KeywordScript> _skillKeywordScripts;

    private int _sp = 0;
    private int _skillPoint 
    {
        get 
        {
            return _sp;
        } 
        set 
        {
            _sp = value;
            UIManager.instance.gameSystemUI.ChangeSkillButtonRedDotText(value);
        }
    }

    private new void Awake()
    {
        base.Awake();

        InitSkills();
        InitSkillScripts();
        _skillPoint = INITIAL_SKILL_POINT;
    }

    private void InitSkills()
    {
        List<List<string>> skillTable = FileRead.Read("SkillTable", out var columnInfo);
        if (skillTable == null) 
        {
            Debug.Log("skill table�� �о���� ���߽��ϴ�.");
            return;
        }

        List<SkillInfo> _skillInformations = new List<SkillInfo>();
        for (int i = 0; i < skillTable.Count; i++)
        {
            SkillInfo skillInfo = new SkillInfo(skillTable[i]);
            _skillInformations.Add(skillInfo);
        }

        _skills = new List<Skill>();
        for (int i = 0; i < _skillInformations.Count; i++)
        {
            Skill skill = new Skill(_skillInformations[i]);
            _skills.Add(skill);
        }
        foreach (SkillInfo info in _skillInformations)
        {
            info.ConnectPrecedenceSkill();
        }
    }
    private void InitSkillScripts()
    {
        List<List<string>> skillNameTable = FileRead.Read("SkillNameScriptTable", out var nameColumnInfo);
        List<List<string>> skillDescriptionTable = FileRead.Read("SkillTooltipScriptTable", out var descColumnInfo);
        List<List<string>> skillKeywordTable = FileRead.Read("KeywordTable", out var keywordColumnInfo);
        if (skillNameTable == null || skillDescriptionTable == null || skillKeywordTable == null)
        {
            Debug.Log("skill Script table�� �о���� ���߽��ϴ�.");
            return;
        }
        _language = UIManager.instance.scriptLanguage;
        _skillNameScripts = new List<SkillNameScript>();
        for (int i = 0; i < skillNameTable.Count; i++)
        {
            SkillNameScript script = new SkillNameScript(i, skillNameTable[i][(int)_language]);
            _skillNameScripts.Add(script);
        }

        _skillDescriptionScripts = new List<SkillDescriptionScript>();
        for (int i = 0; i < skillDescriptionTable.Count; i++)
        {
            SkillDescriptionScript script = new SkillDescriptionScript(i, skillDescriptionTable[i][(int)_language]);
            _skillDescriptionScripts.Add(script);
        }
        _skillKeywordScripts = new List<KeywordScript>();
        for (int i = 0; i < skillKeywordTable.Count; i++)
        {
            KeywordScript script = new KeywordScript(int.Parse(skillKeywordTable[i][0]), skillKeywordTable[i][(int)_language], skillKeywordTable[i][(int)ScriptLanguage.English], skillKeywordTable[i][(int)_language + 3], skillKeywordTable[i][3] == "1");
            _skillKeywordScripts.Add(script);
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
    /// �ش� ������ȣ �Ǵ� �̸��� ���� ��ų�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="index"> ��ȯ�ϰ��� �ϴ� ��ų�� ������ȣ �Ǵ� �̸� </param>
    /// <returns> 
    /// �ش� ������ȣ�� �̸��� ���� ��ų�� �����Ѵٸ� �ش� ��ų�� ��ȯ�մϴ�.
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
    /// �ش� ������ȣ �Ǵ� �̸��� ���� ��ų�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="index"> ��ȯ�ϰ��� �ϴ� ��ų�� ������ȣ �Ǵ� �̸� </param>
    /// <returns> 
    /// �ش� ������ȣ�� �̸��� ���� ��ų�� �����Ѵٸ� �ش� ��ų�� ��ȯ�մϴ�.
    /// �ƴ϶�� null�� ��ȯ�մϴ�.
    /// </returns>
    public Skill GetSkill(string name)
    {
        int idx = -1;
        for (int i = 0; i < _skillNameScripts.Count; i++) 
        {
            if (_skillNameScripts[i].name == name) idx = i;
        }
        if (idx == -1) 
        {
            Debug.Log("�ش� �̸��� ��ų�� ã�� ���߽��ϴ�. �̸�: " + name);
            return null;
        }
        for (int i = 0; i < _skills.Count; i++)
        {
            if (_skills[i].skillInfo.nameIndex == idx)
            {
                return _skills[i];
            }
        }
        return null;
    }

    /// <summary>
    /// ��ų�� �����մϴ�.
    /// </summary>
    /// <param name="index"> �����ϰ��� �ϴ� ��ų�� ������ȣ </param>
    /// <param name="isFreeLearn"></param>
    /// <returns>
    /// ��ų ���������� �������� �ʾҰų�, ��ų ����Ʈ�� ������ ��� ��ų�� ������� �ʰ� false�� ��ȯ�մϴ�.
    /// ���������� ��ų�� ������ ��� true�� ��ȯ�մϴ�.
    /// </returns>
    public bool LearnSkill(int index, bool isFreeLearn = false) 
    {
        Player player = FieldSystem.unitSystem.GetPlayer();
        if (player == null) return false;

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
                if (!IsEnoughSkillPoint() && !isFreeLearn) 
                {
                    Debug.Log("��ų ����Ʈ�� �����մϴ�.");
                    return false;
                }

                _skillPoint -= isFreeLearn ? 0 : REQUIRED_SKILL_POINT;
                _skills[i].LearnSkill();
                PlayerEvents.OnLearnedSkill?.Invoke(_skills[i].skillInfo);
                GameManager.instance.AddPlayerSkillListElement(_skills[i].skillInfo);
                passiveDB.GetPassive(_skills[i].skillInfo.index, player);
                player.SetPassive(GameManager.instance.playerPassiveIndexList.Select(idx => passiveDB.GetPassive(idx, player)).ToList());
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
    public void AddSkillPoint(int sp) 
    {
        _skillPoint += sp;
        PlayerEvents.OnIncSkillPoint?.Invoke();
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
        if (skill == null || _skillNameScripts.Count <= skill.skillInfo.nameIndex) return "";
        return _skillNameScripts[skill.skillInfo.nameIndex].name;
    }
    public string GetSkillDescription(int skillIndex, out List<int> keywords)
    {
        Skill skill = GetSkill(skillIndex);
        if (skill == null || _skillDescriptionScripts.Count <= skill.skillInfo.tooltipIndex)
        {
            keywords = null;
            return null;
        }
        return _skillDescriptionScripts[skill.skillInfo.tooltipIndex].GetDescription(skillIndex, out keywords);
    }

    //delete and replace (search by name) later
    public KeywordScript GetSkillKeyword(int keywordIndex)
    {
        foreach (KeywordScript script in _skillKeywordScripts) 
        {
            if (script.index == keywordIndex) 
            {
                return script;
            }
        }
        return null;
    }
    public KeywordScript GetSkillKeyword(string keywordName)
    {
        foreach (KeywordScript script in _skillKeywordScripts)
        {
            if (script.Ename == keywordName)
            {
                return script;
            }
        }
        return null;
    }
    public KeywordScript FindSkillKeywordByName(string name)
    {
        foreach (KeywordScript script in _skillKeywordScripts)
        {
            if (script.Ename == name)
            {
                return script;
            }
        }
        Debug.Log("Ű���� Ž�� ����");
        return null;
    }
    public int FindUpgradeSkillIndex(SkillInfo sInfo, ActionType type)
    {
        if (sInfo.precedenceSkillInfo.Count == 0)
        {
            return -1;
        }
        else
        {
            foreach (SkillInfo pre in sInfo.precedenceSkillInfo)
            {
                int result = rFindSameActionTypeSkill(pre, type);
                if (result != -1)
                {
                    return result;
                }
            }
            return -1;
        }
    }
    private int rFindSameActionTypeSkill(SkillInfo sInfo, ActionType type)
    {
        if (sInfo.IsActive()) 
        {
            ActiveInfo aInfo = activeDB.GetActiveInfo(sInfo.index);
            if (aInfo.action == type) 
            {
                return sInfo.index;
            }
        }

        return FindUpgradeSkillIndex(sInfo, type);
    }
}
