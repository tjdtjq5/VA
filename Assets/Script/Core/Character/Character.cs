using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(SpineAniController))]
[RequireComponent(typeof(CharacterMove))]
[RequireComponent(typeof(CharacterAttack))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Character : MonoBehaviour
{
    public CharacterTeam team;
    public CharacterControllType control;
    
    SpineAniController _spineAniController;
    CharacterMove _characterMove;
    CharacterAttack _characterAttack;
    SkeletonAnimation _skeletonAnimation;
    Rigidbody2D _rigidbody2D;
    BoxCollider2D _boxCollider2D;

    public bool IsDead { get; private set; } = false;
    public CharacterMove CharacterMove => _characterMove;
    public CharacterAttack CharacterAttack => _characterAttack;

    private void Start()
    {
        _skeletonAnimation = this.GetComponentInChildren<SkeletonAnimation>();
        
        _spineAniController = this.GetComponent<SpineAniController>();
        _spineAniController.Initialize(_skeletonAnimation);
        
        _characterMove = this.GetComponent<CharacterMove>();
        _characterMove?.Initialize(this, _spineAniController);
        
        _characterAttack = this.GetComponent<CharacterAttack>();
        _characterAttack?.Initialize(this, _spineAniController);
        
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _boxCollider2D = this.GetComponent<BoxCollider2D>();
        
        Setting();
        Test();
    }

    public Character SearchTarget(bool isLeft) => GameFunction.SearchTarget(this, isLeft);
    public List<Character> SearchTargets(bool isLeft) => GameFunction.SearchTargets(this, isLeft);
    public bool TargetCheck(bool isLeft)
    {
        Character target = SearchTarget(isLeft);
        if (target is not null && this.transform.position.GetDistanceX(target.transform.position) < CharacterAttack.AttackLength )
            return true;
        else
            return false;
    }

    void Test()
    {
        CameraController cc = FindObjectOfType<CameraController>();
        cc.Initialize(this.transform);
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