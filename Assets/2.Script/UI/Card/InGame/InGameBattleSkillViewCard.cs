using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameBattleSkillViewCard : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));

        base.Initialize();
    }
    
    [SerializeField] private Image bgImage;
    [SerializeField] private ContentSizeRectTransform csrt;

    private readonly float _playTime = 1.6f;
    private float _playTimer;
    private bool _isPlay;
    
    public void UISet(Skill skill)
    {
        GetImage(UIImageE.Layout_Icon).sprite = skill.Icon;
        GetImage(UIImageE.Layout_Icon).SetNativeSize();
        GetText(UITextE.Layout_Text).text = skill.DisplayName;

        bgImage.color = GameDefine.GetColor(skill.Grade);
        csrt.SetFitHorizontal();
        
        this.RectTransform.sizeDelta = csrt.GetComponent<RectTransform>().sizeDelta;

        _playTimer = 0;
        _isPlay = true;
    }

    private void FixedUpdate()
    {
	    if (_isPlay)
	    {
		    _playTimer += Managers.Time.UnscaledTime;

		    if (_playTimer > _playTime)
		    {
			    _playTimer = 0;
			    _isPlay = false;
			    this.gameObject.SetActive(false);
		    }
	    }
    }

    public enum UIImageE
    {
		Layout_Icon,
		Outline,
    }
	public enum UITextE
    {
		Layout_Text,
    }
}