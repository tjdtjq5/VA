using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunningState : EntityCCState
{
    private static readonly string kAnimationClipName = "isStunning";

    public override string Description => "Description";
    protected override string AnimationClipName => kAnimationClipName;
}
