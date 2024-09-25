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

        if (result.selectedTarget)
            entity.Movement.TraceTarget = result.selectedTarget.transform;
        else
            entity.Movement.Destination = result.selectedPosition;
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
        if (moveController && moveController.IsMove)
            return;

        if (RegisterBasicSkill && RegisterBasicSkill.IsInState<ReadyState>() && RegisterBasicSkill.IsUseable)
        {
            RegisterBasicSkill.Owner.SkillSystem.CancelTargetSearching();
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

        entity.Movement.Destination = this.transform.position + (direction * 100);
    }
    public override void MoveDestination(Vector3 destination)
    {
        entity.Movement.Destination = destination;
    }
}
