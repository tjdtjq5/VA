using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemySpawnActionController))]
[RequireComponent(typeof(PlayerSpawnActionController))]
public class InGameManager : SceneBase
{
    [SerializeField] UIMainItemViewList itemViewList;
    
    EnemySpawnActionController enemySpawnActionController;
    PlayerSpawnActionController playerSpawnActionController;

    protected override void Initialize()
    {
        base.Initialize();

        SceneType = SceneType.InGame;

        playerSpawnActionController = GetComponent<PlayerSpawnActionController>();
        playerSpawnActionController.Play();

        enemySpawnActionController = GetComponent<EnemySpawnActionController>();
        enemySpawnActionController.Play(playerSpawnActionController.Player);

        CameraController cameraController = FindObjectOfType<CinemachineVirtualCamera>().GetOrAddComponent<CameraController>();
        cameraController.SetTarget(playerSpawnActionController.Player.transform);
    }
    public override void Clear()
    {
        enemySpawnActionController.Clear();
        playerSpawnActionController.Clear();
    }
    void FixedUpdate()
    {
        enemySpawnActionController.FixedUpdate();
    }
    public override PlayerController GetPlayer()
    {
        return playerSpawnActionController.Player;
    }
    public override int GetPlayerJobCount(Tribe job)
    {
        return playerSpawnActionController.JobCount(job);
    }

    public void ItemViewOff() => itemViewList.UISetOff();
    public void ItemViewSet(ItemTableCodeDefine item) => itemViewList.UISet(item);
}
