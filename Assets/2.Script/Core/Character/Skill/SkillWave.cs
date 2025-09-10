using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public class SkillWave : SkillBehaviourTime
{
    [SerializeField] private SkillTargetType targetType;
    [SerializeField] private Buff deBuff;
    [SerializeField, Range(0f, 1f)] private float _attackTimeValue;
    private readonly string _prefabPath = "Prefab/Effect/Skill/Wave";
    private readonly float _damageValue = 2.2f;
    
    public override void Start(Character owner, object cause)
    {
        base.Start(owner, cause);

        Effect(owner, cause);

        _actions.Add(_attackTimeValue, () => Attack(owner, cause));
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

    void Attack(Character owner, object cause)
    {
        List<Character> findTargets = FindTarget(owner, cause, targetType);
        if (findTargets.Count == 0)
        {
            End(owner, cause);
            return;
        }

        owner.AddPageApplyCount(SkillApplyDamageType.Wave);
        
        float criPercent = owner.Stats.GetValue("CriPercent").ToFloat();
        float waveCriPercent = owner.Stats.GetValue("WaveCriPercent").ToFloat();
        DamageType damageType = DamageType.Skill;
        float damageValue = _damageValue;
        damageValue *= owner.Stats.GetValue("SkillDamage").ToFloat() * 0.01f + 1;
        damageValue *= owner.Stats.GetValue("WaveDamage").ToFloat() * 0.01f + 1;
        
        for (int i = 0; i < findTargets.Count; i++)
        {
            TargetTakeDamage(owner, findTargets[i], damageValue, criPercent + waveCriPercent, damageType, SkillApplyDamageType.Wave);
            findTargets[i].SetHit();

            findTargets[i].CharacterBuff.PushBuff(owner, deBuff);
        }
    }
}
