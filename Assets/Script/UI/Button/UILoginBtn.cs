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
	[SerializeField] GPGSLogin _gpgsLogin;
	[SerializeField] GameCenterLogin _gameCenterLogin;
	[SerializeField] GoogleLogin _googleLogin;
	[SerializeField] AppleLogin _appleLogin;
	[SerializeField] GuestLogin _guestLogin;

    public void Set(ProviderType providerType)
	{
        _providerType = providerType;

        ClickEventSet();
        IconSet();
        TextSet();
    }
	void ClickEventSet()
	{
        switch (_providerType)
        {
            case ProviderType.Guest:
                AddClickEvent((e) => GuestLogin());
                break;
            case ProviderType.GooglePlayGames:
                AddClickEvent((e) => GPGSLogin());
                break;
            case ProviderType.GameCenter:
                AddClickEvent((e) => GameCenterLogin());
                break;
            case ProviderType.Google:
                AddClickEvent((e) => GoogleLogin());
                break;
            case ProviderType.Apple:
                AddClickEvent((e) => AppleLogin());
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

    void GuestLogin()
    {
        _guestLogin.Login(() => { UnityHelper.Log_H($"Success! GuestLogin"); });
    }
    void GPGSLogin()
	{
        _gpgsLogin.Initialize();
        _gpgsLogin.Login(() => { UnityHelper.Log_H($"GPGSLogin Succss!"); });
    }
	void GameCenterLogin()
	{
        _gameCenterLogin.Login(() => 
        {
            UnityHelper.Log_H($"GameCenterLogin Succss!");
        });
    }
    void GoogleLogin()
    {
        _googleLogin.Initialize();
        _googleLogin.Login(() => { UnityHelper.Log_H($"Success! GoogleLogin"); });
    }
    void AppleLogin()
    {
        _appleLogin.Initialize();
        _appleLogin.Login(() => { UnityHelper.Log_H($"Success! AppleLogin"); });
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
