using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySpawnActionInGame : EnemySpawnAction
{
    [SerializeField] List<EnemyController> enemyPrefabs = new List<EnemyController>();

    [UnderlineTitle("Option")]
    [SerializeField, Range(1f, 25f)] float spawnTime; float spawnTimer = 0;
    [SerializeField] Vector2 mapSizeMin;
    [SerializeField] Vector2 mapSizeMax;
    [SerializeField] Vector2 mapOffset;
    [Header("Group")]
    [SerializeField] Vector2Int groupCell;
    [SerializeField] int groupEnemyCount;
    [SerializeField, Range(0f, 1f)] float groupSpawnPosRangeMin;
    [SerializeField, Range(0.1f, 10f)] float groupSpawnPosRangeMax;

    EnemySpawn[,] enemySpawn;
    public override int Count
    {
        get
        {
            int count = 0;
            for (int i = 0; i < groupCell.x; i++)
                for (int j = 0; j < groupCell.y; j++)
                    count += enemySpawn[i, j].Count;

            return count;
        }
    }

    bool isPlay = false;


    public override void Play()
    {
        Clear();

        enemySpawn = new EnemySpawn[groupCell.x, groupCell.y];
        isPlay = true;
    }

    public override void Stop()
    {
        isPlay = false;
    }

    public override void Update()
    {
        if (isPlay)
        {
            spawnTimer += Managers.Time.FixedDeltaTime;
            if (spawnTimer > spawnTime)
            {
                spawnTimer = 0;
                AllSpawn();
            }
        }
    }

    void EnemySpawn(int groupX, int groupY)
    {
        // int random = UnityEngine.Random.Range(0, enemyPrefabs.Count);
        // Character enemy = enemySpawn.Spawn(enemyPrefabs[random]);

        // float randomPosXInMap = UnityEngine.Random.Range(mapSizeMin.x, mapSizeMax.x) + mapOffset.x;
        // float randomPosZInMap = UnityEngine.Random.Range(mapSizeMin.y, mapSizeMax.y) + mapOffset.y;
        // Vector3 randomPosInMap = new Vector3(randomPosXInMap, 0, randomPosZInMap);
        // enemy.transform.position = randomPosInMap;
    }
    void GroupSpawn(int groupX, int groupY)
    {

    }
    void AllSpawn()
    {
        // 그룹 안에 한마리라도 살아있으면 패스
    }
    public override void Clear()
    {
        for (int i = 0; i < groupCell.x; i++)
            for (int j = 0; j < groupCell.y; j++)
                enemySpawn[i, j].Clear();
    }
}
