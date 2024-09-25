using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySkillState : State<Entity>
{
    public Skill RunningSkill { get; private set; }
    protected string AnimatorParameterClipName { get; private set; }

    public override void Enter()
    {
        Entity.Movement?.Stop();

        var playerController = Entity.GetComponent<PlayerController>();
        if (playerController)
            playerController.enabled = false;
    }

    public override void Exit()
    {
        Entity.Animator?.AniController?.Play(AnimatorParameterClipName, false);

        RunningSkill = null;

        var playerController = Entity.GetComponent<PlayerController>();
        if (playerController)
            playerController.enabled = true;
    }

    public override bool OnReceiveMessage(int message, object data)
    {
        if ((EntityStateMessage)message != EntityStateMessage.UsingSkill)
            return false;

        var tupleData = ((Skill, string))data;

        RunningSkill = tupleData.Item1;
        AnimatorParameterClipName = tupleData.Item2;

        UnityHelper.Assert_H(RunningSkill != null,
            $"CastingSkillState({message})::OnReceiveMessage");

        if (RunningSkill.IsTargetSelectSuccessful)
        {
            var selectionResult = RunningSkill.TargetSelectionResult;
            if (selectionResult.selectedTarget != Entity.gameObject)
                Entity.Movement.MoveController.LookAtImmediate(selectionResult.selectedPosition);
        }
        Entity.Animator?.AniController?.Play(AnimatorParameterClipName, false);

        return true;
    }
}
