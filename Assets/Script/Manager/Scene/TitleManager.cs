using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : SceneBase
{
    protected override void Initialize()
    {
        base.Initialize();

        SetMonitor();

        SceneType = SceneType.Title;

        LoginService.AtLogin(() => 
        {
            UILoginFuncTest ulft = Managers.Resources.Instantiate<UILoginFuncTest>("Prefab/UI/Popup/UILoginFuncTest");

        }, () => 
        {
            UI_Login UI_Login = Managers.Resources.Instantiate<UI_Login>("Prefab/UI/Popup/UILogin");

            // UILoginTest ult = Managers.Resources.Instantiate<UILoginTest>("Prefab/UI/Popup/UILoginTest");
            UI_Login.LoginAfterJob(() =>
            {
                Managers.Resources.Destroy(UI_Login.gameObject);

                UILoginFuncTest ulft = Managers.Resources.Instantiate<UILoginFuncTest>("Prefab/UI/Popup/UILoginFuncTest");
            });
        });
    }

    public override void Clear()
    {

    }

    void SetMonitor()
    {
        if (!GameOptionManager.IsRelease)
        {
            Managers.Resources.Instantiate("Prefab/UI/Popup/Monitor/Monitor");
        }
    }

    public override PlayerController GetPlayer()
    {
        return null;
    }
}
