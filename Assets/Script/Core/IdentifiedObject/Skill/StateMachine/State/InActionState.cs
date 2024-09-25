using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InActionState : SkillState
{
    private bool isAutoExecuteType;
    private bool isInstantApplyType;

    protected override void Setup()
    {
        isAutoExecuteType = Entity.ExecutionType == SkillExecutionType.Auto;
        isInstantApplyType = Entity.ApplyType == SkillApplyType.Instant;
    }

    public override void Enter()
    {
        if (!Entity.IsActivated)
            Entity.Activate();

        Entity.StartAction();

        Apply();
    }

    public override void FixedUpdate()
    {
        Entity.CurrentDuration += Managers.Time.FixedDeltaTime;
        Entity.CurrentApplyCycle += Managers.Time.FixedDeltaTime;

        if (Entity.IsToggleType)
            Entity.UseDeltaCost();

        if (isAutoExecuteType && Entity.IsApplicable)
            Apply();
    }

    public override void Exit()
    {
        Entity.CancelSelectTarget();
        Entity.ReleaseAction();
    }

    public override bool OnReceiveMessage(int message, object data)
    {
        var stateMessage = (SkillStateMessage)message;
        if (stateMessage != SkillStateMessage.Use || isAutoExecuteType)
            return false;

        if (Entity.IsApplicable)
        {
            if (Entity.IsTargetSelectionTiming(TargetSelectionTimingOption.UseInAction))
            {
                if (!Entity.IsSearchingTarget)
                    Entity.SelectTarget(OnTargetSelectionCompleted);
            }
            else
                Apply();

            return true;
        }
        else
            return false;
    }

    private void Apply()
    {
        TrySendCommandToOwner(Entity, EntityStateCommand.ToInSkillActionState, Entity.ActionAnimationClipName);

        if (isInstantApplyType)
            Entity.Apply();
        else if (!isAutoExecuteType)
            Entity.CurrentApplyCount++;
    }

    private void OnTargetSelectionCompleted(Skill skill, TargetSearcher targetSearcher, TargetSelectionResult result)
    {
        if (skill.HasValidTargetSelectionResult)
            Apply();
    }
}