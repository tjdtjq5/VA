using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    [SerializeField]
    private EnemyGradeType gradeType;
    public EnemyGradeType GradeType => gradeType;

    float defaultIdleTime = 2.2f; float defaultIdleTimeRange = 0.5f; float idleTime = 2.2f; float idleTimer;
    float randomMoveRange = 5f;
    float traceTargetRadius = 7f;
    float traceTargetRadiusMul;

    private SkillSystem skillSystem;
    private EntityAnimator animator;
    private MoveController moveController;
    private Skill defaultSkill;
    private PlayerController player;
    private Transform target;

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
    public void Setup(PlayerController player)
    {
        this.player = player;
        target = player.transform;

        this.player.onDead -= OnPlayerDead;
        this.player.onDead += OnPlayerDead;

        this.player.onAllive -= OnPlayerAllive;
        this.player.onAllive += OnPlayerAllive;
    }
    public void Allive() => entity.Allive(true);
    public override void Play()
    {
        spawnPos = this.transform.position;

        Initialize();
    }
    private void ReserveSkill(SkillSystem skillSystem, Skill skill, TargetSearcher targetSearcher, TargetSelectionResult result)
    {
        if (!skill.IsInState<SearchingTargetState>())
            return;

        if (result.resultMessage == SearchResultMessage.OutOfRange)
        {
            Vector3 targetPos = result.selectedTarget ? result.selectedTarget.transform.position : result.selectedPosition;

            if (Vector3.SqrMagnitude(targetPos - this.transform.position) > traceTargetRadiusMul)
                entity.Movement.Destination = spawnPos;
            else
                entity.Movement.Destination = targetPos;
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
        if (defaultSkill == null || target == null)
            return;

        if (Vector3.SqrMagnitude(target.transform.position - this.transform.position) > traceTargetRadiusMul)
        {
            // patroll

            if (!moveController.IsMove)
            {
                idleTimer += Managers.Time.FixedDeltaTime;
                if (idleTimer > idleTime)
                {
                    idleTimer = 0;
                    RandomMove();
                }
            }
        }
        else
        {
            // trace 

            if (defaultSkill.IsInState<ReadyState>() && defaultSkill.IsUseable)
            {
                defaultSkill.Owner.SkillSystem.CancelTargetSearching();
                defaultSkill.Use();
            }
        }
    }
    public override void Stop()
    {

    }
    public override void Clear() { }

    public void RandomMove()
    {
        float rx = Random.Range(-randomMoveRange, randomMoveRange);
        float rz = Random.Range(-randomMoveRange, randomMoveRange);
        Vector3 rPos = spawnPos;
        rPos.x += rx;
        rPos.z += rz;

        entity.Movement.Destination = rPos;
    }

    private void OnPlayerAllive(Entity playerEntity)
    {
        target = playerEntity.transform;
    }
    private void OnPlayerDead(Entity playerEntity)
    {
        target = null;
    }
    public override void OnTakeDamage(Entity entity, Entity instigator, object causer, BBNumber damage)
    {
        base.OnTakeDamage(entity, instigator, causer, damage);

        animator.Play(animator.hitClipName, false , 1);
    }
    public override void MoveDirection(Vector3 direction)
    {
        direction.z = direction.y;
        direction.y = 0;

        entity.Movement.Destination = this.transform.position + direction;
    }
    public override void MoveDestination(Vector3 destination)
    {
        entity.Movement.Destination = destination;
    }
}
public enum EnemyGradeType
{
    Nomal,
    Boss,
}