using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJobSkill : MonoBehaviour
{
    Entity owner;
    CharacterJob job;
    int jobCount;
    JobSkill skill;

    Dictionary<CharacterJob, float> applyP = new Dictionary<CharacterJob, float>() 
    {
        { CharacterJob.Cat , 100 },
        { CharacterJob.Dragon , 100 },
        { CharacterJob.Druid , 100 },
        { CharacterJob.Pirate , 100 },
        { CharacterJob.Robot , 100 },
        { CharacterJob.Thief , 100 },
    };

    public void SetUp(Entity owner, CharacterJob job, int jobCount)
    {
        skill = this.gameObject.GetOrAddComponent<JobSkill>();
        skill.Set(job, owner);

        this.owner = owner;
        this.job = job;
        this.jobCount = jobCount;
    }
    public void Apply(Entity target)
    {
        if (target.IsDead)
            return;

        if (UnityHelper.IsApplyPercent(applyP[job]))
            skill.Apply(target);
    }
}
