using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobbyPlayer : Player
{
    public override void Initialize(DungeonTree dungeonTree)
    {
        base.Initialize(dungeonTree);

        CharacterMove.MovingAnimationName = "Work";
        CharacterMove.SetSpeed(0.2f);
    }
    public override void SetIdle()
    {
        if (IsDead)
            return;

        if(CurrentForm == PuzzleType.None)
        {
            SetAnimation(IdleAnimationName, true);
        }
    }
}
