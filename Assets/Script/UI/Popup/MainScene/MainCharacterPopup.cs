public class MainCharacterPopup : UIPopup
{
    protected override void Initialize()
	{
		base.Initialize();

		Bind<UIImage>(typeof(UIImageE));
		Bind<UITabSlider>(typeof(UITabSliderE));

		GetTabSlider(UITabSliderE.TabSlider).TabHandler += TabAction;
    }
    protected override void UISet()
    {
        base.UISet();

		TabAction(0);
    }

    public override void OpenUISet(CanvasOrderType orderType)
    {
        base.OpenUISet(orderType);

		int index = GetTabSlider(UITabSliderE.TabSlider).Index;
		TabAction(index);
    }

    void TabAction(int index)
	{
		switch (index)
		{
			case 0:
                CharacterViewPoup(true);
                break;
			default:
				CharacterViewPoup(false);
                break;
		}
	}

	void CharacterViewPoup(bool isActive)
	{
		string popupName = "MainScene/MainCharacterView";
		if (isActive)
		{
            Managers.UI.ShopPopupUI<MainCharacterViewPopup>(popupName, CanvasOrderType.Middle, this.transform);

			Managers.Scene.InGameManager.ItemViewSet(ItemTableCodeDefine.Elixir);
        }
		else
		{
            Managers.UI.ClosePopupUI(popupName);
        }
    }

    public enum UIImageE
    {
		Bg1,
		Bg2,
    }
	public enum UITabSliderE
    {
		TabSlider,
    }
}
