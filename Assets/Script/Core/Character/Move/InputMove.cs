using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InputMove : Move
{
    public override void Initialize(Character character, Transform transform, SpineAniController spineAniController)
    {
        base.Initialize(character, transform, spineAniController);
        InputSet();
    }

    public override void SetIdle()
    {
        if (!IsMoving)
            return;
        
        switch (Movetype)
        {
            case MoveType.LeftMoving:
                InputUpLeft();
                break;
            case MoveType.RightMoving:
                InputUpRight();
                break;
        }
    }
    public override void FixedUpdate()
    {
        Vector2 currentPosition = this.Transform.position;
        switch (Movetype)
        {
            case MoveType.LeftMoving:
                
                if (!Character.TargetCheck(true))
                    this.Transform.position = Vector2.MoveTowards(currentPosition,currentPosition + Left * Managers.Time.FixedDeltaTime, DefaultSpeed);
                else
                {
                    SetIdle();
                    Character.CharacterAttack.AttackAction(true);
                }
                
                break;
            case MoveType.RightMoving:
                
                if (!Character.TargetCheck(false))
                  this.Transform.position = Vector2.MoveTowards(currentPosition,currentPosition + Right * Managers.Time.FixedDeltaTime, DefaultSpeed);
                else
                {
                    SetIdle();
                    Character.CharacterAttack.AttackAction(false);
                }
                
                break;
        }
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
        if (this.Character.CharacterAttack.IsAttack)
            return;

        if (Character.TargetCheck(true))
            return;
        
        OnMove?.Invoke();
        IsMoving = true;
        IsLeft = true;
        Movetype = MoveType.LeftMoving;
        this.Transform.localScale = LeftScale;
        SpineAniController.Play(Moving, true);
    }
    void InputUpLeft()
    {
        if (Movetype == MoveType.LeftMoving)
        {
            OnStop?.Invoke();
            IsMoving = false;
            Movetype = MoveType.Idle;
            SpineAniController.Play(Idle, true);
        }
    }
    void InputDownRight()
    {
        if (this.Character.CharacterAttack.IsAttack)
            return;
        
        if (Character.TargetCheck(false))
            return;
        
        OnMove?.Invoke();
        IsMoving = true;
        IsLeft = false;
        Movetype = MoveType.RightMoving;
        this.Transform.localScale = RightScale;
        SpineAniController.Play(Moving, true);
    }
    void InputUpRight()
    {
        if (Movetype == MoveType.RightMoving)
        {
            OnStop?.Invoke();
            IsMoving = false;
            Movetype = MoveType.Idle;
            SpineAniController.Play(Idle, true);
        }
    }
}
