using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SelectNearestEntity : SelectTarget
{
    [SerializeField]
    private float castRadius;

    public SelectNearestEntity() { }

    public SelectNearestEntity(SelectNearestEntity copy)
        : base(copy)
    {
        castRadius = copy.castRadius;
    }

    protected override TargetSelectionResult SelectImmediateByPlayer(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject)
    {
        return NearestSelect(targetSearcher, requesterEntity, requesterObject);
    }

    protected override TargetSelectionResult SelectImmediateByAI(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject)
    {
        return NearestSelect(targetSearcher, requesterEntity, requesterObject);
    }

    TargetSelectionResult NearestSelect(TargetSearcher targetSearcher, Entity requesterEntity,
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

            if (castTarget.transform.position != requesterObject.transform.position)
            {
                var hasCategory = requesterEntity.Categories.Any(x => castTarget.HasCategory(x));

                if ((hasCategory && isSelectSameCategory) || (!hasCategory && !isSelectSameCategory))
                    targets.Add(castTarget);
            }
        }

        if (targets.Count <= 0)
            return new TargetSelectionResult(requesterPos, SearchResultMessage.Fail);

        targets = targets.OrderBy(t => Vector3.SqrMagnitude(t.transform.position - requesterPos)).ToList();

        var target = targets.First();

        if (!target)
            return new TargetSelectionResult(requesterPos, SearchResultMessage.Fail);

        requesterEntity.Target = target;


        if (targetSearcher.IsInRange(requesterEntity, requesterObject, target.transform.position))
            return new TargetSelectionResult(target.gameObject, SearchResultMessage.FindTarget);
        else
            return new TargetSelectionResult(target.gameObject, SearchResultMessage.OutOfRange);
    }

    public override object Clone() => new SelectNearestEntity(this);
}
