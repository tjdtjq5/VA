using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class SkillTriggerPassiveBuff : SkillBehaviour
{
    [SerializeField] private TriggerPassiveBuff triggerPassiveBuff;
    
    public override void Start(Character owner, object cause)
    {
        owner.CharacterBuff.PushTriggerPassiveBuff(triggerPassiveBuff);
        this.Skill.OnRemove += (s)=> End(owner, cause);
    }

    public override void End(Character owner, object cause)
    {
        owner.CharacterBuff.RemoveTriggerPassiveBuff(triggerPassiveBuff);
        OnEnd?.Invoke(this, owner, cause);
    }
}
public enum TriggerPassiveBuff
{
    None,
    
    FlashSlash,
    SlashAcceleration,
    ComboTranscendence,
    ComboTranscendence_P,
    InstantCombustion,
    InstantCombustion_P,
    HeatAmplification,
    HeatAmplification_P,
    HeavenlyPunishment,
    DoubleLightning,
    DoubleLightning_P,
    HellFireEvolution,
    CombustionExplosion,
    Skill_AddSequenceAttack_Attack_Sequence,
    PoisonEvolution,
    GasEvolution,
    StunDamageEnhancement,
    StunDamageEnhancement_P,
    PerfectPath,

    Missile_P,
    ShootingStar_P,
    Prism_P,
    Wave_P,
    BloodBlade_P,
}