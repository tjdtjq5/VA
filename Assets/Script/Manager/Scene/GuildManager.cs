using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildManager : SceneBase
{
    protected override void Initialize()
    {
        base.Initialize();

        SceneType = SceneType.Guild;
    }
    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
