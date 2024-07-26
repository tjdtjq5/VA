using UnityEngine;

public class UILoginBtn : UIButton
{
	protected override void Initialize()
	{
		base.Initialize();
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
	}

    [SerializeField] ProviderType _providerType;
	[SerializeField] GPGSLogin _gpgsLogin;
	[SerializeField] GameCenterLogin _gameCenterLogin;

    private void Start()
    {
		 Set();
    }

    public void Set()
	{
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
            case ProviderType.Facebook:
                AddClickEvent((e) => FacebookLogin());
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
            default:
                GetText(UITextE.Text).text = "";
                break;
        }
    }

    void GuestLogin()
    {
        UnityHelper.Log_H($"GuestLogin Succss!");
    }
    void GPGSLogin()
	{
        _gpgsLogin.Initialize();
        _gpgsLogin.Login(() => { UnityHelper.Log_H($"GPGSLogin Succss!"); });
    }
	void FacebookLogin()
	{
        UnityHelper.Log_H($"FacebookLogin Succss!");
    }
	void GameCenterLogin()
	{
        _gameCenterLogin.Login(() => 
        {
            UnityHelper.Log_H($"GameCenterLogin Succss!");
        });
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
