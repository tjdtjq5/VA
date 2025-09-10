using System.Security.Cryptography;
using UnityEngine.EventSystems;

public class UILoginFuncTest : UIPopup
{
	protected override void Initialize()
	{
		base.Initialize();
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIButton>(typeof(UIButtonE));
	}

    protected override void UISet()
    {
        base.UISet();

		GetButton(UIButtonE.BtnList_NeedLogin).AddClickEvent(OnNeedLogin);
		GetButton(UIButtonE.BtnList_NotLogin).AddClickEvent(OnNotLogin);
    }

	void OnNeedLogin(PointerEventData ped)
	{
        Managers.Web.SendPostRequest<string>("Test/LoginCheck", null ,(res) => 
		{
			UnityHelper.Log_H(res);
		});
	}
	void OnNotLogin(PointerEventData ped)
	{
        Managers.Web.SendPostRequest<string>("Test/TestResponse", null, (res) =>
        {
            UnityHelper.Log_H(res);
        });
    }

    public enum UIImageE
    {
		BackGround,
		BtnList_NeedLogin,
		BtnList_NotLogin,
    }
	public enum UIButtonE
    {
		BtnList_NeedLogin,
		BtnList_NotLogin,
    }
}
