using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillWeekConquer : SkillBehaviour
{
    [SerializeField] private SkillTargetType targetType;
    
    public override void Start(Character owner, object cause)
    {
        List<Character> targets = FindTarget(owner, cause, targetType);
        
        for (int i = 0; i < targets.Count; i++)
            targets[i].WeekConquer(owner);
        
        End(owner, cause);
    }

    public override void End(Character owner, object cause)
    {
        OnEnd?.Invoke(this, owner, cause);
    }
}
