using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public class SkillGas : SkillBehaviourTime
{
    [SerializeField] private SkillTargetType targetType;
    [SerializeField] private Buff deBuff;
    
    private readonly string _prefabPath = "Prefab/Effect/Skill/Gas";
    private readonly string _prefabUpgradePath = "Prefab/Effect/Skill/GasEvolution";
    private readonly float _damageValue = 1.5f;
    
    public override void Start(Character owner, object cause)
    {
        base.Start(owner, cause);
        
        Attack(owner, cause);
    }
    
    void Attack(Character owner, object cause)
    {
        List<Character> findTargets = FindTarget(owner, cause, targetType);

        if (findTargets.Count == 0)
        {
            End(owner, cause);
            return;
        }

        bool isUpgrade = owner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.GasEvolution);
        
        owner.AddPageApplyCount(SkillApplyDamageType.Gas);
        owner.OnGasAttack?.Invoke(owner.GetPageApplyCount(SkillApplyDamageType.Gas));

        float criPercent = owner.Stats.GetValue("CriPercent").ToFloat();
        float gasCriPercent = owner.Stats.GetValue("GasCriPercent").ToFloat();
        DamageType damageType = DamageType.Skill;
        float damageValue = _damageValue;
        damageValue *= owner.Stats.GetValue("SkillDamage").ToFloat() * 0.01f + 1;
        damageValue *= owner.Stats.GetValue("GasDamage").ToFloat() * 0.01f + 1;

        for (int i = 0; i < findTargets.Count; i++)
        {
            Poolable pool = Managers.Resources.Instantiate<Poolable>(isUpgrade ? _prefabUpgradePath : _prefabPath);
            if(pool)
                pool.transform.position = findTargets[i].BodyBoneTr.position;
            
            findTargets[i].CharacterBuff.PushBuff(owner, deBuff);

            TargetTakeDamage(owner, findTargets[i], damageValue, criPercent + gasCriPercent, damageType, SkillApplyDamageType.Gas);
            findTargets[i].SetHit();
        }
    }
}
