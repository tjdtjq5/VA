using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AIMove : PlayerMove
{
    private float checkTime = 0.1f;
    private float checkTimer;
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        checkTimer += Managers.Time.FixedDeltaTime;
        if (checkTimer >= checkTime)
            checkTimer = 0;
        else
            return;
        
        if (!IsBackMove)
        {
            if (Movetype == MoveType.Idle)
            {
                Character nearTarget = this.Character.NearestTarget();

                if (nearTarget)
                {
                    bool isTargetLeft = this.Transform.position.x > nearTarget.transform.position.x;
                    if (isTargetLeft)
                        AIDownLeft();
                    else
                        AIDownRight();
                }
            }
            else
            {
                Character nearTarget = this.Character.NearestTarget();

                if (nearTarget)
                {
                    bool isTargetLeft = this.Transform.position.x > nearTarget.transform.position.x;
                    if (isTargetLeft != IsLeft)
                        AIUp();
                    
                }else
                    AIUp();
            }
        }
    }

    void AIUp()
    {
        if (IsLeft)
            AIUpLeft();
        else
            AIUpRight();
    }

    void AIDownLeft()
    {
        _isLeftDown = true;
        LeftDown();
    }
    void AIUpLeft()
    {
        _isLeftDown = false;
        LeftUp();
    }
    void AIDownRight()
    {
        _isRightDown = true;
        RightDown();
    }
    void AIUpRight()
    {
        _isRightDown = false;
        RightUp();
    }
}
