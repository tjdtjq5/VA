using System;
using System.Collections.Generic;
using UnityEngine;

public class UI_Login : UIPopup
{
    string loginBtnPrepab = "Prefab/UI/Button/LoginBtn";

    [SerializeField] Transform _btnListTr;

    Action _callback;

    protected override void Initialize()
	{
		base.Initialize();
		Bind<UIImage>(typeof(UIImageE));
	}

    protected override void UISet()
    {
        base.UISet();

        BtnSpawn();
    }

    void BtnSpawn()
    {
        List<ProviderType> providerTypes = GetProviderTypeList();

        for (int i = 0; i < providerTypes.Count; i++)
        {
            ProviderType type = providerTypes[i];
            GameObject go = Managers.Resources.Instantiate(loginBtnPrepab, _btnListTr);
            UILoginBtn loginBtn = go.GetOrAddComponent<UILoginBtn>();
            loginBtn.Set(type, _callback);
        }
    }

    public void LoginAfterJob(Action callback)
    {
        this._callback = callback;
    }

    public enum UIImageE
    {
		BG,
    }

    List<ProviderType> GetProviderTypeList()
    {
        List<ProviderType> result = new List<ProviderType>();

        if (Application.platform == RuntimePlatform.Android)
        {
            result.Add(ProviderType.Guest);
            result.Add(ProviderType.GooglePlayGames);
            result.Add(ProviderType.Google);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            result.Add(ProviderType.Guest);
            result.Add(ProviderType.GameCenter);
            result.Add(ProviderType.Apple);
            result.Add(ProviderType.Google);
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            result.Add(ProviderType.Guest);
        }
        else
        {
            int len = CSharpHelper.GetEnumLength<ProviderType>();
            for (int i = 0; i < len; i++) 
            {
                result.Add((ProviderType)i);
            }
        }

        return result;
    }
}
