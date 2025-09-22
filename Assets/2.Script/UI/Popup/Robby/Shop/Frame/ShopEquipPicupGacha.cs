using System;
using System.Collections;
using System.Collections.Generic;
using Shared.Fomula;
using UnityEngine;

public class ShopEquipPicupGacha : UIFrame
{
    [SerializeField] private GoodsPrice _goodsPrice_ten;
    [SerializeField] private GoodsPrice _goodsPrice_one;

    protected override void Initialize()
    {
        Bind<TimeFlowGachaPicup>(typeof(TimeFlowGachaPicupE));
        Bind<UIImage>(typeof(UIImageE));
        Bind<UITextPro>(typeof(UITextProE));
        Bind<UIButton>(typeof(UIButtonE));

        Get<TimeFlowGachaPicup>(TimeFlowGachaPicupE.RemainTime).OnTimeEnd -= Set;
        Get<TimeFlowGachaPicup>(TimeFlowGachaPicupE.RemainTime).OnTimeEnd += Set;

        base.Initialize();
    }

    public void Set()
    {
        SetGoodsPrice();
        SetRemainTime();
    }

    private void SetGoodsPrice()
    {
        _goodsPrice_ten.UISet(GachaFomula.PickUpGachaNeedGoodsCode);
        _goodsPrice_one.UISet(GachaFomula.PickUpGachaNeedGoodsCode);

        _goodsPrice_ten.SetCount(GachaFomula.PickUpGachaNeedGoodsCount * 10, false);
        _goodsPrice_one.SetCount(GachaFomula.PickUpGachaNeedGoodsCount, false);
    }

    private void SetRemainTime()
    {
        TimeSpan remainTime = GachaFomula.GetPicupRemainTime(Managers.Time.Current);
        Get<TimeFlowGachaPicup>(TimeFlowGachaPicupE.RemainTime).UISet(remainTime, true);
    }

    public enum TimeFlowGachaPicupE
    {
        RemainTime,
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