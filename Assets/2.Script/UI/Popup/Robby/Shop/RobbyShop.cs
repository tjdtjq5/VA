using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobbyShop : UIRobby
{
    private readonly string _parentName = "Robby/Shop/RobbyShop";

    protected override void Initialize()
    {
		Bind<BackTab>(typeof(BackTabE));

        base.Initialize();
    }

    public override void OpenUISet(CanvasOrderType orderType)
    {
        base.OpenUISet(orderType);

        Get<BackTab>(BackTabE.SafeArea_BackLineTab).SwitchOnHandler -= OnTabSwitchOn;
        Get<BackTab>(BackTabE.SafeArea_BackLineTab).SwitchOnHandler += OnTabSwitchOn;

        Get<BackTab>(BackTabE.SafeArea_BackLineTab).UISet(0);
    }

    void OnTabSwitchOn(int index)
    {
        switch (index)
        {
            case 0:
                // Managers.Observer.RobbyManager.ShopUI(_equipPopupName, _parentName);
                break;
            case 1:
                return;
                // Managers.Observer.RobbyManager.ShopUI(_petPopupName, _parentName);
                break;
            case 2:
                return;
                // Managers.Observer.RobbyManager.ShopUI(_toyPopupName, _parentName);
                break;
            case 3:
                return;
                // Managers.Observer.RobbyManager.ShopUI(_toyPopupName, _parentName);
                break;
        }
    }
    
	public enum BackTabE
    {
		SafeArea_BackLineTab,
    }
}