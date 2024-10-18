using System;
using UnityEngine;

public class GoogleLogin
{
    static string _url = "http://accounts.google.com/o/oauth2/v2/auth";
    static string _clientId = "263943206515-gkikthsoisdbvoaek84bo8l8su0sguoo.apps.googleusercontent.com";
    static string _redirect_uri = "https://slitz95.com/auth/google/callback";
    static string _response_type = "code";
    static string _scope = "https://www.googleapis.com/auth/userinfo.email";

    static string AutoJwtToken
    {
        get
        {
            return PlayerPrefsHelper.GetString_H(PlayerPrefsKey.auto_login_jwt_token);
        }
        set
        {
            PlayerPrefsHelper.Set_H(PlayerPrefsKey.auto_login_jwt_token, value);
        }
    }
    static ProviderType AutoProviderType
    {
        get
        {
            int value = PlayerPrefsHelper.GetInt_H(PlayerPrefsKey.auto_login_provider);
            return (ProviderType)CSharpHelper.EnumClamp<ProviderType>(value, true);
        }
        set
        {
            PlayerPrefsHelper.Set_H(PlayerPrefsKey.auto_login_provider, (int)value);
        }
    }
    static int AutoAccountId
    {
        get
        {
            return PlayerPrefsHelper.GetInt_H(PlayerPrefsKey.auto_login_account_id);
        }
        set
        {
            PlayerPrefsHelper.Set_H(PlayerPrefsKey.auto_login_account_id, value);
        }
    }

    static Action _callback;

    static bool isInit = false;
    public static void Initialize()
    {
        if (isInit)
        {
            return;
        }
        isInit = true;

        Managers.DeepLink.AddAction(DeepLinkResponse);
    }
    public static void Login(Action callback)
    {
        Initialize();

        _callback = callback;

        LinkUrlLogin();
    }
    static void LinkUrlLogin()
    {
        _redirect_uri = System.Web.HttpUtility.UrlDecode(_redirect_uri);
        string url = $"{_url}?client_id={_clientId}&redirect_uri={_redirect_uri}&response_type={_response_type}&scope={_scope}";

        Application.OpenURL(url);
    }
    static void CodeLoginAction(string code)
    {
        AccountLoginRequest req = new AccountLoginRequest()
        {
            ProviderType = ProviderType.Google,
            NetworkIdOrCode = code,
        };

        Managers.Web.SendPostRequest<AccountLoginResponce>("account/login", req, (res) =>
        {
            Managers.Web.JwtToken = res.JwtAccessToken;
            Managers.Web.AccountId = res.AccountId;

            AutoJwtToken = res.JwtAccessToken;
            AutoProviderType = ProviderType.Google;
            AutoAccountId = res.AccountId;

            if (_callback != null)
                _callback.Invoke();

        });
    }
    private static void DeepLinkResponse(ImaginationOverflow.UniversalDeepLinking.LinkActivation s)
    {
        string url = s.Uri;
        UnityHelper.Log_H(url);

        if (!url.Contains(_redirect_uri))
            return;

        string code = s.QueryString["code"];

        CodeLoginAction(code);
    }
}
