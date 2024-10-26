using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterInfoPopup : UIPopup
{
	[SerializeField] Transform main;

	UIPopup currentPopup;
	string characterCode;

    protected override void Initialize()
    {
		Bind<UIButton>(typeof(UIButtonE));
		Bind<UIImage>(typeof(UIImageE));
		Bind<CharacterInfoFrame>(typeof(CharacterInfoFrameE));
		Bind<UITabSlider>(typeof(UITabSliderE));

        base.Initialize();
    }

    protected override void UISet()
    {
        base.UISet();

		GetButton(UIButtonE.Dim).AddClickEvent(Close);
		GetButton(UIButtonE.Main_PopupBg_Close).AddClickEvent(Close);
		GetButton(UIButtonE.Main_PotentialButton).AddClickEvent(PotentialAction);

		GetTabSlider(UITabSliderE.Main_TabSlider).TabHandler += TabAction;

        int tabIndex = GetTabSlider(UITabSliderE.Main_TabSlider).Index;
		TabAction(tabIndex);
    }
	public void UISet(string characterCode)
	{
		this.characterCode = characterCode;
		Get<CharacterInfoFrame>(CharacterInfoFrameE.Main_CharacterInfo).UISet(characterCode);
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
                CharacterDetailInfo detailInfo = Managers.UI.ShopPopupUI<CharacterDetailInfo>("MainScene/CharacterDetailInfo", CanvasOrderType.Top, main);
                currentPopup = detailInfo;
                detailInfo.UISet(characterCode);
                break;
        }
	}
	void PotentialAction(PointerEventData ped)
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
		Main_PotentialButton,
		Main_AwakeButton,
		Main_LevelUpButton,
		Main_SpecialWeapon,
    }
	public enum UIImageE
    {
		Main_PopupBg,
		Main_PopupBg_Pattern,
		Main_PopupBg_Bg,
    }
	public enum CharacterInfoFrameE
    {
		Main_CharacterInfo,
    }
	public enum UITabSliderE
    {
		Main_TabSlider,
    }
}