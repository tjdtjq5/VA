using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PuzzleBattleStateEnemyAction : PuzzleBattleState
{
    private PuzzleBattleStateMachine _fsm;
    private List<Character> _enemies;

    public override void Enter(PuzzleBattleStateMachine fsm)
    {
        _fsm = fsm;

        if (fsm.IsEnded)
            return;

        if (fsm.EnemiesCount <= 0)
        {
            fsm.BattleEnd();
            return;
        }
        
        if (!fsm.InGameManager.Player || fsm.InGameManager.Player.IsNotDetect)
        {
            fsm.Failed();
            return;
        }

        _enemies = fsm.Enemies.ToList();

        EnemyAction(_enemies[0]);
    }

    public override void Exit(PuzzleBattleStateMachine fsm)
    {
        
    }

    public override void Update(PuzzleBattleStateMachine fsm)
    {

    }

    private void EnemyAction(Character enemy)
    {
        enemy.OnDead += Next;

        enemy.CharacterSkill.OnSkillTimingEndDics[SkillTiming.CharacterActionStart] += () =>
        {
            BattleCharacterActionAttack(enemy, _fsm);
        };

        enemy.OnCharacterActionStart?.Invoke();
    }

    private void BattleCharacterActionAttack(Character enemy, PuzzleBattleStateMachine fsm)
    {
        if(enemy.IsNotDetect)
        {
            Next(enemy);
            return;
        }

        if (fsm.InGameManager.Player.IsNotDetect)
        {
            fsm.Failed();
            return;
        }

        enemy.CharacterAttack.OnEnd += () => Next(enemy);
        
        enemy.CharacterAttack.SetAttack(new List<Character>() { Managers.Observer.Player }, null);
        
        enemy.OnPuzzleAttackStart.Invoke(null);
    }

    private void Next(Character currentEnemy)
    {
        currentEnemy.OnDead -= Next;
        currentEnemy.OnCharacterActionEnd?.Invoke();

        _enemies.Remove(currentEnemy);

        if (_enemies.Count <= 0)
        {
            _fsm.TurnLoopNext();
            return;
        }

        Character nextEnemy = _enemies[0];
        if (nextEnemy.IsNotDetect)
        {
            Next(nextEnemy);
            return;
        }

        EnemyAction(nextEnemy);
    }
}
