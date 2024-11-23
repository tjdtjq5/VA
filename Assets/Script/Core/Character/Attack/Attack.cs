using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack
{
    public bool IsAttack { get; set; } = false;
    public int AttackIndex { get; set; } = 0;
    
    public Action<int> OnAttack { get; set; }
    
    protected readonly string Attack1 = "attack_1";
    protected readonly string Attack2 = "attack_2";
    protected readonly string Attack3 = "attack_3";
    protected readonly string Attack4 = "attack_4";

    public virtual void Initialize(Character character, Transform transform, SpineAniController spineAniController)
    {
        this.Character = character;
        this.Transform = transform;
        this.SpineAniController = spineAniController;
    }

    public void AttackAction(bool isLeft)
    {
        var target = Character.SearchTarget(isLeft);

        // 타겟이 없다면 공격하면서 바로 이동 
        // 타겟이 있다면 타겟과의 거리 체크 후 거리가 가깝다면 타겟 바로 앞으로 이동 
        
        if (target is null)
            return;
        
    }
    public abstract void FixedUpdate();
    
    protected  Character Character;
    protected  Transform Transform;
    protected  SpineAniController SpineAniController;
}
