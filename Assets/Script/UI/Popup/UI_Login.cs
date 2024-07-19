public class UI_Login : UIPopup
{
	public override void Initialize()
	{
		base.Initialize();
		Bind<UnityEngine.UI.Image>(typeof(UIImageE));
	}

	public enum UIImageE
    {
		BackGround,
    }
}
