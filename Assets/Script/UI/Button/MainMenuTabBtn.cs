using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTabBtn : UITabButton
{
	protected override void Initialize()
	{
		base.Initialize();
		Bind<UIImage>(typeof(UIImageE));
	}

    protected override void UISet()
    {
        base.UISet();
    }
    public enum UIImageE
    {
		Icon,
    }
}
