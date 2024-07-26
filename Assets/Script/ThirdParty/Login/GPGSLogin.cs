using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GPGSLogin : MonoBehaviour, ILoginService
{
    public void Initialize()
    {
        UnityHelper.Log_H("GPGSLogin    Initialize");
        PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();
    }

    public void Login(System.Action callback)
    {
        UnityHelper.Log_H("GPGSLogin    Login");
        PlayGamesPlatform.Instance.Authenticate((result) => 
        {
            if (result == SignInStatus.Success)
            {
                string name = PlayGamesPlatform.Instance.GetUserDisplayName();
                string id = PlayGamesPlatform.Instance.GetUserId();

                AccountLoginRequest req = new AccountLoginRequest()
                {
                    ProviderType = ProviderType.GooglePlayGames,
                    NetworkId = id,
                };

                UnityHelper.Log_H($"Server GO Token : {id}");

                Managers.Web.SendPostRequest<AccountLoginResponce>("account/login", req, (res) =>
                {
                    UnityHelper.LogSerialize(res);
                });


                UnityHelper.Log_H($"[id] : {id}   [name] : {name}");

                if (callback != null)
                    callback.Invoke();
            }
            else
            {
                UnityHelper.LogError_H($"GPGS Sign Failed!");
            }
        });
    }
}
