using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnActionController : MonoBehaviour
{
    [SerializeReference, SubclassSelector] PlayerSpawnAction playerSpawnAction;

    public PlayerController Player => playerSpawnAction.Player;
    public int JobCount(Tribe job) => playerSpawnAction.JobCount(job);
    public void Play() => playerSpawnAction.Play();
    public void Stop() => playerSpawnAction.Stop();
    public void Clear() => playerSpawnAction.Clear();
}
