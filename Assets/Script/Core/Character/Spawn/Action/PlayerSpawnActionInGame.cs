using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerSpawnActionInGame : PlayerSpawnAction
{
    [SerializeField] List<CharacterSO> characterSOs = new List<CharacterSO>();

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
    public override int JobCount(Tribe job)
    {
        int count = 0;
        for (int i = 0; i < playerCharacters.Length; i++) 
        {
            if (playerCharacters[i] && playerCharacters[i].Job.Equals(job))
                count++;
        }

        return count;
    }

    const int MaxCharacterCount = 12;

    public void TestSet()
    {
        Set(characterSOs.ToArray());

        Managers.Observer.OnJoystic -= JoysticMove;
        Managers.Observer.OnJoystic += JoysticMove;
    }

    void Set(CharacterSO[] sos)
    {
        Clear();

        playerCharacters = new PlayerController[MaxCharacterCount];

        for (int i = 0; i < playerCharacters.Length; i++)
        {
            if (i < sos.Length)
                playerCharacters[i] = PlayerSpawn(sos[i].prefab, sos[i].codeName, i);
        }

        for (int i = 0; i < playerCharacters.Length; i++)
        {
            if (i < sos.Length)
                playerCharacters[i].JobSetUp(JobCount(playerCharacters[i].Job));
        }
    }

    public override void Play()
    {
        if (playerCharacters == null)
            TestSet();

        for (int i = 0; i < playerCharacters.Length; i++)
            if (playerCharacters[i])
                playerCharacters[i].Play(playerCharacters[i].Code);
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

    PlayerController PlayerSpawn(PlayerController pcPrefabs, string code ,int index)
    {
        if (index < 0 || playerCharacters.Length <= index)
        {
            UnityHelper.Error_H($"DeckController Push Error Too Big Index\nindex : {index}");
            return null;
        }

        playerSpawn.Clear(playerCharacters[index]);
        playerCharacters[index] = null;

        PlayerController masterPlayer = Player;

        Character playerCharacter = playerSpawn.Spawn(pcPrefabs, code, GameController.GetIndexWorldPos(masterPlayer, index), index);
        playerCharacters[index] = playerCharacter.GetComponent<PlayerController>();
        return playerCharacters[index];
    }
    public void Remove(int index)
    {
        if (index < 0 || playerCharacters.Length <= index)
        {
            UnityHelper.Error_H($"DeckController Push Error Too Big Index\nindex : {index}");
            return;
        }

        playerSpawn.Clear(playerCharacters[index]);
        playerCharacters[index] = null;
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

            playerCharacters[i].MoveDestination(GameController.GetIndexWorldPos(masterPlayer, i));
        }
    }
}
