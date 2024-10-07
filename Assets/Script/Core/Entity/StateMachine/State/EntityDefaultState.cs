using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDefaultState : State<Entity>
{
    SkillSystem skillSystem;

    protected override void Setup()
    {
        skillSystem = Entity.SkillSystem;
    }

    public override bool OnReceiveMessage(int message, object data)
    {
        if ((EntityStateMessage)message != EntityStateMessage.UsingSkill)
            return false;

        var tupleData = ((Skill skill, string animatorParameter))data;

        Entity.Animator?.Play(tupleData.Item2, false);

        return true;
    }
}
