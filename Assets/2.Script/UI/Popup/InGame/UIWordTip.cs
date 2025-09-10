using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWordTip : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIButton>(typeof(UIButtonE));
		Bind<UIScrollView>(typeof(UIScrollViewE));

        GetButton(UIButtonE.BlackPannel).AddClickEvent((ped) => ClosePopupUI());

        base.Initialize();
    }

    private readonly string _cardPath = "InGame/WordTipCard";

    public void UISet(List<WordTip> wordTips)
    {
        List<ICardData> datas = new();

        for (int i = 0; i < wordTips.Count; i++)
	    {
		    WordTip wordTip = wordTips[i];
		    int index = i;
		    WordTipCardData cardData = new WordTipCardData()
			    { Title = wordTip.Title, Explain = wordTip.Explain, TitleColor = wordTip.TitleColor };
		    datas.Add(cardData);
	    }

        GetScrollView(UIScrollViewE.ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, _cardPath, datas, 0, 1, UIScrollViewLayoutStartCorner.Middle, 0, 35f);
    }
    public enum UIButtonE
    {
		BlackPannel,
    }
	public enum UIScrollViewE
    {
		ScrollView,
    }
}