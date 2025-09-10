using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public class StatOverride
{
    [SerializeField] private Stat stat;
    [SerializeField] private BBNumber defaultValue;

    public void ChangeStats(Character character)
    {
        character.Stats.GetStat(stat).SetBonusValue("StatOverride", defaultValue);
    }
}
