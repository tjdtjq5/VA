using EasyButtons;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : Character
{
    private SkillSystem skillSystem; 
    private MoveController moveController;
    private EntityAnimator animator;
    private CharacterTribeSkill jobSkill;

    public bool IsLeft => moveController.IsLeft;
    public bool IsDown => moveController.IsDown;
    public Tribe Job => job;
    private Vector3 JoysticDir { get; set; } = Vector3.zero;
    private float searchRadius = 1000f;

    [Header("Setting")]
    [SerializeField] private Tribe job;
    [SerializeField] private Skill basicSkill; private Skill RegisterBasicSkill;
    [SerializeField] private Skill activeSkill; [SerializeField] private Skill activeUpgradeSkill; private Skill RegisterActiveSkill;

    public override void Initialize()
    {
        base.Initialize();

        entity = UnityHelper.FindChild<Entity>(this.gameObject, true);

        skillSystem = entity.SkillSystem;
        moveController = entity.Movement.MoveController;
        animator = entity.Animator;
        jobSkill = this.GetOrAddComponent<CharacterTribeSkill>();

        skillSystem.onSkillTargetSelectionCompleted -= ReserveSkill;
        skillSystem.onSkillTargetSelectionCompleted += ReserveSkill;

        Managers.Observer.OnJoystic -= JoysticDirection;
        Managers.Observer.OnJoystic += JoysticDirection;

        // SkillRegister
        if (basicSkill)
        {
            RegisterBasicSkill = skillSystem.Register(basicSkill);
        }

        if (activeUpgradeSkill)
        {
            RegisterActiveSkill = skillSystem.Register(activeUpgradeSkill);
        }
        else
        {
            if (activeSkill)
            {
                RegisterActiveSkill = skillSystem.Register(activeSkill);
            }
        }
    }
    private void ReserveSkill(SkillSystem skillSystem, Skill skill, TargetSearcher targetSearcher, TargetSelectionResult result)
    {
        if (!skill.IsInState<SearchingTargetState>())
            return;

        if (result.resultMessage != SearchResultMessage.OutOfRange)
            return;

        Entity target = GameFunction.SearchTarget(entity, searchRadius);
        Vector3 dest = target == null ? Vector3.zero : target.transform.position;

        if (Managers.Scene.CurrentScene.IsOutDest(this, Index, dest))
        {
            PlayerController masterPlayer = Managers.Scene.CurrentScene.GetPlayer();
            MoveTrance(masterPlayer.transform, GameController.GetIndexLocalPos(masterPlayer, Index));
        }
        else
        {
            MoveDestination(dest);
        }

        skillSystem.CancelTargetSearching();
    }

    public override void Play(string code) { this.Code = code; Initialize(); SetStats(); }
    public override void Stop()
    {
        moveController.Stop();
    }
    public override void Clear()
    {
        onDead = null;
        onTakeDamage = null;
    }
    public override void SetStats()
    {
        List<Stat> stats = Managers.PlayerData.GetStats();

        int removeLen = CSharpHelper.GetEnumLength<OnlyCharacterStatKey>();

        for (int i = 0; i < stats.Count; i++)
        {
            for (int j = 0; j < removeLen; j++) 
            {
                OnlyCharacterStatKey key = (OnlyCharacterStatKey)i;
                stats[i].RemoveBonusValueAndExceptionSub(key.ToString(), Code);
            }
        }

        entity.SetBonusStats(stats);
    }

    void FixedUpdate()
    {
        fuTickCount++;
        if (fuTickCount < fuTickMaxCount)
            return;
        fuTickCount = 0;

        if (!moveController)
            return;

        if (JoysticDir != Vector3.zero)
            return;
        
        // Attack

        if (RegisterActiveSkill && RegisterActiveSkill.IsInState<ReadyState>() && RegisterActiveSkill.IsUseable)
        {
            skillSystem.CancelTargetSearching();
            RegisterActiveSkill.Use();
        }
        else if (RegisterBasicSkill && RegisterBasicSkill.IsInState<ReadyState>() && RegisterBasicSkill.IsUseable)
        {
            skillSystem.CancelTargetSearching();
            RegisterBasicSkill.Use();
        }
    }

    public override void OnTakeDamage(Entity entity, Entity instigator, object causer, BBNumber damage)
    {
        base.OnTakeDamage(entity, instigator, causer, damage);
    }
    public override void MoveDirection(Vector3 direction)
    {
        direction.z = direction.y;
        direction.y = 0;

        entity.Movement.Destination = this.transform.position + (direction * 10);
    }
    public override void MoveDestination(Vector3 destination)
    {
        entity.Movement.Destination = destination;
    }
    public override void MoveTrance(Transform target, Vector3 offset)
    {
        entity.Movement.SetTraceTarget(target,offset);
    }
    public void JoysticDirection(Vector3 direction)
    {
        if (entity.Movement.IsDashing || entity.Movement.IsPreceding)
            return;

        if (!entity.IsInState<EntityDefaultState>())
            entity.StateMachine.ExecuteCommand(EntityStateCommand.ToDefaultState);

        JoysticDir = direction;
    }
    public void JobSetUp(int jobCount)
    {
        jobSkill.SetUp(entity, job, jobCount);
    }
    public void JobSkillAction(Entity target)
    {
        jobSkill.Apply(target);
    }

    #region Debug
    [Button]
    public void DebugBasicSkillState()
    {
        UnityHelper.Log_H(RegisterBasicSkill.GetCurrentStateType());
    }
    [Button]
    public void DebugActiveSkillState()
    {
        UnityHelper.Log_H(RegisterActiveSkill.GetCurrentStateType());
    }
    [Button]
    public void DebugIsMove()
    {
        UnityHelper.Log_H(moveController.IsMove);
    }
    #endregion
}
