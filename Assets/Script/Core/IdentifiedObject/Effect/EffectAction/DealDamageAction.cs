using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DealDamageAction : EffectAction
{
    [SerializeField]
    private float defaultDamage;
    [SerializeField]
    private Stat bonusDamageStat;
    [SerializeField]
    private float bonusDamageStatFactor;
    [SerializeField]
    private float bonusDamagePerLevel;
    [SerializeField]
    private float bonusDamagePerStack;

    private BBNumber GetDefaultDamage(Effect effect)
        => defaultDamage + (effect.DataBonusLevel * bonusDamagePerLevel);

    private BBNumber GetStackDamage(int stack)
        => (stack - 1) * bonusDamagePerStack;

    private BBNumber GetBonusStatDamage(Entity user)
        => user.Stats.GetValue(bonusDamageStat) * bonusDamageStatFactor;

    private BBNumber GetTotalDamage(Effect effect, Entity user, int stack, float scale)
    {
        var totalDamage = GetDefaultDamage(effect) + GetStackDamage(stack);
        if (bonusDamageStat) 
            totalDamage += GetBonusStatDamage(user);

        totalDamage *= scale;

        return totalDamage;
    }

    public override bool Apply(Effect effect, Entity user, Entity target, int level, int stack, float scale)
    {
        if (target.IsDead)
            return false;

        var totalDamage = GetTotalDamage(effect, user, stack, scale);
        target.TakeDamage(user, effect, totalDamage);

        Managers.FloatingText.DamageSpawn(target, effect, totalDamage, false);

        return true;
    }

    protected override IReadOnlyDictionary<string, string> GetStringsByKeyword(Effect effect)
    {
        var descriptionValuesByKeyword = new Dictionary<string, string>
        {
            ["defaultDamage"] = GetDefaultDamage(effect).ToString(".##"),
            ["bonusDamageStat"] = bonusDamageStat?.DisplayName ?? string.Empty,
            ["bonusDamageStatFactor"] = (bonusDamageStatFactor * 100f).ToString() + "%",
            ["bonusDamagePerLevel"] = bonusDamagePerLevel.ToString(),
            ["bonusDamagePerStack"] = bonusDamagePerStack.ToString(),
        };

        if (effect.User)
        {
            descriptionValuesByKeyword["totalDamage"] =
                GetTotalDamage(effect, effect.User, effect.CurrentStack, effect.Scale).ToString(".##");
        }

        return descriptionValuesByKeyword;
    }

    public override object Clone()
    {
        return new DealDamageAction()
        {
            defaultDamage = defaultDamage,
            bonusDamageStat = bonusDamageStat,
            bonusDamageStatFactor = bonusDamageStatFactor,
            bonusDamagePerLevel = bonusDamagePerLevel,
            bonusDamagePerStack = bonusDamagePerStack
        };
    }
}
