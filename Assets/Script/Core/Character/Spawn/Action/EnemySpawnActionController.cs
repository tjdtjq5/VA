using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnActionController : MonoBehaviour
{
    [SerializeReference, SubclassSelector] EnemySpawnAction enemySpawnAction;

    public void Play() => enemySpawnAction.Play();
    public void Stop() => enemySpawnAction.Stop();
    void FixedUpdate() => enemySpawnAction.Update();
}
