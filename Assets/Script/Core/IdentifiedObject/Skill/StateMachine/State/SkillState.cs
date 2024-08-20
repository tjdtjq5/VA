using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillState : State<Skill>
{
    protected void TrySendCommandToOwner(Skill skill, EntityStateCommand command, AnimatorParameter animatorParameter)
    {
        var ownerStateMachine = Entity.Owner.StateMachine;
        if (ownerStateMachine != null && animatorParameter.IsValid)
        {
            if (animatorParameter.type == AnimatorParameterType.Bool && ownerStateMachine.ExecuteCommand(command))
                ownerStateMachine.SendMessage(EntityStateMessage.UsingSkill, (skill, animatorParameter));
            else if (animatorParameter.type == AnimatorParameterType.Trigger)
            {
                ownerStateMachine.ExecuteCommand(EntityStateCommand.ToDefaultState);
                ownerStateMachine.SendMessage(EntityStateMessage.UsingSkill, (skill, animatorParameter));
            }
        }
    }
}