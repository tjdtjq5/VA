using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    private SkillSystem skillSystem;
    private MoveController moveController;

    [SerializeField]
    private Skill basicAttackSkill;

    protected override void Start()
    {
        base.Start();

        entity = UnityHelper.FindChild<Entity>(this.gameObject, true);

        skillSystem = entity.SkillSystem;
        moveController = entity.Movement.MoveController;

        skillSystem.onSkillTargetSelectionCompleted += ReserveSkill;

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

            Managers.Input.AddKeyDownAction(KeyCode.D, InputKeycodeD);
            Managers.Input.AddKeyDownAction(KeyCode.C, InputKeycodeC);
            Managers.Input.AddKeyDownAction(KeyCode.Space, InputSpace);
        }
    }
    private void ReserveSkill(SkillSystem skillSystem, Skill skill, TargetSearcher targetSearcher, TargetSelectionResult result)
    {
        if (result.resultMessage != SearchResultMessage.OutOfRange)
            return;

        UnityHelper.Log_H("ReserveSkill");

        entity.SkillSystem.ReserveSkill(skill);

        if (result.selectedTarget)
            entity.Movement.TraceTarget = result.selectedTarget.transform;
        else
            entity.Movement.Destination = result.selectedPosition;
    }

    public override void Play() { }
    public override void Stop()
    {
        moveController.Stop();
    }
    public override void Clear()
    {
        onDead = null;
        onTakeDamage = null;
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
    void InputKeycodeD()
    {
        entity.Movement.Dash(5f, _inputVector);
    }
    void InputKeycodeC()
    {
        entity.IsAIMove = !entity.IsAIMove;
    }
    void InputSpace()
    {
        if (!skillSystem.Register(basicAttackSkill))
            Debug.LogAssertion($"{basicAttackSkill.CodeName}");

        var skill = skillSystem.Find(basicAttackSkill);
        UnityHelper.Assert_H(skill != null, $"{skill.CodeName}");

        if (skillSystem.Use(basicAttackSkill))
        {
            UnityHelper.Log_H($"already use skill");
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
