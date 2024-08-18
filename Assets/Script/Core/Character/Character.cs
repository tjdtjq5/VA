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
    public Action<Entity, Entity, object, BBNumber> onTakeDamage;

    protected virtual void Start()
    {
        entity = GetComponent<Entity>();

        entity.onTakeDamage -= OnTakeDamage;
        entity.onTakeDamage += OnTakeDamage;

        entity.onDead -= OnDead;
        entity.onDead += OnDead;
    }
    void OnDead(Entity entity) => onDead?.Invoke(entity);
    void OnTakeDamage(Entity entity, Entity instigator, object causer, BBNumber damage) => onTakeDamage?.Invoke(entity, instigator, causer, damage);
}
