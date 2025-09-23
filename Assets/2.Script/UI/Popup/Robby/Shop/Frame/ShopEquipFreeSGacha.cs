using System.Collections;
using System.Collections.Generic;
using Shared.CSharp;
using Shared.DTOs.Player;
using Shared.Enums;
using Shared.Fomula;
using UnityEngine;

public class ShopEquipFreeSGacha : UIFrame
{
    private readonly string _exScript = "<color=#FFE545FF>{0}</color>회 내 반드시 <color=#AE2CC0FF>영웅</color> 장비 획득";

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIButton>(typeof(UIButtonE));
		Bind<UITextPro>(typeof(UITextProE));

        Managers.PlayerData.AddEventListen(typeof(PlayerCounterDto), ButtonSet);
        Managers.PlayerData.AddEventListen(typeof(PlayerCounterDto), SetCounter);

        base.Initialize();
    }

    public void Set()
    {
        Managers.PlayerData.DbGets(typeof(PlayerCounterDto), () =>
        {
            SetCounter(null);
            ButtonSet(null);
        });
    }

    private void SetCounter(PlayerGetsData<object> data)
    {
        long rareCount = Managers.PlayerData.GetPlayerCounterCount($"Gacha_Equip_FreeS_Grade_{GachaGrade.Rare}", PeriodType.Permanent);

        rareCount = GachaFomula.GachaGradeRareCount - rareCount;
        
        GetTextPro(UITextProE.Special_EX_Text).text = $"{CSharpHelper.Format_H(_exScript, rareCount)}";
    }

    private void ButtonSet(PlayerGetsData<object> data)
    {
        // 하루에 한번 무료 뽑기 가능
        long count = Managers.PlayerData.GetPlayerCounterCount($"Gacha_Equip_{GachaGroup.FreeEquipGacha_S}", PeriodType.Permanent);

        if (count >= 1)
        {
            GetButton(UIButtonE.Special_FreeButton).gameObject.SetActive(false);
            GetButton(UIButtonE.Special_AdButton).gameObject.SetActive(true);
            GetButton(UIButtonE.Special_GachaButton).gameObject.SetActive(true);
        }
        else
        {
            GetButton(UIButtonE.Special_FreeButton).gameObject.SetActive(true);
            GetButton(UIButtonE.Special_AdButton).gameObject.SetActive(false);
            GetButton(UIButtonE.Special_GachaButton).gameObject.SetActive(false);
        }
    }
    
	public enum UIImageE
    {
		Special_BG,
		Special_Icon,
		Special_EX,
    }
	public enum UIButtonE
    {
		Special_InfoButton,
		Special_AdButton,
		Special_GachaButton,
		Special_FreeButton,
    }
	public enum UITextProE
    {
		Special_Name,
		Special_EX_Text,
    }
}