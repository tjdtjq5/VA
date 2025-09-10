using System;
using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

public class ObserverManager
{
    #region Common

    public Camera Camera { get; set; }
    public CameraController CameraController { get; set; }

    #endregion

    #region Robby
    public RobbyManager RobbyManager { get; set; }
    #endregion
    
    #region InGame
    public Player Player { get; set; }
    public PuzzleBattleStateMachine PuzzleBattleStateMachine { get; set; }
    public UIPuzzle UIPuzzle { get; set; }
    public UIGoodsController UIGoodsController { get; set; }
    public Action<BBNumber> OnGessoChanged;
    public BBNumber _gesso;
    public BBNumber Gesso 
    {
        get
        {
            return _gesso;
        }
        set 
        { 
            _gesso = value;
            OnGessoChanged?.Invoke(value);
        }
    }
    #endregion

    public void Clear()
    {
        _gesso = 0;
        OnGessoChanged = null;
    }
}
