using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillState : State<Skill>
{
    protected void TrySendCommandToOwner(Skill skill, EntityStateCommand command, string animatorClipName)
    {

        var ownerStateMachine = Entity.Owner.StateMachine;
        if (ownerStateMachine != null && !string.IsNullOrEmpty(animatorClipName))
        {
            if (ownerStateMachine.ExecuteCommand(command))
            {
                ownerStateMachine.SendMessage(EntityStateMessage.UsingSkill, (skill, animatorClipName));
            }
            else
            {
                ownerStateMachine.ExecuteCommand(EntityStateCommand.ToDefaultState);
                ownerStateMachine.SendMessage(EntityStateMessage.UsingSkill, (skill, animatorClipName));
            }
        }
    }
}