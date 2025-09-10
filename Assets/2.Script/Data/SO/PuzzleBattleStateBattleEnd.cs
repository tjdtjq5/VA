using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleBattleStateBattleEnd : PuzzleBattleState
{ 
    public override void Enter(PuzzleBattleStateMachine fsm)
    {
        fsm.OnBattleEnd?.Invoke();

        fsm.InGameManager.Puzzle.Close();
        fsm.InGameManager.UIBattle.TurnClose();

        for (int i = 0; i < fsm.Enemies.Count; i++)
            fsm.Enemies[i].OnBattleEnd.Invoke();
        
        fsm.InGameManager.Player.OnBattleEnd.Invoke();

        // 돈 생성 스폰을 기다리기 위해 딜레이 추가
        Managers.Tween.TweenInvoke(1f).SetOnComplete(() =>
        {
            Managers.Observer.CameraController.SetTarget(Managers.Observer.Player.transform);
            Managers.Observer.CameraController.SetFieldOfView(0, true);

            fsm.StageEnd();
        });
    }

    public override void Exit(PuzzleBattleStateMachine fsm)
    {
    }

    public override void Update(PuzzleBattleStateMachine fsm)
    {
    }
}
