using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class State<EntityType>
{
    public StateMachine<EntityType> Owner { get; private set; }
    public EntityType Entity { get; private set; }
    public int Layer { get; private set; }

    public void Setup(StateMachine<EntityType> owner, EntityType entity, int layer)
    {
        Owner = owner;
        Entity = entity;
        Layer = layer;

        Setup();
    }

    protected virtual void Setup() { }

    public virtual void Enter() { }
    public virtual void FixedUpdate() { }
    public virtual void Exit() { }
    public virtual bool OnReceiveMessage(int message, object data) => false;

}