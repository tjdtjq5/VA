using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public enum EntityControlType
{
    Player,
    AI
}

public class Entity : MonoBehaviour
{
    public delegate void TakeDamageHandler(Entity entity, Entity instigator, object causer, BBNumber damage);
    public delegate void DeadHandler(Entity entity);
    public delegate void AlliaveHandler(Entity entity);

    string skillActionEventName = "action";

    [SerializeField]
    private Category[] categories;
    [SerializeField]
    private EntityControlType controlType;

    private Dictionary<string, Transform> socketsByName = new();

    public EntityControlType ControlType => controlType;
    public IReadOnlyList<Category> Categories => categories;
    public bool IsPlayer => controlType == EntityControlType.Player;
    private bool isAiMove = false;
    public bool IsAIMove
    {
        get
        {
            return controlType != EntityControlType.Player || isAiMove;
        }
        set
        {
            isAiMove = value;
        }
    }

    public Stats Stats { get; private set; }
    bool isDead = false;
    public bool IsDead => isDead;

    public EntityMovement Movement { get; private set; }
    public MonoStateMachine<Entity> StateMachine { get; private set; }
    public SkillSystem SkillSystem { get; private set; }
    public EntityAnimator Animator { get; private set; }
    public BoxCollider Collider { get; private set; }

    public Entity Target { get; set; }

    public event TakeDamageHandler onTakeDamage;
    public event DeadHandler onDead;
    public event AlliaveHandler onAlliave;

    private void Awake()
    {
        Stats = UnityHelper.FindChild<Stats>(this.gameObject, true);
        Stats.Setup(this);

        Movement = UnityHelper.FindChild<EntityMovement>(this.gameObject, true);
        Movement?.Setup(this);

        Animator = UnityHelper.FindChild<EntityAnimator>(this.gameObject, true);
        Animator?.Setup(this);

        SkillSystem = UnityHelper.FindChild<SkillSystem>(this.gameObject, true);
        SkillSystem?.Setup(this);

        StateMachine = UnityHelper.FindChild<MonoStateMachine<Entity>>(this.gameObject, true);
        StateMachine?.Setup(this);

        Collider = UnityHelper.FindChild<BoxCollider>(this.gameObject, true);
        ObjectAnglePositionSetting oaps = UnityHelper.FindChild<ObjectAnglePositionSetting>(this.gameObject, true);
        if (Collider && oaps)
            oaps.PositionSetting(Collider.size.z);

        Animator.AniController.SetEventFunc(skillActionEventName, ApplyCurrentRunningSkill);

        Allive(false);
    }

    public void TakeDamage(Entity instigator, object causer, BBNumber damage)
    {
        UnityHelper.Log_H($"[{damage.ToCountString()}]    : {causer.ToString()}");

        if (IsDead)
            return;

        BBNumber prevValue = Stats.HPStat.DefaultValue;
        Stats.HPStat.DefaultValue -= damage;

        onTakeDamage?.Invoke(this, instigator, causer, damage);

        if (BBNumber.Approximately(Stats.HPStat.DefaultValue, 0f))
            OnDead();
    }

    public void Allive(bool isStatSetup)
    {
        if (isStatSetup)
        {
            Stats = UnityHelper.FindChild<Stats>(this.gameObject, true);
            Stats.Setup(this);
        }

        if (Movement)
            Movement.enabled = true;

        isDead = false;

        Animator.Play(Animator.waitClipName, true);

        onAlliave?.Invoke(this);
    }

    private void OnDead()
    {
        if (Movement)
            Movement.enabled = false;

        Animator.Play(Animator.deadClipName, false);

        onDead?.Invoke(this);

        isDead = true;
    }

    private Transform GetTransformSocket(Transform root, string socketName)
    {
        if (root.name == socketName || string.IsNullOrEmpty(socketName))
            return root;

        foreach (Transform child in root)
        {
            var socket = GetTransformSocket(child, socketName);
            if (socket)
                return socket;
        }

        return null;
    }

    public Transform GetTransformSocket(string socketName)
    {
        if (socketsByName.TryGetValue(socketName, out var socket))
            return socket;

        socket = GetTransformSocket(transform, socketName);
        if (socket)
            socketsByName[socketName] = socket;

        return socket;
    }
    private void ApplyCurrentRunningSkill()
    {
        SkillSystem.ApplyCurrentRunningSkill();
    }

    public bool HasCategory(Category category) => categories.Any(x => x.ID == category.ID);

    public bool IsInState<T>() where T : State<Entity>
           => StateMachine.IsInState<T>();

    public bool IsInState<T>(int layer) where T : State<Entity>
        => StateMachine.IsInState<T>(layer);

    public void Destroy()
    {
        Managers.Resources.Destroy(this.gameObject);
    }
}