using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMove : Move
{
    public override void SetIdle()
    {
        if (!IsMoving)
            return;
        
        switch (Movetype)
        {
            case MoveType.LeftMoving:
                LeftUp();
                break;
            case MoveType.RightMoving:
                RightUp();
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

    protected virtual void LeftDown()
    {
        if (this.Character.CharacterAttack.IsAttack)
            return;
    
        if (Character.TargetCheck(true))
            return;
            
        this.Character.CharacterAttack.EndAction();
        OnMove?.Invoke();
        IsMoving = true;
        IsLeft = true;
        Movetype = MoveType.LeftMoving;
        this.Transform.localScale = LeftScale;
        SpineAniController.Play(Moving, true);
    }
    protected virtual void LeftUp()
    {
        if (Movetype == MoveType.LeftMoving)
        {
            OnStop?.Invoke();
            IsMoving = false;
            Movetype = MoveType.Idle;
            SpineAniController.Play(Idle, true);
        }
    }
    protected virtual void RightDown()
    {
        if (this.Character.CharacterAttack.IsAttack)
            return;
        
        if (Character.TargetCheck(false))
            return;
        
        this.Character.CharacterAttack.EndAction();
        OnMove?.Invoke();
        IsMoving = true;
        IsLeft = false;
        Movetype = MoveType.RightMoving;
        this.Transform.localScale = RightScale;
        SpineAniController.Play(Moving, true);
    }
    protected virtual void RightUp()
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
