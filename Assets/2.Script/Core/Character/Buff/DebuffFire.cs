using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public class DebuffFire : BuffBehaviour
{
    private readonly string prefabPath = "Prefab/Effect/Skill/FireHit";
    private readonly float damageValue = 6f;
    private int _combustionCount = 3;
    
    public override void OnStart(Character useCharacter, Character takeCharacter, object cause)
    {
        useCharacter.AddPageApplyCount(SkillApplyDamageType.Burn);
        useCharacter.OnBurnAttack?.Invoke(useCharacter.GetPageApplyCount(SkillApplyDamageType.Burn));
        Effect(useCharacter, takeCharacter);

        this.Buff.OnCountChange -= CountChange;
        this.Buff.OnCountChange += CountChange;
    }
    public override void OnEnd(Character useCharacter, Character takeCharacter, object cause)
    {

    }
    void CountChange(int buffCount)
    {
        if (buffCount <= _combustionCount)
        {
            this.Buff.ForceEndBuff();
        }
    }
    void Effect(Character useCharacter, Character takeCharacter)
    {
        if (takeCharacter.IsNotDetect)
            return;

        Poolable pool = Managers.Resources.Instantiate<Poolable>(prefabPath);
        if(pool)
        {
            pool.transform.position = takeCharacter.RootBoneTr.position;
            Managers.Tween.TweenInvoke(0.4f).SetOnComplete(() =>
            {
                Attack(useCharacter, takeCharacter);
            });
        }
    }
    void Attack(Character useCharacter, Character takeCharacter)
    {
        if (takeCharacter.IsNotDetect)
            return;

        float criPercent = useCharacter.Stats.GetValue("CriPercent").ToFloat();
        float burnCriPercent = useCharacter.Stats.GetValue("BurnCriPercent").ToFloat();
        DamageType damageType = DamageType.Burn;
        float damageV = this.damageValue;
        damageV *= useCharacter.Stats.GetValue("BurnDamage").ToFloat() * 0.01f + 1;

        TargetTakeDamage(useCharacter, takeCharacter, damageV, criPercent + burnCriPercent, damageType, SkillApplyDamageType.Burn);

        takeCharacter.SetHit();
    }
}