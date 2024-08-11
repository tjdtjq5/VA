using System;
using UnityEngine;

public class GuestLogin : MonoBehaviour, ILoginService
{
    public void Initialize()
    {

    }

    public void Login(Action callback)
    {
        string userId = SystemInfo.deviceUniqueIdentifier;

        Login(userId, callback);
    }

    public void Login(string id, Action callback)
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

            UnityHelper.Log_H($"Id : {res.AccountId}\nToken : {res.JwtAccessToken}");

            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }
}
