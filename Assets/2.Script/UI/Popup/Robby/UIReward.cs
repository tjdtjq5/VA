using System.Collections;
using System.Collections.Generic;
using Shared.DTOs.ETC;
using UnityEngine;

public class UIReward : UIPopup
{
    private readonly string _rewardCardPrefabPath = "Robby/RewardCard";

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIBaseScrollView>(typeof(UIBaseScrollViewE));
		Bind<UIButton>(typeof(UIButtonE));


        GetButton(UIButtonE.CloseButton).AddClickEvent((ped) => ClosePopupUI());

        base.Initialize();
    }

    public void UISet(List<RewardCardData> rewardCardDatas)
    {
        List<ICardData> cardDatas = new List<ICardData>();

        for (int i = 0; i < rewardCardDatas.Count; i++)
        {
            cardDatas.Add(rewardCardDatas[i]);
        }

        GetBaseScrollView(UIBaseScrollViewE.ScrollView).UISet(
            UIScrollViewLayoutStartAxis.Vertical, 
            _rewardCardPrefabPath, 
            cardDatas);

        GetBaseScrollView(UIBaseScrollViewE.ScrollView).PlayAniAll();
    }

    public void UISet(List<ItemCountData> rewardDatas)
    {
        List<RewardCardData> rewardCardDatas = new List<RewardCardData>();
        for (int i = 0; i < rewardDatas.Count; i++)
        {
            rewardCardDatas.Add(new RewardCardData() { Item = rewardDatas[i].ItemCode, Count = rewardDatas[i].Count });
        }

        UISet(rewardCardDatas);
    }
    
	public enum UIImageE
    {
		Background,
		Title,
    }
	public enum UITextE
    {
		Title_Text,
    }
	public enum UIBaseScrollViewE
    {
		ScrollView,
    }
	public enum UIButtonE
    {
		CloseButton,
    }
}