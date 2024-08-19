using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : CharacterSpawn
{
    List<PlayerController> players = new List<PlayerController>();
    public override int Count => players.Count;

    public override Character Spawn(Character prefab)
    {
        Character c = Managers.Resources.Instantiate(prefab);
        PlayerController player = c.GetComponent<PlayerController>();

        if (!players.Contains(player))
            players.Add(player);

        return player;
    }
    public override void Clear() => players.Clear();
}
