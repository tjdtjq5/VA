using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAttackReBack : EnemyAttack
{
    private readonly string _moveEvent = "move";
    private readonly string _moveReName = "Re";

    public override void Initialize(Character character, Transform transform)
    {
        base.Initialize(character, transform);

        for (int i = 0; i < Character.SpineSpineAniControllers.Count; i++)
        {
            Character.SpineSpineAniControllers[i].SetEventFunc(_attackName, _moveEvent , MovePos);
            Character.SpineSpineAniControllers[i].SetEventFunc(_moveReName, _moveEvent , ReMovePos);
            Character.SpineSpineAniControllers[i].SetEndFunc(_moveReName, OnAttackEnd);
        }
    }

    protected override void SetTargetAttack(Character target, object cause)
    {
        StandAttack();
        StartAction(target, cause);
    }

    private void MovePos()
    {
        Vector3 pos = GetTargetPosition(Targets[0], true);
        pos.x += 0.5f;
        TimeMove(null, pos, 0.35f);
    }
    private void ReMovePos()
    {
        TimeMove(null, _moveStartPos, 0.35f);
    }

    protected override void AttackAniEnd()
    {
        this.Character.CharacterMove.SetSpeed(1f);
        SetAnimation(_moveReName, false);
    }
}
