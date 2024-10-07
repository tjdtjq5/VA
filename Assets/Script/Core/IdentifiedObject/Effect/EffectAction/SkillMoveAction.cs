using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillMoveAction : EffectAction
{
    public override bool Apply(Effect effect, Entity user, Entity target, int level, int stack, float scale)
    {
        user.StateMachine.ExecuteCommand(EntityStateCommand.ToInSkillActionState);
        return true;
    }

    public override void Release(Effect effect, Entity user, Entity target, int level, float scale)
    => user.StateMachine.ExecuteCommand(EntityStateCommand.ToDefaultState);

    public override object Clone()
    {
        throw new System.NotImplementedException();
    }
}
