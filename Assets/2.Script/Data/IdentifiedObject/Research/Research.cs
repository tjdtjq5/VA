using System;
using System.Collections;
using System.Collections.Generic;
using Shared.DTOs.Player;
using Shared.Fomula;
using UnityEngine;

public class Research : IdentifiedObject
{
    public int MaxLevel => _researchValues.Length;

    public StatValue StatValue(int level)
    {
        if (level > 0)
        {
            if (level >= MaxLevel)
            {
                return _researchValues[MaxLevel - 1].StatValue;
            }

            return _researchValues[level - 1].StatValue;
        }
        else
        {
            ResearchValue zeroResearchValue = new ResearchValue();
            zeroResearchValue.SetStatValue(new StatValue(_researchValues[0].StatValue.stat, 0));
            return zeroResearchValue.StatValue;
        }
    }

    [SerializeField] private ResearchValue[] _researchValues;
}
[System.Serializable]
public class ResearchValue
{
    public StatValue StatValue => _statValue;

    [SerializeField] private StatValue _statValue;

    public void SetStatValue(StatValue statValue)
    {
        _statValue = statValue;
    }
}