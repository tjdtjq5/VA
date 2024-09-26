using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemySpawn : CharacterSpawn
{
    List<EnemyController> currentSpawnEnemies = new List<EnemyController>();
    public override int Count => currentSpawnEnemies.Where(e => !e.IsDead).Count();

    public override Character Spawn(Character enemyPrefab, Vector3 pos, int index)
    {
        Character c = Managers.Resources.Instantiate(enemyPrefab);
        EnemyController enemy = c.GetComponent<EnemyController>();
        enemy.transform.position = pos;
        enemy.Play();
        enemy.Index = index;

        if (!currentSpawnEnemies.Contains(enemy))
            currentSpawnEnemies.Add(enemy);

        return enemy;
    }

    public override void Clear()
    {
        for (int i = 0; i < currentSpawnEnemies.Count; i++)
            currentSpawnEnemies[i].Clear();

        currentSpawnEnemies.Clear();
    }

    public override void Clear(Character character)
    {
        if (!character)
            return;

        int findIndex = currentSpawnEnemies.FindIndex(p => p == character);

        if (findIndex < 0)
            return;

        currentSpawnEnemies[findIndex].Clear();
        currentSpawnEnemies.RemoveAt(findIndex);
    }
}
