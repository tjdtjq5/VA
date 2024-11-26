using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class UIMove : PlayerMove
{
    public override void Initialize(Character character, Transform transform, SpineAniController spineAniController)
    {
        base.Initialize(character, transform, spineAniController);
        UISet();
    }

    void UISet()
    {
        UIPlayer UIPlayer = GameObject.FindObjectOfType<UIPlayer>();

        UIPlayer.SetLeftDown(InputDownLeft);
        UIPlayer.SetLeftUp(InputUpLeft);
        UIPlayer.SetRightDown(InputDownRight);
        UIPlayer.SetRightUp(InputUpRight);
    }
    
    void InputDownLeft(PointerEventData ped)
    {
        _isLeftDown = true;
        LeftDown();
    }
    void InputUpLeft(PointerEventData ped)
    {
        _isLeftDown = false;
        LeftUp();
    }
    void InputDownRight(PointerEventData ped)
    {
        _isRightDown = true;
        RightDown();
    }
    void InputUpRight(PointerEventData ped)
    {
        _isRightDown = false;
        RightUp();
    }
}
