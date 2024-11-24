using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : Attack
{
    public int AttackIndex { get; set; } = 0;
    public override float AttackRadius() => _moveRadiusAttack[AttackIndex];

    private readonly List<string> _attackNames = new List<string>() { "attack_1", "attack_2", "attack_3", "attack_4" };
    private readonly List<float> _moveRadiusAttack = new List<float>() { 2.5f, 2.5f, 2.5f, 7f };
    private readonly string _moveEvent = "move";
    private readonly string _endEvent = "end";
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

    public override void Initialize(Character character, Transform transform, SpineAniController spineAniController)
    {
        base.Initialize(character, transform, spineAniController);
        
        SpineAniController.SetEventFunc(_attackNames[0], _moveEvent, ()=> AttackMove(false));
        SpineAniController.SetEventFunc(_attackNames[1], _moveEvent, ()=> AttackMove(false));
        SpineAniController.SetEventFunc(_attackNames[2], _moveEvent, ()=> AttackMove(false));
        SpineAniController.SetEventFunc(_attackNames[3], _moveEvent, ()=> AttackMove(false));
        
        SpineAniController.SetEventFunc(_attackNames[0], _actionEvent, AttackActionHit);
        SpineAniController.SetEventFunc(_attackNames[1], _actionEvent, AttackActionHit);
        SpineAniController.SetEventFunc(_attackNames[2], _actionEvent, AttackActionHit);
        SpineAniController.SetEventFunc(_attackNames[3], _actionEvent, AttackActionHit);
        
        SpineAniController.SetEventFunc(_attackNames[0], _endEvent, AttackEnd);
        SpineAniController.SetEventFunc(_attackNames[1], _endEvent, AttackEnd);
        SpineAniController.SetEventFunc(_attackNames[2], _endEvent, AttackEnd);
        SpineAniController.SetEventFunc(_attackNames[3], _endEvent, AttackEnd);
    }

    public override void AttackAction(bool isLeft)
    {
        if (IsAttack)
            return;
        
        if (!Character.TargetCheck(isLeft))
            return;
        
        

        this.IsAttack = true;
        this._isLeft = isLeft;
        this.Character.Look(_isLeft);

        _target = Character.SearchTarget(isLeft);
        
        if (AttackIndex < 3)
        {
            
        }
        else if (AttackIndex == 3)
        {
            
        }
        
        
        // 1 ~ 3타는 제일 가까운 적 타겟
        // 4타에서는 제일 먼 거리의 있는 적 + 적들 사이에 캐릭터 박스 사이즈 만큼 공간이 없다면 가장 마지막 이후 적으로 타겟 변경  
        
        // 애니 실행
        UnityHelper.Log_H("AttackAction");
        this.SpineAniController.Play(_attackNames[AttackIndex], false, true);
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
                targetPos.x -= _isLeft ? _target.BoxWeidth / 2 : -_target.BoxWeidth / 2;
                targetPos.x -= _isLeft ? this.Character.BoxWeidth / 2 : -this.Character.BoxWeidth / 2;
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
        UnityHelper.Log_H("AttackEnd");
        AttackIndexUp();
        this.IsAttack = false;
        OnEnd?.Invoke(AttackIndex);
    }
}
