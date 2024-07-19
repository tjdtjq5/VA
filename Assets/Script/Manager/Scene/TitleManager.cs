using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : SceneBase
{
    protected override void Initialize()
    {
        base.Initialize();

        SceneType = SceneType.Title;
    }
    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
