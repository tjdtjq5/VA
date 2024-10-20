using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class Stats : MonoBehaviour
{
    [SerializeField]
    private Stat hpStat;
    [SerializeField]
    private Stat skillCostStat;

    [Space]
    [SerializeField]
    private StatOverride[] statOverrides;

    private Stat[] stats;
    private Dictionary<string, Stat> statsDicsKeyCode = new();
    private Dictionary<int, Stat> statsDicsId = new();

    public Entity Owner { get; private set; }
    public Stat HPStat { get; private set; }
    public Stat SkillCostStat { get; private set; }

    public void Setup(Entity entity)
    {
        Owner = entity;

        stats = statOverrides.Select(x => x.CreateStat()).ToArray();

        statsDicsKeyCode.Clear();
        statsDicsId.Clear();
        for (int i = 0; i < stats.Length; i++)
        {
            statsDicsKeyCode.Add(stats[i].CodeName, stats[i]);
            statsDicsId.Add(stats[i].ID, stats[i]);
        }

        HPStat = hpStat ? GetStat(hpStat) : null;
        SkillCostStat = skillCostStat ? GetStat(skillCostStat) : null;
    }
    
    private void OnDestroy()
    {
        foreach (var stat in stats)
            Destroy(stat);

        statsDicsKeyCode.Clear();
        statsDicsId.Clear();

        stats = null;
    }

    public Stat GetStat(Stat stat)
    {
        if (statsDicsId.ContainsKey(stat.ID))
            return statsDicsId[stat.ID];
        else
            return null;
    }
    public Stat GetStat(string code)
    {
        if (statsDicsKeyCode.ContainsKey(code))
            return statsDicsKeyCode[code];
        else
            return null;
    }

    public bool TryGetStat(Stat stat, out Stat outStat)
    {
        UnityHelper.Assert_H(stat != null, $"Stats::TryGetStat - stat은 null이 될 수 없습니다.");

        outStat = stats.FirstOrDefault(x => x.ID == stat.ID);
        return outStat != null;
    }

    public BBNumber GetValue(Stat stat)
        => GetStat(stat).Value;
    public BBNumber GetValue(string code)
    => GetStat(code).Value;

    public bool HasStat(Stat stat)
    {
        UnityHelper.Assert_H(stat != null, $"Stats::HasStat - stat은 null이 될 수 없습니다.");
        return stats.Any(x => x.ID == stat.ID);
    }

    public void SetDefaultValue(Stat stat, BBNumber value)
        => GetStat(stat).DefaultValue = value;

    public BBNumber GetDefaultValue(Stat stat)
        => GetStat(stat).DefaultValue;

    public void IncreaseDefaultValue(Stat stat, BBNumber value)
        => SetDefaultValue(stat, GetDefaultValue(stat) + value);

    public void SetBonusValue(Stat stat, object key, BBNumber value)
        => GetStat(stat).SetBonusValue(key, value);
    public void SetBonusValue(Stat stat, object key, object subKey, BBNumber value)
        => GetStat(stat).SetBonusValue(key, subKey, value);
    public void SetBonusValue(Stat bonusStat)
    {
        GetStat(bonusStat).SetBonusValue(bonusStat.BonusValuesByKey);
    } 

    public BBNumber GetBonusValue(Stat stat)
        => GetStat(stat).BonusValue;
    public BBNumber GetBonusValue(Stat stat, object key)
        => GetStat(stat).GetBonusValue(key);
    public BBNumber GetBonusValue(Stat stat, object key, object subKey)
        => GetStat(stat).GetBonusValue(key, subKey);
    
    public void RemoveBonusValue(Stat stat, object key)
        => GetStat(stat).RemoveBonusValue(key);
    public void RemoveBonusValue(Stat stat, object key, object subKey)
        => GetStat(stat).RemoveBonusValue(key, subKey);

    public bool ContainsBonusValue(Stat stat, object key)
        => GetStat(stat).ContainsBonusValue(key);
    public bool ContainsBonusValue(Stat stat, object key, object subKey)
        => GetStat(stat).ContainsBonusValue(key, subKey);

#if UNITY_EDITOR
    [Button]
    public void LoadStats()
    {
        var stats = Resources.LoadAll<Stat>("Stat").OrderBy(x => x.ID);
        statOverrides = stats.Select(x => new StatOverride(x)).ToArray();
    }
#endif
}
