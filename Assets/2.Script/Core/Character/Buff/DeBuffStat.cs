using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public class DeBuffStat : BuffBehaviour
{
    [SerializeField] private string buffDecreaseKey;
    [SerializeField] Stat stat;
    [SerializeField, Range(0f, 100f)] private float percent;
    [SerializeField] private Poolable effectPool;
    
    private Character _useCharacter;
    private Character _takeCharacter;

    public override void OnStart(Character useCharacter, Character takeCharacter, object cause)
    {
        this._useCharacter = useCharacter;
        this._takeCharacter = takeCharacter;

        PushStat();
    }

    public override void OnEnd(Character useCharacter, Character takeCharacter, object cause)
    {
        takeCharacter.Stats.GetStat(stat).RemoveDecreaseValue(buffDecreaseKey);
        UnityHelper.Log_H($"End Buff {stat.CodeName} Stat : {takeCharacter.Stats.GetStat(stat).Value}");
    }

    private void PushStat()
    {
        UnityHelper.Log_H($"Origin {stat.CodeName} Stat : {_takeCharacter.Stats.GetStat(stat).Value}");

        if (!stat.IsPercent)
        {
            BBNumber value =  _takeCharacter.Stats.MaxStatValue(stat) * (percent * 0.01f);
            _takeCharacter.Stats.GetStat(stat).SetDecreaseValue(buffDecreaseKey, value);
        }
        else
        {
            _takeCharacter.Stats.GetStat(stat).SetDecreaseValue(buffDecreaseKey, percent);
        }

        UnityHelper.Log_H($"Take Buff {stat.CodeName} Stat : {_takeCharacter.Stats.GetStat(stat).Value}");
        
        if (effectPool)
        {
            Poolable pool = Managers.Resources.Instantiate<Poolable>(effectPool);
            _takeCharacter.Attach(pool, _takeCharacter.BodyBoneTr);
            pool.transform.localPosition = Vector3.zero;
        }
    }
    
    public override Dictionary<string, string> StringsByKeyword(string preface)
    {
        return new Dictionary<string, string>()
        {
            { $"{preface}Percent", percent.ToString() }
        };
    }
}
