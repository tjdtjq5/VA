using UnityEngine.EventSystems;

public class CharacterInfoPopup : UIPopup
{
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
    }

	void Close(PointerEventData ped) => ClosePopupUIPlayAni();

    public enum UIButtonE
    {
		Dim,
		Main_PopupBg_Close,
		Main_BtnRe,
		Main_BtnDic,
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
		Main_SpecialWeapon,
		Main_SpecialWeapon_Icon,
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