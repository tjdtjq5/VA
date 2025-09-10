using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public class SkillHellFire : SkillBehaviourTime
{
    [SerializeField] private Buff deBuff;
    
    private readonly string _prefabPath = "Prefab/Effect/Skill/HellFire";
    private readonly string _prefabUpgradePath = "Prefab/Effect/Skill/HellFireEvolution";
    private readonly float _damageValue = 1.5f;
    
    public override void Start(Character owner, object cause)
    {
        base.Start(owner, cause);
        
        Attack(owner, cause);
    }
    
    void Attack(Character owner, object cause)
    {
        bool isUpgrade = owner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.HellFireEvolution);

        List<Character> findTargets = FindTarget(owner, cause, isUpgrade ? SkillTargetType.AllEnemies : SkillTargetType.CloseEnemy);
        
        if (findTargets.Count == 0)
        {
            End(owner, cause);
            return;
        }
        
        owner.AddPageApplyCount(SkillApplyDamageType.HellFire);
        owner.OnHellFireAttack?.Invoke(owner.GetPageApplyCount(SkillApplyDamageType.HellFire));

        float criPercent = owner.Stats.GetValue("CriPercent").ToFloat();
        float hellFireCriPercent = owner.Stats.GetValue("HellFireCriPercent").ToFloat();
        DamageType damageType = DamageType.Skill;
        float damageValue = _damageValue;
        damageValue *= owner.Stats.GetValue("SkillDamage").ToFloat() * 0.01f + 1;
        damageValue *= owner.Stats.GetValue("HellFireDamage").ToFloat() * 0.01f + 1;
        
        for (int i = 0; i < findTargets.Count; i++)
        {
            TargetTakeDamage(owner, findTargets[i], damageValue, criPercent + hellFireCriPercent, damageType, SkillApplyDamageType.HellFire);
            findTargets[i].SetHit();
            
            Poolable pool = Managers.Resources.Instantiate<Poolable>(isUpgrade ? _prefabUpgradePath : _prefabPath);
            if(pool)
                pool.transform.position = findTargets[i].RootBoneTr.position;
            
            findTargets[i].CharacterBuff.PushBuff(owner, deBuff);
        }
    }
}
