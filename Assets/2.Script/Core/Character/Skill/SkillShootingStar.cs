using System.Collections.Generic;
using Shared.BBNumber;
using Unity.VisualScripting;
using UnityEngine;

public class SkillShootingStar : SkillBehaviourTime
{
    [SerializeField] private SkillTargetType targetType;
    [SerializeField, Range(0f, 1f)] private float _attackTimeValue;
    private readonly string _prefabPath = "Prefab/Effect/Skill/ShootingStar";
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

        owner.AddPageApplyCount(SkillApplyDamageType.ShootingStar);
        
        float criPercent = owner.Stats.GetValue("CriPercent").ToFloat();
        float shootingStarCriPercent = owner.Stats.GetValue("ShootingStarCriPercent").ToFloat();
        DamageType damageType = DamageType.Skill;
        float damageValue = _damageValue;
        damageValue *= owner.Stats.GetValue("SkillDamage").ToFloat() * 0.01f + 1;
        damageValue *= owner.Stats.GetValue("ShootingStarDamage").ToFloat() * 0.01f + 1;
        
        for (int i = 0; i < findTargets.Count; i++)
        {
            TargetTakeDamage(owner, findTargets[i], damageValue, criPercent + shootingStarCriPercent, damageType, SkillApplyDamageType.ShootingStar);
            findTargets[i].SetHit();
        }

        ChangeCrossPuzzle(owner);
    }

    void ChangeCrossPuzzle(Character owner)
    {
        float crossPuzzlePercent = owner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.ShootingStar_P) ? 100f : 50f;
        bool isCross = UnityHelper.IsApplyPercent(crossPuzzlePercent);

        if(isCross)
        {
            Managers.Observer.UIPuzzle.ChangeCrossPuzzle();
        }
    }
}
