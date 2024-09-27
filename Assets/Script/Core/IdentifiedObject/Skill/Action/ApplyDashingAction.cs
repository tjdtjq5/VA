using UnityEngine;

[System.Serializable]
public class ApplyDashingAction : SkillAction
{
    [SerializeField]
    private float distance;

    [SerializeField]
    private string clipName;

    public override void Apply(Skill skill)
    {

        skill.TargetSearcher.SelectTarget(skill.Owner, skill.Owner.gameObject, (targetSearcher, result) =>
        {
            if (result.resultMessage == SearchResultMessage.FindTarget)
            {
                skill.Owner.Movement.Dash(distance, skill.Owner.transform.position.GetDirection(result.selectedTarget.transform.position), clipName, ()=> { DashCallback(skill); });
            }
            else
                skill.Cancel();
        });
    }
    public override object Clone() => new ApplyDashingAction();

    void DashCallback(Skill skill)
    {
        foreach (var target in skill.Targets)
            target.SkillSystem.Apply(skill);

        if (skill.ApplyCount > skill.CurrentApplyCount)
            skill.Apply();
    }
}
