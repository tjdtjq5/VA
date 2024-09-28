using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SelectRandomEntity : SelectTarget
{
    [SerializeField]
    private float castRadius;

    public SelectRandomEntity() { }

    public SelectRandomEntity(SelectRandomEntity copy)
        : base(copy)
    {
        castRadius = copy.castRadius;
    }

    protected override TargetSelectionResult SelectImmediateByPlayer(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject)
    {
        return RandomSelect(targetSearcher, requesterEntity, requesterObject);
    }

    protected override TargetSelectionResult SelectImmediateByAI(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject)
    {
        return RandomSelect(targetSearcher, requesterEntity, requesterObject);
    }

    TargetSelectionResult RandomSelect(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject)
    {
        Vector3 requesterPos = requesterObject.transform.position;
        var castTargets = Physics.SphereCastAll(requesterPos, castRadius, Vector3.up, 0f).ToList();
        List<Entity> targets = new List<Entity>();

        for (int i = 0; i < castTargets.Count; i++)
        {
            Entity castTarget = castTargets[i].transform.GetComponent<Entity>();

            if (!castTarget)
                continue;

            if (castTarget.IsDead)
                continue;

            if (castTarget.gameObject != requesterObject.gameObject)
            {
                var hasCategory = requesterEntity.Categories.Any(x => castTarget.HasCategory(x));

                if ((hasCategory && isSelectSameCategory) || (!hasCategory && !isSelectSameCategory))
                    targets.Add(castTarget);
            }
        }
        if (targets.Count <= 0)
            return new TargetSelectionResult(requesterPos, SearchResultMessage.Fail);

        var rangeInTargets = targets.FindAll(t => (targetSearcher.IsInRange(requesterEntity, requesterObject, t.transform.position)));

        if (rangeInTargets.Count > 0)
        {
            var rangeInFarTarget = rangeInTargets[Random.Range(0, rangeInTargets.Count)];

            if (!rangeInFarTarget)
                return new TargetSelectionResult(requesterPos, SearchResultMessage.Fail);
            else
                return new TargetSelectionResult(rangeInFarTarget.gameObject, SearchResultMessage.FindTarget);
        }
        else
        {
            var rangeOutTargets = targets.OrderBy(t => Vector3.SqrMagnitude(t.transform.position - requesterPos)).ToList();
            var rangeOutTarget = targets.FirstOrDefault();

            if (!rangeOutTarget)
                return new TargetSelectionResult(requesterPos, SearchResultMessage.Fail);
            else
                return new TargetSelectionResult(rangeOutTarget.gameObject, SearchResultMessage.OutOfRange);
        }
    }

    public override object Clone() => new SelectRandomEntity(this);
}
