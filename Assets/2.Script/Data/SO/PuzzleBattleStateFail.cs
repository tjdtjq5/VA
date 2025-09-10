using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

public class PuzzleBattleStateFail : PuzzleBattleState
{
    public override void Enter(PuzzleBattleStateMachine fsm)
    {
        UnityHelper.Log_H("OnFailed");
        fsm.InGameManager.UIBattle.TurnClose();
        fsm.InGameManager.Puzzle.Close();
        
        UIInGameResult result = Managers.UI.ShopPopupUI<UIInGameResult>("InGame/UIInGameResult", CanvasOrderType.Middle);
        result.UISet(UIInGameResultType.Lose, 10, new Dictionary<string, BBNumber>() {
            { "Gesso" , 100},  { "Nesso" , 100}});
    }

    public override void Exit(PuzzleBattleStateMachine fsm)
    {
    }

    public override void Update(PuzzleBattleStateMachine fsm)
    {
    }
}
