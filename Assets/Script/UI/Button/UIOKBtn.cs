using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOKBtn : UIButton
{
	protected override void Initialize()
	{
		base.Initialize();
		Bind<UIText>(typeof(UITextE));
	}

    protected override void UISet()
    {
        base.UISet();

        GetText(UITextE.Text).text = $"»Æ¿Œ";
    }

    public enum UITextE
    {
		Text,
    }
}
