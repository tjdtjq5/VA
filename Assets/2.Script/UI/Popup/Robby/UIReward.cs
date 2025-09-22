using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIReward : UIPopup
{
    private readonly string _rewardCardPrefabPath = "Robby/RewardCard";

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIScrollView>(typeof(UIScrollViewE));
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

        GetScrollView(UIScrollViewE.ScrollView).UISet(
            UIScrollViewLayoutStartAxis.Vertical, 
            _rewardCardPrefabPath, 
            cardDatas);

        GetScrollView(UIScrollViewE.ScrollView).PlayAniAll();
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
	public enum UIScrollViewE
    {
		ScrollView,
    }
	public enum UIButtonE
    {
		CloseButton,
    }
}