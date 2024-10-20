using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DealDamageAction : EffectAction
{
    [SerializeField]
    private float defaultDamage;
    [SerializeField]
    private float bonusDamageStatFactor;

    private BBNumber GetTotalDamage(Effect effect, Entity user, int stack, float scale)
    {
        var totalDamage = user.Stats.GetValue("Atk") * bonusDamageStatFactor;

        totalDamage *= scale;

        return totalDamage;
    }

    public override bool Apply(Effect effect, Entity user, Entity target, int level, int stack, float scale)
    {
        if (target.IsDead)
            return false;

        var totalDamage = GetTotalDamage(effect, user, stack, scale);
        bool isCri = false;

        if (user.IsPlayer)
        {
            float cri = user.Stats.GetValue("CriPercent").ToFloat();
            isCri = UnityHelper.IsApplyPercent(cri);
            if (isCri)
                totalDamage *= user.Stats.GetValue("CriDamage") + 1;

            float week = user.Stats.GetValue("WeekPercent").ToFloat();
            bool isWeek = UnityHelper.IsApplyPercent(cri);
            if (isWeek)
                totalDamage *= user.Stats.GetValue("WeekDamage") + 1;


            switch (effect.CodeName)
            {
                case "BasicAttackEffect":
                    totalDamage *= user.Stats.GetValue("BasicAttackDamage") + 1;
                    break;
                case "SkillAttackEffect":
                    totalDamage *= user.Stats.GetValue("SkillAttackDamage") + 1;
                    break;
                case "Tribe_Cat":
                    totalDamage *= user.Stats.GetValue("CatSkillDamage") + 1;
                    break;
                case "Tribe_Dragon":
                    totalDamage *= user.Stats.GetValue("DragonSkillDamage") + 1;
                    break;
                case "Tribe_Druid":
                    totalDamage *= user.Stats.GetValue("DruidSkillDamage") + 1;
                    break;
                case "Tribe_Pirate":
                    totalDamage *= user.Stats.GetValue("PirateSkillDamage") + 1;
                    break;
                case "Tribe_Robot":
                    totalDamage *= user.Stats.GetValue("RobotSkillDamage") + 1;
                    break;
                case "Tribe_Thief":
                    totalDamage *= user.Stats.GetValue("ThiefSkillDamage") + 1;
                    break;
            }

            if (target.IsBoss)
                totalDamage *= user.Stats.GetValue("BossMonsterDamage") + 1;
            else
                totalDamage *= user.Stats.GetValue("NomalMonsterDamage") + 1;

            switch (user.Tribe)
            {
                case Tribe.Cat:
                    totalDamage *= user.Stats.GetValue("CatDamage") + 1;
                    break;
                case Tribe.Dragon:
                    totalDamage *= user.Stats.GetValue("DragonDamage") + 1;
                    break;
                case Tribe.Druid:
                    totalDamage *= user.Stats.GetValue("DruidDamage") + 1;
                    break;
                case Tribe.Pirate:
                    totalDamage *= user.Stats.GetValue("PirateDamage") + 1;
                    break;
                case Tribe.Robot:
                    totalDamage *= user.Stats.GetValue("RobotDamage") + 1;
                    break;
                case Tribe.Thief:
                    totalDamage *= user.Stats.GetValue("ThiefDamage") + 1;
                    break;
            }

            switch (user.Job)
            {
                case CharacterJob.Dealer:
                    totalDamage *= user.Stats.GetValue("DealerDamage") + 1;
                    break;
                case CharacterJob.SubDealer:
                    totalDamage *= user.Stats.GetValue("SubDealerDamage") + 1;
                    break;
                case CharacterJob.Supporter:
                    totalDamage *= user.Stats.GetValue("SupporterDamage") + 1;
                    break;
            }

            totalDamage *= user.Stats.GetValue("FinalDamage") + 1;
        }

        target.TakeDamage(user, effect, totalDamage);

        Managers.FloatingText.DamageSpawn(target, effect, totalDamage, isCri);

        return true;
    }

    protected override IReadOnlyDictionary<string, string> GetStringsByKeyword(Effect effect)
    {
        var descriptionValuesByKeyword = new Dictionary<string, string>
        {
            ["bonusDamageStatFactor"] = (bonusDamageStatFactor * 100f).ToString() + "%",
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
            bonusDamageStatFactor = bonusDamageStatFactor,
        };
    }
}
