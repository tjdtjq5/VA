using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

public class RewardCard : UICard
{
    public override void Setting(ICardData data)
    {
        throw new System.NotImplementedException();
    }
}

public class RewardCardData : ICardData
{
    public string Item;
    public BBNumber Count;
}
