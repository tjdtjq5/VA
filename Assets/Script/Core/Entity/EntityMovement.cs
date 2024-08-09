using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
public class EntityMovement : MonoBehaviour
{
    public delegate void SetDestinationHandler(EntityMovement movement, Vector3 destination);

    // �̵� �ӵ��� �� Stat
    [SerializeField]
    private Stat moveSpeedStat;

    private MoveController moveController;
    // Entity�� �����Ͽ� ������ ���
    private Transform traceTarget;
    // �� moveSpeedStat���� Entity�� Stats���� ã�ƿ� Stat
    private Stat entityMoveSpeedStat;

    public Entity Owner { get; private set; }
    public float MoveSpeed => moveController.Speed;

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
            // traceTarget�� �����ϴ� ���� ����
            TraceTarget = null;
            SetDestination(value);
        }
    }

    public event SetDestinationHandler onSetDestination;

    public void Setup(Entity owner)
    {
        Owner = owner;

        moveController = Owner.GetComponent<MoveController>();

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

    // ���� ����� ��ġ�� ��� Destination���� ��������
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
}
