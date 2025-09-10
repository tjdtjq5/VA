using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D;

public class Test : MonoBehaviour
{
    #if UNITY_EDITOR

    [Button]
    public void PushSkill(Skill skill)
    {
        Managers.Observer.Player.CharacterSkill.PushSkill(skill);
    }

    [Button]
    public void PushEnemyDebuff(Buff deBuff)
    {
        List<Character> enemies = Managers.Observer.PuzzleBattleStateMachine.Enemies;
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].CharacterBuff.PushBuff(Managers.Observer.Player, deBuff);
        }
    }
    [Button]
    public void OpenShop()
    {
        Managers.Observer.Gesso += 1000;
        UIInGameShop uiInGameShop = Managers.UI.ShopPopupUI<UIInGameShop>("InGame/UIInGameShop", CanvasOrderType.Middle); 
    }
    [Button]
    public void Reset()
    {
        Managers.Scene.LoadScene(SceneType.InGame);
    }
    [Button]
    public void OpenSkill()
    {
        UIInGameSkill uiInGameSkill = Managers.UI.ShopPopupUI<UIInGameSkill>("InGame/UIInGameSkill", CanvasOrderType.Middle); 
        uiInGameSkill.UISet(UIInGameSkillType.Special);
    }

    [Button]
    public void ChangeLanguage(Language language)
    {
        Managers.Language.ChangeLanguage(language);
    }

    [Button]
    public void TakeDamage()
    {
        Managers.Observer.Player.TakeDamage(Managers.Observer.Player, null, 100, DamageType.None, CriticalType.None);
    }
    [Button]
    public void HpRecovery()
    {
        Managers.Observer.Player.HpRecovery(0.1f);
    }
    [Button]
    public void AddShield()
    {
        Managers.Observer.Player.Stats.shieldStat.DefaultValue += 500;
    }
    [Button]
    public void RemoveShield()
    {
        Managers.Observer.Player.Stats.shieldStat.DefaultValue -= 500;
    }

    private readonly string _plusColor = "#362C2A";
    private readonly string _plusColor2 = "#FB7919";
    private readonly string _minusColor = "#F43E33";
    private readonly string _hpColor = "#8BFA99";
    private readonly string _shieldColor = "#8AD2FA";

    [Button]
    public void SkillImageChange()
    {
        List<Skill> skills = Resources.LoadAll<Skill>(DefinePath.SkillSOResourcesPath() + "/S").ToList();
        skills.AddRange(Resources.LoadAll<Skill>(DefinePath.SkillSOResourcesPath() + "/D"));
        SpriteAtlas spriteAtlas = Resources.Load<SpriteAtlas>("Atlas/Skill");

        for (int i = 0; i < skills.Count; i++)
        {
            Skill skill = skills[i];
            string skillName = skill.CodeName;
            skillName = skillName.Replace("+", "");
            Sprite sprite = spriteAtlas.GetSprite(skillName);
            if(sprite == null)
            {
                UnityHelper.Log_H($"{skillName} 스프라이트가 없습니다.");
                continue;
            }
            skill.Icon = sprite;

            UnityEditor.EditorUtility.SetDirty(skill);
        }

        UnityEditor.AssetDatabase.Refresh();
        UnityEditor.AssetDatabase.SaveAssets();
    }
    public void SkillConditionSetUp()
    {
        List<Skill> conditionSkills = new();
        conditionSkills.Add(Resources.Load<Skill>(DefinePath.SkillSOResourcesPath() + "/D/Momentum"));
        // conditionSkills.Add(Resources.Load<Skill>(DefinePath.SkillSOResourcesPath() + "/D/EnhancedContinuousAttack+"));

        Skill[] skills = Resources.LoadAll<Skill>(DefinePath.SkillSOResourcesPath() + "/D");

        List<string> changeSkillCodeNames = new();
        changeSkillCodeNames.Add("ContinuousAttackFireball");
        changeSkillCodeNames.Add("ContinuousAttackGasExplosion");
        changeSkillCodeNames.Add("ContinuousAttackIceSpike");
        changeSkillCodeNames.Add("ContinuousAttackLightning");
        changeSkillCodeNames.Add("ContinuousLife");
        changeSkillCodeNames.Add("ContinuousProtection");
        changeSkillCodeNames.Add("ContinuousSlash");
        changeSkillCodeNames.Add("EnhancedContinuousAttack");

        for (int i = 0; i < skills.Length; i++)
        {
            Skill skill = skills[i];

            if(changeSkillCodeNames.Contains(skill.CodeName))
            {
                for(int j = 0; j < conditionSkills.Count; j++)
                {
                    if (skill.ConditionSkills.Contains(conditionSkills[j]))
                        continue;

                    skill.ConditionSkills.Add(conditionSkills[j]);
                    UnityEditor.EditorUtility.SetDirty(skill);
                }
            }
        }

        UnityEditor.AssetDatabase.Refresh();
    }
    public void SkillDescriptionSetUp()
    {
        Skill[] skills = Resources.LoadAll<Skill>(DefinePath.SkillSOResourcesPath() + "/S");

        for (int i = 0; i < skills.Length; i++)
        {
            Skill skill = skills[i];

            // if(skill.description.Contains($"##"))
            // {
            //     skill.description = skill.description.Replace($"##", $"#");
            // }


            // if(skill.description.Contains(_minusColor))
            //     continue;

            string data = "스킬";
            if(skill.description.Contains(data))
            {
                skill.description = skill.description.Replace(data, $"마법");
                continue;
            }

            // data = "-$[b1.b.Percent]%";
            // if(skill.description.Contains(data))
            // {
            //     skill.description = skill.description.Replace(data, $"<color=#{_minusColor}>{data}</color>");
            //     continue;
            // }

            // data = "-$[b0.b.Percent]%";
            // if(skill.description.Contains(data))
            // {
            //     skill.description = skill.description.Replace(data, $"<color=#{_minusColor}>{data}</color>");
            //     continue;
            // }

            UnityEditor.EditorUtility.SetDirty(skill);
        }
        
        UnityEditor.AssetDatabase.Refresh();
    }
    public void SkillDescriptionImageSetUp()
    {
        Skill[] skills = Resources.LoadAll<Skill>(DefinePath.SkillSOResourcesPath());

        for (int i = 0; i < skills.Length; i++)
        {
            Skill skill = skills[i];

            string data = "1줄";
            string data2 = $"{data}<sprite name=\"Line\">";

            if(skill.description.Contains(data2))
                continue;

            if(skill.description.Contains(data))
            {
                skill.description = skill.description.Replace(data, data2);
                continue;
            }

            UnityEditor.EditorUtility.SetDirty(skill);
        }
        
        UnityEditor.AssetDatabase.Refresh();
    }
    [Button]
    public void WordTipSetUp()
    {
        Skill[] skills = Resources.LoadAll<Skill>(DefinePath.SkillSOResourcesPath());
        WordTip burnTip = Resources.Load<WordTip>("SO/WordTip/Tip_Burn");
        WordTip poisonTip = Resources.Load<WordTip>("SO/WordTip/Tip_Poison");
        WordTip focusTip = Resources.Load<WordTip>("SO/WordTip/Tip_Focus");
        WordTip fatalTip = Resources.Load<WordTip>("SO/WordTip/Tip_Fatal");
        WordTip ouiTip = Resources.Load<WordTip>("SO/WordTip/Tip_Oui");
        WordTip perfectTip = Resources.Load<WordTip>("SO/WordTip/Tip_Perfect");
        WordTip weaknessTip = Resources.Load<WordTip>("SO/WordTip/Tip_Weakness");
        WordTip slashTip = Resources.Load<WordTip>("SO/WordTip/Tip_Slash");
        WordTip lightningTip = Resources.Load<WordTip>("SO/WordTip/Tip_Lightning");
        WordTip hellfireTip = Resources.Load<WordTip>("SO/WordTip/Tip_HellFire");
        WordTip gasTip = Resources.Load<WordTip>("SO/WordTip/Tip_Gas");
        WordTip iceThornsTip = Resources.Load<WordTip>("SO/WordTip/Tip_IceThorn");
        WordTip sequenceTip = Resources.Load<WordTip>("SO/WordTip/Tip_Sequence");
        WordTip puzzleItemTip = Resources.Load<WordTip>("SO/WordTip/Tip_PuzzleItem");
        WordTip stunTip = Resources.Load<WordTip>("SO/WordTip/Tip_Stun");

        if(burnTip == null || poisonTip == null || focusTip == null || fatalTip == null || ouiTip == null || perfectTip == null || weaknessTip == null || slashTip == null || lightningTip == null || hellfireTip == null || gasTip == null || iceThornsTip == null || sequenceTip == null || puzzleItemTip == null || stunTip == null)
        {
            string nullTips = "";
            if (burnTip == null) nullTips += "burnTip ";
            if (poisonTip == null) nullTips += "poisonTip ";
            if (focusTip == null) nullTips += "focusTip ";
            if (fatalTip == null) nullTips += "fatalTip ";
            if (ouiTip == null) nullTips += "ouiTip ";
            if (perfectTip == null) nullTips += "perfectTip ";
            if (weaknessTip == null) nullTips += "weaknessTip ";
            if (slashTip == null) nullTips += "slashTip ";
            if (lightningTip == null) nullTips += "lightningTip ";
            if (hellfireTip == null) nullTips += "hellfireTip ";
            if (gasTip == null) nullTips += "gasTip ";
            if (iceThornsTip == null) nullTips += "iceThornsTip ";
            if (sequenceTip == null) nullTips += "sequenceTip ";
            if (puzzleItemTip == null) nullTips += "puzzleItemTip ";
            if (stunTip == null) nullTips += "stunTip";
            UnityHelper.Log_H(nullTips);
            return;
        }

        for (int i = 0; i < skills.Length; i++)
        {
            Skill skill = skills[i];

            // skill.wordTips.Clear();

            // if(skill.Description.Contains("퍼즐 아이템"))
            // {
            //     skill.wordTips.Add(puzzleItemTip);
            // }

            // if(skill.Description.Contains("참격"))
            // {
            //     skill.wordTips.Add(slashTip);
            // }

            // if(skill.Description.Contains("번개"))
            // {
            //     skill.wordTips.Add(lightningTip);
            // }

            // if(skill.Description.Contains("불덩이"))
            // {
            //     skill.wordTips.Add(hellfireTip);
            // }
            
            // if(skill.Description.Contains("얼음 가시"))
            // {
            //     skill.wordTips.Add(iceThornsTip);
            // }

            // if(skill.Description.Contains("가스 폭발"))
            // {
            //     skill.wordTips.Add(gasTip);
            // }

            // if(skill.Description.Contains("화상"))
            // {
            //     skill.wordTips.Add(burnTip);
            // }

            // if(skill.Description.Contains("중독"))
            // {
            //     skill.wordTips.Add(poisonTip);
            // }

            // if(skill.Description.Contains("퍼펙트"))
            // {
            //     skill.wordTips.Add(perfectTip);
            // }

            // if(skill.Description.Contains("약점"))
            // {
            //     skill.wordTips.Add(weaknessTip);
            // }

            // if(skill.Description.Contains("집중 공격"))
            // {
            //     skill.wordTips.Add(focusTip);
            // }

            // if(skill.Description.Contains("필살 공격"))
            // {
            //     skill.wordTips.Add(fatalTip);
            // }

            // if(skill.Description.Contains("최종 오의"))
            // {
            //     skill.wordTips.Add(ouiTip);
            // }

            // if(skill.Description.Contains("연속"))
            // {
            //     skill.wordTips.Add(sequenceTip);
            // }

            // if(skill.Description.Contains("기절"))
            // {
            //     skill.wordTips.Add(stunTip);
            // }
            UnityEditor.EditorUtility.SetDirty(skill);
        }
        
        UnityEditor.AssetDatabase.Refresh();
    }
    public void SkillList()
    {
        Skill[] skills = Resources.LoadAll<Skill>(DefinePath.SkillSOResourcesPath() + "/S");

        string codes = "";
        string displayNames = "";
        string descriptions = "";
        for (int i = 0; i < skills.Length; i++)
        {
            Skill skill = skills[i];

            string codeName = skill.CodeName;
            string displayName = skill.DisplayName;
            string description = skill.Description;

            codes += $"{codeName}\n";
            displayNames += $"{displayName}\n";
            descriptions += $"{description}\n";

            // UnityEditor.EditorUtility.SetDirty(skill);
        }

        UnityHelper.Log_H(codes);
        UnityHelper.Log_H(displayNames);
        UnityHelper.Log_H(descriptions);
        
        // UnityEditor.AssetDatabase.Refresh();
    }
    #endif
}