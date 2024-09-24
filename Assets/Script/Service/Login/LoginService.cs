using System;

public class LoginService
{
    public static void AtLogin(Action callback, Action expireCallback)
    {
        AutoLogin.Login(callback, () =>
        {
            if(expireCallback != null)
                expireCallback.Invoke();
        });
    }
    public static void Login(ProviderType providerType, Action callback)
    {
        ProviderLogin(providerType, callback);
    }
    static void ProviderLogin(ProviderType providerType, Action callback)
    {
        switch (providerType)
        {
            case ProviderType.Guest:
                GuestLogin.Login(callback);
                break;
            case ProviderType.Google:
                GoogleLogin.Login(callback);
                break;
            case ProviderType.Apple:
                AppleLogin.Login(callback);
                break;
            default:
                break;
        }
    }
    public static void TestLogin(string id, Action callback)
    {
        GuestLogin.Login(id, callback);
    }
}
