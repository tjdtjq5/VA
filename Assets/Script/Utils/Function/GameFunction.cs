using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameFunction
{
    #region SearchTarget
    public static Character SearchTarget(Character requester, bool isLeft, bool isSameTeam = false)
    {
        List<Character> targets = SearchTargets(requester, isLeft, isSameTeam);

        if (targets.Count <= 0)
            return null;

        Vector3 requesterPos = requester.transform.position;
        targets = targets.OrderBy(t => Vector3.SqrMagnitude(t.transform.position - requesterPos)).ToList();
        return targets.FirstOrDefault();
    }
    public static List<Character> SearchTargets(Character requester, bool isLeft, bool isSameTeam = false)
    {
        float radius = 1000f; 
        Vector3 requesterPos = requester.transform.position;
        var castTargets = Physics2D.BoxCastAll(requesterPos, new Vector2(radius, radius), 0, Vector3.zero).ToList();
        
        List<Character> targets = new List<Character>();
        
        for (int i = 0; i < castTargets.Count; i++)
        {
            Character castTarget = castTargets[i].transform.GetComponent<Character>();

            if (!castTarget)
                continue;

            if (castTarget.IsDead)
                continue;

            if (castTarget.gameObject != requester.gameObject)
            {
                if (isLeft && requester.transform.position.x < castTarget.transform.position.x)
                    continue;
                
                if (!isLeft && requester.transform.position.x > castTarget.transform.position.x)
                    continue;
                
                bool sameTeam = requester.team == castTarget.team;
                if ((sameTeam && isSameTeam) || (!sameTeam && !isSameTeam))
                    targets.Add(castTarget);
            }
        }
        
        return targets;
    }
    public static Character SearchTarget(Character requester, float searchRadius, bool isSameTeam = false)
    {
        List<Character> targets = SearchTargets(requester, searchRadius, isSameTeam);

        if (targets.Count <= 0)
            return null;

        Vector3 requesterPos = requester.transform.position;
        targets = targets.OrderBy(t => Vector3.SqrMagnitude(t.transform.position - requesterPos)).ToList();
        return targets.FirstOrDefault();
    }
    public static List<Character> SearchTargets(Character requester, float searchRadius, bool isSameTeam = false)
    {
        Vector3 requesterPos = requester.transform.position;
        var castTargets = Physics2D.BoxCastAll(requesterPos, new Vector2(searchRadius, searchRadius), 0, Vector3.zero).ToList();
        
        List<Character> targets = new List<Character>();
        
        for (int i = 0; i < castTargets.Count; i++)
        {
            Character castTarget = castTargets[i].transform.GetComponent<Character>();

            if (!castTarget)
                continue;

            if (castTarget.IsDead)
                continue;

            if (castTarget.gameObject != requester.gameObject)
            {
                bool sameTeam = requester.team == castTarget.team;
                if ((sameTeam && isSameTeam) || (!sameTeam && !isSameTeam))
                    targets.Add(castTarget);
            }
        }
        
        return targets;
    }
    #endregion
}
