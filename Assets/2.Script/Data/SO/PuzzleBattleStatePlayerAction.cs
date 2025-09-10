using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleBattleStatePlayerAction : PuzzleBattleState
{
    private PuzzleBattleStateMachine _fsm;
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

        fsm.InGameManager.Player.OnDead += Next;

        fsm.InGameManager.Player.CharacterSkill.OnSkillTimingEndDics[SkillTiming.CharacterActionStart] += () =>
        {
            BattleCharacterActionAttack(fsm);
        };

        fsm.InGameManager.Player.OnCharacterActionStart?.Invoke();
    }

    public override void Exit(PuzzleBattleStateMachine fsm)
    {
    }

    public override void Update(PuzzleBattleStateMachine fsm)
    {

    }
    private void BattleCharacterActionAttack(PuzzleBattleStateMachine fsm)
    {
        if(fsm.InGameManager.Player.IsNotDetect)
        {
            fsm.Failed();
            return;
        }

        
        fsm.InGameManager.Player.CharacterSkill.OnSkillTimingEndDics[SkillTiming.PuzzleAttackEnd] += () =>
        {
            Next(fsm.InGameManager.Player);
        };
        
        fsm.InGameManager.Puzzle.Switch = true;
        fsm.InGameManager.Puzzle.OnPuzzlePointUp += PuzzlePointUp;

        if (fsm.InGameManager.Player.IsOnTriggerPassiveBuff(TriggerPassiveBuff.PerfectPath))
        {
            fsm.InGameManager.Puzzle.PerfectTrail();
        }
    }

    void PuzzlePointUp(PuzzleData data, int combo, int forceCount, int itemCount)
    {
        List<Character> enemies = _fsm.Enemies;

        List<Character> targets = new List<Character>();
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].IsNotDetect)
                continue;
            
            targets.Add(enemies[i]);
        }

        if (targets.Count <= 0)
        {
            UnityHelper.Log_H($"Enemies are not detected");
            _fsm.BattleEnd();
            return;
        }
        
        PuzzleAttackData cause = new PuzzleAttackData() { data = data, combo = combo, forceCount = forceCount, itemCount = itemCount, isSequence = false };
        
        _fsm.InGameManager.Player.CharacterSkill.OnSkillTimingEndDics[SkillTiming.PuzzleAttackStart] += () =>
        {
            _fsm.InGameManager.Player.CharacterAttack.SetAttack(targets, cause);
        };

        _fsm.InGameManager.Player.OnPuzzleAttackStart.Invoke(cause);
        
        _fsm.InGameManager.Player.CharacterAttack.OnEnd += () => { _fsm.InGameManager.Player.OnPuzzleAttackEnd.Invoke(cause);};
        
        _fsm.InGameManager.Puzzle.Switch = false;

        if (_fsm.InGameManager.Player.IsOnTriggerPassiveBuff(TriggerPassiveBuff.PerfectPath))
        {
            _fsm.InGameManager.Puzzle.PerfectTrailEnd();
        }
    }

    private void Next(Character character)
    {
        _fsm.InGameManager.Player.OnDead -= Next;
        _fsm.InGameManager.Player.OnCharacterActionEnd?.Invoke();

        if (_fsm.InGameManager.Player.IsNotDetect)
        {
            _fsm.Failed();
            return;
        }

        if (_fsm.EnemiesCount <= 0)
        {
            _fsm.BattleEnd();
            return;
        }

        _fsm.TurnLoopNext();
    }
}
