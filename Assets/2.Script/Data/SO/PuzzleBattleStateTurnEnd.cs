using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleBattleStateTurnEnd : PuzzleBattleState
{
    public override void Enter(PuzzleBattleStateMachine fsm)
    {
        int turnIndex = fsm.CurrentTurnIndex;
        fsm.OnBattleTurnEnd?.Invoke(turnIndex);

        for (int i = 0; i < fsm.Enemies.Count; i++)
            fsm.Enemies[i].OnTurnEnd.Invoke(turnIndex);
        
        fsm.InGameManager.Player.OnTurnEnd.Invoke(turnIndex);
        
        fsm.TurnLoopNext();
    }

    public override void Exit(PuzzleBattleStateMachine fsm)
    {
        fsm.CurrentTurnIndex++;
    }

    public override void Update(PuzzleBattleStateMachine fsm)
    {
    }
}
