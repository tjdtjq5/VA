using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DashingAction : SkillPrecedingAction
{
    public DashActionType dashActionType;

    public float distance;

    [SerializeField, Min(0.1f)]
    float speed;

    [SerializeField]
    private string clipName;

    public override void Start(Skill skill)
    {
        skill.TargetSearcher.SelectTarget(skill.Owner, skill.Owner.gameObject, (targetSearcher, result) =>
        {
            if (result.resultMessage == SearchResultMessage.FindTarget)
            {
                float dist = dashActionType == DashActionType.Distance ? distance : skill.Owner.transform.position.GetDistance(result.selectedTarget.transform.position);

                skill.Owner.Movement.Dash(dist, skill.Owner.transform.position.GetDirection(result.selectedTarget.transform.position), speed, clipName);
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
