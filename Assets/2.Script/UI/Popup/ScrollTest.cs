using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class ScrollTest : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIScrollView>(typeof(UIScrollViewE));

        base.Initialize();
    }

    [Button]
    void Set(int selectIndex)
    {
	    List<ICardData> cardDatas = new List<ICardData>();
	    for (int i = 0; i < 1000; i++)
	    {
		    cardDatas.Add(new TestCardData() { Id = i});
	    }
	    
	    GetScrollView(UIScrollViewE.ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, "TestCard", cardDatas, selectIndex);
    }

	public enum UIScrollViewE
    {
		ScrollView,
    }
}