using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBattleStateStageEnd : PuzzleBattleState
{
    private readonly string _skillPopup = "InGame/UIInGameSkill";

    public override void Enter(PuzzleBattleStateMachine fsm)
    {
        if (fsm.MaxStage() == fsm.CurrentNode.Stage)
        {
            fsm.Success();
            return;
        }

        DungeonNode dungeonNode = fsm.GetDungeonNode(fsm.CurrentNode.Stage, fsm.CurrentNode.Index);
        if (fsm.CurrentNode.Stage > 0 && dungeonNode.DungeonRoom.IsEnemyRoom)
        {
            UIInGameSkill uiInGameSkill = Managers.UI.ShopPopupUI<UIInGameSkill>(_skillPopup, CanvasOrderType.Middle); 

            if(dungeonNode.DungeonRoom.EnemyRoomType == EnemyRoomType.Normal)
            {
                uiInGameSkill.UISet(UIInGameSkillType.Basic);
            }
            else if(dungeonNode.DungeonRoom.EnemyRoomType == EnemyRoomType.Elite)
            {
                uiInGameSkill.UISet(UIInGameSkillType.Special);
            }

            uiInGameSkill.OnClose += () =>
            {
                fsm.InGameManager.BriefMapOpen(fsm.CurrentNode.Stage, fsm.CurrentNode.Index);
        
                for (int i = 0; i < fsm.Enemies.Count; i++)
                    fsm.Enemies[i].OnStageEnd.Invoke();
        
                fsm.InGameManager.Player.OnStageEnd.Invoke();
            };
        }
        else
        {
            fsm.InGameManager.BriefMapOpen(fsm.CurrentNode.Stage, fsm.CurrentNode.Index);
        
            for (int i = 0; i < fsm.Enemies.Count; i++)
                fsm.Enemies[i].OnStageEnd.Invoke();
        
            fsm.InGameManager.Player.OnStageEnd.Invoke();
        }
    }

    public override void Exit(PuzzleBattleStateMachine fsm)
    {
    }

    public override void Update(PuzzleBattleStateMachine fsm)
    {
    }
}
