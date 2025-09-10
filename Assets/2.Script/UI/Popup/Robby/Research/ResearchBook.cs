using System.Collections;
using System.Collections.Generic;
using Shared.Enums;
using UnityEngine;

public class ResearchBook : UIButton
{
    public void UISet(PlayerGrowResearch type, bool isBig)
    {
        var sprite = Managers.Atlas.GetResearchBook(type.ToString(), isBig);
        this.Image.sprite = sprite;
    }
}

