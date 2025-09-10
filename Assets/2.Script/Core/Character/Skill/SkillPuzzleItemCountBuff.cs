using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SkillPuzzleItemCountBuff : SkillBehaviour
{
    [SerializeField] private Buff buff;
    [SerializeField] private SkillTargetType targetType;
    [SerializeField] private bool isNotBuffRemove;
    
    public override void Start(Character owner, object cause)
    {
        PuzzleAttackData pad = (PuzzleAttackData)cause;
        if (pad == null)
        {
            UnityHelper.Error_H($"SkillPuzzleItemCountBuff Is Must Cause PuzzleAttackData    Current Cause Type Is : {cause.GetType()}");
            return;
        }
       
        int itemCount = pad.itemCount;
        for (int i = 0; i < itemCount; i++)
            SetBuff(owner, cause);
        
        End(owner, cause);
    }

    public override void End(Character owner, object cause)
    {
        OnEnd?.Invoke(this, owner, cause);
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
