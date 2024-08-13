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


    EntityAnimator entityAnimator;

    public Stats Stats { get; private set; }
    public bool IsDead => Stats.HPStat != null && BBNumber.Approximately(Stats.HPStat.DefaultValue, 0f);

    public EntityMovement Movement { get; private set; }
    public MonoStateMachine<Entity> StateMachine { get; private set; }
    public SkillSystem SkillSystem { get; private set; }
    public EntityAnimator Animator { get; private set; }

    public Entity Target { get; set; }

    public event TakeDamageHandler onTakeDamage;
    public event DeadHandler onDead;

    private void Awake()
    {
        entityAnimator = GetComponent<EntityAnimator>();

        Stats = GetComponent<Stats>();
        Stats.Setup(this);

        Movement = GetComponent<EntityMovement>();
        Movement?.Setup(this);

        SkillSystem = GetComponent<SkillSystem>();
        SkillSystem?.Setup(this);

        Animator = GetComponent<EntityAnimator>();
        Animator?.Setup(this);

        StateMachine = GetComponent<MonoStateMachine<Entity>>();
        StateMachine?.Setup(this);
    }

    public void TakeDamage(Entity instigator, object causer, BBNumber damage)
    {
        if (IsDead)
            return;

        BBNumber prevValue = Stats.HPStat.DefaultValue;
        Stats.HPStat.DefaultValue -= damage;

        onTakeDamage?.Invoke(this, instigator, causer, damage);

        if (BBNumber.Approximately(Stats.HPStat.DefaultValue, 0f))
            OnDead();
    }

    private void OnDead()
    {
        if (Movement)
            Movement.enabled = false;

        onDead?.Invoke(this);
    }

    private Transform GetTransformSocket(Transform root, string socketName)
    {
        if (root.name == socketName)
            return root;

        // root transform�� �ڽ� transform���� ��ȸ
        foreach (Transform child in root)
        {
            // ����Լ��� ���� �ڽĵ� �߿� socketName�� �ִ��� �˻���
            var socket = GetTransformSocket(child, socketName);
            if (socket)
                return socket;
        }

        return null;
    }

    public Transform GetTransformSocket(string socketName)
    {
        // dictionary���� socketName�� �˻��Ͽ� �ִٸ� return
        if (socketsByName.TryGetValue(socketName, out var socket))
            return socket;

        // dictionary�� �����Ƿ� ��ȸ �˻�
        socket = GetTransformSocket(transform, socketName);
        // socket�� ã���� dictionary�� �����Ͽ� ���Ŀ� �ٽ� �˻��� �ʿ䰡 ������ ��
        if (socket)
            socketsByName[socketName] = socket;

        return socket;
    }

    public bool HasCategory(Category category) => categories.Any(x => x.ID == category.ID);

    public bool IsInState<T>() where T : State<Entity>
           => StateMachine.IsInState<T>();

    public bool IsInState<T>(int layer) where T : State<Entity>
        => StateMachine.IsInState<T>(layer);
}