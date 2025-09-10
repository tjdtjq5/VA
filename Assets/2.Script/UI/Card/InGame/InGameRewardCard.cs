using System;
using Shared.BBNumber;
using UnityEngine;

public class InGameRewardCard : UICard
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));


        base.Initialize();
    }
    private InGameRewardCardData _data;
    public override void Setting(ICardData data)
    {
	    _data = (InGameRewardCardData)data;

	    Setting(_data.Item, _data.Count, _data.IsBig, _data.IsAlphabet, _data.IconSize, _data.LightSize);
    }

    public void Setting(string itemCode, BBNumber count, bool isBig, bool isAlphabet, float iconScale, float lightSize)
    {
	    GetImage(UIImageE.Icon).sprite = Managers.Atlas.GetItem(itemCode, isBig);
	    GetImage(UIImageE.Icon).SetNativeSize();
	    GetTextPro(UITextProE.Name).text = isAlphabet ? count.Alphabet() : count.ToInt().ToString();
	    SetIconSize(iconScale);
	    SetLightSize(lightSize);
    }
    public void Setting(IdentifiedObject skillOrBuff)
    {
	    Skill skill = skillOrBuff as Skill;
	    Buff buff = skillOrBuff as Buff;

	    if (skill)
		    Setting(skill);
	    else if (buff)
		    Setting(buff);
	    else
		    throw new InvalidCastException($"SkillOrBuff is invalid");
    }
    public void Setting(Skill skill)
    {
	    GetImage(UIImageE.Icon).sprite = skill.Icon;
	    GetImage(UIImageE.Icon).SetNativeSize();

	    GetTextPro(UITextProE.Name).text = skill.Description;
    }
    public void Setting(Buff buff)
    {
	    GetImage(UIImageE.Icon).sprite = buff.Icon;
	    GetImage(UIImageE.Icon).SetNativeSize();

	    GetTextPro(UITextProE.Name).text = buff.Description;
    }

    public void SetIconSize(float size)
    {
	    GetImage(UIImageE.Icon).transform.localScale = new Vector2(size, size);
    }
    public void SetLightSize(float size)
    {
	    GetImage(UIImageE.Light).RectTransform.sizeDelta = new Vector2(size, size);
    }
	public enum UIImageE
    {
		Light,
		Icon,
    }
	public enum UITextProE
    {
		Name,
    }
}
public class InGameRewardCardData : ICardData
{
	public string Item;
	public BBNumber Count;
	public bool IsBig;
	public bool IsAlphabet;
	public float LightSize;
	public float IconSize;
}