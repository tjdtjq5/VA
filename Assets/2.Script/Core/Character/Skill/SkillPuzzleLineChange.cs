using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillPuzzleLineChange : SkillBehaviourTime
{
    public override void Start(Character owner, object cause)
    {
        base.Start(owner, cause);

        if ((int)UnityHelper.Random_H(0, 10000) % 2 == 0)
        {
            Managers.Observer.UIPuzzle.ChangeColumnLinePuzzle();
        }
        else
        {
            Managers.Observer.UIPuzzle.ChangeRowLinePuzzle();
        }
    }

    public override void End(Character owner, object cause)
    {
        base.End(owner, cause);
    }
}
