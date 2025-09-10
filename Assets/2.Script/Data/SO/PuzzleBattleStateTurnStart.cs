using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleBattleStateTurnStart : PuzzleBattleState
{
    public override void Enter(PuzzleBattleStateMachine fsm)
    {
        int turnIndex = fsm.CurrentTurnIndex;
        fsm.OnBattleTurnStart?.Invoke(turnIndex);

        if (turnIndex >= fsm.MaxTurn())
        {
            fsm.Failed();
            return;
        }
        
        fsm.InGameManager.UIBattle.TurnOpen();
        fsm.InGameManager.UIBattle.TurnUISet(turnIndex, fsm.MaxTurn(), turnIndex > 1);

        for (int i = 0; i < fsm.Enemies.Count; i++)
            fsm.Enemies[i].OnTurnStart.Invoke(turnIndex);
        
        fsm.InGameManager.Player.OnTurnStart.Invoke(turnIndex);

        fsm.TurnLoopNext();
    }

    public override void Exit(PuzzleBattleStateMachine fsm)
    {
        
    }

    public override void Update(PuzzleBattleStateMachine fsm)
    {
    }
}
