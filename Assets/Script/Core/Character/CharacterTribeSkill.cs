using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTribeSkill : MonoBehaviour
{
    Entity owner;
    Tribe tribe;
    int tribeCount;
    TribeSkill skill;

    Dictionary<Tribe, float> applyP = new Dictionary<Tribe, float>() 
    {
        { Tribe.Cat , 100 },
        { Tribe.Dragon , 100 },
        { Tribe.Druid , 100 },
        { Tribe.Pirate , 100 },
        { Tribe.Robot , 100 },
        { Tribe.Thief , 100 },
    };

    public void SetUp(Entity owner, Tribe job, int jobCount)
    {
        skill = this.gameObject.GetOrAddComponent<TribeSkill>();
        skill.Set(job, owner);

        this.owner = owner;
        this.tribe = job;
        this.tribeCount = jobCount;
    }
    public void Apply(Entity target)
    {
        if (target.IsDead)
            return;

        if (UnityHelper.IsApplyPercent(applyP[tribe]))
            skill.Apply(target);
    }
}
