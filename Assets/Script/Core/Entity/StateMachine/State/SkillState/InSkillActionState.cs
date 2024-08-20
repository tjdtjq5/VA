using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InSkillActionState : EntitySkillState
{
    public bool IsStateEnded { get; private set; }

    public override void Update()
    {
        if (RunningSkill.InSkillActionFinishOption == InSkillActionFinishOption.FinishWhenAnimationEnded)
            IsStateEnded = !Entity.Animator.AniController.GetBool(AnimatorParameterHash);
    }

    public override bool OnReceiveMessage(int message, object data)
    {
        if (!base.OnReceiveMessage(message, data))
            return false;

        if (RunningSkill.InSkillActionFinishOption != InSkillActionFinishOption.FinishWhenAnimationEnded)
            RunningSkill.onApplied += OnSkillApplied;

        return true;
    }

    public override void Exit()
    {
        IsStateEnded = false;
        RunningSkill.onApplied -= OnSkillApplied;

        base.Exit();
    }

    private void OnSkillApplied(Skill skill, int currentApplyCount)
    {
        switch (skill.InSkillActionFinishOption)
        {
            case InSkillActionFinishOption.FinishOnceApplied:
                IsStateEnded = true;
                break;

            case InSkillActionFinishOption.FinishWhenFullyApplied:
                IsStateEnded = skill.IsFinished;
                break;
        }
    }
}
