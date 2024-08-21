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
    float traceTargetRadius = 7f;
    float traceTargetRadiusMul;
    private SearchResultMessage reserveSearchMessage;
    private Skill defaultSkill;
    private PlayerController player;
    private Transform target;

    protected override void Awake()
    {
        base.Awake();

        traceTargetRadiusMul = traceTargetRadius * traceTargetRadius;

        skillSystem = entity.SkillSystem;
        moveController = entity.Movement.MoveController;
        stateMachine = entity.StateMachine;

        var ownSkills = skillSystem.OwnSkills;
        defaultSkill = ownSkills[0];

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
            return;

        if (defaultSkill.IsInState<ReadyState>() && defaultSkill.IsUseable)
        {
            defaultSkill.Owner.SkillSystem.CancelTargetSearching();
            defaultSkill.Use();
        }

    }
    public override void Stop()
    {

    }
    public override void Clear() { }

    private void OnPlayerAllive(Entity playerEntity)
    {
        target = playerEntity.transform;
    }
    private void OnPlayerDead(Entity playerEntity)
    {
        target = null;
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