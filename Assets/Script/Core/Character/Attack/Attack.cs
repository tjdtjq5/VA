using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack
{
    public bool IsAttack { get; set; } = false;
    public abstract float AttackRadius();
    
    public Action<int> OnAttack { get; set; }
    public Action<int> OnEnd { get; set; }
    
    public virtual void Initialize(Character character, Transform transform, SpineAniController spineAniController)
    {
        this.Character = character;
        this.Transform = transform;
        this.SpineAniController = spineAniController;
    }

    public abstract void AttackAction(bool isLeft);

    public abstract void Clear();

    public virtual void FixedUpdate() { }

    protected  Character Character;
    protected  Transform Transform;
    protected  SpineAniController SpineAniController;
}
