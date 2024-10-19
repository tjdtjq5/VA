using System;
using UnityEngine;
using Assets.SimpleSignIn.Google.Scripts;

public static class GoogleLogin
{
    static public GoogleAuth GoogleAuth;
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
    static public void Initialize()
    {
        if (isInit)
        {
            return;
        }
        isInit = true;

        GoogleAuth = new GoogleAuth();
        GoogleAuth.TryResume(OnSignIn, OnGetTokenResponse);
    }
    static public void Login(Action callback)
    {
        Initialize();

        _callback = callback;
    }
    static public void SignIn()
    {
        GoogleAuth.SignIn(OnSignIn, caching: true);
    }
    static public void SignOut()
    {
        GoogleAuth.SignOut(revokeAccessToken: true);
    }
    static public void GetAccessToken()
    {
        GoogleAuth.GetTokenResponse(OnGetTokenResponse);
    }
    static private void OnSignIn(bool success, string error, UserInfo userInfo)
    {
        UnityHelper.Log_H(success ? $"Hello, {userInfo.name}!" : error);
    }
    static private void OnGetTokenResponse(bool success, string error, TokenResponse tokenResponse)
    {
        UnityHelper.Log_H(success ? $"Access token: {tokenResponse.AccessToken}" : error);

        if (!success) return;

        var jwt = new JWT(tokenResponse.IdToken);

        UnityHelper.Log_H($"JSON Web Token (JWT) Payload: {jwt.Payload}");

        jwt.ValidateSignature(GoogleAuth.ClientId, OnValidateSignature);
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
