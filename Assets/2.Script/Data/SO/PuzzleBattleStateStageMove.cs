using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBattleStateStageMove : PuzzleBattleState
{

    private readonly string _battlePrefabPath = "Prefab/UI/Etc/InGame/Battle";

    public override void Enter(PuzzleBattleStateMachine fsm)
    {
        // 적 생성 or NPC 생성
        fsm.EnemyAllDestroy();
        fsm.SetSpawnPosition();

        // 플레이어 이동
        Vector3 middlePosition = new Vector3(fsm.BeforeStageDistance, 0, 0);
        Managers.Observer.Player.CharacterMove.SetMoveDontStopMotion(middlePosition, () => MoveMiddle(fsm));
    }

    public override void Exit(PuzzleBattleStateMachine fsm)
    {
    }

    public override void Update(PuzzleBattleStateMachine fsm)
    {

    }

    private void MoveMiddle(PuzzleBattleStateMachine fsm)
    {
        bool isBattle = fsm.CurrentNode.DungeonRoom.IsEnemyRoom;
        
        Vector3 nextPosition = isBattle ? fsm._playerBattlePosition : fsm._playerEventPosition;
        nextPosition.x += fsm.StageDistance;
        
        Managers.Observer.Player.CharacterMove.SetMoveActionEnd(nextPosition, () => MoveEnd(fsm));
        
        Managers.Observer.CameraController.SetTarget(Managers.Observer.Player.transform);
    }
    private void MoveEnd(PuzzleBattleStateMachine fsm)
    {
        float cameraMoveX = 0f;

        if(fsm.CurrentNode.DungeonRoom.IsEnemyRoom)
        {
            float playerBoxLeftX = fsm.InGameManager.Player.transform.position.x - fsm.InGameManager.Player.BoxWeidth * 0.5f;
            float lastEnemyBoxRightX = fsm.Enemies[fsm.Enemies.Count - 1].transform.position.x + fsm.Enemies[fsm.Enemies.Count - 1].BoxWeidth * 0.5f;

            cameraMoveX = Mathf.Lerp(playerBoxLeftX, lastEnemyBoxRightX, 0.5f);

            Managers.Observer.CameraController.SetFieldOfViewByTargets(fsm.InGameManager.Player, fsm.Enemies[fsm.Enemies.Count - 1], true);
        }
        else
        {
            cameraMoveX = fsm.StageDistance;
        }

        Managers.Observer.CameraController.Speed = Managers.Observer.CameraController.HalfSpeed;
        Managers.Observer.CameraController.SetMove(cameraMoveX, () => CameraEnd(fsm));
        fsm.StageStart();
    }
    private void CameraEnd(PuzzleBattleStateMachine fsm)
    {
        if (!Managers.Observer.Player.IsChanging)
            Managers.Observer.Player.SetIdle();
            
        Managers.Observer.CameraController.Speed = Managers.Observer.CameraController.OriginSpeed;

        if(fsm.CurrentNode.DungeonRoom != null && fsm.CurrentNode.DungeonRoom.IsEnemyRoom)
        {
            Battle battle = Managers.Resources.Instantiate<Battle>(_battlePrefabPath);
            battle.StartBattle(fsm.CurrentNode.DungeonRoom.EnemyRoomType);
        }
    }
}
