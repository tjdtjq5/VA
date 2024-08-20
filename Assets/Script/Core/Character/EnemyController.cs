using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    [SerializeField]
    private EnemyGradeType gradeType;
    public EnemyGradeType GradeType => gradeType;
    private EnemyMoveType moveType;
    public EnemyMoveType MoveType => moveType;
    private EnemyAttackType attackType;
    public EnemyAttackType AttackType => attackType;
    private SkillSystem skillSystem;
    private MoveController moveController;
    private MonoStateMachine<Entity> stateMachine;
    public bool IsDead => entity.IsDead;
    Vector3 spawnPos;
    float attackTime = 1f;
    float attackTimer = 0;
    float traceTargetRadius = 7f;

    protected override void Start()
    {
        base.Start();

        skillSystem = entity.SkillSystem;
        moveController = entity.Movement.MoveController;
        stateMachine = entity.StateMachine;

        skillSystem.onSkillStateChanged += (s, ss, newstate, prevstate, sssss) =>
        {
            if (ss.StateMachine.IsInState<ReadyState>())
            {
                UseSkill(ss);
            }
        };
        skillSystem.onSkillTargetSelectionCompleted += ReserveSkill;

        if (skillSystem.DefaultSkills.Length > 0)
            UseSkill(skillSystem.DefaultSkills[0]);
    }
    public override void Play()
    {
        spawnPos = this.transform.position;
    }
    private void ReserveSkill(SkillSystem skillSystem, Skill skill, TargetSearcher targetSearcher, TargetSelectionResult result)
    {
        if (result.resultMessage != SearchResultMessage.OutOfRange)
            return;

        entity.SkillSystem.ReserveSkill(skill);
        Vector3 targetPos = result.selectedTarget ? result.selectedTarget.transform.position : result.selectedPosition;

        if (Vector3.SqrMagnitude(targetPos - this.transform.position) > Mathf.Sqrt(traceTargetRadius))
        {
            entity.Movement.Destination = spawnPos;
        }
        else
        {
            entity.Movement.Destination = targetPos;
        }
    }
    public override void Stop()
    {

    }
    public override void Clear() { }

    void UseSkill(Skill skill)
    {
        skillSystem.Use(skill);
    }
}
public enum EnemyMoveType
{
    Patrol,
    Trace,
}
public enum EnemyAttackType
{
    Melee,
    Range,
}
public enum EnemyGradeType
{
    Nomal,
    Boss,
}