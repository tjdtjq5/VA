using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;

public class GPGSLogin : MonoBehaviour, ILoginService
{
    public void Initialize()
    {
        PlayGamesPlatform.DebugLogEnabled = false;
        PlayGamesPlatform.Activate();
    }

    public void Login(System.Action callback)
    {
        PlayGamesPlatform.Instance.Authenticate((result) => 
        {
            if (result == SignInStatus.Success)
            {
                string name = PlayGamesPlatform.Instance.GetUserDisplayName();
                string id = PlayGamesPlatform.Instance.GetUserId();

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
