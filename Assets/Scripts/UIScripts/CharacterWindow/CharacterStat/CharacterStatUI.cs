using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UIStatType
{
    Character,
    SkillAdd,
    SkillMulti,
    Weapon,
    PlusSign,
    MultiSign
}
public class CharacterStatUIInfo
{
    private Dictionary<string, string> _statNameTransleation = new Dictionary<string, string>()
    {
            {"Level",                   "����" },
            {"Exp",                     "����ġ" },
            {"HP",                      "ü��" },
            {"Concentration",           "���߷�" },
            {"Sight Range",             "�þ� ����" },
            {"Speed",                   "�ӵ�" },
            {"Action Point",            "�ൿ ����Ʈ" },
            {"Additional Hit Rate",     "�߰� ���߷�" },
            {"Critical Chance",         "ġ��Ÿ Ȯ��" },
            {"Additional Damage",       "�߰� ������" },
            {"Additional Range",        "�߰� ��Ÿ�" },
            {"Critical Damage",         "ġ��Ÿ ������" },
            {"Name",                    "���� �̸�" },
            {"Ammo",                    "���� źâ �뷮" },
            {"Damage",                  "���� ������" },
            {"Range",                   "���� ��Ÿ�" },
            {"",                        "" },
    };
    //�̰� �̷� ������ ���� �̷��� �� �³�? ���� ��ũ��Ʈ ���̺��� �ʿ��� ��?

    public string statName { get; private set; }
    public Dictionary<UIStatType, float> statValues { get; private set; }

    public CharacterStatUIInfo(string name) 
    {
        statName = name;
        statValues = new Dictionary<UIStatType, float>() 
        {
            {UIStatType.Character,   0.0f},
            {UIStatType.SkillAdd,    0.0f},
            {UIStatType.SkillMulti,  0.0f},
            {UIStatType.Weapon,      0.0f}
        };
    }

    public void SetStatValue(UIStatType statType, float value) 
    {
        if (!statValues.ContainsKey(statType)) 
        {
            Debug.LogError("�߸��� Ű ��� �Է�");
            return;
        }
        statValues[statType] = value;
    }
    public string GetTranslateStatName() 
    {
        return _statNameTransleation[statName];
    }
    public float GetFinalStatValue() 
    {
        float result = 0;
        foreach (float x in statValues.Values) 
        {
            result += x;
        }
        return result;
    }
    public float GetCorrectedValue(float stat)
    {
        //if (statName == "Critical Chance") return ((int)(stat * 100));
        return stat;
    }
    public string GetFinalStatValueString() 
    {
        string finalStat = GetCorrectedValue(GetFinalStatValue()).ToString();
        if (statName == "") return "";
        if (statName == "Exp") return finalStat + " / " + GameManager.instance.GetMaxExp();
        if (statName == "Additional Hit Rate") return finalStat + "%";
        if (statName == "Critical Chance") return finalStat + '%';

        return finalStat.ToString();
    }
}

/// <summary>
/// ĳ���� ���� â���� ĳ������ ���� �� ����ϰ� �ִ� ������ ���� ������ ǥ���ϴ� ����� ������ Ŭ����
/// </summary>
public class CharacterStatUI : UISystem
{
    //Character Stat
    [Header("Character Stat UI")]
    [SerializeField] private GameObject _characterLevelTexts;
    [SerializeField] private GameObject _characterStatTexts;
    [SerializeField] private GameObject _weaponStatTexts;
    static int _textIndex;

    public GameObject _characterStatTooltip;
    private readonly List<string> _stats = new List<string>()
    {
        "Level",
        "Exp",
        "HP",
        "Concentration",
        "Sight Range",
        "Speed",
        "Action Point",
        "",
        "Additional Hit Rate",
        "Critical Chance",
        "Additional Damage",
        "Additional Range",
        "Critical Damage",
        "Name",
        "Ammo",
        "Damage",
        "Range"
    };
    public Dictionary<string, CharacterStatUIInfo> characterStatInfo { get; private set; }

    private void Start()
    {
        characterStatInfo = new Dictionary<string, CharacterStatUIInfo>();
        foreach (string str in _stats)
        {
            characterStatInfo.Add(str, new CharacterStatUIInfo(str));
        }
        UIManager.instance.onPlayerStatChanged.AddListener(() => SetStatText());
    }

    public override void OpenUI()
    {
        base.OpenUI();

        SetStatText();
    }
    public override void CloseUI()
    {
        base.CloseUI();
    }
    private void SetStatText()
    {
        Player player = FieldSystem.unitSystem.GetPlayer();

        SetStatInfo(player);
        _textIndex = 0;
        SetCharacterLevelText();
        SetCharacterStatText();
        SetWeaponStatText();
    }
    private void SetStatInfo(Player player) 
    {
        UnitStat stat = player.stat;
        Weapon weapon = player.weapon;
        //player
        //level
        characterStatInfo["Level"].SetStatValue(UIStatType.Character, GameManager.instance.level);
        characterStatInfo["Exp"].SetStatValue(UIStatType.Character, GameManager.instance.curExp);

        //basic stat
        characterStatInfo["HP"].SetStatValue(UIStatType.Character, stat.GetStat(StatType.MaxHp));
        //characterStatInfo["HP"].SetStatValue("WeaponStat", weapon.bonusStat.maxHp); //����
        characterStatInfo["Concentration"].SetStatValue(UIStatType.Character, stat.concentration);
        characterStatInfo["Sight Range"].SetStatValue(UIStatType.Character, stat.sightRange);
        characterStatInfo["Speed"].SetStatValue(UIStatType.Character, stat.speed);
        characterStatInfo["Action Point"].SetStatValue(UIStatType.Character, stat.maxActionPoint);

        //bonus stat
        characterStatInfo["Additional Hit Rate"].SetStatValue(UIStatType.Character, stat.additionalHitRate);
        characterStatInfo["Additional Hit Rate"].SetStatValue(UIStatType.Weapon, weapon.hitRate);
        characterStatInfo["Critical Chance"].SetStatValue(UIStatType.Character, stat.criticalChance);
        characterStatInfo["Critical Chance"].SetStatValue(UIStatType.Weapon, weapon.criticalChance);

        WeaponType weaponType = weapon.GetWeaponType();
        if (weaponType is WeaponType.Revolver)
        {
            characterStatInfo["Additional Damage"].SetStatValue(UIStatType.Character, stat.revolverAdditionalDamage);
            characterStatInfo["Additional Range"].SetStatValue(UIStatType.Character, stat.revolverAdditionalRange);
            characterStatInfo["Critical Damage"].SetStatValue(UIStatType.Character, stat.revolverCriticalDamage);
        }
        else if (weaponType is WeaponType.Repeater)
        {
            characterStatInfo["Additional Damage"].SetStatValue(UIStatType.Character, stat.repeaterAdditionalDamage);
            characterStatInfo["Additional Range"].SetStatValue(UIStatType.Character, stat.repeaterAdditionalRange);
            characterStatInfo["Critical Damage"].SetStatValue(UIStatType.Character, stat.repeaterCriticalDamage);
        }
        else if (weaponType is WeaponType.Shotgun)
        {
            characterStatInfo["Additional Damage"].SetStatValue(UIStatType.Character, stat.shotgunAdditionalDamage);
            characterStatInfo["Additional Range"].SetStatValue(UIStatType.Character, stat.shotgunAdditionalRange);
            characterStatInfo["Critical Damage"].SetStatValue(UIStatType.Character, stat.shotgunCriticalDamage);
        }
        characterStatInfo["Critical Damage"].SetStatValue(UIStatType.Weapon, weapon.criticalDamage);

        //weapon
        characterStatInfo["Name"].SetStatValue(UIStatType.Weapon, weapon.nameIndex);
        characterStatInfo["Ammo"].SetStatValue(UIStatType.Weapon, weapon.maxAmmo);
        characterStatInfo["Damage"].SetStatValue(UIStatType.Weapon, weapon.weaponDamage);
        characterStatInfo["Range"].SetStatValue(UIStatType.Weapon, weapon.weaponRange);
    }
    private void SetCharacterLevelText()
    {
        for (int i = 0; i < _characterLevelTexts.transform.childCount; i++)
        {
            _characterLevelTexts.transform.GetChild(i)
                .GetComponent<CharacterStatTextElement>().SetCharacterStatText(characterStatInfo[_stats[_textIndex++]]);
        }
    }
    private void SetCharacterStatText()
    {
        for (int i = 0; i < _characterStatTexts.transform.childCount; i++) 
        {
            _characterStatTexts.transform.GetChild(i)
                .GetComponent<CharacterStatTextElement>().SetCharacterStatText(characterStatInfo[_stats[_textIndex++]]);
        }
    }
    private void SetWeaponStatText()
    {
        for (int i = 0; i < _weaponStatTexts.transform.childCount; i++)
        {
            _weaponStatTexts.transform.GetChild(i)
                .GetComponent<CharacterStatTextElement>().SetCharacterStatText(characterStatInfo[_stats[_textIndex++]]);
        }
    }

    public void OpenCharacterTooltip(CharacterStatTextElement textElement, string name, float yPosition) 
    {
        _characterStatTooltip.GetComponent<CharacterTooltip>().SetCharacterTooltip(textElement, characterStatInfo[name], yPosition);
    }
    public void CloseCharacterTooltip()
    {
        _characterStatTooltip.GetComponent<CharacterTooltip>().CloseUI();
    }
}


//HP:
//Concentration:
//Sight Range:
//Speed:
//Action Point:
//Additional Hit Rate:
//Critical Chance:

//Additional Damage:
//Additional Range:
//Critical Damage:

//Name:
//Ammo:
//Damage:
//Range:
//Additional Hit Rate:
//Critical Chance:
//Critical Damage: