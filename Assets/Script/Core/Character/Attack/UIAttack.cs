using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIAttack : PlayerAttack
{
    public override void Initialize(Character character, Transform transform, SpineAniController characterAniController, SpineAniController fxAniController)
    {
        base.Initialize(character, transform, characterAniController,fxAniController);
        UISet();
    }
    
    void UISet()
    {
        UIPlayer UIPlayer = GameObject.FindObjectOfType<UIPlayer>();

        UIPlayer.SetLeftDown(InputDownLeft);
        UIPlayer.SetRightDown(InputDownRight);
    }
    
    void InputDownLeft(PointerEventData ped)
    {
        AttackAction(true);
    }
 
    void InputDownRight(PointerEventData ped)
    {
        AttackAction(false);
    }
 
}
