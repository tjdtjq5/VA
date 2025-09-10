using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public class SkillWind : SkillBehaviourTime
{
    [SerializeField] private SkillTargetType targetType;
    [SerializeField, Range(0f, 1f)] private float _attackTimeValue;
    [SerializeField] private List<float> _hitTimeValues;
    private readonly string _prefabPath = "Prefab/Effect/Skill/Wind";
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

        owner.AddPageApplyCount(SkillApplyDamageType.Wind);
        
        float criPercent = owner.Stats.GetValue("CriPercent").ToFloat();
        float windCriPercent = owner.Stats.GetValue("WindCriPercent").ToFloat();
        DamageType damageType = DamageType.Skill;
        float damageValue = _damageValue;
        damageValue *= owner.Stats.GetValue("SkillDamage").ToFloat() * 0.01f + 1;
        damageValue *= owner.Stats.GetValue("WindDamage").ToFloat() * 0.01f + 1;

        RandomPuzzleItem();

        for (int i = 0; i < findTargets.Count; i++)
        {
            TargetTakeDamage(owner, findTargets[i], damageValue, criPercent + windCriPercent, damageType, SkillApplyDamageType.Wind);
            findTargets[i].SetHit();
        }
    }

    void RandomPuzzleItem()
    {
        int r = (int)UnityHelper.Random_H(0, 4);

        switch (r)
        {
            case 0:
                Managers.Observer.UIPuzzle.RandomSlash();
                break;
            case 1:
                Managers.Observer.UIPuzzle.RandomHellFire();
                break;
            case 2:
                Managers.Observer.UIPuzzle.RandomGas();
                break;
            case 3:
                Managers.Observer.UIPuzzle.RandomLightningSpawn();
                break;
        }
    }
}
