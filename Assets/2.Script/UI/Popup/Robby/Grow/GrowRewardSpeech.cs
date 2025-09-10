using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowRewardSpeech : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));

        base.Initialize();
    }

    public void UISet(ItemValue itemValue)
    {
        GetImage(UIImageE.Icon).sprite = itemValue.item.Icon;
        GetImage(UIImageE.Icon).SetNativeSize();
        GetText(UITextE.Value).text = itemValue.ToValueString();
    }
	public enum UIImageE
    {
		Arrow,
		Icon,
    }
	public enum UITextE
    {
		Value,
    }
}