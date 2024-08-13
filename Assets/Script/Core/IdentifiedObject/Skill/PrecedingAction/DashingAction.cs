using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DashingAction : SkillPrecedingAction
{
    [SerializeField]
    private float distance;

    public override void Start(Skill skill)
        => skill.Owner.Movement.Dash(distance, skill.TargetSelectionResult.selectDirection(skill.Owner.transform));

    public override bool Run(Skill skill) => !skill.Owner.Movement.IsDashing;

    protected override IReadOnlyDictionary<string, string> GetStringsByKeyword()
    {
        var dictionary = new Dictionary<string, string>() { { "distance", distance.ToString("0.##") } };
        return dictionary;
    }

    public override object Clone() => new DashingAction() { distance = distance };
}
