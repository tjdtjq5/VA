using System.Collections;
using System.Collections.Generic;
using Shared.DTOs.Table;
using Shared.Enums;
using UnityEngine;

public class ShopProductCard : UICard
{
    protected override void Initialize()
    {
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIButton>(typeof(UIButtonE));

        GetButton(UIButtonE.Main_Button).AddClickEvent((ped) => OnClickButton());

        base.Initialize();
    }
    [SerializeField] private ContentSizeRectTransform _csrt;
    [SerializeField] private Sprite _adSprite;

    public override void Setting(ICardData data)
    {
        ShopProductCardData shopProductCardData = data as ShopProductCardData;

        if (shopProductCardData == null)
            return;

        UISet(shopProductCardData.Product);
    }

    public void UISet(TableProductDto product)
    {
        GetTextPro(UITextProE.Main_Button_Name).text = product.Name;
        GetImage(UIImageE.Main_Button_Icon).sprite = Managers.Atlas.GetProduct(product.Id);
        GetImage(UIImageE.Main_Button_Icon).SetNativeSize();

        SetPrice(product);
    }

    private void SetPrice(TableProductDto product)
    {
        PayType payType = product.PayType;

        GetImage(UIImageE.Main_Button_Price_Icon).gameObject.SetActive(true);

        switch (payType)
        {
            case PayType.Paid:
                GetImage(UIImageE.Main_Button_Price_Icon).gameObject.SetActive(false);
                GetTextPro(UITextProE.Main_Button_Price_Text).text = "0.0$";
                break;
            case PayType.Ad:
                GetImage(UIImageE.Main_Button_Price_Icon).sprite = _adSprite;
                GetImage(UIImageE.Main_Button_Price_Icon).SetNativeSize();
                GetTextPro(UITextProE.Main_Button_Price_Text).text = "";
                break;
            case PayType.Goods:
                GetImage(UIImageE.Main_Button_Price_Icon).sprite = Managers.Atlas.GetItem(product.PayGoodsCode, false);
                GetImage(UIImageE.Main_Button_Price_Icon).SetNativeSize();
                GetTextPro(UITextProE.Main_Button_Price_Text).text = product.PayGoodsCount.ToString();
                break;
        }

        _csrt.SetFitHorizontal();
    }

    private void OnClickButton()
    {

    }
	
	public enum UITextProE
    {
		Main_Button_Name,
		Main_Button_Price_Text,
    }
	public enum UIImageE
    {
		Main_Button_Icon,
		Main_Button_Price_Icon,
    }
	public enum UIButtonE
    {
		Main_Button,
    }
}
public class ShopProductCardData : ICardData
{
    public TableProductDto Product;
    public TableProductDto PreProduct;
}