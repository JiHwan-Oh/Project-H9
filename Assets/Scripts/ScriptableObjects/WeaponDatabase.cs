using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDatabase", menuName = "ScriptableObjects/WeaponDB", order = 1)]
public class WeaponDatabase : ScriptableObject
{
    public List<WeaponData> weaponList;
    public List<string> weaponNameTable;
    public List<string> weaponScriptTable;
    public WeaponData GetData(int index)
    {
        foreach (var data in weaponList)
        {
            if (data.index == index) return data;
        }
        
        Debug.LogError("there are " + weaponList.Count + " weapons, there is no " + index + " index");
        return null;
    }
    
    public Weapon Clone(int dataIndex)
    {
        var data = GetData(dataIndex);
        Weapon weapon = data.type switch
        {
            WeaponType.Null => null,
            WeaponType.Character => new Melee(),
            WeaponType.Revolver => new Revolver(),
            WeaponType.Repeater => new Repeater(),
            WeaponType.Shotgun => new Shotgun(),
            _ => throw new ArgumentOutOfRangeException()
        };
        //
        // weapon.unit = owner;
        // // ReSharper disable once MergeConditionalExpression
        // weapon.unitStat = weapon.unit is null ? new UnitStat() : weapon.unit.GetStat();

        weapon.nameIndex = data.weaponNameIndex;
        weapon.model = data.weaponModel;
        if (data.weaponModel == null)
        {
            Debug.LogError("Weapon Model Is NULL");
        }
        if (weapon.model == null)
        {
            Debug.LogError("Weapon Model Is NULL");
        }
        weapon.weaponDamage = data.weaponDamage;
        weapon.weaponRange = data.weaponRange;
        weapon.maxAmmo = data.weaponAmmo;
        weapon.currentAmmo = data.weaponAmmo;
        weapon.hitRate = data.weaponHitRate;
        weapon.criticalChance = data.weaponCriticalChance;
        weapon.criticalDamage = data.weaponCriticalDamage;
        weapon.script = data.weaponScript;
        
        //SetUpGimmicks
        
        return weapon;
    }

    [ContextMenu("Load Csv")]
    public void LoadCsv()
    {
        var dataList = FileRead.Read("WeaponTable");
        
        if (weaponList is null) weaponList = new List<WeaponData>();
        else weaponList.Clear();

        for (var i = 0; i < dataList.Count; i++)
        {
            var curData = new WeaponData
            {
                index = int.Parse(dataList[i][0]),
                weaponNameIndex = int.Parse(dataList[i][1]),
                type = (WeaponType)int.Parse(dataList[i][2]),
                weaponRange = int.Parse(dataList[i][3]),
                weaponDamage = int.Parse(dataList[i][4]),
                weaponAmmo = int.Parse(dataList[i][5]),
                weaponHitRate = int.Parse(dataList[i][6]),
                weaponCriticalChance = int.Parse(dataList[i][7]),
                weaponCriticalDamage = int.Parse(dataList[i][8]),
                //weaponPrice;
                //weaponSkill;
                weaponScript = int.Parse(dataList[i][11])
            };
            
            weaponList.Add(curData);
        }
    }
}


[Serializable]
public class WeaponData
{
    public int index;
    public int weaponNameIndex;
    public GameObject weaponModel;
    public WeaponType type;
    public int weaponDamage;
    public int weaponRange;
    public int weaponAmmo;
    public int weaponHitRate;
    public int weaponCriticalChance;
    public int weaponCriticalDamage;

    public int weaponScript;
}
