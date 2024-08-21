using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawn : CharacterSpawn
{
    List<EnemyController> currentSpawnEnemies = new List<EnemyController>();
    public override int Count => currentSpawnEnemies.Where(e => !e.IsDead).Count();

    public override Character Spawn(Character enemyPrefab, Vector3 pos)
    {
        Character c = Managers.Resources.Instantiate(enemyPrefab);
        EnemyController enemy = c.GetComponent<EnemyController>();
        enemy.transform.position = pos;
        enemy.Play();

        if (!currentSpawnEnemies.Contains(enemy))
            currentSpawnEnemies.Add(enemy);

        return enemy;
    }

    public override void Clear() => currentSpawnEnemies.Clear();
}
