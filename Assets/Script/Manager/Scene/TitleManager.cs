using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : SceneBase
{
    protected override void Initialize()
    {
        base.Initialize();

        SceneType = SceneType.Title;

        Managers.Resources.Instantiate("Prefab/UI/Popup/UILogin");
    }
    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
