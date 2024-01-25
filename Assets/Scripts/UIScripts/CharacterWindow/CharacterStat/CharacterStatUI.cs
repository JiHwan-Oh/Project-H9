using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UIStatType
{
    Character,
    Skill,
    Weapon,
    Sign
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
            {UIStatType.Skill,       0.0f},
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
        if (statName == "Critical Damage") return stat / 100.0f;
        return stat;
    }
    public string GetFinalStatValueString() 
    {
        string finalStat = GetCorrectedValue(GetFinalStatValue()).ToString();
        if (statName == "") return "";
        if (statName == "Exp") return finalStat + " / " + GameManager.instance.GetMaxExp();
        if (statName == "Additional Hit Rate") return finalStat + "%";
        if (statName == "Critical Chance") return finalStat + '%';
        if (statName == "Critical Damage") return finalStat + 'x';

        return finalStat.ToString();
    }
}

/// <summary>
/// ĳ���� ���� â���� ĳ������ ���� �� ����ϰ� �ִ� ������ ���� ������ ǥ���ϴ� ����� ������ Ŭ����
/// </summary>
public class CharacterStatUI : UISystem
{
    //Character Stat
    [SerializeField] private GameObject _characterStatUIElements;
    static int _textIndex;

    public GameObject _characterStatTooltip;
    private readonly List<string> _stats = new List<string>()
    {
        "Level",
        "Exp",
        "HP",
        "Sight Range",
        "Speed",
        "Action Point",
        "Concentration",
        "Additional Hit Rate",
        "Additional Damage",
        "Critical Chance",
        "Critical Damage",
        "",
        "Additional Range",
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
        _textIndex = 2;
        SetCharacterStatText();
    }
    private void SetStatInfo(Player player)
    {
        //player
        UnitStat stat = player.stat;
        Weapon weapon = player.weapon;
        WeaponType weaponType = weapon.GetWeaponType();
        if (stat is null) return;

        List<(string, StatType)> _strAndType = new List<(string, StatType)>()
        {
            ("HP",                  StatType.MaxHp),
            ("Sight Range",         StatType.SightRange),
            ("Speed",               StatType.Speed),
            ("Action Point",        StatType.MaxActionPoint),
            ("Concentration",       StatType.Concentration),
            ("Additional Hit Rate", StatType.AdditionalHitRate),
            ("Critical Chance",     StatType.CriticalChance)
        };
        if (weaponType is WeaponType.Revolver)
        {
            _strAndType.Add(("Additional Damage", StatType.RevolverAdditionalDamage));
            _strAndType.Add(("Critical Chance", StatType.CriticalChance));
            _strAndType.Add(("Critical Damage", StatType.RevolverCriticalDamage));
        }
        else if (weaponType is WeaponType.Repeater)
        {
            _strAndType.Add(("Additional Damage", StatType.RepeaterAdditionalDamage));
            _strAndType.Add(("Critical Chance", StatType.CriticalChance));
            _strAndType.Add(("Critical Damage", StatType.RepeaterCriticalDamage));
        }
        else if (weaponType is WeaponType.Shotgun)
        {
            _strAndType.Add(("Additional Damage", StatType.ShotgunAdditionalDamage));
            _strAndType.Add(("Critical Chance", StatType.CriticalChance));
            _strAndType.Add(("Critical Damage", StatType.ShotgunCriticalDamage));
        }

        //level
        characterStatInfo["Level"].SetStatValue(UIStatType.Character, GameManager.instance.level);
        characterStatInfo["Exp"].SetStatValue(UIStatType.Character, GameManager.instance.curExp);

        //basic stat
        for (int i = 0; i < _strAndType.Count; i++)
        {
            float originalStat = stat.GetOriginalStat(_strAndType[i].Item2);
            float buffedStat = stat.GetStat(_strAndType[i].Item2);
            //if (_strAndType[i].Item2 == StatType.CriticalChance) 
            //{
            //    Debug.Log(originalStat + " / " + buffedStat);
            //}
            characterStatInfo[_strAndType[i].Item1].SetStatValue(UIStatType.Character, originalStat);
            characterStatInfo[_strAndType[i].Item1].SetStatValue(UIStatType.Skill, buffedStat - originalStat);
        }

        //bonus stat
        characterStatInfo["Additional Hit Rate"].SetStatValue(UIStatType.Weapon, weapon.hitRate);
        characterStatInfo["Critical Chance"].SetStatValue(UIStatType.Weapon, weapon.criticalChance);
        characterStatInfo["Critical Damage"].SetStatValue(UIStatType.Weapon, weapon.criticalDamage);

        //weapon
        characterStatInfo["Name"].SetStatValue(UIStatType.Weapon, weapon.nameIndex);
        characterStatInfo["Ammo"].SetStatValue(UIStatType.Weapon, weapon.maxAmmo);
        characterStatInfo["Damage"].SetStatValue(UIStatType.Weapon, weapon.weaponDamage);
        characterStatInfo["Range"].SetStatValue(UIStatType.Weapon, weapon.weaponRange);
    }
    private void SetCharacterStatText()
    {
        for (int i = 0; i < _characterStatUIElements.transform.childCount; i++) 
        {
            _characterStatUIElements.transform.GetChild(i)
                .GetComponent<CharacterStatTextElement>().SetCharacterStatText(characterStatInfo[_stats[_textIndex++]]);
        }
    }

    public void OpenCharacterTooltip(CharacterStatTextElement textElement, string name, Vector3 pos) 
    {
        _characterStatTooltip.GetComponent<CharacterTooltip>().SetCharacterTooltip(textElement, characterStatInfo[name], pos);
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