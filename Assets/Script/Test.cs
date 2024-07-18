using EasyButtons;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [UnityEngine.SerializeField] UIScrollView UIScrollView;

    [SerializeField]
    UIScrollViewLayoutStartAxis axis;
    [SerializeField]
    int dataCount;
    [SerializeField]
    int selectIndex;
    [SerializeField]
    int rowColumn;
    [SerializeField]
    UIScrollViewLayoutStartCorner corner;
    [SerializeField]
    Vector2 spacing;

    [Button]
    public void T()
    {
        List<ICardData> cardDataList = new List<ICardData>();
        for (int i = 0; i < dataCount; i++)
        {
            TestCardData cardData = new TestCardData()
            {
                Id = i,
            };
            cardDataList.Add(cardData);
        }

        UIScrollView.View(axis, "TestCard", cardDataList, selectIndex, rowColumn, corner, spacing.x, spacing.y);

        UIScrollView.ScrollBarHorizontalActive(false);
        UIScrollView.ScrollBarVerticalActive(false);
    }

    [Button]
    public void Create()
    {
        Managers.UI.ShopPopupUI<UI_Login>("UI_Login");
    }

    [Button]
    public void Delete()
    {
        Managers.UI.ClosePopupUI();
    }
}
[System.Serializable]
public class TestA
{
    public string itemCode { get; set; }
    public int itemType { get; set; }
    public string tipName { get; set; }
    public string tipName2 { get; set; }
}
