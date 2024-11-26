using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAttack : PlayerAttack
{
    public override void Initialize(Character character, Transform transform, SpineAniController spineAniController)
    {
        base.Initialize(character, transform, spineAniController);
        InputSet();
    }

    void InputSet()
    {
        Managers.Input.AddKeyDownAction(KeyCode.LeftArrow, InputDownLeft);
        Managers.Input.AddKeyDownAction(KeyCode.RightArrow, InputDownRight);
    }

    void InputDownLeft()
    {
        AttackAction(true);
    }
    void InputDownRight()
    {
        AttackAction(false);
    }
}
