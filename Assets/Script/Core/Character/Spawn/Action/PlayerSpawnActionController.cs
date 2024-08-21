using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnActionController : MonoBehaviour
{
    [SerializeReference, SubclassSelector] PlayerSpawnAction playerSpawnAction;

    public PlayerController Player => playerSpawnAction.Player;
    public void Play(Vector3 pos) => playerSpawnAction.Play(pos);
    public void Stop() => playerSpawnAction.Stop();
    public void Clear() => playerSpawnAction.Clear();
}
