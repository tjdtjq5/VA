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

    public override void Play()
    {
        PlayerSpawn();
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

    PlayerController PlayerSpawn()
    {
        Character playerCharacter = playerSpawn.Spawn(playerPrefab);
        player = playerCharacter.GetComponent<PlayerController>();
        return player;
    }
}
