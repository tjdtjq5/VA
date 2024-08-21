using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingState : SkillState
{
    public override void Enter()
    {
        Entity.Activate();
        Entity.StartCustomActions(SkillCustomActionType.Cast);

        TrySendCommandToOwner(Entity, EntityStateCommand.ToCastingSkillState, Entity.CastAnimationParameter);
    }

    public override void FixedUpdate()
    {
        Entity.CurrentCastTime += Managers.Time.FixedDeltaTime;
        Entity.RunCustomActions(SkillCustomActionType.Cast);
    }

    public override void Exit()
        => Entity.ReleaseCustomActions(SkillCustomActionType.Cast);
}
