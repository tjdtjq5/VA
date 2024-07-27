using System;
using UnityEngine;

public class GoogleLogin : MonoBehaviour, ILoginService
{
    string _url = "http://accounts.google.com/o/oauth2/v2/auth";
    string _clientId = "263943206515-gkikthsoisdbvoaek84bo8l8su0sguoo.apps.googleusercontent.com";
    string _redirect_uri_page = "/auth/google/callback";
    string _response_type = "code";
    string _scope = "https://www.googleapis.com/auth/userinfo.email";

    public void Initialize()
    {
        Managers.DeepLink.AddAction(GoogleLoginResponse);
    }

    public async void Login(Action callback)
    {
        string redirect_uri = $"{GameOptionManager.GetServerUrl}{_redirect_uri_page}";
        redirect_uri = System.Web.HttpUtility.UrlDecode(redirect_uri);
        string url = $"{_url}?client_id={_clientId}&redirect_uri={redirect_uri}&response_type={_response_type}&scope={_scope}";

        Application.OpenURL(url);
    }
    public void GoogleLoginResponse(string url)
    {
        string result = url.Split("?")[1];
        string code = result.Split("&")[0].Replace("code=", "");

        UnityHelper.Log_H($"url : {url}\ncode : {code}");

        AccountLoginRequest req = new AccountLoginRequest()
        {
            ProviderType = ProviderType.Google,
            NetworkIdOrCode = code,
        };

        Managers.Web.SendPostRequest<AccountLoginResponce>("account/login", req, (res) =>
        {
            UnityHelper.LogSerialize(res);
        });
    }
}
