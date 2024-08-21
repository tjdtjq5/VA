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
                TraceUpdateCoroutine = TraceUpdate();
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
            moveController.Speed = entityMoveSpeedStat.Value.Float();
            entityMoveSpeedStat.onValueChanged += OnMoveSpeedChanged;
        }

        moveController.onStop -= OnStop;
        moveController.onStop += OnStop;

        moveController.onMove -= OnMove;
        moveController.onMove += OnMove;
    }

    void OnStop()
    {
        Owner.Animator.AniController.SetBool(Owner.Animator.kRunHash, false);
    }
    void OnMove()
    {
        Owner.Animator.AniController.SetBool(Owner.Animator.kRunHash, true);
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

    public void Stop()
    {
        traceTarget = null;

        if (TraceUpdateCoroutine != null)
            StopCoroutine(TraceUpdateCoroutine);

        moveController.Stop();
    }

    IEnumerator TraceUpdateCoroutine;
    private IEnumerator TraceUpdate()
    {
        while (true)
        {
            if (Vector3.SqrMagnitude(TraceTarget.position - transform.position) > 1.0f)
            {
                SetDestination(TraceTarget.position);
                yield return null;
            }
            else
                break;
        }
    }

    private void OnMoveSpeedChanged(Stat stat, BBNumber currentValue, BBNumber prevValue)
        => moveController.Speed = currentValue.Float();

    public void Dash(float distance, Vector3 direction)
    {
        Stop();

        IsDashing = true;
        Owner.Animator.AniController.SetBool(Owner.Animator.kDashHash, true);

        if (_dashUpdateCoroutine != null)
            StopCoroutine(_dashUpdateCoroutine);

        _dashUpdateCoroutine = DashUpdate(distance, direction);
        StartCoroutine(_dashUpdateCoroutine);
    }

    IEnumerator _dashUpdateCoroutine;
    private IEnumerator DashUpdate(float distance, Vector3 direction)
    {
        float currentDashTime = 0f;
        float prevDashDistance = 0f;

        while (true)
        {
            currentDashTime += Managers.Time.FixedDeltaTime * MoveSpeed / 3;

            float timePoint = currentDashTime / distance;
            float inOutSine = -(Mathf.Cos(Mathf.PI * timePoint) - 1f) / 2f;
            float currentDashDistance = Mathf.Lerp(0f, distance, inOutSine);
            float deltaValue = currentDashDistance - prevDashDistance;

            transform.position += (direction.normalized * deltaValue);
            prevDashDistance = currentDashDistance;

            if (currentDashTime >= distance)
                break;
            else
                yield return null;
        }

        Owner.Animator.AniController.SetBool(Owner.Animator.kDashHash, false);

        IsDashing = false;
    }
}
