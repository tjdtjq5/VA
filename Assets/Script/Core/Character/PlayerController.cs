using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(MoveController))]
[RequireComponent(typeof(EntityMovement))]
[RequireComponent(typeof(EntityAnimator))]
[RequireComponent(typeof(EntityStateMachine))]
public class PlayerController : MonoBehaviour
{
    private Entity entity;
    private SkillSystem skillSystem;
    private MoveController moveController;

    [SerializeField]
    private Skill basicAttackSkill;

    private void Start()
    {
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

        entity.SkillSystem.ReserveSkill(skill);

        if (result.selectedTarget)
            entity.Movement.TraceTarget = result.selectedTarget.transform;
        else
            entity.Movement.Destination = result.selectedPosition;
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
            Debug.LogAssertion($"{basicAttackSkill.CodeName}을 등록하지 못했습니다.");

        UnityHelper.Log_H($"Skill 등록 성공: {basicAttackSkill.CodeName}");

        var skill = skillSystem.Find(basicAttackSkill);
        UnityHelper.Assert_H(skill != null, $"{skill.CodeName}을 찾지 못했습니다.");

        UnityHelper.Log_H("testSkill을 통해 skillSystem에 등록된 Skill을 검색에 성공.");
        UnityHelper.Log_H($"{skill.CodeName} 사용 시도...");

        if (skillSystem.Use(basicAttackSkill))
        {
            UnityHelper.Log_H($"공격가능");
        }
        else
        {
            UnityHelper.Log_H(entity.StateMachine.GetCurrentState());
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
