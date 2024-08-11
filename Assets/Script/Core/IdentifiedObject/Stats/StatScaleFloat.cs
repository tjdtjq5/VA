using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StatScaleFloat
{
    public float defaultValue;
    public Stat scaleStat;

    public BBNumber GetValue(Stats stats)
    {
        if (scaleStat && stats.TryGetStat(scaleStat, out var stat))
            return defaultValue * (1 + stat.Value);
        else
            return defaultValue;
    }
}