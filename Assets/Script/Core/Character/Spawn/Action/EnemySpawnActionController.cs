using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnActionController : MonoBehaviour
{
    [SerializeReference, SubclassSelector] EnemySpawnAction enemySpawnAction;

    public void Play(PlayerController player) => enemySpawnAction.Play(player);
    public void Stop() => enemySpawnAction.Stop();
    public void Clear() => enemySpawnAction.Clear();
    public void FixedUpdate() => enemySpawnAction.Update();
}
