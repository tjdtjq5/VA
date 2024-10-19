using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class SceneBase : MonoBehaviour
{
    SceneType _sceneType = SceneType.Unknown;
    public SceneType SceneType { get; protected set; }

    private void Start()
    {
        Initialize();
    }
    protected virtual void Initialize() 
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resources.Instantiate("Prefab/UI/EventSystem").name = "@EventSystem";
    }
    public abstract void Clear();
    public abstract PlayerController GetPlayer();
    public abstract int GetPlayerJobCount(Tribe job);
    public bool IsOutDest(PlayerController player, int index, Vector3 destination)
    {
        PlayerController masterPlayer = GetPlayer();
        if (!masterPlayer || masterPlayer == player)
            return false;

        if (masterPlayer.transform.position.GetSqrMagnitude(destination) > GameController.PlayersMaxSqrDistance
            || masterPlayer.transform.position.GetSqrMagnitude(destination) < GameController.PlayersMinSqrDistance
            && player.transform.position.GetSqrMagnitude(destination) > GameController.PlayersMaxSqrDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public virtual Vector3 GetPlayerMovePos(PlayerController player, int index, Vector3 destination)
    {
        if (IsOutDest(player, index, destination))
        {
            PlayerController masterPlayer = GetPlayer();
            return GameController.GetIndexWorldPos(masterPlayer, index);
        }
        else
        {
            return destination;
        }
    }
}
