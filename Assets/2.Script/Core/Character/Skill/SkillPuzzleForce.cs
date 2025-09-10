using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillPuzzleForce : SkillBehaviourTime
{
    public override void Start(Character owner, object cause)
    {
        base.Start(owner, cause);
       
        Managers.Observer.UIPuzzle.RandomForce();
    }

    public override void End(Character owner, object cause)
    {
        base.End(owner, cause);
    }
}
