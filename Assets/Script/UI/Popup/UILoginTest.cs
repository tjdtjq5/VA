using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UILoginTest : UIPopup
{
    [SerializeField] GuestLogin _guestLogin;

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

		_guestLogin.Login(id, () => 
		{
			this._callback.Invoke();
        });
    }
	public void LoginAfterJob(Action callback)
	{
		this._callback = callback;
	}

	public enum UIImageE
    {
		BG,
		IDInputField,
		OkBtn,
    }
	public enum UIInputFieldE
    {
		IDInputField,
    }
	public enum UIButtonE
    {
		OkBtn,
    }
}
