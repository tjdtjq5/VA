using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class MouseSelectTarget : TargetSelectionAction
{
    [Header("Data")]
    // �˻� ����, 0�� ��� ���Ѵ븦 �ǹ���.
    [Min(0f)]
    [SerializeField]
    private float range;
    [Range(0f, 360f)]
    [SerializeField]
    private float angle;

    private TargetSearcher targetSearcher;
    private Entity requesterEntity;
    private GameObject requesterObject;
    private SelectCompletedHandler onSelectCompleted;

    public override object Range => range;
    public override object ScaledRange => range * Scale;
    public override float Angle => angle;
    public override bool IsMouseAction => true;

    public MouseSelectTarget() { }

    public MouseSelectTarget(MouseSelectTarget copy)
        : base(copy)
    {
        range = copy.range;
        angle = copy.angle;
    }


    // SelectImmidiateByPlayer �Լ��� �����ε� �߻� �Լ�, Posiiton ��� Screen Position�� ����.
    protected abstract TargetSelectionResult SelectImmediateByPlayer(Vector2 screenPoint, TargetSearcher targetSearcher, Entity requesterEntity,
    GameObject requesterObject);

    // ���ڷ� ���� position�� Screen Position���� ��ȯ�Ͽ�, �� SelectImmidiateByPlayer �Լ��� ������
    protected sealed override TargetSelectionResult SelectImmediateByPlayer(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, Vector3 position)
        => SelectImmediateByPlayer(Camera.main.WorldToScreenPoint(position), targetSearcher, requesterEntity, requesterObject);

    private void ResetMouseController()
    {
        Managers.Input.SubMouseAction(OnMouseLeftClick);
    }

    public override void Select(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, SelectCompletedHandler onSelectCompleted)
    {
        if (requesterEntity.IsPlayer)
        {
            this.targetSearcher = targetSearcher;
            this.requesterEntity = requesterEntity;
            this.requesterObject = requesterObject;
            this.onSelectCompleted = onSelectCompleted;

            Managers.Input.AddMouseAction(OnMouseLeftClick);
        }
        else
            onSelectCompleted.Invoke(SelectImmediateByAI(targetSearcher, requesterEntity,
                requesterObject, requesterEntity.Target.transform.position));
    }

    public override void CancelSelect(TargetSearcher targetSearcher)
    {
        ResetMouseController();
    }

    public override bool IsInRange(TargetSearcher targetSearcher, Entity requesterEntity, GameObject requesterObject, Vector3 targetPosition)
    {
        var requesterTransform = requesterObject.transform;
        targetPosition.y = requesterTransform.position.y;

        float sqrRange = range * range * (IsUseScale ? Scale : 1f);
        Vector3 relativePosition = targetPosition - requesterTransform.position;
        float angle = Vector3.Angle(relativePosition, requesterTransform.forward);
        bool IsInAngle = angle <= (Angle / 2f);

        // �˻� ������ �����̰ų�, target�� Range�� Angle�ȿ� �ִٸ� true
        return Mathf.Approximately(0f, range) ||
            (Vector3.SqrMagnitude(relativePosition) <= sqrRange && IsInAngle);
    }

    protected override IReadOnlyDictionary<string, string> GetStringsByKeyword()
    {
        var dictionary = new Dictionary<string, string>() { { "range", range.ToString("0.##") } };
        return dictionary;
    }

    private void OnMouseLeftClick(Vector2 mousePosition)
    {
        ResetMouseController();

        UnityHelper.Log_H(mousePosition);
        // SelectImmidiateByPlayer �Լ��� Mouse Position�� �־ ��� ���� Delegate�� ������ 
        onSelectCompleted?.Invoke(SelectImmediateByPlayer(mousePosition, targetSearcher, requesterEntity, requesterObject));
    }
}
