using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public class SkillIceThorn : SkillBehaviourTime
{
    [SerializeField] private SkillTargetType targetType;
    
    private readonly string _prefabPath = "Prefab/Effect/Skill/IceThorn";
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
        
        owner.AddPageApplyCount(SkillApplyDamageType.IceThorn);
        owner.OnIceThornAttack?.Invoke(owner.GetPageApplyCount(SkillApplyDamageType.IceThorn));

        float criPercent = owner.Stats.GetValue("CriPercent").ToFloat();
        float iceThornCriPercent = owner.Stats.GetValue("IceThornCriPercent").ToFloat();
        DamageType damageType = DamageType.Skill;
        float damageValue = _damageValue;
        damageValue *= owner.Stats.GetValue("SkillDamage").ToFloat() * 0.01f + 1;
        damageValue *= owner.Stats.GetValue("IceThornDamage").ToFloat() * 0.01f + 1;
        
        for (int i = 0; i < findTargets.Count; i++)
        {
            TargetTakeDamage(owner, findTargets[i], damageValue, criPercent + iceThornCriPercent, damageType, SkillApplyDamageType.IceThorn);
            findTargets[i].SetHit();
            
            findTargets[i].WeekConquer(owner);
            
            Poolable pool = Managers.Resources.Instantiate<Poolable>(_prefabPath);
            pool.transform.position = findTargets[i].RootBoneTr.position;
        }
    }
}
