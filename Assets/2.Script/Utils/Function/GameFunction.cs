using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameFunction
{
    #region SearchTarget
    public static Character SearchTarget(Character requester, bool isLeft, SearchTeamType searchTeamType)
    {
        List<Character> targets = SearchTargets(requester, isLeft, searchTeamType);

        if (targets.Count <= 0)
            return null;

        Vector3 requesterPos = requester.transform.position;
        targets = targets.OrderBy(t => Vector3.SqrMagnitude(t.transform.position - requesterPos)).ToList();
        return targets.FirstOrDefault();
    }
    public static List<Character> SearchTargets(Character requester, bool isLeft, SearchTeamType searchTeamType)
    {
        float radius = 1000f; 
        Vector3 requesterPos = requester.transform.position;
        Vector2 size = new Vector2(radius, radius);
        var castTargets = Physics2D.BoxCastAll(requesterPos, size, 0, Vector2.zero).ToList();
        
        List<Character> targets = new List<Character>();
        
        for (int i = 0; i < castTargets.Count; i++)
        {
            Character castTarget = castTargets[i].transform.GetComponent<Character>();

            if (!castTarget)
                continue;

            if (castTarget.IsNotDetect)
                continue;

            if (castTarget.gameObject != requester.gameObject)
            {
                if (isLeft && requester.transform.position.x < castTarget.transform.position.x)
                    continue;
                
                if (!isLeft && requester.transform.position.x > castTarget.transform.position.x)
                    continue;
                
                bool sameTeam = requester.IsPlayer == castTarget.IsPlayer;

                switch (searchTeamType)
                {
                    case SearchTeamType.All:
                        targets.Add(castTarget);
                        break;
                    case SearchTeamType.SameTeam:
                        if (sameTeam)
                            targets.Add(castTarget);
                        break;
                    case SearchTeamType.NotSameTeam:
                        if (!sameTeam)
                            targets.Add(castTarget);
                        break;
                }
            }
        }
        
        return targets;
    }
    public static Character SearchTarget(Character requester, float searchRadius, SearchTeamType searchTeamType)
    {
        List<Character> targets = SearchTargets(requester, searchRadius, searchTeamType);

        if (targets.Count <= 0)
            return null;

        Vector3 requesterPos = requester.transform.position;
        targets = targets.OrderBy(t => Vector3.SqrMagnitude(t.transform.position - requesterPos)).ToList();
        return targets.FirstOrDefault();
    }
    public static List<Character> SearchTargets(Character requester, float searchRadius, SearchTeamType searchTeamType)
    {
        Vector3 requesterPos = requester.transform.position;
        Vector2 size = new Vector2(searchRadius, searchRadius);
        var castTargets = Physics2D.BoxCastAll(requesterPos, size, 0, Vector2.zero).ToList();
        
        List<Character> targets = new List<Character>();
        
        for (int i = 0; i < castTargets.Count; i++)
        {
            Character castTarget = castTargets[i].transform.GetComponent<Character>();

            if (!castTarget)
                continue;

            if (castTarget.IsNotDetect)
                continue;

            if (castTarget.gameObject != requester.gameObject)
            {
                bool sameTeam = requester.IsPlayer == castTarget.IsPlayer;
                
                switch (searchTeamType)
                {
                    case SearchTeamType.All:
                        targets.Add(castTarget);
                        break;
                    case SearchTeamType.SameTeam:
                        if (sameTeam)
                            targets.Add(castTarget);
                        break;
                    case SearchTeamType.NotSameTeam:
                        if (!sameTeam)
                            targets.Add(castTarget);
                        break;
                }
            }
        }
        
        return targets;
    }
    public static List<Character> SearchTargets(float posX, float searchRadius, bool isPlayer)
    {
        Vector3 requesterPos = new Vector3(posX, 0, 0);
        var castTargets = Physics2D.BoxCastAll(requesterPos, new Vector2(searchRadius, searchRadius), 0, Vector3.zero).ToList();
        
        List<Character> targets = new List<Character>();
        
        for (int i = 0; i < castTargets.Count; i++)
        {
            Character castTarget = castTargets[i].transform.GetComponent<Character>();

            if (!castTarget)
                continue;

            if (castTarget.IsNotDetect)
                continue;

            if (isPlayer && castTarget.IsPlayer)
                targets.Add(castTarget);
            
            if (!isPlayer && !castTarget.IsPlayer)
                targets.Add(castTarget);
        }
        
        return targets;
    }
    public enum SearchTeamType
    {
        All,
        SameTeam,
        NotSameTeam,
    }
    #endregion
}
