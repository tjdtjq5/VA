using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SkillBuff : SkillBehaviourTime
{
    [SerializeField] private Buff buff;
    [SerializeField] private SkillTargetType targetType;
    [SerializeField] private bool isNotBuffRemove;
    
    public override void Start(Character owner, object cause)
    {
        base.Start(owner, cause);
        
        SetBuff(owner, cause);
    }

    void SetBuff(Character owner, object cause)
    {
        List<Character> findTargets = FindTarget(owner, cause, targetType);
        for (int i = 0; i < findTargets.Count; i++)
        {
            int index = i;
            Buff cloneBuff = findTargets[index].CharacterBuff.PushBuff(owner, buff);
            
            if(!isNotBuffRemove)
                this.Skill.OnRemove += (s) => findTargets[index].CharacterBuff.RemoveBuff(cloneBuff);
        }
    }

    public override Dictionary<string, string> StringsByKeyword(string preface)
    {
        var stringsByKeyword = base.StringsByKeyword(preface);
        stringsByKeyword.AddRange(buff.StringsByKeyword(preface));
        return stringsByKeyword;
    }
}
