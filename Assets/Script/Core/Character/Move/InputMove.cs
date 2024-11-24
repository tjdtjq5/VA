using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InputMove : PlayerMove
{
    public override void Initialize(Character character, Transform transform, SpineAniController spineAniController)
    {
        base.Initialize(character, transform, spineAniController);
        InputSet();
    }

    void InputSet()
    {
        Managers.Input.AddKeyDownAction(KeyCode.LeftArrow, InputDownLeft);
        Managers.Input.AddKeyUpAction(KeyCode.LeftArrow, InputUpLeft);
        Managers.Input.AddKeyDownAction(KeyCode.RightArrow, InputDownRight);
        Managers.Input.AddKeyUpAction(KeyCode.RightArrow, InputUpRight);
    }

    void InputDownLeft()
    {
        _isLeftDown = true;
        LeftDown();
    }
    void InputUpLeft()
    {
        _isLeftDown = false;
        LeftUp();
    }
    void InputDownRight()
    {
        _isRightDown = true;
        RightDown();
    }
    void InputUpRight()
    {
        _isRightDown = false;
        RightUp();
    }
}
