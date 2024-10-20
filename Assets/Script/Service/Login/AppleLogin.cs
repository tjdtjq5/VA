using System;
using UnityEngine;
using Assets.SimpleSignIn.Apple.Scripts;

public class AppleLogin
{
    static AppleAuth AppleAuth;
    static Action _callback;

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

    static bool isInit = false;

    public static void Initialize()
    {
        if (isInit)
        {
            return;
        }
        isInit = true;

        AppleAuth = new AppleAuth();
        AppleAuth.DebugLog = false;
        AppleAuth.TryResume(OnSignIn, OnGetTokenResponse);
    }

    public static void Login(Action callback)
    {
        Initialize();

        _callback = callback;

        SignIn();
    }
    static public void SignIn()
    {
        AppleAuth.SignIn(OnSignIn, caching: true);
    }

    static public void SignOut()
    {
        AppleAuth.SignOut(revokeAccessToken: true);
        UnityHelper.Log_H("Not signed in");
    }

    static public void GetAccessToken()
    {
        AppleAuth.GetTokenResponse(OnGetTokenResponse);
    }

    static private void OnSignIn(bool success, string error, UserInfo userInfo)
    {
        if (success)
        {
            AccountLoginRequest req = new AccountLoginRequest()
            {
                ProviderType = ProviderType.Apple,
                NetworkIdOrCode = $"[{ProviderType.Apple.ToString()}]{userInfo.Email}",
            };

            Managers.Web.SendPostRequest<AccountLoginResponce>("account/login", req, (res) => 
            {
                Managers.Web.JwtToken = res.JwtAccessToken;
                Managers.Web.AccountId = res.AccountId;

                AutoJwtToken = res.JwtAccessToken;
                AutoProviderType = ProviderType.Apple;
                AutoAccountId = res.AccountId;

                if (_callback != null)
                    _callback.Invoke();
            });
        }
        else
            UnityHelper.Log_H(error);
    }

    static private void OnGetTokenResponse(bool success, string error, TokenResponse tokenResponse)
    {
        UnityHelper.Log_H(success ? $"Access token: {tokenResponse.AccessToken}" : error);

        if (!success) return;

        var jwt = new JWT(tokenResponse.IdToken);

        Debug.Log($"JSON Web Token (JWT) Payload: {jwt.Payload}");

        jwt.ValidateSignature(AppleAuth.ClientId, OnValidateSignature);
    }

    static private void OnValidateSignature(bool success, string error)
    {
        UnityHelper.Log_H(success ? "JWT signature validated" : error);
    }

    static public void Navigate(string url)
    {
        Application.OpenURL(url);
    }
}
