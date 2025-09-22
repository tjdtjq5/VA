using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

public class RewardCard : UICard
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));

        base.Initialize();
    }
    public override void Setting(ICardData data)
    {
        RewardCardData rewardCardData = data as RewardCardData;

        if (rewardCardData == null)
            return;

        UISet(rewardCardData.Item, rewardCardData.Count);
    }

    public void UISet(string itemCode, BBNumber count)
    {
        Item item = Managers.SO.GetItem(itemCode);

        GetImage(UIImageE.Main_BG).sprite = Managers.Atlas.GetItemGradeBg(item.Grade);
        GetImage(UIImageE.Main_Icon).sprite = item.Icon;
        GetImage(UIImageE.Main_Icon).SetNativeSize();
        GetTextPro(UITextProE.Main_Count).text = $"X{item.ToValueString(count)}";
    }
	public enum UIImageE
    {
		Main_BG,
		Main_Icon,
    }
	public enum UITextProE
    {
		Main_Count,
    }
}

public class RewardCardData : ICardData
{
    public string Item;
    public BBNumber Count;
}