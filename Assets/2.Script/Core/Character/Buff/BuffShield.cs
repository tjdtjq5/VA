using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public class BuffShield : BuffBehaviour
{
    [SerializeField, Range(0f, 100f)] float shieldValue;

    private BBNumber _value;
    
    public override void OnStart(Character useCharacter, Character takeCharacter, object cause)
    {
        _value = takeCharacter.Stats.MaxStatValue(takeCharacter.Stats.hpStat) * (shieldValue * 0.01f);
        BBNumber prevValue = takeCharacter.Stats.shieldStat.Value;
        takeCharacter.Stats.shieldStat.DefaultValue = prevValue + _value;
    }

    public override void OnEnd(Character useCharacter, Character takeCharacter, object cause)
    {
        BBNumber prevValue = takeCharacter.Stats.shieldStat.Value;
        takeCharacter.Stats.shieldStat.DefaultValue = prevValue - _value;
    }
    
    public override Dictionary<string, string> StringsByKeyword(string preface)
    {
        return new Dictionary<string, string>()
        {
            { $"{preface}ShieldValue", shieldValue.ToString() }
        };
    }
}
