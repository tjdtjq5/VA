using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Attack
{
    public int AttackIndex { get; set; } = 0;

    protected readonly string Attack1 = "attack_1";
    protected readonly string Attack2 = "attack_2";
    protected readonly string Attack3 = "attack_3";
    protected readonly string Attack4 = "attack_4";

    private readonly List<float> MoveRadiusAttack = new List<float>() { 3f, 3f, 0f, 7f };
    protected int MaxAttackIndex => MoveRadiusAttack.Count;

    private readonly float _attackIndexTime = 0.5f;
    private float _attackIndexTimer;
    private bool _isAttackIndexing = false;

    private readonly float _attackMoveSpeed = 5f;
    private Vector3 _attackMoveDest;
    private readonly float _attackMoveDestDistance = 0.1f;
    private bool _isAttackMoving = false;

    private bool _isLeft = false;
    private Character _target;

    public override void Initialize(Character character, Transform transform, SpineAniController spineAniController)
    {
        base.Initialize(character, transform, spineAniController);
        
        SpineAniController.SetEventFunc(Attack1, ()=> AttackMove(false));
        SpineAniController.SetEventFunc(Attack2, ()=> AttackMove(false));
        SpineAniController.SetEventFunc(Attack3, ()=> AttackMove(false));
        SpineAniController.SetEventFunc(Attack4, ()=> AttackMove(true));
    }

    public override void AttackAction(bool isLeft)
    {
        this._isLeft = isLeft;
        _target = Character.SearchTarget(isLeft);
        
        // 1 ~ 3타는 제일 가까운 적 타겟
        // 4타에서는 제일 먼 거리의 있는 적 + 적들 사이에 캐릭터 박스 사이즈 만큼 공간이 없다면 가장 마지막 이후 적으로 타겟 변경  
        
        // 애니 실행
        
        if (_target is null)
            return;

        AttackIndexUp();
    }

    public override void EndAction()
    {
        _isAttackMoving = false;
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
        AttackIndex++;
       
        if (MaxAttackIndex <= AttackIndex)
            AttackIndex = 0;

        _attackIndexTimer = 0;
        _isAttackIndexing = true;
    }
    // 1 ~ 3타 적을 넘지 못함 
    void AttackMove(bool enemyPass)
    {
        float moveLen = 0;
        if (MaxAttackIndex > 0)
            moveLen = MoveRadiusAttack[AttackIndex];

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
}
