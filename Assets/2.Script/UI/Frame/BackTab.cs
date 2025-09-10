using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTab : UIFrame
{
    public int Index => GetTabButtonParent(UITabButtonParentE.LineTab).Index;
    public Action<int> SwitchOnHandler { get => GetTabButtonParent(UITabButtonParentE.LineTab).SwitchOnHandler; set => GetTabButtonParent(UITabButtonParentE.LineTab).SwitchOnHandler = value; }
    public Action<int> SwitchOffHandler { get => GetTabButtonParent(UITabButtonParentE.LineTab).SwitchOffHandler; set => GetTabButtonParent(UITabButtonParentE.LineTab).SwitchOffHandler = value; }

    private Animator _animator;
    private AniController _aniController;
    private UIRobbyBackTabType _currentBackTabType;

    private readonly string _changeBackAniName = "Change_Back";
    private readonly string _changeNomalAniName = "Change_Nomal";
    private readonly string _changeOnlyTabAniName = "Change_OnlyTab";
    private readonly string _changeOnlyBackAniName = "Change_OnlyBack";

    protected override void Initialize()
    {
		Bind<UIButton>(typeof(UIButtonE));
		Bind<UITabButtonParent>(typeof(UITabButtonParentE));

        _animator = this.GetComponent<Animator>();
        _aniController = _animator.Initialize();

        GetButton(UIButtonE.Back_Button).AddClickAniEvent((ped) => OnBack());

        Managers.Observer.RobbyManager.OnBackTab += OnBackTab;

        base.Initialize();
    }

    public void UISet(int index)
    {
        GetTabButtonParent(UITabButtonParentE.LineTab).UISet(index);
    }

    private void OnBackTab(UIRobbyBackTabType backTabType)
    {
        if (_currentBackTabType == backTabType)
            return;

        switch (backTabType)
        {
            case UIRobbyBackTabType.OnlyTab:
                if (_currentBackTabType == UIRobbyBackTabType.TabAndBack)
                    _aniController.SetTrigger(_changeNomalAniName);
                else if (_currentBackTabType == UIRobbyBackTabType.OnlyBack)
                    _aniController.SetTrigger(_changeOnlyTabAniName);
                else
                    _aniController.SetTrigger(_changeOnlyTabAniName);
                break;
            case UIRobbyBackTabType.OnlyBack:
                if (_currentBackTabType == UIRobbyBackTabType.TabAndBack)
                    _aniController.SetTrigger(_changeOnlyBackAniName);
                else if (_currentBackTabType == UIRobbyBackTabType.OnlyTab)
                    _aniController.SetTrigger(_changeOnlyBackAniName);
                else
                    _aniController.SetTrigger(_changeOnlyBackAniName);
                break;
            case UIRobbyBackTabType.TabAndBack:
                _aniController.SetTrigger(_changeBackAniName);
                break;
        }

        _currentBackTabType = backTabType;
    }

    private void OnBack()
    {
        Managers.Observer.RobbyManager.GoBack();
    }

	public enum UIButtonE
    {
		Back_Button,
    }
	public enum UITabButtonParentE
    {
		LineTab,
    }
}