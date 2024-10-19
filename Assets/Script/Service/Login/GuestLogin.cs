using System;
using UnityEngine;

public class GuestLogin
{
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

    public static void Login(Action callback)
    {
        string userId = SystemInfo.deviceUniqueIdentifier;

        Login(userId, callback);
    }

    public static void Login(string id, Action callback)
    {
        AccountLoginRequest req = new AccountLoginRequest()
        {
            ProviderType = ProviderType.Guest,
            NetworkIdOrCode = id,
        };

        Managers.Web.SendPostRequest<AccountLoginResponce>("account/login", req, (res) =>
        {
            Managers.Web.JwtToken = res.JwtAccessToken;
            Managers.Web.AccountId = res.AccountId;

            AutoJwtToken = res.JwtAccessToken;
            AutoProviderType = ProviderType.Guest;
            AutoAccountId = res.AccountId;

            if (callback != null)
                callback.Invoke();
        });
    }
}
