using System;
using UnityEngine;

public class UILoginBtn : UIButton
{
    protected override void Initialize()
    {
        base.Initialize();
        Bind<UIImage>(typeof(UIImageE));
        Bind<UIText>(typeof(UITextE));
    }

    ProviderType _providerType;

    public void Set(ProviderType providerType, Action callback)
    {
        _providerType = providerType;

        ClickEventSet(callback);
        IconSet();
        TextSet();
    }
    void ClickEventSet(Action callback)
    {
        switch (_providerType)
        {
            case ProviderType.Guest:
                AddClickEvent((e) => GuestLogin(callback));
                break;
            case ProviderType.Google:
                AddClickEvent((e) => GoogleLogin(callback));
                break;
            case ProviderType.Apple:
                AddClickEvent((e) => AppleLogin(callback));
                break;
        }
    }
    void IconSet()
    {

    }
    void TextSet()
    {
        switch (_providerType)
        {
            case ProviderType.Guest:
                GetText(UITextE.Text).text = "Guest";
                break;
            case ProviderType.GooglePlayGames:
                GetText(UITextE.Text).text = "GooglePlayGames";
                break;
            case ProviderType.GameCenter:
                GetText(UITextE.Text).text = "GameCenter";
                break;
            case ProviderType.Facebook:
                GetText(UITextE.Text).text = "Facebook";
                break;
            case ProviderType.Google:
                GetText(UITextE.Text).text = "Google";
                break;
            case ProviderType.Apple:
                GetText(UITextE.Text).text = "Apple";
                break;
            default:
                GetText(UITextE.Text).text = "";
                break;
        }
    }

    void GuestLogin(Action callback)
    {
        LoginService.Login(ProviderType.Guest, () => { UnityHelper.Log_H($"Success! GuestLogin"); callback.Invoke(); });
    }
    void GoogleLogin(Action callback)
    {
        LoginService.Login(ProviderType.Google, () => { UnityHelper.Log_H($"Success! GoogleLogin"); callback.Invoke(); });
    }
    void AppleLogin(Action callback)
    {
        LoginService.Login(ProviderType.Apple, () => { UnityHelper.Log_H($"Success! AppleLogin"); callback.Invoke(); });
    }

    public enum UIImageE
    {
        Icon,
    }
    public enum UITextE
    {
        Text,
    }
}
