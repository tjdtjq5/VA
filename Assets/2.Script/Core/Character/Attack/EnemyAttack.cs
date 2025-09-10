using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAttack : Attack
{
    [SerializeField] private bool _isDontMove;

    protected Character _target;
    
    private readonly float _attackDamage = 1f;
    private readonly float _moveSpeed = 1.55f;
    protected readonly string _attackName = "At";
    protected readonly string _moveName = "Move";
    protected readonly string _moveEvent = "move";
    private readonly string _hitPrefabPath = "Prefab/Effect/Enemy/EnemyAttackHit";
    
    public override void Initialize(Character character, Transform transform)
    {
        base.Initialize(character, transform);

        for (int i = 0; i < Character.SpineSpineAniControllers.Count; i++)
        {
            Character.SpineSpineAniControllers[i].SetEndFunc(_attackName, AttackAniEnd);
            Character.SpineSpineAniControllers[i].SetEventFunc(_attackName, ActionEvent , AttackEffect);
            Character.SpineSpineAniControllers[i].SetEventFunc(_attackName, FxEvent , AttackEffect);
            Character.SpineSpineAniControllers[i].SetEventFunc(_attackName, ActionEvent , AttackAniAction);
        }
    }
    public override void SetAttack(List<Character> targets, object cause, bool isSequence)
    {
        base.SetAttack(targets, cause, isSequence);

        SetTargetAttack(targets[0], cause);
    }
    protected virtual void SetTargetAttack(Character target, object cause)
    {
        if (_isDontMove)
        {
            StartAction(target, cause);
        }
        else
        {
            Vector3 pos = GetTargetPosition(target, true);
            pos.x += 0.5f;

            MoveAttack(target, pos, _moveName, true, _moveSpeed, cause);
        }
    }
    protected override void StartAction(Character target, object cause)
    {
        this._target = target;
        this.Cause = cause;

        SetAnimation(_attackName, false);
    }
    protected void AttackAniAction()
    {
        if (Cause != null && Cause.GetType() == typeof(PuzzleAttackData))
        {
            PuzzleAttackData cause = Cause as PuzzleAttackData;
            if (cause.isSequence)
            {
                cause.isSequence = false;
                Cause = cause;
            }
        }

        OnAttackAction(_target, Cause, _attackDamage);
    }
    protected virtual void AttackAniEnd()
    {
        AttackEnd(_moveName, true, _moveSpeed);
    }
    protected void AttackEffect()
    {
        _target.SetHit();
        
        Managers.Resources.Instantiate(_hitPrefabPath).transform.position = _target.BodyBoneTr.position;
    }
}
