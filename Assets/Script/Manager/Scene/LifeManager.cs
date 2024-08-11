using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : SceneBase
{
    protected override void Initialize()
    {
        base.Initialize();

        SceneType = SceneType.Life;
    }
    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
