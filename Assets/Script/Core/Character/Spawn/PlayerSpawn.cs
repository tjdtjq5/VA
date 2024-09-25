using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerSpawn : CharacterSpawn
{
    List<PlayerController> players = new List<PlayerController>();
    public override int Count => players.Count;

    public override Character Spawn(Character prefab, Vector3 pos)
    {
        Character c = Managers.Resources.Instantiate(prefab);
        PlayerController player = c.GetComponent<PlayerController>();
        player.transform.position = pos;
        player.Play();

        if (!players.Contains(player))
            players.Add(player);

        return player;
    }
    public override void Clear()
    {
        for (int i = 0; i < players.Count; i++)
            players[i].Clear();

        players.Clear();
    }
    public override void Clear(Character character)
    {
        if (!character)
            return;

        int findIndex = players.FindIndex(p => p == character);

        if (findIndex < 0)
            return;

        players[findIndex].Clear();
        players.RemoveAt(findIndex);
    }
}
