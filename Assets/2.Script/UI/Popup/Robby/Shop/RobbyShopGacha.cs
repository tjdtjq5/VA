using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shared.DTOs.Table;
using Shared.Enums;
using UnityEngine;
using UnityEngine.UI;

public class RobbyShopGacha : UIRobby
{
	[SerializeField] private Scrollbar _scrollbar;
    private readonly string _diamondPrefabPath = "Robby/Shop/ShopProductCard1";

    protected override void Initialize()
    {
		Bind<ResearchBGColor>(typeof(ResearchBGColorE));
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<ShopEquipGacha>(typeof(ShopEquipGachaE));
		Bind<ShopFreeGacha>(typeof(ShopFreeGachaE));
		Bind<ShopProductBox>(typeof(ShopProductBoxE));

		_scrollbar.onValueChanged.AddListener(OnScrollValueChanged);

        base.Initialize();
    }

    public override void OpenUISet(CanvasOrderType orderType)
    {
        base.OpenUISet(orderType);

		List<TableProductDto> productDatas = Managers.Table.GetTableData<List<TableProductDto>>().Where(p => p.ShopProductType == ShopProductType.Diamond).ToList();
		DiamondSet(productDatas);

		StartCoroutine(OpenUISetLaterCoroutine());
    }

	IEnumerator OpenUISetLaterCoroutine()
	{
		yield return null;
		_scrollbar.value = 1;
	}

	private void DiamondSet(List<TableProductDto> products)
	{
		List<ShopProductCardData> shopProductCardDatas = new List<ShopProductCardData>();
		TableProductDto adProduct = null;
		for (int i = 0; i < products.Count; i++)
		{
			if (products[i].Id == 7)
			{
				adProduct = products[i];
				continue;
			}

			shopProductCardDatas.Add(new ShopProductCardData() { Product = products[i] });
		}
		shopProductCardDatas[0].PreProduct = adProduct;

		Get<ShopProductBox>(ShopProductBoxE.SafeArea_ScrollView_Viewport_Content_Diamond)
		.UISet(shopProductCardDatas, _diamondPrefabPath);

		Get<ShopProductBox>(ShopProductBoxE.SafeArea_ScrollView_Viewport_Content_Diamond).EnableScrollRect(false);
	}

	private void OnScrollValueChanged(float value)
	{
		if(value == 0)
		{
			Get<ShopProductBox>(ShopProductBoxE.SafeArea_ScrollView_Viewport_Content_Diamond).PlayAniAll();
		}
	}

    public enum ResearchBGColorE
    {
		SafeArea_RobbyBG,
    }
	public enum UIImageE
    {
		SafeArea_ScrollView_Viewport_Content_Gacha_SubTitle,
		SafeArea_ScrollView_Viewport_Content_Gacha_Gacha_PickUPGacha_RemainTime,
		SafeArea_ScrollView_Viewport_Content_FreeGacha_SubTitle,
    }
	public enum UITextProE
    {
		SafeArea_ScrollView_Viewport_Content_Gacha_SubTitle_Text,
		SafeArea_ScrollView_Viewport_Content_Gacha_Gacha_PickUPGacha_RemainTime_Text,
		SafeArea_ScrollView_Viewport_Content_FreeGacha_SubTitle_Text,
    }
	public enum ShopEquipGachaE
    {
		SafeArea_ScrollView_Viewport_Content_Gacha_Gacha_PickUPGacha_EquipGacha,
		SafeArea_ScrollView_Viewport_Content_Gacha_Gacha_NomalGacha_EquipGacha,
    }
	public enum ShopFreeGachaE
    {
		SafeArea_ScrollView_Viewport_Content_FreeGacha_Gacha_Nomal,
		SafeArea_ScrollView_Viewport_Content_FreeGacha_Gacha_Special,
    }
	public enum ShopProductBoxE
    {
		SafeArea_ScrollView_Viewport_Content_Diamond,
    }
}