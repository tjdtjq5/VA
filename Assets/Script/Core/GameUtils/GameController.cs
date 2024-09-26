using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameController
{
    public static int DeckVerticalCount => 4;
    public static float DeckSpacing = 7f;
    public static Vector3 GetIndexWorldPos(PlayerController masterPlayer, int index)
    {
        bool isLeft = masterPlayer == null || masterPlayer.IsLeft;
        bool isDown = masterPlayer == null || masterPlayer.IsDown;

        Vector3 masterPos = masterPlayer == null ? Vector3.zero : masterPlayer.transform.position;
        return masterPos + GetIndexLocalPos(masterPlayer, index);
    }
    public static Vector3 GetIndexLocalPos(PlayerController masterPlayer, int index)
    {
        Vector3 stPos = Vector3.zero;

        bool isLeft = masterPlayer == null || masterPlayer.IsLeft;
        bool isDown = masterPlayer == null || masterPlayer.IsDown;

        int x = index / DeckVerticalCount;
        int z = index % DeckVerticalCount;

        x = isLeft ? x : -x;
        z = isDown ? z : -z;

        return stPos + new Vector3(x * DeckSpacing, 0, z * DeckSpacing);
    }

    public static float PlayersMaxSqrDistance => 225f; // -> 15f
    public static float PlayersMinSqrDistance => 16f; // -> 4f
}
