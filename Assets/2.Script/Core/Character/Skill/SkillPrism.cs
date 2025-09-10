using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

public class SkillPrism : SkillBehaviourTime
{
    [SerializeField] private SkillTargetType targetType;
    [SerializeField, Range(0f, 1f)] private float _attackTimeValue;
    [SerializeField] private List<float> _hitTimeValues;
    private readonly string _prefabPath = "Prefab/Effect/Skill/Prism";
    private readonly float _damageValue = 2.2f;
    public override void Start(Character owner, object cause)
    {
        base.Start(owner, cause);

        Effect(owner, cause);

        _actions.Add(_attackTimeValue, () => Attack(owner, cause));

        foreach (var hitTimeValue in _hitTimeValues)
        {
            _actions.Add(hitTimeValue, () => Hit(owner, cause));
        }
    }

    
    void Effect(Character owner, object cause)
    {
        List<Character> findTargets = FindTarget(owner, cause, targetType);
        if (findTargets.Count == 0)
        {
            return;
        }

        for (int i = 0; i < findTargets.Count; i++)
        {
            Poolable pool = Managers.Resources.Instantiate<Poolable>(_prefabPath);

            if(pool)
            {
                pool.transform.position = findTargets[i].RootBoneTr.position;
            }
        }
    }

    void Hit(Character owner, object cause)
    {
        List<Character> findTargets = FindTarget(owner, cause, targetType);
        if (findTargets.Count == 0)
        {
            return;
        }

        for (int i = 0; i < findTargets.Count; i++)
        {
            findTargets[i].SetHit();
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

        owner.AddPageApplyCount(SkillApplyDamageType.Prism);
        
        float criPercent = owner.Stats.GetValue("CriPercent").ToFloat();
        float prismCriPercent = owner.Stats.GetValue("PrismCriPercent").ToFloat();
        DamageType damageType = DamageType.Skill;
        float damageValue = _damageValue;
        damageValue *= owner.Stats.GetValue("SkillDamage").ToFloat() * 0.01f + 1;
        damageValue *= owner.Stats.GetValue("PrismDamage").ToFloat() * 0.01f + 1;
        
        for (int i = 0; i < findTargets.Count; i++)
        {
            TargetTakeDamage(owner, findTargets[i], damageValue, criPercent + prismCriPercent, damageType, SkillApplyDamageType.Prism);
            findTargets[i].SetHit();

            Week(owner, findTargets[i]);
        }
        
    }

    void Week(Character owner, Character target)
    {
        float weekPercent = owner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.Prism_P) ? 40f : 20f;
        bool isWeek = UnityHelper.IsApplyPercent(weekPercent);

        if(isWeek)
        {
            target.WeekConquer(owner);
        }
    }
}
