using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRobbyTab : UIFrame
{
    public Action<UIButtonE> OnClickTabHandler;

    [SerializeField] private Sprite _noneSprite;
    [SerializeField] private Sprite _selectedSprite;

    private UIButtonE _currentTab;

    private readonly string _growPopupName = "Robby/Grow/RobbyGrow";
    private readonly string _shopPopupName = "Robby/RobbyShop";
    private readonly string _equipPopupName = "Robby/Equip/RobbyEquip";
    private readonly string _mainPopupName = "Robby/RobbyMain";
    private readonly string _dungeonPopupName = "Robby/RobbyDungeon";

    protected override void Initialize()
    {
		Bind<UIButton>(typeof(UIButtonE));

        GetButton(UIButtonE.Tabs_Shop).AddClickEvent((ped) => {
            OnClickTab(UIButtonE.Tabs_Shop);
        });
        GetButton(UIButtonE.Tabs_Equip).AddClickEvent((ped) => {
            OnClickTab(UIButtonE.Tabs_Equip);
        });
        GetButton(UIButtonE.Tabs_Main).AddClickEvent((ped) => {
            OnClickTab(UIButtonE.Tabs_Main);
        });
        GetButton(UIButtonE.Tabs_Grow).AddClickEvent((ped) => {
            OnClickTab(UIButtonE.Tabs_Grow);
        });
        GetButton(UIButtonE.Tabs_Dungeon).AddClickEvent((ped) => {
            OnClickTab(UIButtonE.Tabs_Dungeon);
        });

        base.Initialize();
    }

    public void OnClickTab(UIButtonE button)
    {
        if (_currentTab == button)
            return;

        _currentTab = button;

        if (button == UIButtonE.Tabs_Shop || button == UIButtonE.Tabs_Dungeon)
        {
            return;
        }

        AllOff();

        GetButton(button).Image.sprite = _selectedSprite;

        switch (button)
        {
            case UIButtonE.Tabs_Shop:
                Managers.Observer.RobbyManager.ShopUI(_shopPopupName);
                break;
            case UIButtonE.Tabs_Equip:
                Managers.Observer.RobbyManager.ShopUI(_equipPopupName);
                break;
            case UIButtonE.Tabs_Main:
                Managers.Observer.RobbyManager.ShopUI(_mainPopupName);
                break;
            case UIButtonE.Tabs_Grow:
                Managers.Observer.RobbyManager.ShopUI(_growPopupName);
                break;
            case UIButtonE.Tabs_Dungeon:
                Managers.Observer.RobbyManager.ShopUI(_dungeonPopupName);
                break;
        }

        OnClickTabHandler?.Invoke(button);
    }

    private void AllOff()
    {
        GetButton(UIButtonE.Tabs_Shop).Image.sprite = _noneSprite;
        GetButton(UIButtonE.Tabs_Equip).Image.sprite = _noneSprite;
        GetButton(UIButtonE.Tabs_Main).Image.sprite = _noneSprite;
        GetButton(UIButtonE.Tabs_Grow).Image.sprite = _noneSprite;
        GetButton(UIButtonE.Tabs_Dungeon).Image.sprite = _noneSprite;
    }

	public enum UIButtonE
    {
		Tabs_Shop,
		Tabs_Equip,
		Tabs_Main,
		Tabs_Grow,
		Tabs_Dungeon,
    }
}