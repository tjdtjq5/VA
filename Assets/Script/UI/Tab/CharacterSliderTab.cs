using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSliderTab : UITabSlider
{
    protected override IReadOnlyList<string> TabNames => new List<string>() { "����", "��" };

    protected override void Initialize()
    {
        base.Initialize();
    }
}
