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
    [SerializeField, Min(1)] int groupEnemyCount;
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

    float mapWidth;
    float mapHeight;
    float cellWidth;
    float cellHeight;
    float cellWidthHalf;
    float cellHeightHalf;
    bool isPlay = false;

    public override void Play(PlayerController player)
    {
        this.player = player;

        Clear();

        mapWidth = mapSizeMax.x - mapSizeMin.x;
        mapHeight = mapSizeMax.y - mapSizeMin.y;
        cellWidth = mapWidth / groupCell.x;
        cellHeight = mapHeight / groupCell.y;
        cellWidthHalf = cellWidth / 2;
        cellHeightHalf = cellHeight / 2;

        enemySpawn = new EnemySpawn[groupCell.x, groupCell.y];
        for (int i = 0; i < groupCell.x; i++)
            for (int j = 0; j < groupCell.y; j++)
                enemySpawn[i, j] = new();

        isPlay = true;

        GroupSpawn();
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
                GroupSpawn();
            }
        }
    }

    void EnemySpawn(int groupX, int groupY)
    {
        int random = UnityEngine.Random.Range(0, enemyPrefabs.Count);

        Vector3 groupPosition = new Vector3(mapSizeMin.x + cellWidth * groupX + cellWidthHalf, 0, mapSizeMin.y + cellHeight * groupY + cellHeightHalf);
        float posX = UnityEngine.Random.Range(0, 100) % 2 == 0 ? UnityEngine.Random.Range(groupSpawnPosRangeMin, groupSpawnPosRangeMax) : UnityEngine.Random.Range(-groupSpawnPosRangeMax, -groupSpawnPosRangeMin);
        float posZ = UnityEngine.Random.Range(0, 100) % 2 == 0 ? UnityEngine.Random.Range(groupSpawnPosRangeMin, groupSpawnPosRangeMax) : UnityEngine.Random.Range(-groupSpawnPosRangeMax, -groupSpawnPosRangeMin);
        Vector3 pos = new Vector3(posX, 0, posZ) + groupPosition;

        Character enemy = enemySpawn[groupX, groupY].Spawn(enemyPrefabs[random], pos, GetIndexByGrid(groupX, groupY));
        EnemyController ec = enemy.GetComponent<EnemyController>();
        ec.Allive();

        enemy.onTakeDamage -= OnTakeDamage;
        enemy.onTakeDamage += OnTakeDamage;

        enemy.onDead -= OnDead;
        enemy.onDead += OnDead;
    }
    void GroupSpawn(int minCount = 1)
    {
        for (int i = 0; i < groupCell.x; i++)
            for (int j = 0; j < groupCell.y; j++)
            {
                int groupEnemyC = enemySpawn[i, j].Count;
                if (groupEnemyC < minCount)
                {
                    while (groupEnemyC < groupEnemyCount)
                    {
                        EnemySpawn(i, j);
                        groupEnemyC++;
                    }
                }
            }
    }
    public override void Clear()
    {
        if (enemySpawn == null)
            return;

        for (int i = 0; i < groupCell.x; i++)
            for (int j = 0; j < groupCell.y; j++)
                enemySpawn[i, j].Clear();
    }

    public void OnTakeDamage(Entity entity, Entity instigator, object causer, BBNumber damage)
    {
        onTakeDamage?.Invoke(entity, instigator, causer, damage);
    }
    public void OnDead(Entity entity)
    {
        onDead?.Invoke(entity);
        
        EnemyController enemy = entity.GetComponent<EnemyController>();
        if (enemy == null) return;

        int index = enemy.Index;
        Vector2Int group = GetGroupByIndex(index);
        enemySpawn[group.x, group.y].Clear(enemy);
    }

    int GetIndexByGrid(int groupX, int groupY)
    {
        return groupY * groupCell.x + groupX;
    }
    Vector2Int GetGroupByIndex(int index)
    {
        int x = index % groupCell.x;
        int y = index / groupCell.y;

        return new Vector2Int(x, y);
    }
}
