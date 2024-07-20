public class UI_Login : UIPopup
{
	protected override void Initialize()
	{
		base.Initialize();
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIButton>(typeof(UIButtonE));
	}

	public enum UIImageE
    {
		BG,
		BtnList_Guest,
		BtnList_GPGS,
		BtnList_GameCenter,
		BtnList_Facebook,
    }
	public enum UIButtonE
    {
		BtnList_Guest,
		BtnList_GPGS,
		BtnList_GameCenter,
		BtnList_Facebook,
    }
}
