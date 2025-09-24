using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shared.DTOs.Table;
using Shared.Enums;
using UnityEngine;
using UnityEngine.UI;

public class RobbyShopGacha : UIRobby
{
    protected override void Initialize()
    {
		Bind<ResearchBGColor>(typeof(ResearchBGColorE));
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<ShopEquipPicupGacha>(typeof(ShopEquipPicupGachaE));
		Bind<ShopEquipSGacha>(typeof(ShopEquipSGachaE));
		Bind<ShopEquipFreeNGacha>(typeof(ShopEquipFreeNGachaE));
		Bind<ShopEquipFreeSGacha>(typeof(ShopEquipFreeSGachaE));
		Bind<ShopProductBox>(typeof(ShopProductBoxE));


        base.Initialize();
    }

	[SerializeField] private Scrollbar _scrollbar;
    private readonly string _diamondPrefabPath = "Robby/Shop/ShopProductCard1";

    public override void OpenUISet(CanvasOrderType orderType)
    {
		_scrollbar.onValueChanged.AddListener(OnScrollValueChanged);
		// 
        base.OpenUISet(orderType);

		List<TableProductDto> productDatas = Managers.Table.GetTableData<List<TableProductDto>>().Where(p => p.ShopProductType == ShopProductType.Diamond).ToList();
		DiamondSet(productDatas);

		PicupGachaSet();
		SGachaSet();
		FreeNGachaSet();
		FreeSGachaSet();

		StartCoroutine(OpenUISetLaterCoroutine());
    }

	IEnumerator OpenUISetLaterCoroutine()
	{
		yield return null;
		_scrollbar.value = 1;
	}

	private void PicupGachaSet()
	{
		Get<ShopEquipPicupGacha>(ShopEquipPicupGachaE.SafeArea_ScrollView_Viewport_Content_Gacha_Gacha_PickUPGacha).Set();
	}
	private void SGachaSet()
	{
		Get<ShopEquipSGacha>(ShopEquipSGachaE.SafeArea_ScrollView_Viewport_Content_Gacha_Gacha_SGacha).Set();
	}
	private void FreeNGachaSet()
	{
		Get<ShopEquipFreeNGacha>(ShopEquipFreeNGachaE.SafeArea_ScrollView_Viewport_Content_FreeGacha_Gacha_FreeN).Set();
	}
	private void FreeSGachaSet()
	{
		Get<ShopEquipFreeSGacha>(ShopEquipFreeSGachaE.SafeArea_ScrollView_Viewport_Content_FreeGacha_Gacha_FreeS).Set();
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
		SafeArea_ScrollView,
		SafeArea_ScrollView_Viewport,
		SafeArea_ScrollView_Viewport_Content_Gacha_SubTitle,
		SafeArea_ScrollView_Viewport_Content_FreeGacha_SubTitle,
    }
	public enum UITextProE
    {
		SafeArea_ScrollView_Viewport_Content_Gacha_SubTitle_Text,
		SafeArea_ScrollView_Viewport_Content_FreeGacha_SubTitle_Text,
    }
	public enum ShopEquipPicupGachaE
    {
		SafeArea_ScrollView_Viewport_Content_Gacha_Gacha_PickUPGacha,
    }
	public enum ShopEquipSGachaE
    {
		SafeArea_ScrollView_Viewport_Content_Gacha_Gacha_SGacha,
    }
	public enum ShopEquipFreeNGachaE
    {
		SafeArea_ScrollView_Viewport_Content_FreeGacha_Gacha_FreeN,
    }
	public enum ShopEquipFreeSGachaE
    {
		SafeArea_ScrollView_Viewport_Content_FreeGacha_Gacha_FreeS,
    }
	public enum ShopProductBoxE
    {
		SafeArea_ScrollView_Viewport_Content_Diamond,
    }
}