using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIMove : Move
{
    public override void Initialize(Transform transform, SpineAniController spineAniController)
    {
        this.Transform = transform;    
        this.SpineAniController = spineAniController;
    }

    public override void SetIdle()
    {
        throw new NotImplementedException();
    }

    public override void FixedUpdate()
    {
        throw new System.NotImplementedException();
    }
}
