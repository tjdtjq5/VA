using EasyButtons;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] BBNumber bbb;
    [SerializeField] Entity player;
    [SerializeField] Stats stats;
    [SerializeField] Stat stat;

    [Button]
    public void AddStat(string key, string subKey, float bonus)
    {
        player.Stats.GetStat(stat).SetBonusValue(key, subKey, bonus);
        UnityHelper.Log_H(player.Stats.GetValue(stat));
    }

    [Button]
    public void DebugStats()
    {
        UnityHelper.Log_H(stats.GetStat(stat).Value);
    }
}
