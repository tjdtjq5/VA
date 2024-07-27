using Unity.VisualScripting;
using UnityEngine;

public class UI_Login : UIPopup
{
    string loginBtnPrepab = "Prefab/UI/Button/LoginBtn";

    [SerializeField] Transform _btnListTr;

	protected override void Initialize()
	{
		base.Initialize();
		Bind<UIImage>(typeof(UIImageE));
	}

    private void Start()
    {
        BtnSpawn();
    }

    void BtnSpawn()
    {
        int len = CSharpHelper.GetEnumLength<ProviderType>();

        for (int i = 0; i < len; i++)
        {
            ProviderType type = (ProviderType)i;
            GameObject go = Managers.Resources.Instantiate(loginBtnPrepab, _btnListTr);
            UILoginBtn loginBtn = go.GetOrAddComponent<UILoginBtn>();
            loginBtn.Set(type);
        }
    }

    public enum UIImageE
    {
		BG,
    }
}
