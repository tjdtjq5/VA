using System.Collections.Generic;
using System.Linq;
using Shared.BBNumber;
using Shared.CSharp;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public List<Stat> Gets => _stats;
    
    private List<Stat> _stats = new();
    private readonly Dictionary<string, Stat> _statsDics = new ();
    
    #region StatCode

    private readonly string _hpStat = "HP";
    private readonly string _shieldStat = "Shield";
    private readonly string _atkStat = "Atk";
    private readonly string _finalDmgStat = "FinalDamage";
    private readonly string _defStat = "Def";
    private readonly string _sequenceStat = "Sequence";
    
    #endregion

    #region Frequently used Stat

    [HideInInspector] public Stat hpStat;
    [HideInInspector] public Stat shieldStat;
    [HideInInspector] public Stat atkStat;
    [HideInInspector] public Stat sequenceStat;

    #endregion
    
    #region Prevented Stat

    private Dictionary<string, BBNumber> _preventedStats = new();
    public BBNumber PreventedDamage { get; private set; }
    
    #endregion

    #region Damage Rduction

    public float DamageReduction { get; private set; }
    private readonly int _damageReductionConstant = 5000;

    #endregion
    
    #region Max Stat

    private Dictionary<string, BBNumber> _maxStats = new();

    public BBNumber MaxStatValue(Stat stat)
    {
        return MaxStatValue(stat.CodeName);
    }
    public BBNumber MaxStatValue(string code)
    {
        if (_maxStats.ContainsKey(code))
            return _maxStats[code];
        else
            return 0;
    }
    
    #endregion
  
    public void Initialize(List<Stat> stats)
    {
        _stats.Clear();
        _statsDics.Clear();
        for (int i = 0; i < stats.Count; i++)
        {
            Stat stat = (Stat)stats[i].Clone();
            _stats.Add(stat);
            _statsDics.Add(stat.CodeName, stat);
            stat.onValueChanged += OnValueChanged;
            stat.onBonusValueChanged += OnBonusValueChanged;
            stat.onDecreaseValueChanged += OnDecreasedValueChanged;
        }

        // Frequently used Stat
        this.hpStat = GetStat(_hpStat);
        this.shieldStat = GetStat(_shieldStat);
        this.atkStat = GetStat(_atkStat);
        this.sequenceStat = GetStat(_sequenceStat);

        Stat finalDmgStat = GetStat(_finalDmgStat);

        // Prevented Stat
        if (atkStat != null)
            _preventedStats.Add(_atkStat , atkStat.Value);
        if (finalDmgStat != null)
            _preventedStats.Add(_finalDmgStat , finalDmgStat.Value);
        
        // Max Stat
        if (hpStat != null)
            _maxStats.Add(_hpStat, hpStat.Value);

        SetPreventDamage();
    }
    public void Clear()
    {
        _stats.Clear();
        hpStat = null;
        shieldStat = null; 
        _preventedStats.Clear();
        _maxStats.Clear();
        PreventedDamage = 0;
    }

    void OnValueChanged(Stat stat, BBNumber currentValue, BBNumber prevValue)
    {
        if (_preventedStats.ContainsKey(stat.CodeName))
        {
            _preventedStats[stat.CodeName] = currentValue;
            SetPreventDamage();
        }

        if (stat.CodeName.Equals(_defStat))
        {
            DamageReduction = (currentValue / (currentValue + _damageReductionConstant)).ToFloat();
        }
    }

    void OnBonusValueChanged(Stat stat, BBNumber currentValue, BBNumber prevValue)
    {
        if (_maxStats.ContainsKey(stat.CodeName))
        {
            if (_maxStats[stat.CodeName] < currentValue)
            {
                _maxStats[stat.CodeName] = currentValue;
            }
        }
    }

    void OnDecreasedValueChanged(Stat stat, BBNumber currentValue, BBNumber prevValue)
    {
        if (_maxStats.ContainsKey(stat.CodeName))
        {
            if (_maxStats[stat.CodeName] < currentValue)
            {
                _maxStats[stat.CodeName] = currentValue;
            }
        }
    }

    void SetPreventDamage()
    {
        PreventedDamage = 0;
        foreach (var preventedStatsVariable in _preventedStats)
        {
            if (PreventedDamage == 0)
            {
                PreventedDamage = preventedStatsVariable.Value;
            }
            else
            {
                PreventedDamage *= preventedStatsVariable.Value / 100f + 1;
            }
        }
    }
    
    private void OnDestroy()
    {
        foreach (var stat in _stats)
            Destroy(stat);
        _stats = null;
    }
    
    public Stat GetStat(Stat stat)
    {
        return _stats.FirstOrDefault(x => x.CodeName == stat.CodeName);
    }
    public Stat GetStat(string codeName)
    {
        return _statsDics.TryGet_H(codeName);
    }

    public bool TryGetStat(Stat stat, out Stat outStat)
    {
        outStat = _stats.FirstOrDefault(x => x.CodeName == stat.CodeName);
        return outStat != null;
    }
    public bool TryGetStat(string codeName, out Stat outStat)
    {
        _statsDics.TryGetValue(codeName, out outStat);
        return outStat != null;
    }

    public BBNumber GetValue(Stat stat)
        => GetStat(stat).Value;
    public BBNumber GetValue(string codeName)
        => GetStat(codeName).Value;

    public bool HasStat(Stat stat)
    {
        return _stats.Any(x => x.CodeName == stat.CodeName);
    }
    public bool HasStat(string codeName)
    {
        return _stats.Any(x => x.CodeName == codeName);
    }

    public void SetDefaultValue(Stat stat, BBNumber value)
        => GetStat(stat).DefaultValue = value;
    public void SetDefaultValue(string codeName, BBNumber value)
        => GetStat(codeName).DefaultValue = value;

    public BBNumber GetDefaultValue(Stat stat)
        => GetStat(stat).DefaultValue;
    public BBNumber GetDefaultValue(string codeName)
        => GetStat(codeName).DefaultValue;

    public void IncreaseDefaultValue(Stat stat, BBNumber value)
        => SetDefaultValue(stat, GetDefaultValue(stat) + value);
    public void IncreaseDefaultValue(string codeName, BBNumber value)
        => SetDefaultValue(codeName, GetDefaultValue(codeName) + value);

    public void SetBonusValue(Stat stat, object key, BBNumber value)
        => GetStat(stat).SetBonusValue(key, value);
    public void SetBonusValue(Stat stat, object key, object subKey, BBNumber value)
        => GetStat(stat).SetBonusValue(key, subKey, value);
    public void SetBonusValue(string codeName, object key, BBNumber value)
        => GetStat(codeName).SetBonusValue(key, value);
    public void SetBonusValue(string codeName, object key, object subKey, BBNumber value)
        => GetStat(codeName).SetBonusValue(key, subKey, value);

    public BBNumber GetBonusValue(Stat stat)
        => GetStat(stat).BonusValue;
    public BBNumber GetBonusValue(Stat stat, object key)
        => GetStat(stat).GetBonusValue(key);
    public BBNumber GetBonusValue(Stat stat, object key, object subKey)
        => GetStat(stat).GetBonusValue(key, subKey);
    public BBNumber GetBonusValue(string codeName)
        => GetStat(codeName).BonusValue;
    public BBNumber GetBonusValue(string codeName, object key)
        => GetStat(codeName).GetBonusValue(key);
    public BBNumber GetBonusValue(string codeName, object key, object subKey)
        => GetStat(codeName).GetBonusValue(key, subKey);
    
    public void RemoveBonusValue(Stat stat, object key)
        => GetStat(stat).RemoveBonusValue(key);
    public void RemoveBonusValue(Stat stat, object key, object subKey)
        => GetStat(stat).RemoveBonusValue(key, subKey);
    public void RemoveBonusValue(string codeName, object key)
        => GetStat(codeName).RemoveBonusValue(key);
    public void RemoveBonusValue(string codeName, object key, object subKey)
        => GetStat(codeName).RemoveBonusValue(key, subKey);

    public bool ContainsBonusValue(Stat stat, object key)
        => GetStat(stat).ContainsBonusValue(key);
    public bool ContainsBonusValue(Stat stat, object key, object subKey)
        => GetStat(stat).ContainsBonusValue(key, subKey);
    public bool ContainsBonusValue(string codeName, object key)
        => GetStat(codeName).ContainsBonusValue(key);
    public bool ContainsBonusValue(string codeName, object key, object subKey)
        => GetStat(codeName).ContainsBonusValue(key, subKey);


}