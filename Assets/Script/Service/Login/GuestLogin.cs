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

        AccountLoginRequest req = new AccountLoginRequest()
        {
            ProviderType = ProviderType.Guest,
            NetworkIdOrCode = userId,
        };

        Managers.Web.SendPostRequest<AccountLoginResponce>("account/login", req, (res) =>
        {
            Managers.Web.JwtToken = res.JwtAccessToken;
            Managers.Web.AccountId = res.AccountId;

            if (callback != null)
            {
                callback.Invoke();
            }
        });
    }
}
