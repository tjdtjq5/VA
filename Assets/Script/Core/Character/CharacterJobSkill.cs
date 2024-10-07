using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJobSkill : MonoBehaviour
{
    CharacterJob job;
    int jobCount;
    Skill skill;

    Dictionary<CharacterJob, float> applyP = new Dictionary<CharacterJob, float>() 
    {
        { CharacterJob.Cat , 100 },
        { CharacterJob.Dragon , 100 },
        { CharacterJob.Druid , 100 },
        { CharacterJob.Pirate , 100 },
        { CharacterJob.Robot , 100 },
        { CharacterJob.Thief , 100 },
    };
    string SkillPath => $"Skill/SKILL_JobSkill_{job}";

    public void SetUp(Entity owner, CharacterJob job, int jobCount)
    {
        this.job = job;
        this.jobCount = jobCount;

        if (skill == null || !this.job.Equals(job))
        {
            this.skill = Managers.Resources.Load<Skill>(SkillPath);
            skill.UseAcquisitionCost(owner);
            skill = RegisterWithoutCost(owner, skill, 1);
        }
    }
    public void Apply(Entity target)
    {
        if (target.IsDead)
            return;

        if (UnityHelper.IsApplyPercent(applyP[job]))
        {
            target.SkillSystem.Apply(skill);
        }
    }

    public Skill RegisterWithoutCost(Entity owner, Skill skill, int level = 0)
    {
        var clone = skill.Clone() as Skill;
        if (level > 0)
            clone.Setup(owner, level);
        else
            clone.Setup(owner);

        return clone;
    }
}
