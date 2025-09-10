using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRobbyGrow : UIRobby
{
    [SerializeField] private UIGoodsController _goodsController;
    private readonly string _parentName = "Robby/Grow/RobbyGrow";
    private readonly string _growPopupName = "Robby/Grow/RobbyGrowGrow";
    private readonly string _researchPopupName = "Robby/Research/RobbyGrowResearch";

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
                Managers.Observer.RobbyManager.ShopUI(_growPopupName, _parentName);
			    break;
		    case 1:
                Managers.Observer.RobbyManager.ShopUI(_researchPopupName, _parentName);
			    break;
	    }
    }
	public enum BackTabE
    {
		SafeArea_BackLineTab,
    }
}