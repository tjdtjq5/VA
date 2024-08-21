using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSpawnActionInGame : PlayerSpawnAction
{
    [SerializeField]
    private PlayerController playerPrefab;
    PlayerSpawn playerSpawn = new PlayerSpawn();

    private PlayerController player;
    public override PlayerController Player => player;

    public override void Play(Vector3 pos)
    {
        PlayerSpawn(pos);
        Player.Play();
    }

    public override void Stop()
    {
        Player.Stop();
    }
    public override void Clear()
    {
        Player.Clear();
        playerSpawn.Clear();
    }

    PlayerController PlayerSpawn(Vector3 pos)
    {
        Character playerCharacter = playerSpawn.Spawn(playerPrefab, pos);
        player = playerCharacter.GetComponent<PlayerController>();
        return player;
    }
}
