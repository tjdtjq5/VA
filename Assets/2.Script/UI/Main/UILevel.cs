using System;
using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;
using UnityEngine.Serialization;

public class UILevel : UIFrame
{
    protected override void Initialize()
    {
		Bind<UISlider>(typeof(UISliderE));
		Bind<UITextPro>(typeof(UITextProE));

		_levelUpEffectAniController = levelUpEffectAnimator.Initialize();
		
        base.Initialize();
    }
    
    public Action<int> OnLevelChange;
    public bool IsLevelActionFlag { get; set; } = true;
    
    [SerializeField] private Animator levelUpEffectAnimator;
    
    private AniController _levelUpEffectAniController;
    private readonly int _playHash = Animator.StringToHash("Play");

    private List<BBNumber> MaxExps => new List<BBNumber>() { 100, 150, 200, 250, 300, 350, 400, 450, 500 };
    private const int MaxLevel = 10;
    private int _beforeLevel = 1;

    public void ChangeExp(BBNumber exp)
    {
	    int maxExpIndex = Mathf.Min(_beforeLevel - 1, MaxExps.Count - 1);
	    BBNumber maxExp = MaxExps[maxExpIndex];

	    for (int i = 0; i < _beforeLevel - 1; i++)
		    exp -= MaxExps[Mathf.Min(i, MaxExps.Count - 1)];
	    
	    bool isLevelUp = exp >= maxExp;
	    if (isLevelUp)
	    {
		    _beforeLevel++;
		    if(IsLevelActionFlag)
			    ChangeLevel(_beforeLevel);
	    }

	    if (_beforeLevel >= MaxLevel)
	    {
		    GetSlider(UISliderE.SafeArea_Exp).value = 1;
		    GetTextPro(UITextProE.SafeArea_Level).text = $"Lv. {MaxLevel}";
	    }
	    else
	    {
		    BBNumber remainExp = exp % maxExp;
		    float value = (remainExp / maxExp).ToFloat();
	    
		    UISlider slider = GetSlider(UISliderE.SafeArea_Exp);
		    float startValue = isLevelUp ? 0 : slider.value;
		    Managers.Tween.TweenSlider(GetSlider(UISliderE.SafeArea_Exp), startValue, value, 0.15f);
		    GetTextPro(UITextProE.SafeArea_Level).text = $"Lv. {_beforeLevel}";
	    }
    }

    void ChangeLevel(int level)
    {
	    _levelUpEffectAniController.SetTrigger(_playHash);
	    OnLevelChange?.Invoke(level);
    }

    public enum UISliderE
    {
		SafeArea_Exp,
    }
	public enum UITextProE
    {
		SafeArea_Level,
    }
}