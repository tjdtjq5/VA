using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleBattleState
{
    public abstract void Enter(PuzzleBattleStateMachine fsm);
    public abstract void Update(PuzzleBattleStateMachine fsm);
    public abstract void Exit(PuzzleBattleStateMachine fsm);
}
