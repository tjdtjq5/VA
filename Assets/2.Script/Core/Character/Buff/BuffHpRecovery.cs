using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffHpRecovery : BuffBehaviour
{
    [SerializeField, Range(0f, 100f)] float hpRecoveryValue;
    
    public override void OnStart(Character useCharacter, Character takeCharacter, object cause)
    {
        takeCharacter.HpRecovery(hpRecoveryValue * 0.01f);
    }

    public override void OnEnd(Character useCharacter, Character takeCharacter, object cause)
    {
    }

    public override Dictionary<string, string> StringsByKeyword(string preface)
    {
        return new Dictionary<string, string>()
        {
            { $"{preface}HpRecoveryValue", hpRecoveryValue.ToString() }
        };
    }
}
