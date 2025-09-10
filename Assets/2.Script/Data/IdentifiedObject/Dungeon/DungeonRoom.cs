using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DungeonRoom : IdentifiedObject
{
    [SerializeField]
    private bool isEnemyRoom;
    [SerializeField, ShowIf("isEnemyRoom")]
    private List<Enemy> enemieSpawnDatas = new List<Enemy>();
    [SerializeField, ShowIf("isEnemyRoom")]
    private EnemyRoomType enemyRoomType;
    [SerializeField, HideIf("isEnemyRoom")]
    private GameObject eventObject;
    [SerializeField, HideIf("isEnemyRoom")]
    private List<UIPopup> eventPopups = new List<UIPopup>();
    [SerializeField] private Sprite directionMapIcon;

    public bool IsEnemyRoom => isEnemyRoom;
    public List<Enemy> Enemies => enemieSpawnDatas;
    public GameObject EventObject => eventObject;
    public List<UIPopup> EventPopups => eventPopups;
    public EnemyRoomType EnemyRoomType => enemyRoomType;
    public Sprite DirectionMapIcon => directionMapIcon;
}
public enum EnemyRoomType
{
    Normal,
    Elite,
    Boss,
}