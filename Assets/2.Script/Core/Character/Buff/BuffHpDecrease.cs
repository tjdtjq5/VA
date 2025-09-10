using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class BuffHpDecrease : BuffBehaviour
{
    [SerializeField, Range(0f, 100f)] float hpDecreaseValue;
    [SerializeField] private Poolable effectPool;
    
    public override void OnStart(Character useCharacter, Character takeCharacter, object cause)
    {
        takeCharacter.HpDecrease(hpDecreaseValue * 0.01f);
        
        if (effectPool)
        {
            Poolable pool = Managers.Resources.Instantiate<Poolable>(effectPool);
            takeCharacter.Attach(pool, takeCharacter.BodyBoneTr);
            pool.transform.localPosition = Vector3.zero;
        }
    }

    public override void OnEnd(Character useCharacter, Character takeCharacter, object cause)
    {
    }

    public override Dictionary<string, string> StringsByKeyword(string preface)
    {
        return new Dictionary<string, string>()
        {
            { $"{preface}HpDecreaseValue", hpDecreaseValue.ToString() }
        };
    }
}
