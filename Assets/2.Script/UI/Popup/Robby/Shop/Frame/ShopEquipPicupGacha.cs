using System;
using System.Collections;
using System.Collections.Generic;
using Shared.CSharp;
using Shared.DTOs.Player;
using Shared.Enums;
using Shared.Fomula;
using UnityEngine;

public class ShopEquipPicupGacha : UIFrame
{
    [SerializeField] private GoodsPrice _goodsPrice_ten;
    [SerializeField] private GoodsPrice _goodsPrice_one;

    private readonly string _ex1Script = "<color=#FFE545FF>{0}</color>회 내 반드시 <color=#AE2CC0FF>영웅</color> 장비 획득";
    private readonly string _ex2Script = "<color=#FFE545FF>{0}</color>회 내 반드시 <color=#AE2CC0FF>픽업</color> 장비 획득";

    protected override void Initialize()
    {
        Bind<TimeFlowGachaPicup>(typeof(TimeFlowGachaPicupE));
        Bind<UIImage>(typeof(UIImageE));
        Bind<UITextPro>(typeof(UITextProE));
        Bind<UIButton>(typeof(UIButtonE));

        Get<TimeFlowGachaPicup>(TimeFlowGachaPicupE.RemainTime).OnTimeEnd -= Set;
        Get<TimeFlowGachaPicup>(TimeFlowGachaPicupE.RemainTime).OnTimeEnd += Set;

        Managers.PlayerData.AddEventListen(typeof(PlayerCounterDto), SetCounter);

        base.Initialize();
    }

    public void Set()
    {
        SetGoodsPrice();
        SetRemainTime();

        Managers.PlayerData.DbGets(typeof(PlayerCounterDto), () =>
        {
            SetCounter(null);
        });
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

    private void SetCounter(PlayerGetsData<object> data)
    {
        long picupRareCount = Managers.PlayerData.GetPlayerCounterCount($"Gacha_Equip_Picup_Grade_{GachaGrade.Rare}", PeriodType.Permanent);
        long picupUniqueCount = Managers.PlayerData.GetPlayerCounterCount($"Gacha_Equip_Picup_Grade_{GachaGrade.Unique}", PeriodType.Permanent);

        picupRareCount = GachaFomula.GachaGradeRareCount - picupRareCount;
        picupUniqueCount = GachaFomula.GachaGradeUniqueCount - picupUniqueCount;
        
        GetTextPro(UITextProE.EquipGacha_EX1_Text).text = $"{CSharpHelper.Format_H(_ex1Script, picupRareCount)}";
        GetTextPro(UITextProE.EquipGacha_EX2_Text).text = $"{CSharpHelper.Format_H(_ex2Script, picupUniqueCount)}";
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