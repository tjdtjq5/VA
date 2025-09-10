using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowLevelSpeech : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIText>(typeof(UITextE));

        base.Initialize();
    }

    public void UISet(int level)
    {
        GetText(UITextE.Value).text = level.ToString();
    }
	public enum UITextE
    {
		Value,
    }
}