using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using Shared.CSharp;
using Shared.DTOs.Player;
using Shared.Enums;
using Shared.Fomula;
using UnityEngine;

public class ShopEquipSGacha : UIFrame
{
    [SerializeField] private GoodsPrice _goodsPrice_ten;
    [SerializeField] private GoodsPrice _goodsPrice_one;

    private readonly string _ex1Script = "<color=#FFE545FF>{0}</color>회 내 반드시 <color=#AE2CC0FF>영웅</color> 장비 획득";
    private readonly string _ex2Script = "<color=#FFE545FF>{0}</color>회 내 반드시 <color=#AE2CC0FF>S급 영웅</color> 장비 획득";
    private readonly string _equipGachaResultPopupPath = "Robby/UIGachaResult";

    protected override void Initialize()
    {
        Bind<UIImage>(typeof(UIImageE));
        Bind<UITextPro>(typeof(UITextProE));
        Bind<UIButton>(typeof(UIButtonE));

        Managers.PlayerData.AddEventListen(typeof(PlayerCounterDto), SetCounter);
        Managers.PlayerData.AddEventListen(typeof(PlayerItemDto), SetGoodsPrice);

        GetButton(UIButtonE.EquipGacha_InfoButton).AddClickEvent((ped) => OnClickInfoButton());
        GetButton(UIButtonE.EquipGacha_OneGachaButton).AddClickEvent((ped) => OnClickOneGachaButton());
        GetButton(UIButtonE.EquipGacha_TenGachaButton).AddClickEvent((ped) => OnClickTenGachaButton());

        base.Initialize();
    }

    public void Set()
    {
        SetGoodsPrice(null);

        Managers.PlayerData.DbGets(typeof(PlayerCounterDto), () =>
        {
            SetCounter(null);
        });
    }

    private void SetGoodsPrice(PlayerGetsData<object> data)
    {
        int uniqueKeyCount = Managers.PlayerData.GetPlayerItemCount(GachaFomula.UniqueKeyGachaNeedGoodsCode).ToInt();
        if (uniqueKeyCount > 0)
        {
            _goodsPrice_one.UISet(GachaFomula.UniqueKeyGachaNeedGoodsCode);
            _goodsPrice_one.SetText($"{uniqueKeyCount}/{1}");
        }
        else
        {
            _goodsPrice_one.UISet(GachaFomula.SGachaNeedGoodsCode);
            _goodsPrice_one.SetCount(GachaFomula.SGachaGachaNeedGoodsCount, false);
        }

        if (uniqueKeyCount > 10)
        {
            _goodsPrice_ten.UISet(GachaFomula.UniqueKeyGachaNeedGoodsCode);
            _goodsPrice_ten.SetText($"{uniqueKeyCount}/{10}");
        }
        else
        {
            _goodsPrice_ten.UISet(GachaFomula.SGachaNeedGoodsCode);
            _goodsPrice_ten.SetCount(GachaFomula.SGachaGachaNeedGoodsCount * 10, false);
        }
    }
    private void SetCounter(PlayerGetsData<object> data)
    {
        long rareCount = Managers.PlayerData.GetPlayerCounterCount($"Gacha_Equip_S_Grade_{GachaGrade.Rare}", PeriodType.Permanent);
        long uniqueCount = Managers.PlayerData.GetPlayerCounterCount($"Gacha_Equip_S_Grade_{GachaGrade.Unique}", PeriodType.Permanent);

        rareCount = GachaFomula.GachaGradeRareCount - rareCount;
        uniqueCount = GachaFomula.GachaGradeUniqueCount - uniqueCount;
        
        GetTextPro(UITextProE.EquipGacha_EX1_Text).text = $"{CSharpHelper.Format_H(_ex1Script, rareCount)}";
        GetTextPro(UITextProE.EquipGacha_EX2_Text).text = $"{CSharpHelper.Format_H(_ex2Script, uniqueCount)}";
    }
    private void OnClickInfoButton()
    {
    }
    private void OnClickOneGachaButton()
    {
        PlayerEquipGachaRequest request = new PlayerEquipGachaRequest()
        {
            Count = 1,
        };

        Managers.Web.SendPostRequest<PlayerEquipGachaResponse>("player/gacha/s", request, (response) =>
        {
            Managers.PlayerData.DbUpdate(response.Datas);

            UIEquipGachaResult equipGachaResultPopup = Managers.UI.ShopPopupUI<UIEquipGachaResult>(_equipGachaResultPopupPath, CanvasOrderType.Top);
            equipGachaResultPopup.UISet(GachaGroup.EquipGachaS, response.Results);
        });
    }
    private void OnClickTenGachaButton()
    {
        PlayerEquipGachaRequest request = new PlayerEquipGachaRequest()
        {
            Count = 10,
        };

        Managers.Web.SendPostRequest<PlayerEquipGachaResponse>("player/gacha/s", request, (response) =>
        {
            Managers.PlayerData.DbUpdate(response.Datas);

            UIEquipGachaResult equipGachaResultPopup = Managers.UI.ShopPopupUI<UIEquipGachaResult>(_equipGachaResultPopupPath, CanvasOrderType.Top);
            equipGachaResultPopup.UISet(GachaGroup.EquipGachaS, response.Results);
        });
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