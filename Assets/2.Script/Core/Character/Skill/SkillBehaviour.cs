using System;
using System.Collections.Generic;
using Shared.CSharp;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public abstract class SkillBehaviour
{
    protected Skill Skill;
    public bool IsUse { get; set; }
    public Action<SkillBehaviour, Character, object> OnEnd;
    
    public SkillTiming timing;
    
    [SerializeField, ShowIf("@timing == SkillTiming.PuzzleAttackStart || timing == SkillTiming.PuzzleAttackEnd")] private PuzzleType timingPuzzleType;
    [SerializeField, ShowIf("@timing == SkillTiming.TurnStart || timing == SkillTiming.TurnEnd")] private int timingTurnCycle;
    [SerializeField, ShowIf("@timing == SkillTiming.ApplyAttack")] public SkillApplyDamageType timingDamageType;
    [SerializeField, Range(0f, 100f), ShowIf("@timing == SkillTiming.HpDecrease")] public float hpChangeValue;
    [SerializeField, ShowIf("@timing == SkillTiming.HpDecrease")] public bool isHpChangeIncrease;
    [SerializeField, ShowIf("@timing == SkillTiming.PuzzleAttackStart || timing == SkillTiming.PuzzleAttackEnd || (timing == SkillTiming.ApplyAttack && timingDamageType == SkillApplyDamageType.NomalPuzzleAttack)")] private AttackGrade timingPuzzleComboMin;

    [SerializeField,
     ShowIf(
         "@timing == SkillTiming.SlashAttack || timing == SkillTiming.LightningAttack || timing == SkillTiming.HellFireAttack || timing == SkillTiming.GasAttack || timing == SkillTiming.IceThornAttack || timing == SkillTiming.BurnAttack || timing == SkillTiming.PoisonAttack")]
    private int attackTurnCount;
    
    [SerializeField, Range(0f, 100f)] private float percent = 100;
    [SerializeField, Range(0f, 100f)] private float hpPercentUnder = 100;
    [SerializeField, Min(1)] public int applyCount = 1;
    
    private int _applyCount;

    public void Initialize(Skill skill)
    {
        this.Skill = skill;
        _applyCount = 0;
    }
    public abstract void Start(Character owner, object cause);
    public abstract void End(Character owner, object cause);
    public void AddApply() => _applyCount++;
    public void ClearApply() => _applyCount = 0;
    public bool CheckApply() => applyCount <= 0 || _applyCount < applyCount;
    public void EndApply() => _applyCount = applyCount;
    public bool IsOperate() => UnityHelper.IsApplyPercent(percent);
    public bool IsHpUnder(Character owner) => hpPercentUnder >= owner.HpPercent * 100;
    public bool IsTimingConditionCheck(Character owner, object cause)
    {
        if (timing is SkillTiming.PuzzleAttackStart or SkillTiming.PuzzleAttackEnd)
        {
            PuzzleAttackData pad = (PuzzleAttackData)cause;
            bool isPuzzleTypeCheck = timingPuzzleType == PuzzleType.None || pad.data.puzzleType == timingPuzzleType;
            bool isPuzzleComboMinCheck = timingPuzzleComboMin <= GameDefine.GetAttackGrade(pad.combo);
            return isPuzzleTypeCheck && isPuzzleComboMinCheck;
        }
        else if (timing is SkillTiming.TurnStart or SkillTiming.TurnEnd)
        {
            int cycle = timingTurnCycle;
            
            if (owner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.SlashAcceleration) && this.GetType() == (typeof(SkillPuzzleItem)))
            {
                SkillPuzzleItem spi = (SkillPuzzleItem)this;

                if (spi.PuzzleItem.Skill.CodeName.Equals("PuzzleItemSlash"))
                {
                    cycle--;
                }
            }
            
            int page = (int)cause;
            bool isPageCheck = cycle <= 0 || page % cycle == 0;
            
            return isPageCheck;
        }
        else if (timing is SkillTiming.ApplyAttack)
        {
            CharacterApplyAttack caa = (CharacterApplyAttack)cause;
            
            switch (timingDamageType)
            {
                case SkillApplyDamageType.None:
                    return false;
                case SkillApplyDamageType.NomalPuzzleAttack:
                    if (caa.cause.GetType() == typeof(PuzzleAttackData))
                    {
                        PuzzleAttackData pad = (PuzzleAttackData)caa.cause;
                        if (pad.isSequence)
                        {
                            return false;
                        }

                        return timingPuzzleComboMin <= GameDefine.GetAttackGrade(pad.combo);
                    }else
                        return false;
                case SkillApplyDamageType.Sequence:
                    if (caa.cause.GetType() == typeof(PuzzleAttackData))
                    {
                        PuzzleAttackData pad = (PuzzleAttackData)caa.cause;
                        return pad.isSequence;
                    }else
                        return false;
                default:
                {
                    return caa.damageType == timingDamageType;
                }
            }
        }
        else if (timing is SkillTiming.HpDecrease)
        {
            float hpValue = (float)cause;
            hpValue *= 100f;

            if (isHpChangeIncrease)
            {
                return hpValue >= hpChangeValue;
            }
            else
            {
                return hpValue <= hpChangeValue;
            }
        }
        else if (timing is SkillTiming.SlashAttack or SkillTiming.LightningAttack or SkillTiming.HellFireAttack
                 or SkillTiming.GasAttack or SkillTiming.IceThornAttack or SkillTiming.BurnAttack
                 or SkillTiming.PoisonAttack)
        {
            
            if (attackTurnCount <= 0)
            {
                return true;
            }
            else
            {
                int aPageCount = (int)cause;
                return aPageCount == attackTurnCount;
            }
        }
        else
            return true;
    }
    public virtual void FixedUpdate() { }
    protected virtual List<Character> IgnoreDeadTargets(List<Character> targets)
    {
        return targets.FindAll(t => !t.IsNotDetect);
    }
    protected void TargetTakeDamage(Character owner, Character target, float damageValue, float criPercent, DamageType damageType, SkillApplyDamageType skillApplyDamageType, object cause = null)
    {
        if (cause == null)
            cause = this;
        
        owner.ApplyAttack(target, cause, damageValue, criPercent, damageType, skillApplyDamageType);
    }
    protected List<Character> FindTarget(Character owner, object cause, SkillTargetType targetTypeType)
    {
        List<Character> targets = new List<Character>();
        List<Character> enemies = Managers.Observer.PuzzleBattleStateMachine.Enemies;
        
        switch (targetTypeType)
        {
            case SkillTargetType.My:
                targets.Add(owner);
                break;
            case SkillTargetType.Player:
                targets.Add(Managers.Observer.Player);
                break;
            case SkillTargetType.CloseEnemy:
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].IsNotDetect)
                        continue;

                    targets.Add(enemies[i]);
                    return targets;
                }
                break;
            case SkillTargetType.FarEnemy:
                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    if (enemies[i].IsNotDetect)
                        continue;

                    targets.Add(enemies[i]);
                    return targets;
                }
                break;
            case SkillTargetType.AllEnemies:
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].IsNotDetect)
                        continue;

                    targets.Add(enemies[i]);
                }
                break;
            case SkillTargetType.ApplyAttackTarget:
                CharacterApplyAttack caa = (CharacterApplyAttack)cause;

                if (caa.takeOwner.IsNotDetect)
                    return targets;

                targets.Add(caa.takeOwner);
                break;
            default:
                break;
        }

        return targets;
    }

    public virtual Dictionary<string, string> StringsByKeyword(string preface)
    {
        var stringsByKeyword = new Dictionary<string, string>()
        {
            { $"{preface}Percent", (percent).ToString("###.#") },
            { $"{preface}ApplyCount", applyCount.ToString() },
            { $"{preface}HpPercentUnder", (hpPercentUnder).ToString("###.#") },
        };

        switch (timing)
        {
            case SkillTiming.None:
                break;
            case SkillTiming.Instance:
                break;
            case SkillTiming.StageStart:
                break;
            case SkillTiming.BattleStart:
                break;
            case SkillTiming.TurnStart:
                stringsByKeyword.Add($"{preface}TimingPageCycle", timingTurnCycle.ToString());
                break;
            case SkillTiming.CharacterActionStart:
                break;
            case SkillTiming.PuzzleAttackStart:
                break;
            case SkillTiming.StageEnd:
                break;
            case SkillTiming.BattleEnd:
                break;
            case SkillTiming.TurnEnd:
                stringsByKeyword.Add($"{preface}TimingPageCycle", timingTurnCycle.ToString());
                break;
            case SkillTiming.CharacterActionEnd:
                break;
            case SkillTiming.PuzzleAttackEnd:
                break;
            case SkillTiming.TakeDamage:
                break;
            case SkillTiming.StunSuccess:
                break;
            case SkillTiming.SequenceAttack:
                break;
            case SkillTiming.SlashAttack:
                stringsByKeyword.Add($"{preface}AttackPageCount", attackTurnCount.ToString());
                break;
            case SkillTiming.LightningAttack:
                stringsByKeyword.Add($"{preface}AttackPageCount", attackTurnCount.ToString());
                break;
            case SkillTiming.HellFireAttack:
                stringsByKeyword.Add($"{preface}AttackPageCount", attackTurnCount.ToString());
                break;
            case SkillTiming.GasAttack:
                stringsByKeyword.Add($"{preface}AttackPageCount", attackTurnCount.ToString());
                break;
            case SkillTiming.IceThornAttack:
                stringsByKeyword.Add($"{preface}AttackPageCount", attackTurnCount.ToString());
                break;
            case SkillTiming.BurnAttack:
                stringsByKeyword.Add($"{preface}AttackPageCount", attackTurnCount.ToString());
                break;
            case SkillTiming.PoisonAttack:
                stringsByKeyword.Add($"{preface}AttackPageCount", attackTurnCount.ToString());
                break;
            case SkillTiming.ApplyAttack:
                stringsByKeyword.Add($"{preface}AttackPageCount", attackTurnCount.ToString());
                break;
            case SkillTiming.Perfect:
                break;
            case SkillTiming.HpDecrease:
                break;
            case SkillTiming.Dead:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return stringsByKeyword;
    }
    public bool Equals(SkillBehaviour other)
    {
        return this.GetType() == other.GetType();
    }
}

public enum SkillTargetType
{
    My,
    Player,
    CloseEnemy,
    FarEnemy,
    AllEnemies,
    ApplyAttackTarget,
}

public enum SkillApplyDamageType
{
    None,
    NomalPuzzleAttack,
    Sequence,
    Slash,
    Lightning,
    HellFire,
    Gas,
    IceThorn,
    Burn,
    Poison,
    BloodBlade,
    Missile,
    Prism,
    ShootingStar,
    Wave,
    Wind,
    
    Submersion,
}