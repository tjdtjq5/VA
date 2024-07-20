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
                Get<UIText>(UITextE.Text).text = "�Խ�Ʈ �α���";
                break;
            case ProviderType.GooglePlayGames:
                Get<UIText>(UITextE.Text).text = "���� �÷��� �α���";
                break;
            case ProviderType.GameCenter:
                Get<UIText>(UITextE.Text).text = "���� ���� �α���";
                break;
            case ProviderType.Facebook:
                Get<UIText>(UITextE.Text).text = "���̽��� �α���";
                break;
            default:
                Get<UIText>(UITextE.Text).text = "";
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
        UnityHelper.Log_H($"GameCenterLogin Succss!");
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
