using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SelectFarestEntity : SelectTarget
{
    [SerializeField]
    private float castRadius;
    // Target이 검색을 요청한 Entity와 같은 Category를 가지고 있어야하는가?
    [SerializeField]
    private bool isSelectSameCategory;

    public SelectFarestEntity() { }

    public SelectFarestEntity(SelectFarestEntity copy)
        : base(copy)
    {
        castRadius = copy.castRadius;
        isSelectSameCategory = copy.isSelectSameCategory;
    }

    protected override TargetSelectionResult SelectImmediateByPlayer(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject)
    {
        return FarestSelect(targetSearcher, requesterEntity, requesterObject);
    }

    protected override TargetSelectionResult SelectImmediateByAI(TargetSearcher targetSearcher, Entity requesterEntity,
        GameObject requesterObject)
    {
        return FarestSelect(targetSearcher, requesterEntity, requesterObject);
    }

    TargetSelectionResult FarestSelect(TargetSearcher targetSearcher, Entity requesterEntity,
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

            // 요청자 본인일 경우 제외
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

        var target = targets.Last();

        if (!target)
            return new TargetSelectionResult(requesterPos, SearchResultMessage.Fail);

        requesterEntity.Target = target;

        if (targetSearcher.IsInRange(requesterEntity, requesterObject, target.transform.position))
            return new TargetSelectionResult(target.gameObject, SearchResultMessage.FindTarget);
        else
            return new TargetSelectionResult(target.gameObject, SearchResultMessage.OutOfRange);
    }

    public override object Clone() => new SelectFarestEntity(this);
}
