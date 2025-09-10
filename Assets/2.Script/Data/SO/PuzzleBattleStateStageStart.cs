using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBattleStateStageStart : PuzzleBattleState
{
    private int _currentStartPopupIndex = 0;

    public override void Enter(PuzzleBattleStateMachine fsm)
    {
        fsm.OnStageStarted?.Invoke(fsm.CurrentNode.Stage, fsm.CurrentNode.Index);
        
        _currentStartPopupIndex = 0;
        StagePlay(fsm);
    }

    public override void Exit(PuzzleBattleStateMachine fsm)
    {
    }

    public override void Update(PuzzleBattleStateMachine fsm)
    {
    }

    void StagePlay(PuzzleBattleStateMachine fsm)
    {
        if(fsm.CurrentNode.DungeonRoom == null)
        {
            fsm.StageEnd();
            return;
        }

        if (fsm.CurrentNode.DungeonRoom.EventPopups.Count > _currentStartPopupIndex)
        {
            UIPopup popup = fsm.CurrentNode.DungeonRoom.EventPopups[_currentStartPopupIndex];
            
            if (!popup)
            {
                _currentStartPopupIndex++;
                StagePlay(fsm);
                return;
            }

            Managers.Tween.TweenInvoke(1.5f).SetOnComplete(() =>
            {
                popup = Managers.UI.ShopPopupUI<UIPopup>(popup, CanvasOrderType.Middle);
                popup.OnClose += () =>
                {
                    _currentStartPopupIndex++;
                    StagePlay(fsm);
                };
            });
        }
        else
        {
            // Battle
            if (fsm.EnemiesCount > 0)
            {
                fsm.BattleStart();
            }
            else
            {
                fsm.StageEnd();
            }
        }
    }
}
