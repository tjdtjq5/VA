using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : SceneBase
{
    protected override void Initialize()
    {
        base.Initialize();

        SceneType = SceneType.InGame;
    }
    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
