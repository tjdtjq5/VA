using System;
using System.Collections.Generic;
using System.Linq;
using Shared.BBNumber;
using UnityEngine;

[System.Serializable]
public abstract class DungeonBehaviour
{
    public Action OnDungeonSuccess;
    public Action OnDungeonFailed;
    public Action<Character> OnEnemyDead;
    public Action<int, int> OnStageStarted;
    public Action<int, int> OnStageEnded;
    public Action OnBattleStart;
    public Action OnBattleEnd;
    public Action<int> OnBattleTurnStart;
    public Action<int> OnBattleTurnEnd;
    public Action<int, Character> OnBattleCharacterActionStart;
    public Action<int, Character> OnBattleCharacterActionEnd;
    protected Action<bool> OnPause;
    public bool IsEnded { get; protected set; }
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
    
    protected DungeonTree DungeonTree;
    protected List<Character> BattleCharacters = new List<Character>();
    protected List<Character> enemies = new List<Character>();

    public List<Character> Enemies => enemies;
    public int EnemiesCount => enemies.FindAll(e => !e.IsNotDetect).Count;
    public int MaxStage() => DungeonTree.GetNodes().OrderBy(node => node.Stage).Last().Stage;
    public virtual int MaxTurn() => 0;
    public DungeonNode GetDungeonNode(int stage, int index) => DungeonTree.GetNodes().FirstOrDefault(e => e.Stage == stage && e.Index == index);
    private int _battleTurnIndex = 0;
    

    public virtual void Start(DungeonTree dungeonTree)
    {
        this.DungeonTree = dungeonTree;
        OnBattleCharacterActionStart += BattleCharacterActionStart;
        OnBattleCharacterActionEnd += BattleCharacterActionEnd;
    }
    public virtual void FixedUpdate() { }
    public abstract void End();

    public virtual void Success()
    {
        End();
        OnDungeonSuccess?.Invoke();
    }
    public virtual void Failed()
    {
        End();
        OnDungeonFailed?.Invoke();
    }

    protected abstract void StageStart();
    public abstract void StageEnd();
    protected virtual void BattleStart(List<Character> playerCharacters)
    {
        BattleCharacters = playerCharacters;
        BattleCharacters.AddRange(enemies);
        
        OnBattleStart?.Invoke();

        _battleTurnIndex = 0;

        BattleTurnStart();
    }
    protected virtual void BattleEnd()
    {
        OnBattleEnd?.Invoke();
    }
    protected virtual void BattleTurnStart()
    {
        _battleTurnIndex++;
        
        OnBattleTurnStart?.Invoke(_battleTurnIndex);
        
        OnBattleCharacterActionStart?.Invoke(0, BattleCharacters[0]);
    }
    protected virtual void BattleTurnEnd()
    {
        OnBattleTurnEnd?.Invoke(_battleTurnIndex);
        
        if (EnemiesCount > 0)
        {
            BattleTurnStart();
        }
        else
        {
            BattleEnd();
        }
    }
    protected virtual void BattleCharacterActionStart(int order, Character character)
    {
    }
    protected virtual void BattleCharacterActionEnd(int order, Character character)
    {
        int nextOrder = order + 1;

        if (BattleCharacters.Count > nextOrder)
        {
            OnBattleCharacterActionStart?.Invoke(nextOrder, BattleCharacters[nextOrder]);
        }
        else
        {
            BattleTurnEnd();
        }
    }

    protected virtual void SetEnemyStat(Character enemy, int level)
    {
        enemy.Stats.hpStat.SetBonusValue("Defulat", 350);
        enemy.Stats.atkStat.SetBonusValue("Defulat", 50);
            
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
    protected virtual void SpawnEnemy(Character enemy, Vector3 position, int level)
    {
        enemy.Initialize(DungeonTree);
        enemy.transform.position = position;
        SetEnemyStat(enemy, level);
            
        enemy.OnDead += OnEnemyDeadAction;
        enemy.OnDestroy += OnEnemDestroy;
        
        enemies.Add(enemy);
    }

    protected virtual void EnemyAllDestroy()
    {
        while (enemies.Count > 0)
            enemies[0].Destroy();
    }
    protected virtual void OnEnemyDeadAction(Character character)
    {
        OnEnemyDead?.Invoke(character);
    }
    protected virtual void OnEnemDestroy(Character character)
    {
        if (enemies.Contains(character))
            enemies.Remove(character);
    }
}