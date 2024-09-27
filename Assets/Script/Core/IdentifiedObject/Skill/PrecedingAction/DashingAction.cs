using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DashingAction : SkillPrecedingAction
{
    [SerializeField]
    private float distance;

    [SerializeField]
    private string clipName;

    public override void Start(Skill skill)
    {
        skill.TargetSearcher.SelectTarget(skill.Owner, skill.Owner.gameObject, (targetSearcher, result) =>
        {
            if (result.resultMessage == SearchResultMessage.FindTarget)
            {
                skill.Owner.Movement.Dash(distance, skill.Owner.transform.position.GetDirection(result.selectedTarget.transform.position), clipName);
            }
        });
    }

    public override bool Run(Skill skill) => !skill.Owner.Movement.IsDashing;

    protected override IReadOnlyDictionary<string, string> GetStringsByKeyword()
    {
        var dictionary = new Dictionary<string, string>() { { "distance", distance.ToString("0.##") } };
        return dictionary;
    }

    public override object Clone() => new DashingAction() { distance = distance };
}
