using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shared.DTOs.Table;
using Shared.Enums;
using UnityEngine;
using UnityEngine.UI;

public class RobbyShopPackage : UIRobby
{
    [SerializeField] private Scrollbar _scrollbar;
    private readonly string _packageCardPrefabPath = "Robby/Shop/ShopProductCard1";
    protected override void Initialize()
    {
		Bind<ResearchBGColor>(typeof(ResearchBGColorE));
		Bind<UIImage>(typeof(UIImageE));
		Bind<ShopProductPackageBox>(typeof(ShopProductPackageBoxE));
		Bind<ShopProductBox>(typeof(ShopProductBoxE));

        base.Initialize();
    }

    public override void OpenUISet(CanvasOrderType orderType)
    {
        base.OpenUISet(orderType);

		List<TableProductDto> productDatas = Managers.Table.GetTableData<List<TableProductDto>>().Where(p => p.ShopProductType == ShopProductType.PackageCard).ToList();
		PackageCardSet(productDatas);
    }

	private void PackageCardSet(List<TableProductDto> products)
	{
		List<ShopProductCardData> shopProductCardDatas = new List<ShopProductCardData>();
		for (int i = 0; i < products.Count; i++)
		{
			shopProductCardDatas.Add(new ShopProductCardData() { Product = products[i] });
		}

		Get<ShopProductBox>(ShopProductBoxE.SafeArea_ScrollView_Viewport_Content_NomalPackageBox)
		.UISet(shopProductCardDatas, _packageCardPrefabPath);
	}
    
	public enum ResearchBGColorE
    {
		SafeArea_RobbyBG,
    }
	public enum UIImageE
    {
		SafeArea_ScrollView,
		SafeArea_ScrollView_Viewport,
		SafeArea_ScrollView_ScrollbarHorizontal,
		SafeArea_ScrollView_ScrollbarHorizontal_SlidingArea_Handle,
		SafeArea_ScrollView_ScrollbarVertical,
		SafeArea_ScrollView_ScrollbarVertical_SlidingArea_Handle,
    }
	public enum ShopProductPackageBoxE
    {
		SafeArea_ScrollView_Viewport_Content_ShopProductPackageBox,
    }
	public enum ShopProductBoxE
    {
		SafeArea_ScrollView_Viewport_Content_NomalPackageBox,
    }
}