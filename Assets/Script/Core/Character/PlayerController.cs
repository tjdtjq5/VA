using EasyButtons;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    private SkillSystem skillSystem;
    private MoveController moveController;
    private EntityAnimator animator;

    public bool IsLeft => moveController.IsLeft;
    public bool IsDown => moveController.IsDown;

    [SerializeField]
    private Skill basicSkill; private Skill RegisterBasicSkill;

    protected override void Initialize()
    {
        base.Initialize();

        entity = UnityHelper.FindChild<Entity>(this.gameObject, true);

        skillSystem = entity.SkillSystem;
        moveController = entity.Movement.MoveController;
        animator = entity.Animator;

        skillSystem.onSkillTargetSelectionCompleted += ReserveSkill;

        // SkillRegister
        RegisterBasicSkill = skillSystem.Register(basicSkill);

        if (!GameOptionManager.IsRelease)
        {
  
        }
    }
    private void ReserveSkill(SkillSystem skillSystem, Skill skill, TargetSearcher targetSearcher, TargetSelectionResult result)
    {
        if (!skill.IsInState<SearchingTargetState>())
            return;

        if (result.resultMessage != SearchResultMessage.OutOfRange)
            return;

        Vector3 dest = result.selectedTarget ? result.selectedTarget.transform.position : result.selectedPosition;

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

    public override void Play() { Initialize(); }
    public override void Stop()
    {
        moveController.Stop();
    }
    public override void Clear()
    {
        onDead = null;
        onTakeDamage = null;
    }

    void FixedUpdate()
    {
        fuTickCount++;
        if (fuTickCount < fuTickMaxCount)
            return;
        fuTickCount = 0;


        if (!moveController)
            return;

        if (RegisterBasicSkill && RegisterBasicSkill.IsInState<ReadyState>() && RegisterBasicSkill.IsUseable)
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

    [Button]
    public void DebugBasicSkillState()
    {
        UnityHelper.Log_H(RegisterBasicSkill.GetCurrentStateType());
    }

    [Button]
    public void DebugBasicSkillIsMove()
    {
        UnityHelper.Log_H(moveController.IsMove);
    }
}
