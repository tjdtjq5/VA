using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SelectTarget : TargetSelectionAction
{
    [Header("Data")]
    // 검색 범위, 0일 경우 무한대를 의미함.
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
    public SelectTarget() { }

    public SelectTarget(SelectTarget copy)
        : base(copy)
    {
        range = copy.range;
    }

    protected abstract TargetSelectionResult SelectImmediateByPlayer(TargetSearcher targetSearcher, Entity requesterEntity,
 GameObject requesterObject);

    protected sealed override TargetSelectionResult SelectImmediateByPlayer(TargetSearcher targetSearcher, Entity requesterEntity,
    GameObject requesterObject, Vector3 position)
    => SelectImmediateByPlayer(targetSearcher, requesterEntity, requesterObject);

    protected abstract TargetSelectionResult SelectImmediateByAI(TargetSearcher targetSearcher, Entity requesterEntity,
GameObject requesterObject);

    protected sealed override TargetSelectionResult SelectImmediateByAI(TargetSearcher targetSearcher, Entity requesterEntity,
    GameObject requesterObject, Vector3 position)
    => SelectImmediateByAI(targetSearcher, requesterEntity, requesterObject);


    public override void Select(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject, SelectCompletedHandler onSelectCompleted)
    {
        this.targetSearcher = targetSearcher;
        this.requesterEntity = requesterEntity;
        this.requesterObject = requesterObject;
        this.onSelectCompleted = onSelectCompleted;

        if (requesterEntity.IsPlayer)
            onSelectCompleted?.Invoke(SelectImmediateByPlayer(targetSearcher, requesterEntity, requesterObject));
        else
            onSelectCompleted?.Invoke(SelectImmediateByAI(targetSearcher, requesterEntity, requesterObject));
    }
    public override bool IsInRange(TargetSearcher targetSearcher, Entity requesterEntity, GameObject requesterObject, Vector3 targetPosition)
    {
        var requesterTransform = requesterObject.transform;
        targetPosition.y = requesterTransform.position.y;

        float sqrRange = range * range * (IsUseScale ? Scale : 1f);
        Vector3 relativePosition = targetPosition - requesterTransform.position;
        float angle = Vector3.Angle(relativePosition, requesterTransform.forward);
        bool IsInAngle = angle <= (Angle / 2f);

        // 검색 범위가 무한이거나, target이 Range와 Angle안에 있다면 true
        return Mathf.Approximately(0f, range) ||
            (Vector3.SqrMagnitude(relativePosition) <= sqrRange && IsInAngle);
    }

    protected override IReadOnlyDictionary<string, string> GetStringsByKeyword()
    {
        var dictionary = new Dictionary<string, string>() { { "range", range.ToString("0.##") } };
        return dictionary;
    }
}
