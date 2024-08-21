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

    private void Awake()
    {
        Stats = UnityHelper.FindChild<Stats>(this.gameObject, true);
        Stats.Setup(this);

        Movement = UnityHelper.FindChild<EntityMovement>(this.gameObject, true);
        Movement?.Setup(this);

        SkillSystem = UnityHelper.FindChild<SkillSystem>(this.gameObject, true);
        SkillSystem?.Setup(this);

        Animator = UnityHelper.FindChild<EntityAnimator>(this.gameObject, true);
        Animator?.Setup(this);

        StateMachine = UnityHelper.FindChild<MonoStateMachine<Entity>>(this.gameObject, true);
        StateMachine?.Setup(this);

        Collider = UnityHelper.FindChild<BoxCollider>(this.gameObject, true);
        ObjectAnglePositionSetting oaps = UnityHelper.FindChild<ObjectAnglePositionSetting>(this.gameObject, true);
        if (Collider && oaps)
            oaps.PositionSetting(Collider.size.z);

        Allive(false);
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

    public void Allive(bool isStatSetup)
    {
        if (isStatSetup)
        {
            Stats = UnityHelper.FindChild<Stats>(this.gameObject, true);
            Stats.Setup(this);
        }

        if (Movement)
            Movement.enabled = true;

        Animator.AniController.SetBool(Animator.kDeadHash, false);

        isDead = false;
    }

    private void OnDead()
    {
        if (Movement)
            Movement.enabled = false;

        Animator.AniController.SetBool(Animator.kDeadHash, true);

        onDead?.Invoke(this);

        isDead = true;
    }

    private Transform GetTransformSocket(Transform root, string socketName)
    {
        if (root.name == socketName || string.IsNullOrEmpty(socketName))
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

    public void Destroy()
    {
        Managers.Resources.Destroy(this.gameObject);
    }
}