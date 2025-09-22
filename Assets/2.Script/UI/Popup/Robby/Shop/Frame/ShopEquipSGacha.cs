using System.Collections;
using System.Collections.Generic;
using Shared.Fomula;
using UnityEngine;

public class ShopEquipSGacha : UIFrame
{
    [SerializeField] private GoodsPrice _goodsPrice_ten;
    [SerializeField] private GoodsPrice _goodsPrice_one;
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIButton>(typeof(UIButtonE));

        base.Initialize();
    }

    public void Set()
    {
        SetGoodsPrice();
    }

    private void SetGoodsPrice()
    {
        _goodsPrice_ten.UISet(GachaFomula.SGachaNeedGoodsCode);
        _goodsPrice_one.UISet(GachaFomula.SGachaNeedGoodsCode);

        _goodsPrice_ten.SetCount(GachaFomula.SGachaGachaNeedGoodsCount * 10, false);
        _goodsPrice_one.SetCount(GachaFomula.SGachaGachaNeedGoodsCount, false);
    }
    
	public enum UIImageE
    {
		EquipGacha_BG,
		EquipGacha_Icon,
		EquipGacha_Tile,
		EquipGacha_EX1,
		EquipGacha_EX2,
    }
	public enum UITextProE
    {
		EquipGacha_Name,
		EquipGacha_EX1_Text,
		EquipGacha_EX2_Text,
    }
	public enum UIButtonE
    {
		EquipGacha_InfoButton,
		EquipGacha_OneGachaButton,
		EquipGacha_TenGachaButton,
    }
}