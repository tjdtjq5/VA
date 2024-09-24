using System;
using UnityEngine.EventSystems;

public class UILoginTest : UIPopup
{
    Action _callback;
    
	protected override void Initialize()
	{
		base.Initialize();
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIInputField>(typeof(UIInputFieldE));
		Bind<UIButton>(typeof(UIButtonE));
	}
    protected override void UISet()
    {
        base.UISet();

        GetInputField(UIInputFieldE.IDInputField).placeHolder = $"아이디를 입력하세요";
		GetButton(UIButtonE.OkBtn).AddClickEvent(OnGuestLogin);
		GetButton(UIButtonE.TestLogBtn).AddClickEvent(OnServerLogTest);
    }

	void OnGuestLogin(PointerEventData ped)
	{
		string id = GetInputField(UIInputFieldE.IDInputField).text;

		bool isNull = string.IsNullOrEmpty(id);
		bool isRa = CSharpHelper.IsRegex(id);

		if (isNull || !isRa)
		{
			UnityHelper.LogError_H($"UILoginTest OnGuestLogin Id is Null or Is Not Regex Error\nid : {id}");
			return;
		}

        LoginService.TestLogin(id, () => 
		{
			this._callback.Invoke();
        });
    }
	public void LoginAfterJob(Action callback)
	{
		this._callback = callback;
	}

	void OnServerLogTest(PointerEventData ped)
	{
		Managers.Web.SendPostRequest<string>("Test/TestResponse", null, (res) => 
		{
			UnityHelper.Log_H(res);
		});
	}

	public enum UIImageE
    {
		BG,
		IDInputField,
		OkBtn,
		TestLogBtn,
    }
	public enum UIInputFieldE
    {
		IDInputField,
    }
	public enum UIButtonE
    {
		OkBtn,
		TestLogBtn,
    }
}
