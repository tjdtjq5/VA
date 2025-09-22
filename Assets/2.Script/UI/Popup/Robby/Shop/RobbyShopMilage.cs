using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Shared.DTOs.Table;
using Shared.Enums;

public class RobbyShopMilage : UIRobby
{
    protected override void Initialize()
    {
		Bind<ResearchBGColor>(typeof(ResearchBGColorE));
		Bind<ShopProductBox>(typeof(ShopProductBoxE));

        base.Initialize();
    }
    private readonly string _milagePrefabPath = "Robby/Shop/ShopProductCard1";

    public override void OpenUISet(CanvasOrderType orderType)
    {
        base.OpenUISet(orderType);

		List<TableProductDto> productDatas = Managers.Table.GetTableData<List<TableProductDto>>().Where(p => p.ShopProductType == ShopProductType.Milage).ToList();
		MilageSet(productDatas);
    }

	private void MilageSet(List<TableProductDto> products)
	{
		List<ShopProductCardData> shopProductCardDatas = new List<ShopProductCardData>();
		for (int i = 0; i < products.Count; i++)
		{
			shopProductCardDatas.Add(new ShopProductCardData() { Product = products[i] });
		}

		Get<ShopProductBox>(ShopProductBoxE.SafeArea_ScrollView_Viewport_Content_Milage)
		.UISet(shopProductCardDatas, _milagePrefabPath);
	}
	public enum ResearchBGColorE
    {
		SafeArea_RobbyBG,
    }
	public enum ShopProductBoxE
    {
		SafeArea_ScrollView_Viewport_Content_Milage,
    }
}