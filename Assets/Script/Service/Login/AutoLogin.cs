using System;

public class AutoLogin
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
    static bool IsAuto
    {
        get
        {
            string jwtToken = AutoJwtToken;
            int accountId = AutoAccountId;

            return (!string.IsNullOrEmpty(jwtToken) && accountId >= 0);
        }
    }

    static Action _callback;
    static Action _expireCallback;

    public static void Login(Action callback, Action expireCallback)
    {
        _callback = callback;
        _expireCallback = expireCallback;

        if (!IsAuto)
        {
            LoginExpireAction();
        }

        AccountAutoLoginRequest req = new AccountAutoLoginRequest()
        {
            ProviderType = AutoProviderType,
            JwtToken = AutoJwtToken,
            AccountId = AutoAccountId,
        };

        ErrorResponseJob errorResponseJob = new ErrorResponseJob(HttpResponceMessageType.AutoLoginExpire);
        errorResponseJob._job = new Job(LoginExpireAction);

        Managers.Web.SendPostRequest<AccountLoginResponce>("account/autoLogin", req, (res) =>
        {
            Managers.Web.JwtToken = res.JwtAccessToken;
            Managers.Web.AccountId = res.AccountId;

            AutoJwtToken = res.JwtAccessToken;
            AutoProviderType = ProviderType.Google;
            AutoAccountId = res.AccountId;

            if (_callback != null)
            {
                _callback.Invoke();
            }

        }, errorResponseJob);
    }
    static void LoginExpireAction()
    {
        AutoJwtToken = "";
        AutoAccountId = -1;

        if (_expireCallback != null)
            _expireCallback.Invoke();
    }
}
