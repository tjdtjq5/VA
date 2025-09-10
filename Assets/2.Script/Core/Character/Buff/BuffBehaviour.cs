using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffBehaviour
{
    protected Buff Buff;

    public void Initialize(Buff buff) =>  this.Buff = buff;
    public abstract void OnStart(Character useCharacter, Character takeCharacter, object cause);
    public abstract void OnEnd(Character useCharacter, Character takeCharacter, object cause);
    protected void TargetTakeDamage(Character owner, Character target, float damageValue, float criPercent, DamageType damageType, SkillApplyDamageType skillApplyDamageType)
    {
        owner.ApplyAttack(target, this, damageValue, criPercent, damageType, skillApplyDamageType);
    }
    protected List<Character> FindTarget(Character owner, SkillTargetType targetTypeType)
    {
        List<Character> targets = new List<Character>();
        
        switch (targetTypeType)
        {
            case SkillTargetType.My:
                targets.Add(owner);
                break;
            case SkillTargetType.Player:
                targets.Add(Managers.Observer.Player);
                break;
            case SkillTargetType.CloseEnemy:
                if (Managers.Observer.PuzzleBattleStateMachine.Enemies.Count > 0)
                    targets.Add(Managers.Observer.PuzzleBattleStateMachine.Enemies[0]);
                break;
            case SkillTargetType.FarEnemy:
                int enemyCount = Managers.Observer.PuzzleBattleStateMachine.Enemies.Count;
                if (enemyCount > 0)
                    targets.Add(Managers.Observer.PuzzleBattleStateMachine.Enemies[enemyCount - 1]);
                break;
            case SkillTargetType.AllEnemies:
                targets.AddRange(Managers.Observer.PuzzleBattleStateMachine.Enemies);
                break;
            default:
                break;
        }

        return targets;
    }
    public virtual Dictionary<string, string> StringsByKeyword(string preface) => new ();
}
