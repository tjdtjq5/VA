using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public class DebuffPoison : BuffBehaviour
{
    private readonly string prefabPath = "Prefab/Effect/Buff/DebuffPoison";
    private readonly string prefabUpgradePath = "Prefab/Effect/Buff/DebuffPoisonEvolution";
    private float GetDamageValue => (this.Buff.Count + 1) * 0.5f;

    private Character _useCharacter;
    private Character _takeCharacter;

    public override void OnStart(Character useCharacter, Character takeCharacter, object cause)
    {
        this._useCharacter = useCharacter;
        this._takeCharacter = takeCharacter;

        useCharacter.AddPageApplyCount(SkillApplyDamageType.Poison);
        useCharacter.OnPoisonAttack?.Invoke(useCharacter.GetPageApplyCount(SkillApplyDamageType.Poison));
        // Attack(takeCharacter);

        this.Buff.OnCountChange -= CountChange;
        this.Buff.OnCountChange += CountChange;
    }

    public override void OnEnd(Character useCharacter, Character takeCharacter, object cause)
    {
        // this.Buff?.OnCountChange?.Invoke(this.Buff.Count);
    }

    void CountChange(int buffCount)
    {
        Attack(_takeCharacter);
    }

    void Attack(Character useCharacter)
    {
        List<Character> findTargets = FindTarget(useCharacter, SkillTargetType.My);
        bool isUpgrade = useCharacter.IsOnTriggerPassiveBuff(TriggerPassiveBuff.PoisonEvolution);

        float criPercent = useCharacter.Stats.GetValue("CriPercent").ToFloat();
        float poisonCriPercent = useCharacter.Stats.GetValue("PoisonCriPercent").ToFloat();
        DamageType damageType = DamageType.Poison;

        float damageValue = GetDamageValue;
        damageValue *= useCharacter.Stats.GetValue("PoisonDamage").ToFloat() * 0.01f + 1;
        
        for (int i = 0; i < findTargets.Count; i++)
        {
            TargetTakeDamage(useCharacter, findTargets[i], damageValue, criPercent + poisonCriPercent, damageType, SkillApplyDamageType.Poison);

            findTargets[i].SetHit();
        
            Poolable pool = Managers.Resources.Instantiate<Poolable>(isUpgrade ? prefabUpgradePath : prefabPath);
            if(pool)
                pool.transform.position = findTargets[i].BodyBoneTr.position;
        }
    }
}
