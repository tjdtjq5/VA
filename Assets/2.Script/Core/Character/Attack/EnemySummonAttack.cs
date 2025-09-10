using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySummonAttack : EnemyAttack
{
    // 공격할 때 체력 체크 후 체력이 일정 percent 이하면 Move & Camera 이동 -> Summon -> 공격
    [SerializeField] private Enemy _summonEnemy;
    [SerializeField] private int _summonCharacterCount = 2;
    [SerializeField] private float _summonMoveTime = 0.6f;

    private bool IsCheckSummon => Character.HpPercent <= 0.5f && !_isAlreadySummon;
    private bool _isAlreadySummon = false;
    private List<Enemy> _summonEnemies = new List<Enemy>();
    private List<float> _enemyBoxWidths = new List<float>();

    private List<Character> _targets = new List<Character>();
    private object _cause;
    private bool _isSequence = false;

    private readonly string _summonAnimationName = "Summon";
    private readonly Vector3 _summonStartPosition = new Vector3(-10000, 0, 0);

    public override void Initialize(Character character, Transform transform)
    {
        base.Initialize(character, transform);
        
        for (int i = 0; i < Character.SpineSpineAniControllers.Count; i++)
        {
            Character.SpineSpineAniControllers[i].SetEventFunc(_summonAnimationName, _moveEvent , SummonMove);
            Character.SpineSpineAniControllers[i].SetEndFunc(_summonAnimationName, SummonEnd);
        }
    }

    public override void SetAttack(List<Character> targets, object cause, bool isSequence)
    {
        this._targets = targets;
        this._cause = cause;
        this._isSequence = isSequence;

        if (IsCheckSummon)
        {
            SetSummon();
        }
        else
        {
            base.SetAttack(targets, cause, isSequence);
        }
    }

    private void SetSummon()
    {
        _isAlreadySummon = true;

        Character.SetAnimation(_summonAnimationName, false);
    }

    private void SummonMove()
    {
        for (int i = 0; i < _summonCharacterCount; i++)
        {
            Enemy enemy = Managers.Resources.Instantiate<Enemy>(_summonEnemy);
            enemy.transform.position = _summonStartPosition;
            _enemyBoxWidths.Add(enemy.BoxWeidth);
            _summonEnemies.Add(enemy);
        }
        _enemyBoxWidths.Add(Character.BoxWeidth);

        Vector3 movePosition = Managers.Observer.PuzzleBattleStateMachine.GetEnemySpawnPosition(_summonCharacterCount, _enemyBoxWidths);
        this.Character.CharacterMove.SetTimeMove(movePosition, _summonMoveTime);

        Vector3 cameraFovPositionA = Managers.Observer.Player.transform.position;
        cameraFovPositionA.x -= Managers.Observer.Player.BoxWeidth * 0.5f;
        Vector3 cameraFovPositionB = movePosition;
        cameraFovPositionB.x += Character.BoxWeidth * 0.5f;

        Managers.Observer.CameraController.SetFieldOfViewByPosition(cameraFovPositionA, cameraFovPositionB, true);
    }

    private void SummonEnd()
    {
        // 몬스터 소환 -> 일정 시간 뒤 공격
        for (int i = _summonCharacterCount - 1; i >= 0; i--)
        {
            Enemy enemy = _summonEnemies[i];
            enemy.transform.position = Managers.Observer.PuzzleBattleStateMachine.GetEnemySpawnPosition(i, _enemyBoxWidths);
            Managers.Observer.PuzzleBattleStateMachine.SettingEnemy(enemy, true);
            enemy.SetAnimation(_summonAnimationName, false);
        }

        Managers.Tween.TweenInvoke(0.5f).SetOnComplete(() =>
        {
            base.SetAttack(_targets, _cause, _isSequence);
        });
    }
}
