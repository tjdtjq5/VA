using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StarSpawnPrecedingAction : SkillPrecedingAction
{
    float precedingTime = 0.5f;
    string prefabPath = "Prefab/Effect/SkillStarEffect";

    public override void Start(Skill skill)
    {
        skill.Owner.Movement.Preceding(precedingTime);
        GameObject startObj = Managers.Resources.Instantiate(prefabPath);
        startObj.transform.position  = skill.Owner.transform.position;
    }

    public override bool Run(Skill skill) => !skill.Owner.Movement.IsPreceding;

    public override object Clone() => new StarSpawnPrecedingAction() { precedingTime = this.precedingTime };
}
