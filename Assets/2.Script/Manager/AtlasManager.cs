using Shared.Enums;
using UnityEngine;
using UnityEngine.U2D;

public class AtlasManager
{
    private SpriteAtlas Button { get; set; }  
    private SpriteAtlas Item { get; set; }  
    private SpriteAtlas GradeBgEquip { get; set; }  
    private SpriteAtlas GradeBgItem { get; set; }  
    private SpriteAtlas GradeBgSkill { get; set; }  
    private SpriteAtlas FontCombo { get; set; }  
    private SpriteAtlas FontDamage { get; set; }  
    private SpriteAtlas ResearchBook { get; set; } 
    private SpriteAtlas EquipType { get; set; }  
    public void Initialize()
    {
        Button = Managers.Resources.Load<SpriteAtlas>("Atlas/Button");
        Item = Managers.Resources.Load<SpriteAtlas>("Atlas/Item");
        GradeBgEquip = Managers.Resources.Load<SpriteAtlas>("Atlas/GradeBgEquip");
        GradeBgItem = Managers.Resources.Load<SpriteAtlas>("Atlas/GradeBgItem");
        GradeBgSkill = Managers.Resources.Load<SpriteAtlas>("Atlas/GradeBgSkill");
        FontCombo = Managers.Resources.Load<SpriteAtlas>("Atlas/FontCombo");
        FontDamage = Managers.Resources.Load<SpriteAtlas>("Atlas/FontDamage");
        ResearchBook = Managers.Resources.Load<SpriteAtlas>("Atlas/ResearchBook");
        EquipType = Managers.Resources.Load<SpriteAtlas>("Atlas/EquipType");
    }

    public Sprite GetButton(string code) => Button.GetSprite(code);
    public Sprite GetItem(string item, bool isBig) => Item.GetSprite(isBig ? $"{item}_Big" : item.ToString());
    public Sprite GetSkillGradeBg(Grade grade) => GradeBgSkill.GetSprite($"{grade}");
    public Sprite GetEquipGradeBg(Grade grade) => GradeBgEquip.GetSprite($"{grade}");
    public Sprite GetItemGradeBg(Grade grade) => GradeBgItem.GetSprite($"{grade}");
    public Sprite GetFontCombo(string text) => FontCombo.GetSprite(text);
    public Sprite GetAlphabetNumber(string text) => FontDamage.GetSprite(text);
    public Sprite GetResearchBook(string code, bool isBig) => ResearchBook.GetSprite(isBig ? $"{code}" : $"{code}_S");
    public Sprite GetEquipType(EquipType equipType) => EquipType.GetSprite(equipType.ToString());
}
