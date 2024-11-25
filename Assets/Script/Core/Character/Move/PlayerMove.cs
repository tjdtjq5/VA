using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerMove : Move
{
    protected bool _isLeftDown = false;
    protected bool _isRightDown = false;
    
    protected readonly string Backmove = "miss";
    protected float _backmoveCheckRadius => Character.BoxWeidth * 2;
    protected float _backmoveWidth = 2.5f;
    protected Vector3 _backmoveDest;
    
    public override void Initialize(Character character, Transform transform, SpineAniController spineAniController)
    {
        base.Initialize(character, transform, spineAniController);

        this.Character.CharacterAttack.Attack.OnEnd += AttackEnd;
        this.SpineAniController.SetEndFunc(Backmove, BackMoveEnd);
    }

    public override void SetIdle()
    {
        if (!IsMoving)
            return;
        
        Movetype = MoveType.Idle;
        
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

        if (IsBackMove)
        {
            this.Transform.position = Vector3.Lerp(currentPosition, _backmoveDest, DefaultSpeed * Managers.Time.FixedDeltaTime);
        }
        else
        {
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
    }

    protected virtual void LeftDown()
    {
        if (IsBackMove)
            return;
        
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
        if (IsBackMove)
            return;
        
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
        if (IsBackMove)
            return;
        
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
        if (IsBackMove)
            return;
        
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
            {
                SetIdle();
                Character.CharacterAttack.AttackAction(true);
            }
            else
            {
                if (BackMoveCheck(true))
                    BackMove(true);
                else
                    LeftDown();
            }
        }
        else if (_isRightDown)
        {
            if (Character.TargetCheck(false))
            {
                SetIdle();
                Character.CharacterAttack.AttackAction(false);
            }
            else
            {
                if (BackMoveCheck(false))
                    BackMove(false);
                else
                    RightDown();
            }
               
        }
        else
        {
            if (IsLeft)
                LeftUp();
            else
                RightUp();
        }
    }
    public bool BackMoveCheck(bool isLeft)
    {
        Character target = this.Character.SearchTarget(!isLeft);
        if (target is not null && this.Transform.position.GetDistanceX(target.transform.position) < _backmoveCheckRadius )
            return true;
        else
            return false;
    }
    void BackMove(bool isLeft)
    {
        IsBackMove = true;
        
        this._backmoveDest = this.Transform.position;
        _backmoveDest.x += isLeft ? -_backmoveWidth : _backmoveWidth;
        
        this.Character.CharacterAttack.Clear();
        OnMove?.Invoke();
        IsMoving = true;
        IsLeft = isLeft;
        Movetype = isLeft ? MoveType.LeftMoving : MoveType.RightMoving;
        this.Character.Look(!isLeft);
        SpineAniController.Play(Backmove, true);
    }

    void BackMoveEnd()
    {
        IsBackMove = false;
        
        if (_isLeftDown)
        {
            if (Character.TargetCheck(true))
            {
                SetIdle();
                Character.CharacterAttack.AttackAction(true);
            }
            else
                LeftDown();
        }
        else if (_isRightDown)
        {
            if (Character.TargetCheck(false))
            {
                SetIdle();
                Character.CharacterAttack.AttackAction(false);
            }
            else
                RightDown();
        }
        else
        {
            if (IsLeft)
                LeftUp();
            else
                RightUp();
        }
    }
}
