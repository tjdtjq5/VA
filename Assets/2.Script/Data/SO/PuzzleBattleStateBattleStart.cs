using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleBattleStateBattleStart : PuzzleBattleState
{
    public override void Enter(PuzzleBattleStateMachine fsm)
    {
        fsm.CurrentTurnIndex = 0;
        fsm.OnBattleStart?.Invoke();

        fsm.InGameManager.Puzzle.Open();
        
        for (int i = 0; i < fsm.Enemies.Count; i++)
            fsm.Enemies[i].OnBattleStart.Invoke();
        
        fsm.InGameManager.Player.OnBattleStart.Invoke();

        fsm.TurnLoopStart();
    }

    public override void Exit(PuzzleBattleStateMachine fsm)
    {
    }

    public override void Update(PuzzleBattleStateMachine fsm)
    {
    }
}
