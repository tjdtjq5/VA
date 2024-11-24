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
        LeftDown();
    }
    void InputUpLeft()
    {
        LeftUp();
    }
    void InputDownRight()
    {
        RightDown();
    }
    void InputUpRight()
    {
        RightUp();
    }
}
