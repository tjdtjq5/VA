using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingState : SkillState
{
    // Charge ���°� ����Ǿ��°�? true��� �ٸ� State�� ���̵�
    public bool IsChargeEnded { get; private set; }
    // Charge�� �ּ� �������� ä����, Skill�� ������ �˻��� �����ߴ°�?(=Charge�� ��ġ�� Skill�� ���Ǿ��°�?)
    // ���� ���������� true��� �ٸ� State�� ���̵�
    public bool IsChargeSuccessed { get; private set; }

    public override void Enter()
    {
        Entity.Activate();

        if (Entity.TargetSearcher.IsMouseAction)
        {
            if (Entity.Owner.IsPlayer)
            {
                Entity.SelectTarget(OnTargetSearchCompleted, false);
            }
        }

        Entity.ShowIndicator();
        Entity.StartCustomActions(SkillCustomActionType.Charge);

        TrySendCommandToOwner(Entity, EntityStateCommand.ToChargingSkillState, Entity.ChargeAnimationClipName);
    }

    public override void FixedUpdate()
    {
        Entity.CurrentChargeDuration += Managers.Time.FixedDeltaTime;

        if (!Entity.Owner.IsPlayer && Entity.IsMaxChargeCompleted)
        {
            IsChargeEnded = true;
            Entity.SelectTarget(false);
            TryUse();
        }
        else if (Entity.IsChargeDurationEnded)
        {
            IsChargeEnded = true;
            if (Entity.ChargeFinishActionOption == SkillChargeFinishActionOption.Use)
            {
                //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity))
                //    Entity.SelectTargetImmediate(hitInfo.point);

                Entity.SelectTarget(OnTargetSearchCompleted, false);

                TryUse();
            }
        }

        Entity.RunCustomActions(SkillCustomActionType.Charge);
    }

    public override void Exit()
    {
        IsChargeEnded = false;
        IsChargeSuccessed = false;

        if (Entity.IsSearchingTarget)
            Entity.CancelSelectTarget();
        else
            Entity.HideIndicator();

        Entity.ReleaseCustomActions(SkillCustomActionType.Charge);
    }

    private bool TryUse()
    {
        if (Entity.IsMinChargeCompleted && Entity.IsTargetSelectSuccessful)
            IsChargeSuccessed = true;

        return IsChargeSuccessed;
    }

    //private void OnTargetSearchCompleted(Skill skill, TargetSearcher searcher, TargetSelectionResult result)
    //{
    //    if (!TryUse())
    //        Entity.SelectTarget(OnTargetSearchCompleted, false);
    //}
    private void OnTargetSearchCompleted(Skill skill, TargetSearcher searcher, TargetSelectionResult result)
    {
        UnityHelper.Log_H(result.resultMessage);
    }
}
