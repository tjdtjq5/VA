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
        throw new System.NotImplementedException();
    }
}
