using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public class SkillSlash : SkillBehaviourTime
{
    [SerializeField] private SkillTargetType targetType;
    
    private readonly string _prefabPath = "Prefab/Effect/Skill/Slash";
    private readonly string _hitPrefabPath = "Prefab/Effect/Skill/SlashHit";
    private readonly string _prefabUpgradePath = "Prefab/Effect/Skill/FlashSlash";
    private readonly string _hitPrefabUpgradePath = "Prefab/Effect/Skill/FlashSlashHit";

    private readonly float _damageValue = 2.2f;
    
    public override void Start(Character owner, object cause)
    {
        base.Start(owner, cause);
        
        SlashEffect(owner, cause);
    }

    void SlashEffect(Character owner, object cause)
    {
        List<Character> findTargets = FindTarget(owner, cause, targetType);
        if (findTargets.Count == 0)
        {
            End(owner, cause);
            return;
        }
        
        owner.AddPageApplyCount(SkillApplyDamageType.Slash);
        owner.OnSlashAttack?.Invoke(owner.GetPageApplyCount(SkillApplyDamageType.Slash));
        
        bool isUpgrade = owner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.FlashSlash);
        Poolable pool = Managers.Resources.Instantiate<Poolable>(isUpgrade ? _prefabUpgradePath : _prefabPath);
        if (pool)
        {
            pool.transform.position = owner.BodyBoneTr.position;
            Managers.Tween.TweenInvoke(0.35f).SetOnPerceontCompleted(1, () =>
            {
                Attack(owner, cause);
                pool.Destroy();
            });
        }
    }

    void Attack(Character owner, object cause)
    {
        List<Character> findTargets = FindTarget(owner, cause, targetType);
        if (findTargets.Count == 0)
        {
            End(owner, cause);
            return;
        }

        owner.AddPageApplyCount(SkillApplyDamageType.Slash);
        owner.OnSlashAttack?.Invoke(owner.GetPageApplyCount(SkillApplyDamageType.Slash));

        bool isUpgrade = owner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.FlashSlash);

        float criPercent = owner.Stats.GetValue("CriPercent").ToFloat();
        float slashCriPercent = owner.Stats.GetValue("SlashCriPercent").ToFloat();
        DamageType damageType = DamageType.Skill;
        float damageValue = _damageValue;
        damageValue *= owner.Stats.GetValue("SkillDamage").ToFloat() * 0.01f + 1;
        damageValue *= owner.Stats.GetValue("SlashDamage").ToFloat() * 0.01f + 1;
        
        for (int i = 0; i < findTargets.Count; i++)
        {
            TargetTakeDamage(owner, findTargets[i], damageValue, criPercent + slashCriPercent, damageType, SkillApplyDamageType.Slash);
            findTargets[i].SetHit();

            Poolable pool = Managers.Resources.Instantiate<Poolable>(isUpgrade ? _hitPrefabUpgradePath : _hitPrefabPath);
            if(pool)
                pool.transform.position = findTargets[i].BodyBoneTr.position;
        }
    }
}
