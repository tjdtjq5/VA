using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    [SerializeField]
    private EnemyGradeType gradeType;
    public EnemyGradeType GradeType => gradeType;

    float defaultIdleTime = 2.2f; float defaultIdleTimeRange = 0.5f; float idleTime = 2.2f; float idleTimer;
    float deadMaxTime = 3f; float deadMaxTimer = 0;
    float randomMoveRange = 5f;
    float traceTargetRadius = 7f;
    float traceTargetRadiusMul;

    private SkillSystem skillSystem;
    private EntityAnimator animator;
    private MoveController moveController;
    private Skill defaultSkill;

    public bool IsDead => entity.IsDead;
    Vector3 spawnPos;
    private SearchResultMessage reserveSearchMessage;

    protected override void Initialize()
    {
        base.Initialize();

        traceTargetRadiusMul = traceTargetRadius * traceTargetRadius;
        idleTime = Random.Range(-defaultIdleTimeRange, defaultIdleTimeRange);
        idleTime += defaultIdleTime;

        skillSystem = entity.SkillSystem;
        animator = entity.Animator;
        moveController = entity.Movement.MoveController;

        var ownSkills = skillSystem.OwnSkills;
        defaultSkill = ownSkills[0];

        skillSystem.onSkillTargetSelectionCompleted -= ReserveSkill;
        skillSystem.onSkillTargetSelectionCompleted += ReserveSkill;
    }
    public override void Play()
    {
        spawnPos = this.transform.position;

        Initialize();
    }
    public void Allive() => entity.Allive(true);
    private void ReserveSkill(SkillSystem skillSystem, Skill skill, TargetSearcher targetSearcher, TargetSelectionResult result)
    {
        if (!skill.IsInState<SearchingTargetState>())
            return;

        if (result.resultMessage == SearchResultMessage.OutOfRange)
        {
            Vector3 targetPos = result.selectedTarget ? result.selectedTarget.transform.position : result.selectedPosition;

            if (spawnPos.GetSqrMagnitude(targetPos) < traceTargetRadiusMul)
                entity.Movement.Destination = targetPos;
            else
                entity.Movement.Destination = spawnPos;

        }
        else if (result.resultMessage == SearchResultMessage.Fail)
        {
            if (reserveSearchMessage != result.resultMessage)
                entity.Movement.Destination = spawnPos;
        }

        reserveSearchMessage = result.resultMessage;
    }
    private void FixedUpdate()
    {
        if (IsDead)
        {
            deadMaxTimer += Managers.Time.FixedDeltaTime;
            if (deadMaxTimer > deadMaxTime)
            {
                deadMaxTimer = 0;
                entity.Destroy();
            }
        }

        if (entity.IsInState<EntityDefaultState>())
        {
            if (!moveController.IsMove)
            {
                // patroll
                idleTimer += Managers.Time.FixedDeltaTime;
                if (idleTimer > idleTime)
                {
                    idleTimer = 0;
                    RandomMove();
                }
            }
        }

        fuTickCount++;
        if (fuTickCount < fuTickMaxCount)
            return;
        fuTickCount = 0;

        if (defaultSkill == null)
            return;

        if (entity.IsInState<EntityDefaultState>())
        {
            if (!moveController.IsMove)
            {
                // trace 
                if (defaultSkill.IsInState<ReadyState>() && defaultSkill.IsUseable)
                {
                    defaultSkill.Use();
                }
            }
        }
    }
    public override void Stop()
    {

    }
    public override void Clear() { }
    public override void OnDead(Entity entity)
    {
        base.OnDead(entity);
        deadMaxTimer = 0;
    }

    public void RandomMove()
    {
        float rx = Random.Range(-randomMoveRange, randomMoveRange);
        float rz = Random.Range(-randomMoveRange, randomMoveRange);
        Vector3 rPos = spawnPos;
        rPos.x += rx;
        rPos.z += rz;

        entity.Movement.Destination = rPos;
    }
    public override void OnTakeDamage(Entity entity, Entity instigator, object causer, BBNumber damage)
    {
        base.OnTakeDamage(entity, instigator, causer, damage);
    }
    public override void MoveDirection(Vector3 direction)
    {
        direction.z = direction.y;
        direction.y = 0;

        entity.Movement.Destination = this.transform.position + (direction);
    }
    public override void MoveDestination(Vector3 destination)
    {
        entity.Movement.Destination = destination;
    }
    public override void MoveTrance(Transform target, Vector3 offset)
    {
        entity.Movement.SetTraceTarget(target, offset);
    }
}
public enum EnemyGradeType
{
    Nomal,
    Boss,
}