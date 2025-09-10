using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public class PuzzleBattleStateMachine
{
    public PuzzleBattleStateSO currentState { get; private set; }

    [SerializeField] private PuzzleBattleStateSO _stageStartState;
    [SerializeField] private PuzzleBattleStateSO _stageEndState;
    [SerializeField] private PuzzleBattleStateSO _battleStartState;
    [SerializeField] private PuzzleBattleStateSO _battleEndState;
    [SerializeField] private PuzzleBattleStateSO _stageMoveState;
    [SerializeField] private PuzzleBattleStateSO _successState;
    [SerializeField] private PuzzleBattleStateSO _failedState;
    [SerializeField] private List<PuzzleBattleStateSO> _turnLoopStates;

    private void Update()
    {
        currentState?.State?.Update(this);
    }

    private void ChangeState(PuzzleBattleStateSO nextState)
    {
        currentState?.State?.Exit(this);
        currentState = nextState;
        currentState?.State?.Enter(this);
    }

    #region Behavior
    public Action OnDungeonSuccess;
    public Action OnDungeonFailed;
    public Action<Character> OnEnemyDead;

    public Action<int, int> OnStageStarted;
    public Action<int, int> OnStageEnded;
    public Action OnBattleStart;
    public Action OnBattleEnd;
    public Action<int> OnBattleTurnStart;
    public Action<int> OnBattleTurnEnd;
    public Action<Player> OnPlayerActionStart;
    public Action<Player> OnPlayerActionEnd;
    public Action<Enemy> OnEnemyActionStart;
    public Action<Enemy> OnEnemyActionEnd;

    public Action<bool> OnPause;
    public bool IsEnded { get; private set; }
    private bool _pause = false;
    public bool Pause
    {
        get => _pause;
        set
        {
            _pause = value;
            OnPause?.Invoke(_pause);
        }
    }
    
    public InGameManager InGameManager { get; private set; }
    public DungeonTree DungeonTree { get; private set; }
    public DungeonNode CurrentNode => _currentNode;
    public List<DungeonNode> NextNodes
    {
        get
        {
            List<DungeonNode> nextNodes = new List<DungeonNode>();
            var nodes = DungeonTree.GetNodes();

            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i].PreviousNodes.Contains(CurrentNode))
                {
                    nextNodes.Add(nodes[i]);
                }
            }

            return nextNodes;
        }
    }
         
    public float StageDistance => CurrentNode?.Stage * _stageDistance ?? 0f;

    public List<Character> Enemies => enemies;
    public int EnemiesCount => enemies.FindAll(e => !e.IsNotDetect).Count;
    public int MaxStage() => DungeonTree.GetNodes().OrderBy(node => node.Stage).Last().Stage;
    public int MaxTurn() => 20;
    public DungeonNode GetDungeonNode(int stage, int index) => DungeonTree.GetNodes().FirstOrDefault(e => e.Stage == stage && e.Index == index);
    public float BeforeStageDistance => CurrentNode != null ? (CurrentNode.Stage - 1) * _stageDistance : 0f;
    public int CurrentTurnIndex;
    public readonly Vector3 _playerBattlePosition = new Vector3(-3.4f, 0, 0);
    public readonly Vector3 _playerEventPosition = new Vector3(-1f, 0, 0);
    public readonly Vector3 _eventStartPosition = new Vector3(0.5f, 0, 0);
    public readonly Vector3 _enemyStartPosition = new Vector3(2.2f, 0, 0);
    public readonly float _enemyCountStartX = -0.85f;
    public bool IsBattlling => _isBattlling;

    private List<Character> enemies = new List<Character>();
    private float _stageDistance = 14f;

    private DungeonNode _currentNode;
    private int _currentTurnLoopIndex = 0;
    private bool _isBattlling = false;

    public void Initialize(InGameManager inGameManager, DungeonTree dungeonTree)
    {
        InGameManager = inGameManager;
        DungeonTree = dungeonTree;
        
        _currentNode = GetDungeonNode(0, 0);
    }

    public void StageStart()
    {
        ChangeState(_stageStartState);
    }
    public void StageEnd()
    {
        ChangeState(_stageEndState);
    }
    public void StageMove()
    {
        ChangeState(_stageMoveState);
    }
    public void BattleStart()
    {
        _isBattlling = true;
        ChangeState(_battleStartState);
    }
    public void BattleEnd()
    {
        _isBattlling = false;

        if (_currentTurnLoopIndex != _turnLoopStates.Count - 1)
        {
            _currentTurnLoopIndex = _turnLoopStates.Count - 1;
            ChangeState(_turnLoopStates[_currentTurnLoopIndex]);
        }

        ChangeState(_battleEndState);
    }

    public void TurnLoopStart()
    {
        _currentTurnLoopIndex = 0;
        ChangeState(_turnLoopStates[_currentTurnLoopIndex]);
    }
    public void TurnLoopNext()
    {
        if (!_isBattlling)
        {
            return;
        }

        if (EnemiesCount <= 0)
        {
            BattleEnd();
            return;
        }

        if (InGameManager.Player.IsNotDetect)
        {
            Failed();
            return;
        }

        _currentTurnLoopIndex++;
        if (_currentTurnLoopIndex >= _turnLoopStates.Count)
        {
            _currentTurnLoopIndex = 0;
        }
        ChangeState(_turnLoopStates[_currentTurnLoopIndex]);
    }

    public void Success()
    {
        if (IsEnded)
            return;
            
        UnityHelper.Log_H("Success");
        IsEnded = true;
        ChangeState(_successState);
    }
    public void Failed()
    {
        if (IsEnded)
            return;
            
        UnityHelper.Log_H("Failed");
        IsEnded = true;
        ChangeState(_failedState);
    }

    public void StageNext(int stage, int index)
    {
        _currentNode = GetDungeonNode(stage, index);
        StageMove();
    }

    public void SetSpawnPosition()
    {
        bool isBattle = CurrentNode.DungeonRoom.IsEnemyRoom;
        
        if (!isBattle)
        {
            if (CurrentNode.DungeonRoom.EventObject)
            {
                Vector3 spawnPosition = _eventStartPosition;
                spawnPosition.x += StageDistance;
                
                GameObject eventObject = Managers.Resources.Instantiate(CurrentNode.DungeonRoom.EventObject);
                eventObject.transform.position = spawnPosition;
            }
        }
        else
        {
            List<float> enemyBoxWidths = new List<float>();
            List<Character> enemies = new List<Character>();

            for (int i = 0; i < CurrentNode.DungeonRoom.Enemies.Count; i++)
            {
                Enemy enemy = Managers.Resources.Instantiate<Enemy>(CurrentNode.DungeonRoom.Enemies[i]);
                enemyBoxWidths.Add(enemy.BoxWeidth);
                enemies.Add(enemy);
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].transform.position = GetEnemySpawnPosition(i, enemyBoxWidths);
                SettingEnemy(enemies[i]);
            }
        }
    }
    public Vector3 GetEnemySpawnPosition(int index, List<float> enemyBoxWidths)
    {
        float startX = enemyBoxWidths.Count * _enemyCountStartX;
        Vector3 spawnPosition = _enemyStartPosition;

        spawnPosition.x += StageDistance;
        spawnPosition.x += startX;

        for (int i = 0; i < index; i++)
        {
            spawnPosition.x += enemyBoxWidths[i];
        }
        spawnPosition.x += enemyBoxWidths[index] * 0.5f + 1.1f * index;

        return spawnPosition;
    }
    public void SettingEnemy(Character enemy, bool isInsert = false)
    {
        enemy.Initialize(DungeonTree);

        int level = CurrentNode.Stage;

        SetEnemyStat(enemy, level);
            
        enemy.OnDeadEnd -= OnEnemyDeadAction;
        enemy.OnDestroy -= OnEnemDestroy;
        enemy.OnDeadEnd += OnEnemyDeadAction;
        enemy.OnDestroy += OnEnemDestroy;
        
        if (isInsert)
        {
            Enemies.Insert(0, enemy);
        }
        else
        {
            Enemies.Add(enemy);
        }
    }

    private void SetEnemyStat(Character enemy, int level)
    {
        enemy.Stats.hpStat.SetBonusValue("Defulat", DungeonTree.DefaultHP);
        enemy.Stats.atkStat.SetBonusValue("Defulat", DungeonTree.DefaultATK);
            
        switch (enemy.enemyType)
        {
            case EnemyType.Normal:
                break;
            case EnemyType.Elite:
                enemy.Stats.hpStat.SetBonusValue("EnemyType", 2f);
                break;
            case EnemyType.Boss:
                enemy.Stats.hpStat.SetBonusValue("EnemyType", 5f);
                break;
        }

        BBNumber addHp = level * 0.9f + 1;
        BBNumber addAtk = level * 0.1f + 1;
            
        enemy.Stats.hpStat.SetBonusValue("Level", addHp);
        enemy.Stats.atkStat.SetBonusValue("Level", addAtk);
            
        enemy.HpBar.Initialize(enemy.Stats.MaxStatValue(enemy.Stats.hpStat), enemy.Stats.sequenceStat ? enemy.Stats.sequenceStat.MaxValue : 0);
    }


    public void EnemyAllDestroy()
    {
        while (Enemies.Count > 0)
            Enemies[0].Destroy();
    }
    public void OnEnemyDeadAction(Character character)
    {
        OnEnemyDead?.Invoke(character);
    }
    public void OnEnemDestroy(Character character)
    {
        if (enemies.Contains(character))
            enemies.Remove(character);
    }

    #endregion
}