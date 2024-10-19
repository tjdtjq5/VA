using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabSliderBtn : UIButton
{
	protected override void Initialize()
	{
		base.Initialize();
		Bind<UIText>(typeof(UITextE));
	}

	public void Set(string name, float width)
	{
		GetText(UITextE.Text).text = name;
		this.RectTransform.sizeDelta = new Vector2 (width, this.RectTransform.sizeDelta.y);
    }

	public enum UITextE
    {
		Text,
    }
}
