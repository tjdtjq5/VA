using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : UIBase
{
    enum Buttons
    {
        PointButton
    }
    enum Texts : int
    {
        PointText,
        ScoreText,
    }

    enum GameObjects
    {
        TestObj
    }

    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        Get<Button>(Buttons.PointButton);
    }
}
