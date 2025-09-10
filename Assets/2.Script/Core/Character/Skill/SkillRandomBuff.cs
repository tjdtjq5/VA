using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillRandomBuff : SkillBehaviourTime
{
    [SerializeField] private List<Buff> buffs = new List<Buff>();
    [SerializeField] private SkillTargetType targetType;
    
    public override void Start(Character owner, object cause)
    {
        base.Start(owner, cause);
        
        SetBuff(owner, cause);
    }

    void SetBuff(Character owner, object cause)
    {
        List<Character> findTargets = FindTarget(owner, cause, targetType);
        Buff buff = buffs[(int)UnityHelper.Random_H(0, buffs.Count)];
        for (int i = 0; i < findTargets.Count; i++)
        {
            findTargets[i].CharacterBuff.PushBuff(owner, buff);
        }
    }
}
