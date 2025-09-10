using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffStat : BuffBehaviour
{
    [SerializeField] private string buffBonusKey;
    [SerializeField] Stat stat;
    [SerializeField, Range(0f, 200f)] float percent;
    [SerializeField] private Poolable effectPool;

    private readonly string _buffTextPrefabPath = "Prefab/InGame/BuffText";
    
    private Character _useCharacter;
    private Character _takeCharacter;
    
    public override void OnStart(Character useCharacter, Character takeCharacter, object cause)
    {
        this._useCharacter = useCharacter;
        this._takeCharacter = takeCharacter;

        PushStat();

        // this.Buff.OnCountChange -= CountChange;
        // this.Buff.OnCountChange += CountChange;
    }

    public override void OnEnd(Character useCharacter, Character takeCharacter, object cause)
    {
        takeCharacter.Stats.GetStat(stat).RemoveBonusValue(buffBonusKey);
        UnityHelper.Log_H($"End Buff {stat.CodeName} Stat : {takeCharacter.Stats.GetStat(stat).Value}");
    }

    // private void CountChange(int buffCount)
    // {
    //     PushStat(buffCount);
    // }

    private void PushStat()
    {
        UnityHelper.Log_H($"Origin {stat.CodeName} Stat : {_takeCharacter.Stats.GetStat(stat).Value}");

        float p = percent;
        
        if (stat.GetBonusFormulaType == Stat.BonusFormulaType.AllAdd || _takeCharacter.Stats.GetStat(stat).BonusCount == 0)
        {
            _takeCharacter.Stats.GetStat(stat).SetBonusValue(buffBonusKey, p);
        }
        else
        {
            _takeCharacter.Stats.GetStat(stat).SetBonusValue(buffBonusKey, p * 0.01f + 1);
        }
        
        UnityHelper.Log_H($"Take Buff {stat.CodeName} Stat : {_takeCharacter.Stats.GetStat(stat).Value}");
        
        if (effectPool)
        {
            Poolable pool = Managers.Resources.Instantiate<Poolable>(effectPool);
            pool.transform.position = _takeCharacter.BodyBoneTr.position;
            pool.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        _takeCharacter.PushBuffStatTextSpawn(stat);
    }
    
    public override Dictionary<string, string> StringsByKeyword(string preface)
    {
        return new Dictionary<string, string>()
        {
            { $"{preface}Percent", (percent).ToString("###.#") }
        };
    }
}
