using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillPuzzleItem : SkillBehaviourTime
{
    [SerializeField] PuzzleItem puzzleItem;
    public PuzzleItem PuzzleItem => puzzleItem;
    
    public override void Start(Character owner, object cause)
    {
        base.Start(owner, cause);
       
        Managers.Observer.UIPuzzle.RandomItem(puzzleItem);
    }

    public override void End(Character owner, object cause)
    {
        base.End(owner, cause);
    }
}
