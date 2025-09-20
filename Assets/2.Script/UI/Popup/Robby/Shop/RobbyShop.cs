using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobbyShop : UIRobby
{
    [SerializeField] private UIGoodsController _goodsController;

    private readonly string _parentName = "Robby/Shop/RobbyShop";
    private readonly string _gachaPopupName = "Robby/Shop/RobbyShopGacha";
    private readonly string _packagePopupName = "Robby/Shop/RobbyShopPackage";
    private readonly string _resetPopupName = "Robby/Shop/RobbyShopReset";
    private readonly string _milagePopupName = "Robby/Shop/RobbyShopMilage";

    protected override void Initialize()
    {
		Bind<BackTab>(typeof(BackTabE));

        base.Initialize();
    }

    public override void OpenUISet(CanvasOrderType orderType)
    {
        base.OpenUISet(orderType);

        Managers.Observer.UIGoodsController = _goodsController;
        _goodsController.Set();

        Get<BackTab>(BackTabE.SafeArea_BackLineTab).SwitchOnHandler -= OnTabSwitchOn;
        Get<BackTab>(BackTabE.SafeArea_BackLineTab).SwitchOnHandler += OnTabSwitchOn;

        Get<BackTab>(BackTabE.SafeArea_BackLineTab).UISet(0);
    }

    void OnTabSwitchOn(int index)
    {
        switch (index)
        {
            case 0:
                Managers.Observer.RobbyManager.ShopUI(_gachaPopupName, _parentName);
                break;
            case 1:
                Managers.Observer.RobbyManager.ShopUI(_packagePopupName, _parentName);
                break;
            case 2:
                Managers.Observer.RobbyManager.ShopUI(_resetPopupName, _parentName);
                break;
            case 3:
                Managers.Observer.RobbyManager.ShopUI(_milagePopupName, _parentName);
                break;
        }
    }
    
	public enum BackTabE
    {
		SafeArea_BackLineTab,
    }
}