using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRobby : UIPopup
{
    public UIRobbyBackTabType BackTabType => _backTabType;
    [SerializeField] private UIRobbyBackTabType _backTabType;
}
public enum UIRobbyBackTabType
{
    OnlyTab,
    OnlyBack,
    TabAndBack,
}