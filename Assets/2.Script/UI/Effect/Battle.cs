using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : Poolable
{
    [SerializeField] Animator battleAnimator;

    private AniController _battleAniController;

    private readonly string _baseAnimation = "Basic";
    private readonly string _eliteAnimation = "Elite";
    private readonly string _bossAnimation = "Boss";

    private void Awake()
    {
        _battleAniController = battleAnimator.Initialize();
    }

    public void StartBattle(EnemyRoomType enemyRoomType)
    {
        switch(enemyRoomType)
        {
            case EnemyRoomType.Normal:
                _battleAniController.SetTrigger(_baseAnimation);
                break;
            case EnemyRoomType.Elite:
                _battleAniController.SetTrigger(_eliteAnimation);
                break;
            case EnemyRoomType.Boss:
                _battleAniController.SetTrigger(_bossAnimation);
                break;
        }
    }
}
