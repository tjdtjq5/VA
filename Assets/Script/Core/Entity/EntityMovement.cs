using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
public class EntityMovement : MonoBehaviour
{
    public delegate void SetDestinationHandler(EntityMovement movement, Vector3 destination);

    // 이동 속도로 쓸 Stat
    [SerializeField]
    private Stat moveSpeedStat;

    private MoveController moveController;
    // Entity가 추적하여 움직일 대상
    private Transform traceTarget;
    // 위 moveSpeedStat으로 Entity의 Stats에서 찾아온 Stat
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
                if (TraceUpdateCoroutine == null)
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
            // traceTarget을 추적하는 것을 멈춤
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
            moveController.Speed = entityMoveSpeedStat.Value.GetFloat();
            entityMoveSpeedStat.onValueChanged += OnMoveSpeedChanged;
        }
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

    // 추적 대상의 위치를 계속 Destination으로 설정해줌
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
        => moveController.Speed = currentValue.GetFloat();

    public void Dash(float distance, Vector3 direction)
    {
        Stop();

        IsDashing = true;

        if (_dashUpdateCoroutine != null)
            StopCoroutine(_dashUpdateCoroutine);

        _dashUpdateCoroutine = DashUpdate(distance, direction);
        StartCoroutine(_dashUpdateCoroutine);
    }

    IEnumerator _dashUpdateCoroutine;
    private IEnumerator DashUpdate(float distance, Vector3 direction)
    {
        // 현재까지 구른 시간
        float currentDashTime = 0f;
        // 이전 Frame에 이동한 거리
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

        IsDashing = false;
    }
}
