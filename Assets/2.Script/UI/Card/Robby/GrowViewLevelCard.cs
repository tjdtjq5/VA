using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowViewLevelCard : UICard
{
    public Action<int> OnClickAction;

    private int _growLevel;

    protected override void Initialize()
    {
		Bind<UIButton>(typeof(UIButtonE));
		Bind<UITextPro>(typeof(UITextProE));

        GetButton(UIButtonE.Button).AddClickEvent((ped) => OnClick());

        base.Initialize();
    }
    public override void Setting(ICardData data)
    {
        GrowViewLevelCardData growViewLevelCardData = data as GrowViewLevelCardData;

        _growLevel = growViewLevelCardData.GrowLevel;
        
        GetTextPro(UITextProE.Text).text = $"{_growLevel}";
    }

    public void OnClick()
    {
        OnClickAction?.Invoke(_growLevel);
    }
	public enum UIButtonE
    {
		Button,
    }
	public enum UITextProE
    {
		Text,
    }
}

public class GrowViewLevelCardData : ICardData
{
    public int GrowLevel { get; set; }
}