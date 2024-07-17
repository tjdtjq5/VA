using EasyButtons;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Test : MonoBehaviour
{
    [UnityEngine.SerializeField] UIScrollView UIScrollView;

    private void Start()
    {
        T();
    }

    public void T()
    {
        UIScrollView.Setting("TestCard", 3);

        List<ICardData> cardDataList = new List<ICardData>();
        for (int i = 0; i < 100; i++)
        {
            TestCardData cardData = new TestCardData()
            {
                Id = i,
            };
            cardDataList.Add(cardData);
        }

        UIScrollView.View(cardDataList);
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
