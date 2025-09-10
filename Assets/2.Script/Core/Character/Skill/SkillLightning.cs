using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public class SkillLightning : SkillBehaviourTime
{
    private readonly string _prefabPath = "Prefab/Effect/Skill/Lightning";
    private readonly string _upgradePrefabPath = "Prefab/Effect/Skill/HeavenlyPunishment";
    private readonly float _damageValue = 3f;
    
    public override void Start(Character owner, object cause)
    {
        if (owner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.DoubleLightning))
        {
            _timeMultiplier = 1.5f;
        }
        if (owner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.DoubleLightning_P))
        {
            _timeMultiplier = 2f;
        }
        
        base.Start(owner, cause);
        
        Attack(owner, cause);
        
        if (owner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.DoubleLightning))
        {
            float time = GetTime * 0.5f;
            Managers.Tween.TweenInvoke(time).SetOnComplete(() => Attack(owner, cause));
        }
        if (owner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.DoubleLightning_P))
        {
            float timeD = GetTime * 0.35f;
            Managers.Tween.TweenInvoke(timeD).SetOnComplete(() => Attack(owner, cause));
            
            float timeM = GetTime * 0.7f;
            Managers.Tween.TweenInvoke(timeM).SetOnComplete(() => Attack(owner, cause));
        }
    }

    void Attack(Character owner, object cause)
    {
        string prefabName = "";
        SkillTargetType targetType = SkillTargetType.CloseEnemy;
        if (owner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.HeavenlyPunishment))
        {
            prefabName = _upgradePrefabPath;
            targetType = SkillTargetType.AllEnemies;
        }
        else
        {
            prefabName = _prefabPath;
            targetType = SkillTargetType.CloseEnemy;
        }
        
        List<Character> findTargets = FindTarget(owner, cause, targetType);
        
        if (findTargets.Count == 0)
        {
            End(owner, cause);
            return;
        }
        
        owner.AddPageApplyCount(SkillApplyDamageType.Lightning);
        owner.OnLightningAttack?.Invoke(owner.GetPageApplyCount(SkillApplyDamageType.Lightning));

        float criPercent = owner.Stats.GetValue("CriPercent").ToFloat();
        float lightningCriPercent = owner.Stats.GetValue("LightningCriPercent").ToFloat();
        DamageType damageType = DamageType.Skill;
        float damageValue = _damageValue;
        damageValue *= owner.Stats.GetValue("SkillDamage").ToFloat() * 0.01f + 1;
        damageValue *= owner.Stats.GetValue("LightningDamage").ToFloat() * 0.01f + 1;

        for (int i = 0; i < findTargets.Count; i++)
        {
            TargetTakeDamage(owner, findTargets[i], damageValue, criPercent + lightningCriPercent, damageType, SkillApplyDamageType.Lightning);
            findTargets[i].SetHit();
            
            Poolable pool = Managers.Resources.Instantiate<Poolable>(prefabName);
            if(pool)
                pool.transform.position = findTargets[i].RootBoneTr.position;
        }
    }
}
