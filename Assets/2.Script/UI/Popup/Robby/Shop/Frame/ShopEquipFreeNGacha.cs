using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using Shared.CSharp;
using Shared.DTOs.Player;
using Shared.Enums;
using Shared.Fomula;
using UnityEngine;

public class ShopEquipFreeNGacha : UIFrame
{
    [SerializeField] private GoodsPricePro _goodsPrice;
    private readonly string _exScript = "<color=#FFE545FF>{0}</color>회 내 반드시 <color=#AE2CC0FF>희귀</color> 장비 획득";
    private readonly string _equipGachaResultPopupPath = "Robby/UIGachaResult";

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIButton>(typeof(UIButtonE));
		Bind<UITextPro>(typeof(UITextProE));

        Managers.PlayerData.AddEventListen(typeof(PlayerCounterDto), ButtonSet);
        Managers.PlayerData.AddEventListen(typeof(PlayerCounterDto), SetCounter);
        Managers.PlayerData.AddEventListen(typeof(PlayerItemDto), SetGoodsPrice);

        GetButton(UIButtonE.Nomal_InfoButton).AddClickEvent((ped) => OnClickInfoButton());
        GetButton(UIButtonE.Nomal_FreeButton).AddClickEvent((ped) => OnClickFreeGachaButton());
        GetButton(UIButtonE.Nomal_AdButton).AddClickEvent((ped) => OnClickAdGachaButton());
        GetButton(UIButtonE.Nomal_GachaButton).AddClickEvent((ped) => OnClickNGachaButton());

        base.Initialize();
    }
    public void Set()
    {
        SetGoodsPrice(null);

        Managers.PlayerData.DbGets(typeof(PlayerCounterDto), () =>
        {
            SetCounter(null);
            ButtonSet(null);
        });
    }

    private void SetGoodsPrice(PlayerGetsData<object> data)
    {
        int keyCount = Managers.PlayerData.GetPlayerItemCount(GachaFomula.FreeNGachaNeedGoodsCode).ToInt();
        _goodsPrice.UISet(GachaFomula.FreeNGachaNeedGoodsCode);
        _goodsPrice.SetText($"{keyCount}/{1}");
    }

    private void SetCounter(PlayerGetsData<object> data)
    {
        long rareCount = Managers.PlayerData.GetPlayerCounterCount($"Gacha_Equip_FreeN_Grade_{GachaGrade.Rare}", PeriodType.Permanent);

        rareCount = GachaFomula.GachaGradeRareCount - rareCount;
        
        GetTextPro(UITextProE.Nomal_EX_Text).text = $"{CSharpHelper.Format_H(_exScript, rareCount)}";
    }

    private void ButtonSet(PlayerGetsData<object> data)
    {
        // 하루에 한번 무료 뽑기 가능
        long count = Managers.PlayerData.GetPlayerCounterCount($"Gacha_Equip_{GachaGroup.FreeEquipGacha_N}", PeriodType.Daily);

        if (count >= 1)
        {
            GetButton(UIButtonE.Nomal_FreeButton).gameObject.SetActive(false);
            GetButton(UIButtonE.Nomal_AdButton).gameObject.SetActive(true);
            GetButton(UIButtonE.Nomal_GachaButton).gameObject.SetActive(true);
        }
        else
        {
            GetButton(UIButtonE.Nomal_FreeButton).gameObject.SetActive(true);
            GetButton(UIButtonE.Nomal_AdButton).gameObject.SetActive(false);
            GetButton(UIButtonE.Nomal_GachaButton).gameObject.SetActive(false);
        }
    }

    private void OnClickInfoButton()
    {
    }
    private void OnClickFreeGachaButton()
    {
        PlayerEquipGachaRequest request = new PlayerEquipGachaRequest()
        {
            Count = 1,
        };

        Managers.Web.SendPostRequest<PlayerEquipGachaResponse>("player/gacha/free/n/free", request, (response) =>
        {
            Managers.PlayerData.DbUpdate(response.Datas);

            UIEquipGachaResult equipGachaResultPopup = Managers.UI.ShopPopupUI<UIEquipGachaResult>(_equipGachaResultPopupPath, CanvasOrderType.Top);
            equipGachaResultPopup.UISet(GachaGroup.FreeEquipGacha_N, response.Results);
        });
    }
    private void OnClickAdGachaButton()
    {
        PlayerEquipGachaRequest request = new PlayerEquipGachaRequest()
        {
            Count = 1,
        };

        Managers.Web.SendPostRequest<PlayerEquipGachaResponse>("player/gacha/free/n/ad", request, (response) =>
        {
            Managers.PlayerData.DbUpdate(response.Datas);

            UIEquipGachaResult equipGachaResultPopup = Managers.UI.ShopPopupUI<UIEquipGachaResult>(_equipGachaResultPopupPath, CanvasOrderType.Top);
            equipGachaResultPopup.UISet(GachaGroup.FreeEquipGacha_N, response.Results);
        });

    }
    private void OnClickNGachaButton()
    {
        // 열쇠 아이템이 10개 이하면 n개 만큼 뽑기 
        // 열쇠 아이템이 10개 초과면 10개 만큼 뽑기
        int keyCount = Managers.PlayerData.GetPlayerItemCount(GachaFomula.FreeNGachaNeedGoodsCode).ToInt();

        if (keyCount <= 0)
        {
            // 재화 부족
            return;
        }

        PlayerEquipGachaRequest request = new PlayerEquipGachaRequest();

        if (keyCount < 10)
        {
            request.Count = keyCount;
        }
        else
        {
            request.Count = 10;
        }

        Managers.Web.SendPostRequest<PlayerEquipGachaResponse>("player/gacha/free/n", request, (response) =>
        {
            Managers.PlayerData.DbUpdate(response.Datas);

            UIEquipGachaResult equipGachaResultPopup = Managers.UI.ShopPopupUI<UIEquipGachaResult>(_equipGachaResultPopupPath, CanvasOrderType.Top);
            equipGachaResultPopup.UISet(GachaGroup.FreeEquipGacha_N, response.Results);
        });
    }
    
	public enum UIImageE
    {
		Nomal_BG,
		Nomal_Icon,
		Nomal_EX,
    }
	public enum UIButtonE
    {
		Nomal_InfoButton,
		Nomal_AdButton,
		Nomal_GachaButton,
		Nomal_FreeButton,
    }
	public enum UITextProE
    {
		Nomal_Name,
		Nomal_EX_Text,
    }
}