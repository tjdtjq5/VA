using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterInfoPopup : UIPopup
{
	[SerializeField] Transform main;

	UIPopup currentPopup;

    protected override void Initialize()
    {
		Bind<UIButton>(typeof(UIButtonE));
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIText>(typeof(UITextE));
		Bind<UITabSlider>(typeof(UITabSliderE));

        base.Initialize();
    }

    protected override void UISet()
    {
        base.UISet();

		GetButton(UIButtonE.Dim).AddClickEvent(Close);
		GetButton(UIButtonE.Main_PopupBg_Close).AddClickEvent(Close);
		GetButton(UIButtonE.Main_SpecialWeapon).AddClickEvent(SpecialWeaponAction);

		GetTabSlider(UITabSliderE.Main_TabSlider).TabHandler += TabAction;

        int tabIndex = GetTabSlider(UITabSliderE.Main_TabSlider).Index;
		TabAction(tabIndex);
    }

	void TabAction(int index)
	{
        currentPopup?.ClosePopupUI();
        switch (index)
		{
			case 0:
                currentPopup = Managers.UI.ShopPopupUI<CharacterBasicInfo>("MainScene/CharacterBasicInfo", CanvasOrderType.Top, main);
                break;
            case 1:
                currentPopup = Managers.UI.ShopPopupUI<CharacterAwakeInfo>("MainScene/CharacterAwakeInfo", CanvasOrderType.Top, main);
                break;
            case 2:
                currentPopup = Managers.UI.ShopPopupUI<CharacterDetailInfo>("MainScene/CharacterDetailInfo", CanvasOrderType.Top, main);
                break;
        }
	}
	void SpecialWeaponAction(PointerEventData ped)
	{
        CloseAction(() => 
		{
            Managers.UI.ShopPopupUI<CharacterPotentialPopup>("MainScene/CharacterPotentialPopup", CanvasOrderType.Top);
        });
    }

	void Close(PointerEventData ped) => CloseAction(null);
    void CloseAction(Action callback) => ClosePopupUIPlayAni(() => { currentPopup?.ClosePopupUI(); callback?.Invoke(); });

    public enum UIButtonE
    {
		Dim,
		Main_PopupBg_Close,
		Main_BtnRe,
		Main_BtnDic,
		Main_SpecialWeapon,
    }
	public enum UIImageE
    {
		Main_PopupBg,
		Main_PopupBg_Pattern,
		Main_PopupBg_Bg,
		Main_CharacterInfo_Shadow,
		Main_CharacterInfo_Character,
		Main_CharacterInfo_CharacterInfoSlider,
		Main_CharacterInfo_CharacterInfoSlider_Slider,
		Main_CharacterInfo_CharacterInfoSlider_Slider_Fill,
		Main_CharacterInfo_CharacterInfoSlider_Arrow,
		Main_CharacterInfo_CharacterInfoSlider_Awake,
		Main_CharacterInfo_Type_Grade,
		Main_CharacterInfo_Type_Grade_Icon,
		Main_CharacterInfo_Type_Job,
		Main_CharacterInfo_Type_Tribe,
		Main_BtnDice,
		Main_BtnDice_Icon,
		Main_AwakeButton,
		Main_AwakeButton_Bg,
		Main_AwakeButton_Bg_Icon,
		Main_LevelUpButton,
		Main_LevelUpButton_Bg,
		Main_LevelUpButton_Bg_Icon,
    }
	public enum UITextProE
    {
		Main_CharacterInfo_Text,
		Main_CharacterInfo_CharacterInfoSlider_Awake_Text,
		Main_AwakeButton_Text,
		Main_LevelUpButton_Text,
    }
	public enum UITextE
    {
		Main_CharacterInfo_CharacterInfoSlider_Count,
		Main_CharacterInfo_Type_Job_Text,
		Main_CharacterInfo_Type_Tribe_Text,
		Main_AwakeButton_Bg_Text,
		Main_LevelUpButton_Bg_Text,
    }
	public enum UITabSliderE
    {
		Main_TabSlider,
    }
}