using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameFunction
{
    //public static Entity SearchTarget(Entity requester, float searchRadius, bool isSameCategory = false)
    //{
    //    List<Entity> targets = SearchTargets(requester, searchRadius, isSameCategory);

    //    if (targets.Count <= 0)
    //        return null;

    //    Vector3 requesterPos = requester.transform.position;
    //    targets = targets.OrderBy(t => Vector3.SqrMagnitude(t.transform.position - requesterPos)).ToList();
    //    return targets.FirstOrDefault();
    //}
    //public static List<Entity> SearchTargets(Entity requester, float searchRadius, bool isSameCategory = false)
    //{
    //    Vector3 requesterPos = requester.transform.position;
    //    var castTargets = Physics.SphereCastAll(requesterPos, searchRadius, Vector3.up, 0f).ToList();
    //    List<Entity> targets = new List<Entity>();

    //    for (int i = 0; i < castTargets.Count; i++)
    //    {
    //        Entity castTarget = castTargets[i].transform.GetComponent<Entity>();

    //        if (!castTarget)
    //            continue;

    //        if (castTarget.IsDead)
    //            continue;

    //        if (castTarget.gameObject != requester.gameObject)
    //        {
    //            var hasCategory = requester.Categories.Any(x => castTarget.HasCategory(x));

    //            if ((hasCategory && isSameCategory) || (!hasCategory && !isSameCategory))
    //                targets.Add(castTarget);
    //        }
    //    }

    //    return targets;
    //}
}
