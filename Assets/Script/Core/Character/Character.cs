using EasyButtons;
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
    public string Code { get; protected set; }
    public int Index { get; set; }

    [HideInInspector] public Entity entity;
    public Action<Entity> onDead;
    public Action<Entity> onAllive;
    public Action<Entity, Entity, object, BBNumber> onTakeDamage;

    protected int fuTickMaxCount = 10; protected int fuTickCount = 0;

    public virtual void Initialize()
    {
        entity = GetComponent<Entity>();

        entity.onTakeDamage -= OnTakeDamage;
        entity.onTakeDamage += OnTakeDamage;

        entity.onAlliave -= OnAllive;
        entity.onAlliave += OnAllive;

        entity.onDead -= OnDead;
        entity.onDead += OnDead;
    }
    public virtual void OnDead(Entity entity) => onDead?.Invoke(entity);
    public virtual void OnAllive(Entity entity) => onAllive?.Invoke(entity);
    public virtual void OnTakeDamage(Entity entity, Entity instigator, object causer, BBNumber damage) => onTakeDamage?.Invoke(entity, instigator, causer, damage);
    public abstract void Play(string code);
    public abstract void Stop();
    public abstract void Clear();
    public abstract void SetStats();
    public abstract void MoveDirection(Vector3 direction);
    public abstract void MoveDestination(Vector3 destination);
    public abstract void MoveTrance(Transform target, Vector3 offset);

    [Button]
    public void DebugStats(Stat stat)
    {
        UnityHelper.Log_H($"[{stat.CodeName}] : {entity.Stats.GetStat(stat).Value}");
    }

    [Button]
    public void DebugIsDead()
    {
        UnityHelper.Log_H($"IsDead : {entity.IsDead}");
    }

    [Button]
    public void DebugEntityState()
    {
        UnityHelper.Log_H(entity.StateMachine.GetCurrentState());
    }
}
