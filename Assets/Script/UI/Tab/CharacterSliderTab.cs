using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSliderTab : UITabSlider
{
    protected override IReadOnlyList<string> TabNames => new List<string>() { "관리", "편성" };

    protected override void Initialize()
    {
        base.Initialize();
    }
}
