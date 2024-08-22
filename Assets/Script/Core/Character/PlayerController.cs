using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : Character
{
    private SkillSystem skillSystem;
    private MoveController moveController;
    private EntityAnimator animator;

    [SerializeField]
    private Skill basicSkill; private Skill RegisterBasicSkill;
    [SerializeField]
    private Skill qSkill; private Skill RegisterQSkill;
    [SerializeField]
    private Skill wSkill; private Skill RegisterWSkill;
    [SerializeField]
    private Skill eSkill; private Skill RegisterESkill;
    [SerializeField]
    private Skill aSkill; private Skill RegisterASkill;
    [SerializeField]
    private Skill sSkill; private Skill RegisterSSkill;
    [SerializeField]
    private Skill dSkill; private Skill RegisterDSkill;

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
        RegisterQSkill = skillSystem.Register(qSkill);
        RegisterWSkill = skillSystem.Register(wSkill);
        RegisterESkill = skillSystem.Register(eSkill);
        RegisterASkill = skillSystem.Register(aSkill);
        RegisterSSkill = skillSystem.Register(sSkill);
        RegisterDSkill = skillSystem.Register(dSkill);

        if (!GameOptionManager.IsRelease)
        {
            Managers.Input.AddKeyDownAction(KeyCode.LeftArrow, LeftArrowDown);
            Managers.Input.AddKeyDownAction(KeyCode.RightArrow, RightArrowDown);
            Managers.Input.AddKeyDownAction(KeyCode.UpArrow, UpArrowDown);
            Managers.Input.AddKeyDownAction(KeyCode.DownArrow, DownArrowDown);

            Managers.Input.AddKeyUpAction(KeyCode.LeftArrow, LeftArrowUp);
            Managers.Input.AddKeyUpAction(KeyCode.RightArrow, RightArrowUp);
            Managers.Input.AddKeyUpAction(KeyCode.UpArrow, UpArrowUp);
            Managers.Input.AddKeyUpAction(KeyCode.DownArrow, DownArrowUp);

            Managers.Input.AddKeyDownAction(KeyCode.Q, InputKeycodeQ);
            Managers.Input.AddKeyDownAction(KeyCode.W, InputKeycodeW);
            Managers.Input.AddKeyDownAction(KeyCode.E, InputKeycodeE);
            Managers.Input.AddKeyDownAction(KeyCode.A, InputKeycodeA);
            Managers.Input.AddKeyDownAction(KeyCode.S, InputKeycodeS);
            Managers.Input.AddKeyDownAction(KeyCode.D, InputKeycodeD);

            Managers.Input.AddKeyDownAction(KeyCode.C, InputKeycodeC);
            Managers.Input.AddKeyDownAction(KeyCode.Space, InputSpace);
        }
    }
    private void ReserveSkill(SkillSystem skillSystem, Skill skill, TargetSearcher targetSearcher, TargetSelectionResult result)
    {
        if (!skill.IsInState<SearchingTargetState>())
            return;

        if (result.resultMessage != SearchResultMessage.OutOfRange)
            return;

        // entity.SkillSystem.ReserveSkill(skill);

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
        if (_inputVector != Vector3.zero)
            return;

        if (moveController.IsMove)
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

        animator.SetTrigger(animator.kHitHash);
    }
    #region Input
    Vector3 _inputVector = Vector3.zero;
    void LeftArrowDown()
    {
        _inputVector.x = -moveController.NoneAdjustSpeed;
    }
    void LeftArrowUp()
    {
        if (_inputVector.x > 0)
            return;

        _inputVector.x = 0;
    }
    void RightArrowDown()
    {
        _inputVector.x = moveController.NoneAdjustSpeed;
    }
    void RightArrowUp()
    {
        if (_inputVector.x < 0)
            return;

        _inputVector.x = 0;
    }
    void UpArrowDown()
    {
        _inputVector.z = moveController.NoneAdjustSpeed;
    }
    void UpArrowUp()
    {
        if (_inputVector.z < 0)
            return;

        _inputVector.z = 0;
    }
    void DownArrowDown()
    {
        _inputVector.z = -moveController.NoneAdjustSpeed;
    }
    void DownArrowUp()
    {
        if (_inputVector.z > 0)
            return;

        _inputVector.z = 0;
    }
    void InputKeycodeQ()
    {
        if (RegisterQSkill && RegisterQSkill.IsInState<ReadyState>() && RegisterQSkill.IsUseable)
        {
            RegisterQSkill.Owner.SkillSystem.CancelTargetSearching();
            RegisterQSkill.Use();
        }
    }
    void InputKeycodeW()
    {
        if (RegisterWSkill && RegisterWSkill.IsInState<ReadyState>() && RegisterWSkill.IsUseable)
        {
            RegisterWSkill.Owner.SkillSystem.CancelTargetSearching();
            RegisterWSkill.Use();
        }
    }
    void InputKeycodeE()
    {
        if (RegisterESkill && RegisterESkill.IsInState<ReadyState>() && RegisterESkill.IsUseable)
        {
            RegisterESkill.Owner.SkillSystem.CancelTargetSearching();
            RegisterESkill.Use();
        }
    }
    void InputKeycodeA()
    {
        if (RegisterASkill && RegisterASkill.IsInState<ReadyState>() && RegisterASkill.IsUseable)
        {
            RegisterASkill.Owner.SkillSystem.CancelTargetSearching();
            RegisterASkill.Use();
        }
    }
    void InputKeycodeS()
    {
        if (RegisterSSkill && RegisterSSkill.IsInState<ReadyState>() && RegisterSSkill.IsUseable)
        {
            RegisterSSkill.Owner.SkillSystem.CancelTargetSearching();
            RegisterSSkill.Use();
        }
    }
    void InputKeycodeD()
    {
        if (RegisterDSkill && RegisterDSkill.IsInState<ReadyState>() && RegisterDSkill.IsUseable)
        {
            RegisterDSkill.Owner.SkillSystem.CancelTargetSearching();
            RegisterDSkill.Use();
        }
    }
    void InputKeycodeC()
    {
        entity.IsAIMove = !entity.IsAIMove;
    }
    void InputSpace()
    {
        if (RegisterBasicSkill && RegisterBasicSkill.IsInState<ReadyState>() && RegisterBasicSkill.IsUseable)
        {
            RegisterBasicSkill.Owner.SkillSystem.CancelTargetSearching();
            RegisterBasicSkill.Use();
        }
    }
    private void Update()
    {
        if (_inputVector != Vector3.zero)
        {
            entity.Movement.Destination = this.transform.position + _inputVector;
        }
    }
    #endregion
}
