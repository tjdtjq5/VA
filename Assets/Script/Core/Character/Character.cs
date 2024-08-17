using System;
using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(MoveController))]
[RequireComponent(typeof(EntityMovement))]
[RequireComponent(typeof(EntityAnimator))]
[RequireComponent(typeof(EntityStateMachine))]
public abstract class Character : MonoBehaviour
{
    protected Entity entity;
    public Action<Entity> onDead;

    protected virtual void Start()
    {
        entity = GetComponent<Entity>();

        entity.onDead -= OnDead;
        entity.onDead += OnDead;
    }
    void OnDead(Entity entity)
    {
        onDead?.Invoke(entity);
    }
}
