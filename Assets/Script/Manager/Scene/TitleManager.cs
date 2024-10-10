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
            AfterLogin();

        }, () => 
        {
            UI_Login UI_Login = Managers.UI.ShopPopupUI<UI_Login>("UILogin");

            UI_Login.LoginAfterJob(() =>
            {
                UI_Login.ClosePopupUI();
                AfterLogin();
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
    void AfterLogin()
    {
        Managers.Table.DbGets(() => 
        {
            Managers.PlayerData.DbGets(() => 
            {
              //  UnityHelper.LogSerialize(Managers.PlayerData.Character.Gets());
                UIItemTest ulft = Managers.UI.ShopPopupUI<UIItemTest>("Test/UIItemTest");
            });
        });
    }

    public override PlayerController GetPlayer()
    {
        return null;
    }

    public override int GetPlayerJobCount(Tribe job)
    {
        throw new System.NotImplementedException();
    }
}
