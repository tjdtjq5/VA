using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillPuzzleCrossChange : SkillBehaviourTime
{
    public override void Start(Character owner, object cause)
    {
       base.Start(owner, cause);
       
       Managers.Observer.UIPuzzle.ChangeCrossPuzzle();
    }

    public override void End(Character owner, object cause)
    {
        base.End(owner, cause);
    }
}
