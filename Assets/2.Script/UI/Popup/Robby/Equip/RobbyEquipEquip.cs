using System;
using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using Shared.DTOs.Player;
using Shared.Enums;
using UnityEngine;

public class RobbyEquipEquip : UIRobby
{
    private readonly string _parentName = "Robby/Equip/RobbyEquip";
    private readonly string _smithyPopupName = "Robby/Equip/RobbyEquipSmithy";

    private readonly string equipedCardPath = "Robby/Equip/EquipCard";
    private readonly string equipItemSectorPath = "Robby/Equip/EquipItemSector";

    protected override void Initialize()
    {
		Bind<ResearchBGColor>(typeof(ResearchBGColorE));
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<EquipedCard>(typeof(EquipedCardE));
		Bind<UIPlayer>(typeof(UIPlayerE));
		Bind<UIButton>(typeof(UIButtonE));
		Bind<UIScrollViewWithAdd>(typeof(UIScrollViewWithAddE));

        GetButton(UIButtonE.SafeArea_SmithyButton).AddClickEvent((ped) => OnClickSmithyButton());

        base.Initialize();
    }

    public override void OpenUISet(CanvasOrderType orderType)
    {
        base.OpenUISet(orderType);

        InitSet();

        List<Type> types = new List<Type> { typeof(PlayerEquipDto) };
        Managers.PlayerData.DbGets(types, () =>
        {
            List<PlayerEquipDto> playerEquipDatas = Managers.PlayerData.GetPlayerData<List<PlayerEquipDto>>();
            List<PlayerEquipDto> equipedDatas = playerEquipDatas == null ? null : playerEquipDatas.FindAll(data => data.IsEquip);

            EquipSet(equipedDatas); 
            StatsSet(equipedDatas);
            ScrollViewSet(playerEquipDatas);
        });
    }

    private void InitSet()
    {
        EquipSet(null);
        StatsSet(null);
        UIPlayerSet();
        ScrollViewSet(null);
    }

    private void EquipSet(List<PlayerEquipDto> playerEquipDatas)
    {
        if (playerEquipDatas == null)
        {
            Get<EquipedCard>(EquipedCardE.SafeArea_Equipeds_WeaponEquiped).UISet(null, null);
            Get<EquipedCard>(EquipedCardE.SafeArea_Equipeds_ArmorEquiped).UISet(null, null);
            Get<EquipedCard>(EquipedCardE.SafeArea_Equipeds_GloveEquiped).UISet(null, null);
            Get<EquipedCard>(EquipedCardE.SafeArea_Equipeds_HatEquiped).UISet(null, null);
            Get<EquipedCard>(EquipedCardE.SafeArea_Equipeds_BeltEquiped).UISet(null, null);
            Get<EquipedCard>(EquipedCardE.SafeArea_Equipeds_ShoesEquiped).UISet(null, null);
            return;
        }

        foreach (var data in playerEquipDatas)
        {
            bool isEquiped = data.IsEquip;
            if (!isEquiped)
                continue;

            string code = data.Code;
            Equip equip = Managers.SO.GetEquip(code);
            EquipType equipType = equip.Type;

            switch (equipType)
            {
                case EquipType.Weapon:
                    EquipedCard weaponEquipedCard = Get<EquipedCard>(EquipedCardE.SafeArea_Equipeds_WeaponEquiped);
                    weaponEquipedCard.UISet(data, equip);
                    break;
                case EquipType.Armor:
                    EquipedCard armorEquipedCard = Get<EquipedCard>(EquipedCardE.SafeArea_Equipeds_ArmorEquiped);
                    armorEquipedCard.UISet(data, equip);
                    break;
                case EquipType.Gloves:
                    EquipedCard glovesEquipedCard = Get<EquipedCard>(EquipedCardE.SafeArea_Equipeds_GloveEquiped);
                    glovesEquipedCard.UISet(data, equip);
                    break;
                case EquipType.Hat:
                    EquipedCard hatEquipedCard = Get<EquipedCard>(EquipedCardE.SafeArea_Equipeds_HatEquiped);
                    hatEquipedCard.UISet(data, equip);
                    break;
                case EquipType.Belt:
                    EquipedCard beltEquipedCard = Get<EquipedCard>(EquipedCardE.SafeArea_Equipeds_BeltEquiped);
                    beltEquipedCard.UISet(data, equip);
                    break;
                case EquipType.Boots:
                    EquipedCard shoesEquipedCard = Get<EquipedCard>(EquipedCardE.SafeArea_Equipeds_ShoesEquiped);
                    shoesEquipedCard.UISet(data, equip);
                    break;
            }
        }
    }
    private void StatsSet(List<PlayerEquipDto> playerEquipDatas)
    {
        if (playerEquipDatas == null)
        {
            Get<UITextPro>(UITextProE.SafeArea_Stats_Atk_Text).text = $"0";
            Get<UITextPro>(UITextProE.SafeArea_Stats_HP_Text).text = $"0";
            return;
        }

        BBNumber atk = 0;
        BBNumber hp = 0;

        string atkCode = "Atk";
        string hpCode = "Hp";

        foreach (var data in playerEquipDatas)
        {
            Equip equip = Managers.SO.GetEquip(data.Code);

            atk += equip.GetStatValue(atkCode, data.Level);
            hp += equip.GetStatValue(hpCode, data.Level);
        }

        Get<UITextPro>(UITextProE.SafeArea_Stats_Atk_Text).text = $"{atk.Alphabet()}";
        Get<UITextPro>(UITextProE.SafeArea_Stats_HP_Text).text = $"{hp.Alphabet()}";
    }
    private void UIPlayerSet()
    {
        Get<UIPlayer>(UIPlayerE.SafeArea_UIPlayer).SetForm(PuzzleType.None);
        Get<UIPlayer>(UIPlayerE.SafeArea_UIPlayer).SetIdle(AttackGrade.Basic);
    }
    private void ScrollViewSet(List<PlayerEquipDto> playerEquipDatas)
    {
        List<ICardData> cardDatas = new List<ICardData>();

        if (playerEquipDatas != null)
        {
            for (int i = 0; i < playerEquipDatas.Count; i++)
            {
                PlayerEquipDto data = playerEquipDatas[i];
                Equip equip = Managers.SO.GetEquip(data.Code);
                EquipCardData cardData = new EquipCardData(data, equip);
                cardDatas.Add(cardData);
            }
        }

        // List<ICardData> materialCardDatas = new();
        // materialCardDatas.AddRange(cardDatas);

        // 1) 첫 섹션 세팅
        Get<UIScrollViewWithAdd>(UIScrollViewWithAddE.SafeArea_ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, equipedCardPath, cardDatas, 0, 5, UIScrollViewLayoutStartCorner.Left, 10, 10, 10, 10);
        // 2) 구분선 추가 (프리팹 이름, 높이 픽셀)
        // Get<UIScrollViewWithAdd>(UIScrollViewWithAddE.SafeArea_ScrollView).AddSeparator(equipItemSectorPath, 64f);
        // 3) 다음 섹션 추가 (프리팹, 데이터, 열수, 간격X, 간격Y)
        // Get<UIScrollViewWithAdd>(UIScrollViewWithAddE.SafeArea_ScrollView).AddData(equipedCardPath2, materialCardDatas, 5, 10, 10);
    }
    private void OnClickSmithyButton()
    {
        Managers.Observer.RobbyManager.ShopUI(_smithyPopupName, _parentName);
    }
    public enum ResearchBGColorE
    {
		SafeArea_RobbyBG,
    }
	public enum UIImageE
    {
		SafeArea_Title,
		SafeArea_Stats_Atk_Icon,
		SafeArea_Stats_HP_Icon,
    }
	public enum UITextProE
    {
		SafeArea_Title_Text,
		SafeArea_Stats_Atk_Text,
		SafeArea_Stats_HP_Text,
    }
	public enum EquipedCardE
    {
		SafeArea_Equipeds_WeaponEquiped,
		SafeArea_Equipeds_ArmorEquiped,
		SafeArea_Equipeds_GloveEquiped,
		SafeArea_Equipeds_HatEquiped,
		SafeArea_Equipeds_BeltEquiped,
		SafeArea_Equipeds_ShoesEquiped,
    }
	public enum UIPlayerE
    {
		SafeArea_UIPlayer,
    }
	public enum UIButtonE
    {
		SafeArea_ArrayButton,
		SafeArea_SmithyButton,
    }
	public enum UIScrollViewWithAddE
    {
		SafeArea_ScrollView,
    }
}