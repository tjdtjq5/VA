using System;
using UnityEngine.EventSystems;

public class CharacterPotentialPopup : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIButton>(typeof(UIButtonE));
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIText>(typeof(UITextE));

        base.Initialize();
    }

    protected override void UISet()
    {
        base.UISet();

        GetButton(UIButtonE.Dim).AddClickEvent(Close);
        GetButton(UIButtonE.Main_CloseButton).AddClickEvent(Close);
    }
    void Close(PointerEventData ped) => CloseAction(() => { Managers.UI.ShopPopupUI<CharacterInfoPopup>("MainScene/CharacterInfoPopup", CanvasOrderType.Top); });
    void CloseAction(Action callback) => ClosePopupUIPlayAni(() => { callback?.Invoke(); });
	public enum UIButtonE
    {
		Dim,
		Main_CloseButton,
    }
	public enum UIImageE
    {
		Main_ChangeButton,
		Main_ChangeButton_Bg,
		Main_ChangeButton_Bg_Icon,
    }
	public enum UITextProE
    {
		Main_ChangeButton_Text,
    }
	public enum UITextE
    {
		Main_ChangeButton_Bg_Text,
    }
}