using UnityEngine;
using static InstantApplyEffectSpawnAction;

[System.Serializable]
public class InstantApplyAction : SkillAction
{
    public override void Apply(Skill skill)
    {
        skill.TargetSearcher.SelectTarget(skill.Owner, skill.Owner.gameObject, (targetSearcher, result) =>
        {
            if (result.resultMessage == SearchResultMessage.FindTarget)
            {
                skill.SearchTargets();
                foreach (var target in skill.Targets)
                {
                    target.SkillSystem.Apply(skill);
                }
            }
            else
                skill.Cancel();
        });
    }
    public override object Clone() => new InstantApplyAction();
}
