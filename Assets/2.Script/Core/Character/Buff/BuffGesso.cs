using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuffGesso : BuffBehaviour
{
    [SerializeField] private int value;
    
    public override void OnStart(Character useCharacter, Character takeCharacter, object cause)
    {
        Managers.Observer.Gesso += value;
    }

    public override void OnEnd(Character useCharacter, Character takeCharacter, object cause)
    {
        
    }
}
