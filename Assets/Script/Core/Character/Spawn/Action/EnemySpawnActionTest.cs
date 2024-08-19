using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySpawnActionTest : EnemySpawnAction
{
    [SerializeField] List<EnemyController> enemyPrefabs = new List<EnemyController>();
    [UnderlineTitle("Option")]
    [SerializeField] Vector2 mapSizeMin;
    [SerializeField] Vector2 mapSizeMax;
    [SerializeField] Vector2 mapOffset;

    EnemySpawn enemySpawn = new EnemySpawn();

    bool isPlay = false;

    public override int Count => throw new System.NotImplementedException();

    public override void Clear()
    {
        enemySpawn.Clear();
    }

    public override void Play()
    {
        Clear();

        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            Character enemy = enemySpawn.Spawn(enemyPrefabs[i]);

            Vector3 pos = new Vector3(UnityEngine.Random.Range(mapSizeMin.x, mapSizeMax.x), 0, UnityEngine.Random.Range(mapSizeMin.y, mapSizeMax.y));
            enemy.transform.position = pos;

            enemy.onTakeDamage -= OnTakeDamage;
            enemy.onTakeDamage += OnTakeDamage;

            enemy.onDead -= OnDead;
            enemy.onDead += OnDead;
        }
    }

    public override void Stop()
    {
        isPlay = false;
    }

    public override void Update()
    {
    }
    public void OnTakeDamage(Entity entity, Entity instigator, object causer, BBNumber damage)
    {
        onTakeDamage?.Invoke(entity, instigator, causer, damage);
    }
    public void OnDead(Entity entity)
    {
        onDead?.Invoke(entity);
    }
}
