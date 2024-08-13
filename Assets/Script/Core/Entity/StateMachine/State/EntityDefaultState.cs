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

        var tupleData = ((Skill skill, AnimatorParameter animatorParameter))data;
        Entity.Animator?.Animator?.SetTrigger(tupleData.Item2.Hash);

        return true;
    }
    //public override void Update()
    //{
    //    if (!Entity.IsPlayer || !Entity.IsAIMove || !skillSystem)
    //        return;

    //    bool isUseableSkill = false;
    //    for (int i = 0; i < skillSystem.OwnSkills.Count; i++) 
    //    {
    //        if (skillSystem.OwnSkills[i].IsUseable)
    //        {
    //            isUseableSkill = true;
    //            break;
    //        }
    //    }

    //    if (isUseableSkill) 
    //    {
    //        UnityHelper.Log_H($"스킬을 사용할 수 있습니다");
    //    }
    //}
}
