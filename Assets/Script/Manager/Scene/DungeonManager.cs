using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : SceneBase
{
    protected override void Initialize()
    {
        base.Initialize();

        SceneType = SceneType.Dungeon;
    }
    public override void Clear()
    {
    }

    public override PlayerController GetPlayer()
    {
        return null;
    }

    public override int GetPlayerJobCount(Tribe job)
    {
        throw new System.NotImplementedException();
    }
}
