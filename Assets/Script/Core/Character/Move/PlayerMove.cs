using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerMove : Move
{
    protected bool _isLeftDown = false;
    protected bool _isRightDown = false;
    
    
    public override void Initialize(Character character, Transform transform, SpineAniController spineAniController)
    {
        base.Initialize(character, transform, spineAniController);

        this.Character.CharacterAttack.Attack.OnEnd += AttackEnd;
    }

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
            
        this.Character.CharacterAttack.Clear();
        OnMove?.Invoke();
        IsMoving = true;
        IsLeft = true;
        Movetype = MoveType.LeftMoving;
        this.Character.Look(true);
        SpineAniController.Play(Moving, true);
    }
    protected virtual void LeftUp()
    {
        if (this.Character.CharacterAttack.IsAttack)
            return;
        
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
        
        this.Character.CharacterAttack.Clear();
        OnMove?.Invoke();
        IsMoving = true;
        IsLeft = false;
        Movetype = MoveType.RightMoving;
        this.Character.Look(false);
        SpineAniController.Play(Moving, true);
    }
    protected virtual void RightUp()
    {
        if (this.Character.CharacterAttack.IsAttack)
            return;
        
        if (Movetype == MoveType.RightMoving)
        {
            OnStop?.Invoke();
            IsMoving = false;
            Movetype = MoveType.Idle;
            SpineAniController.Play(Idle, true);
        }
    }

    void AttackEnd(int attackIndex)
    {
        if (_isLeftDown)
        {
            if (Character.TargetCheck(true))
                Character.CharacterAttack.AttackAction(true);
            else
                LeftDown();
        }
        else if (_isRightDown)
        {
            if (Character.TargetCheck(false))
                Character.CharacterAttack.AttackAction(false);
            else
                RightDown();
        }
    }
}
