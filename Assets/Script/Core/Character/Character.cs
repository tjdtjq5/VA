using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(CharacterMove))]
[RequireComponent(typeof(CharacterAttack))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Character : MonoBehaviour
{
    public CharacterTeam team;
    public CharacterControllType control;
    
    SkeletonAnimation _characterAnimation;
    SpineAniController _characterAniController;
    SkeletonAnimation _fxAnimation;
    SpineAniController _fxAniController;
    CharacterMove _characterMove;
    CharacterAttack _characterAttack;
    Rigidbody2D _rigidbody2D;
    BoxCollider2D _boxCollider2D;

    public bool IsDead { get; private set; } = false;
    public CharacterMove CharacterMove => _characterMove;
    public CharacterAttack CharacterAttack => _characterAttack;
    public float BoxWeidth => _boxCollider2D.bounds.size.x;  
    
    protected readonly Vector3 LeftScale = new Vector3(-1f, 1f, 1f);
    protected readonly Vector3 RightScale = new Vector3(1f, 1f, 1f);

    private void Start()
    {
        _characterAnimation =  this.gameObject.FindChildByPath<SkeletonAnimation>("Character");

        if (_characterAnimation)
        {
            _characterAniController = _characterAnimation.GetComponent<SpineAniController>();
            _characterAniController.Initialize(_characterAnimation);
        }
        
        _fxAnimation =  this.gameObject.FindChildByPath<SkeletonAnimation>("Fx");

        if (_fxAnimation)
        {
            _fxAniController = _fxAnimation.GetComponent<SpineAniController>();
            _fxAniController.Initialize(_fxAnimation);
        }
        
        _characterAttack = this.GetComponent<CharacterAttack>();
        _characterAttack?.Initialize(this, _characterAniController, _fxAniController);
        
        _characterMove = this.GetComponent<CharacterMove>();
        _characterMove?.Initialize(this, _characterAniController);
        
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _boxCollider2D = this.GetComponent<BoxCollider2D>();
        
        Setting();

        if (team == CharacterTeam.Player)
        {
            Test();
        }
    }

    public Character SearchTarget(bool isLeft) => GameFunction.SearchTarget(this, isLeft);
    public List<Character> SearchTargets(bool isLeft) => GameFunction.SearchTargets(this, isLeft);
    public bool TargetCheck(bool isLeft)
    {
        Character target = SearchTarget(isLeft);
        if (target is not null && this.transform.position.GetDistanceX(target.transform.position) < CharacterAttack.AttackRadius() )
            return true;
        else
            return false;
    }
   

    public void Look(bool isLeft) => this.transform.localScale = isLeft ? LeftScale : RightScale;

    void Test()
    {
        CameraController cc = FindObjectOfType<CameraController>();
        cc.Initialize(this.transform);
        
        Map map = FindObjectOfType<Map>();
        map.Initialize(this.transform);
    }

    void Setting()
    {
        _rigidbody2D.gravityScale = 0;
        _boxCollider2D.isTrigger = true;
    }
 
}

public enum CharacterTeam
{
    Player,
    Enemy,
}

public enum CharacterControllType
{
    Input,
    UI,
    AI,
}