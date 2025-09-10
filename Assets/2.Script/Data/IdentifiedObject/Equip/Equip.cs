using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using Shared.Enums;
using UnityEngine;

public class Equip : IdentifiedObject
{
    public EquipType Type => _type;
    public bool IsSpecial => _isSpecial;
    public BBNumber GetStatValue(Stat stat, int level)
    {
        BBNumber defaultStatValue = _defaultStats.Find(data => data.stat == stat).value;
        return GetStatValue(defaultStatValue, level);
    }
    public BBNumber GetStatValue(string code, int level)
    {
        BBNumber defaultStatValue = _defaultStats.Find(data => data.stat.CodeName == code).value;
        return GetStatValue(defaultStatValue, level);
    }
    private BBNumber GetStatValue(BBNumber defaultStatValue, int level)
    {
        return defaultStatValue + (1 + level * 0.01f);
    }


    [SerializeField] private EquipType _type;
    [SerializeField] private bool _isSpecial;
    [SerializeField] private List<StatValue> _defaultStats;
    [SerializeField] private List<EquipSkill> _skills;
}
[System.Serializable]
public class EquipSkill
{
    public EquipGrade Grade;
    public Skill Skill;
}