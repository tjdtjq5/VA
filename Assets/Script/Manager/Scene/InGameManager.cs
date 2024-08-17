using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : SceneBase
{
    [SerializeField]
    EnemySpawnActionController enemySpawnActionController;

    protected override void Initialize()
    {
        base.Initialize();

        SceneType = SceneType.InGame;

        enemySpawnActionController.Play();
    }
    public override void Clear()
    {

    }
}
