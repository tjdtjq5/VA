using AppleAuth;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using System;
using System.Text;
using UnityEngine;

public class AppleLogin : MonoBehaviour, ILoginService
{
    private AppleAuthManager appleAuthManager;

    public void Initialize()
    {
        var deserializer = new PayloadDeserializer();
        appleAuthManager = new AppleAuthManager(deserializer);
    }
    void Update()
    {
        if (appleAuthManager != null)
            appleAuthManager.Update();
    }

    public void Login(Action callback)
    {
        var loginArgs = new AppleAuthLoginArgs();

        appleAuthManager.LoginWithAppleId(
            loginArgs,
            credential =>
            {
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    var userId = appleIdCredential.User;
    
                    // 로그인처리

                    AccountLoginRequest req = new AccountLoginRequest()
                    {
                        ProviderType = ProviderType.Apple,
                        NetworkIdOrCode = userId,
                    };

                    Managers.Web.SendPostRequest<AccountLoginResponce>("account/login", req, (res) =>
                    {
                        UnityHelper.LogSerialize(res);

                        if (callback != null)
                        {
                            callback.Invoke();
                        }
                    });
                }
            },
            error =>
            {
                UnityHelper.Log_H("Apple Signin Error");
            });
    }
}
