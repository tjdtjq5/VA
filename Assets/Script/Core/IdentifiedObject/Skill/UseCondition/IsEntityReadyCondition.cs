using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Entity�� Skill�� ����� �� �ִ� �������� Ȯ���ϴ� Action
[System.Serializable]
public class IsEntityReadyCondition : SkillCondition
{
    public override bool IsPass(Skill skill)
    {
        var entity = skill.Owner;
        var skillSystem = entity.SkillSystem;
        var isRunningSkillExist = skillSystem.RunningSkills.Any(x =>
        {
            return !x.IsToggleType && !x.IsPassive &&
            !(x.IsInState<InActionState>() && x.ExecutionType == SkillExecutionType.Input);
        });

        return entity.IsInState<EntityDefaultState>() && !isRunningSkillExist;
    }

    public override object Clone()
        => new IsEntityReadyCondition();
}
