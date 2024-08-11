#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using UnityEngine;

public class GPGSLogin : MonoBehaviour, ILoginService
{
    public void Initialize()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();
#endif
    }

    public void Login(System.Action callback)
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.Authenticate((result) => 
        {
            if (result == SignInStatus.Success)
            {
                string name = PlayGamesPlatform.Instance.GetUserDisplayName();
                string id = PlayGamesPlatform.Instance.GetUserId();

                AccountLoginRequest req = new AccountLoginRequest()
                {
                    ProviderType = ProviderType.GooglePlayGames,
                    NetworkIdOrCode = id,
                };

                Managers.Web.SendPostRequest<AccountLoginResponce>("account/login", req, (res) =>
                {
                    Managers.Web.JwtToken = res.JwtAccessToken;
                    Managers.Web.AccountId = res.AccountId;

                    if (callback != null)
                        callback.Invoke();
                });
            }
            else
            {
                UnityHelper.LogError_H($"GPGS Sign Failed!");
            }
        });
#endif
    }
}
