using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public class SkillBloodBlade : SkillBehaviourTime
{
    [SerializeField] private SkillTargetType targetType;
    [SerializeField, Range(0f, 1f)] private float _attackTimeValue;
    [SerializeField] private List<float> _hitTimeValues;
    private readonly string _prefabPath = "Prefab/Effect/Skill/BloodBlade";
    private readonly float _damageValue = 2.2f;
    private readonly float _hpDamagePercent = 1f; // 1% 체력감소 당 피해량

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
                pool.transform.position = findTargets[i].BodyBoneTr.position;
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

        owner.AddPageApplyCount(SkillApplyDamageType.BloodBlade);
        
        float criPercent = owner.Stats.GetValue("CriPercent").ToFloat();
        float bloodBladeCriPercent = owner.Stats.GetValue("BloodBladeCriPercent").ToFloat();
        DamageType damageType = DamageType.Skill;
        float damageValue = _damageValue;
        damageValue *= owner.Stats.GetValue("SkillDamage").ToFloat() * 0.01f + 1;
        damageValue *= owner.Stats.GetValue("BloodBladeDamage").ToFloat() * 0.01f + 1;

        float hpPercent = 100 - owner.HpPercent * 100;
        float hpDamage = hpPercent * _hpDamagePercent;

        damageValue *= hpDamage * 0.01f + 1;
        
        for (int i = 0; i < findTargets.Count; i++)
        {
            TargetTakeDamage(owner, findTargets[i], damageValue, criPercent + bloodBladeCriPercent, damageType, SkillApplyDamageType.BloodBlade);
            findTargets[i].SetHit();
        }
    }
}
