using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySkillState : State<Entity>
{
    public Skill RunningSkill { get; private set; }
    protected int AnimatorParameterHash { get; private set; }

    public override void Enter()
    {
        Entity.Movement?.Stop();

        var playerController = Entity.GetComponent<PlayerController>();
        if (playerController)
            playerController.enabled = false;
    }

    public override void Exit()
    {
        Entity.Animator?.AniController?.SetBool(AnimatorParameterHash, false);

        RunningSkill = null;

        var playerController = Entity.GetComponent<PlayerController>();
        if (playerController)
            playerController.enabled = true;
    }

    public override bool OnReceiveMessage(int message, object data)
    {
        if ((EntityStateMessage)message != EntityStateMessage.UsingSkill)
            return false;

        var tupleData = ((Skill, AnimatorParameter))data;

        RunningSkill = tupleData.Item1;
        AnimatorParameterHash = tupleData.Item2.Hash;

        UnityHelper.Assert_H(RunningSkill != null,
            $"CastingSkillState({message})::OnReceiveMessage - �߸��� data�� ���޵Ǿ����ϴ�.");

        if (RunningSkill.IsTargetSelectSuccessful)
        {
            var selectionResult = RunningSkill.TargetSelectionResult;
            if (selectionResult.selectedTarget != Entity.gameObject)
                Entity.Movement.MoveController.LookAtImmediate(selectionResult.selectedPosition);
        }
        Entity.Animator?.AniController?.SetBool(AnimatorParameterHash, true);

        return true;
    }
}
