using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
public class EntityMovement : MonoBehaviour
{
    public delegate void SetDestinationHandler(EntityMovement movement, Vector3 destination);

    [SerializeField]
    private Stat moveSpeedStat;

    private MoveController moveController;
    private Transform traceTarget;
    private Stat entityMoveSpeedStat;

    public Entity Owner { get; private set; }
    public float MoveSpeed => moveController.Speed;
    public bool IsDashing { get; private set; }
    public bool IsPreceding { get; private set; }

    public MoveController MoveController { get { return moveController; } }
    public Transform TraceTarget
    {
        get => traceTarget;
        set
        {
            if (traceTarget == value)
                return;

            Stop();

            traceTarget = value;
            if (traceTarget)
            {
                TraceUpdateCoroutine = TraceUpdate(Vector3.zero);
                StartCoroutine(TraceUpdateCoroutine);
            }
        }
    }

    public Vector3 Destination
    {
        get => moveController.Destination;
        set
        {
            TraceTarget = null;
            SetDestination(value);
        }
    }

    public event SetDestinationHandler onSetDestination;

    public void Setup(Entity owner)
    {
        Owner = owner;

        moveController = UnityHelper.FindChild<MoveController>(this.gameObject, true);

        entityMoveSpeedStat = moveSpeedStat ? Owner.Stats.GetStat(moveSpeedStat) : null;
        if (entityMoveSpeedStat)
        {
            moveController.Speed = entityMoveSpeedStat.Value.ToFloat();
            entityMoveSpeedStat.onValueChanged += OnMoveSpeedChanged;
        }

        moveController.onStop -= OnStop;
        moveController.onStop += OnStop;

        moveController.onMove -= OnMove;
        moveController.onMove += OnMove;
    }

    void OnStop()
    {
        Owner.Animator.Play(Owner.Animator.waitClipName, true);
    }
    void OnMove()
    {
        Owner.Animator.Play(Owner.Animator.walkClipName, true);
    }

    private void OnDisable() => Stop();

    private void OnDestroy()
    {
        if (entityMoveSpeedStat)
            entityMoveSpeedStat.onValueChanged -= OnMoveSpeedChanged;
    }

    private void SetDestination(Vector3 destination)
    {
        moveController.Destination = destination;
        onSetDestination?.Invoke(this, destination);
    }
    public void SetTraceTarget(Transform target, Vector3 offset)
    {
        if (traceTarget == target)
            return;

        Stop();

        traceTarget = target;
        if (traceTarget)
        {
            TraceUpdateCoroutine = TraceUpdate(offset);
            StartCoroutine(TraceUpdateCoroutine);
        }
    }

    public void Stop()
    {
        traceTarget = null;

        if (TraceUpdateCoroutine != null)
            StopCoroutine(TraceUpdateCoroutine);

        moveController.Stop();

        IsDashing = false;
    }

    IEnumerator TraceUpdateCoroutine;
    private IEnumerator TraceUpdate(Vector3 offPos)
    {
        while (true)
        {
            if (Vector3.SqrMagnitude((TraceTarget.position + offPos) - transform.position) > 1.0f)
            {
                SetDestination(TraceTarget.position + offPos);
                yield return null;
            }
            else
                break;
        }

        traceTarget = null;
    }

    private void OnMoveSpeedChanged(Stat stat, BBNumber currentValue, BBNumber prevValue)
        => moveController.Speed = currentValue.ToFloat();



    private void FixedUpdate()
    {
        if (IsDashing)
        {
            currentDashTime += Managers.Time.FixedDeltaTime * dashSpeed;

            DashUpdated(dashDistance, dashDirection);

            if (currentDashTime >= dashDistance)
            {
                IsDashing = false;

                Owner.Animator.Play(Owner.Animator.waitClipName, true);

                if (dashCallback != null)
                    dashCallback.Invoke();
            }
        }

        if (IsPreceding)
        {
            precedingTimer += Managers.Time.FixedDeltaTime;
            if (precedingTimer > precedingTime)
            {
                Owner.Animator.Play(Owner.Animator.waitClipName, true);

                precedingTimer = 0;
                IsPreceding = false;
            }
        }
    }

    #region Dash
    float dashDistance = 0;
    Vector3 dashDirection = Vector3.zero;
    float currentDashTime = 0f;
    float prevDashDistance = 0f;
    float dashSpeed = 0f;
    Action dashCallback = null;
    public void Dash(float distance, Vector3 direction, float speedValue, string clipName, Action callback = null)
    {
        Stop();

        if (!string.IsNullOrEmpty(clipName))
            Owner.Animator.Play(clipName, true);

        dashDistance = distance;
        dashDirection = direction;
        currentDashTime = 0;
        prevDashDistance = 0;
        dashCallback = callback;
        dashSpeed = MoveSpeed / 3f * speedValue;

        MoveController.LookAtImmediateDirection(direction);

        IsDashing = true;
    }
    void DashUpdated(float distance, Vector3 direction)
    {
        float timePoint = currentDashTime / distance;
        float inOutSine = -(Mathf.Cos(Mathf.PI * timePoint) - 1f) / 2f;
        float currentDashDistance = Mathf.Lerp(0f, distance, inOutSine);
        float deltaValue = currentDashDistance - prevDashDistance;

        transform.position += (direction.normalized * deltaValue);
        prevDashDistance = currentDashDistance;
    }
    #endregion

    #region Preceding
    float precedingTime;
    float precedingTimer;
    public void Preceding(float time)
    {
        Stop();

        Owner.Animator.Play(Owner.Animator.precedingClipName, true);

        precedingTime = time;
        precedingTimer = 0f;

        IsPreceding = true;
    }
    #endregion
}
