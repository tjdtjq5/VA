public class UIMainMenu : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITabButtonParent>(typeof(UITabButtonParentE));

        base.Initialize();

        GetTabButtonParent(UITabButtonParentE.MenuList).SwitchOnHandler += OnMenueAction;
        GetTabButtonParent(UITabButtonParentE.MenuList).SwitchOffHandler += OffMenueAction;
    }

    void OnMenueAction(int index)
    {
        switch (index)
        {
            case 0:
                CharacterPopup(true);
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }
    void OffMenueAction(int index)
    {
        switch (index)
        {
            case 0:
                CharacterPopup(false);
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
    }

    void CharacterPopup(bool isActive)
    {
        string popupName = "MainScene/MainCharacter";
        if (isActive)
            Managers.UI.ShopPopupUI<MainCharacterPopup>(popupName, CanvasOrderType.Middle);
        else
            Managers.UI.ClosePopupUI(popupName);
    }

    public enum UIImageE
    {
		Bg,
    }
	public enum UITabButtonParentE
    {
		MenuList,
    }
}