using System;
using System.Collections;
using System.Collections.Generic;
using Shared.Enums;
using UnityEngine;

public class UIInGameLearnCard : UICard
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIButton>(typeof(UIButtonE));

		GetButton(UIButtonE.Button).AddClickEvent((ped) => OnClick());

        base.Initialize();
    }
    
    private readonly string _tooltipPrefab = "InGame/UIToolTip";

    private UIInGameLearnCardData _data;
    
    public override void Setting(ICardData data)
    {
	    _data = (UIInGameLearnCardData)data;

	    SetIcon(_data.SkillOrBuff.Icon);

	    Skill skill = _data.SkillOrBuff as Skill;
	    Buff buff = _data.SkillOrBuff as Buff;

	    if (skill)
	    {
		    SetBg(skill.Grade);
		    SetFill(0);
		    SetCount(0);
	    }
	    else if (buff)
	    {
		    SetBg(buff.buffGrade);
		    SetFill(0);
		    SetCount(buff.Count);
	    }
	    else
		    throw new InvalidCastException($"SkillOrBuff is invalid");
    }

    void SetBg(Grade grade)
    {
	    GetImage(UIImageE.Bg).sprite = Managers.Atlas.GetSkillGradeBg(grade);
    }
    void SetIcon(Sprite icon)
    {
	    GetImage(UIImageE.Icon).sprite = icon;
	    GetImage(UIImageE.Icon).SetNativeSize();
    }
    void SetFill(float value)
    {
	    GetImage(UIImageE.Bg_Fill).FillAmount = value;
    }
    void SetCount(int count)
    {
	    if (count <= 1)
	    {
		    GetImage(UIImageE.Count).gameObject.SetActive(false);
		    GetText(UITextE.Count_Text).text = "";
	    }
	    else
	    {
		    GetImage(UIImageE.Count).gameObject.SetActive(true);
		    GetText(UITextE.Count_Text).text = count.ToString();
	    }
    }

    void OnClick()
    {
	    UIToolTip uiToolTip = Managers.UI.ShopPopupUI<UIToolTip>(_tooltipPrefab, CanvasOrderType.Middle);
	    uiToolTip.UISet(_data.SkillOrBuff, GetImage(UIImageE.Bg).transform, TooltipArrow.Up);
    }
	public enum UIImageE
    {
		Bg,
		Bg_Fill,
		Icon,
		Count,
    }
	public enum UITextE
    {
		Count_Text,
    }
	public enum UIButtonE
    {
		Button,
    }
}
public class UIInGameLearnCardData : ICardData
{
	public IdentifiedObject SkillOrBuff;
}