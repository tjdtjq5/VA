using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Move
{
    public bool IsMoving { get; set; } = false;
    public bool IsLeft { get; set; } = false;
    protected MoveType Movetype { get; set; }
    
    public Action OnMove { get; set; }
    public Action OnStop { get; set; }
    
    protected readonly float DefaultSpeed = 5f;

    protected readonly string Idle = "idle";
    protected readonly string Moving = "move";
    protected readonly Vector2 Left = new Vector2(-10f, 0);
    protected readonly Vector2 Right = new Vector2(10f, 0);


    public virtual void Initialize(Character character, Transform transform, SpineAniController spineAniController)
    {
        this.Character = character; 
        this.Transform = transform;    
        this.SpineAniController = spineAniController;
        
        SpineAniController.Play(Idle, true);
    }
    public abstract void SetIdle();
    public abstract void FixedUpdate();
    
    protected  Character Character;
    protected  Transform Transform;
    protected  SpineAniController SpineAniController;
}

public enum MoveType
{
    Idle,
    LeftMoving,
    RightMoving,
}