using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class InGameManager : SceneBase
{
    [SerializeField]
    EnemySpawnActionController enemySpawnActionController;
    [SerializeField]
    PlayerController player;

    protected override void Initialize()
    {
        base.Initialize();

        SceneType = SceneType.InGame;

        enemySpawnActionController.Play();

        CameraController cameraController = FindObjectOfType<CinemachineVirtualCamera>().GetOrAddComponent<CameraController>();
        cameraController.SetTarget(player.transform);
    }
    public override void Clear()
    {

    }
}
