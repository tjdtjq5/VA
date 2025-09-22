using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.DTOs.Table;

public class ShopProductBox : UIFrame
{
    protected override void Initialize()
    {
        Bind<UIImage>(typeof(UIImageE));
        Bind<UITextPro>(typeof(UITextProE));
        Bind<UIScrollView>(typeof(UIScrollViewE));

        base.Initialize();
    }

    public void UISet(List<ShopProductCardData> products, string cardPrefabPath)
    {
        List<ICardData> cardDatas = new List<ICardData>();

        for (int i = 0; i < products.Count; i++)
        {
            cardDatas.Add(products[i]);
        }

        GetScrollView(UIScrollViewE.ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, cardPrefabPath, cardDatas, 0, 3, UIScrollViewLayoutStartCorner.Middle, 10, 10, 0 , 10);
    }

    public void EnableScrollRect(bool switchValue) => GetScrollView(UIScrollViewE.ScrollView).ScrollRect.enabled = switchValue;

    public void PlayAniAll() => GetScrollView(UIScrollViewE.ScrollView).PlayAniAll();
    
	public enum UIImageE
    {
		Title,
		Title_SubTitle,
    }
	public enum UITextProE
    {
		Title_SubTitle_Text,
    }
	public enum UIScrollViewE
    {
		ScrollView,
    }
}