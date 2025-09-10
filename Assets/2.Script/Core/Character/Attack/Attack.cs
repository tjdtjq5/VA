using System;
using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

public abstract class Attack
{
    public bool IsAttack { get; set; } = false;
    public readonly string ActionEvent = "action";
    public readonly string FxEvent = "fx";
    private readonly int _orderInLayer = 101;

    public Action OnStart;
    public Action OnEnd;
    
    protected Character Character;
    protected Transform Transform;
    
    protected List<Character> Targets = new List<Character>();
    protected object Cause;
    protected bool IsSequence;
    protected bool IsAddSequence;

    protected bool _isMoved = false;
    protected Vector3 _moveStartPos = Vector3.zero;
    
    public virtual void Initialize(Character character, Transform transform)
    {
        this.Character = character;
        this.Transform = transform;
    }
    protected void SetAnimation(string aniName, bool isLoop)
    {
        Character.SetAnimation(aniName, isLoop);
    }

    public virtual void SetAttack(List<Character> targets, object cause, bool isSequence)
    {
        this.Cause = cause;
        this.Targets = targets;
        this.IsSequence = isSequence;

        if (isSequence)
            this.Character.OnSequenceAttack?.Invoke();

        this.Character.SetOrderInLayer(_orderInLayer);
    }
    protected virtual void StandAttack()
    {
        this.IsAttack = true;
        this._isMoved = true;
        this._moveStartPos = this.Character.transform.position;
    }
    protected virtual void MoveAttack(Character target, Vector3 movePosition, string aniName, bool isLoop, float moveSpeed, object cause)
    {
        if (IsAttack)
            return;

        this.IsAttack = true;
        this._isMoved = movePosition != this.Character.transform.position;
        this._moveStartPos = this.Character.transform.position;
        this.Character.CharacterMove.SetSpeed(moveSpeed);
        this.Character.CharacterMove.SetMoveDontStopMotion(aniName, isLoop, movePosition, ()=> StartAction(target, cause));
        
        OnStart?.Invoke();
        OnStart = null;
    }
    protected virtual void Move(Vector3 movePosition, string aniName, bool isLoop, float moveSpeed)
    {
        this._isMoved = movePosition != this.Character.transform.position;
        this._moveStartPos = this.Character.transform.position;
        this.Character.CharacterMove.SetSpeed(moveSpeed);
        this.Character.CharacterMove.SetMoveDontStopMotion(aniName, isLoop, movePosition);
    }
    protected virtual void TimeMove(string aniName, Vector3 destPos, float time)
    {
        this._isMoved = destPos != this.Character.transform.position;
        this._moveStartPos = this.Character.transform.position;
        this.Character.CharacterMove.SetTimeMove(aniName, destPos, time, null);
    }
    protected virtual void NotRecordMove(Vector3 movePosition, string aniName, bool isLoop, float moveSpeed)
    {
        this.Character.CharacterMove.SetSpeed(moveSpeed);
        this.Character.CharacterMove.SetMoveDontStopMotion(aniName, isLoop, movePosition);
    }
    protected abstract void StartAction(Character target, object cause);
    protected virtual void OnAttackAction(Character target, object cause, float damageValue)
    {
        float criPercent = this.Character.Stats.GetValue("CriPercent").ToFloat();
        DamageType damageType = DamageType.None;

        damageValue *= this.Character.Stats.GetValue("WeaponDamage").ToFloat() * 0.01f + 1;

        if (IsSequence)
        {
            damageValue *= this.Character.Stats.GetValue("SequenceDamage").ToFloat() * 0.01f + 1;
            criPercent += this.Character.Stats.GetValue("SequenceCriPercent").ToFloat();
            damageType = DamageType.Sequence;
        }
        else
        {
            damageValue *= this.Character.Stats.GetValue("NomalDamage").ToFloat() * 0.01f + 1;
            damageType = DamageType.None;
        }

        this.Character.ApplyAttack(target, cause, damageValue, criPercent, damageType, SkillApplyDamageType.NomalPuzzleAttack);
    }
    protected virtual void AttackEnd(string backMoveAniName, bool isLoop, float backMoveSpeed = 1.0f)
    {
        if (_isMoved)
        {
            this.Character.SetBack(true);
            this.Character.CharacterMove.SetSpeed(backMoveSpeed);
            this.Character.CharacterMove.SetMoveDontStopMotion(backMoveAniName, isLoop, _moveStartPos, OnAttackEnd);
        }
        else
        {
            OnAttackEnd();
        }
    }

    protected virtual void OnAttackEnd()
    {
        Clear();

        this.Character.SetBack(false);
        this.Character.CharacterMove.SetSpeed();
        this.Character.CharacterMove.SetIdle();
        this.Character.SetOrderInLayer(this.Character.OrderInLayer);
        
        Targets = this.Targets.FindAll(t => !t.IsNotDetect);
        if (Targets.Count > 0 && this.Character.Stats.sequenceStat && this.Character.Stats.sequenceStat.IsMax)
        {
            this.Character.Stats.sequenceStat.DefaultValue = 0;
            IsAddSequence = false;
            SetAttack(this.Targets, this.Cause, true);
        }
        else
        {
            if (this.Character.IsOnTriggerPassiveBuff(TriggerPassiveBuff.Skill_AddSequenceAttack_Attack_Sequence) && IsSequence && !IsAddSequence)
            {
                bool isAddSequence = UnityHelper.Random_H(0, 100) >= 50;
                if (isAddSequence)
                {
                    IsAddSequence = true;
                    SetAttack(this.Targets, this.Cause, true);
                }
                else
                {
                    OnEnd?.Invoke();
                    OnEnd = null;
                }
            }
            else
            {
                OnEnd?.Invoke();
                OnEnd = null;
            }
        }
    }

    protected Vector3 GetTargetPosition(Character target, bool isRightSide)
    {
        float boxWidth = this.Character.BoxWeidth * 0.5f + target.BoxWeidth * 0.5f;
        float x = isRightSide ?  target.transform.position.x + boxWidth : target.transform.position.x - boxWidth;
        Vector3 pos = target.transform.position;
        pos.x = x;
        
        return pos;
    }
    
    public virtual void Clear()
    {
        IsAttack = false;
        _isMoved = false;
    }

    public virtual void FixedUpdate() { }
}
