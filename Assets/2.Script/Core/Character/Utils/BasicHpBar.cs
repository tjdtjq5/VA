using System;
using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

public class BasicHpBar : HpBar
{
	[SerializeField] private Transform blueTr;
	[SerializeField] private Transform sequenceTr;
	
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));

        base.Initialize();
    }

    public override void Initialize(BBNumber maxHp, BBNumber maxSequence)
    {
	    base.Initialize(maxHp, maxSequence);

	    OnHpChanged += HpChanged;
	    OnShieldChanged += ShieldChanged;
	    OnSequenceChanged += SequenceChanged;
	    
	    SetUIWhite(1);
	    SetUIGreen(1);
	    SetUIBlue(false, 0);
	    SetSequence(0, 0);
	    
	    sequenceTr.gameObject.SetActive(maxSequence > 0);
	    GetTextPro(UITextProE.Text).text = maxHp.Alphabet();
    }

    void HpChanged(BBNumber oldHp, BBNumber newHp)
    {
	    BBNumber newValueBb = newHp / MaxHp;
	    float newValue = newValueBb.ToFloat();
	    
	    BBNumber oldValueBb = oldHp / MaxHp;
	    float oldValue = oldValueBb.ToFloat();

	    Managers.Tween.TweenFillAmount(GetImage(UIImageE.Slider_GreenFill), oldValue, newValue, 0.3f)
		    .SetOnComplete(()=> Managers.Tween.TweenFillAmount(GetImage(UIImageE.Slider_WhiteFill), oldValue, newValue, 0.3f));
	    
	    GetTextPro(UITextProE.Text).text = newHp.Alphabet();

		ShieldChanged(Shield, Shield);
    }
    void ShieldChanged(BBNumber oldShield, BBNumber newShield)
    {
	    if (newShield <= 0)
	    {
		    SetUIBlue(false, 0);
		    return;
	    }
	    
	    BBNumber newValueBb = newShield / MaxHp;
	    float newValue = newValueBb.ToFloat();
	    
	    BBNumber hpValueBb = Hp / MaxHp;
	    float hpValue = hpValueBb.ToFloat();
	    
	    float sumValue = newValue + hpValue;
	    
	    if (sumValue > 1)
	    {
		    SetUIBlue(true, newValue);
	    }
	    else
	    {
		    SetUIBlue(false, sumValue);
	    }
    }
    void SequenceChanged(BBNumber oldSequence, BBNumber newSequence)
    {
	    BBNumber newValueBb = newSequence / MaxSequence;
	    float newValue = newValueBb.ToFloat();
	    
	    BBNumber oldValueBb = oldSequence / MaxSequence;
	    float oldValue = oldValueBb.ToFloat();

	    SetSequence(oldValue, newValue);
    }

    void SetUIWhite(float value)
    {
	    GetImage(UIImageE.Slider_WhiteFill).FillAmount = value;
    }
    void SetUIGreen(float value)
    {
	    GetImage(UIImageE.Slider_GreenFill).FillAmount = value;
    }
    void SetUIBlue(bool isLeft, float value)
    {
	    if (isLeft)
	    {
		    GetImage(UIImageE.Slider_BlueFill).Image.fillOrigin = 1;
		    blueTr.SetSiblingIndex(3);
	    }
	    else
	    {
		    GetImage(UIImageE.Slider_BlueFill).Image.fillOrigin = 0;
		    blueTr.SetSiblingIndex(1);
	    }
	    
	    GetImage(UIImageE.Slider_BlueFill).FillAmount = value;
    }
    void SetSequence(float oldValue, float newValue)
    {
	    if (oldValue >= newValue || newValue == 0)
	    {
		    GetImage(UIImageE.SequenceSlider_YellowFill).FillAmount = newValue;
	    }
	    else
	    {
		    Managers.Tween.TweenFillAmount(GetImage(UIImageE.SequenceSlider_YellowFill), oldValue, newValue, 0.3f);
	    }
    }

	public enum UIImageE
    {
		Slider_Fill,
		Slider_WhiteFill,
		Slider_GreenFill,
		Slider_BlueFill,
		Slider_Background,
		SequenceSlider_YellowFill,
    }
	public enum UITextProE
    {
		Text,
    }
}