using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillSequencePoint : SkillBehaviour
{
    [SerializeField] SkillSequencePointType type;
    [SerializeField] private int value;
    
    public override void Start(Character owner, object cause)
    {
        switch (type)
        {
            case SkillSequencePointType.Default:
                owner.Stats.sequenceStat.DefaultValue += value;
                break;
            case SkillSequencePointType.Combo:
                
                if (cause != null && cause is CharacterApplyAttack)
                {
                    CharacterApplyAttack caa = (CharacterApplyAttack)cause;

                    if (caa.cause is PuzzleAttackData)
                    {
                        PuzzleAttackData pad = (PuzzleAttackData)caa.cause;
                        int combo = pad.combo;
                        owner.Stats.sequenceStat.DefaultValue += combo * value;
                    }
                }
                break;
        }
        
        End(owner, cause);
    }

    public override void End(Character owner, object cause)
    {
        OnEnd?.Invoke(this, owner, cause);
    }
    
    public override Dictionary<string, string> StringsByKeyword(string preface)
    {
        var stringsByKeyword = base.StringsByKeyword(preface);
        stringsByKeyword.Add($"{preface}Value", value.ToString());
        return stringsByKeyword;
    }
}

public enum SkillSequencePointType
{
    Default,
    Combo,
}
