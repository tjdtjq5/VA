using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSpawnActionInGame : PlayerSpawnAction
{
    [SerializeField] List<PlayerController> testPcPrefabs = new List<PlayerController>();

    PlayerSpawn playerSpawn = new PlayerSpawn();
    int masterPlayerIndex = -1;

    PlayerController[] playerCharacters;
    public override PlayerController Player
    {
        get
        {
            if (masterPlayerIndex >= 0 && masterPlayerIndex < playerCharacters.Length)
            {
                PlayerController mpc = playerCharacters[masterPlayerIndex];

                if (mpc != null)
                    return mpc;
            }

            for (int i = 0; i < playerCharacters.Length; i++)
            {
                if (playerCharacters[i])
                {
                    masterPlayerIndex = i;
                    return playerCharacters[i];
                }
            }

            return null;
        }
    }

    int VerticalCount => 4;
    float Spacing = 7f;
    const int MaxCharacterCount = 12;

    public void TestSet()
    {
        Set(testPcPrefabs.ToArray());

        Managers.Observer.OnJoystic -= JoysticMove;
        Managers.Observer.OnJoystic += JoysticMove;
    }

    void Set(PlayerController[] pcs)
    {
        Clear();

        playerCharacters = new PlayerController[MaxCharacterCount];

        for (int i = 0; i < playerCharacters.Length; i++)
        {
            if (i < pcs.Length)
                playerCharacters[i] = PlayerSpawn(pcs[i], i);
        }
    }

    public override void Play()
    {
        if (playerCharacters == null)
            TestSet();

        for (int i = 0; i < playerCharacters.Length; i++)
            if (playerCharacters[i])
                playerCharacters[i].Play();
    }
    public override void Stop()
    {
        for (int i = 0; i < playerCharacters.Length; i++)
            if (playerCharacters[i])
                playerCharacters[i].Stop();
    }
    public override void Clear()
    {
        if (playerCharacters == null)
            return;

        playerSpawn.Clear();

        for (int i = 0; i < playerCharacters.Length; i++)
            playerCharacters[i] = null;
    }

    PlayerController PlayerSpawn(PlayerController pcPrefabs, int index)
    {
        if (index < 0 || playerCharacters.Length <= index)
        {
            UnityHelper.LogError_H($"DeckController Push Error Too Big Index\nindex : {index}");
            return null;
        }

        playerSpawn.Clear(playerCharacters[index]);
        playerCharacters[index] = null;

        PlayerController masterPlayer = Player;
        bool isLeft = masterPlayer == null || masterPlayer.IsLeft;
        bool isDown = masterPlayer == null || masterPlayer.IsDown;

        Character playerCharacter = playerSpawn.Spawn(pcPrefabs, GetIndexWorldPos(index, isLeft, isDown));
        playerCharacters[index] = playerCharacter.GetComponent<PlayerController>();
        return playerCharacters[index];
    }
    public void Remove(int index)
    {
        if (index < 0 || playerCharacters.Length <= index)
        {
            UnityHelper.LogError_H($"DeckController Push Error Too Big Index\nindex : {index}");
            return;
        }

        playerSpawn.Clear(playerCharacters[index]);
        playerCharacters[index] = null;
    }

   
    Vector3 GetIndexWorldPos(int index, bool isLeft, bool isDown)
    {
        PlayerController masterPlayer = Player;
        Vector3 masterPos = masterPlayer == null ? Vector3.zero : masterPlayer.transform.position;

        return masterPos + GetIndexLocalPos(index, isLeft, isDown);
    }
    Vector3 GetIndexLocalPos(int index, bool isLeft, bool isDown)
    {
        Vector3 stPos = Vector3.zero;

        int x = index / VerticalCount;
        int z = index % VerticalCount;

        x = isLeft ? x : -x;
        z = isDown ? z : -z;

        return stPos + new Vector3(x * Spacing, 0, z * Spacing);
    }

    void JoysticMove(Vector3 direction)
    {
        PlayerController masterPlayer = Player;

        if (!masterPlayer)
            return;

        masterPlayer.MoveDirection(direction);

        OtherMove(masterPlayer);
    }
    void OtherMove(PlayerController masterPlayer)
    {
        bool isLeft = masterPlayer.IsLeft;
        bool isDown = masterPlayer.IsDown;

        for (int i = 0; i < playerCharacters.Length; i++) 
        {
            if (!playerCharacters[i])
                continue;

            if (playerCharacters[i] == masterPlayer )
                continue;

            playerCharacters[i].MoveDestination(GetIndexWorldPos(i, isLeft, isDown));
        }
    }
}
