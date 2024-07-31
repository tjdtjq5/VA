using System;
using UnityEngine;

public class GoogleLogin : MonoBehaviour, ILoginService
{
    string _url = "http://accounts.google.com/o/oauth2/v2/auth";
    string _clientId = "263943206515-gkikthsoisdbvoaek84bo8l8su0sguoo.apps.googleusercontent.com";
    string _redirect_uri = "https://slitz95.com/auth/google/callback";
    string _response_type = "code";
    string _scope = "https://www.googleapis.com/auth/userinfo.email";

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

        _redirect_uri = System.Web.HttpUtility.UrlDecode(_redirect_uri);
        string url = $"{_url}?client_id={_clientId}&redirect_uri={_redirect_uri}&response_type={_response_type}&scope={_scope}";

        Application.OpenURL(url);
    }
    private void DeepLinkResponse(ImaginationOverflow.UniversalDeepLinking.LinkActivation s)
    {
        string url = s.Uri;

        if (!url.Contains(_redirect_uri))
            return;

        string code = s.QueryString["code"];

        AccountLoginRequest req = new AccountLoginRequest()
        {
            ProviderType = ProviderType.Google,
            NetworkIdOrCode = code,
        };

        Managers.Web.SendPostRequest<AccountLoginResponce>("account/login", req, (res) =>
        {
            UnityHelper.LogSerialize(res);

            if (_callback != null)
            {
                _callback.Invoke();
            }
        });
    }
}
