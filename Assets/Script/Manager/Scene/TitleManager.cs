using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : SceneBase
{
    protected override void Initialize()
    {
        base.Initialize();

        SceneType = SceneType.Title;

        // Managers.Resources.Instantiate("Prefab/UI/Popup/UILogin");

        UILoginTest ult = Managers.Resources.Instantiate<UILoginTest>("Prefab/UI/Popup/UILoginTest");
        ult.LoginAfterJob(() => 
        {
            Managers.Resources.Destroy(ult.gameObject);

            UILoginFuncTest ulft = Managers.Resources.Instantiate<UILoginFuncTest>("Prefab/UI/Popup/UILoginFuncTest");
        });
    }

    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
