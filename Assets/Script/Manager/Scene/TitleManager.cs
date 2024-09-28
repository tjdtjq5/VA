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
            UIItemTest ulft = Managers.UI.ShopPopupUI<UIItemTest>("Test/UIItemTest");

        }, () => 
        {
            UI_Login UI_Login = Managers.UI.ShopPopupUI<UI_Login>("UILogin");

            UI_Login.LoginAfterJob(() =>
            {
                UIItemTest ulft = Managers.UI.ShopPopupUI<UIItemTest>("Test/UIItemTest");
                UI_Login.ClosePopupUI();
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
