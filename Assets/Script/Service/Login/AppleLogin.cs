using AppleAuth;
using AppleAuth.Interfaces;
using AppleAuth.Native;
using System;
using UnityEngine;

public class AppleLogin : MonoBehaviour, ILoginService
{
    string _url = "https://appleid.apple.com/auth/authorize";
    string _clientId = "com.Slitz.Villager";
    string _redirect_uri = "https://slitz95.com";
    string _response_type = "code";
    string _scope = "email";
    string _response_mode = "query";

    Action _callback;

    bool isInit = false;

    public void Initialize()
    {
        if (isInit)
        {
            return;
        }
        isInit = true;

        Managers.DeepLink.AddAction(DeepLinkResponse);
    }

    public void Login(Action callback)
    {
        _callback = callback;

       // _redirect_uri = System.Web.HttpUtility.UrlDecode(_redirect_uri);
        string url = $"{_url}?client_id={_clientId}&redirect_uri={_redirect_uri}&response_type={_response_type}&response_mode={_response_mode}";

        Application.OpenURL(url);
    }

    private void DeepLinkResponse(ImaginationOverflow.UniversalDeepLinking.LinkActivation s)
    {
        string url = s.Uri;
        UnityHelper.Log_H(url);

        if (!url.Contains(_redirect_uri))
            return;

        //string code = s.QueryString["code"];

        //AccountLoginRequest req = new AccountLoginRequest()
        //{
        //    ProviderType = ProviderType.Apple,
        //    NetworkIdOrCode = code,
        //};

        //Managers.Web.SendPostRequest<AccountLoginResponce>("account/login", req, (res) =>
        //{
        //    UnityHelper.LogSerialize(res);

        //    if (_callback != null)
        //    {
        //        _callback.Invoke();
        //    }
        //});
    }


    //private AppleAuthManager appleAuthManager;

    //public void Initialize()
    //{
    //    var deserializer = new PayloadDeserializer();
    //    appleAuthManager = new AppleAuthManager(deserializer);
    //}
    //void Update()
    //{
    //    if (appleAuthManager != null)
    //        appleAuthManager.Update();
    //}

    //public void Login(Action callback)
    //{
    //    var loginArgs = new AppleAuthLoginArgs();

    //    appleAuthManager.LoginWithAppleId(
    //        loginArgs,
    //        credential =>
    //        {
    //            var appleIdCredential = credential as IAppleIDCredential;
    //            if (appleIdCredential != null)
    //            {
    //                var userId = appleIdCredential.User;

    //                // 로그인처리

    //                AccountLoginRequest req = new AccountLoginRequest()
    //                {
    //                    ProviderType = ProviderType.Apple,
    //                    NetworkIdOrCode = userId,
    //                };

    //                Managers.Web.SendPostRequest<AccountLoginResponce>("account/login", req, (res) =>
    //                {
    //                    UnityHelper.LogSerialize(res);

    //                    if (callback != null)
    //                    {
    //                        callback.Invoke();
    //                    }
    //                });
    //            }
    //        },
    //        error =>
    //        {
    //            UnityHelper.Log_H("Apple Signin Error");
    //        });
    //}

}
