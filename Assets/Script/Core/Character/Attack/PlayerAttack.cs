using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PlayerAttack : Attack
{
    public int AttackIndex { get; set; } = 0;
    public override float AttackRadius() => _moveRadiusAttack[AttackIndex];

    private readonly List<string> _attackNames = new List<string>() { "attack_1", "attack_2", "attack_3", "attack_4" };
    private readonly List<float> _moveRadiusAttack = new List<float>() { 2.5f, 2.5f, 2.5f, 7f };
    private readonly string _moveEvent = "move";
    private readonly string _actionEvent = "action";
    protected int MaxAttackIndex => _moveRadiusAttack.Count;

    private readonly float _attackIndexTime = 0.8f;
    private float _attackIndexTimer;
    private bool _isAttackIndexing = false;

    private readonly float _attackMoveSpeed = 15f;
    private Vector3 _attackMoveDest;
    private readonly float _attackMoveDestDistance = 0.1f;
    private bool _isAttackMoving = false;

    private bool _isLeft = false;
    private Character _target;

    public override void Initialize(Character character, Transform transform, SpineAniController characterAniController, SpineAniController fxAniController)
    {
        base.Initialize(character, transform, characterAniController,fxAniController);
        
        CharacterAniController.SetEventFunc(_attackNames[0], _moveEvent, ()=> AttackMove(false));
        CharacterAniController.SetEventFunc(_attackNames[1], _moveEvent, ()=> AttackMove(false));
        CharacterAniController.SetEventFunc(_attackNames[2], _moveEvent, ()=> AttackMove(false));
        CharacterAniController.SetEventFunc(_attackNames[3], _moveEvent, ()=> AttackMove(true));
        
        CharacterAniController.SetEventFunc(_attackNames[0], _actionEvent, AttackActionHit);
        CharacterAniController.SetEventFunc(_attackNames[1], _actionEvent, AttackActionHit);
        CharacterAniController.SetEventFunc(_attackNames[2], _actionEvent, AttackActionHit);
        CharacterAniController.SetEventFunc(_attackNames[3], _actionEvent, AttackActionHit);
        
        CharacterAniController.SetEndFunc(_attackNames[0], AttackEnd);
        CharacterAniController.SetEndFunc(_attackNames[1], AttackEnd);
        CharacterAniController.SetEndFunc(_attackNames[2], AttackEnd);
        CharacterAniController.SetEndFunc(_attackNames[3], AttackEnd);
    }

    public override void AttackAction(bool isLeft)
    {
        if (IsAttack)
            return;

        if (Character.CharacterMove.IsBackmove)
            return;
        
        if (!Character.TargetCheck(isLeft))
            return;
        
        this.IsAttack = true;
        this._isLeft = isLeft;
        this.Character.Look(_isLeft);
        this._target = null;
        this._isAttackIndexing = false; 
        
        if (AttackIndex < 3)
        {
            _target = Character.SearchTarget(isLeft);
        }
        else if (AttackIndex == 3)
        {
            var targets = Character.SearchTargets(isLeft);
            float minDistanceX = _moveRadiusAttack[AttackIndex] - this.Character.BoxWeidth / 2;
            float maxDistanceX = _moveRadiusAttack[AttackIndex] + this.Character.BoxWeidth / 2;
            var targetFinds = targets.FindAll(t => t.transform.position.GetDistanceX(Transform.position) >= minDistanceX && t.transform.position.GetDistanceX(Transform.position) <= maxDistanceX);
            while (targetFinds.Count != 0)
            {
                _target = targetFinds.LastOrDefault();
                float targetPosLastX = _target.transform.position.x + (isLeft ? -_target.BoxWeidth / 2 : +_target.BoxWeidth / 2);
                float minPosX = targetPosLastX + (isLeft ? -this.Character.BoxWeidth / 2 : +this.Character.BoxWeidth / 2);
                float maxPosX = targetPosLastX + (isLeft ? this.Character.BoxWeidth / 2 : -this.Character.BoxWeidth / 2);
                
                targetFinds = targets.FindAll(t => t.transform.position.x >= minPosX && t.transform.position.x <= maxPosX);
            }
        }
        this.CharacterAniController.Play(_attackNames[AttackIndex], false, true);
        this.FxAniController.Play(_attackNames[AttackIndex], false, true);
    }

    public override void Clear()
    {
        _isAttackMoving = false;
        this.IsAttack = false;
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (_isAttackIndexing)
        {
            _attackIndexTimer += Managers.Time.FixedDeltaTime;
            if (_attackIndexTimer > _attackIndexTime)
            {
                AttackIndex = 0;
                _isAttackIndexing = false;
            }
        }

        if (_isAttackMoving)
        {
            Transform.position = Vector3.Lerp(Transform.position, _attackMoveDest, _attackMoveSpeed * Managers.Time.FixedDeltaTime);
            if (Transform.position.GetDistance(_attackMoveDest) < _attackMoveDestDistance)
                _isAttackMoving = false;
        }
    }
    void AttackIndexUp()
    {
        if (MaxAttackIndex - 1 <= AttackIndex)
            AttackIndex = 0;
        else
            AttackIndex++;

        _attackIndexTimer = 0;
        _isAttackIndexing = true;
    }
    // 1 ~ 3타 적을 넘지 못함 
    void AttackMove(bool enemyPass)
    {
        float moveLen = 0;
        if (MaxAttackIndex > 0)
            moveLen = _moveRadiusAttack[AttackIndex];

        Vector3 targetPos = Vector3.zero;
        
        if (_target is not null)
        {
            targetPos = _target.transform.position;
            targetPos.x += _isLeft ? _target.BoxWeidth / 2 : -_target.BoxWeidth / 2;
            targetPos.x += _isLeft ? this.Character.BoxWeidth / 2 : -this.Character.BoxWeidth / 2;
            targetPos.y = Transform.position.y;
            targetPos.z = Transform.position.z;
        }
        
        if (_target is null || targetPos.GetDistanceX(Transform.position) > moveLen)
        {
            this._attackMoveDest = Transform.position;
            this._attackMoveDest.x += _isLeft ? -moveLen : moveLen;
        }
        else
        {
            if (!enemyPass)
            {
                this._attackMoveDest = targetPos;
            }
            else
            {
                targetPos.x -= _isLeft ? _target.BoxWeidth : -_target.BoxWeidth;
                targetPos.x -= _isLeft ? this.Character.BoxWeidth : -this.Character.BoxWeidth;
                this._attackMoveDest = targetPos;
            }
        }
        
        _isAttackMoving = true;
    }
    void AttackActionHit()
    {
        this.OnAttack?.Invoke(AttackIndex);
    }
    void AttackEnd()
    {
        AttackIndexUp();
        this.IsAttack = false;
        OnEnd?.Invoke(AttackIndex);
    }
}
